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
    
    public partial class EditEmployee
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
        public int Emp_ID { get; set; }

        protected override async Task OnInitializedAsync()
        {
            employee = await DevOps_Proj_DatabaseService.GetEmployeeByEmpId(empId:Emp_ID);
        }
        protected bool errorVisible;
        protected CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Employee employee;

        protected IEnumerable<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Plant> plantsForPlantID;

        protected IEnumerable<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Position> positionsForPosID;


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

        protected int positionsForPosIDCount;
        protected CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Position positionsForPosIDValue;
        protected async Task positionsForPosIDLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await DevOps_Proj_DatabaseService.GetPositions();
                positionsForPosID = result.Value.AsODataEnumerable();
                positionsForPosIDCount = positionsForPosID.Count();

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
                var result = await DevOps_Proj_DatabaseService.UpdateEmployee(empId:Emp_ID, employee);
                if (result.StatusCode == System.Net.HttpStatusCode.PreconditionFailed)
                {
                     hasChanges = true;
                     canEdit = false;
                     return;
                }
                DialogService.Close(employee);
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

            employee = await DevOps_Proj_DatabaseService.GetEmployeeByEmpId(empId:Emp_ID);
        }
    }
}