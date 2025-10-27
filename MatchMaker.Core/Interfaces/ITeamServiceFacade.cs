using MatchMaker.Core.Utilities;
using MatchMaker.Domain.DTOs.Teams;
using Microsoft.AspNetCore.Http;

namespace MatchMaker.Core.Interfaces;

public interface ITeamServiceFacade
{
    Task<Result<TeamDTO>> CreateTeamAsync(CreateTeamDTO newTeam, IFormFile teamLogo);
    Task<Result<List<TeamDTO>>> GetAllTeamsAsync();
    Task<Result<TeamDTO>> GetTeamByIdAsync(string teamId);
    Task<Result<TeamDTO>> GetTeamByNameAsync(string teamName);
    Task<Result<TeamDTO>> UpdateTeamAsync(UpdateTeamDTO updatedTeam);
    Task<Result<TeamDTO>> DeleteTeamAsync(DeleteTeamDTO deleteTeamDTO);
}
