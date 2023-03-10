using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CloudDevOpsProject1.Server.Models.DevOps_Proj_Database
{
    [Table("Manager", Schema = "dbo")]
    public partial class Manager
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
        public int Manager_ID { get; set; }

        [ConcurrencyCheck]
        public int? Emp_ID { get; set; }

        [ConcurrencyCheck]
        public int? Plant_ID { get; set; }

        public Employee Employee { get; set; }

        public Plant Plant { get; set; }

        public ICollection<Plant> Plants { get; set; }

    }
}