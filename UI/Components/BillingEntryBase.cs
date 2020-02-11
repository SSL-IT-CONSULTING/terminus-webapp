using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using terminus.shared.models;
using terminus_webapp.Components;
using terminus_webapp.Data;

namespace terminus_webapp.Components
{
    public class BillingEntryBase:ComponentBase
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
        public string ErrorMessage { get; set; }
        public bool ShowDialog { get; set; }

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

        public void InitParameters(string _propertyId, string _tenantId, DateTime? _dueDate)
        {
            this.propertyId = _propertyId;
            this.tenantId = _tenantId;
            this.dueDate = _dueDate.HasValue ? _dueDate.Value.ToString("yyyy-MM-dd") : DateTime.Today.ToString("yyyy-MM-dd");
        }

        [Parameter]
        public string propertyId { get; set; }

        [Parameter]
        public string tenantId { get; set; }

        [Parameter]
        public string dueDate { get; set; }

        [Parameter]
        public string billingId { get; set; }

        public Billing billing { get; set; }

        protected BillingLineItemDetail BillingLineItemDetail { get; set; }

        public void BillingLineItemDetail_OnSave()
        {
            if(string.IsNullOrEmpty(BillingLineItemDetail.model.Id))
            {
                billing.billingLineItems.Add(new BillingLineItem()
                {
                    Id = Guid.NewGuid(),
                    description = BillingLineItemDetail.model.description,
                    amount = BillingLineItemDetail.model.amount
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

        protected override async Task OnInitializedAsync()
        {
            billing = new Billing(); 
        }

        public async Task InitNewBilling()
        {
            try
            {
                IsDataLoaded = false;

                UserName = await _sessionStorageService.GetItemAsync<string>("UserName");
                CompanyId = await _sessionStorageService.GetItemAsync<string>("CompanyId");

                if (string.IsNullOrEmpty(billingId))
                {
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



                    //billing = new Billing();

                    billing.billId = Guid.NewGuid();
                    billing.transactionDate = DateTime.Today;

                    var tenantInitials = string.Format("{0}{1}{2}",
                                         string.IsNullOrEmpty(pd.tenant.firstName) ? string.Empty : pd.tenant.firstName.Substring(0, 1),
                                         string.IsNullOrEmpty(pd.tenant.middleName) ? string.Empty : pd.tenant.middleName.Substring(0, 1),
                                         string.IsNullOrEmpty(pd.tenant.lastName) ? string.Empty : pd.tenant.lastName.Substring(0, 1));

                    billing.documentId = $"BILL_{tenantInitials}{DateTime.Now.ToString("yyyyMMddHHmmss")}".ToUpper();
                    billing.dateDue = dateDue;
                    billing.propertyDirectoryId = pd.id;
                    billing.propertyDirectory = pd;

                    List<BillingLineItem> billItems = new List<BillingLineItem>();

                    var dueAmount = 0m;

                    if (!string.IsNullOrEmpty(pd.property.propertyType) && pd.property.propertyType.Equals("PARKING"))
                        dueAmount = pd.ratePerSQM * pd.property.areaInSqm;
                    else
                        dueAmount = pd.monthlyRate;

                    if (pd.totalBalance > 0)
                    {
                        billItems.Add(new BillingLineItem()
                        {
                            Id = Guid.NewGuid(),
                            description = "Penalty",
                            amount = pd.totalBalance * (pd.penaltyPct / 100m),
                            lineNo = 0,
                            generated=true
                        });

                        billItems.Add(new BillingLineItem()
                        {
                            Id = Guid.NewGuid(),
                            description = "Previous balance",
                            amount = pd.totalBalance,
                            lineNo = 1,
                            generated = true
                        });
                    }

                    billItems.Add(new BillingLineItem()
                    {
                        Id = Guid.NewGuid(),
                        description = "Monthly due",
                        amount = dueAmount,
                        lineNo = 2,
                        generated = true
                    });

                    if (pd.associationDues > 0)
                    {
                        billItems.Add(new BillingLineItem()
                        {
                            Id = Guid.NewGuid(),
                            description = "Association dues",
                            amount = pd.associationDues,
                            lineNo = 3,
                            generated = true
                        });
                    }

                    billing.totalAmount = billItems.Sum(a => a.amount);
                    billing.balance = billing.totalAmount;
                    billing.billingLineItems = billItems;
                    billing.createdBy = UserName;
                    billing.companyId = CompanyId;
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

        protected async Task HandleValidSubmit()
        {
            try
            {
                IsDataLoaded = false;
                billing.propertyDirectory.totalBalance = billing.balance;

                appDBContext.Billings.Add(billing);
                await appDBContext.SaveChangesAsync();
                await SaveEventCallback.InvokeAsync(true);
            }
            catch(Exception ex)
            {
                IsDataLoaded = true;
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

            BillingLineItemDetail.ShowDialog=true;
            StateHasChange();
        }

       

        

    }
}
