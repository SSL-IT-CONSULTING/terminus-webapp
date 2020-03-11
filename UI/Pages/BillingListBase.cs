using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using terminus.shared.models;
using terminus_webapp.Common;
using terminus_webapp.Data;

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
        public string ErrorMessage { get; set; }

        public string CompanyId { get; set; }

        public string UserName { get; set; }

        public void CreateBill()
        {
            NavigationManager.NavigateTo("billingentry");
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

                                lineItem = $"<tr><td colspan = \"3\">Rental Due payable to ASRC</td><td>{unit}</td><td>{bill.propertyDirectory.property.areaInSqm.ToString()}</td><td>{item.amount.ToString("#,##0.00;(#,##0.00)")}</td><td>{vat.ToString("#,##0.00;(#,##0.00)")}</td><td>{penalty.ToString("#,##0.00;(#,##0.00)")}</td><td>{wt.ToString("#,##0.00;(#,##0.00)")}</td><td> {total.ToString("#,##0.00;(#,##0.00)")} </td ></tr>";
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
                    if(!string.IsNullOrEmpty(email))
                    {
                        var subject = $"Statement of account - {bill.dateDue.ToString("MMM yyyy")}";

                        var body = new StringBuilder();
                        body.AppendLine($"Dear {bill.propertyDirectory.tenant.firstName} {bill.propertyDirectory.tenant.lastName},");
                        body.AppendLine("<br/><br/>");
                        body.AppendLine($"Please see attached billing for the month of {bill.dateDue.ToString("MMM yyyy")}.");
                        body.AppendLine("<br/><br/><br/>");
                        body.AppendLine("Thanks");
                        var send = await emailService.SendEmail(email, "", subject, body.ToString(), fileAttachment);

                        if(send)
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

                case "ADBCA":
                    templateFile = Path.Combine(path, "template_adbca.html");
                   
                    template = File.ReadAllText(templateFile);
                    fullName = $"{bill.propertyDirectory.tenant.lastName} {bill.propertyDirectory.tenant.firstName} {bill.propertyDirectory.tenant.middleName}";

                    rentalDate = bill.dateDue.ToString("yyyy-MM-01");

                    template = template.Replace("[@Tenant]", fullName);

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

        protected override async Task OnInitializedAsync()
        {
            try
            {
                UserName = await _sessionStorageService.GetItemAsync<string>("UserName");
                CompanyId = await _sessionStorageService.GetItemAsync<string>("CompanyId");

                DataLoaded = false;
                ErrorMessage = string.Empty;

                var data = await appDBContext.Billings
                                             .Include(a=>a.billingLineItems)
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
