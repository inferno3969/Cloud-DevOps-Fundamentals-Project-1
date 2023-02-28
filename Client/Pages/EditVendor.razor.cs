using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using Microsoft.AspNetCore.Authorization;

namespace CloudDevOpsProject1.Client.Pages
{
    [Authorize(Roles="Administrator,Manager")] 
    
    public partial class EditVendor
    {
        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Inject]
        protected DialogService DialogService { get; set; }

        [Inject]
        protected TooltipService TooltipService { get; set; }

        [Inject]
        protected ContextMenuService ContextMenuService { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }
        [Inject]
        public DevOps_Proj_DatabaseService DevOps_Proj_DatabaseService { get; set; }

        [Parameter]
        public int Vendor_ID { get; set; }

        protected override async Task OnInitializedAsync()
        {
            vendor = await DevOps_Proj_DatabaseService.GetVendorByVendorId(vendorId:Vendor_ID);
        }
        protected bool errorVisible;
        protected CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Vendor vendor;

        protected async Task FormSubmit()
        {
            try
            {
                var result = await DevOps_Proj_DatabaseService.UpdateVendor(vendorId:Vendor_ID, vendor);
                if (result.StatusCode == System.Net.HttpStatusCode.PreconditionFailed)
                {
                     hasChanges = true;
                     canEdit = false;
                     return;
                }
                DialogService.Close(vendor);
            }
            catch (Exception ex)
            {
                errorVisible = true;
            }
        }

        protected async Task CancelButtonClick(MouseEventArgs args)
        {
            DialogService.Close(null);
        }


        protected bool hasChanges = false;
        protected bool canEdit = true;

        [Inject]
        protected SecurityService Security { get; set; }


        protected async Task ReloadButtonClick(MouseEventArgs args)
        {
            hasChanges = false;
            canEdit = true;

            vendor = await DevOps_Proj_DatabaseService.GetVendorByVendorId(vendorId:Vendor_ID);
        }
    }
}