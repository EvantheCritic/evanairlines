using System.ComponentModel.DataAnnotations;

namespace evanairlines.Models
{
    public class FlightModel
    {
        [Key]
        public int id { get; set; }
        public string? number { get; set; }
        public string? departure { get; set; }
        public string? arrival { get; set; }
        public string? flTime { get; set; }
        public int cost { get; set; }
        public string? aircraft { get; set; }
    }
}
