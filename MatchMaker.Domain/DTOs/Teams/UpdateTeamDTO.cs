namespace MatchMaker.Domain.DTOs.Teams;

public class UpdateTeamDTO
{
    public string TeamId { get; set; } = null!;
    public string? TeamName { get; set; }
    public string? TeamLogo { get; set; }
}
