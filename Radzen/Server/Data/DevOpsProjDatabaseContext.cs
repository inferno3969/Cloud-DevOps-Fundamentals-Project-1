using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

using RadzenTest.Server.Models.DevOps_Proj_Database;

namespace RadzenTest.Server.Data
{
    public partial class DevOps_Proj_DatabaseContext : DbContext
    {
        public DevOps_Proj_DatabaseContext()
        {
        }

        public DevOps_Proj_DatabaseContext(DbContextOptions<DevOps_Proj_DatabaseContext> options) : base(options)
        {
        }

        partial void OnModelBuilding(ModelBuilder builder);

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            this.OnModelBuilding(builder);
        }

        public DbSet<RadzenTest.Server.Models.DevOps_Proj_Database.TestTable> TestTables { get; set; }

        public DbSet<RadzenTest.Server.Models.DevOps_Proj_Database.TestTable2> TestTable2S { get; set; }
    }
}