namespace evanairlines.Models
{
    public class CheckoutModel
    {
        public int id { get; set; }
        public string? username { get; set; }
        public string? departure { get; set; }
        public string? arrival { get; set; }
        public string? aircraft { get; set; }
        public string? number { get; set; }
        public int cost { get; set; }

    }
}
