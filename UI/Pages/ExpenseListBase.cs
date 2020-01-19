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
    public class ExpenseListBase:ComponentBase
    {
        [Inject]
        public AppDBContext appDBContext { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        public List<ExpenseViewModel> Expenses { get; set; }
        public bool DataLoaded { get; set; }
        public string ErrorMessage { get; set; }


        public void AddExpense()
        {
            NavigationManager.NavigateTo("expense");
        }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                DataLoaded = false;
                ErrorMessage = string.Empty;

                var data = await appDBContext.Expenses
                                             .Include(a => a.account)
                                              .Include(a => a.checkDetails)
                                             .ToListAsync();

                Expenses = data.Select(a => new ExpenseViewModel()
                {
                    id = a.id.ToString(),
                    glAccountCode = a.account.accountCode,
                    glAccountName = a.account.accountDesc,
                    amount = a.cashOrCheck.Equals("0") ? a.amount : a.checkDetails.amount,
                    remarks = a.remarks,
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
