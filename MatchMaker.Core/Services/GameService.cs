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
}
