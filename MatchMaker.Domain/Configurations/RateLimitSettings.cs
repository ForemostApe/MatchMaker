namespace MatchMaker.Domain.Configurations
{
    public class RateLimitSettings
    {
        public int PermitLimit { get; set; } = 5;
        public int WindowInSeconds { get; set; } = 60;
        public int QueueLimit { get; set; } = 1;
    }
}
