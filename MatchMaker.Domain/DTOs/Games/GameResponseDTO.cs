namespace MatchMaker.Domain.DTOs.Games;

public class GameResponseDto
{
    public string GameId { get; set; } = null!;
    public bool Accepted { get; set; }
}
