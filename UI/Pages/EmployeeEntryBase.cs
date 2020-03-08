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
    public class EmployeeEntryBase:ComponentBase
    {
        [Inject]
        public AppDBContext appDBContext { get; set; }

        [Inject]
        public DapperManager dapperManager { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public ISessionStorageService _sessionStorageService { get; set; }

        public bool IsDataLoaded { get; set; }

        public string ErrorMessage { get; set; }

        public EmployeeViewModel employee { get; set; }

        public string CompanyId { get; set; }
        public string UserName { get; set; }

        [Parameter]
        public string employeeId { get; set; }

        protected async Task HandleValidSubmit()
        {
            try
            {
                ErrorMessage = string.Empty;

                if(string.IsNullOrEmpty(employeeId))
                {
                    var entity = new Employee()
                    {
                        EmployeeId = Guid.NewGuid().ToString(),
                        Active = employee.Active,
                        HireDate = employee.HireDate,
                        LastName = employee.LastName,
                        FirstName = employee.FirstName,
                        MiddleName = employee.MiddleName,
                        Position = employee.Position,
                        Address = employee.Address,
                        ContactNo = employee.ContactNo,
                        Email = employee.Email,
                        SSS = employee.SSS,
                        PhilHealth = employee.PhilHealth,
                        PagIbig = employee.PagIbig,
                        TIN = employee.TIN,
                        BirthDate = employee.BirthDate,
                        Gender = employee.Gender,
                        EndDate = employee.EndDate,
                        Remarks = employee.Remarks,
                        CompanyId = CompanyId,
                        createDate = DateTime.Now,
                        createdBy = UserName
                    };

                    appDBContext.Employees.Add(entity);
                    await appDBContext.SaveChangesAsync();

                    NavigateToList();
                }
                else
                {
                    var entity = appDBContext.Employees.Where(a => a.EmployeeId.Equals(employeeId)).FirstOrDefault();

                    entity.Active = employee.Active;
                    entity.HireDate = employee.HireDate;
                    entity.LastName = employee.LastName;
                    entity.FirstName = employee.FirstName;
                    entity.MiddleName = employee.MiddleName;
                    entity.Position = employee.Position;
                    entity.Address = employee.Address;
                    entity.ContactNo = employee.ContactNo;
                    entity.Email = employee.Email;
                    entity.SSS = employee.SSS;
                    entity.PhilHealth = employee.PhilHealth;
                    entity.PagIbig = employee.PagIbig;
                    entity.TIN = employee.TIN;
                    entity.BirthDate = employee.BirthDate;
                    entity.Gender = employee.Gender;
                    entity.EndDate = employee.EndDate;
                    entity.Remarks = employee.Remarks;
                   
                    entity.updateDate = DateTime.Now;
                    entity.updatedBy = UserName;
                    
                    appDBContext.Employees.Update(entity);
                    await appDBContext.SaveChangesAsync();

                    NavigateToList();
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.ToString();
                
            }
        }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                UserName = await _sessionStorageService.GetItemAsync<string>("UserName");
                CompanyId = await _sessionStorageService.GetItemAsync<string>("CompanyId");

                IsDataLoaded = false;
                ErrorMessage = string.Empty;


                if(string.IsNullOrEmpty(employeeId))
                {
                    employee = new EmployeeViewModel();
                    employee.Active = true;
                    employee.HireDate = DateTime.Today;
                }
                else
                {
                    var data = await appDBContext.Employees
                                            .Where(a => a.EmployeeId.Equals(employeeId))
                                            .OrderByDescending(a => a.LastName).ThenBy(a => a.FirstName)
                                            .ToListAsync();

                    employee = data.Select(a => new EmployeeViewModel()
                    {
                        EmployeeId = a.EmployeeId,
                        Active = a.Active,
                        HireDate = a.HireDate,
                        LastName = a.LastName,
                        FirstName = a.FirstName,
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
                    }).FirstOrDefault();
                }

               

            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
            finally
            {
                IsDataLoaded = true;
            }
        }

        protected void NavigateToList()
        {
            NavigationManager.NavigateTo("/employeelist");
        }

    }
}
