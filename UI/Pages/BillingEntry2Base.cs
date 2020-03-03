using Blazored.SessionStorage;
using Dapper;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using terminus.shared.models;
using terminus_webapp.Common;
using terminus_webapp.Components;
using terminus_webapp.Data;

namespace terminus_webapp.Pages
{
    public class BillingEntry2Base : ComponentBase
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

        [Parameter]
        public EventCallback<bool> SaveEventCallback { get; set; }

        [Parameter]
        public EventCallback<bool> CancelEventCallback { get; set; }

        public bool IsDataLoaded { get; set; }
        public bool IsBillInitialized { get; set; }
        public string ErrorMessage { get; set; }
        public StringBuilder DeletedItems { get; set; }
        public bool ShowDialog { get; set; }
        public bool IsViewOnly { get; set; }
        public void CloseDialog()
        {
            ShowDialog = false;
            //StateHasChanged();
        }

        public void OpenDialogBox()
        {
            ShowDialog = true;
            //StateHasChanged();
        }

        public void StateHasChange()
        {
            //StateHasChanged();
        }

        public void InitParameters(string _propertyId, string _tenantId, DateTime? _dueDate, string _billingType, string _billingId)
        {
            this.propertyId = _propertyId;
            this.tenantId = _tenantId;
            this.dueDate = _dueDate.HasValue ? _dueDate.Value.ToString("yyyy-MM-dd") : DateTime.Today.ToString("yyyy-MM-dd");
            this.billingType = _billingType;
            this.billingId = _billingId;
        }

        [Parameter]
        public string propertyId { get; set; }

        [Parameter]
        public string tenantId { get; set; }

        [Parameter]
        public string dueDate { get; set; }

        [Parameter]
        public string billingId { get; set; }

        [Parameter]
        public string billingType { get; set; }

        private string _PropertyId;
        public string PropertyId
        {
            get { return _PropertyId; }
            set
            {
                _PropertyId = value;
                this.billingViewModel.propertyId = value;
                PopulateTenant();
            }
        }

        private void PopulateTenant()
        {
            try
            {
                var dueDate = billingViewModel.dateDue;
                var tenants = appDBContext.PropertyDirectory
                                                .Include(a => a.tenant)
                                                .Where(a => a.propertyId.Equals(PropertyId)
                                                && dueDate >= a.dateFrom && dueDate <= a.dateTo
                                                && a.companyId.Equals(CompanyId)
                                                ).ToList();


                if (!tenants.Any())
                {
                   
                    billingViewModel.tenantId = string.Empty;
                    billingViewModel.tenantName = string.Empty;
                }
                else
                {
                    if (!string.IsNullOrEmpty(billingViewModel.tenantId) &&
                        !tenants.Where(t => t.tenandId.Equals(billingViewModel.tenantId)).Any())
                    {
                        billingViewModel.tenantId = string.Empty;
                        billingViewModel.tenantName = string.Empty;
                    }

                    billingViewModel.tenantId = tenants.First().tenandId;
                    billingViewModel.tenantName = $"{tenants.First().tenant.lastName} {tenants.First().tenant.firstName}";

                }
                StateHasChanged();
            }
            catch (Exception ex)
            {

            }
        }

        public Billing billing { get; set; }

        public BillingViewModel billingViewModel { get; set; }

        protected BillingLineItemDetail BillingLineItemDetail { get; set; }

        public void BillingLineItemDetail_OnSave()
        {
            if (string.IsNullOrEmpty(BillingLineItemDetail.model.Id))
            {
                var lineNo = 0;
                
                if(billing.billingLineItems.Any())
                    lineNo = billing.billingLineItems.Max(a => a.lineNo)+1;

                billing.billingLineItems.Add(new BillingLineItem()
                {
                    Id = Guid.NewGuid(),
                    description = BillingLineItemDetail.model.description,
                    amount = BillingLineItemDetail.model.amount,
                    lineNo = lineNo
                });

                billing.totalAmount = billing.billingLineItems.Sum(a => a.amount);
                billing.balance = billing.totalAmount - billing.amountPaid;
                StateHasChanged();
            }
            else
            {
                var item = billing.billingLineItems.Where(a => a.Id.Equals(Guid.Parse(BillingLineItemDetail.model.Id))).FirstOrDefault();
                item.description = BillingLineItemDetail.model.description;
                item.amount = BillingLineItemDetail.model.amount;
                billing.totalAmount = billing.billingLineItems.Sum(a => a.amount);
                billing.balance = billing.totalAmount - billing.amountPaid;
                StateHasChanged();
            }
        }

