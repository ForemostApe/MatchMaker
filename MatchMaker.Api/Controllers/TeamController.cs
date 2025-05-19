using MatchMaker.Core.Interfaces;
using MatchMaker.Domain.DTOs.Teams;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MatchMaker.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class TeamController(ILogger<TeamController> logger, ITeamServiceFacade teamServiceFacade) : ControllerBase
{
    private readonly ILogger _logger = logger;
    private readonly ITeamServiceFacade _teamServiceFacade = teamServiceFacade;

    [HttpPost]
    [Authorize (Roles = "Admin")]
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

            return CreatedAtRoute(nameof(GetTeamByIdAsync), new { teamId = result.Data!.Id }, result.Data);
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

    [HttpGet(Name = nameof(GetAllTeamsAsync))]
    public async Task<IActionResult> GetAllTeamsAsync()
    {
        try
        {
            var result = await _teamServiceFacade.GetAllTeamsAsync();

            if (!result.IsSuccess) return NotFound(new ProblemDetails()
            {
                Title = "Teams not found",
                Detail = result.Message,
                Status = StatusCodes.Status404NotFound
            });

            return Ok(result.Data!);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An unexpected error occurred while trying to get all teams. {ex.Message}");
            return StatusCode(500, new ProblemDetails
            {
                Title = "Internal Server Error",
                Detail = "An unexpected error occured. Please try again later",
                Status = StatusCodes.Status500InternalServerError
            });
        }
    }


    [HttpGet("id/{teamId}", Name = nameof(GetTeamByIdAsync))]
    public async Task<IActionResult> GetTeamByIdAsync(string teamId)
    {
        if (string.IsNullOrEmpty(teamId)) return BadRequest(new ProblemDetails
        {
            Title = "Bad Request",
            Detail = "TeamId must be provided.",
            Status = StatusCodes.Status400BadRequest
        });

        try
        {
            var result = await _teamServiceFacade.GetTeamByIdAsync(teamId);

            if (!result.IsSuccess) 
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Team not found",
                    Detail = result.Message,
                    Status = StatusCodes.Status404NotFound
                });
            }

            return Ok(result.Data!);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An unexpected error occurred while trying to get team by ID. {ex.Message}");
            return StatusCode(500, new ProblemDetails
            {
                Title = "Internal Server Error",
                Detail = "An unexpected error occured. Please try again later",
                Status = StatusCodes.Status500InternalServerError
            });
        }

    }
    [HttpGet("name/{teamName}", Name = nameof(GetTeamByNameAsync))]
    public async Task<IActionResult> GetTeamByNameAsync(string teamName)
    {
        if (string.IsNullOrEmpty(teamName)) return BadRequest(new ProblemDetails
        {
            Title = "Bad Request",
            Detail = "TeamName must be provided.",
            Status = StatusCodes.Status400BadRequest
        });

        try
        {
            var result = await _teamServiceFacade.GetTeamByNameAsync(teamName);

            if (!result.IsSuccess)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Team not found",
                    Detail = result.Message,
                    Status = StatusCodes.Status404NotFound
                });
            }

            return Ok(result.Data!);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An unexpected error occurred while trying to get team by name. {ex.Message}");
            return StatusCode(500, new ProblemDetails
            {
                Title = "Internal Server Error",
                Detail = "An unexpected error occured. Please try again later",
                Status = StatusCodes.Status500InternalServerError
            });
        }
    }

    [HttpPatch(Name = nameof(UpdateTeamAsync))]
    [Authorize (Roles = "Admin")]
    public async Task<IActionResult> UpdateTeamAsync([FromBody] UpdateTeamDTO updatedTeam)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);

        try
        {
            var result = await _teamServiceFacade.UpdateTeamAsync(updatedTeam);

            if (!result.IsSuccess)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Team not found",
                    Detail = result.Message,
                    Status = StatusCodes.Status404NotFound
                });
            }

            return Ok(result.Data!);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An unexpected error occurred while trying to update team. {ex.Message}");
            return StatusCode(500, new ProblemDetails
            {
                Title = "Internal Server Error",
                Detail = "An unexpected error occured. Please try again later",
                Status = StatusCodes.Status500InternalServerError
            });
        }
    }

    [HttpDelete(Name = nameof(DeleteTeamAsync))]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteTeamAsync([FromBody] DeleteTeamDTO deleteTeamDTO)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);

        try
        {
            var result = await _teamServiceFacade.DeleteTeamAsync(deleteTeamDTO);

            if (!result.IsSuccess)
            {
                return NotFound(new ProblemDetails
                {
                    Title = "Team not found",
                    Detail = result.Message,
                    Status = StatusCodes.Status404NotFound
                });
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An unexpected error occurred while trying to delete team. {ex.Message}");
            return StatusCode(500, new ProblemDetails
            {
                Title = "Internal Server Error",
                Detail = "An unexpected error occured. Please try again later",
                Status = StatusCodes.Status500InternalServerError
            });
        }
    }
}
