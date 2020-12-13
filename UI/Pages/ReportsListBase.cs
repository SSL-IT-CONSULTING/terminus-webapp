using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using terminus.dataaccess;
using terminus.shared.models;

namespace terminus_webapp.Pages
{
    public class ReportsListBase : ComponentBase 
    {


        [Inject]
        public IWebHostEnvironment _env { get; set; }

        [Inject]
        public AppDBContext appDBContext { get; set; }

        [Inject]
        public DapperManager dapperManager { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }


        [Inject]
        public ISessionStorageService _sessionStorageService { get; set; }

        public List<ReferenceViewModal> ReferenceViewModal { get; set; }


        public List<rptPropertypeInventoryMV> rptPropertypeInventoryMV { get; set; }

        public List<rptBuildingDirectoryVM> rptBuildingDirectoryVM { get; set; }

        public List<rptRentalIncomeVM> rptRentalIncomeVM { get; set; }

        public List<rptOtherIncomeDetailVM> rptOtherIncomeDetailVM { get; set; }
        public List<rptExpensesVM> rptExpensesVM { get; set; }

        public List<rptCollectionVM> rptCollectionVM { get; set; }


        public List<rptNetIncomeSummaryVM> rptNetIncomeSummaryVM { get; set; }


        public List<rptNetIncomeVM> rptNetIncomeVM { get; set; }

        public List<rptBalanceSheetVM> rptbalanceSheetVM { get; set; }


        public List<rptTrialBalanceVM> rptTrialBalanceVM { get; set; }

        public List<rptGLDetailViewModel> rptGLDVM { get; set; }

        public List<rptEmployee> rptEmployees { get; set; }

        public List<rptReceiptsOverExpensesVM> rptReceiptsOverExpensesVM { get; set; }

        [Parameter]
        public string reportParams { get; set; }

        //[Parameter]
        public DateTime dateFrom { get; set; }

        public DateTime dateTo { get; set; }
        //[Parameter]
        public string reportType { get; set; }

        public bool DataLoaded { get; set; }
        public string ErrorMessage { get; set; }

        public string _path { get; set; }
        //public string asofdate { get; set; }
        //public string reporttype { get; set; }


        //private IHostingEnvironment _hostingEnvironment;
        //public ImportExportModel(IHostingEnvironment hostingEnvironment)
        //{
        //    _hostingEnvironment = hostingEnvironment;
        //}

        public string CompanyId { get; set; }
        public string UserName { get; set; }





        public void  ExportToExcel()
        {
            //string path = Path.Combine(_env.WebRootPath, "Uploaded");

            //string sWebRootFolder = _env.WebRootPath.ToString();
            string sFileName = @"" + reportType + ".xlsx";
            


            //var newpath = reportType + "" + "_" + DateTime.Today.ToString("MM/dd/yyyy").Replace("/", "").Replace(" ", "").Replace(":", "") + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + ".xlsx";

           //string _filepath = path + newpath;


            //string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, _filepath);

          
            var p = reportParams.Split("|");

            dateFrom = DateTime.Parse(p[0]);
            dateTo= DateTime.Parse(p[1]);
            reportType = p[2];

            DataTable table = new DataTable();


            try
            {

                if (reportType == "Employees")
                {
                    table = (DataTable)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(rptEmployees), (typeof(DataTable)));
                }

                if (reportType == "GLDetails")
                {
                    table = (DataTable)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(rptGLDVM), (typeof(DataTable)));
                }

                if (reportType == "PropertyInventory")
                {

                    table = (DataTable)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(rptPropertypeInventoryMV), (typeof(DataTable)));

                }

