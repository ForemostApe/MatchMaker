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
        ArgumentNullException.ThrowIfNull(newUser);

        try
        {
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
        catch
        {
            throw;
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

        try
        {
            await _userRepo.VerifyEmailAsync(verifiedUser);

            return verifiedUser;
        }
        catch
        {
            throw;
        }
    }

    public async Task<Result<User>> DeleteUserAsync(string userId)
    {
        ArgumentNullException.ThrowIfNull(userId);

        try
        {
            var result = await _userRepo.DeleteUserAsync(userId);

            if (result.DeletedCount <= 0) return Result<User>.Failure($"Failed to delete user.");

            return Result<User>.Success(null, "Successfully deleted user.");
        }
        catch
        {
            throw;
        }
    }
}