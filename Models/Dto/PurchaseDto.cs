using KobaParts.Models.Api.Store;
namespace KobaParts.Models.Dto
{
    public class PurchaseDto
    {

        public int? Id { get; set; }
        public Product Product { get; set; } = new();
        public UserDto User { get; set; } = new();
        public int Count { get; set; } = 0;
    }
}
