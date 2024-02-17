using KobaParts.Models.Api.Client;
using System.ComponentModel.DataAnnotations;

namespace KobaParts.Models.Dto
{
    public class UserDto
    {
        public int? Id { get; set; }
        public string Login { get; set; } = string.Empty;
        [StringLength(30)]
        public string Name { get; set; } = string.Empty;
        [StringLength(30)]
        public string SecondName { get; set; } = string.Empty;
        [StringLength(30)]
        public string SurName { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        [Required]
        public string PhoneNumber { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public UserType Role { get; set; } = new UserType();

    }
}
