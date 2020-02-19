using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using terminus.shared.models;
using terminus_webapp.Data;

namespace terminus_webapp.Pages
{
    public class VendorEntryBase:ComponentBase
    {


        [Inject]
        public AppDBContext appDBContext { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public ISessionStorageService _sessionStorageService { get; set; }

        public DapperManager dapperManager { get; set; }

        public VendorsViewModal vendors { get; set; }

        [Parameter]
        public string vendorId { get; set; }

        public bool DataLoaded { get; set; }
        public string ErrorMessage { get; set; }

        public string CompanyId { get; set; }
        public string UserName { get; set; }

        public Guid? InputVatAccountId { get; set; }
        public string InputVatAccountCode { get; set; }
        public string InputVatAccountName { get; set; }

        public bool IsViewOnly { get; set; }
        public bool IsEditOnly { get; set; }

         
        public bool IsDataLoaded { get; set; }

        public void AddGLAccount()
        {
            NavigationManager.NavigateTo("vendorentry");
        }

        public void NavigateToList()
        {
            NavigationManager.NavigateTo("vendorlist");
        }

        protected async Task HandleValidSubmit()
        {

            UserName = await _sessionStorageService.GetItemAsync<string>("UserName");
            CompanyId = await _sessionStorageService.GetItemAsync<string>("CompanyId");

            var vn = await appDBContext.Vendors.ToListAsync();

            int rowCount =  vn.Max(r => r.rowOrder);


            bool _vatregister;
            string accnt;

            if (vendors.isVatRegistered == "N")
            {
                _vatregister = false;

                accnt = "AEE3CBD3-4D95-4363-A971-D79B9DAA4109";
            }
            else
            {
                _vatregister = true;
                accnt = "52BDDE45-382B-471E-8D08-5CA2613FD6FA";
            }


            int maxRow = appDBContext.Vendors.Max(a => a.rowOrder);
            int _maxRow;
            if (maxRow == null)
            {
                _maxRow = 1;
            }
            else
            {
                _maxRow = maxRow + 1;
            }

            if (string.IsNullOrEmpty(vendors.vendorId))
            {

                var id = Guid.NewGuid();

                Vendor v = new Vendor()
                {
                    //var id = Guid.NewGuid(vendors.inputVatAccountid);

                    vendorId = id.ToString(),
                    companyId = vendors.companyId,
                    vendorName = vendors.vendorName,
                    rowOrder = _maxRow,
                    inputVatAccountId = Guid.Parse(accnt),
                    isVatRegistered = _vatregister

                };


                appDBContext.Vendors.Add(v);

                await appDBContext.SaveChangesAsync();


            }
            else
            {

                var data = await appDBContext.Vendors
                        //.Select(a => new { id = a.id, company = a.company, lastName = a.lastName, firstName = a.firstName, middleName = a.middleName, contactNumber = a.contactNumber, emailAddress = a.emailAddress })
                        .Include(a => a.company)
                        .Where(r => r.vendorId.Equals(vendorId)).FirstOrDefaultAsync();
                //int max_row = await appDBContext.Vendors
                //                                    .Select(a => new { companyId = a.companyId,rowOrder = a.rowOrder })
                //                                   .Where(a => a.companyId.Equals(CompanyId)).Max(a => a.rowOrder);


                var id = Guid.NewGuid();

                data.vendorId = id.ToString();
                data.vendorName = vendors.vendorName.ToString();
                //data.rowOrder = vendors.rowOrder;
               // data.inputVatAccountId = Guid.Parse(accnt);
                data.isVatRegistered = _vatregister;


                appDBContext.Vendors.Update(data);
                await appDBContext.SaveChangesAsync();
            }

            StateHasChanged();

            NavigateToList();
        }

        protected override async Task OnInitializedAsync()
        {
            try
            {



                UserName = await _sessionStorageService.GetItemAsync<string>("UserName");
                CompanyId = await _sessionStorageService.GetItemAsync<string>("CompanyId");

                IsDataLoaded = false;
                IsViewOnly = false;
                IsEditOnly = false;
                ErrorMessage = string.Empty;


                if (string.IsNullOrEmpty(vendorId))
                {

                   
                    vendors = new VendorsViewModal();
                    
                }
                else
                {
                    IsViewOnly = false;
                    IsEditOnly = true;

                    var data = await appDBContext.Vendors
                        .Include(a => a.company)
                        .Include(a => a.inputVatAccount)
                        //.ThenInclude(v => v.accountId)
                        .Where(r => r.vendorId.Equals(vendorId)).FirstOrDefaultAsync();

                    string strIVR;
                    if (data.isVatRegistered == true)
                    {

                        strIVR = "Y";
                    }
                    else
                    {
                        strIVR = "N";
                    }

                    if (data.inputVatAccountId.HasValue)
                    {
                        InputVatAccountId = data.inputVatAccount.accountId;
                        InputVatAccountCode = data.inputVatAccount.accountCode;
                        InputVatAccountName = data.inputVatAccount.accountDesc;
                    }

                    vendors = new VendorsViewModal()
                    {
                        vendorId = data.vendorId.ToString(),
                        vendorName = data.vendorName,
                        companyId  = CompanyId,
                        //rowOrder = data.rowOrder,
                        //inputVatAccountid = data.inputVatAccountId.ToString(),
                        
                        isVatRegistered = strIVR

                    };
                }

                vendors.inputVatAccount = await appDBContext.GLAccounts.Where(a => a.outputVatAccount).ToListAsync();

            }
            catch (Exception ex)
            {
                ErrorMessage = ex.ToString();
            }
            finally
            {
                IsDataLoaded = true;
            }

        }


    }
}
