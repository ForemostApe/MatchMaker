using MatchMaker.Core.Utilities;
using MatchMaker.Domain.DTOs.Teams;
using Microsoft.AspNetCore.Http;

namespace MatchMaker.Core.Interfaces;

public interface ITeamServiceFacade
{
    Task<Result<TeamDto>> CreateTeamAsync(CreateTeamDto newTeam);
    Task<Result<List<TeamDto>>> GetAllTeamsAsync();
    Task<Result<TeamDto>> GetTeamByIdAsync(string teamId);
    Task<Result<TeamDto>> GetTeamByNameAsync(string teamName);
    Task<Result<TeamDto>> UpdateTeamAsync(UpdateTeamDto updatedTeam);
    Task<Result<TeamDto>> DeleteTeamAsync(DeleteTeamDto deleteTeamDto);
}
