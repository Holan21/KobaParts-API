using KobaParts.Models.Api;
using KobaParts.Models.Api.Client;
using KobaParts.Services.AuthService;
using Microsoft.AspNetCore.Mvc;

namespace KobaParts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService auth)
        {
            _authService = auth;
        }

        [HttpPost]
        public async Task<BaseResponse<AuthResponse>> Login(UserLoginRequest user)
        {

            var response = await _authService.LoginAsync(user);

            return new()
            {
                Description = response.Message,
                StatusCode = response.Success ? new OkResult().StatusCode : new BadRequestResult().StatusCode,
                TotalRecords = 1,
                Values = new() { response }
            };
        }

    }
}
