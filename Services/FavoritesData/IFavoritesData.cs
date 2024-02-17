using KobaParts.Models.Api;
using KobaParts.Models.Dto;

namespace KobaParts.Services.FavoritesData
{
    public interface IFavoritesData
    {
        Task<BaseResponse<FavoritesProductDto>> GetFavorites(string? phoneNumber, int? articul);
        Task<BaseResponse<FavoritesProductDto>> DeleteFavorites(string? phoneNumber, int? articul);
        Task<BaseResponse<FavoritesProductDto>> AddFavorites(string? phoneNumber, int? articul);
    }
}
