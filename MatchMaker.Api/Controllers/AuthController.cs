using MatchMaker.Core.Interfaces;
using MatchMaker.Domain.DTOs.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace MatchMaker.Domain.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(ILogger<AuthController> logger, IAuthServiceFacade authServiceFacade) : ControllerBase
    {
        private readonly ILogger<AuthController> _logger = logger;
        private readonly IAuthServiceFacade _authServiceFacade = authServiceFacade;

        [IgnoreAntiforgeryToken]
        [EnableRateLimiting("EmailVerificationPolicy")]
        [HttpPut("verify-email")]
        public async Task<IActionResult> VerifyEmailAsync(string verificationToken)
        {
            try
            {
                if (string.IsNullOrEmpty(verificationToken))
                {
                    return BadRequest();
                }

                var result = await _authServiceFacade.VerifyEmailAsync(verificationToken);
                if (!result.IsSuccess)
                {
                    return BadRequest(result);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while trying to verify user-email.");
                return StatusCode(500, new ProblemDetails
                {
                    Title = "Internal Server Error",
                    Detail = "An unexpected error occured. Please try again later",
                    Status = StatusCodes.Status500InternalServerError
                });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(LoginDTO loginDTO)
        {
            try
            {
                if (!ModelState.IsValid) return ValidationProblem(ModelState);

                var result = await _authServiceFacade.LoginAsync(loginDTO);
                if (result.Data == null)
                {
                    return Unauthorized(result);
                }

                Response.Headers.Append("Authorization", "Bearer " + result.Data!.AccessToken);
                return Ok(result);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while trying to log in user.");
                return StatusCode(500, new ProblemDetails
                {
                    Title = "Internal Server Error",
                    Detail = "An unexpected error occured. Please try again later",
                    Status = StatusCodes.Status500InternalServerError
                });
            }
        }

        [HttpPost("logout")]
        [Authorize]
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
                return StatusCode(500, new ProblemDetails
                {
                    Title = "Internal Server Error",
                    Detail = "An unexpected error occured. Please try again later",
                    Status = StatusCodes.Status500InternalServerError
                });
            }
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshTokens()
        {
            try
            {
                var refreshToken = Request.Cookies["refreshToken"];
                if (string.IsNullOrEmpty(refreshToken))
                {
                    _logger.LogWarning("Refresh-token is missing.");
                    return Unauthorized(new { Message = "Refresh-token required." });
                }

                var result = await _authServiceFacade.GenerateRefreshTokenAsync(refreshToken);

                if (!result.IsSuccess)
                {
                    return Unauthorized(new { result.Message });
                }

                return Ok(result.Data);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Invalid refresh token.");
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error refreshing token.");
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