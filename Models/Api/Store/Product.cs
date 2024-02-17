using KobaParts.Models.Enum.Status;
using KobaParts.Models.Enum.Type;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KobaParts.Models.Api.Store
{
    [Table("Products")]
    public class Product
    {
        [Key]
        public int? Id { get; set; }
        [Required]
        public int Articul { get; set; } = 0;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public string ProducingCountry { get; set; } = string.Empty;
        //public List<String> Photos { get; set; } = new List<String>();
        public ProductType Type { get; set; } = ProductType.Tuning;
        public ProductStatus Status { get; set; } = ProductStatus.InStock;
        public double Cost { get; set; } = double.MaxValue;
    }
}