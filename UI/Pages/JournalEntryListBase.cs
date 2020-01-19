﻿using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using terminus.shared.models;
using terminus_webapp.Data;

namespace terminus_webapp.Pages
{
    public class JournalEntryListBase : ComponentBase
    {
        [Inject]
        public AppDBContext appDBContext { get; set; }

        [Inject]
        public DapperManager dapperManager { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        public List<JEListViewModel> JourlnalAccounts { get; set; }
        public bool DataLoaded { get; set; }
        public string ErrorMessage { get; set; }
        public string companyId { get; set; }
        
        protected override async Task OnInitializedAsync()
        {
            try
            {
                DataLoaded = false;
                ErrorMessage = string.Empty;
                companyId = "ASRC";
                var sqlcommand = $"exec sp_GetGLAccountBalance '{companyId.Replace("'", "''")}'";

                var param = new Dapper.DynamicParameters();
                param.Add("companyId", companyId, System.Data.DbType.String);


                JourlnalAccounts = dapperManager.GetAll<JEListViewModel>("sp_GetGLAccountBalance", param);

                
                
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
            finally
            {
                DataLoaded = true;
            }
        }
    }

}

