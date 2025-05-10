using MatchMaker.Core.Interfaces;
using MatchMaker.Domain.DTOs.Users;
using MatchMaker.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace MatchMaker.Domain.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController(ILogger<UserController> logger, IUserServiceFacade userServiceFacade) : ControllerBase
{
    private readonly ILogger<UserController> _logger = logger;
    private readonly IUserServiceFacade _userServiceFacade = userServiceFacade;

    [HttpPost]
    public async Task<IActionResult> CreateUserAsync([FromBody] CreateUserDTO newUser)
    {
        try
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            var result = await _userServiceFacade.CreateUserAsync(newUser);

            if (!result.IsSuccess)
            {
                return Conflict(new ProblemDetails
                {
                    Title = "User creation failed",
                    Detail = result.Message,
                    Status = StatusCodes.Status409Conflict
                });
            }

            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            var manualUri = $"{baseUrl}/api/User/{result.Data!.Id}";

            return Created(manualUri, result.Data.Id);
        }
        catch (FileNotFoundException ex)
        {
            _logger.LogError(ex, "An unexpected error occured while creating user.");
            return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
            {
                Title = "An unexpected error occurred",
                Detail = $"{ex.Message} An unexpected error occurred while creating the user. Please try again later.",
                Status = StatusCodes.Status500InternalServerError
            });
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

            if (!result.IsSuccess || result.Data == null) return NotFound(new ProblemDetails
            {
                Title = "User not found",
                Detail = result.Message ?? "The specified user was not found.",
                Status = StatusCodes.Status404NotFound
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

            if (!result.IsSuccess || result.Data == null) return NotFound(new ProblemDetails
            {
                Title = "User not found",
                Detail = result.Message ?? "The specified user was not found.",
                Status = StatusCodes.Status404NotFound
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
    public async Task<IActionResult> GetUsersByRole(string userRole)
    {
        try
        {
            if (string.IsNullOrEmpty(userRole)) return BadRequest();

            var result = await _userServiceFacade.GetUsersByRole(userRole);

            if (!result.IsSuccess || result.Data == null) return NotFound(new ProblemDetails
            {
                Title = "Users not found",
                Detail = result.Message ?? "Users with the specified role was not found.",
                Status = StatusCodes.Status404NotFound
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

            if (!result.IsSuccess || result.Data == null) return NotFound(new ProblemDetails()
            {
                Title = "User not found",
                Detail = result.Message ?? "The specified user was not found.",
                Status = StatusCodes.Status404NotFound
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

            if(!result.IsSuccess) return BadRequest(new ProblemDetails
            {
                Title = "Failed to delete user",
                Detail = result.Message,
                Status = StatusCodes.Status400BadRequest
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
