using MatchMaker.Domain.DTOs;
using MatchMaker.Domain.Entities;

namespace MatchMaker.Core.Interfaces;

public interface ITeamService
{
    Task<Result<Team>> CreateTeamAsync(Team newTeam);
    Task<Result<Team>> GetTeamByIdAsync(string teamId);
    Task<Result<Team>> GetTeamByNameAsync(string teamName);
    Task<Result<Team>> DeleteTeamAsync(string teamId);
}
