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

using RadzenTest.Server.Data;

namespace RadzenTest.Server
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


        public async Task ExportTestTablesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/devops_proj_database/testtables/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/devops_proj_database/testtables/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportTestTablesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/devops_proj_database/testtables/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/devops_proj_database/testtables/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnTestTablesRead(ref IQueryable<RadzenTest.Server.Models.DevOps_Proj_Database.TestTable> items);

        public async Task<IQueryable<RadzenTest.Server.Models.DevOps_Proj_Database.TestTable>> GetTestTables(Query query = null)
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

        partial void OnTestTableGet(RadzenTest.Server.Models.DevOps_Proj_Database.TestTable item);

        public async Task<RadzenTest.Server.Models.DevOps_Proj_Database.TestTable> GetTestTableByTest(string test)
        {
            var items = Context.TestTables
                              .AsNoTracking()
                              .Where(i => i.Test == test);

  
            var itemToReturn = items.FirstOrDefault();

            OnTestTableGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnTestTableCreated(RadzenTest.Server.Models.DevOps_Proj_Database.TestTable item);
        partial void OnAfterTestTableCreated(RadzenTest.Server.Models.DevOps_Proj_Database.TestTable item);

        public async Task<RadzenTest.Server.Models.DevOps_Proj_Database.TestTable> CreateTestTable(RadzenTest.Server.Models.DevOps_Proj_Database.TestTable testtable)
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

        public async Task<RadzenTest.Server.Models.DevOps_Proj_Database.TestTable> CancelTestTableChanges(RadzenTest.Server.Models.DevOps_Proj_Database.TestTable item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnTestTableUpdated(RadzenTest.Server.Models.DevOps_Proj_Database.TestTable item);
        partial void OnAfterTestTableUpdated(RadzenTest.Server.Models.DevOps_Proj_Database.TestTable item);

        public async Task<RadzenTest.Server.Models.DevOps_Proj_Database.TestTable> UpdateTestTable(string test, RadzenTest.Server.Models.DevOps_Proj_Database.TestTable testtable)
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

        partial void OnTestTableDeleted(RadzenTest.Server.Models.DevOps_Proj_Database.TestTable item);
        partial void OnAfterTestTableDeleted(RadzenTest.Server.Models.DevOps_Proj_Database.TestTable item);

        public async Task<RadzenTest.Server.Models.DevOps_Proj_Database.TestTable> DeleteTestTable(string test)
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

        partial void OnTestTable2SRead(ref IQueryable<RadzenTest.Server.Models.DevOps_Proj_Database.TestTable2> items);

        public async Task<IQueryable<RadzenTest.Server.Models.DevOps_Proj_Database.TestTable2>> GetTestTable2S(Query query = null)
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

        partial void OnTestTable2Get(RadzenTest.Server.Models.DevOps_Proj_Database.TestTable2 item);

        public async Task<RadzenTest.Server.Models.DevOps_Proj_Database.TestTable2> GetTestTable2ByNateIsGay(string nateisgay)
        {
            var items = Context.TestTable2S
                              .AsNoTracking()
                              .Where(i => i.NateIsGay == nateisgay);

  
            var itemToReturn = items.FirstOrDefault();

            OnTestTable2Get(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnTestTable2Created(RadzenTest.Server.Models.DevOps_Proj_Database.TestTable2 item);
        partial void OnAfterTestTable2Created(RadzenTest.Server.Models.DevOps_Proj_Database.TestTable2 item);

        public async Task<RadzenTest.Server.Models.DevOps_Proj_Database.TestTable2> CreateTestTable2(RadzenTest.Server.Models.DevOps_Proj_Database.TestTable2 testtable2)
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

        public async Task<RadzenTest.Server.Models.DevOps_Proj_Database.TestTable2> CancelTestTable2Changes(RadzenTest.Server.Models.DevOps_Proj_Database.TestTable2 item)
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
              entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
              entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnTestTable2Updated(RadzenTest.Server.Models.DevOps_Proj_Database.TestTable2 item);
        partial void OnAfterTestTable2Updated(RadzenTest.Server.Models.DevOps_Proj_Database.TestTable2 item);

        public async Task<RadzenTest.Server.Models.DevOps_Proj_Database.TestTable2> UpdateTestTable2(string nateisgay, RadzenTest.Server.Models.DevOps_Proj_Database.TestTable2 testtable2)
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

        partial void OnTestTable2Deleted(RadzenTest.Server.Models.DevOps_Proj_Database.TestTable2 item);
        partial void OnAfterTestTable2Deleted(RadzenTest.Server.Models.DevOps_Proj_Database.TestTable2 item);

        public async Task<RadzenTest.Server.Models.DevOps_Proj_Database.TestTable2> DeleteTestTable2(string nateisgay)
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
        }
}