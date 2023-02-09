using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace CloudDevOpsProject1.Client.Pages
{
    public partial class AddPart
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

        protected override async Task OnInitializedAsync()
        {
            part = new CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Part();
        }
        protected bool errorVisible;
        protected CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Part part;

        protected IEnumerable<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Vendor> vendorsForVendorID;


        protected int vendorsForVendorIDCount;
        protected CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Vendor vendorsForVendorIDValue;
        protected async Task vendorsForVendorIDLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await DevOps_Proj_DatabaseService.GetVendors();
                vendorsForVendorID = result.Value.AsODataEnumerable();
                vendorsForVendorIDCount = vendorsForVendorID.Count();

            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load Radzen.Design.EntityProperty" });
            }
        }
        protected async Task FormSubmit()
        {
            try
            {
                var result = await DevOps_Proj_DatabaseService.CreatePart(part);
                DialogService.Close(part);
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
    }
}