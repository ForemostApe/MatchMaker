using DnsClient.Internal;
using Mapster;
using MapsterMapper;
using MatchMaker.Core.Interfaces;
using MatchMaker.Core.Utilities;
using MatchMaker.Domain.DTOs.Games;
using MatchMaker.Domain.Entities;
using MatchMaker.Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace MatchMaker.Core.Facades;

public class GameServiceFacade(ILogger<GameServiceFacade> logger, IGameService gameService, IMapper mapper, IEmailService emailService, IUserService userService) : IGameServiceFacade
{
    private readonly ILogger<GameServiceFacade> _logger = logger;
    private readonly IGameService _gameService = gameService;
    private readonly IMapper _mapper = mapper;
    private readonly IEmailService _emailService = emailService;
    private readonly IUserService _userService = userService;

    public async Task<Result<GameDto>> CreateGameAsync(CreateGameDto newGame)
    {
        ArgumentNullException.ThrowIfNull(newGame);

        try
        {
            var game = _mapper.Map<Game>(newGame);

            var awayTeamCoach = await _userService.GetUsersByRoleAsync(UserRole.Coach, newGame.AwayTeamId);
            if (!awayTeamCoach.IsSuccess)
            {
                return Result<GameDto>.Failure(
                    "Coach not found",
                    awayTeamCoach.Message,
                    StatusCodes.Status404NotFound);
            }

            var result = await _gameService.CreateGameAsync(game);
            if (!result.IsSuccess)
            {
                return Result<GameDto>.Failure(
                    "Creation unsuccessful",
                    result.Message,
                    StatusCodes.Status400BadRequest);
            }

            await _emailService.CreateEmailAsync(awayTeamCoach.Data![0].Email, EmailType.GameNotification);

            var createdGame = _mapper.Map<GameDto>(result.Data!);
            return Result<GameDto>.Success(createdGame, result.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred in GameServiceFacade while trying to create a game.");
            throw;
        }
}

    public async Task<Result<List<GameDto>>> GetAllGamesAsync()
    {
        try
        {
            var result = await _gameService.GetAllGamesAsync();
            if (!result.IsSuccess)
            {
                return Result<List<GameDto>>.Failure(
                    "Game not found",
                    result.Message,
                    StatusCodes.Status404NotFound);
            }

            var games = _mapper.Map<List<GameDto>>(result.Data!);
            return Result<List<GameDto>>.Success(games, result.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred in GameServiceFacade while trying to get all games.");
            throw;
        }
    }

    public async Task<Result<GameDto>> GetGameByIdAsync(string gameId)
    {
        ArgumentException.ThrowIfNullOrEmpty(gameId);

        try
        {
            var result = await _gameService.GetGameByIdAsync(gameId);
            if (!result.IsSuccess)
            {
                return Result<GameDto>.Failure(
                    "Game not found",
                    result.Message,
                    StatusCodes.Status404NotFound
                    );
            }

            var game = _mapper.Map<GameDto>(result.Data!);
            return Result<GameDto>.Success(game, result.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred in GameServiceFacade while trying to get game by GameId.");
            throw;
        }
    }

    public async Task<Result<GameDto>> UpdateGameAsync(UpdateGameDto updatedGame)
    {
        ArgumentNullException.ThrowIfNull(updatedGame);

        try
        {
            var existingGame = await _gameService.GetGameByIdAsync(updatedGame.Id);
            if (!existingGame.IsSuccess)
            {
                return Result<GameDto>.Failure(
                    "Game not found",
                    existingGame.Message,
                    StatusCodes.Status404NotFound
                    );
            }

            updatedGame.Adapt(existingGame.Data!);

            var result = await _gameService.UpdateGameAsync(existingGame.Data!);
            if (!result.IsSuccess)
            {
                return Result<GameDto>.Failure(
                    "No change made.",
                    result.Message,
                    StatusCodes.Status200OK
                    );
            }

            var game = _mapper.Map<GameDto>(result.Data!);
            return Result<GameDto>.Success(game, result.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred in GameServiceFacade while trying to update game.");
            throw;
        }
    }

    public async Task<Result<GameDto>> DeleteGameAsync(string gameId)
    {
        ArgumentException.ThrowIfNullOrEmpty(gameId);

        try
        {
            var result = await _gameService.DeleteGameAsync(gameId);
            if (!result.IsSuccess)
            {
                return Result<GameDto>.Failure(
                    "Deletion unsuccessful.",
                    result.Message,
                    StatusCodes.Status404NotFound
                );
            }

            return Result<GameDto>.Success(null, result.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred in GameServiceFacade while trying to delete game.");
            throw;
        }
    }

    public async Task<Result<GameDto>> HandleUserResponseAsync(GameResponseDto response, List<Claim> userRole)
    {
        ArgumentNullException.ThrowIfNull(response);

        try
        {
            var roleClaim = userRole.FirstOrDefault(c => c.Type == ClaimTypes.Role);
            if (roleClaim == null)
            {
                return Result<GameDto>.Failure(
                    "Invalid user role.",
                    "Role claim missing or invalid.",
                    StatusCodes.Status400BadRequest
                );
            }

            var existingGame = await _gameService.GetGameByIdAsync(response.GameId);
            if (!existingGame.IsSuccess || existingGame.Data is null)
            {
                return Result<GameDto>.Failure(
                    "Game not found",
                    existingGame.Message,
                    StatusCodes.Status404NotFound
                );
            }
            
            Game game = existingGame.Data;

            if (roleClaim.Value == UserRole.Coach.ToString())
            {
                if (!response.Accepted)
                {
                    game.GameStatus = GameStatus.Draft;
                    game.IsCoachSigned = false;
                    response.Adapt(game);

                    var declineResult = await _gameService.UpdateGameAsync(game);
                    if (!declineResult.IsSuccess)
                    {
                        return Result<GameDto>.Failure(
                            "Failed to register declined booking",
                            declineResult.Message,
                            StatusCodes.Status400BadRequest
                        );
                    }
                    return Result<GameDto>.Success(_mapper.Map<GameDto>(game), "Coach declined the game.");
                }

                game.IsCoachSigned = true;
                game.CoachSignedDate = DateTime.UtcNow;
                game.GameStatus = game.IsRefereeSigned ? GameStatus.Booked : GameStatus.Signed;
            }
            else if (roleClaim.Value == UserRole.Referee.ToString())
            {
                if (!response.Accepted)
                {
                    game.GameStatus = GameStatus.Signed;
                    game.IsRefereeSigned = false;
                    response.Adapt(game);

                    var declineResult = await _gameService.UpdateGameAsync(game);
                    if (!declineResult.IsSuccess)
                    {
                        return Result<GameDto>.Failure(
                            "Failed to register declined booking",
                            declineResult.Message,
                            StatusCodes.Status400BadRequest
                        );
                    }
                    return Result<GameDto>.Success(_mapper.Map<GameDto>(game), "Referee declined the game.");
                }

                game.IsRefereeSigned = true;
                game.RefereeSignedDate = DateTime.UtcNow;
                game.GameStatus = game.IsCoachSigned ? GameStatus.Booked : GameStatus.Signed;
            }
            else
            {
                return Result<GameDto>.Failure(
                    "Invalid user role for response handling.",
                    "Role not supported.",
                    StatusCodes.Status400BadRequest
                );
            }

            var acceptedResult = await _gameService.UpdateGameAsync(game);
            return acceptedResult.IsSuccess
                ? Result<GameDto>.Success(_mapper.Map<GameDto>(game), $"{userRole} successfully signed the game.")
                : Result<GameDto>.Failure(
                    "Failed to update game.",
                    acceptedResult.Message,
                    StatusCodes.Status400BadRequest
                );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred in GameServiceFacade while handling user response.");
            throw;
        }   
    }
}
