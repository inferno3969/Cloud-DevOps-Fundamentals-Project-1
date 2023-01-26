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
    [Route("odata/DevOps_Proj_Database/Vendors")]
    public partial class VendorsController : ODataController
    {
        private CloudDevOpsProject1.Server.Data.DevOps_Proj_DatabaseContext context;

        public VendorsController(CloudDevOpsProject1.Server.Data.DevOps_Proj_DatabaseContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Vendor> GetVendors()
        {
            var items = this.context.Vendors.AsQueryable<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Vendor>();
            this.OnVendorsRead(ref items);

            return items;
        }

        partial void OnVendorsRead(ref IQueryable<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Vendor> items);

        partial void OnVendorGet(ref SingleResult<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Vendor> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/DevOps_Proj_Database/Vendors(Vendor_ID={Vendor_ID})")]
        public SingleResult<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Vendor> GetVendor(int key)
        {
            var items = this.context.Vendors.Where(i => i.Vendor_ID == key);
            var result = SingleResult.Create(items);

            OnVendorGet(ref result);

            return result;
        }
        partial void OnVendorDeleted(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Vendor item);
        partial void OnAfterVendorDeleted(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Vendor item);

        [HttpDelete("/odata/DevOps_Proj_Database/Vendors(Vendor_ID={Vendor_ID})")]
        public IActionResult DeleteVendor(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.Vendors
                    .Where(i => i.Vendor_ID == key)
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnVendorDeleted(item);
                this.context.Vendors.Remove(item);
                this.context.SaveChanges();
                this.OnAfterVendorDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnVendorUpdated(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Vendor item);
        partial void OnAfterVendorUpdated(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Vendor item);

        [HttpPut("/odata/DevOps_Proj_Database/Vendors(Vendor_ID={Vendor_ID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutVendor(int key, [FromBody]CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Vendor item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (item == null || (item.Vendor_ID != key))
                {
                    return BadRequest();
                }
                this.OnVendorUpdated(item);
                this.context.Vendors.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Vendors.Where(i => i.Vendor_ID == key);
                ;
                this.OnAfterVendorUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/DevOps_Proj_Database/Vendors(Vendor_ID={Vendor_ID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchVendor(int key, [FromBody]Delta<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Vendor> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.Vendors.Where(i => i.Vendor_ID == key).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnVendorUpdated(item);
                this.context.Vendors.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Vendors.Where(i => i.Vendor_ID == key);
                ;
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnVendorCreated(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Vendor item);
        partial void OnAfterVendorCreated(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Vendor item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Vendor item)
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

                this.OnVendorCreated(item);
                this.context.Vendors.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Vendors.Where(i => i.Vendor_ID == item.Vendor_ID);

                ;

                this.OnAfterVendorCreated(item);

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
