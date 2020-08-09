using Blazored.SessionStorage;
using Dapper;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using terminus.dataaccess;
using terminus.shared.models;
using terminus_webapp.Common;

namespace terminus_webapp.Pages
{
    public class BillingListBase:ComponentBase
    {
        [Inject]
        public IWebHostEnvironment _env { get; set; }

        [Inject]
        public EmailService emailService { get; set; }

        [Inject]
        public AppDBContext appDBContext { get; set; }

        [Inject]
        public UserManager<AppUser> userManager { get; set; }

        [Inject]
        public DapperManager dapperManager { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public ISessionStorageService _sessionStorageService { get; set; }

        [Inject]
        private IJSRuntime JSRuntime { get; set; }

        public List<BillingViewModel> Bills { get; set; }

        public bool DataLoaded { get; set; }

        public bool IsGenerating { get; set; }


        public string ErrorMessage { get; set; }

        public string CompanyId { get; set; }

        public string UserName { get; set; }

        public void CreateBill()
        {
            NavigationManager.NavigateTo("billingentry");
        }

        private async Task<string> GetDocId()
        {
            DynamicParameters dynamicParameters = new DynamicParameters();
            var IdKey = $"BILLING{DateTime.Today.ToString("yyyyMM")}";
            dynamicParameters.Add("IdKey", IdKey);
            dynamicParameters.Add("Format", "000000");
            dynamicParameters.Add("CompanyId", CompanyId);

            var documentIdTable = await dapperManager.GetAllAsync<DocumentIdTable>("spGetNextId", dynamicParameters);
            var documentId = string.Empty;

            if (documentIdTable.Any())
            {
                documentId = $"BILLING{DateTime.Today.ToString("yyyyMM")}{documentIdTable.First().NextId.ToString(documentIdTable.First().Format)}";
            }

            return documentId;
        }

        private async Task<Billing> GetBilling(Guid pdId, string monthYear)
        {
            DynamicParameters dynamicParameters = new DynamicParameters();
            
            dynamicParameters.Add("pdId", pdId);
            dynamicParameters.Add("monthYear", monthYear);

            var bills = await dapperManager.GetAllAsync<Billing>("spGetBillingByMonthYearPd", dynamicParameters);

            return bills.FirstOrDefault();

        }

        protected decimal CalculateBeforeVat(decimal amount)
        {
            if (amount == 0m)
                return 0m;

            return Math.Round(amount / 1.12m, 2);
        }

        protected decimal CalculateVat(decimal amount)
        {
            if (amount == 0m)
                return 0m;

            return amount - Math.Round(amount / 1.12m, 2);
        }

        protected decimal CalculateWT(decimal amount)
        {
            if (amount == 0m)
                return 0m;

            return Math.Round((amount * .05m), 2);
        }

        private async Task GenerateBill(PropertyDirectory pd, DateTime dueDate)
        {
            var currentBill = await GetBilling(pd.id, dueDate.ToString("yyyyMM"));

            if(currentBill==null)
            {
                currentBill = new Billing();
                currentBill.billId = Guid.NewGuid();
                currentBill.transactionDate = DateTime.Today;
                currentBill.dateDue = dueDate;
                currentBill.MonthYear = dueDate.ToString("yyyyMM");
                currentBill.propertyDirectoryId = pd.id;
                currentBill.createDate = DateTime.Now;
                currentBill.createdBy = UserName;
                currentBill.companyId = CompanyId;
                currentBill.documentId = await GetDocId();
                currentBill.billType = "MB";

                var parameters = new DynamicParameters();

                parameters.Add("propertyDirectoryid", pd.id);
                parameters.Add("MonthYear", currentBill.MonthYear);

                decimal monthlyRentBalance = 0m;
                decimal monthlyAssocDueBalance = 0m;

                var balanceView = await dapperManager.GetAllAsync<PropertyBalanceViewModel>("spGetMonthlyBalance", parameters);

                monthlyRentBalance = balanceView.Where(a => a.billLineType.Equals(Constants.BillLineTypes.MONTHLYBILLITEM, StringComparison.OrdinalIgnoreCase)
                                                            || a.billLineType.Equals(Constants.BillLineTypes.MONTHLYBILLITEM_PREVBAL, StringComparison.OrdinalIgnoreCase)
                                                            || a.billLineType.Equals(Constants.BillLineTypes.MONTHLYBILLITEMPENALTY, StringComparison.OrdinalIgnoreCase)
                                                            || a.billLineType.Equals(Constants.BillLineTypes.MONTHLYBILLITEM_VAT, StringComparison.OrdinalIgnoreCase)
                                                            || a.billLineType.Equals(Constants.BillLineTypes.MONTHLYBILLITEM_WT, StringComparison.OrdinalIgnoreCase)

                                                            )
                                                .Sum(a => a.balance);

                monthlyAssocDueBalance = balanceView.Where(a => a.billLineType.Equals(Constants.BillLineTypes.MONTHLYASSOCDUE, StringComparison.OrdinalIgnoreCase)
                                                            || a.billLineType.Equals(Constants.BillLineTypes.MONTHLYASSOCDUEPENALTY, StringComparison.OrdinalIgnoreCase)
                                                            || a.billLineType.Equals(Constants.BillLineTypes.MONTHLYASSOCDUE_PREVBAL, StringComparison.OrdinalIgnoreCase)
                                                            || a.billLineType.Equals(Constants.BillLineTypes.MONTHLYASSOCDUE_VAT, StringComparison.OrdinalIgnoreCase)
                                                            )
                                                .Sum(a => a.balance);

                List<BillingLineItem> billItems = new List<BillingLineItem>();

                var dueAmount = 0m;

                dueAmount = pd.monthlyRate;
                var dueAmountVat = CalculateVat(dueAmount);
                var dueAmountBeforeVat = dueAmount - dueAmountVat;
                var wtAmt = 0m;

                if (dueAmountBeforeVat != 0m && pd.withWT)
                {
                    wtAmt = CalculateWT(dueAmountBeforeVat);
                    dueAmountBeforeVat = dueAmount - (dueAmountVat + wtAmt);
                }

                if (monthlyRentBalance > 0)
                {
                    var penaltyPct = pd.penaltyPct;
                    var penalty = 0m;
                    if (penaltyPct > 0)
                        penalty = monthlyRentBalance * (penaltyPct / 100m);

                    if (penalty > 0)
                    {
                        billItems.Add(new BillingLineItem()
                        {
                            Id = Guid.NewGuid(),
                            description = $"Monthly rent penalty {penaltyPct.ToString("0.00")}%",
                            amount = penalty,
                            lineNo = 0,
                            generated = true,
                            billLineType = Constants.BillLineTypes.MONTHLYBILLITEMPENALTY
                        });
                    }

                    billItems.Add(new BillingLineItem()
                    {
                        Id = Guid.NewGuid(),
                        description = "Previous balance",
                        amount = monthlyRentBalance,
                        lineNo = 1,
                        generated = true,
                        billLineType = Constants.BillLineTypes.MONTHLYBILLITEM_PREVBAL
                    });
                }

                if (dueAmountBeforeVat != 0m)
                {
                    billItems.Add(new BillingLineItem()
                    {
                        Id = Guid.NewGuid(),
                        description = "Monthly due",
                        amount = dueAmountBeforeVat,
                        amountPaid = 0,
                        lineNo = 2,
                        generated = true,
                        billLineType = Constants.BillLineTypes.MONTHLYBILLITEM
                    });
                }

                if (dueAmountBeforeVat != 0m)
                {
                    billItems.Add(new BillingLineItem()
                    {
                        Id = Guid.NewGuid(),
                        description = "Monthly due (VAT)",
                        amount = dueAmountVat,
                        amountPaid = 0,
                        lineNo = 2,
                        generated = true,
                        billLineType = Constants.BillLineTypes.MONTHLYBILLITEM_VAT
                    });
                }


                if (wtAmt != 0m)
                {
                    billItems.Add(new BillingLineItem()
                    {
                        Id = Guid.NewGuid(),
                        description = "Monthly due (WT)",
                        amount = wtAmt,
                        amountPaid = 0,
                        lineNo = 2,
                        generated = true,
                        billLineType = Constants.BillLineTypes.MONTHLYBILLITEM_WT
                    });
                }

                if (monthlyAssocDueBalance > 0)
                {
                    billItems.Add(new BillingLineItem()
                    {
                        Id = Guid.NewGuid(),
                        description = "Association dues penalty",
                        amount = monthlyAssocDueBalance * (pd.penaltyPct / 100m),
                        lineNo = 3,
                        generated = true,
                        billLineType = Constants.BillLineTypes.MONTHLYASSOCDUEPENALTY
                    });

                    billItems.Add(new BillingLineItem()
                    {
                        Id = Guid.NewGuid(),
                        description = "Previous association dues balance",
                        amount = monthlyAssocDueBalance,
                        lineNo = 4,
                        generated = true,
                        billLineType = Constants.BillLineTypes.MONTHLYASSOCDUE_PREVBAL
                    });
                }

                if (pd.associationDues > 0)
                {
                    var assocDuesBeforevat = CalculateBeforeVat(pd.associationDues);
                    var assocDuesVat = CalculateVat(pd.associationDues);

                    billItems.Add(new BillingLineItem()
                    {
                        Id = Guid.NewGuid(),
                        description = "Association dues",
                        amount = assocDuesBeforevat,
                        amountPaid = 0,
                        lineNo = 6,
                        generated = true,
                        billLineType = Constants.BillLineTypes.MONTHLYASSOCDUE
                    });

                    billItems.Add(new BillingLineItem()
                    {
                        Id = Guid.NewGuid(),
                        description = "Association dues (VAT)",
                        amount = assocDuesVat,
                        amountPaid = 0,
                        lineNo = 7,
                        generated = true,
                        billLineType = Constants.BillLineTypes.MONTHLYASSOCDUE_VAT
                    });
                }

                currentBill.totalAmount = billItems.Sum(a => a.amount);
                currentBill.balance = billItems.Sum(a => a.amount - a.amountPaid);
                currentBill.amountPaid = billItems.Sum(a => a.amountPaid);
                currentBill.billingLineItems = billItems;
                var result = await SaveBill(currentBill);
            }
        }

        protected async Task<bool> SaveBill(Billing billing)
        {
            using (var dbConnection = dapperManager.GetConnection())
            {
                dbConnection.Open();

                using (var transaction = dbConnection.BeginTransaction())
                {
                    try
                    {
                        var param = new Dapper.DynamicParameters();
                        param.Add("billId", billing.billId, System.Data.DbType.Guid);
                        param.Add("createdBy", UserName, System.Data.DbType.String);
                        param.Add("dateDue", billing.dateDue, System.Data.DbType.DateTime);
                        param.Add("totalAmount", billing.totalAmount, System.Data.DbType.Decimal);
                        param.Add("status", billing.status, System.Data.DbType.String);
                        param.Add("propertyDirectoryId", billing.propertyDirectoryId, System.Data.DbType.Guid);
                        param.Add("companyId", CompanyId, System.Data.DbType.String);
                        param.Add("amountPaid", billing.amountPaid, System.Data.DbType.Decimal);
                        param.Add("balance", billing.balance, System.Data.DbType.Decimal);
                        param.Add("documentId", billing.documentId, System.Data.DbType.String);
                        param.Add("transactionDate", billing.transactionDate, System.Data.DbType.DateTime);
                        param.Add("MonthYear", billing.MonthYear, System.Data.DbType.String);
                        param.Add("billType", billing.billType, System.Data.DbType.String);

                        var result = await dapperManager.ExecuteAsync("spInsertBillings", transaction, dbConnection, param);

                        foreach (BillingLineItem item in billing.billingLineItems)
                        {
                            var paramLine = new Dapper.DynamicParameters();
                            paramLine.Add("Id", item.Id, System.Data.DbType.Guid);
                            paramLine.Add("description", item.description, System.Data.DbType.String);
                            paramLine.Add("amount", item.amount, System.Data.DbType.Decimal);
                            paramLine.Add("lineNo", item.lineNo, System.Data.DbType.Int32);
                            paramLine.Add("billingId", billing.billId, System.Data.DbType.Guid);
                            paramLine.Add("generated", item.generated, System.Data.DbType.Boolean);
                            paramLine.Add("amountPaid", item.amountPaid, System.Data.DbType.Decimal);
                            paramLine.Add("billLineType", item.billLineType, System.Data.DbType.String);
                            await dapperManager.ExecuteAsync("spInsertBillingLineItem", transaction, dbConnection, paramLine);
                        }

                        param = null;
                        param = new Dapper.DynamicParameters();
                        param.Add("billId", billing.billId, System.Data.DbType.Guid);
                        result = await dapperManager.ExecuteAsync("spClosePreviousBill", transaction, dbConnection, param);

                        transaction.Commit();
                        return true;
                            
                    }
                    catch (Exception ex)
                    {
                        ErrorMessage = ex.ToString();
                        transaction.Rollback();
                        return false;
                    }
                    finally
                    {
                        if (dbConnection.State == ConnectionState.Open)
                            dbConnection.Close();
                    }
                }
            }

        }
               
        

        protected async Task GenerateBills()
        {
            try
            {
                ErrorMessage = string.Empty;
                IsGenerating = true;
                var pdlist = await appDBContext.PropertyDirectory
                                               .Where(a => a.companyId.Equals(CompanyId)
                                               && DateTime.Today >= a.dateFrom && DateTime.Today <= a.dateTo
                                               && !a.deleted).ToListAsync();


                var dueDate = DateTime.Parse(DateTime.Today.ToString("yyyy-MM-07"));

                foreach(var pd in pdlist)
                {
                    await GenerateBill(pd, dueDate);
                }
                await JSRuntime.InvokeVoidAsync("alert", $"Generating bills completed.");
                await GetBills();
                StateHasChanged();
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.ToString();
            }
            finally
            {
                IsGenerating = false;

            }   
        }

        public async Task SendBill(string BillId)
        {

            var bill = await appDBContext.Billings
                                          .Include(a => a.propertyDirectory)
                                          .ThenInclude(b=>b.property)
                                            .Include(a => a.propertyDirectory)
                                          .ThenInclude(b => b.tenant)
                                         .Include(a=>a.billingLineItems)
                                         .Where(a => a.billId.Equals(Guid.Parse(BillId))).FirstOrDefaultAsync();

            var user = await userManager.FindByNameAsync(UserName);

            var preparedBy = $"{user.firstName} {user.lastName}";

            var path = Path.Combine(_env.WebRootPath, "Templates");

            var resultPath = Path.Combine(_env.WebRootPath, "Uploaded","SOA");
            var templateFile = string.Empty;

            var template = string.Empty;
            var fullName = string.Empty;
            var rentalDate = string.Empty;
            var unit = string.Empty;
            var billItems = new StringBuilder();
            var fileName = string.Empty;
            var fileAttachment = string.Empty;
            var email = string.Empty;
            var ownerEmail = string.Empty;

            switch (CompanyId.ToUpper())
            {
                case "ASRC":
                    templateFile = Path.Combine(path, "template_asrc.html");
                    template = File.ReadAllText(templateFile);
                    fullName = $"{bill.propertyDirectory.tenant.lastName} {bill.propertyDirectory.tenant.firstName} {bill.propertyDirectory.tenant.middleName}";
                    rentalDate = bill.dateDue.ToString("yyyy-MM-01");
                    template = template.Replace("[@Tenant]", fullName);
                    template = template.Replace("[@BillDocumentId]", bill.documentId);
                    unit = bill.propertyDirectory.property.description;

                    template = template.Replace("[@Unit]", unit);
                    template = template.Replace("[@TransactionDate]", bill.transactionDate.ToString("yyyy-MM-dd"));
                    template = template.Replace("[@RentalPeriod]", rentalDate);
                    template = template.Replace("[@DueDate]", bill.dateDue.ToString("yyyy-MM-dd"));
                    template = template.Replace("[@TotalAmount]", bill.totalAmount.ToString("#,##0.00;(#,##0.00)"));
                    template = template.Replace("[@PreparedBy]", preparedBy);
                    template = template.Replace("[@PenaltyPct]", $"{bill.propertyDirectory.penaltyPct.ToString("0.00")}%");
                   
                    foreach(var item in bill.billingLineItems)
                    {
                        var vat = 0m;
                        var penalty = 0m;
                        var wt = 0m;
                        var total = 0m;
                        var lineItem = string.Empty;

                        switch (item.billLineType)
                        {
                            case Constants.BillLineTypes.MONTHLYBILLITEM:
                               
                                vat = bill.billingLineItems.Where(a => a.billLineType.Equals(Constants.BillLineTypes.MONTHLYBILLITEM_VAT)).Sum(a => a.amount);
                                penalty = bill.billingLineItems.Where(a => a.billLineType.Equals(Constants.BillLineTypes.MONTHLYBILLITEMPENALTY)).Sum(a => a.amount);
                                wt = bill.billingLineItems.Where(a => a.billLineType.Equals(Constants.BillLineTypes.MONTHLYBILLITEM_WT)).Sum(a => a.amount);
                                total = item.amount + vat + penalty + wt;

                                lineItem = $"<tr><td colspan = \"3\">Rental Due payable to ASRC</td><td>{unit}</td><td>{bill.propertyDirectory.property.areaInSqm.ToString()}</td><td>{item.amount.ToString("#,##0.00;(#,##0.00)")}</td><td>{vat.ToString("#,##0.00;(#,##0.00)")}</td><td>{penalty.ToString("#,##0.00;(#,##0.00)")}</td><td>{wt.ToString("#,##0.00;(#,##0.00)")}</td><td> {total.ToString("#,##0.00;(#,##0.00)")} </td></tr>";
                                billItems.AppendLine(lineItem);
                                break;
                            case Constants.BillLineTypes.MONTHLYASSOCDUE:
                               
                                 vat = bill.billingLineItems.Where(a => a.billLineType.Equals(Constants.BillLineTypes.MONTHLYASSOCDUE_VAT)).Sum(a => a.amount);
                                 penalty = bill.billingLineItems.Where(a => a.billLineType.Equals(Constants.BillLineTypes.MONTHLYASSOCDUEPENALTY)).Sum(a => a.amount);
                               
                                 total = item.amount + vat + penalty + wt;

                                lineItem = $"<tr><td colspan = \"3\">Assoc Due Payable to ADBCA ({bill.dateDue.ToString("MMM yyyy")})</td><td>{unit}</td><td>{bill.propertyDirectory.property.areaInSqm.ToString()}</td><td>{item.amount.ToString("#,##0.00;(#,##0.00)")}</td><td>{vat.ToString("#,##0.00;(#,##0.00)")}</td><td>{penalty.ToString("#,##0.00;(#,##0.00)")}</td><td>&nbsp;</td><td> {total.ToString("#,##0.00;(#,##0.00)")} </td ></tr>";
                                billItems.AppendLine(lineItem);
                                break;

                            case Constants.BillLineTypes.MONTHLYBILLITEMPENALTY:
                            case Constants.BillLineTypes.MONTHLYBILLITEM_VAT:
                            case Constants.BillLineTypes.MONTHLYBILLITEM_WT:
                            case Constants.BillLineTypes.MONTHLYASSOCDUEPENALTY:
                            case Constants.BillLineTypes.MONTHLYASSOCDUE_VAT:
                                break;

                            default:
                                lineItem = $"<tr><td colspan = \"3\">{item.description}</td><td>{unit}</td><td>{bill.propertyDirectory.property.areaInSqm.ToString()}</td><td></td><td></td><td></td><td>&nbsp;</td><td> {item.amount.ToString("#,##0.00;(#,##0.00)")} </td ></tr>";
                                billItems.AppendLine(lineItem);
                                break;

                        }
                        
                       
                    }
                    template = template.Replace("[@BillItems]", billItems.ToString());
                    template = template.Replace("[@PreparedBy]", preparedBy);
                    fileName = $"{BillId}_{DateTime.Now.ToString("yyyyMMddHHmmss")}.html";
                    fileAttachment = Path.Combine(resultPath, fileName);
                    File.WriteAllText(fileAttachment, template);
                    email = bill.propertyDirectory.tenant.emailAddress;

                    ownerEmail = bill.propertyDirectory.property.ownerEmailAdd;

                    if ((!string.IsNullOrEmpty(email)|| !string.IsNullOrEmpty(ownerEmail)))
                    {
                        var subject = $"Statement of account - {bill.dateDue.ToString("MMM yyyy")}";

                        var body = new StringBuilder();
                        body.AppendLine($"Dear {bill.propertyDirectory.tenant.firstName} {bill.propertyDirectory.tenant.lastName},");
                        body.AppendLine("<br/><br/>");
                        body.AppendLine($"Please see attached billing for the month of {bill.dateDue.ToString("MMM yyyy")}.");
                        body.AppendLine("<br/><br/><br/>");
                        body.AppendLine("Thanks");
                        var send = await emailService.SendEmail($"{email};{ownerEmail}", "", subject, body.ToString(), fileAttachment);

                        if(send)
                        {
                            await JSRuntime.InvokeVoidAsync("alert", $"Bill successfully sent to {email};{ownerEmail}.");
                        }
                        else
                        {
                            await JSRuntime.InvokeVoidAsync("alert", $"Bill sending failed. Please contact your system administrator.");
                        }
                    }
                    else
                    {
                        await JSRuntime.InvokeVoidAsync("alert", $"The tenant do not have a email address.");
                    }
                    break;

                case "ADBCA":
                    templateFile = Path.Combine(path, "template_adbca.html");
                   
                    template = File.ReadAllText(templateFile);
                    fullName = $"{bill.propertyDirectory.tenant.lastName} {bill.propertyDirectory.tenant.firstName} {bill.propertyDirectory.tenant.middleName}";

                    rentalDate = bill.dateDue.ToString("yyyy-MM-01");

                    template = template.Replace("[@Tenant]", fullName);
                    template = template.Replace("[@OwnerName]", bill.propertyDirectory.property.ownerFullName);

                    template = template.Replace("[@BillDocumentId]", bill.documentId);
                    unit = bill.propertyDirectory.property.description;
                    var area = bill.propertyDirectory.property.areaInSqm.ToString("0.00");
                    var rate = bill.propertyDirectory.ratePerSQMAssocDues.ToString("#,##0.00;(#,##0.00)");

                    template = template.Replace("[@Unit]", unit);
                    template = template.Replace("[@Area]", area);
                    template = template.Replace("[@Rate]", rate);
                    template = template.Replace("[@TransactionDate]", bill.transactionDate.ToString("yyyy-MM-dd"));
                    template = template.Replace("[@RentalPeriod]", rentalDate);
                    template = template.Replace("[@DueDate]", bill.dateDue.ToString("yyyy-MM-dd"));
                    template = template.Replace("[@TotalAmount]", bill.totalAmount.ToString("#,##0.00;(#,##0.00)"));
                    template = template.Replace("[@PreparedBy]", preparedBy);

                    foreach (var item in bill.billingLineItems)
                    {

                        var lineItem = $"<tr><td style=\"border: 1px solid #000000;\">{item.description}</td><td style=\"width: 50px; border: 1px solid #000000;text-align: center;\">PHP</td><td style=\"width: 200px; border: 1px solid #000000; text-align: right;\">{item.amount.ToString("#,##0.00;(#,##0.00)")}</td></tr>";
                        billItems.AppendLine(lineItem);
                    }
                    template = template.Replace("[@BillItems]", billItems.ToString());
                    template = template.Replace("[@PreparedBy]", preparedBy);
                    fileName = $"{BillId}_{DateTime.Now.ToString("yyyyMMddHHmmss")}.html";
                    fileAttachment = Path.Combine(resultPath, fileName);
                    File.WriteAllText(fileAttachment, template);
                    email = bill.propertyDirectory.tenant.emailAddress;
                    if (!string.IsNullOrEmpty(email))
                    {
                        var subject = $"Statement of account - {bill.dateDue.ToString("MMM yyyy")}";

                        var body = new StringBuilder();
                        body.AppendLine($"Dear {bill.propertyDirectory.tenant.firstName} {bill.propertyDirectory.tenant.lastName},");
                        body.AppendLine("<br/><br/>");
                        body.AppendLine($"Please see attached billing for the month of {bill.dateDue.ToString("MMM yyyy")}.");
                        body.AppendLine("<br/><br/><br/>");
                        body.AppendLine("Thanks");
                        var send = await emailService.SendEmail(email, "", subject, body.ToString(), fileAttachment);

                        if (send)
                        {
                            await JSRuntime.InvokeVoidAsync("alert", $"Bill successfully sent to {email}.");
                        }
                        else
                        {
                            await JSRuntime.InvokeVoidAsync("alert", $"Bill sending failed. Please contact your system administrator.");
                        }
                    }
                    else
                    {
                        await JSRuntime.InvokeVoidAsync("alert", $"The tenant do not have a email address.");
                    }

                    break;
                case "APMI":
                    templateFile = Path.Combine(path, "template_apmi.html");
                    break;
            }

          

            //template

        }

        protected async Task GetBills()
        {
            var data = await appDBContext.Billings.AsNoTracking()
                                            .Include(a => a.billingLineItems)
                                            .Include(a => a.propertyDirectory).ThenInclude(b => b.tenant)
                                            .Include(a => a.propertyDirectory).ThenInclude(b => b.property)
                                            .Where(a => a.companyId.Equals(CompanyId))
                                            .OrderByDescending(a => a.createDate)
                                            .ToListAsync();

            Bills = data.Select(a => new BillingViewModel()
            {
                billId = a.billId.ToString(),
                documentId = a.documentId,
                billType = a.billType,
                tenantName = $"{a.propertyDirectory.tenant.firstName} {a.propertyDirectory.tenant.lastName}",
                propertyDesc = a.propertyDirectory.property.description,
                balance = a.balance,
                amountPaid = a.amountPaid,
                totalAmount = a.totalAmount,
                transactionDate = a.transactionDate
            }).ToList();
        }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                UserName = await _sessionStorageService.GetItemAsync<string>("UserName");
                CompanyId = await _sessionStorageService.GetItemAsync<string>("CompanyId");

                DataLoaded = false;
                ErrorMessage = string.Empty;

                await GetBills();

            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
            finally
            {
                DataLoaded = true;
            }
        }

    }
}
