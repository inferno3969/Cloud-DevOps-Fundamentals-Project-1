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

namespace CloudDevOpsProject1.Server.Controllers.DevOps_Proj_Database
{
    [Route("odata/DevOps_Proj_Database/Parts")]
    public partial class PartsController : ODataController
    {
        private CloudDevOpsProject1.Server.Data.DevOps_Proj_DatabaseContext context;

        public PartsController(CloudDevOpsProject1.Server.Data.DevOps_Proj_DatabaseContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Part> GetParts()
        {
            var items = this.context.Parts.AsQueryable<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Part>();
            this.OnPartsRead(ref items);

            return items;
        }

        partial void OnPartsRead(ref IQueryable<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Part> items);

        partial void OnPartGet(ref SingleResult<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Part> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/DevOps_Proj_Database/Parts(Part_ID={Part_ID})")]
        public SingleResult<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Part> GetPart(int key)
        {
            var items = this.context.Parts.Where(i => i.Part_ID == key);
            var result = SingleResult.Create(items);

            OnPartGet(ref result);

            return result;
        }
        partial void OnPartDeleted(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Part item);
        partial void OnAfterPartDeleted(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Part item);

        [HttpDelete("/odata/DevOps_Proj_Database/Parts(Part_ID={Part_ID})")]
        public IActionResult DeletePart(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.Parts
                    .Where(i => i.Part_ID == key)
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnPartDeleted(item);
                this.context.Parts.Remove(item);
                this.context.SaveChanges();
                this.OnAfterPartDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnPartUpdated(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Part item);
        partial void OnAfterPartUpdated(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Part item);

        [HttpPut("/odata/DevOps_Proj_Database/Parts(Part_ID={Part_ID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutPart(int key, [FromBody]CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Part item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (item == null || (item.Part_ID != key))
                {
                    return BadRequest();
                }
                this.OnPartUpdated(item);
                this.context.Parts.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Parts.Where(i => i.Part_ID == key);
                Request.QueryString = Request.QueryString.Add("$expand", "Inventory,Vendor");
                this.OnAfterPartUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/DevOps_Proj_Database/Parts(Part_ID={Part_ID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchPart(int key, [FromBody]Delta<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Part> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.Parts.Where(i => i.Part_ID == key).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnPartUpdated(item);
                this.context.Parts.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Parts.Where(i => i.Part_ID == key);
                Request.QueryString = Request.QueryString.Add("$expand", "Inventory,Vendor");
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnPartCreated(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Part item);
        partial void OnAfterPartCreated(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Part item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Part item)
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

                this.OnPartCreated(item);
                this.context.Parts.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Parts.Where(i => i.Part_ID == item.Part_ID);

                Request.QueryString = Request.QueryString.Add("$expand", "Inventory,Vendor");

                this.OnAfterPartCreated(item);

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
