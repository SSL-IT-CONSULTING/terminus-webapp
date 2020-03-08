﻿using Blazored.SessionStorage;
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
    public class TenantEntryBase : ComponentBase
    {

        [Inject]
        public IWebHostEnvironment _env { get; set; }

        [Inject]
        public AppDBContext appDBContext { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public ISessionStorageService _sessionStorageService { get; set; }

        public DapperManager dapperManager { get; set; }

        public TenantViewModel tenants { get; set; }


        public TenantDocument tenantDouments { get; set; }

        public string fileid { get; set; }
        public string progressinfoid { get; set; }
        public string progressbarid { get; set; }
        public string filter { get; set; } 
        public bool multiple { get; set; }

        

        [Parameter]
        public string Name { get; set; } // optional - can be used for managing multiple file upload controls on a page

        [Parameter]
        public string Filter { get; set; } // optional - for restricting types of files that can be selected

        [Parameter]
        public string Multiple { get; set; } // optional - enable multiple file uploads

        [Parameter]
        public string id { get; set; }
        [Parameter]
        public string tenandId { get; set; }
        public bool DataLoaded { get; set; }
        public string ErrorMessage { get; set; }


        public string errorMessagePropertyDateRange { get; set; }

        public string CompanyId { get; set; }
        public string UserName { get; set; }


        public bool IsViewOnly { get; set; }

        public bool IsEditOnly { get; set; }

        public bool IsDataLoaded { get; set; }

        public void AddGLAccount()
        {
            NavigationManager.NavigateTo("tenantentry");
        }

        public void NavigateToList()
        {
            NavigationManager.NavigateTo("tenantlist");
        }



        public int numLines;
        public IFileListEntry[] selectedFiles;

        public void HandleSelection(IFileListEntry[] files)
        {
            selectedFiles = files;
        }

        //public async Task LoadFile(IFileListEntry[] files)
        //{
        //    // So the UI updates to show progress
        //    //file.OnDataRead += (sender, eventArgs) => InvokeAsync(StateHasChanged);

        //    // Just load into .NET memory to show it can be done
        //    // Alternatively it could be saved to disk, or parsed in memory, or similar

        //    DateTime datetoday = DateTime.Now;

        //    foreach (var file in files)
        //    {


        //        var tmpPath = Path.Combine(_env.WebRootPath, "TenantDocument");

        //        if (!Directory.Exists(tmpPath))
        //        {
        //            Directory.CreateDirectory(tmpPath);
        //        }

        //        string fileId = Guid.NewGuid().ToString();

        //        var prefix = $"{DateTime.Today.ToString("yyyyMMdd")}{Guid.NewGuid().ToString()}";


        //        var _filename = file.Name.Split(".");

        //        string outputfile = _filename[0].ToString();
        //        string extname = "." + _filename[1].ToString();

        //        var filedestination = tmpPath + "\\" + prefix + extname;



        //        var td = new TenantDocument();

        //        td.createDate = datetoday;
        //        td.createdBy = UserName;
        //        td.id = Guid.Parse(fileId);
        //        td.fileName = _filename.ToString();
        //        td.filePath = filedestination.ToString();
        //        td.fileDesc = file.Name;
        //        td.extName = extname;


        //        using (FileStream DestinationStream = File.Create(filedestination))
        //        {
        //            await file.Data.CopyToAsync(DestinationStream);
        //        }
        //    }


        //    //var ms = new MemoryStream();
            
        //}

        protected async Task HandleValidSubmit()
        {


            UserName = await _sessionStorageService.GetItemAsync<string>("UserName");
            CompanyId = await _sessionStorageService.GetItemAsync<string>("CompanyId");

            DateTime datetoday = DateTime.Now;


            string propertyid = tenants.propertyid.ToString();


            errorMessagePropertyDateRange = "";


            var company = await appDBContext.Companies.Where(a => a.companyId.Equals(CompanyId)).FirstOrDefaultAsync();
            var property = await appDBContext.Properties
                                .Where(a => a.id.Equals(propertyid) && a.companyId.Equals(CompanyId)).FirstOrDefaultAsync();

            


            decimal areasq = property.areaInSqm;
            decimal asssq = tenants.ratePerSQM;

            if (areasq==null)
            {
                areasq = 0;
            }

            if (asssq == null)
                {
                asssq = 0;
            }

            decimal _monthlyrate = areasq * asssq;
            decimal _assorate = property.areaInSqm * tenants.ratePerSQMAssocDues;

            bool withWT_sw;


            if (tenants.withWT == "N")
            {
                withWT_sw = false;
            }
            else
            {
                withWT_sw = true;
            }




            var pdpropertyid = await appDBContext.PropertyDirectory.Where(a => a.propertyId.Equals(Guid.Parse(tenants.propertyid)) && a.companyId.Equals(CompanyId) && a.dateFrom <= tenants.dateTo && a.dateTo >= tenants.dateFrom).ToListAsync();

            if (pdpropertyid.Count > 0)
            {
                errorMessagePropertyDateRange = "Invalid Date Range. The property is with other tenant.";
                return;
            }

            if (string.IsNullOrEmpty(id))
            {

                var tenantId = Guid.NewGuid();

                var properDirectoryId = Guid.NewGuid().ToString();

                Tenant t = new Tenant()
                {

                    id = tenantId.ToString(),
                    company = company,
                    updateDate = datetoday,
                    updatedBy = UserName,
                    lastName = tenants.lastName,
                    firstName = tenants.firstName,
                    middleName = tenants.middleName,
                    contactNumber = tenants.contactNumber,
                    emailAddress = tenants.emailAddress,

                };

                appDBContext.Tenants.Add(t);
                await appDBContext.SaveChangesAsync();

                //---------------------------------------------
                
                var pd = new PropertyDirectory();

                //id = properDirectoryId;
                pd.id = Guid.Parse(properDirectoryId.ToString());
                pd.createDate = datetoday;
                pd.createdBy = UserName;
                pd.propertyId = tenants.propertyid;
                //pd.property = tenants.properties.Where(a => a.id.Equals(Guid.Parse(tenants.propertyid))).FirstOrDefault();
                pd.dateFrom = tenants.dateFrom;
                pd.dateTo = tenants.dateTo;
                pd.companyId = CompanyId;
                pd.monthlyRate = tenants.monthlyRate;
                pd.tenandId = tenantId.ToString();

                pd.associationDues = _assorate;
                pd.penaltyPct = tenants.penaltyPct;
                pd.ratePerSQM = tenants.ratePerSQM;
                pd.totalBalance = tenants.totalBalance;
                pd.withWT = withWT_sw;
                pd.ratePerSQMAssocDues = tenants.ratePerSQMAssocDues;

                appDBContext.PropertyDirectory.Add(pd);
                await appDBContext.SaveChangesAsync();



                foreach (var file in selectedFiles)
                {


                    var tmpPath = Path.Combine(_env.WebRootPath, "TenantDocument");

                    if (!Directory.Exists(tmpPath))
                    {
                        Directory.CreateDirectory(tmpPath);
                    }

                    string fileId = Guid.NewGuid().ToString();

                    var prefix = $"{DateTime.Today.ToString("yyyyMMdd")}{Guid.NewGuid().ToString()}";


                    var _filename = file.Name.Split(".");

                    string outputfile = _filename[0].ToString();
                    string extname = "." + _filename[1].ToString();

                    var filedestination = tmpPath + "\\" + prefix + extname;



                    var td = new TenantDocument();

                    td.createDate = datetoday;
                    td.createdBy = UserName;
                    td.propertyDirectoryId = Guid.Parse(properDirectoryId);
                    td.id = Guid.Parse(fileId);
                    td.fileName = prefix.ToString();
                    td.filePath = filedestination.ToString();
                    td.fileDesc = file.Name;
                    td.extName = extname;

                    appDBContext.TenantDocuments.Add(td);
                    await appDBContext.SaveChangesAsync();

                    using (FileStream DestinationStream = File.Create(filedestination))
                    {
                        await file.Data.CopyToAsync(DestinationStream);
                    }
                }




            }
            else
            {




                var t = await appDBContext.Tenants
                                                //.Select(a => new { id = a.id, company = a.company, lastName = a.lastName, firstName = a.firstName, middleName = a.middleName, contactNumber = a.contactNumber, emailAddress = a.emailAddress })
                                                .Include(a => a.company)
                                                .Where(r => r.id.Equals(tenandId) && r.company.companyId.Equals(CompanyId)).FirstOrDefaultAsync();



                t.lastName = tenants.lastName;
                t.firstName = tenants.firstName;
                t.middleName = tenants.middleName;
                t.contactNumber = tenants.contactNumber;
                t.emailAddress = tenants.emailAddress;


                appDBContext.Tenants.Update(t);
                await appDBContext.SaveChangesAsync();



                var pd = await appDBContext.PropertyDirectory
                                                //.Select(a => new { id = a.id, company = a.company, lastName = a.lastName, firstName = a.firstName, middleName = a.middleName, contactNumber = a.contactNumber, emailAddress = a.emailAddress })
                                                .Include(a => a.company)
                                                .Include(a => a.property)
                                                .Include(a => a.tenant)
                                                .Where(r => r.id.Equals(Guid.Parse(id)) && r.companyId.Equals(CompanyId)).FirstOrDefaultAsync();


                //id = properDirectoryId;
                
                pd.updateDate = datetoday;
                pd.updatedBy = UserName;

                pd.dateFrom = tenants.dateFrom;
                pd.dateTo = tenants.dateTo;
                pd.monthlyRate = tenants.monthlyRate;
                    
                pd.associationDues = _assorate;
                pd.penaltyPct = tenants.penaltyPct;
                pd.ratePerSQM = tenants.ratePerSQM;
                pd.totalBalance = tenants.totalBalance;
                pd.withWT = withWT_sw;
                pd.ratePerSQMAssocDues = tenants.ratePerSQMAssocDues;




                foreach (var file in selectedFiles)
                {


                    var tmpPath = Path.Combine(_env.WebRootPath, "Uploaded/TenantDocument");

                    if (!Directory.Exists(tmpPath))
                    {
                        Directory.CreateDirectory(tmpPath);
                    }

                    string fileId = Guid.NewGuid().ToString();

                    var prefix = $"{DateTime.Today.ToString("yyyyMMdd")}{Guid.NewGuid().ToString()}";


                    var _filename = file.Name.Split(".");

                    string outputfile = _filename[0].ToString();
                    string extname = "." + _filename[1].ToString();

                    var filedestination = tmpPath + "\\" + prefix + extname;



                    var td = new TenantDocument();

                    td.createDate = datetoday;
                    td.createdBy = UserName;
                    td.propertyDirectoryId = Guid.Parse(id);
                    td.id = Guid.Parse(fileId);
                    td.fileName = prefix.ToString();
                    td.filePath = filedestination.ToString();
                    td.fileDesc = file.Name;
                    td.extName = extname;

                    appDBContext.TenantDocuments.Add(td);
                    await appDBContext.SaveChangesAsync();

                    using (FileStream DestinationStream = File.Create(filedestination))
                    {
                        await file.Data.CopyToAsync(DestinationStream);
                    }
                }

                appDBContext.PropertyDirectory.Update(pd);
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

                if (string.IsNullOrEmpty(id))
                {


                    tenants = new TenantViewModel();
                    // glAccount.createDate = DateTime.Today;
                    tenants.id = Guid.NewGuid().ToString();
                    //tenants.propertyDictory.id = Guid.NewGuid();
                    tenants.dateFrom = DateTime.Now;
                    tenants.dateTo = DateTime.Now;

                    
                }
                else
                {



                    //pd.id = Guid.Parse(properDirectoryId.ToString());
                    //pd.createDate = datetoday;
                    //pd.createdBy = UserName;
                    //pd.propertyId = tenants.propertyid;
                    ////pd.property = tenants.properties.Where(a => a.id.Equals(Guid.Parse(tenants.propertyid))).FirstOrDefault();
                    //pd.dateFrom = tenants.dateFrom;
                    //pd.dateTo = tenants.dateTo;
                    //pd.companyId = CompanyId;
                    //pd.monthlyRate = tenants.monthlyRate;
                    //pd.tenandId = tenantId.ToString();

                    //pd.associationDues = tenants.associationDues;
                    //pd.penaltyPct = tenants.penaltyPct;
                    //pd.ratePerSQM = tenants.ratePerSQM;
                    //pd.totalBalance = tenants.totalBalance;

                    IsViewOnly = false;
                    IsEditOnly = true;

                    //var id = Guid.Parse(Id);
                    //var tenatnid = Guid.Parse(id);
                    var data = await appDBContext.PropertyDirectory
                                                    .Include(a => a.company)
                                                    .Include(a => a.property)
                                                    .Include(a => a.tenant)
                                                    .Where(r => r.id.Equals(Guid.Parse(id)) && r.companyId.Equals(CompanyId) ).FirstOrDefaultAsync();
                    //var data = await appDBContext.Tenants.Where(a => a.id.Equals(Guid.Parse(id))).ToListAsync();

                    tenants = new TenantViewModel()
                    {
                        id = data.id.ToString(),
                        companyid = data.company.companyId,
                        company = data.company,
                        
                        lastName = data.tenant.lastName,
                        firstName = data.tenant.firstName,
                        middleName = data.tenant.middleName,
                        contactNumber = data.tenant.contactNumber,
                        emailAddress = data.tenant.emailAddress,
                        propertyid = data.propertyId,
                        dateFrom = data.dateFrom,
                        dateTo = data.dateTo,
                        monthlyRate = data.monthlyRate,
                        associationDues = data.associationDues,
                        penaltyPct = data.penaltyPct,
                        ratePerSQM = data.ratePerSQM,
                        totalBalance = data.totalBalance,
                        withWT = data.withWT?"Yes":"No",
                        ratePerSQMAssocDues = data.ratePerSQMAssocDues
                };


                    tenants.tenantDocument = await appDBContext.TenantDocuments
                                                                        .Where(r => r.propertyDirectoryId.Equals(Guid.Parse(id)))
                                                                        .ToListAsync();
                    
                }
                tenants.properties = await appDBContext.Properties
                                                            .Where(r => r.companyId.Equals(CompanyId))
                                                            .ToListAsync();





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
