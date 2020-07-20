using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using terminus.dataaccess;
using terminus.shared.models;
using terminus_webapp.Data;

namespace terminus_webapp.Pages
{
    public class JournalDetailsBase:ComponentBase
    {
        [Inject]
        public AppDBContext appDBContext { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }


        [Inject]
        public DapperManager dapperManager { get; set; }

        [Inject]
        public ISessionStorageService _sessionStorageService { get; set; }

        [Parameter]
        public string accountId { get; set; }
        public string CompanyId { get; set; }
        public string UserName { get; set; }

        public bool IsDataLoaded { get; set; }
        public string ErrorMessage { get; set; }
        public List<JournalDetailViewModel> JournalEntries { get; set; }

        public decimal? totalDR { get; set; }
        public decimal? totalCR { get; set; }

        public decimal? totalBalance { get; set; }

        public string HeaderTitle { get; set; }
        public void NavigateToList()
        {
            NavigationManager.NavigateTo("/journalentrylist");
        }
        protected override async Task OnInitializedAsync()
        {
            try
            {
                IsDataLoaded = false;
                ErrorMessage = string.Empty;

                UserName = await _sessionStorageService.GetItemAsync<string>("UserName");
                CompanyId = await _sessionStorageService.GetItemAsync<string>("CompanyId");

                var param = new Dapper.DynamicParameters();
                param.Add("accountId", Guid.Parse(accountId), System.Data.DbType.Guid);


                JournalEntries = await dapperManager.GetAllAsync<JournalDetailViewModel>("spGetGLTransactions", param);

                var glAccount = await appDBContext.GLAccounts.Where(a => a.accountId.Equals(Guid.Parse(accountId))).FirstOrDefaultAsync();

                if(glAccount!=null)
                    HeaderTitle = $"{glAccount.accountCode} - {glAccount.accountDesc}";
                
                totalDR = JournalEntries.Sum(a => a.drAmt);
                totalCR = JournalEntries.Sum(a => a.crAmt);

                if(totalDR.HasValue || totalCR.HasValue)
                {
                    totalBalance = (totalDR.HasValue ? totalDR.Value : 0) - (totalCR.HasValue ? totalCR.Value : 0);
                }

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
