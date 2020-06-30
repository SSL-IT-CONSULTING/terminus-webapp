using Blazored.SessionStorage;
using BlazorInputFile;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
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

        [Inject]
        protected IWebHostEnvironment env { get; set; }

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
                        attachmentRefKey = employee.attachmentRefKey,
                    CompanyId = CompanyId,
                        createDate = DateTime.Now,
                        createdBy = UserName
                    };

                    foreach (var item in employee.attachments)
                    {
                        Dapper.DynamicParameters dynamicParameters = new Dapper.DynamicParameters();
                        if (!item.isDeleted)
                        {

                            dynamicParameters.Add("id", item.id);
                            dynamicParameters.Add("displayName", item.displayName);
                            dynamicParameters.Add("fileName", item.fileName);
                            dynamicParameters.Add("documentType", item.documentType);
                            dynamicParameters.Add("refKey", employee.attachmentRefKey);

                            await dapperManager.ExecuteAsync("spInsertAttachment", dynamicParameters);
                        }
                        else
                        {
                            dynamicParameters.Add("id", item.id);
                            await dapperManager.ExecuteAsync("spDeleteAttachment", dynamicParameters);
                        }
                    }

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
                    entity.attachmentRefKey = employee.attachmentRefKey;

                    foreach(var item in employee.attachments)
                    {
                        Dapper.DynamicParameters dynamicParameters = new Dapper.DynamicParameters();
                        if (!item.isDeleted)
                        {

                            dynamicParameters.Add("id", item.id);
                            dynamicParameters.Add("displayName", item.displayName);
                            dynamicParameters.Add("fileName", item.fileName);
                            dynamicParameters.Add("documentType", item.documentType);
                            dynamicParameters.Add("refKey", employee.attachmentRefKey);

                            await dapperManager.ExecuteAsync("spInsertAttachment", dynamicParameters);
                        }
                        else
                        {
                            dynamicParameters.Add("id", item.id);
                            await dapperManager.ExecuteAsync("spDeleteAttachment", dynamicParameters);
                        }
                    }

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

        protected void HandleDeleteAttachment(string id)
        {
            var item = employee.attachments.FirstOrDefault(a => a.id.Equals(id));
            if (item != null)
                item.isDeleted = true;
            //StateHasChanged();
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
                    employee.attachmentRefKey = Guid.NewGuid().ToString();

                    employee.Active = true;
                    employee.HireDate = DateTime.Today;
                    employee.attachments = new List<AttachmentViewModel>();
                }
                else
                {
                    var data = await appDBContext.Employees
                                            .Where(a => a.EmployeeId.Equals(employeeId))
                                            .OrderBy(a => a.LastName).ThenBy(a => a.FirstName).ThenBy(a => a.MiddleName)
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
                        Remarks = a.Remarks,
                        attachmentRefKey = string.IsNullOrEmpty(a.attachmentRefKey)?Guid.NewGuid().ToString():a.attachmentRefKey
                    }).FirstOrDefault();


                    var attachments = await appDBContext.Attachments
                                            .Where(a => a.refKey.Equals(employee.attachmentRefKey))
                                            .ToListAsync();

                    employee.attachments = attachments.Select(a => new AttachmentViewModel() { id=a.id, displayName=a.displayName, documentType=a.documentType, fileName=a.fileName, url = $"Uploaded/Attachments/{a.fileName}" }).ToList();

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

        public async Task  HandleFileSelected(IFileListEntry[] files)
        {
            var uploadPath = Path.Combine(env.WebRootPath,"Uploaded", "Attachments");
            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            foreach(var file in files)
            {
                var ext = string.Empty;
                if(file.Name.LastIndexOf(".")>0)
                {
                    ext = file.Name.Substring(file.Name.LastIndexOf("."));
                }

                var fileName = $"{Guid.NewGuid().ToString()}.{ext.TrimStart('.')}";

                using (var fs = new FileStream(Path.Combine(uploadPath, fileName), FileMode.Create))
                {
                    await file.Data.CopyToAsync(fs);

                    this.employee.attachments.Add(new AttachmentViewModel() { displayName = file.Name, fileName = fileName, id = Guid.NewGuid().ToString(), documentType = "EmployeeFile", url = $"Uploaded/Attachments/{fileName}" });

                }
            }
        }

        protected void NavigateToList()
        {
            NavigationManager.NavigateTo("/employeelist");
        }

    }
}
