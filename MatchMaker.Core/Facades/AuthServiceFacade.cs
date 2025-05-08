using MapsterMapper;
using MatchMaker.Core.Interfaces;
using MatchMaker.Domain.DTOs;
using MatchMaker.Domain.DTOs.Authentication;
using MatchMaker.Domain.DTOs.Users;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace MatchMaker.Core.Facades
{
    public class AuthServiceFacade(ILogger<AuthServiceFacade> logger, ICookieFactory cookieFactory, IUserService userService, ITokenService tokenService, ISessionManager sessionManager, IMapper mapper) : IAuthServiceFacade
    {
        private readonly ILogger<AuthServiceFacade> _logger = logger;
        private readonly ICookieFactory _cookieFactory = cookieFactory;
        private readonly IUserService _userService = userService;
        private readonly ITokenService _tokenService = tokenService;
        private readonly ISessionManager _sessionManager = sessionManager;
        private readonly IMapper _mapper = mapper;

        public async Task<Result<bool>> VerifyEmailAsync(string token)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(token))
                {
                    return Result<bool>.Failure("Token cannot be empty");
                }

                var principal = _tokenService.DecryptToken(token);
                if (principal == null)
                {
                    return Result<bool>.Failure("Invalid or expired verification token");
                }

                var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new UnauthorizedAccessException($"User-Id claim is missing.");
                var userEmail = principal.FindFirst(ClaimTypes.Email)?.Value ?? throw new UnauthorizedAccessException($"User-email claim is missing.");
                var tokenUsage = principal.FindFirst("token_usage")?.Value ?? throw new UnauthorizedAccessException($"Token-claim is missing.");

                if (tokenUsage != "verification")
                {
                    return Result<bool>.Failure("This token is not for email-verification");
                }

                var user = await _userService.GetUserByIdAsync(userId);
                if (!user.IsSuccess)
                {
                    return Result<bool>.Failure(user.Message);
                }

                if (user.Data!.Email.ToLower() != userEmail.ToLower())
                {
                    return Result<bool>.Failure("Email does not match verification token");
                }

                if (user.Data!.IsVerified)
                {
                    return Result<bool>.Success(true, "Email already verified");
                }

                user.Data!.IsVerified = true;

                var updateResult = await _userService.VerifyEmailAsync(user.Data!);

                return Result<bool>.Success(true, "Email successfully verified");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during email-verification.");
                throw new ApplicationException("An unexpected error occurred. Please try again later.");
            }
        }

        public async Task<Result<AuthenticationDTO>> LoginAsync(LoginDTO loginDTO)
        {
            try
            {
                _logger.LogInformation("Attempting to login user with email {email}", loginDTO.Email);

                var user = await _userService.GetUserByEmailAsync(loginDTO.Email);

                if (user == null)
                {
                    _logger.LogError("User not found.");
                    return Result<AuthenticationDTO>.Failure("Invalid email-address or password.");
                }

                if (user.Data == null || !BCrypt.Net.BCrypt.Verify(loginDTO.Password, user.Data!.PasswordHash))
                {
                    _logger.LogError("Invalid email-address or password.");

                    return Result<AuthenticationDTO>.Failure("Invalid email-address or password.");
                }

                var accessToken = await _tokenService.GenerateAccessToken(user.Data!);
                var refreshToken = await _tokenService.GenerateRefreshToken(user.Data);
                _cookieFactory.CreateHttpOnlyCookie("refreshToken", refreshToken);
                _logger.LogInformation("Token created: {token}", accessToken);

                AuthenticationDTO result = new AuthenticationDTO()
                {
                    AccessToken = accessToken,
                    User = _mapper.Map<AuthenticatedUserDTO>(user)
                };

                return Result<AuthenticationDTO>.Success(result, "User successfully authenticated.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during login.");
                throw new ApplicationException("An unexpected error occurred. Please try again later.");
            }
        }

        public async Task<Result<AuthenticationDTO>> GenerateRefreshTokenAsync(string refreshToken)
        {
            try
            {
                var user = await _tokenService.ValidateRefreshToken(refreshToken);

                if (user == null) throw new UnauthorizedAccessException("Invalid refresh token.");

                var newAccessToken = await _tokenService.GenerateAccessToken(user);
                var newRefreshToken = await _tokenService.GenerateRefreshToken(user);
                _cookieFactory.CreateHttpOnlyCookie("refreshToken", newRefreshToken);

                AuthenticationDTO result = new AuthenticationDTO()
                {
                    AccessToken = newAccessToken,
                    User = _mapper.Map<AuthenticatedUserDTO>(user)
                };

                return Result<AuthenticationDTO>.Success(result, "User successfully reauthenticated.");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error refreshing token.");
                throw new ApplicationException("An unexpected error occurred trying to refresh token. Please try again later.");
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
