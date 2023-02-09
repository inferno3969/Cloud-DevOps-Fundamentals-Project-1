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

        [NotMapped]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("@odata.etag")]
        public string ETag
        {
                get;
                set;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Inv_ID { get; set; }

        [Required]
        [ConcurrencyCheck]
        public int Plant_ID { get; set; }

        [ConcurrencyCheck]
        public int? Part_ID { get; set; }

        [ConcurrencyCheck]
        public int? Quanity { get; set; }

        public Part Part { get; set; }

        public Plant Plant { get; set; }

    }
}