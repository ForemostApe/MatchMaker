using MatchMaker.Core.Interfaces;
using MatchMaker.Domain.DTOs.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MatchMaker.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UserController(ILogger<UserController> logger, IUserServiceFacade userServiceFacade) : ControllerBase
{
    private readonly ILogger<UserController> _logger = logger;
    private readonly IUserServiceFacade _userServiceFacade = userServiceFacade;

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> CreateUserAsync([FromBody] CreateUserDTO newUser)
    {
        try
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var result = await _userServiceFacade.CreateUserAsync(newUser);

            if (!result.IsSuccess) return StatusCode(result.GetStatusCodeOrDefault(), new ProblemDetails()
            {
                Title = result.Title,
                Detail = result.Message,
                Status = result.StatusCode
            });

            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            var manualUri = $"{baseUrl}/api/User/{result.Data!.Id}";

            return Created(manualUri, result.Data.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occured while creating user.");
            return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
            {
                Title = "An unexpected error occurred",
                Detail = "An unexpected error occurred while creating the user. Please try again later.",
                Status = StatusCodes.Status500InternalServerError
            });
        }
    }

    [HttpGet("email/{email}")]
    public async Task<IActionResult> GetUserByEmailAsync(string email)
    {
        try
        {
            if (string.IsNullOrEmpty(email)) return BadRequest(new ProblemDetails()
            {
                Title = "Invalid input.",
                Detail = "Email cannot be empty.",
                Status = StatusCodes.Status400BadRequest
            });

            var result = await _userServiceFacade.GetUserByEmailAsync(email);

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
            _logger.LogError(ex, "An unexpected error occured while trying to get user with email {Email}.", email);
            return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
            {
                Title = "An unexpected error occurred",
                Detail = "An unexpected error occurred while trying to fetch the user. Please try again later.",
                Status = StatusCodes.Status500InternalServerError
            });
        }
    }

    [HttpGet("id/{userId}")]
    public async Task<IActionResult> GetUserByIdAsync(string userId)
    {
        try
        {
            if (string.IsNullOrEmpty(userId)) return BadRequest();

            var result = await _userServiceFacade.GetUserByIdAsync(userId);

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
            _logger.LogError(ex, "An unexpected error occured while trying to get user {UserId}.", userId);
            return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
            {
                Title = "An unexpected error occurred",
                Detail = "An unexpected error occurred while trying to fetch the user. Please try again later.",
                Status = StatusCodes.Status500InternalServerError
            });
        }
    }

    [HttpGet("role/{userRole}")]
    public async Task<IActionResult> GetUsersByRole(string userRole, string? teamId = null)
    {
        try
        {
            if (string.IsNullOrEmpty(userRole)) return BadRequest();

            var result = await _userServiceFacade.GetUsersByRole(userRole, teamId);

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
            _logger.LogError(ex, "An unexpected error occured while trying to get users with the role {userRole}.", userRole);
            return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
            {
                Title = "An unexpected error occurred",
                Detail = "An unexpected error occurred while trying to fetch the user. Please try again later.",
                Status = StatusCodes.Status500InternalServerError
            });
        }
    }

    [HttpPatch]
    public async Task<IActionResult> UpdateUserAsync([FromBody] UpdateUserDTO updatedUser)
    {
        try
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var result = await _userServiceFacade.UpdateUserAsync(updatedUser);

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
            _logger.LogError(ex, "An unexpected error occured while trying to update user {UserId}.", updatedUser.Id);
            return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
            {
                Title = "An unexpected error occurred",
                Detail = "An unexpected error occurred while trying to update the user. Please try again later.",
                Status = StatusCodes.Status500InternalServerError
            });
        }
    }

    [HttpDelete("{userId}")]
    public async Task<IActionResult> DeleteUserAsync(string userId)
    {
        try
        {
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest(new ProblemDetails
                {
                    Title = "Invalid input",
                    Detail = "User ID cannot be null or empty.",
                    Status = StatusCodes.Status400BadRequest
                });
            }

            var result = await _userServiceFacade.DeleteUserAsync(userId);

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
            _logger.LogError(ex, "An unexpected error occured while trying to delete user {UserId}.", userId);
            return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
            {
                Title = "An unexpected error occurred",
                Detail = "An unexpected error occurred while trying to delete the user. Please try again later.",
                Status = StatusCodes.Status500InternalServerError
            });
        }
    }
}
