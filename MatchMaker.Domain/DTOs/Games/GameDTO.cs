using MatchMaker.Domain.Entities;

namespace MatchMaker.Domain.DTOs.Games;

public class GameDTO
{
    public string Id { get; set; } = null!;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string GameType { get; set; }
    public string Location { get; set; }
    public GameStatus GameStatus { get; set; }
    public int HomeTeamId { get; set; }
    public int AwayTeamId { get; set; }
    public string RefereeId { get; set; }
    public Conditions Conditions { get; set; }
}
