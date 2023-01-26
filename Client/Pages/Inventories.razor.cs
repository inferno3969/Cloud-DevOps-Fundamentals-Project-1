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
    public partial class Inventories
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

        protected IEnumerable<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Inventory> inventories;

        protected RadzenDataGrid<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Inventory> grid0;
        protected int count;

        protected async Task Grid0LoadData(LoadDataArgs args)
        {
            try
            {
                var result = await DevOps_Proj_DatabaseService.GetInventories(filter: $"{args.Filter}", expand: "Part,Plant", orderby: $"{args.OrderBy}", top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null);
                inventories = result.Value.AsODataEnumerable();
                count = result.Count;
            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load Inventories" });
            }
        }    

        protected async Task AddButtonClick(MouseEventArgs args)
        {
            await grid0.InsertRow(new CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Inventory());
        }

        protected async Task GridDeleteButtonClick(MouseEventArgs args, CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Inventory inventory)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await DevOps_Proj_DatabaseService.DeleteInventory(invId:inventory.Inv_ID);

                    if (deleteResult != null)
                    {
                        await grid0.Reload();
                    }
                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage
                { 
                    Severity = NotificationSeverity.Error,
                    Summary = $"Error", 
                    Detail = $"Unable to delete Inventory" 
                });
            }
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

        protected async Task CancelButtonClick(MouseEventArgs args)
        {
            NavigationManager.NavigateTo("inventories");
        }



        protected async Task GridRowUpdate(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Inventory args)
        {
            await DevOps_Proj_DatabaseService.UpdateInventory(args.Inv_ID, args);
        }

        protected async Task GridRowCreate(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Inventory args)
        {
            await DevOps_Proj_DatabaseService.CreateInventory(args);
            await grid0.Reload();
        }

        protected async Task EditButtonClick(MouseEventArgs args, CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Inventory data)
        {
            await grid0.EditRow(data);
        }

        protected async Task SaveButtonClick(MouseEventArgs args, CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Inventory data)
        {
            await grid0.UpdateRow(data);
        }

        protected async Task CancelButtonClick(MouseEventArgs args, CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Inventory data)
        {
            grid0.CancelEditRow(data);
            await grid0.Reload();
        }
    }
}