using System;
using System.Net;
using System.Data;
using System.Linq;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Formatter;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace RadzenTest.Server.Controllers.DevOps_Proj_Database
{
    [Route("odata/DevOps_Proj_Database/TestTables")]
    public partial class TestTablesController : ODataController
    {
        private RadzenTest.Server.Data.DevOps_Proj_DatabaseContext context;

        public TestTablesController(RadzenTest.Server.Data.DevOps_Proj_DatabaseContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<RadzenTest.Server.Models.DevOps_Proj_Database.TestTable> GetTestTables()
        {
            var items = this.context.TestTables.AsQueryable<RadzenTest.Server.Models.DevOps_Proj_Database.TestTable>();
            this.OnTestTablesRead(ref items);

            return items;
        }

        partial void OnTestTablesRead(ref IQueryable<RadzenTest.Server.Models.DevOps_Proj_Database.TestTable> items);

        partial void OnTestTableGet(ref SingleResult<RadzenTest.Server.Models.DevOps_Proj_Database.TestTable> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/DevOps_Proj_Database/TestTables(Test={Test})")]
        public SingleResult<RadzenTest.Server.Models.DevOps_Proj_Database.TestTable> GetTestTable(string key)
        {
            var items = this.context.TestTables.Where(i => i.Test == Uri.UnescapeDataString(key));
            var result = SingleResult.Create(items);

            OnTestTableGet(ref result);

            return result;
        }
        partial void OnTestTableDeleted(RadzenTest.Server.Models.DevOps_Proj_Database.TestTable item);
        partial void OnAfterTestTableDeleted(RadzenTest.Server.Models.DevOps_Proj_Database.TestTable item);

        [HttpDelete("/odata/DevOps_Proj_Database/TestTables(Test={Test})")]
        public IActionResult DeleteTestTable(string key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.TestTables
                    .Where(i => i.Test == Uri.UnescapeDataString(key))
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnTestTableDeleted(item);
                this.context.TestTables.Remove(item);
                this.context.SaveChanges();
                this.OnAfterTestTableDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnTestTableUpdated(RadzenTest.Server.Models.DevOps_Proj_Database.TestTable item);
        partial void OnAfterTestTableUpdated(RadzenTest.Server.Models.DevOps_Proj_Database.TestTable item);

        [HttpPut("/odata/DevOps_Proj_Database/TestTables(Test={Test})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutTestTable(string key, [FromBody]RadzenTest.Server.Models.DevOps_Proj_Database.TestTable item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (item == null || (item.Test != Uri.UnescapeDataString(key)))
                {
                    return BadRequest();
                }
                this.OnTestTableUpdated(item);
                this.context.TestTables.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.TestTables.Where(i => i.Test == Uri.UnescapeDataString(key));
                ;
                this.OnAfterTestTableUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/DevOps_Proj_Database/TestTables(Test={Test})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchTestTable(string key, [FromBody]Delta<RadzenTest.Server.Models.DevOps_Proj_Database.TestTable> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.TestTables.Where(i => i.Test == Uri.UnescapeDataString(key)).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnTestTableUpdated(item);
                this.context.TestTables.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.TestTables.Where(i => i.Test == Uri.UnescapeDataString(key));
                ;
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnTestTableCreated(RadzenTest.Server.Models.DevOps_Proj_Database.TestTable item);
        partial void OnAfterTestTableCreated(RadzenTest.Server.Models.DevOps_Proj_Database.TestTable item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] RadzenTest.Server.Models.DevOps_Proj_Database.TestTable item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (item == null)
                {
                    return BadRequest();
                }

                this.OnTestTableCreated(item);
                this.context.TestTables.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.TestTables.Where(i => i.Test == item.Test);

                ;

                this.OnAfterTestTableCreated(item);

                return new ObjectResult(SingleResult.Create(itemToReturn))
                {
                    StatusCode = 201
                };
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }
    }
}
