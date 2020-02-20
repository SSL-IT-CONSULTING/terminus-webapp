using Blazored.SessionStorage;
using Dapper;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using terminus.shared.models;
using terminus_webapp.Common;
using terminus_webapp.Components;
using terminus_webapp.Data;

namespace terminus_webapp.Pages
{
    public class CollectionsEntryBase:ComponentBase
    {
        [Inject]
        private AppDBContext appDBContext { get; set; }

        [Inject]
        private DapperManager dapperManager { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }


        [Inject]
        public ISessionStorageService _sessionStorageService { get; set; }

        [Inject]
        private IJSRuntime JSRuntime { get; set; }

        [Parameter]
        public string revenueId { get; set; }

        public RevenueViewModel revenue { get; set; }

        //private string _VendorId;
        //public string VendorId
        //{
        //    get { return _VendorId; }
        //    set
        //    {
        //        _VendorId = value;
        //        this.expense.vendorId = _VendorId;
        //        VendorSelected(_VendorId);
        //    }
        //}

        private string _PropertyId;
        public string PropertyId { get { return _PropertyId; } set { 
            _PropertyId = value;
                this.revenue.propertyId = value;
                 PopulateTenant();
            } }

        private string _BillingType;
        public string BillingType
        {
            get { return _BillingType; }
            set
            {
                _BillingType = value;
                this.revenue.billingType = value;
               
            }
        }

        private void PopulateTenant()
        {
            try
            {
                var dueDate = revenue.dueDate.HasValue ? revenue.dueDate.Value : DateTime.Today;
                var tenants = appDBContext.PropertyDirectory
                                                .Include(a=>a.tenant)
                                                .Where(a => a.propertyId.Equals(PropertyId)
                                                && dueDate >= a.dateFrom && dueDate<=a.dateTo
                                                && a.companyId.Equals(CompanyId)
                                                ).ToList();


                if(!tenants.Any())
                {
                    revenue.tenants = tenants.Select(a => a.tenant).ToList();
                    revenue.tenantId = string.Empty;
                    revenue.tenantName = string.Empty;
                }
                else
                {
                    if(!string.IsNullOrEmpty(revenue.tenantId) && 
                        !tenants.Where(t=>t.tenandId.Equals(revenue.tenantId)).Any())
                    {
                        revenue.tenantId = string.Empty;
                        revenue.tenantName = string.Empty;
                    }

                    revenue.tenantId = tenants.First().tenandId;
                    revenue.tenantName = $"{tenants.First().tenant.lastName} {tenants.First().tenant.firstName}";
                    
                }
                StateHasChanged();
            }
            catch (Exception ex)
            {

            }
        }

        public bool IsDataLoaded { get; set; }
        public bool IsViewonly { get; set; }

        public string ErrorMessage { get; set; }
        public bool DataSaved { get; set; }

        public string CompanyId { get; set; }
        public string UserName { get; set; }

        protected BillingEntry BillingEntry { get; set; }

        protected ApplyPayment ApplyPayment { get; set; }

        protected Billing Bill { get; set; }

        protected async void CreateBillEntry()
        {
            Billing bill = null;

            if (!revenue.dueDate.HasValue)
            {
                await JSRuntime.InvokeVoidAsync("alert", "Due date is required.");

                return;
            }

            if (BillingType.Equals("MB"))
            {
               bill = await appDBContext.Billings
                .Include(b => b.propertyDirectory)
                .Where(b => b.propertyDirectory.propertyId.Equals(revenue.propertyId)
                && b.propertyDirectory.tenandId.Equals(revenue.tenantId)
                 && revenue.dueDate.Value >= b.propertyDirectory.dateFrom
                 && revenue.dueDate.Value <= b.propertyDirectory.dateTo
                 && b.companyId.Equals(CompanyId)
                 && b.billType.Equals(BillingType)
                 && b.MonthYear.Equals(revenue.dueDate.Value.ToString("yyyyMM"))
                ).FirstOrDefaultAsync();
            }
            
            //if(bill!=null)
            //{
            //    revenue.billingDocumentId = bill.documentId;
            //    revenue.billingId = bill.billId.ToString();
                
            //    if(bill.balance>0)
            //    {
            //            this.revenue.amount = bill.balance;
            //            this.revenue.checkAmount = bill.balance;
            //    }
            //    StateHasChanged();
            //    return;
            //}

            var pd = await appDBContext.PropertyDirectory
             
               .Where(b => b.propertyId.Equals(revenue.propertyId)
               && b.tenandId.Equals(revenue.tenantId)
                && revenue.dueDate.Value >= b.dateFrom
                && revenue.dueDate.Value <= b.dateTo
                && b.companyId.Equals(CompanyId)).FirstOrDefaultAsync();

            if(pd==null)
            {
                await JSRuntime.InvokeVoidAsync("alert", "Tenant is not currently renting the selected property.");

                return;
            }

            var billingId = bill == null ? string.Empty : bill.billId.ToString();

            BillingEntry.InitParameters(revenue.propertyId, revenue.tenantId, revenue.dueDate, BillingType, billingId);
            await BillingEntry.InitNewBilling();

            BillingEntry.OpenDialogBox();
            StateHasChanged();
        }

