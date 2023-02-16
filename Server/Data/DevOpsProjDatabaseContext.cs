using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

using CloudDevOpsProject1.Server.Models.DevOps_Proj_Database;

namespace CloudDevOpsProject1.Server.Data
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

            builder.Entity<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Employee>()
              .HasOne(i => i.Plant)
              .WithMany(i => i.Employees)
              .HasForeignKey(i => i.Plant_ID)
              .HasPrincipalKey(i => i.Plant_ID);

            builder.Entity<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Employee>()
              .HasOne(i => i.Position)
              .WithMany(i => i.Employees)
              .HasForeignKey(i => i.Pos_ID)
              .HasPrincipalKey(i => i.Pos_ID);

            builder.Entity<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Inventory>()
              .HasOne(i => i.Part)
              .WithMany(i => i.Inventories)
              .HasForeignKey(i => i.Part_ID)
              .HasPrincipalKey(i => i.Part_ID);

            builder.Entity<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Inventory>()
              .HasOne(i => i.Plant)
              .WithMany(i => i.Inventories)
              .HasForeignKey(i => i.Plant_ID)
              .HasPrincipalKey(i => i.Plant_ID);

            builder.Entity<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Manager>()
              .HasOne(i => i.Employee)
              .WithMany(i => i.Managers)
              .HasForeignKey(i => i.Emp_ID)
              .HasPrincipalKey(i => i.Emp_ID);

            builder.Entity<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Manager>()
              .HasOne(i => i.Plant)
              .WithMany(i => i.Managers)
              .HasForeignKey(i => i.Plant_ID)
              .HasPrincipalKey(i => i.Plant_ID);

            builder.Entity<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Part>()
              .HasOne(i => i.Vendor)
              .WithMany(i => i.Parts)
              .HasForeignKey(i => i.Vendor_ID)
              .HasPrincipalKey(i => i.Vendor_ID);

            builder.Entity<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Plant>()
              .HasOne(i => i.Manager)
              .WithMany(i => i.Plants)
              .HasForeignKey(i => i.Manager_ID)
              .HasPrincipalKey(i => i.Manager_ID);
            this.OnModelBuilding(builder);
        }

        public DbSet<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Employee> Employees { get; set; }

        public DbSet<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Inventory> Inventories { get; set; }

        public DbSet<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Manager> Managers { get; set; }

        public DbSet<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Part> Parts { get; set; }

        public DbSet<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Plant> Plants { get; set; }

        public DbSet<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Position> Positions { get; set; }

        public DbSet<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.TestTable> TestTables { get; set; }

        public DbSet<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.TestTable2> TestTable2S { get; set; }

        public DbSet<CloudDevOpsProject1.Server.Models.DevOps_Proj_Database.Vendor> Vendors { get; set; }
    }
}