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

        var result = await _teamRepo.CreateTeamAsync(newTeam);

        return Result<Team>.Success(result, "Team successfully created.");
    }

    public async Task<Result<List<Team>>> GetAllTeamsAsync()
    {
        var result = await _teamRepo.GetAllTeamsAsync();

        return result.Count > 0 
            ? Result<List<Team>>.Success(result, "Teams successfully found.")
            : Result<List<Team>>.Failure("No teams found.");
    }

    public async Task<Result<Team>> GetTeamByIdAsync(string teamId)
    {
        ArgumentException.ThrowIfNullOrEmpty(teamId);

        var existingTeam = await _teamRepo.GetTeamByIdAsync(teamId);

        return existingTeam != null
            ? Result<Team>.Success(existingTeam, "Team successfully found.")
            : Result<Team>.Failure("Coulnd't find the specified team.");
    }

    public async Task<Result<Team>> GetTeamByNameAsync(string teamName)
    {
        ArgumentException.ThrowIfNullOrEmpty(teamName);

        var existingTeam = await _teamRepo.GetTeamByNameAsync(teamName);

        return existingTeam != null 
            ? Result<Team>.Success(existingTeam, "Team successfully found.")
            : Result<Team>.Failure("Coulnd't find the specified team.");
    }

    public async Task<Result<Team>> UpdateTeamAsync(Team updatedTeam)
    {
        ArgumentNullException.ThrowIfNull(updatedTeam);

        var result = await _teamRepo.UpdateTeamAsync(updatedTeam);

        return result.ModifiedCount > 0
            ? Result<Team>.Success(updatedTeam, "Team successfully updated.")
            : Result<Team>.Failure("An error occurred trying to update team.");
    }

    public async Task<Result<Team>> DeleteTeamAsync(string teamId)
    {
        ArgumentException.ThrowIfNullOrEmpty(teamId);

        var result = await _teamRepo.DeleteTeamAsync(teamId);

        return result.DeletedCount > 0
            ? Result<Team>.Success(null, "Team successfully deleted.")
            : Result<Team>.Failure("Team not found.");
    }
}
