using MatchMaker.Domain.DTOs;
using MatchMaker.Domain.Entities;

namespace MatchMaker.Core.Interfaces;

public interface IUserService
{
    Task<Result<User>> CreateUserAsync(User newUser);
    Task<Result<User>> GetUserByEmailAsync(string email);
    Task<Result<User>> GetUserByIdAsync(string userId);
    Task<Result<User>> UpdateUserAsync(User updatedUser);
    Task<Result<bool>> DeleteUserAsync(string userId);
}
