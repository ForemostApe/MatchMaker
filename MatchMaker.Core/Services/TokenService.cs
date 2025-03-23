using MatchMaker.Core.Interfaces;
using MatchMaker.Domain.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MatchMaker.Core.Services
{ 
    public class TokenService(JwtOptions jwtOptions, ILogger<TokenService> logger, IUserService userService) : ITokenService
    {

        private readonly JwtOptions _jwtOptions = jwtOptions;
        private readonly ILogger<TokenService> _logger = logger;
        private readonly IUserService _userService = userService;

        private TokenValidationParameters TokenValidationParameters => new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = _jwtOptions.SigningKey,
            ValidateIssuer = true,
            ValidIssuer = _jwtOptions.Issuer,
            ValidateAudience = true,
            ValidAudience = _jwtOptions.Audience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromMinutes(1)
        };

        private string GenerateToken(User user, TimeSpan expiration, string tokenType)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = _jwtOptions.SigningKey;

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
                    Issuer = _jwtOptions.Issuer,
                    Audience = _jwtOptions.Audience,
                    SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature),
                    EncryptingCredentials = new EncryptingCredentials(_jwtOptions.EncryptionKey, SecurityAlgorithms.Aes256KW, SecurityAlgorithms.Aes256CbcHmacSha512)
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
                var key = _jwtOptions.SigningKey;

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

        public ClaimsPrincipal DecryptToken(string encryptedToken)
        {
            try
            {
                _logger.LogInformation("Attempting to decrypt token.");
                byte[] serializedToken = Base64UrlEncoder.DecodeBytes(encryptedToken);
                string decryptedToken = Encoding.UTF8.GetString(serializedToken);

                var tokenHandler = new JwtSecurityTokenHandler();

                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = false,
                    IssuerSigningKey = _jwtOptions.SigningKey,
                    TokenDecryptionKey = _jwtOptions.EncryptionKey,
                    RequireSignedTokens = true
                };

                var jwtToken = tokenHandler.ReadJwtToken(decryptedToken);

                var principal = tokenHandler.ValidateToken(decryptedToken, validationParameters, out var validatedToken);

                return principal;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occured while trying to decrypt token.");
                throw new SecurityTokenException("Invalid token or decryption error.", ex);
            }
        }
    }
}