using System.ComponentModel.DataAnnotations.Schema;

namespace KobaParts.Models.Api.Client
{
    [Table("Roles")]
    public class UserType
    {
        public int? Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
