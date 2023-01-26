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
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Emp_ID { get; set; }

        public string Emp_Name { get; set; }

        public string Emp_Address { get; set; }

        public string Emp_Phone { get; set; }

        public int? Pos_ID { get; set; }

        public int? Plant_ID { get; set; }

        public Plant Plant { get; set; }

        public Position Position { get; set; }

        public ICollection<Manager> Managers { get; set; }

    }
}