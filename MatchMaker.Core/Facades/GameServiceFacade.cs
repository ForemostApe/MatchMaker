using DnsClient.Internal;
using Mapster;
using MapsterMapper;
using MatchMaker.Core.Interfaces;
using MatchMaker.Domain.DTOs;
using MatchMaker.Domain.DTOs.Games;
using MatchMaker.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace MatchMaker.Core.Facades;

public class GameServiceFacade(ILogger<GameServiceFacade> logger, IGameService gameService, IMapper mapper, IEmailService emailService, IUserService userService) : IGameServiceFacade
{
    private readonly ILogger<GameServiceFacade> _logger = logger;
    private readonly IGameService _gameService = gameService;
    private readonly IMapper _mapper = mapper;
    private readonly IEmailService _emailService = emailService;
    private readonly IUserService _userService = userService;

    public async Task<Result<GameDTO>> CreateGameAsync(CreateGameDTO newGame)
    {
        ArgumentNullException.ThrowIfNull(newGame);

        try
        {
            var game = _mapper.Map<Game>(newGame);

            var awayTeamCoach = await _userService.GetUsersByRoleAsync(UserRole.Coach, newGame.AwayTeamId);
            if (!awayTeamCoach.IsSuccess)
            {
                return Result<GameDTO>.Failure(
                    "Coach not found",
                    awayTeamCoach.Message,
                    StatusCodes.Status404NotFound);
            }

            var result = await _gameService.CreateGameAsync(game);
            if (!result.IsSuccess)
            {
                return Result<GameDTO>.Failure(
                    "Creation unsuccessful",
                    result.Message,
                    StatusCodes.Status400BadRequest);
            }

            await _emailService.CreateEmailAsync(awayTeamCoach.Data![0].Email, Services.EmailService.EmailType.GameNotification);

            var createdGame = _mapper.Map<GameDTO>(result.Data!);
            return Result<GameDTO>.Success(createdGame, result.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred in GameServiceFacade while trying to create a game.");
            throw;
        }
}

    public async Task<Result<List<GameDTO>>> GetAllGamesAsync()
    {
        try
        {
            var result = await _gameService.GetAllGamesAsync();
            if (!result.IsSuccess)
            {
                return Result<List<GameDTO>>.Failure(
                    "Game not found",
                    result.Message,
                    StatusCodes.Status404NotFound);
            }

            var games = _mapper.Map<List<GameDTO>>(result.Data!);
            return Result<List<GameDTO>>.Success(games, result.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred in GameServiceFacade while trying to get all games.");
            throw;
        }
    }

    public async Task<Result<GameDTO>> GetGameByIdAsync(string gameId)
    {
        ArgumentException.ThrowIfNullOrEmpty(gameId);

        try
        {
            var result = await _gameService.GetGameByIdAsync(gameId);
            if (!result.IsSuccess)
            {
                return Result<GameDTO>.Failure(
                    "Game not found",
                    result.Message,
                    StatusCodes.Status404NotFound
                    );
            }

            var game = _mapper.Map<GameDTO>(result.Data!);
            return Result<GameDTO>.Success(game, result.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred in GameServiceFacade while trying to get game by GameId.");
            throw;
        }
    }

    public async Task<Result<GameDTO>> UpdateGameAsync(UpdateGameDTO updatedGame)
    {
        ArgumentNullException.ThrowIfNull(updatedGame);

        try
        {
            var existingGame = await _gameService.GetGameByIdAsync(updatedGame.Id);
            if (!existingGame.IsSuccess)
            {
                return Result<GameDTO>.Failure(
                    "Game not found",
                    existingGame.Message,
                    StatusCodes.Status404NotFound
                    );
            }

            updatedGame.Adapt(existingGame.Data!);

            var result = await _gameService.UpdateGameAsync(existingGame.Data!);
            if (!result.IsSuccess)
            {
                return Result<GameDTO>.Failure(
                    "No change made.",
                    result.Message,
                    StatusCodes.Status200OK
                    );
            }

            var game = _mapper.Map<GameDTO>(result.Data!);
            return Result<GameDTO>.Success(game, result.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred in GameServiceFacade while trying to update game.");
            throw;
        }
    }

    public async Task<Result<GameDTO>> DeleteGameAsync(string gameId)
    {
        ArgumentException.ThrowIfNullOrEmpty(gameId);

        try
        {
            var result = await _gameService.DeleteGameAsync(gameId);
            if (!result.IsSuccess)
            {
                return Result<GameDTO>.Failure(
                    "Deletion unsuccessful.",
                    result.Message,
                    StatusCodes.Status404NotFound
                );
            }

            return Result<GameDTO>.Success(null, result.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred in GameServiceFacade while trying to delete game.");
            throw;
        }
    }

    public async Task<Result<GameDTO>> HandleCoachResponseAsync(GameResponseDTO response)
    {
        ArgumentNullException.ThrowIfNull(response);

        try
        {
            var existingGame = await _gameService.GetGameByIdAsync(response.GameId);
            if (!existingGame.IsSuccess || existingGame.Data is null)
            {
                return Result<GameDTO>.Failure(
                    "Game not found",
                    existingGame.Message,
                    StatusCodes.Status404NotFound
                );
            }

            Game game = existingGame.Data;

            if (!response.Accepted)
            {
                game.GameStatus = GameStatus.Draft;
                game.IsCoachSigned = false;

                response.Adapt(game);

                var declineResult = await _gameService.UpdateGameAsync(game);

                if (!declineResult.IsSuccess)
                {
                    return Result<GameDTO>.Failure(
                        "Failed to register declined booking",
                        declineResult.Message,
                        StatusCodes.Status400BadRequest
                        );
                }

                return Result<GameDTO>.Success(_mapper.Map<GameDTO>(game), "Coach declined the game.");
            }

            game.IsCoachSigned = true;
            game.CoachSignedDate = DateTime.UtcNow;
            game.GameStatus = game.IsRefereeSigned ? GameStatus.Booked : GameStatus.Signed;

            var acceptedResult = await _gameService.UpdateGameAsync(game);
            return acceptedResult.IsSuccess
                ? Result<GameDTO>.Success(_mapper.Map<GameDTO>(game), "Coach successfully signed the game.")
                : Result<GameDTO>.Failure(
                    "Failed to update game.",
                    acceptedResult.Message,
                    StatusCodes.Status400BadRequest
                );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred in GameServiceFacade while trying to handle coach-response.");
            throw;
        }
    }

    public async Task<Result<GameDTO>> HandleRefereeResponseAsync(GameResponseDTO response)
    {
        ArgumentNullException.ThrowIfNull(response);

        try
        {
            var existingGame = await _gameService.GetGameByIdAsync(response.GameId);
            if (!existingGame.IsSuccess || existingGame.Data is null)
            {
                return Result<GameDTO>.Failure(
                    "Game not found",
                    existingGame.Message,
                    StatusCodes.Status404NotFound
                );
            }

            Game game = existingGame.Data;

            if (!response.Accepted)
            {
                game.GameStatus = GameStatus.Signed;
                game.IsRefereeSigned = false;

                response.Adapt(game);

                var declineResult = await _gameService.UpdateGameAsync(game);

                if (!declineResult.IsSuccess)
                {
                    return Result<GameDTO>.Failure(
                        "Failed to register declined booking",
                        declineResult.Message,
                        StatusCodes.Status400BadRequest
                        );
                }

                return Result<GameDTO>.Success(_mapper.Map<GameDTO>(game), "Referee declined the game.");
            }

            game.IsRefereeSigned = true;
            game.RefereeSignedDate = DateTime.UtcNow;
            game.GameStatus = game.IsCoachSigned ? GameStatus.Booked : GameStatus.Signed;

            var acceptedResult = await _gameService.UpdateGameAsync(game);
            return acceptedResult.IsSuccess
                ? Result<GameDTO>.Success(_mapper.Map<GameDTO>(game), "Referee successfully signed the game.")
                : Result<GameDTO>.Failure(
                    "Failed to update game.",
                    acceptedResult.Message,
                    StatusCodes.Status400BadRequest
                );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred in GameServiceFacade while trying to handle referee-response.");
            throw;
        }
    }
}
