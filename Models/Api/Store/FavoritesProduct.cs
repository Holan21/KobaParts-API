using KobaParts.Models.Api.Client;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KobaParts.Models.Api.Store
{
    [Table("Favorites")]
    public class FavoritesProduct
    {
        [Key]
        public int? Id { get; set; }
        public Product Product { get; set; } = new();
        public User User { get; set; } = new();
    }
}
