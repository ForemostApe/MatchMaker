namespace MatchMaker.Domain.Entities;
public enum GameStatus
{
    Cancelled,
    Draft,
    Planned,
    Booked,
    Completed,  
}

public class Game : SchemaBase<Game>
{
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string GameType { get; set; } = null!;
    public string Location { get; set; } = null!;
    public GameStatus GameStatus { get; set; } = GameStatus.Draft;
    public int HomeTeamId { get; set; }
    public int AwayTeamId { get; set; }
    public string RefereeId { get; set; } = null!;
    public Conditions Conditions { get; set; }
}

public class Conditions
{
    public string Court { get; set; }
    public string OffensiveConditions { get; set; }
    public string DefensiveConditions { get; set; }
    public string Specialists { get; set; }
    public string Penalties { get; set; }
}