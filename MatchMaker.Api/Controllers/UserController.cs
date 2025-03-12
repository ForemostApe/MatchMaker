using MatchMaker.Core.Interfaces;
using MatchMaker.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace MatchMaker.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController(ILogger<UserController> logger, IUserService userService) : ControllerBase
{
    private readonly ILogger<UserController> _logger = logger;
    private readonly IUserService _userService = userService;

    [HttpPost]
    public async Task<IActionResult> CreateUserAsync(CreateUserDTO newUser)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            var result = await _userService.CreateUserAsync(newUser);

            if (result.IsSuccess)
            {
                var baseUrl = $"{Request.Scheme}://{Request.Host}";
                var manualUri = $"{baseUrl}/api/User/{result.Data!.UserId}";

                _logger.LogInformation("User successfully created with user-Id: {result.User.UserId}", result.Data!.UserId);

                return Created(manualUri, result.Data.UserId);
            }
            else
            {
                _logger.LogWarning("User with user-Id: {result.User.UserId} couldn't be found.", result.Data!.UserId);
                return Conflict(new ProblemDetails
                {
                    Title = "User creation failed",
                    Detail = result.Message,
                    Status = StatusCodes.Status409Conflict
                });
            }
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

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetUserByIdAsync(string userId)
    {
        try
        {
            if (string.IsNullOrEmpty(userId)) return BadRequest();

            var result = await _userService.GetUserByIdAsync(userId);
        
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
}
