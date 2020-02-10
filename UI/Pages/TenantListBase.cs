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
    public class TenantListBase: ComponentBase
    {

        [Inject]
        public AppDBContext appDBContext { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public ISessionStorageService _sessionStorageService { get; set; }

        public DapperManager dapperManager { get; set; }

        public List<Tenant> tenants { get; set; }

        public bool DataLoaded { get; set; }
        public string ErrorMessage { get; set; }

        public string CompanyId { get; set; }
        public string UserName { get; set; }

        public void TenantAdd()
        {
            NavigationManager.NavigateTo("tenantentry");
        }

        public void TenantEdit()
        {
            NavigationManager.NavigateTo("tenantedit");
        }

        public void TenantList()
        {
            NavigationManager.NavigateTo("tenantlist");
     
        }




        protected override async Task OnInitializedAsync()
        {
            try
            {

                DataLoaded = false;
                ErrorMessage = string.Empty;

                UserName = await _sessionStorageService.GetItemAsync<string>("UserName");
                CompanyId = await _sessionStorageService.GetItemAsync<string>("CompanyId");

                var data = await appDBContext.Tenants   
                                             .OrderByDescending(a => a.createDate)
                                             .Select(a => new { id = a.id, company = a.company, lastName = a.lastName, firstName = a.firstName, middleName = a.middleName, contactNumber = a.contactNumber, emailAddress = a.emailAddress})
                                             .ToListAsync();





                tenants = data.Select(a => new Tenant()
                {
                    id = a.id,
                    company = a.company,
                    lastName = a.lastName,
                    firstName = a.firstName,
                    middleName = a.middleName,
                    contactNumber = a.contactNumber,
                    emailAddress = a.emailAddress,


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
