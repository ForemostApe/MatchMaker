using MatchMaker.Domain.Entities;

namespace MatchMaker.Data.Interfaces;

public interface ITeamRepo
{
    Task CreateTeamAsync(Team newTeam);
}
