﻿using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using terminus.shared.models;
using terminus_webapp.Data;

namespace terminus_webapp.Pages
{
    public class VendorEntryBase:ComponentBase
    {


        [Inject]
        public AppDBContext appDBContext { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Inject]
        public ISessionStorageService _sessionStorageService { get; set; }

        public DapperManager dapperManager { get; set; }

        public VendorsViewModal vendors { get; set; }

        [Parameter]
        public string vendorId { get; set; }

        public bool DataLoaded { get; set; }
        public string ErrorMessage { get; set; }

        public string CompanyId { get; set; }
        public string UserName { get; set; }

        public Guid? InputVatAccountId { get; set; }
        public string InputVatAccountCode { get; set; }
        public string InputVatAccountName { get; set; }

        public bool IsViewOnly { get; set; }
        public bool IsEditOnly { get; set; }

         
        public bool IsDataLoaded { get; set; }

        public void AddGLAccount()
        {
            NavigationManager.NavigateTo("vendorentry");
        }

        public void NavigateToList()
        {
            NavigationManager.NavigateTo("vendorlist");
        }

        protected async Task HandleValidSubmit()
        {

            UserName = await _sessionStorageService.GetItemAsync<string>("UserName");
            CompanyId = await _sessionStorageService.GetItemAsync<string>("CompanyId");

            bool _vatregister;

            if (vendors.isVatRegistered.ToString() == "N")
            {
                _vatregister = false;


            }
            else
            {
                _vatregister = true;
            }

            if (string.IsNullOrEmpty(vendors.vendorId))
            {



                var id = Guid.NewGuid();



                Vendor v = new Vendor()
                {
                    //var id = Guid.NewGuid(vendors.inputVatAccountid);

                    vendorId = vendors.vendorId,
                    companyId = vendors.companyId,
                    vendorName = vendors.vendorName,
                    rowOrder = vendors.rowOrder,
                    inputVatAccountId = id,
                    isVatRegistered = _vatregister


                };


                appDBContext.Vendors.Add(v);

                await appDBContext.SaveChangesAsync();


            }
            else
            {

                var data = await appDBContext.Vendors
                        //.Select(a => new { id = a.id, company = a.company, lastName = a.lastName, firstName = a.firstName, middleName = a.middleName, contactNumber = a.contactNumber, emailAddress = a.emailAddress })
                        .Include(a => a.company)
                        .Where(r => r.vendorId.Equals(vendorId)).FirstOrDefaultAsync();


                data.vendorId = vendors.vendorId;
                data.vendorName = vendors.vendorName;
                data.rowOrder = vendors.rowOrder;
                data.inputVatAccountId = Guid.Parse(vendors.inputVatAccountid);
                data.isVatRegistered = _vatregister;


                appDBContext.Vendors.Update(data);
                await appDBContext.SaveChangesAsync();
            }

            StateHasChanged();

            NavigateToList();
        }

        protected override async Task OnInitializedAsync()
        {
            try
            {



                UserName = await _sessionStorageService.GetItemAsync<string>("UserName");
                CompanyId = await _sessionStorageService.GetItemAsync<string>("CompanyId");

                IsDataLoaded = false;
                IsViewOnly = false;
                IsEditOnly = false;
                ErrorMessage = string.Empty;


                if (string.IsNullOrEmpty(vendorId))
                {

                    IsEditOnly = false;
                    vendors = new VendorsViewModal();

                }
                else
                {
                    IsViewOnly = false;
                    IsEditOnly = true;

                    var data = await appDBContext.Vendors
                        .Include(a => a.company)
                        .Include(a => a.inputVatAccount)
                        //.ThenInclude(v => v.accountId)
                        .Where(r => r.vendorId.Equals(vendorId)).FirstOrDefaultAsync();

                    string strIVR;
                    if (data.isVatRegistered == true)
                    {

                        strIVR = "Y";
                    }
                    else
                    {
                        strIVR = "N";
                    }

                    if (data.inputVatAccountId.HasValue)
                    {
                        InputVatAccountId = data.inputVatAccount.accountId;
                        InputVatAccountCode = data.inputVatAccount.accountCode;
                        InputVatAccountName = data.inputVatAccount.accountDesc;
                    }

                    vendors = new VendorsViewModal()
                    {
                        vendorId = data.vendorId.ToString(),
                        vendorName = data.vendorName,
                        companyId  = CompanyId,
                        rowOrder = data.rowOrder,
                        inputVatAccountid = data.inputVatAccountId.ToString(),
                        
                        isVatRegistered = strIVR

                    };
                }

                vendors.inputVatAccount = await appDBContext.GLAccounts.Where(a => a.outputVatAccount).ToListAsync();

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


    }
}
