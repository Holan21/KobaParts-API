using KobaParts.Models.Api;
using KobaParts.Models.Dto;

namespace KobaParts.Services.BacketData
{
    public interface IBasketData
    {
        Task<BaseResponse<PurchaseDto>> GetBasket(string? backetFilter);
        Task<BaseResponse<PurchaseDto>> AddOrderInBacket(int? articul, string? phoneNumber, int? count);
        Task<BaseResponse<PurchaseDto>> DeleteOrderFromBascket(int id);
    }
}
