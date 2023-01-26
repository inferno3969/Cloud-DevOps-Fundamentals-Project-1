using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CloudDevOpsProject1.Server.Models.DevOps_Proj_Database
{
    [Table("Plant", Schema = "dbo")]
    public partial class Plant
    {
        [Key]
        [Required]
        public int Plant_ID { get; set; }

        public string Plant_Name { get; set; }

        public string Plant_Address { get; set; }

        public string Plant_Phone { get; set; }

        public int? Manager_ID { get; set; }

        public ICollection<Employee> Employees { get; set; }

        public ICollection<Inventory> Inventories { get; set; }

        public ICollection<Manager> Managers { get; set; }

        public Manager Manager { get; set; }

    }
}