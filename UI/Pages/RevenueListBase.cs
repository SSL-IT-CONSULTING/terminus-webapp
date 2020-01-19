using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using terminus_webapp.Data;
using terminus.shared.models;

namespace terminus_webapp.Pages
{
    public class RevenueListBase:ComponentBase
    {
        [Inject]
        public AppDBContext appDBContext { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        public List<RevenueViewModel> Revenues { get; set; }
        public bool DataLoaded { get; set; }
        public string ErrorMessage { get; set; }


        public void AddRevenue()
        {
            NavigationManager.NavigateTo("revenue");
        }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                DataLoaded = false;
                ErrorMessage = string.Empty;

                var data = await appDBContext.Revenues
                                             .Include(a=>a.account)
                                              .Include(a=>a.checkDetails)      
                                             .ToListAsync();

                Revenues = data.Select(a => new RevenueViewModel()
                {
                    id = a.id.ToString(),
                    glAccountCode = a.account.accountCode,
                    glAccountName = a.account.accountDesc,
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
