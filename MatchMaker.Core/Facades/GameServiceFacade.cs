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
}
