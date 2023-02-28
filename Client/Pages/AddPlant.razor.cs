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
    [Authorize(Roles="Administrator")] 
    
    public partial class AddPlant
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
            plant = new CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Plant();
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
        protected async Task FormSubmit()
        {
            try
            {
                var result = await DevOps_Proj_DatabaseService.CreatePlant(plant);
                DialogService.Close(plant);
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