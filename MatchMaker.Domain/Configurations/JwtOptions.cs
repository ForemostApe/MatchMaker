using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace MatchMaker.Domain.Configurations;

public class JwtOptions(JwtSettings settings)
{
    public string Issuer { get; } = settings.Issuer;
    public string Audience { get; } = settings.Audience;
    public int VerificationTokenExpirationMinutes { get; } = settings.VerificationTokenExpirationMinutes;
    public int AccessTokenExpirationMinutes { get; } = settings.AccessTokenExpirationMinutes;
    public int RefreshTokenExpirationDays { get; } = settings.RefreshTokenExpirationDays;
    public SymmetricSecurityKey SigningKey { get; } = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.SigningKey));
    public SymmetricSecurityKey EncryptionKey { get; } = new SymmetricSecurityKey(Convert.FromBase64String(settings.EncryptionKey));
    

    public TokenValidationParameters GetTokenValidationParameters(bool validateLifetime = true)
    {
        return new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = validateLifetime,
            ClockSkew = TimeSpan.FromMinutes(1),
            ValidIssuer = Issuer,
            ValidAudience = Audience,
            IssuerSigningKey = SigningKey,
            TokenDecryptionKey = EncryptionKey,
            RoleClaimType = ClaimTypes.Role
        };
    }
}