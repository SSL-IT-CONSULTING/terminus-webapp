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
    public class PropertyEntryBase : ComponentBase
    {


        [Inject]
        public AppDBContext appDBContext { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public ISessionStorageService _sessionStorageService { get; set; }

        public DapperManager dapperManager { get; set; }

        public Property properties { get; set; }

        [Parameter]
        public string id { get; set; }

        public bool DataLoaded { get; set; }
        public string ErrorMessage { get; set; }

        public string CompanyId { get; set; }
        public string UserName { get; set; }


        public bool IsViewOnly { get; set; }

        public bool IsEditOnly { get; set; }
        
        public bool IsDataLoaded { get; set; }

        public void AddGLAccount()
        {
            NavigationManager.NavigateTo("propertyentry");
        }

        public void NavigateToList()
        {
            NavigationManager.NavigateTo("propertylist");
        }

        protected async Task HandleValidSubmit()
        {

            UserName = await _sessionStorageService.GetItemAsync<string>("UserName");
            CompanyId = await _sessionStorageService.GetItemAsync<string>("CompanyId");

            if (string.IsNullOrEmpty(properties.id))
            {

                Property pr = new Property()
                {


                    id = Guid.NewGuid().ToString(),
                    createDate = DateTime.Now,
                    createdBy = UserName,
                    description = properties.description,
                    address = properties.address,
                    propertyType = properties.propertyType,
                    area = properties.area

                };


                appDBContext.Properties.Add(pr);

                await appDBContext.SaveChangesAsync();


            }
            else
            {
                var data = await appDBContext.Properties
                          //.Select(a => new { id = a.id, company = a.company, lastName = a.lastName, firstName = a.firstName, middleName = a.middleName, contactNumber = a.contactNumber, emailAddress = a.emailAddress })
                          .Include(a => a.company)
                          .Where(r => r.id.Equals(id)).FirstOrDefaultAsync();


                data.updateDate = DateTime.Now;
                data.updatedBy = UserName;
                data.description = properties.description;
                data.address = properties.address;
                data.propertyType = properties.propertyType;
                data.area = properties.area;


                appDBContext.Properties.Update(data);
                await appDBContext.SaveChangesAsync();

            }

            StateHasChanged();

            NavigateToList();
        }

        protected override async Task OnInitializedAsync()
        {
            try
            {

                IsDataLoaded = false;
                IsViewOnly = false;
                IsEditOnly = false;
                ErrorMessage = string.Empty;

                UserName = await _sessionStorageService.GetItemAsync<string>("UserName");
                CompanyId = await _sessionStorageService.GetItemAsync<string>("CompanyId");



                if (string.IsNullOrEmpty(id))
                {
                    IsEditOnly = false;
                    properties = new Property();
                    // glAccount.createDate = DateTime.Today;
                    
                    //properties.id = Guid.Parse(Id).ToString();


                }
                else
                {
                    IsViewOnly = false;
                    IsEditOnly = true;

                    var data = await appDBContext.Properties.Where(a => a.id.Equals(id)).FirstOrDefaultAsync();

                    properties = new Property()
                    {
                        id = data.id,
                        description = data.description,
                        address = data.address,
                        propertyType = data.propertyType,
                        area = data.area

                    };
                }
                //glAccount = await appDBContext.GLAccountsVM.ToList();

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
