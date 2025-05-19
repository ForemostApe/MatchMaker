using DnsClient.Internal;
using MatchMaker.Core.Interfaces;
using MatchMaker.Core.Utilities;
using MatchMaker.Data.Interfaces;
using MatchMaker.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace MatchMaker.Core.Services;

public class TeamService(ILogger<TeamService> logger, ITeamRepo teamRepo) : ITeamService
{
    private readonly ILogger<TeamService> _logger = logger;
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
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occured in the TeamService while trying to create team.");
            throw;
        }
    }

    public async Task<Result<List<Team>>> GetAllTeamsAsync()
    {
        try
        {
            var result = await _teamRepo.GetAllTeamsAsync();

            return result.Count > 0
                ? Result<List<Team>>.Success(result, "Teams successfully found.")
                : Result<List<Team>>.Failure("No teams found.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occured in the TeamService while trying to get all teams.");
            throw;
        }
    }

    public async Task<Result<Team>> GetTeamByIdAsync(string teamId)
    {
        ArgumentException.ThrowIfNullOrEmpty(teamId);

        try
        {
            var existingTeam = await _teamRepo.GetTeamByIdAsync(teamId);

            return existingTeam != null
                ? Result<Team>.Success(existingTeam, "Team successfully found.")
                : Result<Team>.Failure("Coulnd't find the specified team.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occured in the TeamService while trying to get team by TeamId.");
            throw;
        }
    }

    public async Task<Result<Team>> GetTeamByNameAsync(string teamName)
    {
        ArgumentException.ThrowIfNullOrEmpty(teamName);

        try
        {
            var existingTeam = await _teamRepo.GetTeamByNameAsync(teamName);

            return existingTeam != null
                ? Result<Team>.Success(existingTeam, "Team successfully found.")
                : Result<Team>.Failure("Coulnd't find the specified team.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occured in the TeamService while trying to get team by TeamName.");
            throw;
        }
    }

    public async Task<Result<Team>> UpdateTeamAsync(Team updatedTeam)
    {
        ArgumentNullException.ThrowIfNull(updatedTeam);

        try
        {
            var result = await _teamRepo.UpdateTeamAsync(updatedTeam);

            return result.ModifiedCount > 0
                ? Result<Team>.Success(updatedTeam, "Team successfully updated.")
                : Result<Team>.Failure("An error occurred trying to update team.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occured in the TeamService while trying to update team.");
            throw;
        }
    }

    public async Task<Result<Team>> DeleteTeamAsync(string teamId)
    {
        ArgumentException.ThrowIfNullOrEmpty(teamId);

        try
        {
            var result = await _teamRepo.DeleteTeamAsync(teamId);

            return result.DeletedCount > 0
                ? Result<Team>.Success(null, "Team successfully deleted.")
                : Result<Team>.Failure("Team not found.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occured in the TeamService while trying to delete team.");
            throw;
        }
    }
}
