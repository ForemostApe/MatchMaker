using MatchMaker.Domain.DTOs;
using MatchMaker.Domain.Entities;
using MongoDB.Driver;

namespace MatchMaker.Core.Interfaces;

public interface IUserService
{
    Task<User?> CreateUserAsync(User newUser);
    Task<Result<User>> GetUserByEmailAsync(string email);
    Task<Result<User>> GetUserByIdAsync(string userId);
    Task<Result<User>> UpdateUserAsync(User updatedUser);
    Task<User?> VerifyEmailAsync(User verifiedUser);
    Task<Result<User>> DeleteUserAsync(string userId);
}
