using MatchMaker.Core.Interfaces;
using MatchMaker.Data.Interfaces;
using MatchMaker.Domain.DTOs;
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

    public async Task<Result<User>> GetUserByEmailAsync(string email)
    {
        ArgumentNullException.ThrowIfNull(email);

        try
        {
            var user = await _userRepo.GetUserByEmailAsync(email);
            if (user == null)
            {
                return Result<User>.Failure($"Couldn't find user with email-address {email}");
            }

            return Result<User>.Success(user, "User successfully found.");
        }
        catch
        {
            throw;
        }
    }

    public async Task<Result<User>> GetUserByIdAsync(string userId)
    {
        ArgumentNullException.ThrowIfNull(userId);

        try
        {
            var user = await _userRepo.GetUserByIdAsync(userId);
            if (user == null) return Result<User>.Failure("Failed to find user.");

            return Result<User>.Success(user, "User successfully found.");
        }
        catch
        {
            throw;
        }
    }

    public async Task<Result<User>> UpdateUserAsync(User updatedUser)
    {
        ArgumentNullException.ThrowIfNull(updatedUser);

        try
        {
            updatedUser.PasswordHash = _authService.HashPassword(updatedUser.PasswordHash);

            var result = await _userRepo.UpdateUserAsync(updatedUser);

            if (result.ModifiedCount <= 0) return Result<User>.Failure("An error occurred trying to update user.");

            return Result<User>.Success(updatedUser, "User successully updated.");
        }
        catch 
        {
            throw;
        }
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