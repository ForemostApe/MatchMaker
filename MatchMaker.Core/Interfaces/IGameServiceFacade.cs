using MatchMaker.Domain.DTOs.Games;
using MatchMaker.Core.Utilities;
using System.Security.Claims;

namespace MatchMaker.Core.Interfaces
{
    public interface IGameServiceFacade
    {
        Task<Result<GameDto>> CreateGameAsync(CreateGameDto newGame);
        Task<Result<List<GameDto>>> GetAllGamesAsync();
        Task<Result<GameDto>> GetGameByIdAsync(string gameId);
        Task<Result<GameDto>> UpdateGameAsync(UpdateGameDto updatedGame);
        Task<Result<GameDto>> DeleteGameAsync(string gameId);
        Task<Result<GameDto>> HandleUserResponseAsync(GameResponseDto response, List<Claim> userClaims);
    }
}
