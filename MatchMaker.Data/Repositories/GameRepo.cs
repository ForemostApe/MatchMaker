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
}
