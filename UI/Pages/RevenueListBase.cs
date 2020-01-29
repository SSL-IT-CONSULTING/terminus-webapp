using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using terminus_webapp.Data;
using terminus.shared.models;
using Microsoft.AspNetCore.Http;
using Blazored.SessionStorage;

namespace terminus_webapp.Pages
{
    public class RevenueListBase:ComponentBase
    {
        [Inject]
        public AppDBContext appDBContext { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public ISessionStorageService _sessionStorageService { get; set; }
        
        public List<RevenueViewModel> Revenues { get; set; }

        public bool DataLoaded { get; set; }
        public string ErrorMessage { get; set; }

        public string CompanyId { get; set; }

        public string UserName { get; set; }

        public void AddRevenue()
        {
            NavigationManager.NavigateTo("collection");
        }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                UserName = await _sessionStorageService.GetItemAsync<string>("UserName");
                CompanyId = await _sessionStorageService.GetItemAsync<string>("CompanyId");

                DataLoaded = false;
                ErrorMessage = string.Empty;

                var data = await appDBContext.Revenues
                                             .Include(a=>a.account)
                                             .Include(a=>a.checkDetails)
                                             .Include(a => a.propertyDirectory).ThenInclude(b => b.tenant)
                                             .Include(a=>a.propertyDirectory).ThenInclude(b=>b.property)
                                             .Where(a=>a.companyId.Equals(CompanyId))
                                             .OrderByDescending(a => a.createDate)
                                             .ToListAsync();

                Revenues = data.Select(a => new RevenueViewModel()
                {
                    id = a.id.ToString(),
                    glAccountCode = a.account.accountCode,
                    glAccountName = a.account.accountDesc,
                    tenantName = $"{a.propertyDirectory.tenant.firstName} {a.propertyDirectory.tenant.lastName}",
                    propertyDescription = a.propertyDirectory.property.description,
                    amount = a.cashOrCheck.Equals("0") ? a.amount : a.checkDetails.amount,
                    remarks = a.remarks,
                    transactionDate = a.transactionDate
                }).ToList();

            }
            catch(Exception ex)
            {
                ErrorMessage = ex.Message;
            }
            finally
            {
                DataLoaded = true;
            }
        }
    }
}
