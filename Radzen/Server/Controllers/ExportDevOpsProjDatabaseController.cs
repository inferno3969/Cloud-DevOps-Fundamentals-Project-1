using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

using RadzenTest.Server.Data;

namespace RadzenTest.Server.Controllers
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
    }
}
