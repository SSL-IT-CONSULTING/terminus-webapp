﻿using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;
using terminus.dataaccess;
using terminus.shared.models;


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
        [Inject]
        public ISessionStorageService _sessionStorageService { get; set; }
        public string accountId { get; set; }

        //public List<ReferenceViewModal> referenceViewModal { get; set; }

        public bool IsDataLoaded { get; set; }


        public ReportParameterViewModel reportParameterViewModel { get; set; }
        public bool DataLoaded { get; set; }
        public string ErrorMessage { get; set; }
        public string CompanyId { get; set; }
        public string UserName { get; set; }

        protected void NavigateToList()
        {
            NavigationManager.NavigateTo($"/reportslist/{reportParameterViewModel.dateFrom.ToString("yyyy-MM-dd")}|{reportParameterViewModel.dateTo.ToString("yyyy-MM-dd")}|{reportParameterViewModel.ReportType}");
        }


        protected void CreateArticle()
        {
            NavigationManager.NavigateTo($"/reportslist/{reportParameterViewModel.dateFrom.ToString("yyyy-MM-dd")}|{reportParameterViewModel.dateTo.ToString("yyyy-MM-dd")}|{reportParameterViewModel.ReportType}");
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

                UserName = await _sessionStorageService.GetItemAsync<string>("UserName");
                CompanyId = await _sessionStorageService.GetItemAsync<string>("CompanyId");



                //CompanyId = "ASRC";

                DataLoaded = false;
                ErrorMessage = string.Empty;
                //companyId = "ASRC";
                // var sqlcommand = $"exec ASRCReportsDtls '{companyId.Replace("'", "''")}'";
                // var sqlcommand = $"exec ASRCReportsDtls '{companyId.Replace("'", "''")}'";
                var param = new Dapper.DynamicParameters();
                param.Add("companyId", CompanyId, System.Data.DbType.String);

                reportParameterViewModel = new ReportParameterViewModel();
                reportParameterViewModel.dateFrom = DateTime.Parse("01/01/" + DateTime.Today.Year.ToString());
                reportParameterViewModel.dateTo = DateTime.Today;

                //reportParameterViewModel.ReferenceVM = await dapperManager.GetAllAsync<ReferenceViewModal>("ASRCReportsDtls", param);
                if (CompanyId == "ASRC")
                {
                    reportParameterViewModel.ReferenceVM = dapperManager.GetAll<ReferenceViewModal>("ASRCReportsDtls", param);
                }

                if (CompanyId == "ADBCA")
                {
                    reportParameterViewModel.ReferenceVM = dapperManager.GetAll<ReferenceViewModal>("ADBCAReportsDtls", param);
                }

                if (CompanyId == "APMI")
                {
                    reportParameterViewModel.ReferenceVM = dapperManager.GetAll<ReferenceViewModal>("APMIReportsDtls", param);
                }
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
