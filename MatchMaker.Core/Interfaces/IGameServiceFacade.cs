using MatchMaker.Domain.DTOs.Games;
using MatchMaker.Domain.DTOs;

namespace MatchMaker.Core.Interfaces
{
    public interface IGameServiceFacade
    {
        Task<Result<GameDTO>> CreateGameAsync(CreateGameDTO newGame);
    }
}
