using MatchMaker.Core.Interfaces;
using MatchMaker.Core.Utilities;
using MatchMaker.Data.Interfaces;
using MatchMaker.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace MatchMaker.Core.Services;

public class UserService(ILogger<UserService> logger, IUserRepo userRepo) : IUserService
{
    private readonly ILogger<UserService> _logger = logger;
    private readonly IUserRepo _userRepo = userRepo;

    public async Task<Result<User>> CreateUserAsync(User newUser)
    {
        ArgumentNullException.ThrowIfNull(newUser);

        try
        {
            var result = await _userRepo.CreateUserAsync(newUser);
            return result != null
                ? Result<User>.Success(result, "User successfully created")
                : Result<User>.Failure("Couldn't create user");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occured in the service-layer trying to create user.");
            throw;
        }
    }

    public async Task<Result<User>> GetUserByEmailAsync(string email)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(email);

        try { 
            var user = await _userRepo.GetUserByEmailAsync(email);
            return user != null 
                ? Result<User>.Success(user, "User successfully found.") 
                : Result<User>.Failure($"Couldn't find user with email-address {email}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occured in the service-layer trying to find user by email-address.");
            throw;
        }
}

    public async Task<Result<User>> GetUserByIdAsync(string userId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(userId);

        try 
        {
            var user = await _userRepo.GetUserByIdAsync(userId);
            return user != null
                ? Result<User>.Success(user, "User successfully found.")
                : Result<User>.Failure("Failed to find user.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occured in the service-layer trying to find user by UserId.");
            throw;
        }
    }

    public async Task<Result<List<User>>> GetUsersByRoleAsync(UserRole parsedRole, string? teamId = null)
    {
        try
        {
            var users = await _userRepo.GetUsersByRole(parsedRole, teamId);
            return users.Count > 0
                ? Result<List<User>>.Success(users, $"Users with role {parsedRole} successfully found.")
                : Result<List<User>>.Failure($"Failed to find any users with the role {parsedRole}" + (teamId != null ? $" in team {teamId}" : ""));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occured in the service-layer trying to find user by UserRole.");
            throw;
        }
    }

    public async Task<Result<User>> UpdateUserAsync(User updatedUser)
    {
        ArgumentNullException.ThrowIfNull(updatedUser);

        try 
        { 
            var result = await _userRepo.UpdateUserAsync(updatedUser);

            if (result.MatchedCount > 0 && result.ModifiedCount == 0) return Result<User>.Success(updatedUser, "User already updated.");
        
            return result.ModifiedCount > 0 
                ? Result<User>.Success(updatedUser, "User successully updated.") 
                : Result<User>.Failure("An error occurred trying to update user.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occured in the service-layer trying to find update user.");
            throw;
        }
    }
     

    public async Task<Result<User>> VerifyEmailAsync(User verifiedUser)
    {
        ArgumentNullException.ThrowIfNull(verifiedUser);

        try
        {
            var result = await _userRepo.VerifyEmailAsync(verifiedUser);
            return result.ModifiedCount > 0
                ? Result<User>.Success(verifiedUser, "Email-address successfully verified.")
                : Result<User>.Failure("Failed to verify email-address.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occured in the service-layer trying to find verify user-email.");
            throw;
        }
    }

    public async Task<Result<User>> DeleteUserAsync(string userId)
    {
        ArgumentNullException.ThrowIfNull(userId);

        try
        {
            var result = await _userRepo.DeleteUserAsync(userId);
            return result.DeletedCount > 0
                ? Result<User>.Success(null, "Successfully deleted user.")
                : Result<User>.Failure("Failed to delete user.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occured in the service-layer trying to delete user.");
            throw;
        }
    }
}