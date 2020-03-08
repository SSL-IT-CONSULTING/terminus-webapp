using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace terminus_webapp.Pages
{
    public class IndexBase : ComponentBase
    {

        [Inject]
        public ISessionStorageService _sessionStorageService { get; set; }
        protected string companyLogo { get; set; }
        protected string CompanyId { get; set; }

        protected override async Task OnInitializedAsync()
        {

            CompanyId = await _sessionStorageService.GetItemAsync<string>("CompanyId");
            companyLogo = $"/Logos/{CompanyId}.png";
        }
    }
}
