using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using terminus.shared.models;
using terminus_webapp.Data;
using Microsoft.EntityFrameworkCore;

namespace terminus_webapp.Components
{
    public class JournalEntryDetailBase:ComponentBase
    {
        [Inject]
        public AppDBContext appDBContext { get; set; }

        public JournalEntryDtlViewModel JournalEntryDtl { get; set; }

        public List<GLAccount> GLAccounts { get; set; }

        [Parameter]
        public EventCallback<bool> SaveEventCallback { get; set; }

        [Parameter]
        public EventCallback<bool> CancelEventCallback { get; set; }


        public bool IsDataLoaded { get; set; }
        public string ErrorMessage { get; set; }
        public bool ShowDialog { get; set; }

        public void CloseDialog()
        {
            ShowDialog = false;
            StateHasChanged();
        }

        public void OpenDialogBox()
        {
            ShowDialog = true;
            StateHasChanged();
        }

        public void StateHasChange()
        {
            StateHasChanged();
        }

        protected async Task HandleValidSubmit()
        {
            var glAccount = GLAccounts.Where(a => a.accountId.ToString()
                                      .Equals(this.JournalEntryDtl.accountId))
                                      .FirstOrDefault();

            this.JournalEntryDtl.accountCode = glAccount.accountCode;
            this.JournalEntryDtl.accountName = glAccount.accountDesc;

            await SaveEventCallback.InvokeAsync(true);
            StateHasChanged();
        }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                IsDataLoaded = false;
                JournalEntryDtl = new JournalEntryDtlViewModel();
                GLAccounts = await appDBContext.GLAccounts.ToListAsync();

            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
            finally
            {
                IsDataLoaded = true;
            }

        }



    }
}
