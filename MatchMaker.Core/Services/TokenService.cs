using MatchMaker.Core.Interfaces;
using MatchMaker.Domain.Configurations;
using MatchMaker.Domain.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MatchMaker.Core.Services
{ 
    public class TokenService(JwtSettings jwtSettings, ILogger<TokenService> logger, IUserService userService) : ITokenService
    {

        private readonly JwtSettings _jwtSettings = jwtSettings;
        private readonly ILogger<TokenService> _logger = logger;
        private readonly IUserService _userService = userService;

        public async Task<string> GenerateAccessToken(User user)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_jwtSettings.SigningKey);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                    new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.UserRole.ToString())
                }),
                        Expires = DateTime.UtcNow.AddMinutes(15),
                        Issuer = _jwtSettings.Issuer,
                        Audience = _jwtSettings.Audience,
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var accessToken = tokenHandler.CreateToken(tokenDescriptor);
                _logger.LogInformation("Successfully generated access-token for user-ID {userId}", user.Id);
                return await Task.FromResult(tokenHandler.WriteToken(accessToken));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating access-token for user-ID {userId}", user.Id);
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
                _logger.LogInformation("Trying to generate JWT-token for user-ID: {userId}", user.Id);

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_jwtSettings.SigningKey);

                var tokenDescriptor = new SecurityTokenDescriptor()
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                    new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.UserRole.ToString())
                }),
                    Expires = DateTime.UtcNow.AddDays(7),
                    Issuer = _jwtSettings.Issuer,
                    Audience = _jwtSettings.Audience,
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var refreshToken = tokenHandler.CreateToken(tokenDescriptor);
                _logger.LogInformation("Successfully generated refresh-token for user-ID {userId}", user.Id);
                return await Task.FromResult(tokenHandler.WriteToken(refreshToken));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating access-token for user-ID {userId}", user.Id);
                throw;
            }
        }

        public async Task<User> ValidateRefreshToken(string refreshToken)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_jwtSettings.SigningKey);

                var principal = tokenHandler.ValidateToken(refreshToken, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _jwtSettings.Issuer,
                    ValidateAudience = true,
                    ValidAudience = _jwtSettings.Audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(1)
                }, out var validatedToken);

                var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    throw new Exception();
                }

                var user = await _userService.GetUserByIdAsync(userIdClaim.Value);
                if (user == null)
                {
                    throw new Exception();
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