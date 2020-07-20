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
    public class ExpenseListBase:ComponentBase
    {
        [Inject]
        public AppDBContext appDBContext { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public ISessionStorageService _sessionStorageService { get; set; }

        public List<ExpenseViewModel> Expenses { get; set; }
        public bool DataLoaded { get; set; }
        public string ErrorMessage { get; set; }

        public string CompanyId { get; set; }
        public string UserName { get; set; }

        protected string FormatVendor(string _vendorId, string _vendorOther, string _vendorName)
        {
            return _vendorId.StartsWith("OTHER") ? _vendorOther : _vendorName;
        }

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
                UserName = await _sessionStorageService.GetItemAsync<string>("UserName");
                CompanyId = await _sessionStorageService.GetItemAsync<string>("CompanyId");

                var data = await appDBContext.Expenses
                                             .Include(a => a.account)
                                             .Include(a => a.checkDetails)
                                             .Include(a=>a.vendor)
                                             .Where(a=>a.companyId.Equals(CompanyId))
                                             .OrderByDescending(a=>a.createDate)
                                             .ToListAsync();

                Expenses = data.Select(a => new ExpenseViewModel()
                {
                    id = a.id.ToString(),
                    documentId = a.documentId,
                    dueDate = DateTime.Today,
                    glAccountCode = a.account.accountCode,
                    glAccountName = a.account.accountDesc,
                    amount = a.cashOrCheck.Equals("0") ? a.amount : a.checkDetails.amount,
                    remarks = a.remarks,
                    transactionDate = a.transactionDate,
                    vendorId = a.vendorId,
                    vendorName = a.vendor==null?string.Empty: a.vendor.vendorName,
                    vendorOther = a.vendor == null ? string.Empty : a.vendorOther

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
