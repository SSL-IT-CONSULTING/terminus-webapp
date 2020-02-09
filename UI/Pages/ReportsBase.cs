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
    public class ReportsBase : ComponentBase
    {


        [Inject]
        public AppDBContext appDBContext { get; set; }

        [Inject]
        public DapperManager dapperManager { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        public string accountId { get; set; }

        //public List<ReferenceViewModal> referenceViewModal { get; set; }

        public bool IsDataLoaded { get; set; }


        public ReportParameterViewModel reportParameterViewModel { get; set; }
        public bool DataLoaded { get; set; }
        public string ErrorMessage { get; set; }
        public string companyId { get; set; }

        protected void NavigateToList()
        {
            NavigationManager.NavigateTo($"/reportslist/{reportParameterViewModel.AsOfDate.ToString("yyyy-MM-dd")}|{reportParameterViewModel.ReportType}");
        }


        protected void CreateArticle()
        {
            NavigationManager.NavigateTo($"/reportslist/{reportParameterViewModel.AsOfDate.ToString("yyyy-MM-dd")}|{reportParameterViewModel.ReportType}");
        }

        //protected async Task HandleValidSubmit()
        //{

        //}

        //protected async Task HandleInvalidSubmit()
        //{

        //}

        protected override async Task OnInitializedAsync()
        {

            try
            {
                companyId = "ASRC";

                DataLoaded = false;
                ErrorMessage = string.Empty;
                companyId = "ASRC";
                // var sqlcommand = $"exec ASRCReportsDtls '{companyId.Replace("'", "''")}'";
                // var sqlcommand = $"exec ASRCReportsDtls '{companyId.Replace("'", "''")}'";
                var param = new Dapper.DynamicParameters();
                param.Add("companyId", companyId, System.Data.DbType.String);

                reportParameterViewModel = new ReportParameterViewModel();
                reportParameterViewModel.AsOfDate = DateTime.Today;

                reportParameterViewModel.ReferenceVM = await dapperManager.GetAllAsync<ReferenceViewModal>("ASRCReportsDtls", param);
                //dapperManager.GetAllAsync<ReferenceViewModal>("ASRCReportsDtls", param);

                //reportParameterViewModel.ReferenceVM = await
                //     dapperManager.GetAllAsync<ReferenceViewModal>("ASRCReportsDtls", param);
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
            finally
            {
                IsDataLoaded = true;
            }

            IsDataLoaded = true;


        }










        //protected override async Task OnInitializedAsync()
        //{
        //    try
        //    {
        //        DataLoaded = false;
        //        ErrorMessage = string.Empty;
        //        companyId = "ASRC";
        //        // var sqlcommand = $"exec ASRCReportsDtls '{companyId.Replace("'", "''")}'";
        //        // var sqlcommand = $"exec ASRCReportsDtls '{companyId.Replace("'", "''")}'";
        //        var param = new Dapper.DynamicParameters();
        //        param.Add("companyId", companyId, System.Data.DbType.String);


        //        ReferenceVM = await
        //            dapperManager.GetAllAsync<ReferenceViewModal>("ASRCReportsDtls", param);

        //        //var path = @"d:\test.xlsx";


        //        //DataTable table = (DataTable)JsonConvert.DeserializeObject(JsonConvert.SerializeObject(ReferenceViewModal), (typeof(DataTable)));
        //        //FileInfo filePath = new FileInfo(path);
        //        //using (var excelPack = new ExcelPackage(filePath))
        //        //{
        //        //    var ws = excelPack.Workbook.Worksheets.Add("WriteTest");
        //        //    ws.Cells.LoadFromDataTable(table, true, OfficeOpenXml.Table.TableStyles.Light8);
        //        //    excelPack.Save();
        //        //}


        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorMessage = ex.Message;
        //    }
        //    finally
        //    {
        //        DataLoaded = true;
        //    }
        //}

    }
    }
