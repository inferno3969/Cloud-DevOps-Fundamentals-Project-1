using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CloudDevOpsProject1.Server.Models.DevOps_Proj_Database
{
    [Table("Parts", Schema = "dbo")]
    public partial class Part
    {
        [Key]
        [Required]
        public int Part_ID { get; set; }

        public string Name { get; set; }

        public string Specs { get; set; }

        public int? Vendor_ID { get; set; }

        public int? Inv_ID { get; set; }

        public ICollection<Inventory> Inventories { get; set; }

        public Inventory Inventory { get; set; }

        public Vendor Vendor { get; set; }

    }
}