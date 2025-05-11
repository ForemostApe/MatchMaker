using MatchMaker.Domain.DTOs;
using MatchMaker.Domain.Entities;

namespace MatchMaker.Core.Interfaces;

public interface IUserService
{
    Task<Result<User>> CreateUserAsync(User newUser);
    Task<Result<User>> GetUserByEmailAsync(string email);
    Task<Result<User>> GetUserByIdAsync(string userId);
    Task<Result<List<User>>> GetUsersByRoleAsync(UserRole parsedRole, string? teamId = null);
    Task<Result<User>> UpdateUserAsync(User updatedUser);
    Task<Result<User>> VerifyEmailAsync(User verifiedUser);
    Task<Result<User>> DeleteUserAsync(string userId);
}
