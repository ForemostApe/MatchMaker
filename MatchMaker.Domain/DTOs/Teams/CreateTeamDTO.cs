using Microsoft.AspNetCore.Http;

namespace MatchMaker.Domain.DTOs.Teams;

public class CreateTeamDTO
{
    public string TeamName { get; set; } = null!;
    public IFormFile? TeamLogo { get; set; }
}
