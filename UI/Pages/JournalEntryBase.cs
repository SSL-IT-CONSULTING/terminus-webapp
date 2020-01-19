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
    public class JournalEntryBase:ComponentBase
    {
        [Inject]
        public AppDBContext appDBContext { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Parameter]
        public string accountId { get; set; }
        public string companyId { get; set; }


        public bool IsDataLoaded { get; set; }
        public string ErrorMessage { get; set; }
        public JournalEntryViewModel journalEntry { get; set; }

        protected async Task HandleValidSubmit()
        {
            var id = Guid.NewGuid();

            var jeHdr = new JournalEntryHdr() { createDate = DateTime.Now, createdBy = "testadmin", id = id, source = "JE", sourceId = id.ToString() };

            jeHdr.description = journalEntry.detailDescription;
            jeHdr.companyId = companyId;
            jeHdr.postingDate = journalEntry.postingDate;
            jeHdr.transactionDate = journalEntry.transactionDate;

            jeHdr.remarks = journalEntry.remarks;

            var amount = journalEntry.amount;
         
            var jeList = new List<JournalEntryDtl>()
                {
                    new JournalEntryDtl()
                    {
                    id = Guid.NewGuid().ToString(),
                    createDate = DateTime.Now,
                    createdBy = "testadmin",
                    lineNumber=0,
                    amount = amount,
                    type =journalEntry.type,
                    accountId = Guid.Parse(journalEntry.accountId),
                    description = journalEntry.detailDescription
                    }
                };

            jeHdr.JournalDetails = jeList.AsEnumerable();
            

            appDBContext.JournalEntriesHdr.Add(jeHdr);
            await appDBContext.SaveChangesAsync();


            StateHasChanged();
            NavigateToList();
        }

        protected async Task HandleInvalidSubmit()
        {

        }

        protected void NavigateToList()
        {
            NavigationManager.NavigateTo("/journalentrylist");
        }

        protected override async Task OnInitializedAsync()
        {
            companyId = "ASRC";

           

            journalEntry = new JournalEntryViewModel();
            journalEntry.transactionDate = DateTime.Today;
            journalEntry.postingDate = DateTime.Today;

            try
            {
                var id = Guid.Parse(accountId);

                var account = await appDBContext.GLAccounts.Where(a=>a.accountId.Equals(id)).FirstOrDefaultAsync();

                journalEntry.accountId = accountId;
                journalEntry.accountCode = account.accountCode;
                journalEntry.accountName = account.accountDesc;
            }
            catch(Exception ex)
            {
                ErrorMessage = ex.Message;
            }
           finally
            {
                IsDataLoaded = true;
            }

            IsDataLoaded = true;


        }
    }
}
