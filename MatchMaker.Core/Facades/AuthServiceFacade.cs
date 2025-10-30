using MapsterMapper;
using MatchMaker.Core.Helpers;
using MatchMaker.Core.Interfaces;
using MatchMaker.Core.Utilities;
using MatchMaker.Domain.DTOs.Authentication;
using MatchMaker.Domain.DTOs.Users;
using MatchMaker.Domain.Entities;
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

        public async Task<Result<bool>> VerifyEmailAsync(string verificationToken)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(verificationToken))
                    return Result<bool>.Failure("Token får inte vara tom.");

                var principal = _tokenService.DecryptToken(verificationToken);
                if (principal == null)
                    return Result<bool>.Failure("Ogiltig eller utgången verifikationstoken");

                TokenClaims claims = TokenClaimHelper.CheckAndExtractClaims(principal);

                if (claims.TokenUsage != "verification")
                {
                    _logger.LogError("Provided token was not of the correct type.");
                    return Result<bool>.Failure("Ett oväntat fel uppstod. Vänligen försök igen senare.");
                }

                var getUserResult = await _userService.GetUserByIdAsync(claims.UserId);
                if (!getUserResult.IsSuccess)
                    return Result<bool>.Failure(getUserResult.Message);

                var existingUser = getUserResult.Data!;

                if (!existingUser.Email.Equals(claims.UserEmail, StringComparison.OrdinalIgnoreCase))
                {
                    _logger.LogError("User-email doesn't match the email provided in token.");
                    return Result<bool>.Failure("Ett oväntat fel uppstod. Vänligen försök igen senare.");
                }

                if (existingUser.IsVerified)
                    return Result<bool>.Success(true, "Email-adressen är redan verifierad.");

                existingUser.IsVerified = true;
                var updateResult = await _userService.VerifyEmailAsync(existingUser);
                return Result<bool>.Success(true, "Email-adressen har verifierats.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during email-verification.");
                throw new ApplicationException("Ett oväntat fel uppstod. Vänligen försök igen senare.");
            }
        }

        public async Task<Result<AuthenticationDto>> LoginAsync(LoginDto loginDto)
        {
            try
            {
                var userResult = await _userService.GetUserByEmailAsync(loginDto.Email);

                if (!userResult.IsSuccess || userResult.Data == null) 
                    return Result<AuthenticationDto>.Failure("Felaktig email-adress eller felaktigt lösenord.");

                var user = userResult.Data;

                if (!BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash)) 
                    return Result<AuthenticationDto>.Failure("Felaktig email-adress eller felaktigt lösenord.");

                if (!user.IsVerified) 
                    return Result<AuthenticationDto>.Failure("Vänligen verifiera din email-adress för att kunna logga in.");

                _sessionManager.ClearSession("refreshToken");

                var accessToken = await _tokenService.GenerateAccessToken(user);
                var refreshToken = await _tokenService.GenerateRefreshToken(user);
                _cookieFactory.CreateHttpOnlyCookie("refreshToken", refreshToken);

                AuthenticationDto result = new ()
                {
                    AccessToken = accessToken,
                    User = _mapper.Map<AuthenticatedUserDto>(user)
                };

                return Result<AuthenticationDto>.Success(result, "Du är nu inloggad.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during login.");
                throw new ApplicationException("Ett oväntat fel uppstod. Vänligen försök igen senare.");
            }
        }

        public async Task<Result<AuthenticationDto>> GenerateRefreshTokenAsync(string refreshToken)
        {
            try
            {
                var user = await _tokenService.ValidateRefreshToken(refreshToken) ?? throw new UnauthorizedAccessException("Ogiltig refresh-token.");

                var newAccessToken = await _tokenService.GenerateAccessToken(user);
                var newRefreshToken = await _tokenService.GenerateRefreshToken(user);
                _cookieFactory.CreateHttpOnlyCookie("refreshToken", newRefreshToken);

                AuthenticationDto result = new AuthenticationDto()
                {
                    AccessToken = newAccessToken,
                    User = _mapper.Map<AuthenticatedUserDto>(user)
                };

                return Result<AuthenticationDto>.Success(result, "Token förnyades och användaren är åter autentiserad.");

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
