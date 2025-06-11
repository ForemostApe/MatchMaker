namespace MatchMaker.Domain.Entities
{
    public record TokenClaims
    {
        public string UserId { get; set; } = default!;
        public string UserEmail { get; set; } = default!;
        public string TokenUsage { get; init; } = default!;
    }
}
