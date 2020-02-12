using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using terminus.shared.models;
using terminus_webapp.Components;
using terminus_webapp.Data;

namespace terminus_webapp.Pages
{
    public class CollectionsEntryBase:ComponentBase
    {
        [Inject]
        private AppDBContext appDBContext { get; set; }

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

                    //revenue.tenants = tenants.Select(a => a.tenant).ToList();
                    //StateHasChanged();
                    if (tenants.Count==1)
                    {
                        revenue.tenantId = tenants.First().tenandId;
                        revenue.tenantName = $"{tenants.First().tenant.lastName} {tenants.First().tenant.firstName}";
                    }
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

        protected async void CreateBillEntry()
        {
            var bill = await appDBContext.Billings
                .Include(b => b.propertyDirectory)
                .Where(b => b.propertyDirectory.propertyId.Equals(revenue.propertyId)
                && b.propertyDirectory.tenandId.Equals(revenue.tenantId)
                 && revenue.dueDate.Value >= b.propertyDirectory.dateFrom
                 && revenue.dueDate.Value <= b.propertyDirectory.dateTo
                 && b.companyId.Equals(CompanyId)
                 && b.dateDue == revenue.dueDate.Value
                ).FirstOrDefaultAsync();

            if(bill!=null)
            {
                revenue.billingDocumentId = bill.documentId;
                revenue.billingId = bill.billId.ToString();
                
                if(bill.balance>0)
                {
                        this.revenue.amount = bill.balance;
                        this.revenue.checkAmount = bill.balance;
                }
                StateHasChanged();
                return;
            }

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

            BillingEntry.InitParameters(revenue.propertyId, revenue.tenantId, revenue.dueDate);
            await BillingEntry.InitNewBilling();

            BillingEntry.OpenDialogBox();
            //BillingEntry.StateHasChange();
            StateHasChanged();
        }

        protected void BillEntry_OnSave()
        {
            this.revenue.billingId = BillingEntry.billing.billId.ToString();
            this.revenue.billingDocumentId = BillingEntry.billing.documentId;
            this.revenue.amount = BillingEntry.billing.balance;
            this.revenue.checkAmount = BillingEntry.billing.balance;
          
        }

        protected void BillEntry_OnCancel()
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

                    var vatAccount = await appDBContext.GLAccounts.Where(a => a.outputVatAccount).FirstOrDefaultAsync();

                    var r = new Revenue();
                    r.id = Guid.NewGuid();
                    r.transactionDate = revenue.transactionDate;
                    r.dueDate = revenue.dueDate;
                    r.description = revenue.description;
                    r.account = revenue.revenueAccounts.Where(a => a.accountId.Equals(Guid.Parse(revenue.glAccountId))).FirstOrDefault();
                    r.cashAccount = await appDBContext.GLAccounts.Where(a => a.accountId.Equals(Guid.Parse(revenue.cashAccountId))).FirstOrDefaultAsync();
                    r.propertyDirectory = pd; //revenue.propertyDirectories.Where(a => a.id.ToString().Equals(revenue.propertyDirectoryId, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                    r.amount = revenue.amount;
                    r.createDate = DateTime.Now;
                    r.createdBy = UserName;
                    r.receiptNo = revenue.receiptNo;
                    r.reference = revenue.reference;
                    r.remarks = string.Format("{0}_{1}_{2} {3}", r.account.accountDesc, r.propertyDirectory.property.description, r.propertyDirectory.tenant.lastName, r.propertyDirectory.tenant.firstName);
                    r.companyId = CompanyId;
                    r.cashOrCheck = revenue.cashOrCheck;
                    r.billId = Guid.Parse(revenue.billingId);

                    var bill = await appDBContext.Billings.Include(a=>a.propertyDirectory)
                        .Where(b => b.billId.Equals(r.billId)).FirstOrDefaultAsync();

                    if (r.cashOrCheck.Equals("1"))
                    {
                        r.checkDetails = new CheckDetails()
                        {
                            amount = revenue.checkAmount,
                            bankName = revenue.bankName,
                            branch = revenue.branch,
                            checkDate = revenue.checkDate.HasValue ? revenue.checkDate.Value : DateTime.MinValue,
                            checkDetailId = Guid.NewGuid()
                        };
                        r.amount = 0;
                    }
                    
                    appDBContext.Revenues.Add(r);


                    var jeHdr = new JournalEntryHdr() { createDate = DateTime.Now, createdBy = UserName, id = Guid.NewGuid(), source = "revenue", sourceId = r.id.ToString(), companyId = CompanyId, postingDate = r.transactionDate };

                    jeHdr.description = r.remarks;
                    jeHdr.postingDate = r.transactionDate;

                    var amount = r.cashOrCheck.Equals("1") ? r.checkDetails.amount : r.amount;

                    if (bill != null)
                    {
                        bill.amountPaid = bill.amountPaid + amount;
                        bill.balance = bill.totalAmount - bill.amountPaid;
                        bill.propertyDirectory.totalBalance = bill.balance;

                        appDBContext.Billings.Update(bill);
                    }

                    var beforeVat = 0m;
                    var vat = 0m;

                    if (amount != 0)
                    {
                        beforeVat = Math.Round(amount / 1.12m, 2);
                        vat = amount - beforeVat;
                    }

                    r.beforeTax = beforeVat;
                    r.taxAmount = vat;
                    var jeList = new List<JournalEntryDtl>()
                {
                    new JournalEntryDtl()
                    {
                    id = Guid.NewGuid().ToString(),
                    createDate = DateTime.Now,
                    createdBy = UserName,
                    lineNumber=0,
                    amount = amount - vat,
                    type ="C",
                    account = r.account
                    },
                    new JournalEntryDtl()
                    {
                    id = Guid.NewGuid().ToString(),
                    createDate = DateTime.Now,
                    createdBy = UserName,
                    lineNumber=1,
                    amount = vat,
                    type ="C",
                    account = vatAccount
                    },
                    new JournalEntryDtl()
                    {
                    id = Guid.NewGuid().ToString(),
                    createDate = DateTime.Now,
                    createdBy = UserName,
                    lineNumber=2,
                    amount = amount,
                    type ="D",
                    account = r.cashAccount
                    },
                };

                    jeHdr.JournalDetails = jeList.AsEnumerable();
                    r.journalEntry = jeHdr;

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
                        .Include(a => a.account)
                        .Include(a => a.cashAccount)
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
                        glAccountId = data.accountId.ToString(),
                        glAccountCode = data.account.accountCode,
                        glAccountName = data.account.accountDesc,
                        amount = data.cashOrCheck.Equals("0") ? data.amount : data.checkDetails.amount,
                        cashAccountId = data.cashAccount.accountId.ToString(),
                        cashAccountCode = data.cashAccount.accountCode,
                        cashAccountName = data.cashAccount.accountDesc,
                        cashOrCheck = data.cashOrCheck,
                        checkAmount = data.cashOrCheck.Equals("1") ? data.checkDetails.amount : 0,
                        bankName = data.cashOrCheck.Equals("1") ? data.checkDetails.bankName : "",
                        branch = data.cashOrCheck.Equals("1") ? data.checkDetails.branch : "",
                        checkDate = data.cashOrCheck.Equals("1") ? (DateTime?)data.checkDetails.checkDate : null,
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

                revenue.revenueAccounts = await appDBContext.GLAccounts.Where(a => a.revenue || a.cashAccount).ToListAsync();
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
