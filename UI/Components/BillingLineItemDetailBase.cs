using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;
using terminus.dataaccess;
using terminus.shared.models;

namespace terminus_webapp.Components
{
    public class BillingLineItemDetailBase:ComponentBase
    {
        [Inject]
        public AppDBContext appDBContext { get; set; }

        [Parameter]
        public string description { get; set; }
        
        [Parameter]
        public decimal amount { get; set; }

        [Parameter]
        public Guid lineId { get; set; }

        [Parameter]
        public EventCallback<bool> SaveEventCallback { get; set; }

        [Parameter]
        public EventCallback<bool> CancelEventCallback { get; set; }

        public bool IsDataLoaded { get; set; }
        public string ErrorMessage { get; set; }
        public bool ShowDialog { get; set; }

        public BillingLineItemViewModel model { get; set; }

        public void CloseDialog()
        {
            ShowDialog = false;
            StateHasChanged();
        }

        public void OpenDialogBox()
        {
            ShowDialog = true;
            StateHasChanged();
        }

        public void StateHasChange()
        {
            StateHasChanged();
        }

        protected async Task HandleValidSubmit()
        {
            await SaveEventCallback.InvokeAsync(true);
            CloseDialog();
            StateHasChanged();
        }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                IsDataLoaded = false;
                model = new BillingLineItemViewModel();
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

    }
}
