using MatchMaker.Domain.DTOs;
using MatchMaker.Domain.Entities;

namespace MatchMaker.Core.Interfaces;

public interface ITeamService
{
    Task<Result<Team>> CreateTeamAsync(Team newTeam);
}
