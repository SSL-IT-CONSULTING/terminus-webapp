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

        public void HandleSelection(IFileListEntry[] files)
        {
            selectedFiles = files;
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
                        ownerRemarks = properties.ownerRemarks


                    };

                appDBContext.Properties.Add(pr);

                await appDBContext.SaveChangesAsync();
                }
                else
                {

                    Property pr = new Property()
                    {

                        id = propertyid,
                        companyId = CompanyId,
                        createDate = DateTime.Now,
                        createdBy = UserName,
                        description = properties.description,
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
                        otherRestenanted = properties.otherRestenanted,
                        otherResResiding = properties.otherResResiding,
                        otherResFullName1 = properties.otherResFullName1,
                        otherResRelationshipToOwner1 = properties.otherResRelationshipToOwner1,
                        otherResFullName2 = properties.otherResFullName2,
                        otherResRelationshipToOwner2 = properties.otherResRelationshipToOwner2,
                        otherResFullName3 = properties.otherResFullName3,
                        otherResRelationshipToOwner3 = properties.otherResRelationshipToOwner3
                    };

                    appDBContext.Properties.Add(pr);

                    await appDBContext.SaveChangesAsync();
                }







            }
            else
            {
                var data = await appDBContext.Properties
                          //.Select(a => new { id = a.id, company = a.company, lastName = a.lastName, firstName = a.firstName, middleName = a.middleName, contactNumber = a.contactNumber, emailAddress = a.emailAddress })
                          .Include(a => a.company)
                          .Where(r => r.id.Equals(id) &&  r.companyId.Equals(CompanyId) && r.deleted.Equals(false)).FirstOrDefaultAsync();


                data.updateDate = DateTime.Now;
                data.updatedBy = UserName;
                data.description = properties.description;
                data.address = properties.address;
                data.propertyType = properties.propertyType;
                data.areaInSqm = properties.areaInSqm;
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
                    data.otherRestenanted = properties.otherRestenanted;
                    data.otherResResiding = properties.otherResResiding;
                    data.otherResFullName1 = properties.otherResFullName1;
                    data.otherResRelationshipToOwner1 = properties.otherResRelationshipToOwner1;
                    data.otherResFullName2 = properties.otherResFullName2;
                    data.otherResRelationshipToOwner2 = properties.otherResRelationshipToOwner2;
                    data.otherResFullName3 = properties.otherResFullName3;
                    data.otherResRelationshipToOwner3 = properties.otherResRelationshipToOwner3;
                }
                appDBContext.Properties.Update(data);
                await appDBContext.SaveChangesAsync();

            }



            if (selectedFiles != null)
            { 
            
            
                foreach (var file in selectedFiles)
                {


                    var tmpPath = Path.Combine(_env.WebRootPath, "Uploaded/PropertyDocument");

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



                    var pd = new PropertyDocument();

                    pd.createDate = datetoday;
                    pd.createdBy = UserName;
                    pd.propertyId = Guid.Parse(id);
                    pd.id = Guid.Parse(fileId);
                    pd.fileName = prefix.ToString();
                    pd.filePath = filedestination.ToString();
                    pd.fileDesc = file.Name;
                    pd.extName = extname;

                    appDBContext.PropertyDocument.Add(pd);
                    await appDBContext.SaveChangesAsync();

                    using (FileStream DestinationStream = File.Create(filedestination))
                    {
                        await file.Data.CopyToAsync(DestinationStream);
                    }
                }
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

                    properties = new PropertyViewModel()
                    {
                        id = data.id,
                        description = data.description,
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
                        otherResFullName1 = data.otherResFullName1,
                        otherResRelationshipToOwner1 = data.otherResRelationshipToOwner1,
                        otherResFullName2 = data.otherResFullName2,
                        otherResRelationshipToOwner2 = data.otherResRelationshipToOwner2,
                        otherResFullName3 = data.otherResFullName3,
                        otherResRelationshipToOwner3 = data.otherResRelationshipToOwner3

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
