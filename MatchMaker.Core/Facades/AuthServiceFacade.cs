using MatchMaker.Core.Interfaces;
using MatchMaker.Domain.DTOs;
using Microsoft.Extensions.Logging;

namespace MatchMaker.Core.Facades
{
    public class AuthServiceFacade(ILogger<AuthServiceFacade> logger, ICookieFactory cookieFactory, IUserService userService, ITokenService tokenService, ISessionManager sessionManager) : IAuthServiceFacade
    {
        private readonly ILogger<AuthServiceFacade> _logger = logger;
        private readonly ICookieFactory _cookieFactory = cookieFactory;
        private readonly IUserService _userService = userService;
        private readonly ITokenService _tokenService = tokenService;
        private readonly ISessionManager _sessionManager = sessionManager;

        public async Task<Result<AuthenticationDTO>> LoginAsync(LoginDTO loginDTO)
        {
            try
            {
                _logger.LogInformation("Attempting to login user with email {email}", loginDTO.Email);

                var user = await _userService.GetUserByEmailAsync(loginDTO.Email);
                if (user == null || !BCrypt.Net.BCrypt.Verify(loginDTO.Password, user.Data!.PasswordHash))
                {
                    _logger.LogError("Invalid email-address or password.");

                    return Result<AuthenticationDTO>.Failure("Invalid email-address or password.");
                }

                var accessToken = await _tokenService.GenerateAccessToken(user.Data!);
                var refreshToken = await _tokenService.GenerateAccessToken(user.Data!);
                _cookieFactory.CreateHttpOnlyCookie("refreshToken", refreshToken);
                _logger.LogInformation("Token created: {token}", accessToken);

                AuthenticationDTO token = new AuthenticationDTO()
                {
                    AccessToken = accessToken
                };

                return Result<AuthenticationDTO>.Success(token, "User successfully authenticated.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during login.");
                throw new ApplicationException("An unexpected error occurred. Please try again later.");
            }
        }

        public async Task<bool> RefreshTokenAsync(string refreshToken)
        {
            try
            {
                var user = await _tokenService.ValidateRefreshToken(refreshToken);

                if (user == null)
                {
                    _logger.LogWarning("Invalid refresh token.");
                    throw new UnauthorizedAccessException("Invalid refresh token.");
                }
                    
                var newAccessToken = await _tokenService.GenerateAccessToken(user);
                _logger.LogInformation("Successfully generated new access token for user-ID {userId}", user.Id);

                return true;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error refreshing token.");
                return false;
            }
        }

        public Result<string> Logout()
        {
            try
            {
                _cookieFactory.ExpireCookie();
                _sessionManager.ClearSession();

                return Result<string>.Success("Cookie successfully deleted");
            }
            catch (Exception ex)
            {
                _logger.LogError("Couldn't delete cookie.");
                throw new InvalidOperationException("An error occured while trying to logout user.", ex);
            }
        }
    }
}
