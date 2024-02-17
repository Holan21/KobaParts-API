using KobaParts.Models.Api;
using KobaParts.Models.Api.Client;

namespace KobaParts.Services.AuthService
{
    public interface IAuthService
    {
        public Task<AuthResponse> LoginAsync(UserLoginRequest loginRequest);
    }
}
