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
    public class ExpenseEntryBase:ComponentBase
    {
        [Inject]
        public AppDBContext appDBContext { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Parameter]
        public string expenseId { get; set; }

        public ExpenseViewModel expense { get; set; }

        public bool IsDataLoaded { get; set; }
        public bool IsViewOnly { get; set; }

        protected async Task HandleValidSubmit()
        {
            if (string.IsNullOrEmpty(expense.id))
            {
                var vatAccount = await appDBContext.GLAccounts.Where(a => a.outputVatAccount).FirstOrDefaultAsync();

                var company = await appDBContext.Companies.Where(a => a.companyId.Equals("ASRC")).FirstOrDefaultAsync();

                var r = new Expense();
                r.id = Guid.NewGuid();
                r.transactionDate = expense.transactionDate;
                r.dueDate = expense.dueDate;
                r.description = expense.description;
                r.account = expense.expenseAccounts.Where(a => a.accountId.Equals(Guid.Parse(expense.glAccountId))).FirstOrDefault();
                r.cashAccount = await appDBContext.GLAccounts.Where(a => a.accountId.Equals(Guid.Parse(expense.cashAccountId))).FirstOrDefaultAsync();
            
                r.amount = expense.amount;
                r.createDate = DateTime.Now;
                r.createdBy = "testadmin";
                r.receiptNo = expense.receiptNo;
                r.reference = expense.reference;
                r.remarks = $"{r.account.accountCode} {r.account.accountDesc}";

                r.company = company;
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
                        checkDetailId = Guid.NewGuid()
                    };
                }

                appDBContext.Expenses.Add(r);

                var jeHdr = new JournalEntryHdr() { createDate = DateTime.Now, createdBy = "testadmin", id = Guid.NewGuid(),source = "expense", sourceId = r.id.ToString() };
                jeHdr.description = r.remarks;
                jeHdr.company = company;

                var amount = r.cashOrCheck.Equals("1") ? r.checkDetails.amount : r.amount;

                //var vat = Math.Round(amount * 0.12m, 2);
               // r.taxAmount = vat;
                var jeList = new List<JournalEntryDtl>()
                {
                    new JournalEntryDtl()
                    {
                    id = Guid.NewGuid().ToString(),
                    createDate = DateTime.Now,
                    createdBy = "testadmin",
                    lineNumber=0,
                    amount = amount,
                    type ="C",
                    account = r.account
                    },
                    //new JournalEntryDtl()
                    //{
                    //id = Guid.NewGuid().ToString(),
                    //createDate = DateTime.Now,
                    //createdBy = "testadmin",
                    //lineNumber=1,
                    //amount = vat,
                    //type ="C",
                    //account = vatAccount
                    //},
                    new JournalEntryDtl()
                    {
                    id = Guid.NewGuid().ToString(),
                    createDate = DateTime.Now,
                    createdBy = "testadmin",
                    lineNumber=2,
                    amount = amount,
                    type ="D",
                    account = r.cashAccount
                    },
                };

                jeHdr.JournalDetails = jeList.AsEnumerable();
                r.journalEntry = jeHdr;

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
            IsDataLoaded = false;
            IsViewOnly = false;

            if (string.IsNullOrEmpty(expenseId))
            {
                expense = new ExpenseViewModel();
                expense.transactionDate = DateTime.Today;
                expense.cashOrCheck = "0";
            }
            else
            {
                IsViewOnly = true;
                var id = Guid.Parse(expenseId);

                var data = await appDBContext.Expenses
                    .Include(a => a.account)
                    .Include(a => a.cashAccount)
                    .Include(a => a.checkDetails)
                    .Include(a=>a.vendor)
                    .Where(r => r.id.Equals(id)).FirstOrDefaultAsync();

                expense = new ExpenseViewModel()
                {
                    id = data.id.ToString(),
                    transactionDate = data.transactionDate,
                    dueDate = data.transactionDate,
                    glAccountCode = data.account.accountCode,
                    glAccountName = data.account.accountDesc,
                    amount = data.cashOrCheck.Equals("0") ? data.amount : data.checkDetails.amount,
                    cashAccountId = data.cashAccount.accountId.ToString(),
                    cashAccountCode = data.cashAccount.accountCode,
                    cashAccountName = data.cashAccount.accountDesc,
                    cashOrCheck = data.cashOrCheck,
                    checkAmount = data.cashOrCheck.Equals("1") ? data.checkDetails.amount : 0,
                    bankName = data.cashOrCheck.Equals("1") ? data.checkDetails.bankName : "",
                    branch = data.cashOrCheck.Equals("1") ? data.checkDetails.branch : "",
                    checkDate = data.cashOrCheck.Equals("1") ? (DateTime?)data.checkDetails.checkDate : null,
                    vendorId = data.vendor.vendorId,
                    vendorOther = data.vendorOther
                };
            }

            expense.expenseAccounts = await appDBContext.GLAccounts.Where(a => a.expense || a.cashAccount).ToListAsync();
            expense.vendors = await appDBContext.Vendors.OrderBy(a=>a.rowOrder).ToListAsync();

            IsDataLoaded = true;
        }

        public void NavigateToList()
        {
            NavigationManager.NavigateTo("/expenselist");
        }


    }
}
