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
    public class GLAccountEntryBase : ComponentBase
    {
    

        [Inject]
        public AppDBContext appDBContext { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public ISessionStorageService _sessionStorageService { get; set; }

        public DapperManager dapperManager { get; set; }

        public GLAccountVM glAccount { get; set; }

        [Parameter]
        public string accountId { get; set; }

        public bool DataLoaded { get; set; }
        public string ErrorMessage { get; set; }

        public string CompanyId { get; set; }
        public string UserName { get; set; }


        public bool IsViewOnly { get; set; }

        public bool IsDataLoaded { get; set; }

        public void AddGLAccount()
        {
            NavigationManager.NavigateTo("glaccountentry");
        }


        protected override async Task OnInitializedAsync()
        {
            try
            {
                UserName = await _sessionStorageService.GetItemAsync<string>("UserName");
                CompanyId = await _sessionStorageService.GetItemAsync<string>("CompanyId");

                IsDataLoaded = false;
                IsViewOnly = false;
                ErrorMessage = string.Empty;

                if (string.IsNullOrEmpty(accountId))
                {
                    glAccount = new GLAccountVM();
                    glAccount.createDate = DateTime.Today;

                }
                else
                {
                    IsViewOnly = true;
                    var id = Guid.Parse(accountId);

                    var data = await appDBContext.GLAccounts.FirstOrDefaultAsync();

                    glAccount = new GLAccountVM()
                    {
                        accountId = data.accountId,
                        accountCode = data.accountCode,
                        accountDesc = data.accountDesc,
                        balance = data.balance,
                        companyid = data.companyId,
                        revenue = data.revenue,
                        expense = data.expense,
                        cashAccount = data.cashAccount,
                        outputVatAccount = data.outputVatAccount,
                        rowOrder = data.rowOrder,

                    };
                }
                //glAccount = await appDBContext.GLAccountsVM.ToList();

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


    }
}
