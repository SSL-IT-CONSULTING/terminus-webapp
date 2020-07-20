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
    public class PropertyDirectoryBase : ComponentBase
    {


        [Inject]
        public AppDBContext appDBContext { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public ISessionStorageService _sessionStorageService { get; set; }

        public DapperManager dapperManager { get; set; }

        public PropertyDirectoryViewModal propertyDirectory { get; set; }

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
            NavigationManager.NavigateTo("propertydirectorylist");
        }

        public void NavigateToList()
        {
            NavigationManager.NavigateTo("propertydirectorylist");
        }

        protected async Task HandleValidSubmit()
        {

            UserName = await _sessionStorageService.GetItemAsync<string>("UserName");
            CompanyId = await _sessionStorageService.GetItemAsync<string>("CompanyId");
            var pd1 = await appDBContext.PropertyDirectory.ToListAsync();

            

            if (string.IsNullOrEmpty(id))
            {

                //var newid = Guid.NewGuid();
                //string revenueid = "52BDDE45-382B-471E-8D08-5CA2613FD6FA";
                //PropertyDirectory pd = new PropertyDirectory()
                //{

                //    id = Guid.NewGuid(),
                //    createDate = DateTime.Now,
                //    createdBy = UserName,
                //    propertyId = propertyDirectory.propertyId,
                //    dateFrom = propertyDirectory.dateFrom,
                //    dateTo = propertyDirectory.dateFrom,
                //    companyId = CompanyId,
                //    monthlyRate = propertyDirectory.monthlyRate,
                //    tenandId = propertyDirectory.tenandId,
                //    associationDues = propertyDirectory.associationDues,
                //    penaltyPct = propertyDirectory.penaltyPct,
                //    ratePerSQM = propertyDirectory.ratePerSQM,
                //    totalBalance = propertyDirectory.totalBalance,
                //    revenueAccountId = Guid.Parse(revenueid)
                //};


                //appDBContext.PropertyDirectory.Add(pd);

                //await appDBContext.SaveChangesAsync();


            }
            else
            {
                var data = await appDBContext.PropertyDirectory
                          //.Select(a => new { id = a.id, company = a.company, lastName = a.lastName, firstName = a.firstName, middleName = a.middleName, contactNumber = a.contactNumber, emailAddress = a.emailAddress })
                          .Include(a => a.company)
                          .Where(r => r.id.Equals(id)).FirstOrDefaultAsync();


                data.id = Guid.Parse(propertyDirectory.id.ToString());
                data.updateDate = DateTime.Now;
                data.updatedBy = UserName;
                data.propertyId = propertyDirectory.propertyId;
                data.dateFrom = propertyDirectory.dateFrom;
                data.dateTo = propertyDirectory.dateFrom;
                data.companyId = CompanyId;
                data.monthlyRate = propertyDirectory.monthlyRate;
                data.tenandId = propertyDirectory.tenandId;
                data.associationDues = propertyDirectory.associationDues;
                data.penaltyPct = propertyDirectory.penaltyPct;
                data.ratePerSQM = propertyDirectory.ratePerSQM;
                data.totalBalance = propertyDirectory.totalBalance;


                appDBContext.PropertyDirectory.Update(data);
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
                ErrorMessage = string.Empty;
                IsEditOnly = false;

                if (string.IsNullOrEmpty(id))
                {



                    propertyDirectory = new PropertyDirectoryViewModal();
                    propertyDirectory.dateFrom = DateTime.Today;
                    propertyDirectory.dateTo = DateTime.Today;
                }


                else
                {
                    IsViewOnly = false;
                    IsEditOnly = true;

                    var data = await appDBContext.PropertyDirectory.Where(a => a.id.Equals(Guid.Parse(id))).FirstOrDefaultAsync();

                    propertyDirectory = new PropertyDirectoryViewModal()
                    {
                        id =  Guid.Parse(data.id.ToString()),
                        propertyId = data.propertyId,
                        //property = data.property,
                        dateFrom = data.dateFrom,
                        dateTo = data.dateTo,
                        monthlyRate = data.monthlyRate,
                        revenueAccountId = data.revenueAccountId,
                        status = data.status,
                        tenandId = data.tenandId,
                        //tenant = data.tenant,
                        associationDues = data.associationDues,
                        penaltyPct = data.penaltyPct,
                        ratePerSQM = data.ratePerSQM,
                        totalBalance = data.totalBalance
                    };
                }

                //expense.expenseAccounts = await appDBContext.GLAccounts.Where(a => a.expense || a.cashAccount).ToListAsync();
                //propertyDirectory.property = await appDBContext.Properties.ToList();
                //.Include(a => a.inputVatAccount)
                //.OrderBy(a => a.createDate).ToListAsync();

                propertyDirectory.property = await appDBContext.Properties.Where(a => a.companyId.Equals(CompanyId)).ToListAsync();
                propertyDirectory.tenant = await appDBContext.Tenants.ToListAsync();

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
