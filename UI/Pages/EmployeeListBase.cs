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
    public class EmployeeListBase:ComponentBase
    {
        [Inject]
        public AppDBContext appDBContext { get; set; }


        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public ISessionStorageService _sessionStorageService { get; set; }

        public List<EmployeeViewModel> Employees { get; set; }

        public bool DataLoaded { get; set; }
        public string ErrorMessage { get; set; }

        public string CompanyId { get; set; }

        public string UserName { get; set; }

        public void AddEmployee()
        {
           
            NavigationManager.NavigateTo("employeeentry");
        }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                UserName = await _sessionStorageService.GetItemAsync<string>("UserName");
                CompanyId = await _sessionStorageService.GetItemAsync<string>("CompanyId");

                DataLoaded = false;
                ErrorMessage = string.Empty;

                var data = await appDBContext.Employees
                                             .Where(a => a.CompanyId.Equals(CompanyId))
                                             .OrderByDescending(a => a.LastName).ThenBy(a=>a.FirstName)
                                             .ToListAsync();


                Employees = data.Select(a => new EmployeeViewModel()
                {
                    EmployeeId = a.EmployeeId,
                    Active = a.Active,
                    HireDate = a.HireDate,
                    LastName = a.LastName,
                    FirstName=a.FirstName,
                    MiddleName = a.MiddleName,
                    Position = a.Position,
                    Address = a.Address,
                    ContactNo = a.ContactNo,
                    Email = a.Email,
                    SSS = a.SSS,
                    PhilHealth = a.PhilHealth,
                    PagIbig = a.PagIbig,
                    TIN = a.TIN,
                    BirthDate = a.BirthDate,
                    Gender = a.Gender,
                    EndDate = a.EndDate,
                    Remarks = a.Remarks
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
