using DnsClient.Internal;
using MatchMaker.Core.Interfaces;
using MatchMaker.Core.Utilities;
using MatchMaker.Data.Interfaces;
using MatchMaker.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace MatchMaker.Core.Services;

public class GameService(ILogger<GameService> logger, IGameRepo gameRepo) : IGameService
{
    private readonly ILogger<GameService> _logger = logger;
    private readonly IGameRepo _gameRepo = gameRepo;
    public async Task<Result<Game>> CreateGameAsync(Game newGame)
    {
        ArgumentNullException.ThrowIfNull(newGame);

        try
        {
            var result = await _gameRepo.CreateGameAsync(newGame);
            return result != null
                ? Result<Game>.Success(result, "Game successfully created.")
                : Result<Game>.Failure("Couldn't create game.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred in GameService while trying to create a game.");
            throw;
        }
    }

    public async Task<Result<List<Game>>> GetAllGamesAsync()
    {
        try
        {
            var result = await _gameRepo.GetAllGamesAsync();
            return result.Count > 0
                ? Result<List<Game>>.Success(result, "Games successfully found.")
                : Result<List<Game>>.Failure("Couldn't find any games.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred in GameService while trying to get all games.");
            throw;
        }
    }

    public async Task<Result<Game>> GetGameByIdAsync(string gameId)
    {
        ArgumentNullException.ThrowIfNull(gameId);

        try
        {
            var result = await _gameRepo.GetGameByIdAsync(gameId);
            return result != null
                ? Result<Game>.Success(result, "Game successfully found.")
                : Result<Game>.Failure("Couldn't find game.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred in GameService while trying to get game by GameId.");
            throw;
        }
    }

    public async Task<Result<Game>> UpdateGameAsync(Game updatedGame)
    {
        ArgumentNullException.ThrowIfNull(updatedGame);

        try
        {
            var result = await _gameRepo.UpdateGameAsync(updatedGame);
            return result.ModifiedCount > 0
                ? Result<Game>.Success(updatedGame, "Game successfully updated.")
                : Result<Game>.Failure("No changes were made.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred in GameService while trying to update game.");
            throw;
        }
    }

    public async Task<Result<Game>> DeleteGameAsync(string gameId)
    {
        ArgumentException.ThrowIfNullOrEmpty(gameId);

        try
        {
            var result = await _gameRepo.DeleteGameAsync(gameId);
            return result.DeletedCount > 0
                ? Result<Game>.Success(null, "Game successfully deleted.")
                : Result<Game>.Failure("Game not found.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred in GameService while trying to delete game.");
            throw;
        }
    }
}
