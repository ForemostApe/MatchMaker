using MatchMaker.Domain.DTOs;
using MatchMaker.Domain.Entities;

namespace MatchMaker.Core.Interfaces;

public interface IGameService
{
    Task<Result<Game>> CreateGameAsync(Game newGame);
}
