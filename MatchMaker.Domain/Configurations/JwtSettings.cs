namespace MatchMaker.Domain.Configurations;

public class JwtSettings
{
    public string SigningKey { get; set; } = null!;
    public string Issuer { get; set; } = null!;
    public string Audience { get; set; } = null!;
    public int VerificationTokenExpirationMinutes { get; set; }
    public int AccessTokenExpirationMinutes { get; set; }
    public int RefreshTokenExpirationDays { get; set; }
    public string EncryptionKey { get; set; } = null!;
}