using KobaParts.Models.Api.Client;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KobaParts.Models.Api.Store
{
    [Table("Backets")]
    public class Purchase
    {
        [Key]
        public int? Id { get; set; }
        public Product Product { get; set; } = new();
        public User User { get; set; } = new();
        public int Count { get; set; } = 1;
    }
}
