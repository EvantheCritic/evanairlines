using System.ComponentModel.DataAnnotations;

namespace evanairlines.Models
{
    public class EmployeeModel
    {
        [Key]
        public int id { get; set; }
        public string? name { get; set; }
        public string? job { get; set; }
        public int experience { get; set; }
        public double hours { get; set; }
        public double pay { get; set; }

    }
}
