using Microsoft.AspNetCore.Components;
using Blazored.SessionStorage;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using terminus.shared.models;
using terminus_webapp.Data;
namespace terminus_webapp.Pages
{
    public class CustomerListBase : ComponentBase
    {
        [Inject]
        public AppDBContext appDBContext { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }
        [Inject]
        public ISessionStorageService _sessionStorageService { get; set; }
        public DapperManager dapperManager { get; set; }
        public List<CustomerViewModel> customers { get; set; }
        public bool DataLoaded { get; set; }
        public string ErrorMessage { get; set; }
        public string CompanyId { get; set; }
        public string UserName { get; set; }

        public void CustomerAdd()
        {
            NavigationManager.NavigateTo("customerentry");
        }


        public void CustomerList()
        {
            NavigationManager.NavigateTo("customerlist");

        }
        protected async Task DeleteArticle(string id)
        {
            var c = await appDBContext.Customers                                                
                                                .Where(r => r.companyId.Equals(CompanyId) && r.id.Equals(id)).FirstOrDefaultAsync();
            appDBContext.Customers.Update(c);

            await appDBContext.SaveChangesAsync();

            StateHasChanged();
        }



        protected override async Task OnInitializedAsync()
        {
            try
            {

                DataLoaded = false;
                ErrorMessage = string.Empty;

                UserName = await _sessionStorageService.GetItemAsync<string>("UserName");
                CompanyId = await _sessionStorageService.GetItemAsync<string>("CompanyId");

                var data = await appDBContext.Customers
                                             .Where(a => a.companyId.Equals(CompanyId))
                                             .OrderByDescending(a => a.description)
                                             .ToListAsync();





                customers = data.Select(a => new CustomerViewModel()
                {

                    id = a.id,
                    companyId = a.companyId,
                    lastName = a.lastName,
                    firstName = a.firstName,
                    description = a.description,
                    vatRegistered = a.vatRegistered?"Y":"N",
                    address = a.address,
                    contactNo1 = a.contactNo1,
                    contactNo2 = a.contactNo2,
                    contactNo3 = a.contactNo3
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
