using MatchMaker.Domain.Entities;

namespace MatchMaker.Data.Interfaces;

public interface ITeamRepo
{
    Task CreateTeamAsync(Team newTeam);
    Task<Team?> GetTeamByIdAsync(string teamId);
    Task<Team?> GetTeamByNameAsync(string teamName);
    Task UpdateTeamAsync(Team updatedTeam);
    Task DeleteTeamAsync(string teamId);
}
