using KobaParts.Models.Api;
using KobaParts.Models.Dto;
using KobaParts.Services.HistoryData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KobaParts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class History : Controller
    {
        private readonly IHistoryData _history;

        public History(IHistoryData history) => _history = history;

        [HttpGet]
        [Authorize(Roles = "User,Administrator")]
        public async Task<BaseResponse<OrderDto>> Get(string phoneNumber) => await _history.GetHistoryByUser(phoneNumber);

        [HttpPost]
        [Authorize(Roles = "User,Administrator")]
        public async Task<BaseResponse<OrderDto>> Post(string phoneNumber, int articul, int count) => await _history.AddOrderIntoHistory(phoneNumber, articul, count);
    }
}
