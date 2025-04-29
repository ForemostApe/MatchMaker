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

        try
        {
            var existingTeam = await _teamRepo.GetTeamByNameAsync(newTeam.TeamName);

            if (existingTeam != null) return Result<Team>.Failure("Team already exists.");

            await _teamRepo.CreateTeamAsync(newTeam);

            return Result<Team>.Success(newTeam, "Team successfully created.");
        }
        catch
        {
            throw;
        }
    }

    public async Task<Result<Team>> GetTeamByIdAsync(string teamId)
    {
        ArgumentException.ThrowIfNullOrEmpty(teamId);

        try
        {
            var existingTeam = await _teamRepo.GetTeamByIdAsync(teamId);

            if (existingTeam == null) return Result<Team>.Failure("Coulnd't find the specified team.");

            return Result<Team>.Success(existingTeam, "Team successfully found.");
        }
        catch
        {
            throw;
        }
    }
}
