using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PersonalFinanceManagement.BLL.Exceptions;
using PersonalFinanceManagement.Domain.DALEntities;
using PersonalFinanceManagement.Domain.Interfaces.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PersonalFinanceManagement.BLL.Services
{
    public class AuthService : IAuthService
    {
        private const int _STANDARD_TOKEN_LIFETIME_IN_MINUTES = 15;
        private readonly IUsersService _userService;
        private readonly IConfiguration _configuration;
        private readonly int _expirationInMinutes;

        public AuthService(IUsersService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;

            if (int.TryParse(_configuration["JwtSettings:ExpirationInMinutes"], out int parsedValue) && parsedValue > 0)
                _expirationInMinutes = parsedValue;
            else
                _expirationInMinutes = _STANDARD_TOKEN_LIFETIME_IN_MINUTES;
        }

        public async Task<string> UserLogin(string email, string password, CancellationToken cancel)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(email))
                throw new ArgumentException(string.IsNullOrWhiteSpace(email)
                    ? $"{nameof(email)} cannot be null, empty, or contain only whitespace."
                    : $"{nameof(password)} cannot be null, empty, or contain only whitespace.");

            if (!await _userService.ExistByEmailAsync(email, cancel).ConfigureAwait(false))
                throw new InvalidCredentialsException($"The user with the email '{email}' is not registered.");

            var user = await _userService.GetUserByEmailAsync(email, cancel).ConfigureAwait(false);

            if (!_userService.VerifyPassword(password, user.PasswordHash))
                throw new InvalidCredentialsException("Invalid password.");

            return GenerateJWT(user);
        }

        private string GenerateJWT(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]);
            var tokenDescriptor = GetSecurityTokenDescriptor(user, key);
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        private SecurityTokenDescriptor GetSecurityTokenDescriptor(User user, byte[] key)
        {
            return new SecurityTokenDescriptor
            {
                Issuer = _configuration["JwtSettings:Issuer"],
                Audience = _configuration["JwtSettings:Audience"],
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("id", user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                }),
                Expires = DateTime.UtcNow.AddMinutes(_expirationInMinutes),
                NotBefore = DateTime.UtcNow,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
        }
    }
}
