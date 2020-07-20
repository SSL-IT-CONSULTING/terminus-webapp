using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using terminus.dataaccess;
using terminus.shared.models;

namespace terminus_webapp.Components
{
    public class ApplyPaymentBase:ComponentBase
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
        public EventCallback<RevenueLineItemViewModel> SaveEventCallback { get; set; }

        [Parameter]
        public EventCallback<bool> CancelEventCallback { get; set; }

        public bool IsDataLoaded { get; set; }
        public string ErrorMessage { get; set; }
        public bool ShowDialog { get; set; }
        
        [Parameter]
        public string revenueLineIdItemId { get; set; }


        [Parameter]
        public List<GLAccount> GLAccounts { get; set; }

        public RevenueLineItemViewModel revenueLineItem { get; set; }

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

        public void InitParameters(string _revenueLineIdItemId, List<GLAccount> _gLAccounts, RevenueLineItemViewModel _revenueLineItem)
        {
            IsDataLoaded = false;
            this.revenueLineIdItemId = _revenueLineIdItemId;
            GLAccounts = _gLAccounts;
            this.revenueLineItem = _revenueLineItem;
            IsDataLoaded = true;
        }

        protected async override Task OnInitializedAsync()
        {
            UserName = await _sessionStorageService.GetItemAsync<string>("UserName");
            CompanyId = await _sessionStorageService.GetItemAsync<string>("CompanyId");
            IsDataLoaded = true;
        }

        protected async Task HandleValidSubmit()
        {
            try
            {
                IsDataLoaded = false;

                await SaveEventCallback.InvokeAsync(this.revenueLineItem);
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
    }
}
