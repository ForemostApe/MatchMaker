using MatchMaker.Domain.DTOs.Games;
using MatchMaker.Domain.DTOs;

namespace MatchMaker.Core.Interfaces
{
    public interface IGameServiceFacade
    {
        Task<Result<GameDTO>> CreateGameAsync(CreateGameDTO newGame);
        Task<Result<List<GameDTO>>> GetAllGamesAsync();
        Task<Result<GameDTO>> GetGameByIdAsync(string gameId);
    }
}
