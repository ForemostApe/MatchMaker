namespace MatchMaker.Domain.DTOs.Games;

public class GameResponseDTO
{
    public string GameId { get; set; } = null!;
    public bool Accepted { get; set; }
}
