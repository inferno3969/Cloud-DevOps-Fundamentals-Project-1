using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace RadzenTest.Client.Pages
{
    public partial class EditTestTable2
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
        public string NateIsGay { get; set; }

        protected override async Task OnInitializedAsync()
        {
            testTable2 = await DevOps_Proj_DatabaseService.GetTestTable2ByNateIsGay(nateIsGay:NateIsGay);
        }
        protected bool errorVisible;
        protected RadzenTest.Server.Models.DevOps_Proj_Database.TestTable2 testTable2;

        protected async Task FormSubmit()
        {
            try
            {
                await DevOps_Proj_DatabaseService.UpdateTestTable2(nateIsGay:NateIsGay, testTable2);
                DialogService.Close(testTable2);
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
    }
}