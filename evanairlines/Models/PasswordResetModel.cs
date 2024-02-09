namespace evanairlines.Models
{
    public class PasswordResetModel
    {
        public string email { get; set; }
        public string token { get; set; }
        public string new_password { get; set; }
    }
}
