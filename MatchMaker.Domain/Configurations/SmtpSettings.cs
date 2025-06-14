﻿namespace MatchMaker.Domain.Configurations;

public class SmtpSettings
{
    public string Host { get; set; } = null!;
    public int Port { get; set; }
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string FromEmail { get; set; } = null!;
    public string SenderName { get; set; } = null!;
    public bool UseTsl { get; set; }
}
