namespace MatchMaker.Domain.Configurations;

public class SmtpSettings
{
    public string SmtpHost { get; set; } = null!;
    public int SmtpPort { get; set; }
    public string SmtpUsername { get; set; } = null!;
    public string SmtpPassword { get; set; } = null!;
    public string SmtpFromEmail { get; set; } = null!;
}
