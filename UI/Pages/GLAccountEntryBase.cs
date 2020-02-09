using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using System;
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

        public GLAccountVM glAccountView { get; set; }

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

        public void NavigateToList()
        {
            NavigationManager.NavigateTo("/glaacountlsit");
        }

        protected async Task HandleValidSubmit()
        {
            if (string.IsNullOrEmpty(glAccountView.accountCode))
            {
                UserName = await _sessionStorageService.GetItemAsync<string>("UserName");
                CompanyId = await _sessionStorageService.GetItemAsync<string>("CompanyId");

                GLAccount gl = new GLAccount() { accountId = Guid.NewGuid() };

                //var gla = new GLAccount() { accountId = Guid.NewGuid() };
                //gla.accountId = Guid.NewGuid();
                //gla.createDate = DateTime.Now;
                //gla.createdBy = UserName;
                //gla.accountCode = glAccount.accountCode;
                //gla.accountDesc = glAccount.accountDesc;
                //gla.balance = glAccount.balance;
                //gla.revenue = Boolean.Parse(glAccount.revenue.ToString());
                //gla.expense = Boolean.Parse(glAccount.expense.ToString());
                //gla.cashAccount = Boolean.Parse(glAccount.cashAccount.ToString());
                //gla.outputVatAccount = Boolean.Parse(glAccount.outputVatAccount.ToString());
                //gla.rowOrder = glAccount.rowOrder;

                //appDBContext.GLAccounts.Add(gla);


                
                //await appDBContext.SaveChangesAsync();

                StateHasChanged();

                NavigateToList();
            }
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
                    glAccountView = new GLAccountVM();
                   // glAccount.createDate = DateTime.Today;

                }
                else
                {
                    IsViewOnly = true;
                    var id = Guid.Parse(accountId);

                    var data = await appDBContext.GLAccounts.Where(a=>a.accountId.Equals(Guid.Parse(accountId))).FirstOrDefaultAsync();

                    glAccountView = new GLAccountVM()
                    {
                        accountId = data.accountId,
                        accountCode = data.accountCode,
                        accountDesc = data.accountDesc,
                        balance = data.balance,
                        companyid = data.companyId,
                        revenue = data.revenue?"Y":"N",
                        expense = data.expense?"Y":"N",
                        cashAccount = data.cashAccount?"Y":"N",
                        outputVatAccount = data.outputVatAccount?"Y":"N",
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