        public void BillingLineItemDetail_OnCancel()
        {

        }

        protected async override Task OnInitializedAsync()
        {
            UserName = await _sessionStorageService.GetItemAsync<string>("UserName");
            CompanyId = await _sessionStorageService.GetItemAsync<string>("CompanyId");
            IsDataLoaded = false;
            if(string.IsNullOrEmpty(billingId))
            {
                IsBillInitialized = false;
                billingViewModel = new BillingViewModel();
                billingViewModel.dateDue = DateTime.Today;
                billingViewModel.billType = "MB";

                var pdlist = await appDBContext.PropertyDirectory.Include(a => a.property).ToListAsync();

                billingViewModel.properties = pdlist.GroupBy(a => a.propertyId)
    .Select(grp => grp.First().property).ToList();
                IsDataLoaded = true;
            }
            else
            {
                IsBillInitialized = true;
                await InitNewBilling();
            }
           
            //billing = new Billing();
            //billing.billingLineItems = new List<BillingLineItem>();

        }

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

        protected decimal CalculateWT(decimal amount)
        {
            if (amount == 0m)
                return 0m;

            return Math.Round((amount * .05m), 2);
        }

        public async Task InitNewBilling()
        {
            try
            {
                IsViewOnly = false;

                DeletedItems = new StringBuilder();

                IsDataLoaded = false;

                UserName = await _sessionStorageService.GetItemAsync<string>("UserName");
                CompanyId = await _sessionStorageService.GetItemAsync<string>("CompanyId");

                if (string.IsNullOrEmpty(billingId))
                {
                    DynamicParameters dynamicParameters = new DynamicParameters();

                    var IdKey = $"BILLING{DateTime.Today.ToString("yyyyMM")}";
                    dynamicParameters.Add("IdKey", IdKey);
                    dynamicParameters.Add("Format", "000000");
                    dynamicParameters.Add("CompanyId", CompanyId);

                    var documentIdTable = await dapperManager.GetAllAsync<DocumentIdTable>("spGetNextId", dynamicParameters);
                    var documentId = string.Empty;

                    if (documentIdTable.Any())
                    {
                        documentId = $"BILLING{DateTime.Today.ToString("yyyyMM")}{documentIdTable.First().NextId.ToString(documentIdTable.First().Format)}";
                    }

                    var dateDue = DateTime.Parse(dueDate);
                    var pdlist = await appDBContext.
                                        PropertyDirectory
                                        .Include(a => a.property)
                                        .Include(a => a.tenant)
                                        .Where(p => p.propertyId.Equals(propertyId)
                                        && p.tenandId.Equals(tenantId)
                                        && dateDue >= p.dateFrom && dateDue <= p.dateTo
                                        && p.companyId.Equals(CompanyId)
                                        ).ToListAsync();

                    if (!pdlist.Any())
                    {
                        ErrorMessage = "No property directory found.";
                        return;
                    }

                    var pd = pdlist.First();
                    billing.billId = Guid.NewGuid();
                    billing.documentId = documentId;
                    billing.transactionDate = DateTime.Today;
                    billing.billType = this.billingType;
                    billing.MonthYear = dateDue.ToString("yyyyMM");

                    billing.dateDue = dateDue;
                    billing.propertyDirectoryId = pd.id;
                    billing.propertyDirectory = pd;
                    billing.MonthYear = dateDue.ToString("yyyyMM");
                    billing.billType = this.billingType;
                    billing.createdBy = UserName;
                    billing.companyId = CompanyId;

                    if (billingType.Equals("MB"))
                    {
                        var parameters = new DynamicParameters();

                        parameters.Add("propertyDirectoryid", pdlist.First().id);
                        parameters.Add("MonthYear", billing.MonthYear);

                        decimal monthlyRentBalance = 0m;
                        decimal monthlyAssocDueBalance = 0m;

                        var balanceView = await dapperManager.GetAllAsync<PropertyBalanceViewModel>("spGetMonthlyBalance", parameters);

                        monthlyRentBalance = balanceView.Where(a => a.billLineType.Equals(Constants.BillLineTypes.MONTHLYBILLITEM, StringComparison.OrdinalIgnoreCase)
                                                                 || a.billLineType.Equals(Constants.BillLineTypes.MONTHLYBILLITEM_PREVBAL, StringComparison.OrdinalIgnoreCase)
                                                                 || a.billLineType.Equals(Constants.BillLineTypes.MONTHLYBILLITEMPENALTY, StringComparison.OrdinalIgnoreCase)
                                                                 || a.billLineType.Equals(Constants.BillLineTypes.MONTHLYBILLITEM_VAT, StringComparison.OrdinalIgnoreCase)
                                                                 || a.billLineType.Equals(Constants.BillLineTypes.MONTHLYBILLITEM_WT, StringComparison.OrdinalIgnoreCase)

                                                                 )
                                                        .Sum(a => a.balance);

                        monthlyAssocDueBalance = balanceView.Where(a => a.billLineType.Equals(Constants.BillLineTypes.MONTHLYASSOCDUE, StringComparison.OrdinalIgnoreCase)
                                                                 || a.billLineType.Equals(Constants.BillLineTypes.MONTHLYASSOCDUEPENALTY, StringComparison.OrdinalIgnoreCase)
                                                                 || a.billLineType.Equals(Constants.BillLineTypes.MONTHLYASSOCDUE_PREVBAL, StringComparison.OrdinalIgnoreCase)
                                                                  || a.billLineType.Equals(Constants.BillLineTypes.MONTHLYASSOCDUE_VAT, StringComparison.OrdinalIgnoreCase)
                                                                 )
                                                        .Sum(a => a.balance);

                        List<BillingLineItem> billItems = new List<BillingLineItem>();

                        var dueAmount = 0m;

                        //if (!string.IsNullOrEmpty(pd.property.propertyType) && pd.property.propertyType.Equals("PARKING"))
                        //    dueAmount = pd.ratePerSQM * pd.property.areaInSqm;
                        //else
                        //    dueAmount = pd.monthlyRate;

                        dueAmount = pd.monthlyRate;
                        var dueAmountVat = CalculateVat(dueAmount);
                        var dueAmountBeforeVat = dueAmount - dueAmountVat;
                        var wtAmt = 0m;

                        if (dueAmountBeforeVat != 0m && pd.withWT)
                        {
                            wtAmt = CalculateWT(dueAmountBeforeVat);
                            dueAmountBeforeVat = dueAmount - (dueAmountVat + wtAmt);
                        }

                        if (monthlyRentBalance > 0)
                        {
                            var penaltyPct = pd.penaltyPct == 0m ? 3m : pd.penaltyPct;
                            var penalty = monthlyRentBalance * (penaltyPct / 100m);

                            if (penalty > 0)
                            {
                                billItems.Add(new BillingLineItem()
                                {
                                    Id = Guid.NewGuid(),
                                    description = $"Monthly rent penalty {penaltyPct.ToString("0.00")}%",
                                    amount = penalty,
                                    lineNo = 0,
                                    generated = true,
                                    billLineType = Constants.BillLineTypes.MONTHLYBILLITEMPENALTY
                                });
                            }

                            billItems.Add(new BillingLineItem()
                            {
                                Id = Guid.NewGuid(),
                                description = "Previous balance",
                                amount = monthlyRentBalance,
                                lineNo = 1,
                                generated = true,
                                billLineType = Constants.BillLineTypes.MONTHLYBILLITEM_PREVBAL
                            });
                        }


                        billItems.Add(new BillingLineItem()
                        {
                            Id = Guid.NewGuid(),
                            description = "Monthly due",
                            amount = dueAmountBeforeVat,
                            amountPaid = 0,
                            lineNo = 2,
                            generated = true,
                            billLineType = Constants.BillLineTypes.MONTHLYBILLITEM
                        });

                        billItems.Add(new BillingLineItem()
                        {
                            Id = Guid.NewGuid(),
                            description = "Monthly due (VAT)",
                            amount = dueAmountVat,
                            amountPaid = 0,
                            lineNo = 2,
                            generated = true,
                            billLineType = Constants.BillLineTypes.MONTHLYBILLITEM_VAT
                        });

                        if (wtAmt != 0m)
                        {
                            billItems.Add(new BillingLineItem()
                            {
                                Id = Guid.NewGuid(),
                                description = "Monthly due (WT)",
                                amount = wtAmt,
                                amountPaid = 0,
                                lineNo = 2,
                                generated = true,
                                billLineType = Constants.BillLineTypes.MONTHLYBILLITEM_WT
                            });
                        }

                        if (monthlyAssocDueBalance > 0)
                        {
                            billItems.Add(new BillingLineItem()
                            {
                                Id = Guid.NewGuid(),
                                description = "Association dues penalty",
                                amount = monthlyAssocDueBalance * (pd.penaltyPct / 100m),
                                lineNo = 3,
                                generated = true,
                                billLineType = Constants.BillLineTypes.MONTHLYASSOCDUEPENALTY
                            });

                            billItems.Add(new BillingLineItem()
                            {
                                Id = Guid.NewGuid(),
                                description = "Previous association dues balance",
                                amount = monthlyAssocDueBalance,
                                lineNo = 4,
                                generated = true,
                                billLineType = Constants.BillLineTypes.MONTHLYASSOCDUE_PREVBAL
                            });
                        }

                        if (pd.associationDues > 0)
                        {
                            var assocDuesBeforevat = CalculateBeforeVat(pd.associationDues);
                            var assocDuesVat = CalculateVat(pd.associationDues);

                            billItems.Add(new BillingLineItem()
                            {
                                Id = Guid.NewGuid(),
                                description = "Association dues",
                                amount = assocDuesBeforevat,
                                amountPaid = 0,
                                lineNo = 6,
                                generated = true,
                                billLineType = Constants.BillLineTypes.MONTHLYASSOCDUE
                            });

                            billItems.Add(new BillingLineItem()
                            {
                                Id = Guid.NewGuid(),
                                description = "Association dues (VAT)",
                                amount = assocDuesVat,
                                amountPaid = 0,
                                lineNo = 7,
                                generated = true,
                                billLineType = Constants.BillLineTypes.MONTHLYASSOCDUE_VAT
                            });

                        }

                        billing.totalAmount = billItems.Sum(a => a.amount);
                        billing.balance = billItems.Sum(a => a.amount - a.amountPaid);
                        billing.amountPaid = billItems.Sum(a => a.amountPaid);

                        billing.billingLineItems = billItems;
                    }
                }
                else
                {
                    billing = await appDBContext.Billings.AsNoTracking()
                         .Include(b => b.propertyDirectory)
                         .ThenInclude(a => a.property)
                          .Include(b => b.propertyDirectory)
                          .ThenInclude(a => a.tenant)
                        .Include(b => b.billingLineItems)
                        .Where(b => b.billId.Equals(Guid.Parse(this.billingId))).FirstOrDefaultAsync();

                    IsViewOnly = true;
                }

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

      
        protected async Task HandleInitializeBill()
        {
            IsBillInitialized = true;
            IsDataLoaded = false;
            billing = new Billing();
            billing.transactionDate = DateTime.Today;
            billing.billingLineItems = new List<BillingLineItem>();

            InitParameters(billingViewModel.propertyId,
                            billingViewModel.tenantId,
                            billingViewModel.dateDue,
                            billingViewModel.billType, 
                            string.Empty);
            await InitNewBilling();
            IsDataLoaded = true;
        }

        protected async Task HandleValidSubmit()
        {
            try
            {
                IsDataLoaded = false;

                if (string.IsNullOrEmpty(billingId))
                {
                    using (var dbConnection = dapperManager.GetConnection())
                    {
                        dbConnection.Open();

                        using (var transaction = dbConnection.BeginTransaction())
                        {
                            try
                            {
                                var param = new Dapper.DynamicParameters();
                                param.Add("billId", billing.billId, System.Data.DbType.Guid);
                                param.Add("createdBy", UserName, System.Data.DbType.String);
                                param.Add("dateDue", billing.dateDue, System.Data.DbType.DateTime);
                                param.Add("totalAmount", billing.totalAmount, System.Data.DbType.Decimal);
                                param.Add("status", billing.status, System.Data.DbType.String);
                                param.Add("propertyDirectoryId", billing.propertyDirectoryId, System.Data.DbType.Guid);
                                param.Add("companyId", CompanyId, System.Data.DbType.String);
                                param.Add("amountPaid", billing.amountPaid, System.Data.DbType.Decimal);
                                param.Add("balance", billing.balance, System.Data.DbType.Decimal);
                                param.Add("documentId", billing.documentId, System.Data.DbType.String);
                                param.Add("transactionDate", billing.transactionDate, System.Data.DbType.DateTime);
                                param.Add("MonthYear", billing.MonthYear, System.Data.DbType.String);
                                param.Add("billType", billing.billType, System.Data.DbType.String);

                                var result = await dapperManager.ExecuteAsync("spInsertBillings", transaction, dbConnection, param);

                                foreach (BillingLineItem item in billing.billingLineItems)
                                {
                                    var paramLine = new Dapper.DynamicParameters();
                                    paramLine.Add("Id", item.Id, System.Data.DbType.Guid);
                                    paramLine.Add("description", item.description, System.Data.DbType.String);
                                    paramLine.Add("amount", item.amount, System.Data.DbType.Decimal);
                                    paramLine.Add("lineNo", item.lineNo, System.Data.DbType.Int32);
                                    paramLine.Add("billingId", billing.billId, System.Data.DbType.Guid);
                                    paramLine.Add("generated", item.generated, System.Data.DbType.Boolean);
                                    paramLine.Add("amountPaid", item.amountPaid, System.Data.DbType.Decimal);
                                    paramLine.Add("billLineType", item.billLineType, System.Data.DbType.String);
                                    await dapperManager.ExecuteAsync("spInsertBillingLineItem", transaction, dbConnection, paramLine);
                                }

                                param = null;
                                param = new Dapper.DynamicParameters();
                                param.Add("billId", billing.billId, System.Data.DbType.Guid);
                                result = await dapperManager.ExecuteAsync("spClosePreviousBill", transaction, dbConnection, param);

                                transaction.Commit();
                                NavigateToList();
                            }
                            catch (Exception ex)
                            {
                                ErrorMessage = ex.ToString();
                                transaction.Rollback();
                            }
                            finally
                            {
                                if (dbConnection.State == ConnectionState.Open)
                                    dbConnection.Close();
                            }
                        }
                    }


                }
                else
                {
                    using (var dbConnection = dapperManager.GetConnection())
                    {
                        dbConnection.Open();

                        using (var transaction = dbConnection.BeginTransaction())
                        {
                            try
                            {
                                var param = new Dapper.DynamicParameters();
                                param.Add("billId", billing.billId, System.Data.DbType.Guid);
                                param.Add("updatedBy", UserName, System.Data.DbType.String);

                                param.Add("dateDue", billing.dateDue, System.Data.DbType.DateTime);
                                param.Add("totalAmount", billing.totalAmount, System.Data.DbType.Decimal);
                                param.Add("status", billing.status, System.Data.DbType.String);
                                param.Add("amountPaid", billing.amountPaid, System.Data.DbType.Decimal);
                                param.Add("balance", billing.balance, System.Data.DbType.Decimal);

                                var result = await dapperManager.ExecuteAsync("spUpdateBillings", transaction, dbConnection, param);

                                foreach (BillingLineItem item in billing.billingLineItems)
                                {
                                    var paramLine = new Dapper.DynamicParameters();
                                    paramLine.Add("Id", item.Id, System.Data.DbType.Guid);
                                    paramLine.Add("description", item.description, System.Data.DbType.String);
                                    paramLine.Add("amount", item.amount, System.Data.DbType.Decimal);
                                    paramLine.Add("lineNo", item.lineNo, System.Data.DbType.Int32);
                                    paramLine.Add("billingId", billing.billId, System.Data.DbType.Guid);
                                    paramLine.Add("generated", item.generated, System.Data.DbType.Boolean);
                                    paramLine.Add("amountPaid", item.amountPaid, System.Data.DbType.Decimal);
                                    paramLine.Add("billLineType", item.billLineType, System.Data.DbType.String);
                                    await dapperManager.ExecuteAsync("spInsertOrUpdateBillingLineItem", transaction, dbConnection, paramLine);
                                }

                                if (!string.IsNullOrEmpty(DeletedItems.ToString()))
                                {
                                    var itemsToDelete = DeletedItems.ToString().Split("|");
                                    foreach (string item in itemsToDelete)
                                    {
                                        Guid id = Guid.Empty;
                                        if (Guid.TryParse(item, out id))
                                        {
                                            var paramLine = new Dapper.DynamicParameters();
                                            paramLine.Add("Id", id, System.Data.DbType.Guid);
                                            await dapperManager.ExecuteAsync("spDeleteBillingLineItem", transaction, dbConnection, paramLine);
                                        }
                                    }
                                }

                                transaction.Commit();
                                NavigateToList();

                            }
                            catch (Exception ex)
                            {
                                ErrorMessage = ex.ToString();
                                transaction.Rollback();
                            }
                            finally
                            {
                                if (dbConnection.State == ConnectionState.Open)
                                    dbConnection.Close();
                            }
                        }
                    }

                }
                
                await SaveEventCallback.InvokeAsync(true);
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.ToString();
                IsDataLoaded = true;
            }

            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ShowDialog = true;
                StateHasChanged();
                return;
            }

            ShowDialog = false;
            StateHasChanged();
        }

        protected async Task HandleInvalidSubmit()
        {

        }

        protected void EditBillItem(Guid Id)
        {
            var billItem = billing.billingLineItems.Where(a => a.Id.Equals(Id)).FirstOrDefault();

            BillingLineItemDetail.model = null;
            BillingLineItemDetail.model = new BillingLineItemViewModel()
            {
                Id = Id.ToString(),
                description = billItem.description,
                amount = billItem.amount
            };

            BillingLineItemDetail.ShowDialog = true;
            StateHasChange();
        }

        protected void DeleteBillItem(Guid Id)
        {
            var billItem = billing.billingLineItems.Where(a => a.Id.Equals(Id)).FirstOrDefault();
            billing.billingLineItems.Remove(billItem);
            billing.totalAmount = billing.billingLineItems.Sum(a => a.amount);
            billing.balance = billing.totalAmount - billing.amountPaid;
            BillingLineItemDetail.ShowDialog = false;
            DeletedItems.AppendFormat("{0}|", Id.ToString());

            StateHasChange();
        }

        protected void NavigateToCollections()
        {
            NavigationManager.NavigateTo("/billinglist");
        }

        protected void AddDetail()
        {
            BillingLineItemDetail.model = null;
            BillingLineItemDetail.model = new BillingLineItemViewModel();

            BillingLineItemDetail.ShowDialog = true;
            StateHasChange();
        }

        public void NavigateToList()
        {
            NavigationManager.NavigateTo("/billinglist");
        }



    }
}
