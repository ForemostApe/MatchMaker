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
                    return Result<bool>.Failure("Token får inte vara tom.");
                }

                var principal = _tokenService.DecryptToken(token);
                if (principal == null)
                {
                    return Result<bool>.Failure("Ogiltig eller utgången verifikationstoken");
                }

                var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new UnauthorizedAccessException($"User-Id claim saknas.");
                var userEmail = principal.FindFirst(ClaimTypes.Email)?.Value ?? throw new UnauthorizedAccessException($"User-email claim saknas.");
                var tokenUsage = principal.FindFirst("token_usage")?.Value ?? throw new UnauthorizedAccessException($"Token-claim is saknas.");

                if (tokenUsage != "verification")
                {
                    return Result<bool>.Failure("Tillhandhahållen token är inte avsedd för email-verifikation.");
                }

                var user = await _userService.GetUserByIdAsync(userId);
                if (!user.IsSuccess)
                {
                    return Result<bool>.Failure(user.Message);
                }

                if (user.Data!.Email.ToLower() != userEmail.ToLower())
                {
                    return Result<bool>.Failure("Email-adressen matchar inte email-adressen i tillhandahållen token.");
                }

                if (user.Data!.IsVerified)
                {
                    return Result<bool>.Success(true, "Email-adressen är redan verifierad.");
                }

                user.Data!.IsVerified = true;

                var updateResult = await _userService.VerifyEmailAsync(user.Data!);

                return Result<bool>.Success(true, "Email-adressen har verifierats.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during email-verification.");
                throw new ApplicationException("Ett oväntat fel uppstod. Vänligen försök igen senare.");
            }
        }

        public async Task<Result<AuthenticationDTO>> LoginAsync(LoginDTO loginDTO)
        {
            try
            {
                var userResult = await _userService.GetUserByEmailAsync(loginDTO.Email);

                if (!userResult.IsSuccess || userResult.Data == null) return Result<AuthenticationDTO>.Failure("Felaktig email-adress eller felaktigt lösenord.");

                var user = userResult.Data;

                if (!BCrypt.Net.BCrypt.Verify(loginDTO.Password, user.PasswordHash)) return Result<AuthenticationDTO>.Failure("Felaktig email-adress eller felaktigt lösenord.");

                if (!user.IsVerified) return Result<AuthenticationDTO>.Failure("Vänligen verifiera din email-adress för att kunna logga in.");

                _sessionManager.ClearSession("refreshToken");

                var accessToken = await _tokenService.GenerateAccessToken(user);
                var refreshToken = await _tokenService.GenerateRefreshToken(user);
                _cookieFactory.CreateHttpOnlyCookie("refreshToken", refreshToken);

                AuthenticationDTO result = new AuthenticationDTO()
                {
                    AccessToken = accessToken,
                    User = _mapper.Map<AuthenticatedUserDTO>(user)
                };

                return Result<AuthenticationDTO>.Success(result, "Du är nu inloggad.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during login.");
                throw new ApplicationException("Ett oväntat fel uppstod. Vänligen försök igen senare.");
            }
        }

        public async Task<Result<AuthenticationDTO>> GenerateRefreshTokenAsync(string refreshToken)
        {
            try
            {
                var user = await _tokenService.ValidateRefreshToken(refreshToken) ?? throw new UnauthorizedAccessException("Ogiltig refresh-token.");

                var newAccessToken = await _tokenService.GenerateAccessToken(user);
                var newRefreshToken = await _tokenService.GenerateRefreshToken(user);
                _cookieFactory.CreateHttpOnlyCookie("refreshToken", newRefreshToken);

                AuthenticationDTO result = new AuthenticationDTO()
                {
                    AccessToken = newAccessToken,
                    User = _mapper.Map<AuthenticatedUserDTO>(user)
                };

                return Result<AuthenticationDTO>.Success(result, "Token förnyades och användaren är åter autentiserad.");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error refreshing token.");
                throw new ApplicationException("Ett fel uppstod vid förnyelse av token. Vänligen försök igen senare.");
            }
        }

        public Result<string> Logout()
        {
            try
            {
                _sessionManager.ClearSession("refreshToken");

                return Result<string>.Success("Du har loggats ut.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Couldn't delete cookie.");
                throw new InvalidOperationException("Ett fel uppstod vid utloggning.");
            }
        }
    }
}
