using MatchMaker.Domain.DTOs;
using MatchMaker.Domain.Entities;

namespace MatchMaker.Core.Interfaces;

public interface IGameService
{
    Task<Result<Game>> CreateGameAsync(Game newGame);
    Task<Result<List<Game>>> GetAllGamesAsync();
    Task<Result<Game>> GetGameByIdAsync(string gameId);
    Task<Result<Game>> UpdateGameAsync(Game updatedGame);
    Task<Result<Game>> DeleteGameAsync(string gameId);
}
