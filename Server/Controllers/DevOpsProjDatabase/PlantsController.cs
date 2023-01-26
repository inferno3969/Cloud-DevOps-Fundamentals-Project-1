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
    [Route("odata/DevOps_Proj_Database/Plants")]
    public partial class PlantsController : ODataController
    {
        private CloudDevOpsProject1.Server.Data.DevOps_Proj_DatabaseContext context;

        public PlantsController(CloudDevOpsProject1.Server.Data.DevOps_Proj_DatabaseContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Plant> GetPlants()
        {
            var items = this.context.Plants.AsQueryable<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Plant>();
            this.OnPlantsRead(ref items);

            return items;
        }

        partial void OnPlantsRead(ref IQueryable<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Plant> items);

        partial void OnPlantGet(ref SingleResult<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Plant> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/DevOps_Proj_Database/Plants(Plant_ID={Plant_ID})")]
        public SingleResult<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Plant> GetPlant(int key)
        {
            var items = this.context.Plants.Where(i => i.Plant_ID == key);
            var result = SingleResult.Create(items);

            OnPlantGet(ref result);

            return result;
        }
        partial void OnPlantDeleted(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Plant item);
        partial void OnAfterPlantDeleted(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Plant item);

        [HttpDelete("/odata/DevOps_Proj_Database/Plants(Plant_ID={Plant_ID})")]
        public IActionResult DeletePlant(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.Plants
                    .Where(i => i.Plant_ID == key)
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnPlantDeleted(item);
                this.context.Plants.Remove(item);
                this.context.SaveChanges();
                this.OnAfterPlantDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnPlantUpdated(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Plant item);
        partial void OnAfterPlantUpdated(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Plant item);

        [HttpPut("/odata/DevOps_Proj_Database/Plants(Plant_ID={Plant_ID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutPlant(int key, [FromBody]CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Plant item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (item == null || (item.Plant_ID != key))
                {
                    return BadRequest();
                }
                this.OnPlantUpdated(item);
                this.context.Plants.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Plants.Where(i => i.Plant_ID == key);
                Request.QueryString = Request.QueryString.Add("$expand", "Manager");
                this.OnAfterPlantUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/DevOps_Proj_Database/Plants(Plant_ID={Plant_ID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchPlant(int key, [FromBody]Delta<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Plant> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.Plants.Where(i => i.Plant_ID == key).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnPlantUpdated(item);
                this.context.Plants.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Plants.Where(i => i.Plant_ID == key);
                Request.QueryString = Request.QueryString.Add("$expand", "Manager");
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnPlantCreated(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Plant item);
        partial void OnAfterPlantCreated(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Plant item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Plant item)
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

                this.OnPlantCreated(item);
                this.context.Plants.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Plants.Where(i => i.Plant_ID == item.Plant_ID);

                Request.QueryString = Request.QueryString.Add("$expand", "Manager");

                this.OnAfterPlantCreated(item);

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
