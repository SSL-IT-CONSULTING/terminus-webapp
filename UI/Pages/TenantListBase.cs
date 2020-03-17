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

        public List<PropertyDirectoryViewModal> propertyDirectories { get; set; }

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



        protected async Task DeleteArticle(string id)
        {



            var pd = await appDBContext.PropertyDirectory
                                                .Include(a => a.company)
                                                .Where(r => r.id.Equals(id) && r.deleted.Equals(false)).FirstOrDefaultAsync();


            pd.deleted = true;            

            appDBContext.PropertyDirectory.Update(pd);
            await appDBContext.SaveChangesAsync();

            var t = await appDBContext.Tenants
                                                .Include(a => a.company)
                                                .Where(r => r.id.Equals(id) && r.deleted.Equals(false)).FirstOrDefaultAsync();

            t.deleted = true;

            appDBContext.Tenants.Update(t);

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

                var data = await appDBContext.PropertyDirectory   
                                             .Include(a => a.tenant)
                                             .Include(a => a.property)
                                             
                                             .Where(a => a.companyId.Equals(CompanyId) && a.deleted.Equals(false))
                                             .OrderBy(a => a.property.description)
                                             .ToListAsync();



                    //                < td > @pi.description </ td >
                    //< td > @pi.propertyType </ td >
                    //< td > @pi.TenantName </ td >
                    //< td > @pi.contactNumber </ td >
                    //< td > @pi.emailAddress </ td >
                    //< td > @pi.dateFrom.ToString("MM/dd/yyyy").Replace("01/01/0001", "") </ td >

                    //< td > @pi.dateTo.ToString("MM/dd/yyyy").Replace("01/01/0001", "") </ td >
                    //< td > @pi.dueDate.ToString("MM/dd/yyyy").Replace("01/01/0001", "") </ td >

                propertyDirectories = data.Select(a => new PropertyDirectoryViewModal()
                {
                    id = a.id,
                    propertyId = a.property.id,
                    propertyDesc= a.property.description,
                    propertyEntity = a.property,
                    tenandId = a.tenant.id,
                    tenantLastNName = a.tenant.lastName,
                    tenantFirtsName = a.tenant.firstName,
                    monthlyRate = a.monthlyRate,
                    associationDues = a.associationDues,
                    penaltyPct = a.penaltyPct,
                    ratePerSQM = a.ratePerSQM,
                    totalBalance = a.totalBalance,
                    dateFrom = a.dateFrom,
                    //withWT =  a.withWT.ToString() ? "Yes" : "No",
                    withWT = a.withWT ? "Yes" : "No",
                    dateTo = a.dateTo
                    

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
