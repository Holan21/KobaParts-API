using KobaParts.Data.DatabaseContext;
using KobaParts.Models.Api;
using KobaParts.Models.Api.Client;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace KobaParts.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;
        public AuthService(DataContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public async Task<AuthResponse> LoginAsync(UserLoginRequest loginRequest)
        {
            var request = await _context.Users
                .Include(t => t.Role)
                .FirstOrDefaultAsync(u => u.PhoneNumber == loginRequest.PhoneNumber);

            if (request == null)
            {
                return new AuthResponse { Message = "Authentication failed" };
            }

            if (request.IsLocked != null && request.IsLocked == true)
            {
                return new AuthResponse { Message = "User is locked" };
            }

            if (!VerifyPasswordHash(loginRequest.Password, request.PasswordHash, request.PasswordSalt))
            {
                request.InvalidLoginAttemps += 1;
                if (request.InvalidLoginAttemps > _configuration.GetValue<int>("Autorizathion:MaxFailedAttemp"))
                {
                    request.IsLocked = true;
                }

                await _context.SaveChangesAsync();
                return new AuthResponse()
                {
                    Message = "Authentication failed"
                };
            }

            string token = CreateToken(request);

            return new AuthResponse
            {
                Success = true,
                Token = token,
                Role = request.Role != null ? request.Role.Name : "",
                UserName = request.Login
            };
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512(passwordSalt);

            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(passwordHash);
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512();

            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.HomePhone, user.PhoneNumber),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role , user.Role.Name)
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("Autorizathion:TokenKey").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(claims: claims, expires: DateTime.Now.AddDays(1), signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}
