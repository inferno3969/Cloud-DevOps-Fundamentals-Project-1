using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CloudDevOpsProject1.Server.Models.DevOps_Proj_Database
{
    [Table("Position", Schema = "dbo")]
    public partial class Position
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
        public int Pos_ID { get; set; }

        [ConcurrencyCheck]
        public string Pos_Name { get; set; }

        [ConcurrencyCheck]
        public string Pos_Description { get; set; }

        public ICollection<Employee> Employees { get; set; }

    }
}