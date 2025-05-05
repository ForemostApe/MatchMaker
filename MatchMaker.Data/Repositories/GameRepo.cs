using MatchMaker.Data.Interfaces;
using MatchMaker.Domain.Entities;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

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
}
