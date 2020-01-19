using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using terminus_webapp.Data;
using terminus.shared.models;
using Microsoft.AspNetCore.Components.Authorization;




namespace terminus_webapp.Pages.LoginPages
{
    public class LoginBase:ComponentBase
    {
        [Inject]
        private AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        [Inject]
        private Blazored.SessionStorage.ISessionStorageService sessionStorage { get; set; }

        protected LoginViewModel login;

        protected override Task OnInitializedAsync()
        {
            login = new LoginViewModel();
            return base.OnInitializedAsync();
        }

        protected async Task<bool> ValidateUser()
        {
            //assume that user is valid
            //call an API

            ((CustomAuthenticationStateProvider)AuthenticationStateProvider).MarkUserAsAuthenticated(login.userName, "ASRC");

            NavigationManager.NavigateTo("/");

            await sessionStorage.SetItemAsync("UserName", login.userName);
            await sessionStorage.SetItemAsync("UserName", login.userName);

            return await Task.FromResult(true);
        }
    }


}
