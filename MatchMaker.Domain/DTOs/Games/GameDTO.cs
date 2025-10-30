using MatchMaker.Domain.Entities;

namespace MatchMaker.Domain.DTOs.Games;

public class GameDto
{
    public string Id { get; set; } = null!;
    public DateTime StartTime { get; set; }
    public string? GameType { get; set; }
    public string? Location { get; set; }
    public string? GameStatus { get; set; }
    public string? HomeTeamId { get; set; }
    public string? AwayTeamId { get; set; }
    public string? RefereeId { get; set; }
    public Conditions? Conditions { get; set; }
}
