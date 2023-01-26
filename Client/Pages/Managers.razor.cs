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
    public partial class Managers
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

        protected IEnumerable<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Manager> managers;

        protected RadzenDataGrid<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Manager> grid0;
        protected int count;

        protected async Task Grid0LoadData(LoadDataArgs args)
        {
            try
            {
                var result = await DevOps_Proj_DatabaseService.GetManagers(filter: $"{args.Filter}", expand: "Employee,Plant", orderby: $"{args.OrderBy}", top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null);
                managers = result.Value.AsODataEnumerable();
                count = result.Count;
            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load Managers" });
            }
        }    

        protected async Task AddButtonClick(MouseEventArgs args)
        {
            await grid0.InsertRow(new CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Manager());
        }

        protected async Task GridDeleteButtonClick(MouseEventArgs args, CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Manager manager)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await DevOps_Proj_DatabaseService.DeleteManager(managerId:manager.Manager_ID);

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
                    Detail = $"Unable to delete Manager" 
                });
            }
        }
        protected bool errorVisible;
        protected CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Manager manager;

        protected IEnumerable<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Employee> employeesForEmpID;

        protected IEnumerable<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Plant> plantsForPlantID;


        protected int employeesForEmpIDCount;
        protected CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Employee employeesForEmpIDValue;
        protected async Task employeesForEmpIDLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await DevOps_Proj_DatabaseService.GetEmployees();
                employeesForEmpID = result.Value.AsODataEnumerable();
                employeesForEmpIDCount = employeesForEmpID.Count();

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
            NavigationManager.NavigateTo("managers");
        }



        protected async Task GridRowUpdate(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Manager args)
        {
            await DevOps_Proj_DatabaseService.UpdateManager(args.Manager_ID, args);
        }

        protected async Task GridRowCreate(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Manager args)
        {
            await DevOps_Proj_DatabaseService.CreateManager(args);
            await grid0.Reload();
        }

        protected async Task EditButtonClick(MouseEventArgs args, CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Manager data)
        {
            await grid0.EditRow(data);
        }

        protected async Task SaveButtonClick(MouseEventArgs args, CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Manager data)
        {
            await grid0.UpdateRow(data);
        }

        protected async Task CancelButtonClick(MouseEventArgs args, CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Manager data)
        {
            grid0.CancelEditRow(data);
            await grid0.Reload();
        }
    }
}