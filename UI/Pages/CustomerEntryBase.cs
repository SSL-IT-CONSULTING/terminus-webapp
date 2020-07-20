using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using terminus.dataaccess;
using terminus.shared.models;


namespace terminus_webapp.Pages
{
    public class CustomerEntryBase : ComponentBase
    {


        [Inject]
        public AppDBContext appDBContext { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public ISessionStorageService _sessionStorageService { get; set; }

        public DapperManager dapperManager { get; set; }

        public CustomerViewModel customers { get; set; }

        [Parameter]
        public string Id { get; set; }

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
            NavigationManager.NavigateTo("customerentry");
        }

        public void NavigateToList()
        {
            NavigationManager.NavigateTo("customerlist");
        }

        protected async Task HandleValidSubmit()
        {

            UserName = await _sessionStorageService.GetItemAsync<string>("UserName");
            CompanyId = await _sessionStorageService.GetItemAsync<string>("CompanyId");

            var vn = await appDBContext.Customers
                                        .Where(a => a.companyId.Equals(CompanyId))
                                        .ToListAsync();


            bool _vatregister;
            string accnt = string.Empty;

            if (customers.vatRegistered == "Y")
            {
                _vatregister = false;

                accnt = (string)null;
            }
            else
            {
                _vatregister = true;
                accnt = "52BDDE45-382B-471E-8D08-5CA2613FD6FA";
            }

            Guid? new_accnt;
            if (accnt == null)
            {


                new_accnt = null;
            }
            else

            {
                new_accnt = Guid.Parse(accnt);
            }




            if (string.IsNullOrEmpty(Id))
            {



                Customer c = new Customer()
                {
                  
                    id = Guid.NewGuid().ToString(),
                    companyId = CompanyId,
                    lastName = customers.lastName,
                    firstName = customers.firstName,
                    description = customers.description,
                    vatRegistered = _vatregister,
                    address = customers.address,
                    contactNo1 = customers.contactNo1,
                    contactNo2 = customers.contactNo2,
                    contactNo3 = customers.contactNo3
                };


                appDBContext.Customers.Add(c);

                await appDBContext.SaveChangesAsync();


            }
            else
            {


                var data = await appDBContext.Customers                        
                        .Where(r => r.id.Equals(Id) && r.companyId.Equals(CompanyId)).FirstOrDefaultAsync();

                data.lastName = customers.lastName;
                data.firstName = customers.firstName;
                data.description = customers.description;
                data.vatRegistered = _vatregister;
                data.address = customers.address;
                data.contactNo1 = customers.contactNo1;
                data.contactNo2 = customers.contactNo2;
                data.contactNo3 = customers.contactNo3;

                appDBContext.Customers.Update(data);
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


                if (string.IsNullOrEmpty(Id))
                {


                    customers = new CustomerViewModel();

                }
                else
                {
                    IsViewOnly = false;
                    IsEditOnly = true;

                    var data = await appDBContext.Customers
                        .Where(r => r.id.Equals(Id) && r.companyId.Equals(CompanyId)).FirstOrDefaultAsync();

                    string strIVR;
                    if (data.vatRegistered == true)
                    {

                        strIVR = "Y";
                    }
                    else
                    {
                        strIVR = "N";
                    }


                    customers = new CustomerViewModel()
                    {
                        id = data.id.ToString(),
                        companyId = CompanyId,
                        lastName = data.lastName,
                        firstName = data.firstName,
                        description = data.description,
                        vatRegistered = strIVR,
                        address = data.address,
                        contactNo1 = data.contactNo1,
                        contactNo2 = data.contactNo2,
                        contactNo3 = data.contactNo3

                    };
                }

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
