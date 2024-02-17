using KobaParts.Models.Api.Store;
using KobaParts.Models.Enum.Status;

namespace KobaParts.Models.Dto
{
    public class OrderDto
    {
        public int? Id { get; set; }
        public Product Product { get; set; } = new();
        public DateTime DateTime { get; set; } = DateTime.Now;
        public OrderStatus Status { get; set; } = OrderStatus.InProgress;
        public UserDto User { get; set; } = new();
        public int Count { get; set; } = 0;
    }
}
