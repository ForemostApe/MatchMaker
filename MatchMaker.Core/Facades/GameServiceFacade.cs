using Mapster;
using MapsterMapper;
using MatchMaker.Core.Interfaces;
using MatchMaker.Domain.DTOs;
using MatchMaker.Domain.DTOs.Games;
using MatchMaker.Domain.Entities;

namespace MatchMaker.Core.Facades;

public class GameServiceFacade(IGameService gameService, IMapper mapper) : IGameServiceFacade
{
    private readonly IGameService _gameService = gameService;
    private readonly IMapper _mapper = mapper;

    public async Task<Result<GameDTO>> CreateGameAsync(CreateGameDTO newGame)
    {
        ArgumentNullException.ThrowIfNull(newGame);

        try
        {
            var game = _mapper.Map<Game>(newGame);

            var result = await _gameService.CreateGameAsync(game);
            if (!result.IsSuccess) return Result<GameDTO>.Failure(result.Message);



            var createdGame = _mapper.Map<GameDTO>(result.Data!);
            return Result<GameDTO>.Success(createdGame, result.Message);
        }
        catch
        {
            throw;
        }
    }

    public async Task<Result<List<GameDTO>>> GetAllGamesAsync()
    {
        try
        {
            var result = await _gameService.GetAllGamesAsync();
            if (!result.IsSuccess) return Result<List<GameDTO>>.Failure(result.Message);

            var games = _mapper.Map<List<GameDTO>>(result.Data!);
            return Result<List<GameDTO>>.Success(games, result.Message);
        }
        catch
        {
            throw;
        }
    }

    public async Task<Result<GameDTO>> GetGameByIdAsync(string gameId)
    {
        ArgumentException.ThrowIfNullOrEmpty(gameId);

        try
        {
            var result = await _gameService.GetGameByIdAsync(gameId);
            if(!result.IsSuccess) return Result<GameDTO>.Failure(result.Message);

            var game = _mapper.Map<GameDTO>(result.Data!);
            return Result<GameDTO>.Success(game, result.Message);
        }
        catch
        {
            throw;
        }
    }

    public async Task<Result<GameDTO>> UpdateGameAsync(UpdateGameDTO updatedGame)
    {
        ArgumentNullException.ThrowIfNull(updatedGame);

        try
        {
            var existingGame = await _gameService.GetGameByIdAsync(updatedGame.Id);
            if(!existingGame.IsSuccess) return Result<GameDTO>.Failure(existingGame.Message);

            updatedGame.Adapt(existingGame.Data!);

            var result = await _gameService.UpdateGameAsync(existingGame.Data!);

            if (!result.IsSuccess) return Result<GameDTO>.Failure(result.Message);

            var game = result.Data!.Adapt<GameDTO>();

            return Result<GameDTO>.Success(game, result.Message);

        }
        catch
        {
            throw;
        }
    }

    public async Task<Result<GameDTO>> DeleteGameAsync(string gameId)
    {
        ArgumentException.ThrowIfNullOrEmpty(gameId);
        try
        {
            var result = await _gameService.DeleteGameAsync(gameId);

            if (!result.IsSuccess) return Result<GameDTO>.Failure(result.Message);

            return Result<GameDTO>.Success(null, result.Message);
        }
        catch
        {
            throw;
        }
    }
}
