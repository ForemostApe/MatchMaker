using MatchMaker.Data.Interfaces;
using MatchMaker.Domain.Entities;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace MatchMaker.Data.Repositories;

public class TeamRepo(ILogger<TeamRepo> logger, IMongoDatabase database) : RepositoryBase<Team>(logger, database, "teams"), ITeamRepo
{
    public async Task<Team> CreateTeamAsync(Team newTeam)
    {
        ArgumentNullException.ThrowIfNull(newTeam);

        try
        {
            return await InsertOneAsync(newTeam);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred ín the TeamRepo while trying to create team.");
            throw;
        }
    }

    public async Task<List<Team>> GetAllTeamsAsync()
    {
        try
        {
            return await FindAllAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred ín the TeamRepo while trying to get all teams.");
            throw;
        }
    }

    public async Task<Team?> GetTeamByIdAsync(string teamId)
    {
        ArgumentException.ThrowIfNullOrEmpty(teamId);

        try
        {
            var filter = Builders<Team>.Filter.Eq(t => t.Id, teamId);
            return await FindOneAsync(filter);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred ín the TeamRepo while trying to get team by TeamId.");
            throw;
        }
    }

    public async Task<Team?> GetTeamByNameAsync(string teamName)
    {
        ArgumentException.ThrowIfNullOrEmpty(teamName);

        try
        {
            var filter = Builders<Team>.Filter.Regex(t => t.TeamName, new MongoDB.Bson.BsonRegularExpression($"^{teamName}$", "i"));
            return await FindOneAsync(filter);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred ín the TeamRepo while trying to get team by TeamName.");
            throw;
        }
    }

    public async Task<UpdateResult> UpdateTeamAsync(Team updatedTeam)
    {
        ArgumentNullException.ThrowIfNull(updatedTeam);

        try
        {
            var filter = Builders<Team>.Filter.Eq(t => t.Id, updatedTeam.Id);
            var update = Builders<Team>.Update
                .Set(t => t.TeamName, updatedTeam.TeamName)
                .Set(t => t.TeamLogo, updatedTeam.TeamLogo);

            return await UpdateOneAsync(filter, update);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred ín the TeamRepo while trying to update team.");
            throw;
        }
    }

    public async Task<DeleteResult> DeleteTeamAsync(string teamId)
    {
        ArgumentException.ThrowIfNullOrEmpty(teamId);

        try
        {
            var filter = Builders<Team>.Filter.Eq(t => t.Id, teamId);
            return await DeleteOneAsync(filter);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred ín the TeamRepo while trying to delete team.");
            throw;
        }
    }
}
