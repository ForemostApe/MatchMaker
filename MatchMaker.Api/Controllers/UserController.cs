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
                return BadRequest(ModelState);
            }

            var result = await _userService.CreateUserAsync(newUser);
            if (result.IsSuccess)
            {
                var baseUrl = $"{Request.Scheme}://{Request.Host}";
                var manualUri = $"{baseUrl}/api/User/{result.userDTO!.UserId}";

                _logger.LogInformation("User successfully created with user-Id: {result.User.UserId}", result.userDTO!.UserId);

                return Created(manualUri, result.userDTO.UserId);
            }
            else
            {
                _logger.LogWarning("User with user-Id: {result.User.UserId} couldn't be found.", result.userDTO!.UserId);
                return Conflict(result.Message);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occured while creating user.");
            return StatusCode(500, "An error occurred while creating user.");
        }
    }
}
