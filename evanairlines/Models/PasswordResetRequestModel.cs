using System.ComponentModel.DataAnnotations;

namespace evanairlines.Models
{
    public class PasswordResetRequestModel
    {
        public string email { get; set; }
        public bool emailSent { get; set; }
    }
}
