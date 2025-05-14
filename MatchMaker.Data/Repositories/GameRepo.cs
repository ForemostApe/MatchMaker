using MatchMaker.Data.Interfaces;
using MatchMaker.Domain.Entities;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System.Data;

namespace MatchMaker.Data.Repositories;

public class GameRepo(ILogger<GameRepo> logger, IMongoDatabase database) : RepositoryBase<Game>(logger, database, "games"), IGameRepo
{
    public async Task<Game> CreateGameAsync(Game newGame)
    {
        try
        {
            return await InsertOneAsync(newGame);
        }
        catch
        {
            throw;
        }
    }

    public async Task<List<Game>> GetAllGamesAsync()
    {
        try
        {
            return await FindAllAsync();
        }
        catch
        {
            throw;
        }
    }

    public async Task<Game> GetGameByIdAsync(string gameId)
    {
        try
        {
            var filter = Builders<Game>.Filter.Eq(g => g.Id, gameId);
            return await FindOneAsync(filter);
        }
        catch
        {
            throw;
        }
    }

    public async Task<UpdateResult> UpdateGameAsync(Game updatedGame)
    {
        try
        {
            var filter = Builders<Game>.Filter.Eq(g => g.Id, updatedGame.Id);
            var update = Builders<Game>.Update
                .Set(g => g.StartTime, updatedGame.StartTime)
                .Set(g => g.Location, updatedGame.Location)
                .Set(g => g.RefereeId, updatedGame.RefereeId)
                .Set(g => g.IsCoachSigned, updatedGame.IsCoachSigned)
                .Set(g => g.CoachSignedDate, updatedGame.CoachSignedDate)
                .Set(g => g.IsRefereeSigned, updatedGame.IsRefereeSigned)
                .Set(g => g.RefereeSignedDate, updatedGame.RefereeSignedDate);

            if (updatedGame.Conditions != null)
            {
                update = update.Set(g => g.Conditions.Court, updatedGame.Conditions.Court)
                    .Set(g => g.Conditions.OffensiveConditions, updatedGame.Conditions.OffensiveConditions)
                    .Set(g => g.Conditions.DefensiveConditions, updatedGame.Conditions.DefensiveConditions)
                    .Set(g => g.Conditions.Specialists, updatedGame.Conditions.Specialists)
                    .Set(g => g.Conditions.Penalties, updatedGame.Conditions.Penalties);
            }

            return await UpdateOneAsync(filter, update);
        }
        catch
        {
            throw;
        }
    }

    public async Task<DeleteResult> DeleteGameAsync(string gameId)
    {
        try
        {
            var filter = Builders<Game>.Filter.Eq(g => g.Id, gameId);
            return await DeleteOneAsync(filter);
        }
        catch
        {
            throw;
        }
    }
}