        protected void ApplyPaymentLineItem(Guid Id)
        {
            RevenueLineItemViewModel revenueLineItem = null;

            var lineItem = this.revenue.revenueLineItems.Where(line => line.Id.Equals(Id)).First();

            revenueLineItem = new RevenueLineItemViewModel()
            {
                Id = lineItem.Id,
                billingLineItemId = lineItem.billingLineItemId,
                amountApplied = lineItem.amountApplied,
                description = lineItem.description,
                debitAccountId = lineItem.debitAccountId,
                creditAccountId = lineItem.creditAccountId,
                cashOrCheck = string.IsNullOrEmpty(lineItem.cashOrCheck) ? "0" : lineItem.cashOrCheck,
                bankName = lineItem.bankName,
                branch = lineItem.branch,
                checkDate = lineItem.checkDate,
                checkAmount = lineItem.checkAmount
            };

            ApplyPayment.InitParameters(Id.ToString(),revenue.revenueAccounts, revenueLineItem);
          
            ApplyPayment.OpenDialogBox();
            StateHasChanged();
        }

        protected void CollectBill()
        {
            this.revenue.collect = true;
            StateHasChanged();
        }

        protected void BillEntry_OnSave()
        {
            var billId = BillingEntry.billing.billId;

            this.revenue.billingId = billId.ToString();
            this.revenue.billingDocumentId = BillingEntry.billing.documentId;
            this.revenue.amount = BillingEntry.billing.balance;
            this.revenue.checkAmount = BillingEntry.billing.balance;
            this.revenue.collect = true;
            try
            {
                IsDataLoaded = false;
                var param = new DynamicParameters();
                param.Add("billId", billId);

                var list = dapperManager.GetAll<RevenueLineItemViewModel>("spInitCollection", param);
                var companyDefault = appDBContext.CompanyDefaults
                                    .Include(a=>a.RevenueAssocDuesAccount)
                                    .Include(a => a.RevenueAssocDuesDebitAccount)
                                    .Include(a => a.RevenueAssocDuesVatAccount)
                                    .Include(a => a.RevenueMonthlyDueAccount)
                                    .Include(a => a.RevenueMonthlyDueDebitAccount)
                                    .Include(a => a.RevenueMonthlyDueVatAccount)
                                    .Where(a => a.companyId.Equals(CompanyId)).FirstOrDefault();

                list.ForEach(a => { 
                    if(Guid.Empty.Equals(a.Id))
                    {
                        a.Id = Guid.NewGuid();
                    }

                    if (companyDefault != null)
                    {
                       switch(a.billLineType)
                        {
                            case Constants.BillLineTypes.MONTHLYBILLITEM_PREVBAL:
                            case Constants.BillLineTypes.MONTHLYBILLITEM:
                            case Constants.BillLineTypes.MONTHLYBILLITEMPENALTY:
                                if (companyDefault.RevenueMonthlyDueAccountId.HasValue)
                                {
                                    a.creditAccountId = companyDefault.RevenueMonthlyDueAccountId.Value.ToString();
                                    a.creditAccountCode = companyDefault.RevenueMonthlyDueAccount.accountCode;
                                    a.creditAccountName = companyDefault.RevenueMonthlyDueAccount.accountDesc;
                                }
                                if (companyDefault.RevenueMonthlyDueDebitAccountId.HasValue)
                                {
                                    a.debitAccountId = companyDefault.RevenueMonthlyDueAccountId.Value.ToString();
                                    a.debitAccountCode = companyDefault.RevenueMonthlyDueDebitAccount.accountCode;
                                    a.debitAccountName = companyDefault.RevenueMonthlyDueDebitAccount.accountDesc;
                                }
                                break;
                            case Constants.BillLineTypes.MONTHLYBILLITEM_VAT:
                                if (companyDefault.RevenueMonthlyDueVatAccountId.HasValue)
                                {
                                    a.creditAccountId = companyDefault.RevenueMonthlyDueVatAccountId.Value.ToString();
                                    a.creditAccountCode = companyDefault.RevenueMonthlyDueVatAccount.accountCode;
                                    a.creditAccountName = companyDefault.RevenueMonthlyDueVatAccount.accountDesc;
                                }
                                if (companyDefault.RevenueMonthlyDueDebitAccountId.HasValue)
                                {
                                    a.debitAccountId = companyDefault.RevenueMonthlyDueDebitAccountId.Value.ToString();
                                    a.debitAccountCode = companyDefault.RevenueMonthlyDueDebitAccount.accountCode;
                                    a.debitAccountName = companyDefault.RevenueMonthlyDueDebitAccount.accountDesc;
                                }
                                break;

                            case Constants.BillLineTypes.MONTHLYASSOCDUE_PREVBAL:
                            case Constants.BillLineTypes.MONTHLYASSOCDUE:
                            case Constants.BillLineTypes.MONTHLYASSOCDUEPENALTY:
                                if (companyDefault.RevenueAssocDuesAccountId.HasValue)
                                {
                                    a.creditAccountId = companyDefault.RevenueAssocDuesAccountId.Value.ToString();
                                    a.creditAccountCode = companyDefault.RevenueAssocDuesAccount.accountCode;
                                    a.creditAccountName = companyDefault.RevenueAssocDuesAccount.accountDesc;
                                }
                                if (companyDefault.RevenueAssocDuesDebitAccountId.HasValue)
                                {
                                    a.debitAccountId = companyDefault.RevenueAssocDuesDebitAccountId.Value.ToString();
                                    a.debitAccountCode = companyDefault.RevenueAssocDuesDebitAccount.accountCode;
                                    a.debitAccountName = companyDefault.RevenueAssocDuesDebitAccount.accountDesc;
                                }
                                break;
                            case Constants.BillLineTypes.MONTHLYASSOCDUE_VAT:
                                if (companyDefault.RevenueAssocDuesVatAccountId.HasValue)
                                {
                                    a.creditAccountId = companyDefault.RevenueAssocDuesVatAccountId.Value.ToString();
                                    a.creditAccountCode = companyDefault.RevenueAssocDuesVatAccount.accountCode;
                                    a.creditAccountName = companyDefault.RevenueAssocDuesVatAccount.accountDesc;
                                }
                                if (companyDefault.RevenueAssocDuesDebitAccountId.HasValue)
                                {
                                    a.debitAccountId = companyDefault.RevenueAssocDuesDebitAccountId.Value.ToString();
                                    a.debitAccountCode = companyDefault.RevenueAssocDuesDebitAccount.accountCode;
                                    a.debitAccountName = companyDefault.RevenueAssocDuesDebitAccount.accountDesc;
                                }
                                break;
                            default:
                                if (companyDefault.RevenueMonthlyDueAccountId.HasValue)
                                {
                                    a.creditAccountId = companyDefault.RevenueMonthlyDueAccountId.Value.ToString();
                                    a.creditAccountCode = companyDefault.RevenueMonthlyDueAccount.accountCode;
                                    a.creditAccountName = companyDefault.RevenueMonthlyDueAccount.accountDesc;
                                }
                                if (companyDefault.RevenueMonthlyDueDebitAccountId.HasValue)
                                {
                                    a.debitAccountId = companyDefault.RevenueMonthlyDueAccountId.Value.ToString();
                                    a.debitAccountCode = companyDefault.RevenueMonthlyDueDebitAccount.accountCode;
                                    a.debitAccountName = companyDefault.RevenueMonthlyDueDebitAccount.accountDesc;
                                }
                                break;
                        }
                    }
                
                });

                if (revenue.revenueLineItems == null)
                    revenue.revenueLineItems = new List<RevenueLineItemViewModel>();
                else
                    revenue.revenueLineItems.Clear();

                revenue.revenueLineItems.AddRange(list);


            }
            catch(Exception ex)
            {
            }
            IsDataLoaded = true;
            
        }

