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
    public class VendorListBase:ComponentBase
    {

        [Inject]
        public AppDBContext appDBContext { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public ISessionStorageService _sessionStorageService { get; set; }

        public DapperManager dapperManager { get; set; }

        public List<Vendor> vendors { get; set; }

        public List<GLAccount> glaccount { get; set; }

        public bool DataLoaded { get; set; }
        public string ErrorMessage { get; set; }

        public string CompanyId { get; set; }
        public string UserName { get; set; }

        public void VendorEntry()
        {
            NavigationManager.NavigateTo("vendorentry");
        }

        public void VendorEdit()
        {
            NavigationManager.NavigateTo("vendoredit");
        }

        public void PropertyList()
        {
            NavigationManager.NavigateTo("Vendorlist");

        }

        protected override async Task OnInitializedAsync()
        {
            try
            {

                DataLoaded = false;
                ErrorMessage = string.Empty;

                UserName = await _sessionStorageService.GetItemAsync<string>("UserName");
                CompanyId = await _sessionStorageService.GetItemAsync<string>("CompanyId");


                //var data = await appDBContext.Vendors
                //                             .OrderByDescending(a => a.createDate)
                //                             .Select(a => new { id = a.id, company = a.company, description = a.description, address = a.address })
                //                             .Include(a => a.companyId)
                //                             .Include(a => a.inputVatAccount)
                //                             .OrderBy(a => a.vendorName)
                //                             .ToListAsync();

                var data = await appDBContext.Vendors
                             //.OrderByDescending(a => a.createDate)
                             .Select(a => new { vendorId = a.vendorId, vendorName = a.vendorName, companyId = a.companyId, company = a.company, isVatRegistered = a.isVatRegistered, inputVatAccountId = a.inputVatAccountId, inputVatAccount = a.inputVatAccount, rowOrder = a.rowOrder })
                             .Where(a => a.companyId.Equals(CompanyId))
                             .OrderBy(a => a.vendorName)
                             .ToListAsync();




                var gla = await appDBContext.GLAccounts.Where(a => a.accountId.Equals(CompanyId)).FirstOrDefaultAsync();

                vendors = data.Select(a => new Vendor()
                {
                    vendorId = a.vendorId,
                    vendorName = a.vendorName,
                    companyId = a.companyId,
                    company = a.company,
                    isVatRegistered = a.isVatRegistered,
                    inputVatAccountId = a.inputVatAccountId,
                    inputVatAccount = a.inputVatAccount,
                    rowOrder = a.rowOrder
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
