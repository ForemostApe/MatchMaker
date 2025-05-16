using MatchMaker.Domain.Entities;

namespace MatchMaker.Domain.DTOs.Games;

public class CreateGameDTO
{
    public DateTime StartTime { get; set; }
    public string GameType { get; set; } = null!;
    public string Location { get; set; } = null!;
    public string GameStatus { get; set; } = "Planned";
    public string HomeTeamId { get; set; } = null!;
    public string AwayTeamId { get; set; } = null!;
    public string? RefereeId { get; set; } = null!;
    public Conditions Conditions { get; set; } = null!;
}
