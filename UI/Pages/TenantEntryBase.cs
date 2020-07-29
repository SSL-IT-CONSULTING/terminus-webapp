using Blazored.SessionStorage;
using BlazorInputFile;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using terminus.dataaccess;
using terminus.shared.models;
using System.Collections.Generic;


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

        public void NavigateFiles()
        {

        }

        public void NavigateToList()
        {
            NavigationManager.NavigateTo("tenantlist");
        }





        public int numLines;
        public IFileListEntry[] selectedFiles;

        public async Task HandleSelection(IFileListEntry[] files)
        { 
            if (this.tenants.tenantDocument == null)
                this.tenants.tenantDocument = new List<TenantDocument>();

            var uploadPath = Path.Combine(_env.WebRootPath, "Uploaded", "Attachments");
            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            foreach (var file in files)
            {
                var ext = string.Empty;
                if (file.Name.LastIndexOf(".") > 0)
                {
                    ext = file.Name.Substring(file.Name.LastIndexOf("."));
                }

                var prefix = $"{DateTime.Today.ToString("yyyyMMdd")}{Guid.NewGuid().ToString()}";

                var fileName = $"{prefix}.{ext.TrimStart('.')}";

                using (var fs = new FileStream(Path.Combine(uploadPath, fileName), FileMode.Create))
                {
                    await file.Data.CopyToAsync(fs);
                    var td = new TenantDocument();

                    td.createDate = DateTime.Now;
                    td.createdBy = UserName;
                    td.id = Guid.NewGuid();
                    td.fileName = prefix.ToString();
                    td.filePath = Path.Combine(uploadPath,  $"{prefix}{ext}");
                    td.fileDesc = file.Name;
                    td.extName = ext;

                    tenants.tenantDocument.Add(td);     
                }               
            }
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

        protected void HandleDeleteAttachment(string id)
         {
            var item = tenants.tenantDocument.FirstOrDefault(a => a.id.Equals(Guid.Parse(id)));
            if (item != null)
            {
                item.deleted = true;
 
            }
            //StateHasChanged();
        }

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


            decimal new_monthly_rate = 0;
            if (CompanyId == "ADBCA")
            {
                new_monthly_rate = 0;
            }
            else
            {
                new_monthly_rate = tenants.monthlyRate;
            }



            if (tenants.withWT == "N")
            {
                withWT_sw = false;
            }
            else
            {
                withWT_sw = true;
            }




            var pdpropertyid = await appDBContext.PropertyDirectory.Where(a => a.propertyId.Equals(Guid.Parse(tenants.propertyid)) && a.companyId.Equals(CompanyId) && a.dateFrom <= tenants.dateTo && a.dateTo >= tenants.dateFrom && a.deleted.Equals(false)).ToListAsync();

         
            if (pdpropertyid.Count > 0)
            {
                errorMessagePropertyDateRange = "Invalid Date Range. The property is with other tenant.";
                return;
            }

            if (string.IsNullOrEmpty(id))
            {

                var tenantId = Guid.NewGuid();

                var properDirectoryId = Guid.NewGuid();

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


                    //Owned_Mgd = tenants.Owned_Mgd,
                    //MgtFeePct = tenants.MgtFeePct,
                    //CCTNumber = tenants.CCTNumber,


                    contactNo2 = tenants.contactNo2,
                    contactNo3 = tenants.contactNo3,
                    workAddress = tenants.workAddress,
                    emergyFullName = tenants.emergyFullName,
                    emergyContactNo = tenants.emergyContactNo,
                    emergyAdrress = tenants.emergyAdrress,
                    emergyRelationshipOwner = tenants.emergyRelationshipOwner,
                    //otherRestenanted1 = tenants.otherRestenanted1,
                    //otherResResiding1 = tenants.otherResResiding1,
                    //otherResFullName1 = tenants.otherResFullName1,
                    //otherResRelationshipToOwner1 = tenants.otherResRelationshipToOwner1,
                    //otherRestenanted2 = tenants.otherRestenanted2,
                    //otherResResiding2 = tenants.otherResResiding2,
                    //otherResFullName2 = tenants.otherResFullName2,
                    //otherResRelationshipToOwner2 = tenants.otherResRelationshipToOwner2,
                    //otherRestenanted3 = tenants.otherRestenanted3,
                    //otherResResiding3 = tenants.otherResResiding3,
                    //otherResFullName3 = tenants.otherResFullName3,
                    //otherResRelationshipToOwner3 = tenants.otherResRelationshipToOwner3,

                    subTenantFullName1 = tenants.subTenantFullName1,
                    subTenantID1 = tenants.subTenantID1,
                    subTenantHomeAddress1 = tenants.subTenantHomeAddress1,
                    subTenantWorkAddress1 = tenants.subTenantWorkAddress1,
                    subTenantContactNo1 = tenants.subTenantContactNo1,
                    subTenantEmailAdd1 = tenants.subTenantEmailAdd1,
                    RelToPrimary1 = tenants.RelToPrimary1,

                    subTenantFullName2 = tenants.subTenantFullName2,
                    subTenantID2 = tenants.subTenantID2,
                    subTenantHomeAddress2 = tenants.subTenantHomeAddress2,
                    subTenantWorkAddress2 = tenants.subTenantWorkAddress2,
                    subTenantContactNo2 = tenants.subTenantContactNo2,
                    subTenantEmailAdd2 = tenants.subTenantEmailAdd2,
                    RelToPrimary2 = tenants.RelToPrimary2,

                    subTenantFullName3 = tenants.subTenantFullName1,
                    subTenantID3 = tenants.subTenantID1,
                    subTenantHomeAddress3 = tenants.subTenantHomeAddress3,
                    subTenantWorkAddress3 = tenants.subTenantWorkAddress3,
                    subTenantContactNo3 = tenants.subTenantContactNo3,
                    subTenantEmailAdd3 = tenants.subTenantEmailAdd3,
                    RelToPrimary3 = tenants.RelToPrimary3
       

                };

                if(tenants.tenantDocument!=null && tenants.tenantDocument.Any())
                {
                    foreach (var td in tenants.tenantDocument)
                    {
                        appDBContext.TenantDocuments.Add(new TenantDocument() {
                        id=td.id,
                        propertyDirectoryId=properDirectoryId,
                        createDate=td.createDate,
                        createdBy=td.createdBy,
                        fileName = td.fileName,
                        fileDesc = td.fileDesc,
                        extName=td.extName,
                        filePath = td.filePath
                        });
                    }
                }

                appDBContext.Tenants.Add(t);
                //await appDBContext.SaveChangesAsync();

                //---------------------------------------------


                var pd = new PropertyDirectory();

                //id = properDirectoryId;
                pd.id = properDirectoryId;
                pd.createDate = datetoday;
                pd.createdBy = UserName;
                pd.propertyId = tenants.propertyid;
                //pd.property = tenants.properties.Where(a => a.id.Equals(Guid.Parse(tenants.propertyid))).FirstOrDefault();

                if (CompanyId == "ADBCA")
                {
                    pd.dateFrom = DateTime.Parse("01/01/1900");
                    pd.dateTo = DateTime.Parse("12/31/2099");
                }
                else
                {
                    pd.dateFrom = tenants.dateFrom;
                    pd.dateTo = tenants.dateTo;
                }

                pd.companyId = CompanyId;
                pd.monthlyRate = new_monthly_rate;
                pd.tenandId = tenantId.ToString();

                pd.associationDues = _assorate;
                pd.penaltyPct = tenants.penaltyPct;
                pd.ratePerSQM = tenants.ratePerSQM;
                pd.totalBalance = tenants.totalBalance;
                pd.withWT = withWT_sw;
                pd.ratePerSQMAssocDues = tenants.ratePerSQMAssocDues;

                appDBContext.PropertyDirectory.Add(pd);
                await appDBContext.SaveChangesAsync();


            }
            else
            {




                var t = await appDBContext.Tenants
                                                //.Select(a => new { id = a.id, company = a.company, lastName = a.lastName, firstName = a.firstName, middleName = a.middleName, contactNumber = a.contactNumber, emailAddress = a.emailAddress })
                                                .Include(a => a.company)
                                                .Where(r => r.id.Equals(tenandId) && r.company.companyId.Equals(CompanyId) && r.deleted.Equals(false)).FirstOrDefaultAsync();



                t.lastName = tenants.lastName;
                t.firstName = tenants.firstName;
                t.middleName = tenants.middleName;
                t.contactNumber = tenants.contactNumber;
                t.emailAddress = tenants.emailAddress;

                //t.Owned_Mgd = tenants.Owned_Mgd;
                //t.MgtFeePct = tenants.MgtFeePct;
                //t.CCTNumber = tenants.CCTNumber;


                t.contactNo2 = tenants.contactNo2;
                t.contactNo3 = tenants.contactNo3;
                t.workAddress = tenants.workAddress;
                t.emergyFullName = tenants.emergyFullName;
                t.emergyContactNo = tenants.emergyContactNo;
                t.emergyAdrress = tenants.emergyAdrress;
                t.emergyRelationshipOwner = tenants.emergyRelationshipOwner;
                //t.otherRestenanted1 = tenants.otherRestenanted1;
                //t.otherResResiding1 = tenants.otherResResiding1;
                //t.otherResFullName1 = tenants.otherResFullName1;
                //t.otherResRelationshipToOwner1 = tenants.otherResRelationshipToOwner1;
                //t.otherRestenanted2 = tenants.otherRestenanted2;
                //t.otherResResiding2 = tenants.otherResResiding2;
                //t.otherResFullName2 = tenants.otherResFullName2;
                //t.otherResRelationshipToOwner2 = tenants.otherResRelationshipToOwner2;
                //t.otherRestenanted3 = tenants.otherRestenanted3;
                //t.otherResResiding3 = tenants.otherResResiding3;
                //t.otherResFullName3 = tenants.otherResFullName3;
                //t.otherResRelationshipToOwner3 = tenants.otherResRelationshipToOwner3;

                t.subTenantFullName1 = tenants.subTenantFullName1;
                t.subTenantID1 = tenants.subTenantID1;
                t.subTenantHomeAddress1 = tenants.subTenantHomeAddress1;
                t.subTenantWorkAddress1 = tenants.subTenantWorkAddress1;
                t.subTenantContactNo1 = tenants.subTenantContactNo1;
                t.subTenantEmailAdd1 = tenants.subTenantEmailAdd1;
                t.RelToPrimary1 = tenants.RelToPrimary1;

                t.subTenantFullName2 = tenants.subTenantFullName2;
                t.subTenantID2 = tenants.subTenantID2;
                t.subTenantHomeAddress2 = tenants.subTenantHomeAddress2;
                t.subTenantWorkAddress2 = tenants.subTenantWorkAddress2;
                t.subTenantContactNo2 = tenants.subTenantContactNo2;
                t.subTenantEmailAdd2 = tenants.subTenantEmailAdd2;
                t.RelToPrimary2 = tenants.RelToPrimary2;

                t.subTenantFullName3 = tenants.subTenantFullName1;
                t.subTenantID3 = tenants.subTenantID1;
                t.subTenantHomeAddress3 = tenants.subTenantHomeAddress3;
                t.subTenantWorkAddress3 = tenants.subTenantWorkAddress3;
                t.subTenantContactNo3 = tenants.subTenantContactNo3;
                t.subTenantEmailAdd3 = tenants.subTenantEmailAdd3;
                t.RelToPrimary3 = tenants.RelToPrimary3;


                appDBContext.Tenants.Update(t);
                //await appDBContext.SaveChangesAsync();



                var pd = await appDBContext.PropertyDirectory
                                                //.Select(a => new { id = a.id, company = a.company, lastName = a.lastName, firstName = a.firstName, middleName = a.middleName, contactNumber = a.contactNumber, emailAddress = a.emailAddress })
                                                .Include(a => a.company)
                                                .Include(a => a.property)
                                                .Include(a => a.tenant)
                                                .Where(r => r.id.Equals(Guid.Parse(id)) && r.companyId.Equals(CompanyId) && r.deleted.Equals(false)).FirstOrDefaultAsync();


                //id = properDirectoryId;
                
                pd.updateDate = datetoday;
                pd.updatedBy = UserName;

                //pd.dateFrom = tenants.dateFrom;
                //pd.dateTo = tenants.dateTo;
                //pd.monthlyRate = tenants.monthlyRate;
                pd.monthlyRate = new_monthly_rate;
                pd.associationDues = _assorate;
                pd.penaltyPct = tenants.penaltyPct;
                pd.ratePerSQM = tenants.ratePerSQM;
                pd.totalBalance = tenants.totalBalance;
                pd.withWT = withWT_sw;
                pd.ratePerSQMAssocDues = tenants.ratePerSQMAssocDues;


                appDBContext.PropertyDirectory.Update(pd);
                //await appDBContext.SaveChangesAsync();


                //if (selectedFiles != null)
                //{
                //    foreach (var file in selectedFiles)
                //    {


                //        //var tmpPath = Path.Combine(_env.WebRootPath, "Uploaded/TenantDocument");
                //        var tmpPath = Path.Combine(_env.WebRootPath, "Uploaded", "Attachments");
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
                //        td.propertyDirectoryId = Guid.Parse(id);
                //        td.id = Guid.Parse(fileId);
                //        td.fileName = prefix.ToString();
                //        td.filePath = filedestination.ToString();
                //        td.fileDesc = file.Name;
                //        td.extName = extname;

                //        appDBContext.TenantDocuments.Add(td);
                //        await appDBContext.SaveChangesAsync();

                //        using (FileStream DestinationStream = File.Create(filedestination))
                //        {
                //            await file.Data.CopyToAsync(DestinationStream);
                //        }
                //    }
                //}

                var tenantDocs = await appDBContext.TenantDocuments.Where(td => td.propertyDirectoryId.Equals(pd.id)).ToListAsync();


                if (tenants.tenantDocument != null && tenants.tenantDocument.Any())
                {
                    foreach (var td in tenants.tenantDocument)
                    {
                        var tenantDoc = tenantDocs.Where(d => d.id.Equals(td.id)).FirstOrDefault();
                        if(tenantDoc==null)
                        {

                            appDBContext.TenantDocuments.Add(new TenantDocument()
                            {
                                id = td.id,
                                propertyDirectoryId = pd.id,
                                createDate = td.createDate,
                                createdBy = td.createdBy,
                                fileName = td.fileName,
                                fileDesc = td.fileDesc,
                                extName = td.extName,
                                filePath = td.filePath
                            });
                        }
                        else
                        {
                            tenantDoc.deleted = tenantDoc.deleted;
                            appDBContext.TenantDocuments.Update(tenantDoc);
                        }
                    }
                }

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
                                                    .Where(r => r.id.Equals(Guid.Parse(id)) && r.companyId.Equals(CompanyId) && r.deleted.Equals(false)).FirstOrDefaultAsync();
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
                        withWT = data.withWT?"Y":"N",
                        ratePerSQMAssocDues = data.ratePerSQMAssocDues,

                        //Owned_Mgd = data.tenant.Owned_Mgd,
                        //MgtFeePct = data.tenant.MgtFeePct,
                        //CCTNumber = data.tenant.CCTNumber,


                        contactNo2 = data.tenant.contactNo2,
                        contactNo3 = data.tenant.contactNo3,
                        workAddress = data.tenant.workAddress,
                        emergyFullName = data.tenant.emergyFullName,
                        emergyContactNo = data.tenant.emergyContactNo,
                        emergyAdrress = data.tenant.emergyAdrress,
                        emergyRelationshipOwner = data.tenant.emergyRelationshipOwner,
                        //otherRestenanted1 = data.tenant.otherRestenanted1,
                        //otherResResiding1 = data.tenant.otherResResiding1,
                        //otherResFullName1 = data.tenant.otherResFullName1,
                        //otherResRelationshipToOwner1 = data.tenant.otherResRelationshipToOwner1,
                        //otherRestenanted2 = data.tenant.otherRestenanted2,
                        //otherResResiding2 = data.tenant.otherResResiding2,
                        //otherResFullName2 = data.tenant.otherResFullName2,
                        //otherResRelationshipToOwner2 = data.tenant.otherResRelationshipToOwner2,
                        //otherRestenanted3 = data.tenant.otherRestenanted3,
                        //otherResResiding3 = data.tenant.otherResResiding3,
                        //otherResFullName3 = data.tenant.otherResFullName3,
                        //otherResRelationshipToOwner3 = data.tenant.otherResRelationshipToOwner3,

                        subTenantFullName1 = data.tenant.subTenantFullName1,
                        subTenantID1 = data.tenant.subTenantID1,
                        subTenantHomeAddress1 = data.tenant.subTenantHomeAddress1,
                        subTenantWorkAddress1 = data.tenant.subTenantWorkAddress1,
                        subTenantContactNo1 = data.tenant.subTenantContactNo1,
                        subTenantEmailAdd1 = data.tenant.subTenantEmailAdd1,
                        RelToPrimary1 = data.tenant.RelToPrimary1,

                        subTenantFullName2 = data.tenant.subTenantFullName2,
                        subTenantID2 = data.tenant.subTenantID2,
                        subTenantHomeAddress2 = data.tenant.subTenantHomeAddress2,
                        subTenantWorkAddress2 = data.tenant.subTenantWorkAddress2,
                        subTenantContactNo2 = data.tenant.subTenantContactNo2,
                        subTenantEmailAdd2 = data.tenant.subTenantEmailAdd2,
                        RelToPrimary2 = data.tenant.RelToPrimary2,

                        subTenantFullName3 = data.tenant.subTenantFullName1,
                        subTenantID3 = data.tenant.subTenantID1,
                        subTenantHomeAddress3 = data.tenant.subTenantHomeAddress3,
                        subTenantWorkAddress3 = data.tenant.subTenantWorkAddress3,
                        subTenantContactNo3 = data.tenant.subTenantContactNo3,
                        subTenantEmailAdd3 = data.tenant.subTenantEmailAdd3,
                        RelToPrimary3 = data.tenant.RelToPrimary3
                    };


                    tenants.tenantDocument = await appDBContext.TenantDocuments
                                                                        .Where(r => r.propertyDirectoryId.Equals(Guid.Parse(id)) && r.deleted.Equals(false))
                                                                        .ToListAsync();
                    
                }
                tenants.properties = await appDBContext.Properties
                                                            .Where(r => r.companyId.Equals(CompanyId) && r.deleted.Equals(false))
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
