﻿using Blazored.SessionStorage;
using Dapper;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using terminus.dataaccess;
using terminus.shared.models;

namespace terminus_webapp.Pages
{
    public class ExpenseEntryBase:ComponentBase
    {
        [Inject]
        public AppDBContext appDBContext { get; set; }

        [Inject]
        public DapperManager dapperManager { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public ISessionStorageService _sessionStorageService { get; set; }

        [Parameter]
        public string expenseId { get; set; }

        public ExpenseViewModel expense { get; set; }

        public bool IsDataLoaded { get; set; }
        public bool IsViewOnly { get; set; }

        public string ErrorMessage = string.Empty;

        public string CompanyId { get; set; }
        public string UserName { get; set; }

        private string _VendorId;
        public string VendorId { get { return _VendorId; } 
            set {
                _VendorId = value;
                this.expense.vendorId = _VendorId;
                VendorSelected(_VendorId);
            } }

        public bool IsVatRegistered { get; set; }
        public Guid? InputVatAccountId { get; set; }
        public string InputVatAccountCode { get; set; }
        public string InputVatAccountName { get; set; }


        protected decimal CalculateBeforeVat(decimal amount)
        {
            if (amount == 0m)
                return 0m;

            return Math.Round(amount / 1.12m, 2);
        }

        protected decimal CalculateVat(decimal amount)
        {
            if (amount == 0m)
                return 0m;

            return amount - Math.Round(amount / 1.12m, 2);
        }


        public void VendorSelected(string vendorId)
        {
            var id = vendorId;
            var vendor = this.expense.vendors.Where(v => v.vendorId.Equals(id, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

            if(vendor!=null && vendor.inputVatAccountId.HasValue && vendor.isVatRegistered)
            {
                InputVatAccountId = vendor.inputVatAccountId;
                InputVatAccountCode = vendor.inputVatAccount.accountCode;
                InputVatAccountName = vendor.inputVatAccount.accountDesc;
            }
            else
            {
                InputVatAccountId =null;
                InputVatAccountCode = string.Empty;
                InputVatAccountName = string.Empty;
            }

        }

        protected decimal GetAmount(string cashOrCheck, decimal amount, decimal checkAmount)
        {
            if (cashOrCheck.Equals("0"))
                return amount;
            else
                return checkAmount;
        }

        protected string GetAccountDesc(string account)
        {
            var data = expense.expenseAccounts.Where(a => a.accountId.ToString().Equals(account, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            if (data != null)
                return $"{data.accountCode} - {data.accountDesc}";

            return string.Empty;
        }

        protected async Task HandleValidSubmit()
        {
            if (string.IsNullOrEmpty(expense.id))
            {
                var vatAccount = await appDBContext.GLAccounts.Where(a => a.outputVatAccount).FirstOrDefaultAsync();

                var r = new Expense();
                r.id = Guid.NewGuid();
                r.transactionDate = expense.transactionDate;
                r.dueDate = expense.dueDate;
                r.description = expense.description;
                r.account = expense.expenseAccounts.Where(a => a.accountId.Equals(Guid.Parse(expense.glAccountId))).FirstOrDefault();
                r.cashAccount = await appDBContext.GLAccounts.Where(a => a.accountId.Equals(Guid.Parse(expense.cashAccountId))).FirstOrDefaultAsync();
                r.documentId = expense.documentId;

                r.amount = expense.amount;
                r.createDate = DateTime.Now;
                r.createdBy = UserName;
                r.receiptNo = expense.receiptNo;
                r.reference = expense.reference;
                r.remarks = $"{r.account.accountCode} {r.account.accountDesc}";

                r.companyId = CompanyId;
                r.cashOrCheck = expense.cashOrCheck;
                r.vendorId = expense.vendorId;
                r.vendorOther = expense.vendorOther;

                if (r.cashOrCheck.Equals("1"))
                {
                    r.checkDetails = new CheckDetails()
                    {
                        amount = expense.checkAmount,
                        bankName = expense.bankName,
                        branch = expense.branch,
                        checkDate = expense.checkDate.HasValue ? expense.checkDate.Value : DateTime.MinValue,
                        checkDetailId = Guid.NewGuid(),
                        checkNo = expense.checkNo,
                        releaseDate = expense.checkReleaseDate
                    };
                }

               
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

                var jeHdr = new JournalEntryHdr() { documentId = documentId, 
                                                    createDate = DateTime.Now, 
                                                    createdBy = UserName, 
                                                    id = Guid.NewGuid(),source = "expense", 
                                                    sourceId = r.id.ToString(), 
                                                    companyId=CompanyId, 
                                                    postingDate = r.transactionDate,
                                                    transactionDate = r.transactionDate
                };
                jeHdr.description = r.remarks;
              
                var amount = r.cashOrCheck.Equals("1") ? r.checkDetails.amount : r.amount;
                r.beforeTax = amount;

                var jeList = new List<JournalEntryDtl>()
                {
                    new JournalEntryDtl()
                    {
                    id = Guid.NewGuid().ToString(),
                    createDate = DateTime.Now,
                    createdBy = UserName,
                    lineNumber=0,
                    amount = InputVatAccountId.HasValue? CalculateBeforeVat(amount):amount,
                    type ="D",
                    account = r.account
                    }
                };

                if(InputVatAccountId.HasValue)
                {
                    var beforeVat = CalculateBeforeVat(amount);
                    var taxAmount = CalculateVat(amount);
                    r.beforeTax=  beforeVat;
                    r.taxAmount = taxAmount;

                    jeList.Add(new JournalEntryDtl()
                    {
                        id = Guid.NewGuid().ToString(),
                        createDate = DateTime.Now,
                        createdBy = UserName,
                        lineNumber = 1,
                        amount = taxAmount,
                        type = "D",
                        accountId = InputVatAccountId.Value
                    });
                }

                jeList.Add(new JournalEntryDtl()
                {
                    id = Guid.NewGuid().ToString(),
                    createDate = DateTime.Now,
                    createdBy = UserName,
                    lineNumber = 2,
                    amount = amount,
                    type = "C",
                    account = r.cashAccount
                });

                jeHdr.JournalDetails = jeList.AsEnumerable();
                r.journalEntry = jeHdr;

                appDBContext.Expenses.Add(r);
                appDBContext.JournalEntriesHdr.Add(jeHdr);
                await appDBContext.SaveChangesAsync();

                StateHasChanged();

                NavigateToList();
            }
        }

        protected async Task HandleInvalidSubmit()
        {

        }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                UserName = await _sessionStorageService.GetItemAsync<string>("UserName");
                CompanyId = await _sessionStorageService.GetItemAsync<string>("CompanyId");

                IsDataLoaded = false;
                IsViewOnly = false;
                ErrorMessage = string.Empty;

                if (string.IsNullOrEmpty(expenseId))
                {
                    DynamicParameters dynamicParameters = new DynamicParameters();
                    var IdKey = $"EXP{DateTime.Today.ToString("yyyyMM")}";
                    dynamicParameters.Add("IdKey", IdKey);
                    dynamicParameters.Add("Format", "000000");
                    dynamicParameters.Add("CompanyId", CompanyId);

                    var documentIdTable = await dapperManager.GetAllAsync<DocumentIdTable>("spGetNextId", dynamicParameters);
                    var documentId = string.Empty;

                    if (documentIdTable.Any())
                        documentId = $"{IdKey}{documentIdTable.First().NextId.ToString(documentIdTable.First().Format)}";
                   
                    expense = new ExpenseViewModel();
                    expense.transactionDate = DateTime.Today;
                    expense.cashOrCheck = "1";
                    expense.documentId = documentId;
                }
                else
                {
                    IsViewOnly = true;
                    var id = Guid.Parse(expenseId);

                    var data = await appDBContext.Expenses
                        .Include(a => a.account)
                        .Include(a => a.cashAccount)
                        .Include(a => a.checkDetails)
                        .Include(a => a.vendor)
                        .ThenInclude(v => v.inputVatAccount)

                        .Where(r => r.id.Equals(id)).FirstOrDefaultAsync();

                    if(data.vendor.inputVatAccountId.HasValue)
                    {
                        IsVatRegistered = true;
                        InputVatAccountId = data.vendor.inputVatAccountId;
                        InputVatAccountCode = data.vendor.inputVatAccount.accountCode;
                        InputVatAccountName = data.vendor.inputVatAccount.accountDesc;
                    }

                    expense = new ExpenseViewModel()
                    {
                        id = data.id.ToString(),
                        documentId = data.documentId,
                        transactionDate = data.transactionDate,
                        dueDate = data.transactionDate,
                        glAccountId = data.accountId.ToString(),
                        glAccountCode = data.account.accountCode,
                        glAccountName = data.account.accountDesc,
                        amount = data.cashOrCheck.Equals("0") ? data.amount : data.checkDetails.amount,
                        cashAccountId = data.cashAccount.accountId.ToString(),
                        cashAccountCode = data.cashAccount.accountCode,
                        cashAccountName = data.cashAccount.accountDesc,
                        cashOrCheck = data.cashOrCheck,
                        checkReleaseDate = data.cashOrCheck.Equals("1") ? (DateTime?)data.checkDetails.releaseDate : null,
                        checkNo = data.cashOrCheck.Equals("1") ? data.checkDetails.checkNo : null,
                        checkAmount = data.cashOrCheck.Equals("1") ? data.checkDetails.amount : 0,
                        bankName = data.cashOrCheck.Equals("1") ? data.checkDetails.bankName : "",
                        branch = data.cashOrCheck.Equals("1") ? data.checkDetails.branch : "",
                        checkDate = data.cashOrCheck.Equals("1") ? (DateTime?)data.checkDetails.checkDate : null,
                        vendorId = data.vendorId,
                        vendorOther = data.vendorOther,
                        vendorName = data.vendor.vendorName,
                        reference = data.reference,
                        receiptNo = data.receiptNo
                    };
                }

                expense.expenseAccounts = await appDBContext.GLAccounts.Where(a => (a.expense || a.cashAccount) && a.companyId.Equals(CompanyId)).ToListAsync();
                expense.vendors = await appDBContext.Vendors.Where(a=>a.companyId.Equals(CompanyId))
                                                    .Include(a=>a.inputVatAccount)
                                                    .OrderBy(a => a.rowOrder).ToListAsync();

            }
            catch(Exception ex)
            {
                ErrorMessage = ex.ToString();
            }
            finally
            {
                IsDataLoaded = true;
            }
            
        }

        public void NavigateToList()
        {
            NavigationManager.NavigateTo("/expenselist");
        }


    }
}
