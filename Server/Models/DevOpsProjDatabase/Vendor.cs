using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CloudDevOpsProject1.Server.Models.DevOps_Proj_Database
{
    [Table("Vendor", Schema = "dbo")]
    public partial class Vendor
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
        public int Vendor_ID { get; set; }

        [ConcurrencyCheck]
        public string Ven_Name { get; set; }

        [ConcurrencyCheck]
        public string Ven_Phone { get; set; }

        [ConcurrencyCheck]
        public string Ven_Address { get; set; }

        public ICollection<Part> Parts { get; set; }

    }
}