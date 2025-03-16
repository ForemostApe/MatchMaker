using MatchMaker.Core.Interfaces;
using MatchMaker.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MatchMaker.Core.Services
{
    public class TokenService(IConfiguration configuration, ILogger<TokenService> logger, IUserService userService) : ITokenService
    {
        private readonly string _tokenKey = configuration["JwtSettings:TokenKey"] ?? throw new ArgumentNullException(nameof(configuration), "TokenKey is missing from configuration.");
        private readonly string _issuer = configuration["JwtSettings:Issuer"] ?? throw new ArgumentNullException(nameof(configuration), "Issuer is missing from configuration.");
        private readonly string _audience = configuration["JwtSettings:Audience"] ?? throw new ArgumentNullException(nameof(configuration), "Audience is missing from configuration.");
        private readonly ILogger<TokenService> _logger = logger;
        private readonly IUserService _userService = userService;

        public async Task<string> GenerateAccessToken(User user)
        {
            if (user == null)
            {
                _logger.LogError("UserEntity cannot be null when trying to create access-token.");
                throw new ArgumentNullException(nameof(user));
            }

            try
            {
                _logger.LogInformation("Trying to generate access-token for user-ID: {userId}", user.ID);

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_tokenKey);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.ID.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.UserRole.ToString())
            }),
                    Expires = DateTime.UtcNow.AddMinutes(15),
                    Issuer = _issuer,
                    Audience = _audience,
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var accessToken = tokenHandler.CreateToken(tokenDescriptor);
                _logger.LogInformation("Successfully generated access-token for user-ID {userId}", user.ID);
                return await Task.FromResult(tokenHandler.WriteToken(accessToken));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating access-token for user-ID {userId}", user.ID);
                throw;
            }
        }

        public async Task<string> GenerateRefreshToken(User user)
        {
            if (user == null)
            {
                _logger.LogError("UserEntity cannot be null when trying to create JWT-token.");
                throw new ArgumentNullException(nameof(user));
            }
            try
            {
                _logger.LogInformation("Trying to generate JWT-token for user-ID: {userId}", user.ID);

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_tokenKey);

                var tokenDescriptor = new SecurityTokenDescriptor()
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                    new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.NameIdentifier, user.ID),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.UserRole.ToString())
                }),
                    Expires = DateTime.UtcNow.AddDays(7),
                    Issuer = _issuer,
                    Audience = _audience,
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var refreshToken = tokenHandler.CreateToken(tokenDescriptor);
                _logger.LogInformation("Successfully generated refresh-token for user-ID {userId}", user.ID);
                return await Task.FromResult(tokenHandler.WriteToken(refreshToken));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating access-token for user-ID {userId}", user.ID);
                throw;
            }
        }

        public async Task<User> ValidateRefreshToken(string refreshToken)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_tokenKey);

                var principal = tokenHandler.ValidateToken(refreshToken, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _issuer,
                    ValidateAudience = true,
                    ValidAudience = _audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                }, out var validatedToken);

                var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    return null;
                }

                var user = await _userService.GetUserByIdAsync(userIdClaim.Value);
                if (user == null)
                {
                    throw new Exception("Shit be crazy!");
                }
                return user.Data!;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occured when trying to validate refresh-token.");
                throw new Exception("An unexpected error occured when trying to validate refresh-token.", ex);
            }
        }
    }
}