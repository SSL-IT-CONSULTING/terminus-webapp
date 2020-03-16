using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using terminus_webapp.Data;
using terminus.shared.models;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace terminus_webapp.Pages.Account
{
    public class LoginBase:ComponentBase
    {
        [Inject]
        private AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        [Inject]
        private IPasswordHasher<AppUser> PasswordHasher { get; set; }

        [Inject]
        private UserManager<AppUser> UserManager { get; set; }

      
        [Inject]
        private Blazored.SessionStorage.ISessionStorageService sessionStorage { get; set; }

        protected LoginViewModel login;
        protected string errorMessage;
        protected override Task OnInitializedAsync()
        {
            login = new LoginViewModel();
            errorMessage = "";

            ((CustomAuthenticationStateProvider)AuthenticationStateProvider).MarkUserAsLoggedOut();


            return base.OnInitializedAsync();
        }

        protected async Task<bool> ValidateUser()
        {
            //assume that user is valid
            //call an API
            errorMessage = "";
            var user = await UserManager.Users.Include(a=>a.company).Where(a=>a.UserName.Equals(login.userName)).FirstOrDefaultAsync();


            if (user!=null && (PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, login.password) ==
                PasswordVerificationResult.Success))
            {
                ((CustomAuthenticationStateProvider)AuthenticationStateProvider).MarkUserAsAuthenticated(login.userName, user.companyId, user.company.companyName, user.company.address);

                await sessionStorage.SetItemAsync("UserName", login.userName);
                await sessionStorage.SetItemAsync("CompanyId", user.companyId);

                NavigationManager.NavigateTo("/");

                return await Task.FromResult(true);
            }
            errorMessage = "Invalid user name or password.";
            return await Task.FromResult(false);
        }
    }


}
