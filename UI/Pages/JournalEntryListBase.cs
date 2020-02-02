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
    public class JournalEntryListBase : ComponentBase
    {
        [Inject]
        public AppDBContext appDBContext { get; set; }

        [Inject]
        public DapperManager dapperManager { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public ISessionStorageService _sessionStorageService { get; set; }

        public List<JEListViewModel> JourlnalAccounts { get; set; }
        public bool DataLoaded { get; set; }
        public string ErrorMessage { get; set; }
        public string CompanyId { get; set; }

        public string UserName { get; set; }

        public void AddJE()
        {
            NavigationManager.NavigateTo("journalentry");
        }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                DataLoaded = false;
                ErrorMessage = string.Empty;

                UserName = await _sessionStorageService.GetItemAsync<string>("UserName");
                CompanyId = await _sessionStorageService.GetItemAsync<string>("CompanyId");

                var param = new Dapper.DynamicParameters();
                param.Add("companyId", CompanyId, System.Data.DbType.String);

                JourlnalAccounts = await dapperManager.GetAllAsync<JEListViewModel>("sp_GetGLAccountBalance", param);

                
                
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

