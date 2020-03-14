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
    public class GLAccountListBase:ComponentBase

    {

        [Inject]
        public AppDBContext appDBContext { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public ISessionStorageService _sessionStorageService { get; set; }

        public DapperManager dapperManager { get; set; }

        public List<GLAccountVM> glAccount { get; set; }

        public bool DataLoaded { get; set; }
        public string ErrorMessage { get; set; }

        public string CompanyId { get; set; }
        public string UserName { get; set; }

        public void AddGLAccount()
        {
            NavigationManager.NavigateTo("glaccountentry");
        }




        protected override async Task OnInitializedAsync()
        {
            try
            {

                DataLoaded = false;
                ErrorMessage = string.Empty;

                UserName = await _sessionStorageService.GetItemAsync<string>("UserName");
                CompanyId = await _sessionStorageService.GetItemAsync<string>("CompanyId");

                var data = await appDBContext.GLAccounts
                                             .OrderByDescending(a => a.createDate)
                                             .Select(a => new { accountId = a.accountId, accountCode = a.accountCode, accountDesc = a.accountDesc, balance = a.balance, companyId = a.companyId, revenue = a.revenue, expense = a.expense, cashAccount = a.cashAccount, outputVatAccount = a.outputVatAccount, rowOrder = a.rowOrder, deleted = a.deleted})
                                             .Where(a => a.companyId.Equals(CompanyId) && a.deleted.Equals(false))
                                             .OrderBy(a => a.rowOrder)
                                             .ToListAsync();

                


                glAccount = data.Select(a => new GLAccountVM()
                {


                accountId = a.accountId,
                    accountCode = a.accountCode,
                    accountDesc = a.accountDesc,
                    balance = a.balance,
                    companyid = a.companyId,
                    revenue = a.revenue ? "Yes":"No",
                    expense = a.expense ? "Yes":"No",
                    cashAccount = a.cashAccount ? "Yes":"No",
                    outputVatAccount = a.outputVatAccount ? "Yes":"No",
                    rowOrder = a.rowOrder,

                }).ToList();

            }
            catch (Exception ex)
            {
                ErrorMessage = ex.ToString();
            }
            finally
            {
                DataLoaded = true;
            }
        }
    }
}
