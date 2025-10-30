using Microsoft.AspNetCore.Http;

namespace MatchMaker.Domain.DTOs.Teams;

public class CreateTeamDto
{
    public string TeamName { get; set; } = null!;
    public IFormFile? TeamLogo { get; set; }
}
