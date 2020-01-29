using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using terminus.shared.models;
using terminus_webapp.Data;
using OfficeOpenXml;

namespace terminus_webapp.Pages
{
    public class ReportsListBase : ComponentBase
    {

        [Inject]
        public AppDBContext appDBContext { get; set; }

        [Inject]
        public DapperManager dapperManager { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        public List<ReferenceViewModal> ReferenceViewModal { get; set; }


        public List<rptPropertypeInventoryMV> rptPropertypeInventoryMV { get; set; }
        
        public List<rptRentalIncomeVM> rptRentalIncomeVM { get; set; }

        public List<rptOtherIncomeDetailVM> rptOtherIncomeDetailVM { get; set; }
        public List<rptExpensesVM> rptExpensesVM { get; set; }

      
        public List<rptNetIncomeSummaryVM> rptNetIncomeSummaryVM { get; set; }


        public List<rptNetIncomeVM> rptNetIncomeVM { get; set; }


        public List<rptTrialBalanceVM> rptTrialBalanceVM { get; set; }
        

   



        [Parameter]
        public string reportParams { get; set; }

        //[Parameter]
        public DateTime asOfDate { get; set; }
        //[Parameter]
        public string reportType { get; set; }

        public bool DataLoaded { get; set; }
        public string ErrorMessage { get; set; }

        //public string asofdate { get; set; }
        //public string reporttype { get; set; }


        protected override async Task OnInitializedAsync()
        {
            try
            {
                DataLoaded = false;
                ErrorMessage = string.Empty;

                var p = reportParams.Split("|");

                asOfDate = DateTime.Parse(p[0]);
                reportType = p[1];

                // var sqlcommand = $"exec ASRCReportsDtls '{companyId.Replace("'", "''")}'";
                // var sqlcommand = $"exec ASRCReportsDtls '{companyId.Replace("'", "''")}'";
                var param = new Dapper.DynamicParameters();
                param.Add("AsofDate", asOfDate, System.Data.DbType.DateTime);
                param.Add("ReportType", reportType, System.Data.DbType.String);



                string path; 
                DataTable table = new DataTable();


                if (reportType == "PropertyInventory")
                {
                    rptPropertypeInventoryMV = await
                        dapperManager.GetAllAsync<rptPropertypeInventoryMV>("ASRCReports", param);
                    
                    
                    path = @"d:\PropertyInventory"+ "_" + DateTime.Today.ToString() +".xlsx";

                    table = (DataTable)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(rptPropertypeInventoryMV), (typeof(DataTable)));


                }

                if (reportType == "RentalIncomeDetails")
                {
                    rptRentalIncomeVM = await
                        dapperManager.GetAllAsync<rptRentalIncomeVM>("ASRCReports", param);

                    path = @"d:\RentalIncomeDetails" + "_" + DateTime.Today.ToString() + ".xlsx";
                    table = (DataTable)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(rptRentalIncomeVM), (typeof(DataTable)));
                }

                if (reportType == "OtherIncomeDetails")
                {
                    rptOtherIncomeDetailVM = await
                        dapperManager.GetAllAsync<rptOtherIncomeDetailVM>("ASRCReports", param);

                    path = @"d:\OtherIncomeDetails" + "_" + DateTime.Today.ToString() + ".xlsx";


                    table = (DataTable)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(rptOtherIncomeDetailVM), (typeof(DataTable)));
                }

                if (reportType == "Expenses")
                {
                    rptExpensesVM = await
                        dapperManager.GetAllAsync<rptExpensesVM>("ASRCReports", param);


                    path = @"d:\Expenses" + "_" + DateTime.Today.ToString() + ".xlsx";

                    table = (DataTable)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(rptExpensesVM), (typeof(DataTable)));
                }

                if (reportType == "NetIncomeSummary")
                {
                    rptNetIncomeSummaryVM = await
                        dapperManager.GetAllAsync<rptNetIncomeSummaryVM>("ASRCReports", param);


                    path = @"d:\NetIncomeSummary" + "_" + DateTime.Today.ToString() + ".xlsx";

                    table = (DataTable)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(rptNetIncomeSummaryVM), (typeof(DataTable)));
                }

                if (reportType == "TrialBalance")
                {
                    rptTrialBalanceVM = await
                        dapperManager.GetAllAsync<rptTrialBalanceVM>("ASRCReports", param);

                    table = (DataTable)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(rptTrialBalanceVM), (typeof(DataTable)));
                }


                if (reportType == "NetIncome")
                {
                    rptNetIncomeVM = await
                        dapperManager.GetAllAsync<rptNetIncomeVM>("ASRCReports", param);


                    


                    table = (DataTable)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(rptNetIncomeVM), (typeof(DataTable)));
                }




                var newpath = @"d:\"+ reportType +"" + "_" + DateTime.Today.ToString().Replace("/","").Replace(" ","").Replace(":","") + ".xlsx";

             


                FileInfo filePath = new FileInfo(newpath);
                using (var excelPack = new ExcelPackage(filePath))
                {
                    var ws = excelPack.Workbook.Worksheets.Add("WriteTest");
                    ws.Cells.LoadFromDataTable(table, true, OfficeOpenXml.Table.TableStyles.Light8);
                    excelPack.Save();


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