        protected void BillEntry_OnCancel()
        {

        }

        protected void ApplyPayment_OnSave(RevenueLineItemViewModel revenueLine)
        {
            IsDataLoaded = false;

            var revenueLineIdItemId = revenueLine.Id.ToString();

            var lineItem = this.revenue.revenueLineItems.Where(item => item.Id.Equals(Guid.Parse(revenueLineIdItemId))).First();

            lineItem.amountApplied = revenueLine.amountApplied;
            lineItem.cashOrCheck = revenueLine.cashOrCheck;
            lineItem.bankName  = revenueLine.bankName;
            lineItem.branch  = revenueLine.branch;
            lineItem.checkDate = revenueLine.checkDate;
            lineItem.debitAccountId = revenueLine.debitAccountId;

            lineItem.debitAccountCode = this.revenue.revenueAccounts.Where(a => a.accountId.Equals(Guid.Parse(lineItem.debitAccountId))).First().accountCode;
            lineItem.debitAccountName = this.revenue.revenueAccounts.Where(a => a.accountId.Equals(Guid.Parse(lineItem.debitAccountId))).First().accountDesc;

            lineItem.creditAccountId = revenueLine.creditAccountId;
            lineItem.creditAccountCode = this.revenue.revenueAccounts.Where(a => a.accountId.Equals(Guid.Parse(lineItem.creditAccountId))).First().accountCode;
            lineItem.creditAccountName = this.revenue.revenueAccounts.Where(a => a.accountId.Equals(Guid.Parse(lineItem.creditAccountId))).First().accountDesc;

            IsDataLoaded = true;

        }

