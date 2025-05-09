using MatchMaker.Core.Interfaces;
using MatchMaker.Domain.DTOs.Games;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MatchMaker.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController(ILogger<GameController> logger, IGameServiceFacade gameServiceFacade) : ControllerBase
    {
        private readonly ILogger _logger = logger;
        private readonly IGameServiceFacade _gameServiceFacade = gameServiceFacade;

        [HttpPost]
        public async Task<IActionResult> CreateGameAsync([FromBody] CreateGameDTO newGame)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var result = await _gameServiceFacade.CreateGameAsync(newGame);
                if (!result.IsSuccess) return BadRequest(new ProblemDetails()
                {
                    Title = "Couldn't create game.",
                    Detail = result.Message ?? "Couldn't create game.",
                    Status = StatusCodes.Status400BadRequest
                });

                return CreatedAtRoute(nameof(GetGameByIdAsync), new { gameId = result.Data!.Id }, result.Data);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An unexpected error occurred while trying to create game {ex.Message}");
                return StatusCode(500, new ProblemDetails
                {
                    Title = "Internal Server Error",
                    Detail = "An unexpected error occured. Please try again later",
                    Status = StatusCodes.Status500InternalServerError
                });
            }
        }
        [Authorize]
        [HttpGet(Name = nameof(GetAllGamesAsync))]
        public async Task<IActionResult> GetAllGamesAsync()
        {
            try
            {
                var result = await _gameServiceFacade.GetAllGamesAsync();
                if (!result.IsSuccess) return NotFound(new ProblemDetails()
                {
                    Title = "Games not found.",
                    Detail = result.Message ?? "No games were found.",
                    Status = StatusCodes.Status404NotFound
                });

                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An unexpected error occurred while trying to find all games {ex.Message}");
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
                if (!result.IsSuccess) return NotFound(new ProblemDetails
                {
                    Title = "Game not found",
                    Detail = result.Message ?? "The specified team was not found.",
                    Status = StatusCodes.Status404NotFound
                });

                return Ok(result.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An unexpected error occurred while trying to create game {ex.Message}");
                return StatusCode(500, new ProblemDetails
                {
                    Title = "Internal Server Error",
                    Detail = "An unexpected error occured. Please try again later",
                    Status = StatusCodes.Status500InternalServerError
                });
            }
        }

        [HttpPatch(Name = nameof(UpdateGameAsync))]
        public async Task<IActionResult> UpdateGameAsync([FromBody] UpdateGameDTO updatedGame)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var result = await _gameServiceFacade.UpdateGameAsync(updatedGame);

                if (!result.IsSuccess)
                {
                    return NotFound(new ProblemDetails
                    {
                        Title = "Game not found",
                        Detail = result.Message,
                        Status = StatusCodes.Status404NotFound
                    });
                }

                return Ok(result.Data!);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An unexpected error occurred while trying to update game {ex.Message}");
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

                if (!result.IsSuccess) return NotFound(new ProblemDetails()
                {
                    Title = "Game not found",
                    Detail = result.Message,
                    Status = StatusCodes.Status404NotFound
                });

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An unexpected error occurred while trying to delete game {ex.Message}");
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
