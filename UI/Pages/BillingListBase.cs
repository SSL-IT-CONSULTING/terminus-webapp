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
    public class BillingListBase:ComponentBase
    {
        [Inject]
        public AppDBContext appDBContext { get; set; }

        [Inject]
        public DapperManager dapperManager { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public ISessionStorageService _sessionStorageService { get; set; }

        public List<BillingViewModel> Bills { get; set; }

        public bool DataLoaded { get; set; }
        public string ErrorMessage { get; set; }

        public string CompanyId { get; set; }

        public string UserName { get; set; }

        public void CreateBill()
        {
            NavigationManager.NavigateTo("billingentry");
        }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                UserName = await _sessionStorageService.GetItemAsync<string>("UserName");
                CompanyId = await _sessionStorageService.GetItemAsync<string>("CompanyId");

                DataLoaded = false;
                ErrorMessage = string.Empty;

                var data = await appDBContext.Billings
                                             .Include(a=>a.billingLineItems)
                                             .Include(a => a.propertyDirectory).ThenInclude(b => b.tenant)
                                             .Include(a => a.propertyDirectory).ThenInclude(b => b.property)
                                             .Where(a => a.companyId.Equals(CompanyId))
                                             .OrderByDescending(a => a.createDate)
                                             .ToListAsync();

                Bills = data.Select(a => new BillingViewModel()
                {
                    billId = a.billId.ToString(),
                    documentId = a.documentId,
                    billType = a.billType,
                    tenantName = $"{a.propertyDirectory.tenant.firstName} {a.propertyDirectory.tenant.lastName}",
                    propertyDesc = a.propertyDirectory.property.description,
                    balance = a.balance,
                    amountPaid = a.amountPaid,
                    totalAmount = a.totalAmount,
                    transactionDate = a.transactionDate
                }).ToList();

            }
            catch (Exception ex)
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