        protected void ApplyPayment_OnCancel()
        {

        }

        public void HandleAccountChange(ChangeEventArgs e)
        {
            var selected = e.Value;
        }

        protected string GetAccountDesc(string account)
        {
            var data = revenue.revenueAccounts.Where(a => a.accountId.ToString().Equals(account, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            if (data != null)
                return $"{data.accountCode} - {data.accountDesc}";

            return string.Empty;
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

        protected decimal GetAmount(string cashOrCheck, decimal amount, decimal checkAmount)
        {
            if (cashOrCheck.Equals("0"))
                return amount;
            else
                return checkAmount;
        }

        protected async Task HandleValidSubmit()
        {
            try
            {
                if (string.IsNullOrEmpty(revenue.id))
                {
                    var pd = await appDBContext.PropertyDirectory
                    .Where(b => b.propertyId.Equals(revenue.propertyId)
                     && b.tenandId.Equals(revenue.tenantId)
                      && revenue.dueDate.Value >= b.dateFrom
                      && revenue.dueDate.Value <= b.dateTo
                      && b.companyId.Equals(CompanyId)).FirstOrDefaultAsync();

                    if (pd == null)
                    {
                        await JSRuntime.InvokeVoidAsync("alert", "Tenant is not currently renting the selected property.");

                        return;
                    }

                    var validationMessage = new StringBuilder();

                    if(revenue.revenueLineItems.Where(a => string.IsNullOrEmpty(a.debitAccountId)).Any())
                    {
                        validationMessage.AppendLine("Debit account is required. Please select a debit account.");
                    }

                    if (revenue.revenueLineItems.Where(a => string.IsNullOrEmpty(a.creditAccountId)).Any())
                    {
                        validationMessage.AppendLine("Credit account is required. Please select a Credit account.");
                    }

                    if(!string.IsNullOrEmpty(validationMessage.ToString()))
                    {
                        await JSRuntime.InvokeVoidAsync("alert", validationMessage.ToString());

                        return;
                    }

                    //var vatAccount = await appDBContext.GLAccounts.Where(a => a.outputVatAccount).FirstOrDefaultAsync();

                    var r = new Revenue();
                    r.id = Guid.NewGuid();
                    r.transactionDate = revenue.transactionDate;
                    r.dueDate = revenue.dueDate;
                    r.description = revenue.description;
                    //r.account = revenue.revenueAccounts.Where(a => a.accountId.Equals(Guid.Parse(revenue.glAccountId))).FirstOrDefault();
                    //r.cashAccount = await appDBContext.GLAccounts.Where(a => a.accountId.Equals(Guid.Parse(revenue.cashAccountId))).FirstOrDefaultAsync();
                    r.propertyDirectory = pd; //revenue.propertyDirectories.Where(a => a.id.ToString().Equals(revenue.propertyDirectoryId, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                    
                    r.amount = revenue.revenueLineItems.Sum(a=>a.amountApplied);
                    r.beforeTax = revenue.revenueLineItems.Where(a=>!a.description.Contains("VAT")).Sum(a => a.amountApplied);
                    r.taxAmount = revenue.revenueLineItems.Where(a => a.description.Contains("VAT")).Sum(a => a.amountApplied);

                    r.createDate = DateTime.Now;
                    r.createdBy = UserName;
                    r.receiptNo = revenue.receiptNo;
                    r.reference = revenue.reference;
                    r.remarks = string.Empty; //string.Format("{0}_{1}_{2} {3}", r.account.accountDesc, r.propertyDirectory.property.description, r.propertyDirectory.tenant.lastName, r.propertyDirectory.tenant.firstName);
                    r.companyId = CompanyId;
                    //r.cashOrCheck = revenue.cashOrCheck;
                    r.billId = Guid.Parse(revenue.billingId);

                    //var bill = await appDBContext.Billings.Include(a=>a.propertyDirectory)
                    //    .Where(b => b.billId.Equals(r.billId)).FirstOrDefaultAsync();

                    //if (r.cashOrCheck.Equals("1"))
                    //{
                    //    r.checkDetails = new CheckDetails()
                    //    {
                    //        amount = revenue.checkAmount,
                    //        bankName = revenue.bankName,
                    //        branch = revenue.branch,
                    //        checkDate = revenue.checkDate.HasValue ? revenue.checkDate.Value : DateTime.MinValue,
                    //        checkDetailId = Guid.NewGuid()
                    //    };
                    //    r.amount = 0;
                    //}
                    
                  


                    var jeHdr = new JournalEntryHdr() { createDate = DateTime.Now, createdBy = UserName, id = Guid.NewGuid(), source = "revenue", sourceId = r.id.ToString(), companyId = CompanyId, postingDate = r.transactionDate };

                    jeHdr.description = r.remarks;
                    jeHdr.postingDate = r.transactionDate;
                    var jeList = new List<JournalEntryDtl>();

                    var lineNo = 0;
                    r.revenueLineItems = new List<RevenueLineItem>();

                    foreach(var line in this.revenue.revenueLineItems)
                    {
                        var jeLine = new JournalEntryDtl();
                        jeLine.id = Guid.NewGuid().ToString();
                        jeLine.description = line.description;
                        jeLine.createDate = DateTime.Now;
                        jeLine.createdBy = UserName;
                        jeLine.lineNumber = lineNo;
                        jeLine.amount = line.amountApplied;
                        jeLine.type = "D";
                        jeLine.accountId = Guid.Parse(line.debitAccountId);
                        jeList.Add(jeLine);

                        jeLine = new JournalEntryDtl();
                        jeLine.id = Guid.NewGuid().ToString();
                        jeLine.description = line.description;
                        jeLine.createDate = DateTime.Now;
                        jeLine.createdBy = UserName;
                        jeLine.lineNumber = lineNo;
                        jeLine.amount = line.amountApplied;
                        jeLine.type = "C";
                        jeLine.accountId = Guid.Parse(line.creditAccountId);
                        jeList.Add(jeLine);


                        r.revenueLineItems.Add(new RevenueLineItem()
                        {
                            id = line.Id,
                            billingLineItemId = line.billingLineItemId,
                            amount = line.amountApplied,
                            cashOrCheck = line.cashOrCheck,
                            //checkDetails = line.cashOrCheck.Equals("1") ? new CheckDetails() { bankName = line.bankName, checkDate = line.checkDate.Value, branch = line.branch, amount = line.amount } : null,
                            bankName = line.bankName,
                            branch = line.branch,
                            checkDate = line.checkDate,
                            debitAccountId = Guid.Parse(line.debitAccountId),
                            creditAccountId = Guid.Parse(line.creditAccountId),
                            description = line.description

                        }); ;

                    }
                    r.journalEntry = jeHdr;

                    appDBContext.Revenues.Add(r);

                    jeHdr.JournalDetails = jeList.AsEnumerable();
                   

                    appDBContext.JournalEntriesHdr.Add(jeHdr);
                    await appDBContext.SaveChangesAsync();
                    StateHasChanged();

                    NavigateToList();

                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.ToString();
            }
            finally
            {
                DataSaved = true;
            }
        }

        protected async Task HandleInvalidSubmit()
        {

        }


        protected override async Task OnInitializedAsync()
        {
            IsDataLoaded = false;
            DataSaved = false;
            ErrorMessage = string.Empty;
            _BillingType = "MB";
            try
            {

                UserName = await _sessionStorageService.GetItemAsync<string>("UserName");
                CompanyId = await _sessionStorageService.GetItemAsync<string>("CompanyId");

                if (string.IsNullOrEmpty(revenueId))
                {
                    revenue = new RevenueViewModel();
                    revenue.transactionDate = DateTime.Today;
                    revenue.dueDate = DateTime.Today;
                    revenue.cashOrCheck = "0";
                    IsViewonly = false;
                }
                else
                {
                    var id = Guid.Parse(revenueId);
                    IsViewonly = true;

                    var data = await appDBContext.Revenues
                        //.Include(a => a.account)
                        //.Include(a => a.cashAccount)
                        .Include(a => a.propertyDirectory).ThenInclude(b => b.property)
                        .Include(a => a.propertyDirectory).ThenInclude(b => b.tenant)
                        .Include(a => a.checkDetails)
                        .Include(a=>a.billing)
                        .Where(r => r.id.Equals(id)).FirstOrDefaultAsync();

                    revenue = new RevenueViewModel()
                    {
                        id = data.id.ToString(),
                        transactionDate = data.transactionDate,
                        dueDate = data.dueDate,
                        //glAccountId = data.accountId.ToString(),
                        //glAccountCode = data.account.accountCode,
                        //glAccountName = data.account.accountDesc,
                        //amount = data.cashOrCheck.Equals("0") ? data.amount : data.checkDetails.amount,
                        //cashAccountId = data.cashAccount.accountId.ToString(),
                        //cashAccountCode = data.cashAccount.accountCode,
                        //cashAccountName = data.cashAccount.accountDesc,
                        //cashOrCheck = data.cashOrCheck,
                        //checkAmount = data.cashOrCheck.Equals("1") ? data.checkDetails.amount : 0,

                        //bankName = data.cashOrCheck.Equals("1") ? data.checkDetails.bankName : "",
                        //branch = data.cashOrCheck.Equals("1") ? data.checkDetails.branch : "",
                        //checkDate = data.cashOrCheck.Equals("1") ? (DateTime?)data.checkDetails.checkDate : null,
                        propertyDirectoryId = data.propertyDirectory.id.ToString(),
                        propertyDescription = data.propertyDirectory.property.description,
                        propertyId = data.propertyDirectory.propertyId,
                        tenantName = $"{data.propertyDirectory.tenant.firstName} {data.propertyDirectory.tenant.lastName}",
                        reference = data.reference,
                        receiptNo = data.receiptNo,
                        billingId = data.billing == null ? string.Empty : data.billing.billId.ToString(),
                        billingDocumentId = data.billing==null?string.Empty:data.billing.documentId
                    };
                }

                revenue.revenueAccounts = await appDBContext.GLAccounts.ToListAsync();
                //revenue.propertyDirectories = await appDBContext.PropertyDirectory.Include(a => a.property).Include(a => a.tenant).ToListAsync();
                var pdlist = await appDBContext.PropertyDirectory.Include(a => a.property).ToListAsync();

                    revenue.properties = pdlist.GroupBy(a => a.propertyId)
      .Select(grp => grp.First().property).ToList();

                revenue.tenants = new List<Tenant>();

                var vatAccount = await appDBContext.GLAccounts.Where(a => a.outputVatAccount).FirstOrDefaultAsync();

                if (vatAccount != null)
                    revenue.outputVatAccount = $"{vatAccount.accountCode} - {vatAccount.accountDesc}";

                IsDataLoaded = true;
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.ToString();
            }
            finally
            {
                IsDataLoaded = true;
            }
        }

        public void NavigateToList()
        {
            NavigationManager.NavigateTo("/collectionslist");
        }
    }
}
