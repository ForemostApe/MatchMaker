using MatchMaker.Domain.Entities;

namespace MatchMaker.Data.Interfaces;

public interface IGameRepo
{
    Task<Game> CreateGameAsync(Game newGame);
    Task<Game> GetGameByIdAsync(string gameId);
    Task<List<Game>> GetAllGamesAsync();
}
