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

        public bool IsEditOnly { get; set; }
        
        public bool IsDataLoaded { get; set; }

        public void AddGLAccount()
        {
            NavigationManager.NavigateTo("glaccountentry");
        }

        public void NavigateToList()
        {
            NavigationManager.NavigateTo("/glaccountlist");
        }

        protected async Task HandleValidSubmit()
        {


            UserName = await _sessionStorageService.GetItemAsync<string>("UserName");
            CompanyId = await _sessionStorageService.GetItemAsync<string>("CompanyId");

            bool _rev;
            bool _exp;
            bool _cshacc;
            bool _outvat;


            if (glAccountView.revenue == "N")
            {
                _rev = false;
            }
            else
            {
                _rev = true;
            }


            if (glAccountView.expense == "N")
            {
                _exp = false;
            }
            else
            {
                _exp = true;
            }


            if (glAccountView.cashAccount == "N")
            {
                _cshacc = false;
            }
            else
            {
                _cshacc = true;
            }


            if (glAccountView.outputVatAccount == "N")
            {
                _outvat = false;
            }
            else
            {
                _outvat = true;
            }


            if (string.IsNullOrEmpty(accountId))
            {

                //var data = await appDBContext.GLAccounts.ToListAsync();
                //.Select(a => new { id = a.id, company = a.company, lastName = a.lastName, firstName = a.firstName, middleName = a.middleName, contactNumber = a.contactNumber, emailAddress = a.emailAddress })

                int maxRow = appDBContext.GLAccounts.Max(a => a.rowOrder);
                int _maxRow;
                if (maxRow == null)
                {
                    _maxRow = 1;
                }
                else
                {
                    _maxRow = maxRow + 1;
                }

                GLAccount gl = new GLAccount()
                {


                    accountId = Guid.NewGuid(),
                    createDate = DateTime.Now,
                    createdBy = UserName,
                    companyId = CompanyId,
                    accountCode = glAccountView.accountCode,
                    accountDesc = glAccountView.accountDesc,
                    balance = glAccountView.balance,
                    revenue = _rev,
                    expense = _exp,
                    cashAccount = _cshacc,
                    rowOrder = _maxRow,
                    outputVatAccount = _outvat
                };


                appDBContext.GLAccounts.Add(gl);
                await appDBContext.SaveChangesAsync();
            }

            else
            {

                var data = await appDBContext.GLAccounts
                                    //.Select(a => new { id = a.id, company = a.company, lastName = a.lastName, firstName = a.firstName, middleName = a.middleName, contactNumber = a.contactNumber, emailAddress = a.emailAddress })
                                    .Include(a => a.company)
                                    .Where(r => r.accountId.Equals(Guid.Parse(accountId))).FirstOrDefaultAsync();


                data.updateDate = DateTime.Now;
                data.updatedBy = UserName;
                data.accountCode = glAccountView.accountCode;
                data.accountDesc = glAccountView.accountDesc;
                data.balance = glAccountView.balance;
                data.revenue = _rev;
                data.expense = _exp;
                data.outputVatAccount = _outvat;
                //data.cashAccount = _cshacc;
                //data.rowOrder = _maxRow;

                appDBContext.GLAccounts.Update(data);
                await appDBContext.SaveChangesAsync();
            }


                

                StateHasChanged();

                NavigateToList();
            
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
                IsEditOnly = false;

                if (string.IsNullOrEmpty(accountId))
                {
                    glAccountView = new GLAccountVM();
                   // glAccount.createDate = DateTime.Today;

                }
                else
                {
                    IsViewOnly = false;
                    IsEditOnly = true;
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
