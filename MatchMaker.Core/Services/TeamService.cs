using MatchMaker.Core.Interfaces;
using MatchMaker.Data.Interfaces;
using MatchMaker.Domain.DTOs;
using MatchMaker.Domain.Entities;

namespace MatchMaker.Core.Services;

public class TeamService(ITeamRepo teamRepo) : ITeamService
{
    private readonly ITeamRepo _teamRepo = teamRepo;
    public async Task<Result<Team>> CreateTeamAsync(Team newTeam)
    {
        ArgumentNullException.ThrowIfNull(newTeam);

        var existingTeam = await _teamRepo.GetTeamByNameAsync(newTeam.TeamName);

        if (existingTeam != null) return Result<Team>.Failure("Team already exists.");

        await _teamRepo.CreateTeamAsync(newTeam);
     
        return Result<Team>.Success(newTeam, "Team successfully created.");
    }
}
