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
    public class PropertyListBase:ComponentBase
    {

        [Inject]
        public AppDBContext appDBContext { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public ISessionStorageService _sessionStorageService { get; set; }

        public DapperManager dapperManager { get; set; }

        public List<Property> properties { get; set; }

        public bool DataLoaded { get; set; }
        public string ErrorMessage { get; set; }

        public string CompanyId { get; set; }
        public string UserName { get; set; }

        public void PropertyEntry()
        {
            NavigationManager.NavigateTo("propertyentry");
        }

        public void PropertyEdit()
        {
            NavigationManager.NavigateTo("propertyedit");
        }

        public void PropertyList()
        {
            NavigationManager.NavigateTo("propertylist");

        }

        protected override async Task OnInitializedAsync()
        {
            try
            {

                DataLoaded = false;
                ErrorMessage = string.Empty;

                UserName = await _sessionStorageService.GetItemAsync<string>("UserName");
                CompanyId = await _sessionStorageService.GetItemAsync<string>("CompanyId");


                var data = await appDBContext.Properties
                                             //.OrderByDescending(a => a.createDate)
                                             .Select(a => new { id = a.id, company = a.company, description = a.description, address = a.address})
                                             .OrderBy(a => a.description)
                                             .ToListAsync();


                


                properties = data.Select(a => new Property()
                {

                    

                id = a.id,
                    company = a.company,
                    description = a.description,
                    address = a.address
                    //propertyType = a.propertyType,
                    //area =a.area
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
