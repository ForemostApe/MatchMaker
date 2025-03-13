namespace MatchMaker.Domain.Entities;
public enum GameStatus
{
    Planned,
    Booked,
    Completed,
    Cancelled
}

public class Game : SchemaBase<Game>
{
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string GameType { get; set; } = null!;
    public string Location { get; set; } = null!;
    public GameStatus GameStatus { get; set; } = GameStatus.Planned;
    public int HomeTeamId { get; set; }
    public int AwayTeamId { get; set; }
    public string RefereeId { get; set; } = null!;
}
