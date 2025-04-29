using MatchMaker.Domain.Entities;
using MongoDB.Driver;

namespace MatchMaker.Data.Interfaces;

public interface ITeamRepo
{
    Task<Team> CreateTeamAsync(Team newTeam);
    Task<Team?> GetTeamByIdAsync(string teamId);
    Task<Team?> GetTeamByNameAsync(string teamName);
    Task UpdateTeamAsync(Team updatedTeam);
    Task<DeleteResult> DeleteTeamAsync(string teamId);
}
