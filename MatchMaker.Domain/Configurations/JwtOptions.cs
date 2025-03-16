using MatchMaker.Domain.Configurations;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace MatchMaker.Core.Services;

public class JwtOptions
{
    public string Issuer { get; }
    public string Audience { get; }
    public int AccessTokenExpirationMinutes { get; }
    public SymmetricSecurityKey SigningKey { get; }
    public SymmetricSecurityKey EncryptionKey { get; }

    public JwtOptions(JwtSettings settings)
    {
        Issuer = settings.Issuer;
        Audience = settings.Audience;
        AccessTokenExpirationMinutes = settings.AccessTokenExpirationMinutes;

        SigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.SigningKey));
        EncryptionKey = new SymmetricSecurityKey(Convert.FromBase64String(settings.EncryptionKey));
    }
}