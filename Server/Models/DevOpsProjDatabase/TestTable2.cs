using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CloudDevOpsProject1.Server.Models.DevOps_Proj_Database
{
    [Table("Test Table 2", Schema = "dbo")]
    public partial class TestTable2
    {
        [Key]
        [Required]
        public string NateIsGay { get; set; }

    }
}