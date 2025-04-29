using MatchMaker.Domain.Entities;

namespace MatchMaker.Core.Interfaces;

public interface ITeamService
{
    Task<Team> CreateTeamAsync(Team newTeam);
}
