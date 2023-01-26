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
    [Route("odata/DevOps_Proj_Database/Employees")]
    public partial class EmployeesController : ODataController
    {
        private CloudDevOpsProject1.Server.Data.DevOps_Proj_DatabaseContext context;

        public EmployeesController(CloudDevOpsProject1.Server.Data.DevOps_Proj_DatabaseContext context)
        {
            this.context = context;
        }

    
        [HttpGet]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IEnumerable<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Employee> GetEmployees()
        {
            var items = this.context.Employees.AsQueryable<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Employee>();
            this.OnEmployeesRead(ref items);

            return items;
        }

        partial void OnEmployeesRead(ref IQueryable<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Employee> items);

        partial void OnEmployeeGet(ref SingleResult<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Employee> item);

        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        [HttpGet("/odata/DevOps_Proj_Database/Employees(Emp_ID={Emp_ID})")]
        public SingleResult<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Employee> GetEmployee(int key)
        {
            var items = this.context.Employees.Where(i => i.Emp_ID == key);
            var result = SingleResult.Create(items);

            OnEmployeeGet(ref result);

            return result;
        }
        partial void OnEmployeeDeleted(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Employee item);
        partial void OnAfterEmployeeDeleted(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Employee item);

        [HttpDelete("/odata/DevOps_Proj_Database/Employees(Emp_ID={Emp_ID})")]
        public IActionResult DeleteEmployee(int key)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }


                var item = this.context.Employees
                    .Where(i => i.Emp_ID == key)
                    .FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                this.OnEmployeeDeleted(item);
                this.context.Employees.Remove(item);
                this.context.SaveChanges();
                this.OnAfterEmployeeDeleted(item);

                return new NoContentResult();

            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnEmployeeUpdated(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Employee item);
        partial void OnAfterEmployeeUpdated(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Employee item);

        [HttpPut("/odata/DevOps_Proj_Database/Employees(Emp_ID={Emp_ID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PutEmployee(int key, [FromBody]CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Employee item)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (item == null || (item.Emp_ID != key))
                {
                    return BadRequest();
                }
                this.OnEmployeeUpdated(item);
                this.context.Employees.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Employees.Where(i => i.Emp_ID == key);
                Request.QueryString = Request.QueryString.Add("$expand", "Plant,Position");
                this.OnAfterEmployeeUpdated(item);
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        [HttpPatch("/odata/DevOps_Proj_Database/Employees(Emp_ID={Emp_ID})")]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult PatchEmployee(int key, [FromBody]Delta<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Employee> patch)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var item = this.context.Employees.Where(i => i.Emp_ID == key).FirstOrDefault();

                if (item == null)
                {
                    return BadRequest();
                }
                patch.Patch(item);

                this.OnEmployeeUpdated(item);
                this.context.Employees.Update(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Employees.Where(i => i.Emp_ID == key);
                Request.QueryString = Request.QueryString.Add("$expand", "Plant,Position");
                return new ObjectResult(SingleResult.Create(itemToReturn));
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return BadRequest(ModelState);
            }
        }

        partial void OnEmployeeCreated(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Employee item);
        partial void OnAfterEmployeeCreated(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Employee item);

        [HttpPost]
        [EnableQuery(MaxExpansionDepth=10,MaxAnyAllExpressionDepth=10,MaxNodeCount=1000)]
        public IActionResult Post([FromBody] CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Employee item)
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

                this.OnEmployeeCreated(item);
                this.context.Employees.Add(item);
                this.context.SaveChanges();

                var itemToReturn = this.context.Employees.Where(i => i.Emp_ID == item.Emp_ID);

                Request.QueryString = Request.QueryString.Add("$expand", "Plant,Position");

                this.OnAfterEmployeeCreated(item);

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
