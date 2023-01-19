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
    [Route("odata/DevOps_Proj_Database/TestTable2S")]
    public partial class TestTable2SController : ODataController
    {
        private RadzenTest.Server.Data.DevOps_Proj_DatabaseContext context;

        public TestTable2SController(RadzenTest.Server.Data.DevOps_Proj_DatabaseContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<RadzenTest.Server.Models.DevOps_Proj_Database.TestTable2> GetTestTable2S()
        {
            var items = this.context.TestTable2S.AsQueryable<RadzenTest.Server.Models.DevOps_Proj_Database.TestTable2>();
            this.OnTestTable2SRead(ref items);

            return items;
        }

        partial void OnTestTable2SRead(ref IQueryable<RadzenTest.Server.Models.DevOps_Proj_Database.TestTable2> items);

        partial void OnTestTable2Get(ref SingleResult<RadzenTest.Server.Models.DevOps_Proj_Database.TestTable2> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/DevOps_Proj_Database/TestTable2S(NateIsGay={NateIsGay})")]
        public SingleResult<RadzenTest.Server.Models.DevOps_Proj_Database.TestTable2> GetTestTable2(string key)
        {
            var items = this.context.TestTable2S.Where(i => i.NateIsGay == Uri.UnescapeDataString(key));
            var result = SingleResult.Create(items);

            OnTestTable2Get(ref result);

            return result;
        }
        partial void OnTestTable2Deleted(RadzenTest.Server.Models.DevOps_Proj_Database.TestTable2 item);
        partial void OnAfterTestTable2Deleted(RadzenTest.Server.Models.DevOps_Proj_Database.TestTable2 item);

        [HttpDelete("/odata/DevOps_Proj_Database/TestTable2S(NateIsGay={NateIsGay})")]
        public IActionResult DeleteTestTable2(string key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.TestTable2S
                    .Where(i => i.NateIsGay == Uri.UnescapeDataString(key))
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnTestTable2Deleted(item);
                this.context.TestTable2S.Remove(item);
                this.context.SaveChanges();
                this.OnAfterTestTable2Deleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnTestTable2Updated(RadzenTest.Server.Models.DevOps_Proj_Database.TestTable2 item);
        partial void OnAfterTestTable2Updated(RadzenTest.Server.Models.DevOps_Proj_Database.TestTable2 item);

        [HttpPut("/odata/DevOps_Proj_Database/TestTable2S(NateIsGay={NateIsGay})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutTestTable2(string key, [FromBody]RadzenTest.Server.Models.DevOps_Proj_Database.TestTable2 item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (item == null || (item.NateIsGay != Uri.UnescapeDataString(key)))
                {
                    return BadRequest();
                }
                this.OnTestTable2Updated(item);
                this.context.TestTable2S.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.TestTable2S.Where(i => i.NateIsGay == Uri.UnescapeDataString(key));
                ;
                this.OnAfterTestTable2Updated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/DevOps_Proj_Database/TestTable2S(NateIsGay={NateIsGay})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchTestTable2(string key, [FromBody]Delta<RadzenTest.Server.Models.DevOps_Proj_Database.TestTable2> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.TestTable2S.Where(i => i.NateIsGay == Uri.UnescapeDataString(key)).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnTestTable2Updated(item);
                this.context.TestTable2S.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.TestTable2S.Where(i => i.NateIsGay == Uri.UnescapeDataString(key));
                ;
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnTestTable2Created(RadzenTest.Server.Models.DevOps_Proj_Database.TestTable2 item);
        partial void OnAfterTestTable2Created(RadzenTest.Server.Models.DevOps_Proj_Database.TestTable2 item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] RadzenTest.Server.Models.DevOps_Proj_Database.TestTable2 item)
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

                this.OnTestTable2Created(item);
                this.context.TestTable2S.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.TestTable2S.Where(i => i.NateIsGay == item.NateIsGay);

                ;

                this.OnAfterTestTable2Created(item);

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
