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

        [Inject]
        protected SecurityService Security { get; set; }

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
            await DialogService.OpenAsync<AddInventory>("Add Inventory", null);
            await grid0.Reload();
        }

        protected async Task EditRow(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Inventory args)
        {
            await DialogService.OpenAsync<EditInventory>("Edit Inventory", new Dictionary<string, object> { {"Inv_ID", args.Inv_ID} });
            await grid0.Reload();
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
    }
}