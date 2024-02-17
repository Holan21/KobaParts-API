using KobaParts.Models.Api.Client;
using KobaParts.Models.Enum.Status;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KobaParts.Models.Api.Store
{
    [Table("Orders")]
    public class Order
    {
        [Key]
        public int? Id { get; set; }
        public Product Product { get; set; } = new();
        public DateTime DateTime { get; set; } = DateTime.Now;
        public OrderStatus Status { get; set; } = OrderStatus.InProgress;
        public User User { get; set; } = new();
        public int Count { get; set; } = 0;
    }
}
