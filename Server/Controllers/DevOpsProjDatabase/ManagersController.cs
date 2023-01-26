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
    [Route("odata/DevOps_Proj_Database/Managers")]
    public partial class ManagersController : ODataController
    {
        private CloudDevOpsProject1.Server.Data.DevOps_Proj_DatabaseContext context;

        public ManagersController(CloudDevOpsProject1.Server.Data.DevOps_Proj_DatabaseContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Manager> GetManagers()
        {
            var items = this.context.Managers.AsQueryable<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Manager>();
            this.OnManagersRead(ref items);

            return items;
        }

        partial void OnManagersRead(ref IQueryable<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Manager> items);

        partial void OnManagerGet(ref SingleResult<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Manager> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/DevOps_Proj_Database/Managers(Manager_ID={Manager_ID})")]
        public SingleResult<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Manager> GetManager(int key)
        {
            var items = this.context.Managers.Where(i => i.Manager_ID == key);
            var result = SingleResult.Create(items);

            OnManagerGet(ref result);

            return result;
        }
        partial void OnManagerDeleted(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Manager item);
        partial void OnAfterManagerDeleted(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Manager item);

        [HttpDelete("/odata/DevOps_Proj_Database/Managers(Manager_ID={Manager_ID})")]
        public IActionResult DeleteManager(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.Managers
                    .Where(i => i.Manager_ID == key)
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnManagerDeleted(item);
                this.context.Managers.Remove(item);
                this.context.SaveChanges();
                this.OnAfterManagerDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnManagerUpdated(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Manager item);
        partial void OnAfterManagerUpdated(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Manager item);

        [HttpPut("/odata/DevOps_Proj_Database/Managers(Manager_ID={Manager_ID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutManager(int key, [FromBody]CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Manager item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (item == null || (item.Manager_ID != key))
                {
                    return BadRequest();
                }
                this.OnManagerUpdated(item);
                this.context.Managers.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Managers.Where(i => i.Manager_ID == key);
                Request.QueryString = Request.QueryString.Add("$expand", "Employee,Plant");
                this.OnAfterManagerUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/DevOps_Proj_Database/Managers(Manager_ID={Manager_ID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchManager(int key, [FromBody]Delta<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Manager> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.Managers.Where(i => i.Manager_ID == key).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnManagerUpdated(item);
                this.context.Managers.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Managers.Where(i => i.Manager_ID == key);
                Request.QueryString = Request.QueryString.Add("$expand", "Employee,Plant");
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnManagerCreated(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Manager item);
        partial void OnAfterManagerCreated(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Manager item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Manager item)
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

                this.OnManagerCreated(item);
                this.context.Managers.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Managers.Where(i => i.Manager_ID == item.Manager_ID);

                Request.QueryString = Request.QueryString.Add("$expand", "Employee,Plant");

                this.OnAfterManagerCreated(item);

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