                if (reportType == "BuildingDirectory")
                {

                    table = (DataTable)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(rptBuildingDirectoryVM), (typeof(DataTable)));

                }

                if (reportType == "RentalIncomeDetails")
                {

                    table = (DataTable)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(rptRentalIncomeVM), (typeof(DataTable)));
                }

                if (reportType == "OtherIncomeDetails")
                {

                    table = (DataTable)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(rptOtherIncomeDetailVM), (typeof(DataTable)));
                }

                if (reportType == "Expenses")
                {

                    table = (DataTable)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(rptExpensesVM), (typeof(DataTable)));
                }

                if (reportType == "NetIncomeSummary")
                {

                    table = (DataTable)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(rptNetIncomeSummaryVM), (typeof(DataTable)));
                }

                if (reportType == "TrialBalance")
                {

                    table = (DataTable)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(rptTrialBalanceVM), (typeof(DataTable)));
                }


                if (reportType == "NetIncome")
                {

                    table = (DataTable)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(rptNetIncomeVM), (typeof(DataTable)));
                }

                if (reportType == "BalanceSheet")
                {

                    table = (DataTable)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(rptbalanceSheetVM), (typeof(DataTable)));
                }

                if (reportType == "ReceiptsOverExpenses")
                {
                    table = (DataTable)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(rptReceiptsOverExpensesVM), (typeof(DataTable)));
                }

                if (reportType == "Collections")
                {
                    table = (DataTable)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(rptCollectionVM), (typeof(DataTable)));
                }

                var fileName = $"{DateTime.Today.ToString("yyyyMMdd")}{Guid.NewGuid().ToString()}.xlsx"; 

                FileInfo file = new FileInfo(Path.Combine(_env.WebRootPath, "Uploaded", fileName));
                //FileInfo filePath = new FileInfo(_filepath);
                using (var excelPack = new ExcelPackage(file))
                {
                    var ws = excelPack.Workbook.Worksheets.Add("sheet1");
                    ws.Cells.LoadFromDataTable(table, true, OfficeOpenXml.Table.TableStyles.Light8);
                    excelPack.Save();
                    _path = $"/Uploaded/{fileName}";
                }
                
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }



        protected override async Task OnInitializedAsync()
        {
            try
            {
                DataLoaded = false;
                ErrorMessage = string.Empty;


                UserName = await _sessionStorageService.GetItemAsync<string>("UserName");
                CompanyId = await _sessionStorageService.GetItemAsync<string>("CompanyId");


                var p = reportParams.Split("|");

                dateFrom = DateTime.Parse(p[0]);
                dateTo = DateTime.Parse(p[1]);
                reportType = p[2];

                // var sqlcommand = $"exec ASRCReportsDtls '{companyId.Replace("'", "''")}'";
                // var sqlcommand = $"exec ASRCReportsDtls '{companyId.Replace("'", "''")}'";
                var param = new Dapper.DynamicParameters();
                param.Add("dateFrom", dateFrom, System.Data.DbType.DateTime);
                param.Add("dateTo", dateTo, System.Data.DbType.DateTime);
                param.Add("ReportType", reportType, System.Data.DbType.String);
                param.Add("companyId", CompanyId, System.Data.DbType.String);



                //string path; 
                DataTable table = new DataTable();

                if (reportType == "Employees")
                {
                    var emp = await
                        dapperManager.GetAllAsync<rptEmployee>("terminusReports", param);
                    
                    rptEmployees = emp.OrderBy(a => a.LastName).ThenBy(a => a.FirstName).ThenBy(a => a.MiddleName).ToList();

                    //rptEmployees.OrderBy(a => a.LastName).ThenBy(a => a.FirstName).ThenBy(a => a.MiddleName);

                    table = (DataTable)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(rptEmployees), (typeof(DataTable)));
                }

                if (reportType == "GLDetails")
                {
                    rptGLDVM = await
                        dapperManager.GetAllAsync<rptGLDetailViewModel>("terminusReports", param);

                    
                    table = (DataTable)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(rptGLDVM), (typeof(DataTable)));
                }


                if (reportType == "PropertyInventory")
                {

                    //rptPropertypeInventoryMV = await
                    //    dapperManager.GetAllAsync<rptPropertypeInventoryMV>("terminusReports", param);

                    var pti = await dapperManager.GetAllAsync<rptPropertypeInventoryMV>("terminusReports", param);
                    rptPropertypeInventoryMV = pti.OrderBy(a => a.description).ToList();

                    table = (DataTable)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(rptPropertypeInventoryMV), (typeof(DataTable)));
                }


                if (reportType == "BuildingDirectory")
                {
                    rptBuildingDirectoryVM = await
                        dapperManager.GetAllAsync<rptBuildingDirectoryVM>("terminusReports", param);

                    table = (DataTable)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(rptBuildingDirectoryVM), (typeof(DataTable)));
                }


                if (reportType == "RentalIncomeDetails")
                {
                    rptRentalIncomeVM = await
                        dapperManager.GetAllAsync<rptRentalIncomeVM>("terminusReports", param);

                    
                    table = (DataTable)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(rptRentalIncomeVM), (typeof(DataTable)));
                }

                if (reportType == "OtherIncomeDetails")
                {
                    rptOtherIncomeDetailVM = await
                        dapperManager.GetAllAsync<rptOtherIncomeDetailVM>("terminusReports", param);




                    table = (DataTable)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(rptOtherIncomeDetailVM), (typeof(DataTable)));
                }

                if (reportType == "Expenses")
                {
                    rptExpensesVM = await
                        dapperManager.GetAllAsync<rptExpensesVM>("terminusReports", param);

                    
                    //path = @"d:\Expenses" + "_" + DateTime.Today.ToString() + ".xlsx";

                    table = (DataTable)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(rptExpensesVM), (typeof(DataTable)));
                }

                if (reportType == "NetIncomeSummary")
                {
                    rptNetIncomeSummaryVM = await
                        dapperManager.GetAllAsync<rptNetIncomeSummaryVM>("terminusReports", param);


                    table = (DataTable)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(rptNetIncomeSummaryVM), (typeof(DataTable)));
                }

                if (reportType == "TrialBalance")
                {
                    rptTrialBalanceVM = await
                        dapperManager.GetAllAsync<rptTrialBalanceVM>("terminusReports", param);

                    table = (DataTable)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(rptTrialBalanceVM), (typeof(DataTable)));
                }


                if (reportType == "NetIncome")
                {
                    rptNetIncomeVM = await
                        dapperManager.GetAllAsync<rptNetIncomeVM>("terminusReports", param);

                    table = (DataTable)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(rptNetIncomeVM), (typeof(DataTable)));
                }

                if (reportType == "BalanceSheet")
                {
                    rptbalanceSheetVM = await
                        dapperManager.GetAllAsync<rptBalanceSheetVM>("terminusReports", param);

                    table = (DataTable)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(rptbalanceSheetVM), (typeof(DataTable)));
                }

                if (reportType == "ReceiptsOverExpenses")
                {
                    rptReceiptsOverExpensesVM = await
                        dapperManager.GetAllAsync<rptReceiptsOverExpensesVM>("terminusReports", param);

                    table = (DataTable)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(rptReceiptsOverExpensesVM), (typeof(DataTable)));
                }

                if (reportType == "Collections")
                {
                    rptCollectionVM = await
                        dapperManager.GetAllAsync<rptCollectionVM>("terminusReports", param);

                    table = (DataTable)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(rptCollectionVM), (typeof(DataTable)));
                }

                //var newpath = @"d:\"+ reportType +"" + "_" + DateTime.Today.ToString("MM/dd/yyyy").Replace("/","").Replace(" ","").Replace(":","") + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + ".xlsx";

                //string sFileName = @"" + reportType + ".xlsx";

                if (table != null && table.Rows.Count>0)
                {
                    var tmpPath = Path.Combine(_env.WebRootPath, "Uploaded");
                    if (!Directory.Exists(tmpPath))
                    {
                        Directory.CreateDirectory(tmpPath);
                    }


                    var fileName = $"{DateTime.Today.ToString("yyyyMMdd")}{Guid.NewGuid().ToString()}.xlsx";

                    FileInfo file = new FileInfo(Path.Combine(tmpPath, fileName));

                    using (var excelPack = new ExcelPackage(file))
                    {
                        var ws = excelPack.Workbook.Worksheets.Add("Sheet1");
                        int rowIndex = 1;
                        int colnameindex = 1;

                        foreach (DataColumn col in table.Columns)
                        {
                            if (colnameindex != 1)
                            {
                                ws.Cells[rowIndex, colnameindex -1].Value = col.ColumnName.ToString();
                                
                            }
                            colnameindex += 1;

                        }

                        rowIndex = 2;

                        foreach (DataRow row in table.Rows)
                        {
                            int colIndex = 1;
                            foreach (DataColumn col in table.Columns)
                            {
                                if (colIndex != 1)
                                {
                                    ws.Cells[rowIndex, colIndex - 1].Value = row[col.ColumnName].ToString().Replace("1/1/0001 12:00:00 AM", "");

                                }
                                colIndex += 1;
                            }
                            rowIndex += 1;

                        }


                        //ws.Cells.LoadFromDataTable(table, true, OfficeOpenXml.Table.TableStyles.Light8);
                        excelPack.Save();

                    }

                    _path = $"/Uploaded/{fileName}"; //file.FullName;

                }



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
