using Blazored.SessionStorage;
using Dapper;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using terminus.shared.models;
using terminus_webapp.Components;
using terminus_webapp.Data;

namespace terminus_webapp.Pages
{
    public class JournalEntryBase:ComponentBase
    {
        [Inject]
        public AppDBContext appDBContext { get; set; }

        [Inject]
        public DapperManager dapperManager { get; set; }

        
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public ISessionStorageService _sessionStorageService { get; set; }

        public string CompanyId { get; set; }
        public string UserName { get; set; }

        public bool IsDataLoaded { get; set; }
        public bool IsSavingDone { get; set; }

        public string ErrorMessage { get; set; }

        public decimal TotalDR { get; set; }
        public decimal TotalCR { get; set; }
        public decimal TotalBalance { get; set; }

        [Parameter]
        public string JEId { get; set; }

        public bool IsViewOnly { get; set; }

        public JournalEntryViewModel journalEntry { get; set; }

        protected JournalEntryDetail JournalEntryDetail { get; set; }

        private JournalEntryDtlViewModel CopyDetails(JournalEntryDtlViewModel source, JournalEntryDtlViewModel dest)
        {
            var dtl = new JournalEntryDtlViewModel();
            string id = string.IsNullOrEmpty(source.id)?Guid.NewGuid().ToString():source.id;

            dtl.id = id;
            dtl.remarks = source.remarks;
            dtl.description = source.description;
            dtl.type = source.type;
            dtl.amount = source.amount;
            dtl.accountId = source.accountId;
            dtl.accountCode = source.accountCode;
            dtl.accountName = source.accountName;
            dtl.reference = source.reference;
            
            return dtl;
        }
        public void JournalEntryDetail_OnSave()
        {
            if(string.IsNullOrEmpty(JournalEntryDetail.JournalEntryDtl.id))
            {
                this.journalEntry.journalEntryDtls.Add(CopyDetails(JournalEntryDetail.JournalEntryDtl,null));
            }
            else
            {
                var dest = this.journalEntry.journalEntryDtls.Where(a => a.id.Equals(JournalEntryDetail.JournalEntryDtl.id)).FirstOrDefault();
                CopyDetails(JournalEntryDetail.JournalEntryDtl, dest);
            }

            TotalDR = journalEntry.journalEntryDtls.Where(a => a.type.Equals("D", StringComparison.OrdinalIgnoreCase)).Sum(a => a.amount);
            TotalCR = journalEntry.journalEntryDtls.Where(a => a.type.Equals("C", StringComparison.OrdinalIgnoreCase)).Sum(a => a.amount);
            TotalBalance = TotalDR - TotalCR;

            JournalEntryDetail.CloseDialog();
            StateHasChanged();
        }

        public async void JournalEntryDetail_OnCancel()
        {
            JournalEntryDetail.CloseDialog();
            StateHasChanged();
        }


        protected async Task HandleValidSubmit()
        {
            ErrorMessage = string.Empty;

            if (!journalEntry.journalEntryDtls.Any())
            {
                ErrorMessage = "There is no entry.";
                StateHasChanged();
                return;
            }

            if (TotalBalance != 0)
            {
                ErrorMessage = "Total Balance must be 0.";
                StateHasChanged();
                return;
            }

            var id = Guid.NewGuid();

            var jeHdr = new JournalEntryHdr() { createDate = DateTime.Now, createdBy = UserName, id = id, source = "JE", sourceId = id.ToString(), companyId = CompanyId };
            jeHdr.documentId = journalEntry.documentId;
            jeHdr.description = journalEntry.description; 
            jeHdr.postingDate = journalEntry.postingDate;
            jeHdr.transactionDate = journalEntry.transactionDate;
            jeHdr.reference = journalEntry.reference;
            jeHdr.remarks = journalEntry.remarks;
            jeHdr.companyId = CompanyId;
            jeHdr.createdBy = UserName;
            jeHdr.createDate = DateTime.Now;

            var jeList = new List<JournalEntryDtl>();

            int lineNo = 1; 

            foreach (var dtl in journalEntry.journalEntryDtls)
            {
                jeList.Add(new JournalEntryDtl()
                {
                    id = Guid.NewGuid().ToString(),
                    createDate = DateTime.Now,
                    createdBy = jeHdr.createdBy, 
                    lineNumber = lineNo,
                    amount = dtl.amount,
                    type = dtl.type,
                    accountId = Guid.Parse(dtl.accountId),
                    reference = dtl.reference,
                    description = dtl.description,
                    remarks = dtl.remarks
                });
                lineNo++;
            }

            jeHdr.JournalDetails = jeList.AsEnumerable();
          
            appDBContext.JournalEntriesHdr.Add(jeHdr);
            await appDBContext.SaveChangesAsync();


            StateHasChanged();
            NavigateToList();
        }

