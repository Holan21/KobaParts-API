using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KobaParts.Models.Api.Client
{
    [Table("Users")]
    public class User
    {
        [Key]
        public int? Id { get; set; }
        public string Login { get; set; } = string.Empty;

        [StringLength(30)]
        public string Name { get; set; } = string.Empty;
        [StringLength(30)]
        public string SecondName { get; set; } = string.Empty;
        [StringLength(30)]
        public string SurName { get; set; } = string.Empty;
        public UserType Role { get; set; } = new UserType();

        public string CreatedBy { get; set; } = string.Empty;

        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string PhoneNumber { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public int InvalidLoginAttemps { get; set; } = 0;
        public bool IsLocked { get; set; } = false;
        public byte[] PasswordHash { get; set; } = new byte[32];
        public string? PasswordResetToken { get; set; }
        public string? VerificationToken { get; set; }
        public byte[] PasswordSalt { get; set; } = new byte[32];
        public DateTime? VerifiedAt { get; set; }


    }
}
