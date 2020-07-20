using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using terminus.dataaccess;
using terminus.shared.models;



namespace terminus_webapp.Pages
{
    public class PropertyDirectoryListBase:ComponentBase
    {

        [Inject]
        public AppDBContext appDBContext { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public ISessionStorageService _sessionStorageService { get; set; }

        public DapperManager dapperManager { get; set; }

        public List<PropertyDirectoryViewModal> propertyDirectory { get; set; }

        public bool DataLoaded { get; set; }
        public string ErrorMessage { get; set; }

        public string CompanyId { get; set; }
        public string UserName { get; set; }

        public void AddProperDirectory()
        {
            NavigationManager.NavigateTo("propertydirectory");
        }




        protected override async Task OnInitializedAsync()
        {
            try
            {

                DataLoaded = false;
                ErrorMessage = string.Empty;

                UserName = await _sessionStorageService.GetItemAsync<string>("UserName");
                CompanyId = await _sessionStorageService.GetItemAsync<string>("CompanyId");

                var data = await appDBContext.PropertyDirectory
                                             .Include(a => a.property)
                                             .Include(a => a.tenant)
                                             .Include(a => a.company)
                                             .ToListAsync();





                propertyDirectory = data.Select(a => new PropertyDirectoryViewModal()
                {
                    id = a.id,
                    propertyId = a.propertyId,
                    propertyDesc = a.property.description,
                    dateFrom = a.dateFrom,
                    dateTo = a.dateTo,
                    companyId= "ASRC",
                    monthlyRate = a.monthlyRate,
                    revenueAccountId = a.revenueAccountId,
                    status = a.status,
                    tenandId = a.tenandId,
                    tenantLastNName = a.tenant.lastName,
                    tenantFirtsName = a.tenant.firstName,
                    associationDues = a.associationDues,
                    penaltyPct = a.penaltyPct,
                    ratePerSQM = a.ratePerSQM,
                    totalBalance = a.totalBalance

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
