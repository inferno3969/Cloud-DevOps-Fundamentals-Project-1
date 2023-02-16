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
    [Route("odata/DevOps_Proj_Database/Positions")]
    public partial class PositionsController : ODataController
    {
        private CloudDevOpsProject1.Server.Data.DevOps_Proj_DatabaseContext context;

        public PositionsController(CloudDevOpsProject1.Server.Data.DevOps_Proj_DatabaseContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Position> GetPositions()
        {
            var items = this.context.Positions.AsQueryable<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Position>();
            this.OnPositionsRead(ref items);

            return items;
        }

        partial void OnPositionsRead(ref IQueryable<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Position> items);

        partial void OnPositionGet(ref SingleResult<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Position> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/DevOps_Proj_Database/Positions(Pos_ID={Pos_ID})")]
        public SingleResult<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Position> GetPosition(int key)
        {
            var items = this.context.Positions.Where(i => i.Pos_ID == key);
            var result = SingleResult.Create(items);

            OnPositionGet(ref result);

            return result;
        }
        partial void OnPositionDeleted(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Position item);
        partial void OnAfterPositionDeleted(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Position item);

        [HttpDelete("/odata/DevOps_Proj_Database/Positions(Pos_ID={Pos_ID})")]
        public IActionResult DeletePosition(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var items = this.context.Positions
                    .Where(i => i.Pos_ID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Position>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnPositionDeleted(item);
                this.context.Positions.Remove(item);
                this.context.SaveChanges();
                this.OnAfterPositionDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnPositionUpdated(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Position item);
        partial void OnAfterPositionUpdated(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Position item);

        [HttpPut("/odata/DevOps_Proj_Database/Positions(Pos_ID={Pos_ID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutPosition(int key, [FromBody]CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Position item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.Positions
                    .Where(i => i.Pos_ID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Position>(Request, items);

                var firstItem = items.FirstOrDefault();

                if (firstItem == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                this.OnPositionUpdated(item);
                this.context.Positions.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Positions.Where(i => i.Pos_ID == key);
                ;
                this.OnAfterPositionUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/DevOps_Proj_Database/Positions(Pos_ID={Pos_ID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchPosition(int key, [FromBody]Delta<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Position> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var items = this.context.Positions
                    .Where(i => i.Pos_ID == key)
                    .AsQueryable();

                items = Data.EntityPatch.ApplyTo<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Position>(Request, items);

                var item = items.FirstOrDefault();

                if (item == null)
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
                patch.Patch(item);

                this.OnPositionUpdated(item);
                this.context.Positions.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Positions.Where(i => i.Pos_ID == key);
                ;
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnPositionCreated(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Position item);
        partial void OnAfterPositionCreated(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Position item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Position item)
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

                this.OnPositionCreated(item);
                this.context.Positions.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Positions.Where(i => i.Pos_ID == item.Pos_ID);

                ;

                this.OnAfterPositionCreated(item);

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
