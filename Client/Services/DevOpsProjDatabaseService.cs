
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Web;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using Radzen;

namespace CloudDevOpsProject1.Client
{
    public partial class DevOps_Proj_DatabaseService
    {
        private readonly HttpClient httpClient;
        private readonly Uri baseUri;
        private readonly NavigationManager navigationManager;

        public DevOps_Proj_DatabaseService(NavigationManager navigationManager, HttpClient httpClient, IConfiguration configuration)
        {
            this.httpClient = httpClient;

            this.navigationManager = navigationManager;
            this.baseUri = new Uri($"{navigationManager.BaseUri}odata/DevOps_Proj_Database/");
        }


        public async System.Threading.Tasks.Task ExportEmployeesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/devops_proj_database/employees/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/devops_proj_database/employees/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportEmployeesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/devops_proj_database/employees/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/devops_proj_database/employees/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetEmployees(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Employee>> GetEmployees(Query query)
        {
            return await GetEmployees(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Employee>> GetEmployees(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"Employees");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetEmployees(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Employee>>(response);
        }

        partial void OnCreateEmployee(HttpRequestMessage requestMessage);

        public async Task<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Employee> CreateEmployee(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Employee employee = default(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Employee))
        {
            var uri = new Uri(baseUri, $"Employees");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(employee), Encoding.UTF8, "application/json");

            OnCreateEmployee(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Employee>(response);
        }

        partial void OnDeleteEmployee(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteEmployee(int empId = default(int))
        {
            var uri = new Uri(baseUri, $"Employees({empId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteEmployee(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetEmployeeByEmpId(HttpRequestMessage requestMessage);

        public async Task<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Employee> GetEmployeeByEmpId(string expand = default(string), int empId = default(int))
        {
            var uri = new Uri(baseUri, $"Employees({empId})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetEmployeeByEmpId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Employee>(response);
        }

        partial void OnUpdateEmployee(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateEmployee(int empId = default(int), CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Employee employee = default(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Employee))
        {
            var uri = new Uri(baseUri, $"Employees({empId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);

            httpRequestMessage.Headers.Add("If-Match", employee.ETag);    

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(employee), Encoding.UTF8, "application/json");

            OnUpdateEmployee(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportInventoriesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/devops_proj_database/inventories/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/devops_proj_database/inventories/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportInventoriesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/devops_proj_database/inventories/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/devops_proj_database/inventories/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetInventories(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Inventory>> GetInventories(Query query)
        {
            return await GetInventories(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Inventory>> GetInventories(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"Inventories");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetInventories(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Inventory>>(response);
        }

        partial void OnCreateInventory(HttpRequestMessage requestMessage);

        public async Task<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Inventory> CreateInventory(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Inventory inventory = default(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Inventory))
        {
            var uri = new Uri(baseUri, $"Inventories");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(inventory), Encoding.UTF8, "application/json");

            OnCreateInventory(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Inventory>(response);
        }

        partial void OnDeleteInventory(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteInventory(int invId = default(int))
        {
            var uri = new Uri(baseUri, $"Inventories({invId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteInventory(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetInventoryByInvId(HttpRequestMessage requestMessage);

        public async Task<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Inventory> GetInventoryByInvId(string expand = default(string), int invId = default(int))
        {
            var uri = new Uri(baseUri, $"Inventories({invId})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetInventoryByInvId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Inventory>(response);
        }

        partial void OnUpdateInventory(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateInventory(int invId = default(int), CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Inventory inventory = default(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Inventory))
        {
            var uri = new Uri(baseUri, $"Inventories({invId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);

            httpRequestMessage.Headers.Add("If-Match", inventory.ETag);    

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(inventory), Encoding.UTF8, "application/json");

            OnUpdateInventory(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportManagersToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/devops_proj_database/managers/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/devops_proj_database/managers/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportManagersToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/devops_proj_database/managers/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/devops_proj_database/managers/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetManagers(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Manager>> GetManagers(Query query)
        {
            return await GetManagers(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Manager>> GetManagers(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"Managers");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetManagers(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Manager>>(response);
        }

        partial void OnCreateManager(HttpRequestMessage requestMessage);

        public async Task<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Manager> CreateManager(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Manager manager = default(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Manager))
        {
            var uri = new Uri(baseUri, $"Managers");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(manager), Encoding.UTF8, "application/json");

            OnCreateManager(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Manager>(response);
        }

        partial void OnDeleteManager(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteManager(int managerId = default(int))
        {
            var uri = new Uri(baseUri, $"Managers({managerId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteManager(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetManagerByManagerId(HttpRequestMessage requestMessage);

        public async Task<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Manager> GetManagerByManagerId(string expand = default(string), int managerId = default(int))
        {
            var uri = new Uri(baseUri, $"Managers({managerId})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetManagerByManagerId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Manager>(response);
        }

        partial void OnUpdateManager(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateManager(int managerId = default(int), CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Manager manager = default(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Manager))
        {
            var uri = new Uri(baseUri, $"Managers({managerId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);

            httpRequestMessage.Headers.Add("If-Match", manager.ETag);    

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(manager), Encoding.UTF8, "application/json");

            OnUpdateManager(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportPartsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/devops_proj_database/parts/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/devops_proj_database/parts/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportPartsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/devops_proj_database/parts/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/devops_proj_database/parts/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetParts(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Part>> GetParts(Query query)
        {
            return await GetParts(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Part>> GetParts(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"Parts");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetParts(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Part>>(response);
        }

        partial void OnCreatePart(HttpRequestMessage requestMessage);

        public async Task<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Part> CreatePart(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Part part = default(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Part))
        {
            var uri = new Uri(baseUri, $"Parts");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(part), Encoding.UTF8, "application/json");

            OnCreatePart(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Part>(response);
        }

        partial void OnDeletePart(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeletePart(int partId = default(int))
        {
            var uri = new Uri(baseUri, $"Parts({partId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeletePart(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetPartByPartId(HttpRequestMessage requestMessage);

        public async Task<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Part> GetPartByPartId(string expand = default(string), int partId = default(int))
        {
            var uri = new Uri(baseUri, $"Parts({partId})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetPartByPartId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Part>(response);
        }

        partial void OnUpdatePart(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdatePart(int partId = default(int), CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Part part = default(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Part))
        {
            var uri = new Uri(baseUri, $"Parts({partId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);

            httpRequestMessage.Headers.Add("If-Match", part.ETag);    

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(part), Encoding.UTF8, "application/json");

            OnUpdatePart(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportPlantsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/devops_proj_database/plants/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/devops_proj_database/plants/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportPlantsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/devops_proj_database/plants/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/devops_proj_database/plants/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetPlants(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Plant>> GetPlants(Query query)
        {
            return await GetPlants(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Plant>> GetPlants(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"Plants");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetPlants(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Plant>>(response);
        }

        partial void OnCreatePlant(HttpRequestMessage requestMessage);

        public async Task<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Plant> CreatePlant(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Plant plant = default(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Plant))
        {
            var uri = new Uri(baseUri, $"Plants");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(plant), Encoding.UTF8, "application/json");

            OnCreatePlant(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Plant>(response);
        }

        partial void OnDeletePlant(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeletePlant(int plantId = default(int))
        {
            var uri = new Uri(baseUri, $"Plants({plantId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeletePlant(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetPlantByPlantId(HttpRequestMessage requestMessage);

        public async Task<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Plant> GetPlantByPlantId(string expand = default(string), int plantId = default(int))
        {
            var uri = new Uri(baseUri, $"Plants({plantId})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetPlantByPlantId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Plant>(response);
        }

        partial void OnUpdatePlant(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdatePlant(int plantId = default(int), CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Plant plant = default(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Plant))
        {
            var uri = new Uri(baseUri, $"Plants({plantId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);

            httpRequestMessage.Headers.Add("If-Match", plant.ETag);    

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(plant), Encoding.UTF8, "application/json");

            OnUpdatePlant(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportPositionsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/devops_proj_database/positions/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/devops_proj_database/positions/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportPositionsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/devops_proj_database/positions/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/devops_proj_database/positions/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetPositions(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Position>> GetPositions(Query query)
        {
            return await GetPositions(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Position>> GetPositions(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"Positions");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetPositions(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Position>>(response);
        }

        partial void OnCreatePosition(HttpRequestMessage requestMessage);

        public async Task<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Position> CreatePosition(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Position position = default(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Position))
        {
            var uri = new Uri(baseUri, $"Positions");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(position), Encoding.UTF8, "application/json");

            OnCreatePosition(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Position>(response);
        }

        partial void OnDeletePosition(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeletePosition(int posId = default(int))
        {
            var uri = new Uri(baseUri, $"Positions({posId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeletePosition(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetPositionByPosId(HttpRequestMessage requestMessage);

        public async Task<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Position> GetPositionByPosId(string expand = default(string), int posId = default(int))
        {
            var uri = new Uri(baseUri, $"Positions({posId})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetPositionByPosId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Position>(response);
        }

        partial void OnUpdatePosition(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdatePosition(int posId = default(int), CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Position position = default(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Position))
        {
            var uri = new Uri(baseUri, $"Positions({posId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);

            httpRequestMessage.Headers.Add("If-Match", position.ETag);    

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(position), Encoding.UTF8, "application/json");

            OnUpdatePosition(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportTestTablesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/devops_proj_database/testtables/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/devops_proj_database/testtables/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportTestTablesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/devops_proj_database/testtables/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/devops_proj_database/testtables/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetTestTables(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.TestTable>> GetTestTables(Query query)
        {
            return await GetTestTables(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.TestTable>> GetTestTables(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"TestTables");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetTestTables(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.TestTable>>(response);
        }

        partial void OnCreateTestTable(HttpRequestMessage requestMessage);

        public async Task<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.TestTable> CreateTestTable(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.TestTable testTable = default(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.TestTable))
        {
            var uri = new Uri(baseUri, $"TestTables");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(testTable), Encoding.UTF8, "application/json");

            OnCreateTestTable(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.TestTable>(response);
        }

        partial void OnDeleteTestTable(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteTestTable(string test = default(string))
        {
            var uri = new Uri(baseUri, $"TestTables('{HttpUtility.UrlEncode(test.Trim().Replace("'", "''"))}')");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteTestTable(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetTestTableByTest(HttpRequestMessage requestMessage);

        public async Task<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.TestTable> GetTestTableByTest(string expand = default(string), string test = default(string))
        {
            var uri = new Uri(baseUri, $"TestTables('{HttpUtility.UrlEncode(test.Trim().Replace("'", "''"))}')");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetTestTableByTest(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.TestTable>(response);
        }

        partial void OnUpdateTestTable(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateTestTable(string test = default(string), CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.TestTable testTable = default(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.TestTable))
        {
            var uri = new Uri(baseUri, $"TestTables('{HttpUtility.UrlEncode(test.Trim().Replace("'", "''"))}')");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);

            httpRequestMessage.Headers.Add("If-Match", testTable.ETag);    

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(testTable), Encoding.UTF8, "application/json");

            OnUpdateTestTable(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportTestTable2SToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/devops_proj_database/testtable2s/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/devops_proj_database/testtable2s/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportTestTable2SToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/devops_proj_database/testtable2s/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/devops_proj_database/testtable2s/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetTestTable2S(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.TestTable2>> GetTestTable2S(Query query)
        {
            return await GetTestTable2S(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.TestTable2>> GetTestTable2S(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"TestTable2S");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetTestTable2S(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.TestTable2>>(response);
        }

        partial void OnCreateTestTable2(HttpRequestMessage requestMessage);

        public async Task<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.TestTable2> CreateTestTable2(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.TestTable2 testTable2 = default(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.TestTable2))
        {
            var uri = new Uri(baseUri, $"TestTable2S");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(testTable2), Encoding.UTF8, "application/json");

            OnCreateTestTable2(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.TestTable2>(response);
        }

        partial void OnDeleteTestTable2(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteTestTable2(string nateIsGay = default(string))
        {
            var uri = new Uri(baseUri, $"TestTable2S('{HttpUtility.UrlEncode(nateIsGay.Trim().Replace("'", "''"))}')");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteTestTable2(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetTestTable2ByNateIsGay(HttpRequestMessage requestMessage);

        public async Task<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.TestTable2> GetTestTable2ByNateIsGay(string expand = default(string), string nateIsGay = default(string))
        {
            var uri = new Uri(baseUri, $"TestTable2S('{HttpUtility.UrlEncode(nateIsGay.Trim().Replace("'", "''"))}')");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetTestTable2ByNateIsGay(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.TestTable2>(response);
        }

        partial void OnUpdateTestTable2(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateTestTable2(string nateIsGay = default(string), CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.TestTable2 testTable2 = default(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.TestTable2))
        {
            var uri = new Uri(baseUri, $"TestTable2S('{HttpUtility.UrlEncode(nateIsGay.Trim().Replace("'", "''"))}')");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);

            httpRequestMessage.Headers.Add("If-Match", testTable2.ETag);    

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(testTable2), Encoding.UTF8, "application/json");

            OnUpdateTestTable2(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        public async System.Threading.Tasks.Task ExportVendorsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/devops_proj_database/vendors/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/devops_proj_database/vendors/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportVendorsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/devops_proj_database/vendors/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/devops_proj_database/vendors/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetVendors(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Vendor>> GetVendors(Query query)
        {
            return await GetVendors(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Vendor>> GetVendors(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"Vendors");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetVendors(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Vendor>>(response);
        }

        partial void OnCreateVendor(HttpRequestMessage requestMessage);

        public async Task<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Vendor> CreateVendor(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Vendor vendor = default(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Vendor))
        {
            var uri = new Uri(baseUri, $"Vendors");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(vendor), Encoding.UTF8, "application/json");

            OnCreateVendor(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Vendor>(response);
        }

        partial void OnDeleteVendor(HttpRequestMessage requestMessage);

        public async Task<HttpResponseMessage> DeleteVendor(int vendorId = default(int))
        {
            var uri = new Uri(baseUri, $"Vendors({vendorId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            OnDeleteVendor(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }

        partial void OnGetVendorByVendorId(HttpRequestMessage requestMessage);

        public async Task<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Vendor> GetVendorByVendorId(string expand = default(string), int vendorId = default(int))
        {
            var uri = new Uri(baseUri, $"Vendors({vendorId})");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetVendorByVendorId(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Vendor>(response);
        }

        partial void OnUpdateVendor(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateVendor(int vendorId = default(int), CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Vendor vendor = default(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Vendor))
        {
            var uri = new Uri(baseUri, $"Vendors({vendorId})");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);

            httpRequestMessage.Headers.Add("If-Match", vendor.ETag);    

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(vendor), Encoding.UTF8, "application/json");

            OnUpdateVendor(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }
    }
}