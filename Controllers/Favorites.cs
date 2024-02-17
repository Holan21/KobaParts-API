using KobaParts.Models.Api;
using KobaParts.Models.Dto;
using KobaParts.Services.FavoritesData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KobaParts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Favorites : Controller
    {
        private readonly IFavoritesData _favoritesData;
        public Favorites(IFavoritesData favoritesData)
            => _favoritesData = favoritesData;

        [HttpGet]
        [Authorize(Roles = "User,Administrator")]
        public async Task<BaseResponse<FavoritesProductDto>> Get([FromQuery] string? phone, [FromQuery] int? articul) => await _favoritesData.GetFavorites(phone, articul);

        [HttpDelete]
        [Authorize(Roles = "User,Administrator")]
        public async Task<BaseResponse<FavoritesProductDto>> Delete([FromQuery] string? phone, [FromQuery] int? articul) => await _favoritesData.DeleteFavorites(phone, articul);

        [HttpPost]
        [Authorize(Roles = "User,Administrator")]
        public async Task<BaseResponse<FavoritesProductDto>> Post([FromQuery] string? phone, [FromQuery] int? articul) => await _favoritesData.AddFavorites(phone, articul);
    }
}
