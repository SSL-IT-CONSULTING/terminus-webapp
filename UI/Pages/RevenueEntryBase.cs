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

        [Parameter]
        public string revenueId { get; set; }

        public RevenueViewModel revenue { get; set; }
   
        public bool IsDataLoaded { get; set; }

        public void HandleAccountChange(ChangeEventArgs e)
        {
            var selected = e.Value;
        }

        protected async Task HandleValidSubmit()
        {
            if(string.IsNullOrEmpty(revenue.id))
            {
                var vatAccount = await appDBContext.GLAccounts.Where(a => a.outputVatAccount).FirstOrDefaultAsync();
                
                var company =  await appDBContext.Companies.Where(a => a.companyId.Equals("ASRC")).FirstOrDefaultAsync();

                var r = new Revenue();
                r.id = Guid.NewGuid();
                r.transactionDate = revenue.transactionDate;
                r.dueDate = revenue.dueDate;
                r.description = revenue.description;
                r.account = revenue.revenueAccounts.Where(a => a.accountId.Equals(Guid.Parse(revenue.glAccountId))).FirstOrDefault();
                r.cashAccount = await appDBContext.GLAccounts.Where(a => a.accountId.Equals(Guid.Parse(revenue.cashAccountId))).FirstOrDefaultAsync();
                r.propertyDirectory = revenue.propertyDirectories.Where(a => a.id.ToString().Equals(revenue.propertyDirectoryId, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                r.amount = revenue.amount;
                r.createDate = DateTime.Now;
                r.createdBy = "testadmin";
                r.receiptNo = revenue.receiptNo;
                r.reference = revenue.reference;
                r.remarks = string.Format("{0}_{1}_{2} {3}", r.account.accountDesc, r.propertyDirectory.property.description, r.propertyDirectory.tenant.lastName, r.propertyDirectory.tenant.lastName);
                r.company = company;
                r.cashOrCheck = revenue.cashOrCheck;
                
                if(r.cashOrCheck.Equals("1"))
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


                var jeHdr = new JournalEntryHdr() { createDate = DateTime.Now, createdBy = "testadmin", id = Guid.NewGuid(), source="revenue", sourceId=r.id.ToString() };

                jeHdr.description = r.remarks;
                jeHdr.company = company;

                var amount = r.cashOrCheck.Equals("1") ? r.checkDetails.amount : r.amount;
                var beforeVat = 0m;
                var vat = 0m;

                if (amount!=0)
                {
                    beforeVat = Math.Round(amount / 1.12m, 2);
                    vat = amount - beforeVat;
                }
               
                r.taxAmount = vat;
                var jeList = new List<JournalEntryDtl>()
                {
                    new JournalEntryDtl()
                    {
                    id = Guid.NewGuid().ToString(),
                    createDate = DateTime.Now,
                    createdBy = "testadmin",
                    lineNumber=0,
                    amount = amount - vat,
                    type ="D",
                    account = r.account
                    },
                    new JournalEntryDtl()
                    {
                    id = Guid.NewGuid().ToString(),
                    createDate = DateTime.Now,
                    createdBy = "testadmin",
                    lineNumber=1,
                    amount = vat,
                    type ="D",
                    account = vatAccount
                    },
                    new JournalEntryDtl()
                    {
                    id = Guid.NewGuid().ToString(),
                    createDate = DateTime.Now,
                    createdBy = "testadmin",
                    lineNumber=2,
                    amount = amount,
                    type ="C",
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

        protected async Task HandleInvalidSubmit()
        {

        }


        protected override async Task OnInitializedAsync()
        {
            IsDataLoaded = false;
        

            if(string.IsNullOrEmpty(revenueId))
            {
                revenue = new RevenueViewModel();
                revenue.transactionDate = DateTime.Today;
                revenue.cashOrCheck = "0";
            }
            else
            {
                var id = Guid.Parse(revenueId);

                var data = await appDBContext.Revenues
                    .Include(a=>a.account)
                    .Include(a=>a.cashAccount)
                    .Include(a=>a.propertyDirectory)
                    .Include(a=>a.checkDetails)
                    .Where(r => r.id.Equals(id)).FirstOrDefaultAsync();

                revenue = new RevenueViewModel() {
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
                    checkAmount = data.cashOrCheck.Equals("1")?data.checkDetails.amount:0,
                    bankName = data.cashOrCheck.Equals("1") ? data.checkDetails.bankName : "",
                    branch = data.cashOrCheck.Equals("1") ? data.checkDetails.branch : "", 
                    checkDate = data.cashOrCheck.Equals("1") ? (DateTime?)data.checkDetails.checkDate : null,
                    propertyDirectoryId = data.propertyDirectory.id.ToString()
                };
            }

            revenue.revenueAccounts = await appDBContext.GLAccounts.Where(a => a.revenue || a.cashAccount).ToListAsync();
            revenue.propertyDirectories = await appDBContext.PropertyDirectory.Include(a => a.property).Include(a => a.tenant).ToListAsync();

            IsDataLoaded = true;
        }

        public void NavigateToList()
        {
            NavigationManager.NavigateTo("/revenuelist");
        }
    }
}
