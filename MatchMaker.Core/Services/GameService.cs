using MatchMaker.Core.Interfaces;
using MatchMaker.Data.Interfaces;
using MatchMaker.Domain.DTOs;
using MatchMaker.Domain.Entities;

namespace MatchMaker.Core.Services;

public class GameService(IGameRepo gameRepo) : IGameService
{
    private readonly IGameRepo _gameRepo = gameRepo;
    public async Task<Result<Game>> CreateGameAsync(Game newGame)
    {
        ArgumentNullException.ThrowIfNull(newGame);

        var result = await _gameRepo.CreateGameAsync(newGame);
        return result != null 
            ? Result<Game>.Success(result, "Game successfully created.")
            : Result<Game>.Failure("Couldn't create game.");
    }

    public async Task<Result<List<Game>>> GetAllGamesAsync()
    {
        var result = await _gameRepo.GetAllGamesAsync();
        return result.Count > 0
            ? Result<List<Game>>.Success(result, "Games successfully found.")
            : Result<List<Game>>.Failure("Couldn't find any games.");
    }

    public async Task<Result<Game>> GetGameByIdAsync(string gameId)
    {
        ArgumentNullException.ThrowIfNull(gameId);

        var result = await _gameRepo.GetGameByIdAsync(gameId);
        return result != null 
            ? Result<Game>.Success(result, "Game successfully found.") 
            : Result<Game>.Failure("Couldn't find game.");
    }

    public async Task<Result<Game>> UpdateGameAsync(Game updatedGame)
    {
        ArgumentNullException.ThrowIfNull(updatedGame);

        var result = await _gameRepo.UpdateGameAsync(updatedGame);
        return result.ModifiedCount > 0 
            ? Result<Game>.Success(updatedGame, "Game successfully updated.") 
            : Result<Game>.Failure("No changes were made.");
    }

    public async Task<Result<Game>> DeleteGameAsync(string gameId)
    {
        ArgumentException.ThrowIfNullOrEmpty(gameId);

        var result = await _gameRepo.DeleteGameAsync(gameId);
        return result.DeletedCount > 0 
            ? Result<Game>.Success(null, "Game successfully deleted.") 
            : Result<Game>.Failure("Game not found.");
    }
}
