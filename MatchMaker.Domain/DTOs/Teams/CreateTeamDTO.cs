namespace MatchMaker.Domain.DTOs.Teams;

public class CreateTeamDTO
{
    public string TeamName { get; set; } = null!;
    public string? TeamLogo { get; set; }
}
