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

            var result = await _teamRepo.CreateTeamAsync(newTeam);

            return Result<Team>.Success(result, "Team successfully created.");
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

    public async Task<Result<Team>> GetTeamByNameAsync(string teamName)
    {
        ArgumentException.ThrowIfNullOrEmpty(teamName);

        try
        {
            var existingTeam = await _teamRepo.GetTeamByNameAsync(teamName);

            if (existingTeam == null) return Result<Team>.Failure("Coulnd't find the specified team.");

            return Result<Team>.Success(existingTeam, "Team successfully found.");
        }
        catch
        {
            throw;
        }
    }

    public async Task<Result<Team>> UpdateTeamAsync(Team updatedTeam)
    {
        ArgumentNullException.ThrowIfNull(updatedTeam);

        try
        {
            var result = await _teamRepo.UpdateTeamAsync(updatedTeam);

            return Result<Team>.Success(updatedTeam, "Team successfully updated.");

        }
        catch
        {
            throw;
        }
    }

    public async Task<Result<Team>> DeleteTeamAsync(string teamId)
    {
        ArgumentException.ThrowIfNullOrEmpty(teamId);

        try
        {
            var result = await _teamRepo.DeleteTeamAsync(teamId);

            if (result.DeletedCount <= 0) return Result<Team>.Failure("Team not found.");

            return Result<Team>.Success(null, "Team successfully deleted.");
        }
        catch
        {
            throw;
        }
    }
}
