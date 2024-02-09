using System.ComponentModel.DataAnnotations;

namespace evanairlines.Models
{
    public class LogbookModel
    {
        [Key]
        public int id { get; set; }
        public string? pilot { get; set; }
        public string? copilot { get; set; }
        public string? engineer { get; set; }
        public string? fl_attendant { get; set; }
        public string? departure { get; set; }
        public string? arrival { get; set; }
        public double hours { get; set; }
        public double minutes { get; set; }
        public string? date { get; set; }
        public int pay { get; set; }
    }
}
