using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using terminus.shared.models;
using terminus_webapp.Data;

namespace terminus_webapp.Pages
{
    public class RevenueEntryBase:ComponentBase
    {

        [Inject]
        public AppDBContext appDBContext { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }


        [Inject]
        public ISessionStorageService _sessionStorageService { get; set; }

        [Parameter]
        public string revenueId { get; set; }

        public RevenueViewModel revenue { get; set; }
   
        public bool IsDataLoaded { get; set; }
        public bool IsViewonly { get; set; }

        public string ErrorMessage { get; set; }
        public bool DataSaved { get; set; }

        public string CompanyId { get; set; }
        public string UserName { get; set; }

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

        protected decimal GetAmount(string cashOrCheck,decimal amount, decimal checkAmount)
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
                    var vatAccount = await appDBContext.GLAccounts.Where(a => a.outputVatAccount).FirstOrDefaultAsync();

                    var r = new Revenue();
                    r.id = Guid.NewGuid();
                    r.transactionDate = revenue.transactionDate;
                    r.dueDate = revenue.dueDate;
                    r.description = revenue.description;
                    r.amount = revenue.amount;
                    r.createDate = DateTime.Now;
                    r.createdBy = UserName;
                    r.receiptNo = revenue.receiptNo;
                    r.reference = revenue.reference;
                    r.companyId = CompanyId;
                    r.cashOrCheck = revenue.cashOrCheck;

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
                    }

                    appDBContext.Revenues.Add(r);


                    var jeHdr = new JournalEntryHdr() { createDate = DateTime.Now, createdBy = UserName, id = Guid.NewGuid(), source = "revenue", sourceId = r.id.ToString(), companyId=CompanyId, postingDate = r.transactionDate };

                    jeHdr.description = r.remarks;
                    jeHdr.postingDate = r.transactionDate;

                    var amount = r.cashOrCheck.Equals("1") ? r.checkDetails.amount : r.amount;
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
                    //account = r.account
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
                    //account = r.cashAccount
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
            catch(Exception ex)
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
            try {

                UserName = await _sessionStorageService.GetItemAsync<string>("UserName");
                CompanyId = await _sessionStorageService.GetItemAsync<string>("CompanyId");

                if (string.IsNullOrEmpty(revenueId))
            {
                revenue = new RevenueViewModel();
                revenue.transactionDate = DateTime.Today;
                revenue.cashOrCheck = "0";
                    IsViewonly = false;
            }
            else
            {
                var id = Guid.Parse(revenueId);
                IsViewonly = true;

                var data = await appDBContext.Revenues
                   .Include(a => a.propertyDirectory).ThenInclude(b=>b.property)
                    .Include(a => a.propertyDirectory).ThenInclude(b => b.tenant)
                    .Include(a => a.checkDetails)
                    .Where(r => r.id.Equals(id)).FirstOrDefaultAsync();

                revenue = new RevenueViewModel() {
                    id = data.id.ToString(),
                    transactionDate = data.transactionDate,
                    dueDate = data.dueDate,
                   
                    amount = data.cashOrCheck.Equals("0") ? data.amount : data.checkDetails.amount,
                    
                    cashOrCheck = data.cashOrCheck,
                    checkAmount = data.cashOrCheck.Equals("1") ? data.checkDetails.amount : 0,
                    bankName = data.cashOrCheck.Equals("1") ? data.checkDetails.bankName : "",
                    branch = data.cashOrCheck.Equals("1") ? data.checkDetails.branch : "",
                    checkDate = data.cashOrCheck.Equals("1") ? (DateTime?)data.checkDetails.checkDate : null,
                    propertyDirectoryId = data.propertyDirectory.id.ToString(),
                    propertyDescription = data.propertyDirectory.property.description,
                    tenantName = $"{data.propertyDirectory.tenant.firstName} {data.propertyDirectory.tenant.lastName}",
                    reference = data.reference,
                    receiptNo = data.receiptNo
                };
            }

            revenue.revenueAccounts = await appDBContext.GLAccounts.Where(a => a.revenue || a.cashAccount).ToListAsync();
         
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
