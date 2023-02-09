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
        public int Part_ID { get; set; }

        [ConcurrencyCheck]
        public string Name { get; set; }

        [ConcurrencyCheck]
        public string Specs { get; set; }

        [ConcurrencyCheck]
        public int? Vendor_ID { get; set; }

        public ICollection<Inventory> Inventories { get; set; }

        public Vendor Vendor { get; set; }

    }
}