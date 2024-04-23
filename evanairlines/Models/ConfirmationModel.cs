namespace evanairlines.Models
{
    public class ConfirmationModel
    {
        public int id { get; set; }
        public string? confirmationCode { get; set; }
        public string? username { get; set; }
        public string? firstName { get; set; }
        public string? lastName { get; set; }
        public string? route { get; set; }
        public string? aircraft { get; set; }
        public string? number { get; set; }
    }
}
