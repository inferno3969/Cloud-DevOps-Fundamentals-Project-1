
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

namespace RadzenTest.Client
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


        public async System.Threading.Tasks.Task ExportTestTablesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/devops_proj_database/testtables/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/devops_proj_database/testtables/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async System.Threading.Tasks.Task ExportTestTablesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/devops_proj_database/testtables/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/devops_proj_database/testtables/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnGetTestTables(HttpRequestMessage requestMessage);

        public async Task<Radzen.ODataServiceResult<RadzenTest.Server.Models.DevOps_Proj_Database.TestTable>> GetTestTables(Query query)
        {
            return await GetTestTables(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<RadzenTest.Server.Models.DevOps_Proj_Database.TestTable>> GetTestTables(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"TestTables");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetTestTables(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<RadzenTest.Server.Models.DevOps_Proj_Database.TestTable>>(response);
        }

        partial void OnCreateTestTable(HttpRequestMessage requestMessage);

        public async Task<RadzenTest.Server.Models.DevOps_Proj_Database.TestTable> CreateTestTable(RadzenTest.Server.Models.DevOps_Proj_Database.TestTable testTable = default(RadzenTest.Server.Models.DevOps_Proj_Database.TestTable))
        {
            var uri = new Uri(baseUri, $"TestTables");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(testTable), Encoding.UTF8, "application/json");

            OnCreateTestTable(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<RadzenTest.Server.Models.DevOps_Proj_Database.TestTable>(response);
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

        public async Task<RadzenTest.Server.Models.DevOps_Proj_Database.TestTable> GetTestTableByTest(string expand = default(string), string test = default(string))
        {
            var uri = new Uri(baseUri, $"TestTables('{HttpUtility.UrlEncode(test.Trim().Replace("'", "''"))}')");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetTestTableByTest(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<RadzenTest.Server.Models.DevOps_Proj_Database.TestTable>(response);
        }

        partial void OnUpdateTestTable(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateTestTable(string test = default(string), RadzenTest.Server.Models.DevOps_Proj_Database.TestTable testTable = default(RadzenTest.Server.Models.DevOps_Proj_Database.TestTable))
        {
            var uri = new Uri(baseUri, $"TestTables('{HttpUtility.UrlEncode(test.Trim().Replace("'", "''"))}')");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);


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

        public async Task<Radzen.ODataServiceResult<RadzenTest.Server.Models.DevOps_Proj_Database.TestTable2>> GetTestTable2S(Query query)
        {
            return await GetTestTable2S(filter:$"{query.Filter}", orderby:$"{query.OrderBy}", top:query.Top, skip:query.Skip, count:query.Top != null && query.Skip != null);
        }

        public async Task<Radzen.ODataServiceResult<RadzenTest.Server.Models.DevOps_Proj_Database.TestTable2>> GetTestTable2S(string filter = default(string), string orderby = default(string), string expand = default(string), int? top = default(int?), int? skip = default(int?), bool? count = default(bool?), string format = default(string), string select = default(string))
        {
            var uri = new Uri(baseUri, $"TestTable2S");
            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:filter, top:top, skip:skip, orderby:orderby, expand:expand, select:select, count:count);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetTestTable2S(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<Radzen.ODataServiceResult<RadzenTest.Server.Models.DevOps_Proj_Database.TestTable2>>(response);
        }

        partial void OnCreateTestTable2(HttpRequestMessage requestMessage);

        public async Task<RadzenTest.Server.Models.DevOps_Proj_Database.TestTable2> CreateTestTable2(RadzenTest.Server.Models.DevOps_Proj_Database.TestTable2 testTable2 = default(RadzenTest.Server.Models.DevOps_Proj_Database.TestTable2))
        {
            var uri = new Uri(baseUri, $"TestTable2S");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, uri);

            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(testTable2), Encoding.UTF8, "application/json");

            OnCreateTestTable2(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<RadzenTest.Server.Models.DevOps_Proj_Database.TestTable2>(response);
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

        public async Task<RadzenTest.Server.Models.DevOps_Proj_Database.TestTable2> GetTestTable2ByNateIsGay(string expand = default(string), string nateIsGay = default(string))
        {
            var uri = new Uri(baseUri, $"TestTable2S('{HttpUtility.UrlEncode(nateIsGay.Trim().Replace("'", "''"))}')");

            uri = Radzen.ODataExtensions.GetODataUri(uri: uri, filter:null, top:null, skip:null, orderby:null, expand:expand, select:null, count:null);

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);

            OnGetTestTable2ByNateIsGay(httpRequestMessage);

            var response = await httpClient.SendAsync(httpRequestMessage);

            return await Radzen.HttpResponseMessageExtensions.ReadAsync<RadzenTest.Server.Models.DevOps_Proj_Database.TestTable2>(response);
        }

        partial void OnUpdateTestTable2(HttpRequestMessage requestMessage);
        
        public async Task<HttpResponseMessage> UpdateTestTable2(string nateIsGay = default(string), RadzenTest.Server.Models.DevOps_Proj_Database.TestTable2 testTable2 = default(RadzenTest.Server.Models.DevOps_Proj_Database.TestTable2))
        {
            var uri = new Uri(baseUri, $"TestTable2S('{HttpUtility.UrlEncode(nateIsGay.Trim().Replace("'", "''"))}')");

            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Patch, uri);


            httpRequestMessage.Content = new StringContent(Radzen.ODataJsonSerializer.Serialize(testTable2), Encoding.UTF8, "application/json");

            OnUpdateTestTable2(httpRequestMessage);

            return await httpClient.SendAsync(httpRequestMessage);
        }
    }
}