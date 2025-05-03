using MatchMaker.Domain.DTOs;
using MatchMaker.Domain.DTOs.Teams;

namespace MatchMaker.Core.Interfaces;

public interface ITeamServiceFacade
{
    Task<Result<TeamDTO>> CreateTeamAsync(CreateTeamDTO newTeam);
    Task<Result<TeamDTO>> GetTeamByIdAsync(string teamId);
    Task<Result<TeamDTO>> GetTeamByNameAsync(string teamName);
    Task<Result<TeamDTO>> UpdateTeamAsync(UpdateTeamDTO updatedTeam);
    Task<Result<TeamDTO>> DeleteTeamAsync(DeleteTeamDTO deleteTeamDTO);
}
