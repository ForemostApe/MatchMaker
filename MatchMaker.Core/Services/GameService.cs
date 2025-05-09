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

        try
        {
            var result = await _gameRepo.CreateGameAsync(newGame);

            if (result == null) return Result<Game>.Failure("Couldn't create game.");

            return Result<Game>.Success(result, "Game successfully created.");
        }

        catch
        {
            throw;
        }
    }

    public async Task<Result<List<Game>>> GetAllGamesAsync()
    {
        try
        {
            var result = await _gameRepo.GetAllGamesAsync();
            if (result.Count == 0) return Result<List<Game>>.Failure("Couldn't find any games.");

            return Result<List<Game>>.Success(result, "Games successfully found.");
        }
        catch
        {
            throw;
        }
    }

    public async Task<Result<Game>> GetGameByIdAsync(string gameId)
    {
        ArgumentNullException.ThrowIfNull(gameId);

        try
        {
            var result = await _gameRepo.GetGameByIdAsync(gameId);

            if (result == null) return Result<Game>.Failure("Couldn't find game.");

            return Result <Game>.Success(result, "Game successfully found.");
        }
        catch
        {
            throw;
        }
    }

    public async Task<Result<Game>> UpdateGameAsync(Game updatedGame)
    {
        ArgumentNullException.ThrowIfNull(updatedGame);

        try
        {
            var result = await _gameRepo.UpdateGameAsync(updatedGame);

            if (result.ModifiedCount <= 0) return Result<Game>.Failure("No changes were made.");

            return Result<Game>.Success(updatedGame, "Game successfully updated.");
        }
        catch
        {
            throw;
        }
    }

    public async Task<Result<Game>> DeleteGameAsync(string gameId)
    {
        ArgumentException.ThrowIfNullOrEmpty(gameId);

        try
        {
            var result = await _gameRepo.DeleteGameAsync(gameId);
            if (result.DeletedCount <= 0) return Result<Game>.Failure("Game not found.");

            return Result<Game>.Success(null, "Game successfully deleted.");
        }
        catch
        {
            throw;
        }
    }
}
