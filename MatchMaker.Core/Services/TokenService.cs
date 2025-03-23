using MatchMaker.Core.Interfaces;
using MatchMaker.Domain.Configurations;
using MatchMaker.Domain.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Buffers.Text;
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

        private TokenValidationParameters TokenValidationParameters => new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SigningKey)),
            ValidateIssuer = true,
            ValidIssuer = _jwtSettings.Issuer,
            ValidateAudience = true,
            ValidAudience = _jwtSettings.Audience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromMinutes(1)
        };

        private string GenerateToken(User user, TimeSpan expiration, string tokenType)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_jwtSettings.SigningKey);

                var claims = new List<Claim>()
                {
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.NameIdentifier, user.Id)
                };

                if (tokenType == "access")
                {
                    claims.Add(new Claim(ClaimTypes.Email, user.Email));
                    claims.Add(new Claim(ClaimTypes.Role, user.UserRole.ToString()));
                }

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.UtcNow.Add(expiration),
                    Issuer = _jwtSettings.Issuer,
                    Audience = _jwtSettings.Audience,
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var accessToken = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(accessToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating access-token for user-ID {userId}", user.Id);
                throw new Exception($"Error generating access-token for user-ID {user.Id}", ex);
            }
        }

        public async Task<string> GenerateAccessToken(User user)
        {
            return await Task.FromResult(GenerateToken(user, TimeSpan.FromMinutes(15), "access"));
        }

        public async Task<string> GenerateRefreshToken(User user)
        {
            return await Task.FromResult(GenerateToken(user, TimeSpan.FromDays(7), "refresh"));
        }

        public async Task<User> ValidateRefreshToken(string refreshToken)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_jwtSettings.SigningKey);

                var principal = tokenHandler.ValidateToken(refreshToken, TokenValidationParameters, out var validatedToken);

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

        public string GenerateUrlSafeToken(string data)
        {
            try
            {
                return Base64UrlEncoder.Encode(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error encoding data into a URL-safe token.");
                throw new ApplicationException("An error occurred while generating the token.", ex);
            }
        }
        public string DecodeUrlSafeToken(string urlSafeToken)
        {
            try
            {
                return Base64UrlEncoder.Decode(urlSafeToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error decoding URL-safe token: {Token}", urlSafeToken);
                throw new ApplicationException("An error occurred while decoding the token.", ex);
            }
        }
    }
}