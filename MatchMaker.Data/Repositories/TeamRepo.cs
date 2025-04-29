using MatchMaker.Data.Interfaces;
using MatchMaker.Domain.Entities;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace MatchMaker.Data.Repositories;

public class TeamRepo(ILogger<TeamRepo> logger, IMongoDatabase database) : RepositoryBase<Team>(logger, database, "teams"), ITeamRepo
{
    public async Task<Team> CreateTeamAsync(Team newTeam)
    {
        return await InsertOneAsync(newTeam);
    }

    public async Task<Team?> GetTeamByIdAsync(string teamId)
    {
        var filter = Builders<Team>.Filter.Eq(t => t.Id, teamId);
        return await FindOneAsync(filter);
    }

    public async Task<Team?> GetTeamByNameAsync(string teamName)
    {
        var filter = Builders<Team>.Filter.Regex(t => t.TeamName, new MongoDB.Bson.BsonRegularExpression($"^{teamName}$", "i"));
        return await FindOneAsync(filter);
    }

    public async Task UpdateTeamAsync(Team updatedTeam)
    {
        var filter = Builders<Team>.Filter.Eq(t => t.Id, updatedTeam.Id);
        var update = Builders<Team>.Update
            .Set(t => t.TeamName, updatedTeam.TeamName)
            .Set(t => t.TeamLogo, updatedTeam.TeamName);

        await UpdateOneAsync(filter, update);
    }

    public async Task<DeleteResult> DeleteTeamAsync(string teamId)
    {
        var filter = Builders<Team>.Filter.Eq(t => t.Id, teamId);
        return await DeleteOneAsync(filter);
    }
}
