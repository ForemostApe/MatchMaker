using MatchMaker.Domain.Entities;

namespace MatchMaker.Core.Interfaces;

public interface IUserService
{
    Task<User?> CreateUserAsync(User newUser);
    Task<User?> GetUserByEmailAsync(string email);
    Task<User?> GetUserByIdAsync(string userId);
    Task<User?> UpdateUserAsync(User updatedUser);
    Task<User?> VerifyEmailAsync(User verifiedUser);
    Task<bool> DeleteUserAsync(string userId);
}
