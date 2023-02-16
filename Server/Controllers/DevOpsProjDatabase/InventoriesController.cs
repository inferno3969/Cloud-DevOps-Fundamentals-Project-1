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
    [Route("odata/DevOps_Proj_Database/Inventories")]
    public partial class InventoriesController : ODataController
    {
        private CloudDevOpsProject1.Server.Data.DevOps_Proj_DatabaseContext context;

        public InventoriesController(CloudDevOpsProject1.Server.Data.DevOps_Proj_DatabaseContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Inventory> GetInventories()
        {
            var items = this.context.Inventories.AsQueryable<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Inventory>();
            this.OnInventoriesRead(ref items);

            return items;
        }

        partial void OnInventoriesRead(ref IQueryable<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Inventory> items);

        partial void OnInventoryGet(ref SingleResult<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Inventory> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/DevOps_Proj_Database/Inventories(Inv_ID={Inv_ID})")]
        public SingleResult<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Inventory> GetInventory(int key)
        {
            var items = this.context.Inventories.Where(i => i.Inv_ID == key);
            var result = SingleResult.Create(items);

            OnInventoryGet(ref result);

            return result;
        }
        partial void OnInventoryDeleted(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Inventory item);
        partial void OnAfterInventoryDeleted(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Inventory item);

        [HttpDelete("/odata/DevOps_Proj_Database/Inventories(Inv_ID={Inv_ID})")]
        public IActionResult DeleteInventory(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var items = this.context.Inventories
                    .Where(i => i.Inv_ID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Inventory>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnInventoryDeleted(item);
                this.context.Inventories.Remove(item);
                this.context.SaveChanges();
                this.OnAfterInventoryDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnInventoryUpdated(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Inventory item);
        partial void OnAfterInventoryUpdated(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Inventory item);

        [HttpPut("/odata/DevOps_Proj_Database/Inventories(Inv_ID={Inv_ID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutInventory(int key, [FromBody]CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Inventory item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.Inventories
                    .Where(i => i.Inv_ID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Inventory>(Request, items);

                var firstItem = items.FirstOrDefault();

                if (firstItem == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnInventoryUpdated(item);
                this.context.Inventories.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Inventories.Where(i => i.Inv_ID == key);
                Request.QueryString = Request.QueryString.Add("$expand", "Part,Plant");
                this.OnAfterInventoryUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/DevOps_Proj_Database/Inventories(Inv_ID={Inv_ID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchInventory(int key, [FromBody]Delta<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Inventory> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.Inventories
                    .Where(i => i.Inv_ID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Inventory>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                patch.Patch(item);

                this.OnInventoryUpdated(item);
                this.context.Inventories.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Inventories.Where(i => i.Inv_ID == key);
                Request.QueryString = Request.QueryString.Add("$expand", "Part,Plant");
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnInventoryCreated(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Inventory item);
        partial void OnAfterInventoryCreated(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Inventory item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Inventory item)
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

                this.OnInventoryCreated(item);
                this.context.Inventories.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Inventories.Where(i => i.Inv_ID == item.Inv_ID);

                Request.QueryString = Request.QueryString.Add("$expand", "Part,Plant");

                this.OnAfterInventoryCreated(item);

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