        protected async Task HandleInvalidSubmit()
        {

        }

        protected void NavigateToList()
        {
            NavigationManager.NavigateTo("/journalentrylist");
        }

        protected void AddDetail()
        {
            JournalEntryDetail.JournalEntryDtl = null;
            JournalEntryDetail.JournalEntryDtl = new JournalEntryDtlViewModel();
            JournalEntryDetail.OpenDialogBox();
            JournalEntryDetail.StateHasChange();
        }
        protected void EditJE(string id)
        {
            JournalEntryDetail.JournalEntryDtl = null;
            JournalEntryDetail.JournalEntryDtl = this.journalEntry.journalEntryDtls.Where(a => a.id.Equals(id)).FirstOrDefault();
            JournalEntryDetail.OpenDialogBox();
            JournalEntryDetail.StateHasChange();
        }

        protected void DeleteJE(string id)
        {
            var je = this.journalEntry.journalEntryDtls.Where(a => a.id.Equals(id)).FirstOrDefault();
            this.journalEntry.journalEntryDtls.Remove(je);
            JournalEntryDetail.StateHasChange();
        }
        protected override async Task OnInitializedAsync()
        {
            try
            {
                IsViewOnly = false;
                UserName = await _sessionStorageService.GetItemAsync<string>("UserName");
                CompanyId = await _sessionStorageService.GetItemAsync<string>("CompanyId");

                if(string.IsNullOrEmpty(JEId))
                {
                    DynamicParameters dynamicParameters = new DynamicParameters();
                    var IdKey = $"JE{DateTime.Today.ToString("yyyyMM")}";
                    dynamicParameters.Add("IdKey", IdKey);
                    dynamicParameters.Add("Format", "000000");
                    dynamicParameters.Add("CompanyId", CompanyId);

                    var documentIdTable = await dapperManager.GetAllAsync<DocumentIdTable>("spGetNextId", dynamicParameters);
                    var documentId = string.Empty;

                    if (documentIdTable.Any())
                    {
                        documentId = $"{IdKey}{documentIdTable.First().NextId.ToString(documentIdTable.First().Format)}";
                    }

                    journalEntry = new JournalEntryViewModel();
                    journalEntry.documentId = documentId;
                    journalEntry.transactionDate = DateTime.Today;
                    journalEntry.postingDate = DateTime.Today;
                    journalEntry.journalEntryDtls = new List<JournalEntryDtlViewModel>();
                }
                else
                {
                   IsViewOnly = true;
                   var je = await appDBContext
                                 .JournalEntriesHdr
                                 .Include(a=>a.JournalDetails)
                                 .ThenInclude(a=>a.account)
                                 .Where(a=>a.id.Equals(Guid.Parse(JEId))).FirstOrDefaultAsync();

                    journalEntry = new JournalEntryViewModel();
                    journalEntry.id = je.id;
                    journalEntry.documentId = je.documentId;
                    journalEntry.transactionDate =je.transactionDate;
                    journalEntry.postingDate = je.postingDate;
                    journalEntry.journalEntryDtls = je.JournalDetails.Select(a=> new JournalEntryDtlViewModel() { 
                    id = a.id,
                    accountId = a.accountId.ToString(),
                    accountCode = a.account.accountCode,
                    accountName = a.account.accountCode,
                    description = a.description,
                    amount = a.amount,
                    type = a.type
                    }).ToList();

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

            IsDataLoaded = true;


        }
    }
}
