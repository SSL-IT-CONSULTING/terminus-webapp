using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using terminus.dataaccess;
using terminus.shared.models;


namespace terminus_webapp.Pages
{
    public class JournalDetailsBase:ComponentBase
    {
        [Inject]
        private IJSRuntime JSRuntime { get; set; }

        [Inject]
        public IWebHostEnvironment _env { get; set; }

        [Inject]
        public AppDBContext appDBContext { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }


        [Inject]
        public DapperManager dapperManager { get; set; }

        [Inject]
        public ISessionStorageService _sessionStorageService { get; set; }

        [Parameter]
        public string accountId { get; set; }
        public string CompanyId { get; set; }
        public string UserName { get; set; }

        public bool IsDataLoaded { get; set; }
        public string ErrorMessage { get; set; }
        public List<JournalDetailViewModel> JournalEntries { get; set; }

        public decimal? totalDR { get; set; }
        public decimal? totalCR { get; set; }

        public decimal? totalBalance { get; set; }

        public string HeaderTitle { get; set; }
        public void NavigateToList()
        {
            NavigationManager.NavigateTo("/journalentrylist");
        }
        protected override async Task OnInitializedAsync()
        {
            try
            {
                IsDataLoaded = false;
                ErrorMessage = string.Empty;

                UserName = await _sessionStorageService.GetItemAsync<string>("UserName");
                CompanyId = await _sessionStorageService.GetItemAsync<string>("CompanyId");

                var param = new Dapper.DynamicParameters();
                param.Add("accountId", Guid.Parse(accountId), System.Data.DbType.Guid);


                JournalEntries = await dapperManager.GetAllAsync<JournalDetailViewModel>("spGetGLTransactions", param);

                var glAccount = await appDBContext.GLAccounts.Where(a => a.accountId.Equals(Guid.Parse(accountId))).FirstOrDefaultAsync();

                if(glAccount!=null)
                    HeaderTitle = $"{glAccount.accountCode} - {glAccount.accountDesc}";
                
                totalDR = JournalEntries.Sum(a => a.drAmt);
                totalCR = JournalEntries.Sum(a => a.crAmt);

                if(totalDR.HasValue || totalCR.HasValue)
                {
                    totalBalance = (totalDR.HasValue ? totalDR.Value : 0) - (totalCR.HasValue ? totalCR.Value : 0);
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

        protected async Task DownloadReport()
        {
            try
            {

                IsDataLoaded = false;
                ErrorMessage = string.Empty;

                if (JournalEntries!=null)
                {
                    var fileName = $"{DateTime.Today.ToString("yyyyMMdd")}{Guid.NewGuid().ToString()}.xlsx";

                    FileInfo file = new FileInfo(Path.Combine(_env.WebRootPath, "Uploaded", fileName));
                    //FileInfo filePath = new FileInfo(_filepath);
                    using (var excelPack = new ExcelPackage(file))
                    {
                        var ws = excelPack.Workbook.Worksheets.Add("sheet1");

                        var rng = ws.Cells[1, 1, 1, 8];
                        rng.Merge=true;
                        rng.Style.Font.Bold = true;
                        rng.Style.Font.Size = 32;
                        rng.Value = HeaderTitle;

                        ws.Cells[3, 1, 3, 8].Style.Font.Bold=true;

                        ws.Cells[3, 1].Value = "Document Id";
                        ws.Cells[3, 2].Value = "Posting Date";
                        ws.Cells[3, 3].Value = "Entry Date";
                        ws.Cells[3, 4].Value = "DR (Amt)";
                        ws.Cells[3, 5].Value = "CR (Amt)";
                        ws.Cells[3, 6].Value = "Description";
                        ws.Cells[3, 7].Value = "Reference";
                        ws.Cells[3, 8].Value = "Remarks";

                        var rowNo = 4;
                        var accountingFormat = "_-* #,##0.00_-;-* #,##0.00_-;_-* \"-\"??_-;_-@_-";

                        foreach (var record in JournalEntries)
                        {
                            ws.Cells[rowNo, 1].Value = record.documentId;

                            ws.Cells[rowNo, 2].Style.Numberformat.Format = "yyyy-MM-dd";
                            ws.Cells[rowNo, 2].Value = record.postingDate;

                            ws.Cells[rowNo, 3].Style.Numberformat.Format = "yyyy-MM-dd";
                            ws.Cells[rowNo, 3].Value = record.transactionDate;

                            ws.Cells[rowNo, 4].Style.Numberformat.Format = accountingFormat;
                            ws.Cells[rowNo, 4].Value = record.drAmt;

                            ws.Cells[rowNo, 5].Style.Numberformat.Format = accountingFormat;
                            ws.Cells[rowNo, 5].Value = record.crAmt;

                            ws.Cells[rowNo, 6].Value = record.description;
                            ws.Cells[rowNo, 7].Value = record.reference;
                            ws.Cells[rowNo, 8].Value = record.remarks;
                            rowNo++;
                        }

                        ws.Cells[rowNo, 4].Style.Font.Bold = true;
                        ws.Cells[rowNo, 4].Style.Numberformat.Format = accountingFormat;
                        ws.Cells[rowNo,4].Formula = $"=SUM({  ws.Cells[4, 4].Address } : { ws.Cells[(rowNo-1), 4].Address  })";

                        ws.Cells[rowNo, 5].Style.Font.Bold = true;
                        ws.Cells[rowNo, 5].Style.Numberformat.Format = accountingFormat;
                        ws.Cells[rowNo, 5].Formula = $"=SUM({  ws.Cells[4, 5].Address } : { ws.Cells[(rowNo - 1), 5].Address  })";
                        rowNo++;
                        ws.Cells[rowNo, 3].Style.Font.Bold = true;
                        ws.Cells[rowNo, 3].Value = "Balance";
                        ws.Cells[rowNo, 4].Style.Font.Bold = true;
                        ws.Cells[rowNo, 4].Style.Numberformat.Format = accountingFormat;
                        ws.Cells[rowNo, 4].Formula = $"=SUM({  ws.Cells[(rowNo-1), 4].Address } : { ws.Cells[(rowNo - 1), 5].Address  })";

                        ws.Cells[1, 1, rowNo, 8].AutoFitColumns();



                        //ws.Cells.LoadFromDataTable(table, true, OfficeOpenXml.Table.TableStyles.Light8);
                        excelPack.Save();


                        var _path = $"/Uploaded/{fileName}";

                        await JSRuntime.InvokeAsync<object>("FileSaveAs","JournalDetails.xlsx",_path);
                        //await JSRuntime.InvokeAsync<object>("TestScript", _path);
                    }
                }
            }
            catch(Exception ex)
            {
                ErrorMessage = ex.Message;
            }
            finally
            {
                IsDataLoaded = true;
            }

        }


    }
}
