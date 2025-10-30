using MatchMaker.Core.Interfaces;
using MatchMaker.Domain.DTOs.Games;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MatchMaker.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GameController(ILogger<GameController> logger, IGameServiceFacade gameServiceFacade) : ControllerBase
    {
        private readonly ILogger _logger = logger;
        private readonly IGameServiceFacade _gameServiceFacade = gameServiceFacade;

        [HttpPost]
        [Authorize (Roles = "Coach")]
        public async Task<IActionResult> CreateGameAsync([FromBody] CreateGameDto newGame)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var result = await _gameServiceFacade.CreateGameAsync(newGame);
                if (!result.IsSuccess) return StatusCode(result.GetStatusCodeOrDefault(), new ProblemDetails()
                {
                    Title = result.Title,
                    Detail = result.Message,
                    Status = result.StatusCode
                });

                return CreatedAtRoute(nameof(GetGameByIdAsync), new { gameId = result.Data!.Id }, result.Data);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while trying to create game.");
                return StatusCode(500, new ProblemDetails
                {
                    Title = "Internal Server Error",
                    Detail = "An unexpected error occured. Please try again later",
                    Status = StatusCodes.Status500InternalServerError
                });
            }
        }

        [HttpGet(Name = nameof(GetAllGamesAsync))]
        public async Task<IActionResult> GetAllGamesAsync()
        {
            try
            {
                var result = await _gameServiceFacade.GetAllGamesAsync();
                if (!result.IsSuccess) return StatusCode(result.GetStatusCodeOrDefault(), new ProblemDetails()
                {
                    Title = result.Title,
                    Detail = result.Message,
                    Status = result.StatusCode
                });

                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while trying to find all games.");
                return StatusCode(500, new ProblemDetails
                {
                    Title = "Internal Server Error",
                    Detail = "An unexpected error occured. Please try again later",
                    Status = StatusCodes.Status500InternalServerError
                });
            }
        }

        [HttpGet("id/{gameId}", Name = nameof(GetGameByIdAsync))]
        public async Task<IActionResult> GetGameByIdAsync(string gameId)
        {
            ArgumentException.ThrowIfNullOrEmpty(gameId);

            try
            {
                var result = await _gameServiceFacade.GetGameByIdAsync(gameId);
                if (!result.IsSuccess) return StatusCode(result.GetStatusCodeOrDefault(), new ProblemDetails()
                {
                    Title = result.Title,
                    Detail = result.Message,
                    Status = result.StatusCode
                });

                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while trying to create game.");
                return StatusCode(500, new ProblemDetails
                {
                    Title = "Internal Server Error",
                    Detail = "An unexpected error occured. Please try again later",
                    Status = StatusCodes.Status500InternalServerError
                });
            }
        }

        [Authorize (Roles = "Coach")]
        [HttpPatch(Name = nameof(UpdateGameAsync))]
        public async Task<IActionResult> UpdateGameAsync([FromBody] UpdateGameDto updatedGame)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var result = await _gameServiceFacade.UpdateGameAsync(updatedGame);
                if (!result.IsSuccess) return StatusCode(result.GetStatusCodeOrDefault(), new ProblemDetails()
                {
                    Title = result.Title,
                    Detail = result.Message,
                    Status = result.StatusCode
                });

                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while trying to update game.");
                return StatusCode(500, new ProblemDetails
                {
                    Title = "Internal Server Error",
                    Detail = "An unexpected error occured. Please try again later",
                    Status = StatusCodes.Status500InternalServerError
                });
            }
        }

        [HttpDelete(Name = nameof(DeleteGameAsync))]
        public async Task<IActionResult> DeleteGameAsync(string gameId)
        {
            ArgumentException.ThrowIfNullOrEmpty(gameId);

            try
            {
                var result = await _gameServiceFacade.DeleteGameAsync(gameId);
                if (!result.IsSuccess) return StatusCode(result.GetStatusCodeOrDefault(), new ProblemDetails()
                {
                    Title = result.Title,
                    Detail = result.Message,
                    Status = result.StatusCode
                });

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while trying to delete game.");
                return StatusCode(500, new ProblemDetails
                {
                    Title = "Internal Server Error",
                    Detail = "An unexpected error occured. Please try again later",
                    Status = StatusCodes.Status500InternalServerError
                });
            }
        }

        [HttpPost("userResponse", Name = nameof(HandleUserResponseAsync))]
        [Authorize (Roles = "Coach, Referee")]
        public async Task<IActionResult> HandleUserResponseAsync([FromBody] GameResponseDto response)
        {
            ArgumentNullException.ThrowIfNull(response);

            List<Claim> userClaims = User.Claims.ToList();

            try
            {
                var result = await _gameServiceFacade.HandleUserResponseAsync(response, userClaims);
                if (!result.IsSuccess) return StatusCode(result.GetStatusCodeOrDefault(), new ProblemDetails()
                {
                    Title = result.Title,
                    Detail = result.Message,
                    Status = result.StatusCode
                });
                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while trying to respond to game.");
                return StatusCode(500, new ProblemDetails
                {
                    Title = "Internal Server Error",
                    Detail = "An unexpected error occured. Please try again later",
                    Status = StatusCodes.Status500InternalServerError
                });
            }
        }
    }
}
