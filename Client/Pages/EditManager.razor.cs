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
    public partial class EditManager
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
        public int Manager_ID { get; set; }

        protected override async Task OnInitializedAsync()
        {
            manager = await DevOps_Proj_DatabaseService.GetManagerByManagerId(managerId:Manager_ID);
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
        protected async Task FormSubmit()
        {
            try
            {
                var result = await DevOps_Proj_DatabaseService.UpdateManager(managerId:Manager_ID, manager);
                if (result.StatusCode == System.Net.HttpStatusCode.PreconditionFailed)
                {
                     hasChanges = true;
                     canEdit = false;
                     return;
                }
                DialogService.Close(manager);
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

            manager = await DevOps_Proj_DatabaseService.GetManagerByManagerId(managerId:Manager_ID);
        }
    }
}