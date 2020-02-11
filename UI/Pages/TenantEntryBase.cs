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
    public class TenantEntryBase:ComponentBase
    {

        [Inject]
        public AppDBContext appDBContext { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public ISessionStorageService _sessionStorageService { get; set; }

        public DapperManager dapperManager { get; set; }

        public TenantViewModel tenants { get; set; }

        [Parameter]
        public string id { get; set; }

        public bool DataLoaded { get; set; }
        public string ErrorMessage { get; set; }

        public string CompanyId { get; set; }
        public string UserName { get; set; }


        public bool IsViewOnly { get; set; }

        public bool IsEditOnly { get; set; }

        public bool IsDataLoaded { get; set; }

        public void AddGLAccount()
        {
            NavigationManager.NavigateTo("tenantentry");
        }

        public void NavigateToList()
        {
            NavigationManager.NavigateTo("tenantlist");
        }

        protected async Task HandleValidSubmit()
        {


            UserName = await _sessionStorageService.GetItemAsync<string>("UserName");
            CompanyId = await _sessionStorageService.GetItemAsync<string>("CompanyId");

            var company = await appDBContext.Companies.Where(a => a.companyId.Equals(CompanyId)).FirstOrDefaultAsync();

            if (string.IsNullOrEmpty(id))
            {

                var id = Guid.NewGuid();

                

                Tenant t = new Tenant()
                {

                    id = id.ToString(),
                    company = company,

                    updateDate = DateTime.Now,
                    updatedBy = UserName,
                    lastName = tenants.lastName,
                    firstName = tenants.firstName,
                    middleName = tenants.middleName,
                    contactNumber = tenants.contactNumber,
                    emailAddress = tenants.emailAddress,

                };


                appDBContext.Tenants.Add(t);

                

                

            }
            else
            {


                var data = await appDBContext.Tenants
                                                //.Select(a => new { id = a.id, company = a.company, lastName = a.lastName, firstName = a.firstName, middleName = a.middleName, contactNumber = a.contactNumber, emailAddress = a.emailAddress })
                                                .Include(a => a.company)
                                                .Where(r => r.id.Equals(id)).FirstOrDefaultAsync();



                data.lastName = tenants.lastName;
                data.firstName = tenants.firstName;
                data.middleName = tenants.middleName;
                data.contactNumber = tenants.contactNumber;
                data.emailAddress = tenants.emailAddress;
 





                appDBContext.Tenants.Update(data);
                


                
            }


            await appDBContext.SaveChangesAsync();
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
                IsEditOnly = false;
                ErrorMessage = string.Empty;

                if (string.IsNullOrEmpty(id))
                {
                    tenants = new TenantViewModel();
                    // glAccount.createDate = DateTime.Today;
                    tenants.id = Guid.NewGuid().ToString();
                }
                else
                {
                    IsViewOnly = false;
                    IsEditOnly = true;

                    //var id = Guid.Parse(Id);
                    //var tenatnid = Guid.Parse(id);
                    var data = await appDBContext.Tenants
                                                    .Include(a => a.company)
                                                    .Where(r => r.id.Equals(id)).FirstOrDefaultAsync();
                    //var data = await appDBContext.Tenants.Where(a => a.id.Equals(Guid.Parse(id))).ToListAsync();

                    tenants = new TenantViewModel()
                    {
                        id = data.id,
                        companyid = data.company.companyId,
                        company = data.company,
                        lastName = data.lastName,
                        firstName = data.firstName,
                        middleName = data.middleName,
                        contactNumber = data.contactNumber,
                        emailAddress = data.emailAddress
                    };




                }
                

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
