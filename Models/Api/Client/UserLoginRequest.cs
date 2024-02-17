using System.ComponentModel.DataAnnotations;

namespace KobaParts.Models.Api.Client
{
    public class UserLoginRequest
    {
        [Required]

        public string PhoneNumber { get; set; } = string.Empty;
        [Required]

        public string Password { get; set; } = string.Empty;
    }
}
