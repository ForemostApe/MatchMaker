using MatchMaker.Domain.Entities;
using MongoDB.Driver;

namespace MatchMaker.Data.Interfaces;

public interface IGameRepo
{
    Task<Game> CreateGameAsync(Game newGame);
    Task<Game> GetGameByIdAsync(string gameId);
    Task<List<Game>> GetAllGamesAsync();
    Task<UpdateResult> UpdateGameAsync(Game updatedGame);
    Task<DeleteResult> DeleteGameAsync(string gameId);
}
