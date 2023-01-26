using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CloudDevOpsProject1.Server.Models.DevOps_Proj_Database
{
    [Table("Inventory", Schema = "dbo")]
    public partial class Inventory
    {
        [Key]
        [Required]
        public int Inv_ID { get; set; }

        [Required]
        public int Plant_ID { get; set; }

        public int? Part_ID { get; set; }

        public int? Quanity { get; set; }

        public Part Part { get; set; }

        public Plant Plant { get; set; }

        public ICollection<Part> Parts { get; set; }

    }
}