using MatchMaker.Core.Utilities;
using MatchMaker.Domain.Entities;

namespace MatchMaker.Core.Interfaces;

public interface ITeamService
{
    Task<Result<Team>> CreateTeamAsync(Team newTeam);
    Task<Result<List<Team>>> GetAllTeamsAsync();    
    Task<Result<Team>> GetTeamByIdAsync(string teamId);
    Task<Result<Team>> GetTeamByNameAsync(string teamName);
    Task<Result<Team>> UpdateTeamAsync(Team updatedTeam);
    Task<Result<Team>> DeleteTeamAsync(string teamId);
}
