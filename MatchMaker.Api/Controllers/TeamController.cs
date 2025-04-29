using MatchMaker.Core.Interfaces;
using MatchMaker.Domain.DTOs.Teams;
using Microsoft.AspNetCore.Mvc;

namespace MatchMaker.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TeamController(ILogger<TeamController> logger, ITeamServiceFacade teamServiceFacade) : ControllerBase
{
    private readonly ILogger _logger = logger;
    private readonly ITeamServiceFacade _teamServiceFacade = teamServiceFacade;

    [HttpPost]
    public async Task<IActionResult> CreateTeamAsync([FromBody] CreateTeamDTO newTeam)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);

        try
        {
            var result = await _teamServiceFacade.CreateTeamAsync(newTeam);

            if (!result.IsSuccess)
            {
                return Conflict(new ProblemDetails
                {
                    Title = "Team creation failed",
                    Detail = result.Message,
                    Status = StatusCodes.Status409Conflict
                });
            }

            var test = CreatedAtRoute(nameof(GetTeamByIdAsync), new { teamId = result.Data!.Id }, result.Data);

            return test;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An unexpected error occurred while trying to create team {ex.Message}");
            return StatusCode(500, new ProblemDetails
            {
                Title = "Internal Server Error",
                Detail = "An unexpected error occured. Please try again later",
                Status = StatusCodes.Status500InternalServerError
            });
        }
    }

    [HttpGet("{teamId}", Name = "GetTeamByIdAsync")]
    public async Task<IActionResult> GetTeamByIdAsync(string teamId)
    {
        _logger.LogInformation($"{teamId}");
        return Ok();
    }
}
