using MatchMaker.Core.Interfaces;
using MatchMaker.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace MatchMaker.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(ILogger<AuthController> logger, IAuthServiceFacade authServiceFacade) : ControllerBase
    {
        private readonly ILogger<AuthController> _logger = logger;
        private readonly IAuthServiceFacade _authServiceFacade = authServiceFacade;

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(LoginDTO loginDTO)
        {
            try
            {
                if (!ModelState.IsValid) return ValidationProblem(ModelState);

                var loginResult = await _authServiceFacade.LoginAsync(loginDTO);
                if (loginResult.Data == null)
                {
                    _logger.LogWarning("Invalid email-address or password.");
                    return Unauthorized(loginResult);
                }

                _logger.LogInformation("User successfully logged in.");
                Response.Headers.Append("Authorization", "Bearer" + loginResult.Data!.AccessToken);
                return Ok(loginResult);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while trying to log in user.");
                return StatusCode(500, "An error occured while trying to log in user.");
            }
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            try
            {
                var result = _authServiceFacade.Logout();
                if (result == null)
                {
                    _logger.LogInformation("Failed logging user out.");
                    return BadRequest();
                }

                _logger.LogInformation("Successfully logged user out.");
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occured when trying to delete session-cookie.");
                return StatusCode(500, "An unexpected error occured.");
            }
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            try
            {
                var refreshToken = Request.Cookies["refreshToken"];
                if (string.IsNullOrEmpty(refreshToken))
                {
                    _logger.LogWarning("Refresh-token is missing.");
                    return Unauthorized("Refresh-token is missing.");
                }

                var newAccessToken = await _authServiceFacade.RefreshTokenAsync(refreshToken);
                return Ok();
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Invalid refresh token.");
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error refreshing token.");
                return StatusCode(500, "An error occurred while refreshing the token.");
            }
        }
    }
}