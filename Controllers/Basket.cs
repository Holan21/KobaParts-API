using KobaParts.Models.Api;
using KobaParts.Models.Dto;
using KobaParts.Services.BacketData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KobaParts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Basket : Controller
    {
        private readonly IBasketData _backetData;
        public Basket(IBasketData backetData)
        {
            _backetData = backetData;
        }

        [HttpGet]
        [Authorize(Roles = "User,Administrator")]
        public async Task<BaseResponse<PurchaseDto>> Get([FromQuery] string phoneNumber) => await _backetData.GetBasket(phoneNumber);

        [HttpPost]
        [Authorize(Roles = "User,Administrator")]
        public async Task<BaseResponse<PurchaseDto>> Post([FromQuery] int? articul, string? phoneNumber, int? count) => await _backetData.AddOrderInBacket(articul, phoneNumber, count);

        [HttpDelete]
        [Authorize(Roles = "User,Administrator")]
        public async Task<BaseResponse<PurchaseDto>> Delete([FromQuery] int id) => await _backetData.DeleteOrderFromBascket(id);
    }
}
