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
    public partial class EditInventory
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
        public int Inv_ID { get; set; }

        protected override async Task OnInitializedAsync()
        {
            inventory = await DevOps_Proj_DatabaseService.GetInventoryByInvId(invId:Inv_ID);
        }
        protected bool errorVisible;
        protected CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Inventory inventory;

        protected IEnumerable<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Part> partsForPartID;

        protected IEnumerable<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Plant> plantsForPlantID;


        protected int partsForPartIDCount;
        protected CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Part partsForPartIDValue;
        protected async Task partsForPartIDLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await DevOps_Proj_DatabaseService.GetParts();
                partsForPartID = result.Value.AsODataEnumerable();
                partsForPartIDCount = partsForPartID.Count();

            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load Radzen.Design.EntityProperty" });
            }
        }

        protected int plantsForPlantIDCount;
        protected CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Plant plantsForPlantIDValue;
        protected async Task plantsForPlantIDLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await DevOps_Proj_DatabaseService.GetPlants();
                plantsForPlantID = result.Value.AsODataEnumerable();
                plantsForPlantIDCount = plantsForPlantID.Count();

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
                var result = await DevOps_Proj_DatabaseService.UpdateInventory(invId:Inv_ID, inventory);
                if (result.StatusCode == System.Net.HttpStatusCode.PreconditionFailed)
                {
                     hasChanges = true;
                     canEdit = false;
                     return;
                }
                DialogService.Close(inventory);
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

            inventory = await DevOps_Proj_DatabaseService.GetInventoryByInvId(invId:Inv_ID);
        }
    }
}