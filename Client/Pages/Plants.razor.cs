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
    public partial class Plants
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

        protected IEnumerable<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Plant> plants;

        protected RadzenDataGrid<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Plant> grid0;
        protected int count;

        protected async Task Grid0LoadData(LoadDataArgs args)
        {
            try
            {
                var result = await DevOps_Proj_DatabaseService.GetPlants(filter: $"{args.Filter}", expand: "Manager", orderby: $"{args.OrderBy}", top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null);
                plants = result.Value.AsODataEnumerable();
                count = result.Count;
            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load Plants" });
            }
        }    

        protected async Task AddButtonClick(MouseEventArgs args)
        {
            await grid0.InsertRow(new CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Plant());
        }

        protected async Task GridDeleteButtonClick(MouseEventArgs args, CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Plant plant)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await DevOps_Proj_DatabaseService.DeletePlant(plantId:plant.Plant_ID);

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
                    Detail = $"Unable to delete Plant" 
                });
            }
        }
        protected bool errorVisible;
        protected CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Plant plant;

        protected IEnumerable<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Manager> managersForManagerID;


        protected int managersForManagerIDCount;
        protected CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Manager managersForManagerIDValue;
        protected async Task managersForManagerIDLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await DevOps_Proj_DatabaseService.GetManagers();
                managersForManagerID = result.Value.AsODataEnumerable();
                managersForManagerIDCount = managersForManagerID.Count();

            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load Radzen.Design.EntityProperty" });
            }
        }

        protected async Task CancelButtonClick(MouseEventArgs args)
        {
            NavigationManager.NavigateTo("plants");
        }



        protected async Task GridRowUpdate(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Plant args)
        {
            await DevOps_Proj_DatabaseService.UpdatePlant(args.Plant_ID, args);
        }

        protected async Task GridRowCreate(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Plant args)
        {
            await DevOps_Proj_DatabaseService.CreatePlant(args);
            await grid0.Reload();
        }

        protected async Task EditButtonClick(MouseEventArgs args, CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Plant data)
        {
            await grid0.EditRow(data);
        }

        protected async Task SaveButtonClick(MouseEventArgs args, CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Plant data)
        {
            await grid0.UpdateRow(data);
        }

        protected async Task CancelButtonClick(MouseEventArgs args, CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Plant data)
        {
            grid0.CancelEditRow(data);
            await grid0.Reload();
        }
    }
}