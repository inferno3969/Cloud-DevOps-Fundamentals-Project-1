using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CloudDevOpsProject1.Server.Models.DevOps_Proj_Database
{
    [Table("Employee", Schema = "dbo")]
    public partial class Employee
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
        public int Emp_ID { get; set; }

        [ConcurrencyCheck]
        public string Emp_Name { get; set; }

        [ConcurrencyCheck]
        public string Emp_Address { get; set; }

        [ConcurrencyCheck]
        public string Emp_Phone { get; set; }

        [ConcurrencyCheck]
        public int? Pos_ID { get; set; }

        [ConcurrencyCheck]
        public int? Plant_ID { get; set; }

        public Plant Plant { get; set; }

        public Position Position { get; set; }

        public ICollection<Manager> Managers { get; set; }

    }
}