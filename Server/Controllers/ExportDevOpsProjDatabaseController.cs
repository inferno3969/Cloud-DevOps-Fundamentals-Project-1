using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

using CloudDevOpsProject1.Server.Data;

namespace CloudDevOpsProject1.Server.Controllers
{
    public partial class ExportDevOps_Proj_DatabaseController : ExportController
    {
        private readonly DevOps_Proj_DatabaseContext context;
        private readonly DevOps_Proj_DatabaseService service;

        public ExportDevOps_Proj_DatabaseController(DevOps_Proj_DatabaseContext context, DevOps_Proj_DatabaseService service)
        {
            this.service = service;
            this.context = context;
        }

        [HttpGet("/export/DevOps_Proj_Database/employees/csv")]
        [HttpGet("/export/DevOps_Proj_Database/employees/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportEmployeesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetEmployees(), Request.Query), fileName);
        }

        [HttpGet("/export/DevOps_Proj_Database/employees/excel")]
        [HttpGet("/export/DevOps_Proj_Database/employees/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportEmployeesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetEmployees(), Request.Query), fileName);
        }

        [HttpGet("/export/DevOps_Proj_Database/inventories/csv")]
        [HttpGet("/export/DevOps_Proj_Database/inventories/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportInventoriesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetInventories(), Request.Query), fileName);
        }

        [HttpGet("/export/DevOps_Proj_Database/inventories/excel")]
        [HttpGet("/export/DevOps_Proj_Database/inventories/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportInventoriesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetInventories(), Request.Query), fileName);
        }

        [HttpGet("/export/DevOps_Proj_Database/managers/csv")]
        [HttpGet("/export/DevOps_Proj_Database/managers/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportManagersToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetManagers(), Request.Query), fileName);
        }

        [HttpGet("/export/DevOps_Proj_Database/managers/excel")]
        [HttpGet("/export/DevOps_Proj_Database/managers/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportManagersToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetManagers(), Request.Query), fileName);
        }

        [HttpGet("/export/DevOps_Proj_Database/parts/csv")]
        [HttpGet("/export/DevOps_Proj_Database/parts/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportPartsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetParts(), Request.Query), fileName);
        }

        [HttpGet("/export/DevOps_Proj_Database/parts/excel")]
        [HttpGet("/export/DevOps_Proj_Database/parts/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportPartsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetParts(), Request.Query), fileName);
        }

        [HttpGet("/export/DevOps_Proj_Database/plants/csv")]
        [HttpGet("/export/DevOps_Proj_Database/plants/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportPlantsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetPlants(), Request.Query), fileName);
        }

        [HttpGet("/export/DevOps_Proj_Database/plants/excel")]
        [HttpGet("/export/DevOps_Proj_Database/plants/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportPlantsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetPlants(), Request.Query), fileName);
        }

        [HttpGet("/export/DevOps_Proj_Database/positions/csv")]
        [HttpGet("/export/DevOps_Proj_Database/positions/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportPositionsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetPositions(), Request.Query), fileName);
        }

        [HttpGet("/export/DevOps_Proj_Database/positions/excel")]
        [HttpGet("/export/DevOps_Proj_Database/positions/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportPositionsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetPositions(), Request.Query), fileName);
        }

        [HttpGet("/export/DevOps_Proj_Database/testtables/csv")]
        [HttpGet("/export/DevOps_Proj_Database/testtables/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportTestTablesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetTestTables(), Request.Query), fileName);
        }

        [HttpGet("/export/DevOps_Proj_Database/testtables/excel")]
        [HttpGet("/export/DevOps_Proj_Database/testtables/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportTestTablesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetTestTables(), Request.Query), fileName);
        }

        [HttpGet("/export/DevOps_Proj_Database/testtable2s/csv")]
        [HttpGet("/export/DevOps_Proj_Database/testtable2s/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportTestTable2SToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetTestTable2S(), Request.Query), fileName);
        }

        [HttpGet("/export/DevOps_Proj_Database/testtable2s/excel")]
        [HttpGet("/export/DevOps_Proj_Database/testtable2s/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportTestTable2SToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetTestTable2S(), Request.Query), fileName);
        }

        [HttpGet("/export/DevOps_Proj_Database/vendors/csv")]
        [HttpGet("/export/DevOps_Proj_Database/vendors/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportVendorsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetVendors(), Request.Query), fileName);
        }

        [HttpGet("/export/DevOps_Proj_Database/vendors/excel")]
        [HttpGet("/export/DevOps_Proj_Database/vendors/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportVendorsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetVendors(), Request.Query), fileName);
        }
    }
}
