using MatchMaker.Domain.Entities;

namespace MatchMaker.Domain.DTOs.Games;

public class CreateGameDTO
{
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string GameType { get; set; } = null!;
    public string Location { get; set; } = null!;
    public GameStatus GameStatus { get; set; }
    public int HomeTeamId { get; set; }
    public int AwayTeamId { get; set; }
    public string? RefereeId { get; set; } = null!;
    public Conditions Conditions { get; set; } = null!;
}
