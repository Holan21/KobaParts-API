using KobaParts.Models.Api;
using KobaParts.Models.Dto;

namespace KobaParts.Services.HistoryData
{
    public interface IHistoryData
    {
        Task<BaseResponse<OrderDto>> GetHistoryByUser(string phoneNumber);
        Task<BaseResponse<OrderDto>> AddOrderIntoHistory(string? phoneNumber, int? articul, int? count);
    }
}
