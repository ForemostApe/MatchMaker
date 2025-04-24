using MatchMaker.Core.Interfaces;
using MatchMaker.Data.Interfaces;
using MatchMaker.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace MatchMaker.Core.Services;

public class UserService(ILogger<UserService> logger, IUserRepo userRepo, IAuthService authService) : IUserService
{
    private readonly ILogger<UserService> _logger = logger;
    private readonly IUserRepo _userRepo = userRepo;
    private readonly IAuthService _authService = authService;

    public async Task<User?> CreateUserAsync(User newUser)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(newUser);

            var existingUser = await _userRepo.GetUserByEmailAsync(newUser.Email);
            if (existingUser != null)
            {
                _logger.LogWarning("User already exists.");
                return null;
            }

            newUser.PasswordHash = _authService.HashPassword(newUser.PasswordHash);
            
            await _userRepo.CreateUserAsync(newUser);

            return newUser;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred in the business-logic when creating new user {Email}.", newUser.Email);
            throw new Exception($"An unexpected error occurred in the business-logic when creating new user {newUser.Email}.", ex);
        }
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(email);

            var user = await _userRepo.GetUserByEmailAsync(email);
            if (user == null)
            {
                _logger.LogWarning($"User with email {email} couldn't be found.");
                return null;
            }

            return user;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred in the business-logic when trying to find user by email {Email}.", email);
            throw new Exception($"An unexpected error occurred in the business-logic when trying to find user by email {email}.", ex);
        }
    }

    public async Task<User?> GetUserByIdAsync(string userId)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(userId);

            var user = await _userRepo.GetUserByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning($"User with id {userId} couldn't be found.");
                return null;
            }

            return user;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred in the business-logic when trying to find user by Id {Id}.", userId);
            throw new Exception($"An unexpected error occurred in the business-logic when trying to find user by Id {userId}.", ex);
        }
    }

    public async Task<User?> UpdateUserAsync(User updatedUser)
    {
        ArgumentNullException.ThrowIfNull(updatedUser);

        await _userRepo.UpdateUserAsync(updatedUser);

        return updatedUser;
    }
     

    public async Task<User?> VerifyEmailAsync(User verifiedUser)
    {
        ArgumentNullException.ThrowIfNull(verifiedUser);

        await _userRepo.VerifyEmailAsync(verifiedUser);

        return verifiedUser;
    }

    public async Task<bool> DeleteUserAsync(string userId)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(userId);

            await _userRepo.DeleteUserAsync(userId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred in the business-logic when trying to delete user {Id}.", userId);
            throw new Exception($"An unexpected error occurred in the business-logic when trying to delete user {userId}.", ex);
        }
    }
}