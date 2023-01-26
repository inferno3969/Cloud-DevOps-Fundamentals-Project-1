using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CloudDevOpsProject1.Server.Models.DevOps_Proj_Database
{
    [Table("Test Table", Schema = "dbo")]
    public partial class TestTable
    {
        [Key]
        [Required]
        public string Test { get; set; }

        public int? CanNateSeeThis { get; set; }

    }
}