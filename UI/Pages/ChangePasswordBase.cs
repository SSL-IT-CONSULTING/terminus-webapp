using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using terminus.shared.models;

namespace terminus_webapp.Pages
{
    public class ChangePasswordBase:ComponentBase
    {
        [Inject]
        public ISessionStorageService _sessionStorageService { get; set; }


        [Inject]
        private AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        [Inject]
        private IPasswordHasher<AppUser> PasswordHasher { get; set; }

        [Inject]
        private UserManager<AppUser> UserManager { get; set; }

        [Inject]
        private SignInManager<AppUser> SignInManager { get; set; }

        protected bool IsDataLoaded { get; set; }

        protected ChangePasswordViewModel ChangePassword { get; set; }

        protected string ErrorMessage { get; set; }
        protected string SuccessMessage { get; set; }

        protected string UserName { get; set; }

        protected async Task HandleValidSubmit()
        {
            ErrorMessage = string.Empty;
            SuccessMessage = string.Empty;
            try
            {
                if (!ChangePassword.ConfirmPassword.Equals(ChangePassword.NewPassword))
                {
                    ErrorMessage = "New password and confirm password does not match.";
                    return;
                }

                var user = await UserManager.FindByNameAsync(UserName);

                var result = await UserManager.ChangePasswordAsync(user, ChangePassword.OldPassword, ChangePassword.NewPassword);

                if(result.Succeeded)
                {
                    SuccessMessage = "Password successfully changed.";
                }
                else
                {
                    ErrorMessage = string.Join(Environment.NewLine, result.Errors.Select(a => a.Description).ToArray());
                }
            }
            catch(Exception ex)
            {
                ErrorMessage = ex.ToString();
            }
        }

        protected override async Task OnInitializedAsync()
        {
            IsDataLoaded = false;
            UserName = await _sessionStorageService.GetItemAsync<string>("UserName");
            ChangePassword = new ChangePasswordViewModel();
            IsDataLoaded = true;
        }
    }
}
