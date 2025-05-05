using MatchMaker.Core.Interfaces;
using MatchMaker.Domain.DTOs.Games;
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
        public async Task<IActionResult> CreateGameAsync(CreateGameDTO newGame)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var result = await _gameServiceFacade.CreateGameAsync(newGame);
                if (!result.IsSuccess) return BadRequest(result.Message);

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

        [HttpGet("id/{gameId}", Name = nameof(GetGameByIdAsync))]
        public async Task<IActionResult> GetGameByIdAsync(string gameId)
        {
            return Ok();
        }
    }
}
