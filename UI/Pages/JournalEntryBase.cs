using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using terminus_webapp.Data;

namespace terminus_webapp.Pages
{
    public class JournalEntryBase:ComponentBase
    {
        [Inject]
        public AppDBContext appDBContext { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Parameter]
        public string accountId { get; set; }

        public bool IsDataLoaded { get; set; }
    }
}
