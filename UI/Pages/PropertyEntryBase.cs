﻿using Blazored.SessionStorage;
using BlazorInputFile;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using terminus.dataaccess;
using terminus.shared.models;


namespace terminus_webapp.Pages
{
    public class PropertyEntryBase : ComponentBase
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

        public PropertyViewModel properties { get; set; }

        public PropertyDocument tenantDouments { get; set; }

        public ReferenceViewModal referenceVM { get; set; }

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


        public int numLines;
        public IFileListEntry[] selectedFiles;

        public async Task HandleSelection(IFileListEntry[] files)
        {
            if (this.properties.propertyDocument == null)
                this.properties.propertyDocument = new List<PropertyDocument>();

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
                    var pd = new PropertyDocument();
                  
                    pd.createDate = DateTime.Now;
                    pd.createdBy = UserName;
                    pd.id = Guid.NewGuid();
                    pd.fileName = prefix.ToString();
                    pd.filePath = Path.Combine(uploadPath, $"{prefix}{ext}");
                    pd.fileDesc = file.Name;
                    pd.extName = ext;

                    properties.propertyDocument.Add(pd);
                }
            }
        }


        protected void HandleDeleteAttachment(string id)
        {
            var item = properties.propertyDocument.FirstOrDefault(a => a.id.Equals(Guid.Parse(id)));
            if (item != null)
            {
                item.deleted = true;

            }
        }

        protected async Task HandleValidSubmit()
          {

            UserName = await _sessionStorageService.GetItemAsync<string>("UserName");
            CompanyId = await _sessionStorageService.GetItemAsync<string>("CompanyId");

            DateTime datetoday = DateTime.Now;

            string propertyid = Guid.NewGuid().ToString();

            if (string.IsNullOrEmpty(properties.id))
            {

                string _pro = properties.propertyType;

                if (CompanyId == "ADBCA")
                {
                    Property pr = new Property()
                    {


                        id = propertyid,
                        companyId = CompanyId,
                        createDate = DateTime.Now,
                        createdBy = UserName,
                        description = properties.description,
                        BuildingCode = "ADBC",
                        address = properties.address,
                        propertyType = properties.propertyType,
                        areaInSqm = properties.areaInSqm,


                        ownerLastName = properties.ownerLastName,
                        ownerFirstName = properties.ownerFirstName,
                        ownerMiddleName = properties.ownerMiddleName,
                        ownerFullName = properties.ownerFullName,
                        ownerAddress = properties.ownerAddress,
                        ownerContactNo = properties.ownerContactNo,
                        ownerEmailAdd = properties.ownerEmailAdd,
                        ownerRemarks = properties.ownerRemarks,
                        ownerProxID = properties.ownerProxID

                    };

                appDBContext.Properties.Add(pr);

               // await appDBContext.SaveChangesAsync();
                }
                else
                {

                    string resident_type = properties.residingType.ToString();

                    string _tenanted;
                    string _residing;

                    if (resident_type == "Tenanted")
                    {
                        _tenanted = "Y";
                        _residing = "N";


                        Property pr = new Property()
                        {



                            id = propertyid,
                            companyId = CompanyId,
                            createDate = DateTime.Now,
                            createdBy = UserName,
                            description = properties.description,
                            //BuildingCode = properties.BuildingCode,
                            BuildingCode = "ADBC",
                            address = properties.address,
                            propertyType = properties.propertyType,
                            areaInSqm = properties.areaInSqm,


                            Owned_Mgd = properties.Owned_Mgd,
                            MgtFeePct = properties.MgtFeePct,
                            CCTNumber = properties.CCTNumber,
                            emergyFullName = properties.emergyFullName,
                            emergyContactNo = properties.emergyContactNo,
                            emergyAdrress = properties.emergyAdrress,
                            emergyRelationshipOwner = properties.emergyRelationshipOwner,

                            otherRestenanted = _tenanted,
                            otherResResiding = _residing,


                            otherResFullName1 = "",
                            otherResRelationshipToOwner1 = "",
                            otherResContactNo1 = "",
                            otherResFullName2 = "",
                            otherResRelationshipToOwner2 = "",
                            otherResContactNo2 = "",
                            otherResFullName3 = "",
                            otherResRelationshipToOwner3 = "",
                            otherResContactNo3 = "",
                            otherResProxID1 = "",
                            otherResProxID2 = "",
                            otherResProxID3 = ""
                    };
                        appDBContext.Properties.Add(pr);
                    }
                    else if (resident_type == "Residing")
                    {
                        _tenanted = "N";
                        _residing = "Y";


                        Property pr = new Property()
                        {

                            id = propertyid,
                            companyId = CompanyId,
                            createDate = DateTime.Now,
                            createdBy = UserName,
                            description = properties.description,
                            BuildingCode = "ADBC",
                            
                            address = properties.address,
                            propertyType = properties.propertyType,
                            areaInSqm = properties.areaInSqm,


                            Owned_Mgd = properties.Owned_Mgd,
                            MgtFeePct = properties.MgtFeePct,
                            CCTNumber = properties.CCTNumber,
                            emergyFullName = properties.emergyFullName,
                            emergyContactNo = properties.emergyContactNo,
                            emergyAdrress = properties.emergyAdrress,
                            emergyRelationshipOwner = properties.emergyRelationshipOwner,

                            otherRestenanted = _tenanted,
                            otherResResiding = _residing,


                            otherResFullName1 = properties.otherResFullName1,
                            otherResContactNo1 = properties.otherResContactNo1,
                            otherResRelationshipToOwner1 = properties.otherResRelationshipToOwner1,
                            otherResFullName2 = properties.otherResFullName2,
                            otherResContactNo2 = properties.otherResContactNo2,
                            otherResRelationshipToOwner2 = properties.otherResRelationshipToOwner2,
                            otherResFullName3 = properties.otherResFullName3,
                            otherResContactNo3 = properties.otherResContactNo3,
                            otherResRelationshipToOwner3 = properties.otherResRelationshipToOwner3,
                            otherResProxID1 = properties.otherResProxID1,
                            otherResProxID2 = properties.otherResProxID2,
                            otherResProxID3 = properties.otherResProxID3
                        };
                        appDBContext.Properties.Add(pr);
                    }
                    else 
                    {
                        _tenanted = "Y";
                        _residing = "N";

                        Property pr = new Property()
                        {
                            id = propertyid,
                            companyId = CompanyId,
                            createDate = DateTime.Now,
                            createdBy = UserName,
                            description = properties.description,
                            BuildingCode = "ADBC",
                            address = properties.address,
                            propertyType = properties.propertyType,
                            areaInSqm = properties.areaInSqm,


                            Owned_Mgd = properties.Owned_Mgd,
                            MgtFeePct = properties.MgtFeePct,
                            CCTNumber = properties.CCTNumber,
                            emergyFullName = properties.emergyFullName,
                            emergyContactNo = properties.emergyContactNo,
                            emergyAdrress = properties.emergyAdrress,
                            emergyRelationshipOwner = properties.emergyRelationshipOwner,

                            otherRestenanted = _tenanted,
                            otherResResiding = _residing,


                            otherResFullName1 = "",
                            otherResContactNo1 = "",
                            otherResRelationshipToOwner1 = "",
                            otherResFullName2 = "",
                            otherResContactNo2 = "",
                            otherResRelationshipToOwner2 = "",
                            otherResFullName3 = "",
                            otherResContactNo3 = "",
                            otherResRelationshipToOwner3 = ""

                            
                        };
                        appDBContext.Properties.Add(pr);
                    }






                    // await appDBContext.SaveChangesAsync();
                }


                if (properties.propertyDocument != null && properties.propertyDocument.Any())
                {
                    foreach (var td in properties.propertyDocument)
                    {
                        appDBContext.PropertyDocument.Add(new PropertyDocument()
                        {
                            id = td.id,
                            propertyId = Guid.Parse(propertyid),
                            createDate = td.createDate,
                            createdBy = td.createdBy,
                            fileName = td.fileName,
                            fileDesc = td.fileDesc,
                            extName = td.extName,
                            filePath = td.filePath
                        });
                    }
                }


                await appDBContext.SaveChangesAsync();


            }
            else
            {
                var data = await appDBContext.Properties
                          //.Select(a => new { id = a.id, company = a.company, lastName = a.lastName, firstName = a.firstName, middleName = a.middleName, contactNumber = a.contactNumber, emailAddress = a.emailAddress })
                          .Include(a => a.company)
                          .Where(r => r.id.Equals(id) &&  r.companyId.Equals(CompanyId) && r.deleted.Equals(false)).FirstOrDefaultAsync();


                string resident_type = properties.residingType;

                string _tenanted;
                string _residing;

                if (resident_type == "Tenanted")
                {
                    _tenanted = "Y";
                    _residing = "N";

                    data.otherResFullName1 = "";
                    data.otherResContactNo1 = "";
                    data.otherResRelationshipToOwner1 = "";
                    data.otherResFullName2 = "";
                    data.otherResContactNo2 = "";
                    data.otherResRelationshipToOwner2 = "";
                    data.otherResFullName3 = "";
                    data.otherResContactNo3 = "";
                    data.otherResRelationshipToOwner3 = "";

                    
                    data.otherResProxID1 = "";
                    data.otherResProxID2 = "";
                    data.otherResProxID3 = "";
                }
                else if (resident_type == "Residing")
                {
                    _tenanted = "N";
                    _residing = "Y";

                    data.otherResFullName1 = properties.otherResFullName1;
                    data.otherResContactNo1 = properties.otherResContactNo1;
                    data.otherResRelationshipToOwner1 = properties.otherResRelationshipToOwner1;
                    data.otherResFullName2 = properties.otherResFullName2;
                    data.otherResContactNo2 = properties.otherResContactNo2;
                    data.otherResRelationshipToOwner2 = properties.otherResRelationshipToOwner2;
                    data.otherResFullName3 = properties.otherResFullName3;
                    data.otherResContactNo3 = properties.otherResContactNo3;
                    data.otherResRelationshipToOwner3 = properties.otherResRelationshipToOwner3;

                    data.otherResProxID1 = properties.otherResProxID1;
                    data.otherResProxID2 = properties.otherResProxID2;
                    data.otherResProxID3 = properties.otherResProxID3;
                }
                else
                {
                    _tenanted = "N";
                    _residing = "N";
                    data.otherResFullName1 = "";
                    data.otherResContactNo1 = "";
                    data.otherResRelationshipToOwner1 = "";
                    data.otherResFullName2 = "";
                    data.otherResContactNo2 = "";
                    data.otherResRelationshipToOwner2 = "";
                    data.otherResFullName3 = "";
                    data.otherResContactNo3 = "";
                    data.otherResRelationshipToOwner3 = "";
                }



                data.updateDate = DateTime.Now;
                data.updatedBy = UserName;
                data.description = properties.description;
                data.BuildingCode = "ADBC";
                data.address = properties.address;
                data.propertyType = properties.propertyType;
                data.areaInSqm = properties.areaInSqm;


                data.Owned_Mgd = properties.Owned_Mgd;
                data.MgtFeePct = properties.MgtFeePct;
                data.CCTNumber = properties.CCTNumber;
                data.emergyFullName = properties.emergyFullName;
                data.emergyContactNo = properties.emergyContactNo;
                data.emergyAdrress = properties.emergyAdrress;
                data.emergyRelationshipOwner = properties.emergyRelationshipOwner;
                data.otherRestenanted = _tenanted;
                data.otherResResiding = _residing;


                if (CompanyId == "ADBCA")
                {
                    data.ownerLastName = properties.ownerLastName;
                    data.ownerFirstName = properties.ownerFirstName;
                    data.ownerMiddleName = properties.ownerMiddleName;
                    data.ownerFullName = properties.ownerFullName;
                    data.ownerAddress = properties.ownerAddress;
                    data.ownerContactNo = properties.ownerContactNo;
                    data.ownerEmailAdd = properties.ownerEmailAdd;
                    data.ownerRemarks = properties.ownerRemarks;
                    data.Owned_Mgd = properties.Owned_Mgd;
                    data.MgtFeePct = properties.MgtFeePct;
                    data.CCTNumber = properties.CCTNumber;
                    data.emergyFullName = properties.emergyFullName;
                    data.emergyContactNo = properties.emergyContactNo;
                    data.emergyAdrress = properties.emergyAdrress;
                    data.emergyRelationshipOwner = properties.emergyRelationshipOwner;
                    data.otherRestenanted = _tenanted;
                    data.otherResResiding = _residing;
                    data.otherResFullName1 = properties.otherResFullName1;
                    data.otherResContactNo1 = properties.otherResContactNo1;
                    data.otherResRelationshipToOwner1 = properties.otherResRelationshipToOwner1;
                    data.otherResFullName2 = properties.otherResFullName2;
                    data.otherResContactNo2 = properties.otherResContactNo2;
                    data.otherResRelationshipToOwner2 = properties.otherResRelationshipToOwner2;
                    data.otherResFullName3 = properties.otherResFullName3;
                    data.otherResContactNo3 = properties.otherResContactNo3;
                    data.otherResRelationshipToOwner3 = properties.otherResRelationshipToOwner3;

                    data.ownerProxID = properties.ownerProxID;
                    data.otherResProxID1 = properties.otherResProxID1;
                    data.otherResProxID2 = properties.otherResProxID2;
                    data.otherResProxID3 = properties.otherResProxID3;
                }
                appDBContext.Properties.Update(data);


                var propertyDocs = await appDBContext.PropertyDocument.Where(td => td.propertyId.Equals(Guid.Parse(data.id))).ToListAsync();


                if (properties.propertyDocument != null && properties.propertyDocument.Any())
                {
                    foreach (var pd in properties.propertyDocument)
                    {
                        var propertyDoc = propertyDocs.Where(d => d.id.Equals(pd.id)).FirstOrDefault();
                        if (propertyDoc == null)
                        {

                            appDBContext.PropertyDocument.Add(new PropertyDocument()
                            {
                                id = pd.id,
                                propertyId = Guid.Parse(data.id),
                                createDate = pd.createDate,
                                createdBy = pd.createdBy,
                                fileName = pd.fileName,
                                fileDesc = pd.fileDesc,
                                extName = pd.extName,
                                filePath = pd.filePath
                            });
                        }
                        else
                        {
                            propertyDoc.deleted = propertyDoc.deleted;
                            appDBContext.PropertyDocument.Update(propertyDoc);
                        }
                    }
                }

                await appDBContext.SaveChangesAsync();

            }



            //if (selectedFiles != null)
            //{ 
            
            
            //    foreach (var file in selectedFiles)
            //    {


            //        var tmpPath = Path.Combine(_env.WebRootPath, "Uploaded/PropertyDocument");

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



            //        var pd = new PropertyDocument();

            //        pd.createDate = datetoday;
            //        pd.createdBy = UserName;
            //        pd.propertyId = Guid.Parse(id);
            //        pd.id = Guid.Parse(fileId);
            //        pd.fileName = prefix.ToString();
            //        pd.filePath = filedestination.ToString();
            //        pd.fileDesc = file.Name;
            //        pd.extName = extname;

            //        appDBContext.PropertyDocument.Add(pd);
            //        await appDBContext.SaveChangesAsync();

            //        using (FileStream DestinationStream = File.Create(filedestination))
            //        {
            //            await file.Data.CopyToAsync(DestinationStream);
            //        }
            //    }
            //}


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
                    properties = new PropertyViewModel();
                    // glAccount.createDate = DateTime.Today;

                    //properties.id = Guid.Parse(Id).ToString();

                    
                }
                else
                {
                    IsViewOnly = false;
                    IsEditOnly = true;




                    var data = await appDBContext.Properties
                                                    .Where(a => a.id.Equals(id) &&  a.companyId.Equals(CompanyId) && a.deleted.Equals(false))
                                                    .OrderBy(a => a.description)
                                                    .FirstOrDefaultAsync();




                    string resident_type = data.otherRestenanted;

                    if (string.IsNullOrEmpty(data.otherRestenanted))
                    {
                        resident_type = "";
                    }
                    else
                    {
                        if (data.otherRestenanted == "Y")
                        {
                            resident_type = "Tenanted";
                        }
                        else if (data.otherResResiding == "Y")
                        {
                            resident_type = "Residing";
                        }
                        else
                        {
                            resident_type = "Vacant";
                        }
                    }

                    /*
                     * Code not in use
                    string _tenanted;
                    string _residing;

                    if (resident_type == "Tenanted")
                    {
                        _tenanted = "Y";
                        _residing = "N";
                    }
                    else if (resident_type == "Residing")
                    {
                        _tenanted = "N";
                        _residing = "Y";
                    }
                    else
                    {
                        _tenanted = "N";
                        _residing = "N";

                    }
                    */

                    properties = new PropertyViewModel()
                    {
                        id = data.id,
                        description = data.description,
                        BuildingCode = data.BuildingCode,
                        address = data.address,
                        propertyType = data.propertyType,
                        areaInSqm = data.areaInSqm,

                        ownerLastName = data.ownerLastName,
                        ownerFirstName = data.ownerFirstName,
                        ownerMiddleName = data.ownerMiddleName,
                        ownerFullName = data.ownerFullName,
                        ownerAddress = data.ownerAddress,
                        ownerContactNo = data.ownerContactNo,
                        ownerEmailAdd = data.ownerEmailAdd,
                        ownerRemarks = data.ownerRemarks,

                        Owned_Mgd = data.Owned_Mgd,
                        MgtFeePct = data.MgtFeePct,
                        CCTNumber = data.CCTNumber,
                        emergyFullName = data.emergyFullName,
                        emergyContactNo = data.emergyContactNo,
                        emergyAdrress = data.emergyAdrress,
                        emergyRelationshipOwner = data.emergyRelationshipOwner,
                        otherRestenanted = data.otherRestenanted,
                        otherResResiding = data.otherResResiding,
                        residingType = resident_type,
                        otherResFullName1 = data.otherResFullName1,
                        otherResContactNo1 = data.otherResContactNo1,
                        otherResRelationshipToOwner1 = data.otherResRelationshipToOwner1,
                        otherResFullName2 = data.otherResFullName2,
                        otherResContactNo2 = data.otherResContactNo2,
                        otherResRelationshipToOwner2 = data.otherResRelationshipToOwner2,
                        otherResFullName3 = data.otherResFullName3,
                        otherResContactNo3 = data.otherResContactNo3,
                        otherResRelationshipToOwner3 = data.otherResRelationshipToOwner3,
                        ownerProxID = data.ownerProxID,
                        otherResProxID1 = data.otherResProxID1,
                        otherResProxID2 = data.otherResProxID2,
                        otherResProxID3 = data.otherResProxID3


                    };

                    properties.propertyDocument = await appDBContext.PropertyDocument
                                                                        .Where(r => r.propertyId.Equals(Guid.Parse(id)) && r.deleted.Equals(false))
                                                                        .ToListAsync();
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
