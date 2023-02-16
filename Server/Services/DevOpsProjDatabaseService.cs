using System;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Radzen;

using CloudDevOpsProject1.Server.Data;

namespace CloudDevOpsProject1.Server
{
    public partial class DevOps_Proj_DatabaseService
    {
        DevOps_Proj_DatabaseContext Context
        {
           get
           {
             return this.context;
           }
        }

        private readonly DevOps_Proj_DatabaseContext context;
        private readonly NavigationManager navigationManager;

        public DevOps_Proj_DatabaseService(DevOps_Proj_DatabaseContext context, NavigationManager navigationManager)
        {
            this.context = context;
            this.navigationManager = navigationManager;
        }

        public void Reset() => Context.ChangeTracker.Entries().Where(e => e.Entity != null).ToList().ForEach(e => e.State = EntityState.Detached);


        public async Task ExportEmployeesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/devops_proj_database/employees/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/devops_proj_database/employees/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportEmployeesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/devops_proj_database/employees/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/devops_proj_database/employees/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnEmployeesRead(ref IQueryable<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Employee> items);

        public async Task<IQueryable<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Employee>> GetEmployees(Query query = null)
        {
            var items = Context.Employees.AsQueryable();

            items = items.Include(i => i.Plant);
            items = items.Include(i => i.Position);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnEmployeesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnEmployeeGet(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Employee item);

        public async Task<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Employee> GetEmployeeByEmpId(int empid)
        {
            var items = Context.Employees
                              .AsNoTracking()
                              .Where(i => i.Emp_ID == empid);

            items = items.Include(i => i.Plant);
            items = items.Include(i => i.Position);
  
            var itemToReturn = items.FirstOrDefault();

            OnEmployeeGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnEmployeeCreated(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Employee item);
        partial void OnAfterEmployeeCreated(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Employee item);

        public async Task<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Employee> CreateEmployee(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Employee employee)
        {
            OnEmployeeCreated(employee);

            var existingItem = Context.Employees
                              .Where(i => i.Emp_ID == employee.Emp_ID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Employees.Add(employee);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(employee).State = EntityState.Detached;
                throw;
            }

            OnAfterEmployeeCreated(employee);

            return employee;
        }

        public async Task<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Employee> CancelEmployeeChanges(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Employee item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnEmployeeUpdated(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Employee item);
        partial void OnAfterEmployeeUpdated(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Employee item);

        public async Task<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Employee> UpdateEmployee(int empid, CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Employee employee)
        {
            OnEmployeeUpdated(employee);

            var itemToUpdate = Context.Employees
                              .Where(i => i.Emp_ID == employee.Emp_ID)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(employee);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterEmployeeUpdated(employee);

            return employee;
        }

        partial void OnEmployeeDeleted(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Employee item);
        partial void OnAfterEmployeeDeleted(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Employee item);

        public async Task<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Employee> DeleteEmployee(int empid)
        {
            var itemToDelete = Context.Employees
                              .Where(i => i.Emp_ID == empid)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnEmployeeDeleted(itemToDelete);


            Context.Employees.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterEmployeeDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportInventoriesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/devops_proj_database/inventories/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/devops_proj_database/inventories/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportInventoriesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/devops_proj_database/inventories/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/devops_proj_database/inventories/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnInventoriesRead(ref IQueryable<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Inventory> items);

        public async Task<IQueryable<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Inventory>> GetInventories(Query query = null)
        {
            var items = Context.Inventories.AsQueryable();

            items = items.Include(i => i.Part);
            items = items.Include(i => i.Plant);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnInventoriesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnInventoryGet(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Inventory item);

        public async Task<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Inventory> GetInventoryByInvId(int invid)
        {
            var items = Context.Inventories
                              .AsNoTracking()
                              .Where(i => i.Inv_ID == invid);

            items = items.Include(i => i.Part);
            items = items.Include(i => i.Plant);
  
            var itemToReturn = items.FirstOrDefault();

            OnInventoryGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnInventoryCreated(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Inventory item);
        partial void OnAfterInventoryCreated(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Inventory item);

        public async Task<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Inventory> CreateInventory(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Inventory inventory)
        {
            OnInventoryCreated(inventory);

            var existingItem = Context.Inventories
                              .Where(i => i.Inv_ID == inventory.Inv_ID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Inventories.Add(inventory);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(inventory).State = EntityState.Detached;
                throw;
            }

            OnAfterInventoryCreated(inventory);

            return inventory;
        }

        public async Task<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Inventory> CancelInventoryChanges(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Inventory item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnInventoryUpdated(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Inventory item);
        partial void OnAfterInventoryUpdated(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Inventory item);

        public async Task<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Inventory> UpdateInventory(int invid, CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Inventory inventory)
        {
            OnInventoryUpdated(inventory);

            var itemToUpdate = Context.Inventories
                              .Where(i => i.Inv_ID == inventory.Inv_ID)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(inventory);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterInventoryUpdated(inventory);

            return inventory;
        }

        partial void OnInventoryDeleted(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Inventory item);
        partial void OnAfterInventoryDeleted(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Inventory item);

        public async Task<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Inventory> DeleteInventory(int invid)
        {
            var itemToDelete = Context.Inventories
                              .Where(i => i.Inv_ID == invid)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnInventoryDeleted(itemToDelete);


            Context.Inventories.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterInventoryDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportManagersToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/devops_proj_database/managers/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/devops_proj_database/managers/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportManagersToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/devops_proj_database/managers/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/devops_proj_database/managers/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnManagersRead(ref IQueryable<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Manager> items);

        public async Task<IQueryable<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Manager>> GetManagers(Query query = null)
        {
            var items = Context.Managers.AsQueryable();

            items = items.Include(i => i.Employee);
            items = items.Include(i => i.Plant);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnManagersRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnManagerGet(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Manager item);

        public async Task<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Manager> GetManagerByManagerId(int managerid)
        {
            var items = Context.Managers
                              .AsNoTracking()
                              .Where(i => i.Manager_ID == managerid);

            items = items.Include(i => i.Employee);
            items = items.Include(i => i.Plant);
  
            var itemToReturn = items.FirstOrDefault();

            OnManagerGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnManagerCreated(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Manager item);
        partial void OnAfterManagerCreated(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Manager item);

        public async Task<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Manager> CreateManager(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Manager manager)
        {
            OnManagerCreated(manager);

            var existingItem = Context.Managers
                              .Where(i => i.Manager_ID == manager.Manager_ID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Managers.Add(manager);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(manager).State = EntityState.Detached;
                throw;
            }

            OnAfterManagerCreated(manager);

            return manager;
        }

        public async Task<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Manager> CancelManagerChanges(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Manager item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnManagerUpdated(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Manager item);
        partial void OnAfterManagerUpdated(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Manager item);

        public async Task<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Manager> UpdateManager(int managerid, CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Manager manager)
        {
            OnManagerUpdated(manager);

            var itemToUpdate = Context.Managers
                              .Where(i => i.Manager_ID == manager.Manager_ID)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(manager);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterManagerUpdated(manager);

            return manager;
        }

        partial void OnManagerDeleted(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Manager item);
        partial void OnAfterManagerDeleted(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Manager item);

        public async Task<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Manager> DeleteManager(int managerid)
        {
            var itemToDelete = Context.Managers
                              .Where(i => i.Manager_ID == managerid)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnManagerDeleted(itemToDelete);


            Context.Managers.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterManagerDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportPartsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/devops_proj_database/parts/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/devops_proj_database/parts/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportPartsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/devops_proj_database/parts/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/devops_proj_database/parts/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnPartsRead(ref IQueryable<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Part> items);

        public async Task<IQueryable<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Part>> GetParts(Query query = null)
        {
            var items = Context.Parts.AsQueryable();

            items = items.Include(i => i.Vendor);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnPartsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnPartGet(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Part item);

        public async Task<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Part> GetPartByPartId(int partid)
        {
            var items = Context.Parts
                              .AsNoTracking()
                              .Where(i => i.Part_ID == partid);

            items = items.Include(i => i.Vendor);
  
            var itemToReturn = items.FirstOrDefault();

            OnPartGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnPartCreated(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Part item);
        partial void OnAfterPartCreated(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Part item);

        public async Task<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Part> CreatePart(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Part part)
        {
            OnPartCreated(part);

            var existingItem = Context.Parts
                              .Where(i => i.Part_ID == part.Part_ID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Parts.Add(part);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(part).State = EntityState.Detached;
                throw;
            }

            OnAfterPartCreated(part);

            return part;
        }

        public async Task<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Part> CancelPartChanges(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Part item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnPartUpdated(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Part item);
        partial void OnAfterPartUpdated(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Part item);

        public async Task<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Part> UpdatePart(int partid, CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Part part)
        {
            OnPartUpdated(part);

            var itemToUpdate = Context.Parts
                              .Where(i => i.Part_ID == part.Part_ID)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(part);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterPartUpdated(part);

            return part;
        }

        partial void OnPartDeleted(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Part item);
        partial void OnAfterPartDeleted(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Part item);

        public async Task<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Part> DeletePart(int partid)
        {
            var itemToDelete = Context.Parts
                              .Where(i => i.Part_ID == partid)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnPartDeleted(itemToDelete);


            Context.Parts.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterPartDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportPlantsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/devops_proj_database/plants/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/devops_proj_database/plants/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportPlantsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/devops_proj_database/plants/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/devops_proj_database/plants/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnPlantsRead(ref IQueryable<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Plant> items);

        public async Task<IQueryable<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Plant>> GetPlants(Query query = null)
        {
            var items = Context.Plants.AsQueryable();

            items = items.Include(i => i.Manager);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnPlantsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnPlantGet(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Plant item);

        public async Task<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Plant> GetPlantByPlantId(int plantid)
        {
            var items = Context.Plants
                              .AsNoTracking()
                              .Where(i => i.Plant_ID == plantid);

            items = items.Include(i => i.Manager);
  
            var itemToReturn = items.FirstOrDefault();

            OnPlantGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnPlantCreated(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Plant item);
        partial void OnAfterPlantCreated(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Plant item);

        public async Task<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Plant> CreatePlant(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Plant plant)
        {
            OnPlantCreated(plant);

            var existingItem = Context.Plants
                              .Where(i => i.Plant_ID == plant.Plant_ID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Plants.Add(plant);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(plant).State = EntityState.Detached;
                throw;
            }

            OnAfterPlantCreated(plant);

            return plant;
        }

        public async Task<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Plant> CancelPlantChanges(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Plant item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnPlantUpdated(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Plant item);
        partial void OnAfterPlantUpdated(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Plant item);

        public async Task<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Plant> UpdatePlant(int plantid, CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Plant plant)
        {
            OnPlantUpdated(plant);

            var itemToUpdate = Context.Plants
                              .Where(i => i.Plant_ID == plant.Plant_ID)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(plant);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterPlantUpdated(plant);

            return plant;
        }

        partial void OnPlantDeleted(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Plant item);
        partial void OnAfterPlantDeleted(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Plant item);

        public async Task<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Plant> DeletePlant(int plantid)
        {
            var itemToDelete = Context.Plants
                              .Where(i => i.Plant_ID == plantid)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnPlantDeleted(itemToDelete);


            Context.Plants.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterPlantDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportPositionsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/devops_proj_database/positions/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/devops_proj_database/positions/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportPositionsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/devops_proj_database/positions/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/devops_proj_database/positions/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnPositionsRead(ref IQueryable<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Position> items);

        public async Task<IQueryable<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Position>> GetPositions(Query query = null)
        {
            var items = Context.Positions.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnPositionsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnPositionGet(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Position item);

        public async Task<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Position> GetPositionByPosId(int posid)
        {
            var items = Context.Positions
                              .AsNoTracking()
                              .Where(i => i.Pos_ID == posid);

  
            var itemToReturn = items.FirstOrDefault();

            OnPositionGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnPositionCreated(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Position item);
        partial void OnAfterPositionCreated(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Position item);

        public async Task<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Position> CreatePosition(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Position position)
        {
            OnPositionCreated(position);

            var existingItem = Context.Positions
                              .Where(i => i.Pos_ID == position.Pos_ID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Positions.Add(position);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(position).State = EntityState.Detached;
                throw;
            }

            OnAfterPositionCreated(position);

            return position;
        }

        public async Task<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Position> CancelPositionChanges(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Position item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnPositionUpdated(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Position item);
        partial void OnAfterPositionUpdated(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Position item);

        public async Task<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Position> UpdatePosition(int posid, CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Position position)
        {
            OnPositionUpdated(position);

            var itemToUpdate = Context.Positions
                              .Where(i => i.Pos_ID == position.Pos_ID)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(position);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterPositionUpdated(position);

            return position;
        }

        partial void OnPositionDeleted(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Position item);
        partial void OnAfterPositionDeleted(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Position item);

        public async Task<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Position> DeletePosition(int posid)
        {
            var itemToDelete = Context.Positions
                              .Where(i => i.Pos_ID == posid)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnPositionDeleted(itemToDelete);


            Context.Positions.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterPositionDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportTestTablesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/devops_proj_database/testtables/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/devops_proj_database/testtables/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportTestTablesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/devops_proj_database/testtables/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/devops_proj_database/testtables/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnTestTablesRead(ref IQueryable<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.TestTable> items);

        public async Task<IQueryable<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.TestTable>> GetTestTables(Query query = null)
        {
            var items = Context.TestTables.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnTestTablesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnTestTableGet(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.TestTable item);

        public async Task<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.TestTable> GetTestTableByTest(string test)
        {
            var items = Context.TestTables
                              .AsNoTracking()
                              .Where(i => i.Test == test);

  
            var itemToReturn = items.FirstOrDefault();

            OnTestTableGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnTestTableCreated(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.TestTable item);
        partial void OnAfterTestTableCreated(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.TestTable item);

        public async Task<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.TestTable> CreateTestTable(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.TestTable testtable)
        {
            OnTestTableCreated(testtable);

            var existingItem = Context.TestTables
                              .Where(i => i.Test == testtable.Test)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.TestTables.Add(testtable);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(testtable).State = EntityState.Detached;
                throw;
            }

            OnAfterTestTableCreated(testtable);

            return testtable;
        }

        public async Task<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.TestTable> CancelTestTableChanges(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.TestTable item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnTestTableUpdated(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.TestTable item);
        partial void OnAfterTestTableUpdated(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.TestTable item);

        public async Task<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.TestTable> UpdateTestTable(string test, CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.TestTable testtable)
        {
            OnTestTableUpdated(testtable);

            var itemToUpdate = Context.TestTables
                              .Where(i => i.Test == testtable.Test)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(testtable);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterTestTableUpdated(testtable);

            return testtable;
        }

        partial void OnTestTableDeleted(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.TestTable item);
        partial void OnAfterTestTableDeleted(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.TestTable item);

        public async Task<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.TestTable> DeleteTestTable(string test)
        {
            var itemToDelete = Context.TestTables
                              .Where(i => i.Test == test)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnTestTableDeleted(itemToDelete);


            Context.TestTables.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterTestTableDeleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportTestTable2SToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/devops_proj_database/testtable2s/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/devops_proj_database/testtable2s/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportTestTable2SToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/devops_proj_database/testtable2s/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/devops_proj_database/testtable2s/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnTestTable2SRead(ref IQueryable<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.TestTable2> items);

        public async Task<IQueryable<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.TestTable2>> GetTestTable2S(Query query = null)
        {
            var items = Context.TestTable2S.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnTestTable2SRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnTestTable2Get(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.TestTable2 item);

        public async Task<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.TestTable2> GetTestTable2ByNateIsGay(string nateisgay)
        {
            var items = Context.TestTable2S
                              .AsNoTracking()
                              .Where(i => i.NateIsGay == nateisgay);

  
            var itemToReturn = items.FirstOrDefault();

            OnTestTable2Get(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnTestTable2Created(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.TestTable2 item);
        partial void OnAfterTestTable2Created(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.TestTable2 item);

        public async Task<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.TestTable2> CreateTestTable2(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.TestTable2 testtable2)
        {
            OnTestTable2Created(testtable2);

            var existingItem = Context.TestTable2S
                              .Where(i => i.NateIsGay == testtable2.NateIsGay)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.TestTable2S.Add(testtable2);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(testtable2).State = EntityState.Detached;
                throw;
            }

            OnAfterTestTable2Created(testtable2);

            return testtable2;
        }

        public async Task<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.TestTable2> CancelTestTable2Changes(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.TestTable2 item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnTestTable2Updated(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.TestTable2 item);
        partial void OnAfterTestTable2Updated(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.TestTable2 item);

        public async Task<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.TestTable2> UpdateTestTable2(string nateisgay, CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.TestTable2 testtable2)
        {
            OnTestTable2Updated(testtable2);

            var itemToUpdate = Context.TestTable2S
                              .Where(i => i.NateIsGay == testtable2.NateIsGay)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(testtable2);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterTestTable2Updated(testtable2);

            return testtable2;
        }

        partial void OnTestTable2Deleted(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.TestTable2 item);
        partial void OnAfterTestTable2Deleted(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.TestTable2 item);

        public async Task<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.TestTable2> DeleteTestTable2(string nateisgay)
        {
            var itemToDelete = Context.TestTable2S
                              .Where(i => i.NateIsGay == nateisgay)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnTestTable2Deleted(itemToDelete);


            Context.TestTable2S.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterTestTable2Deleted(itemToDelete);

            return itemToDelete;
        }
    
        public async Task ExportVendorsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/devops_proj_database/vendors/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/devops_proj_database/vendors/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportVendorsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/devops_proj_database/vendors/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/devops_proj_database/vendors/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnVendorsRead(ref IQueryable<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Vendor> items);

        public async Task<IQueryable<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Vendor>> GetVendors(Query query = null)
        {
            var items = Context.Vendors.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnVendorsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnVendorGet(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Vendor item);

        public async Task<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Vendor> GetVendorByVendorId(int vendorid)
        {
            var items = Context.Vendors
                              .AsNoTracking()
                              .Where(i => i.Vendor_ID == vendorid);

  
            var itemToReturn = items.FirstOrDefault();

            OnVendorGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnVendorCreated(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Vendor item);
        partial void OnAfterVendorCreated(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Vendor item);

        public async Task<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Vendor> CreateVendor(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Vendor vendor)
        {
            OnVendorCreated(vendor);

            var existingItem = Context.Vendors
                              .Where(i => i.Vendor_ID == vendor.Vendor_ID)
                              .FirstOrDefault();

            if (existingItem != null)
            {
               throw new Exception("Item already available");
            }            

            try
            {
                Context.Vendors.Add(vendor);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(vendor).State = EntityState.Detached;
                throw;
            }

            OnAfterVendorCreated(vendor);

            return vendor;
        }

        public async Task<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Vendor> CancelVendorChanges(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Vendor item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnVendorUpdated(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Vendor item);
        partial void OnAfterVendorUpdated(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Vendor item);

        public async Task<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Vendor> UpdateVendor(int vendorid, CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Vendor vendor)
        {
            OnVendorUpdated(vendor);

            var itemToUpdate = Context.Vendors
                              .Where(i => i.Vendor_ID == vendor.Vendor_ID)
                              .FirstOrDefault();

            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }
                
            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(vendor);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterVendorUpdated(vendor);

            return vendor;
        }

        partial void OnVendorDeleted(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Vendor item);
        partial void OnAfterVendorDeleted(CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Vendor item);

        public async Task<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Vendor> DeleteVendor(int vendorid)
        {
            var itemToDelete = Context.Vendors
                              .Where(i => i.Vendor_ID == vendorid)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnVendorDeleted(itemToDelete);


            Context.Vendors.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterVendorDeleted(itemToDelete);

            return itemToDelete;
        }
        }
}