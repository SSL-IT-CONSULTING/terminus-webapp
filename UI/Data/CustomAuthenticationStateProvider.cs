using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using Blazored.SessionStorage;


namespace terminus_webapp.Data
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private ISessionStorageService _sessionStorageService;

        public CustomAuthenticationStateProvider(ISessionStorageService sessionStorageService)
        {
          //  throw new Exception("CustomAuthenticationStateProviderException");
            _sessionStorageService = sessionStorageService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var userName = await _sessionStorageService.GetItemAsync<string>("UserName");
            var companyId = await _sessionStorageService.GetItemAsync<string>("CompanyId");

            ClaimsIdentity identity;

            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(companyId))
            {
                identity = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, userName),
                     new Claim("CompanyId", companyId),
                }, "apiauth_type");
            }
            else
            {
                identity = new ClaimsIdentity();
            }

            var user = new ClaimsPrincipal(identity);

            return await Task.FromResult(new AuthenticationState(user));
        }

        public void MarkUserAsAuthenticated(string userName, string companyId)
        {
            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, userName),
                new Claim("CompanyId", companyId),
            }, "apiauth_type");

            var user = new ClaimsPrincipal(identity);

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }

        public void MarkUserAsLoggedOut()
        {
            _sessionStorageService.RemoveItemAsync("UserName");
            _sessionStorageService.RemoveItemAsync("CompanyId");
            var identity = new ClaimsIdentity();

            var user = new ClaimsPrincipal(identity);

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
        }
    }
}
