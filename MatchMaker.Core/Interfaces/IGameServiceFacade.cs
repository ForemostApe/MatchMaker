﻿using MatchMaker.Domain.DTOs.Games;
using MatchMaker.Core.Utilities;

namespace MatchMaker.Core.Interfaces
{
    public interface IGameServiceFacade
    {
        Task<Result<GameDTO>> CreateGameAsync(CreateGameDTO newGame);
        Task<Result<List<GameDTO>>> GetAllGamesAsync();
        Task<Result<GameDTO>> GetGameByIdAsync(string gameId);
        Task<Result<GameDTO>> UpdateGameAsync(UpdateGameDTO updatedGame);
        Task<Result<GameDTO>> DeleteGameAsync(string gameId);
        Task<Result<GameDTO>> HandleCoachResponseAsync(GameResponseDTO response);
        Task<Result<GameDTO>> HandleRefereeResponseAsync(GameResponseDTO response);
    }
}
