using MatchMaker.Core.Utilities;
using MatchMaker.Domain.DTOs.Users;

namespace MatchMaker.Core.Interfaces;

public interface IUserServiceFacade
{
    Task<Result<UserDto>> CreateUserAsync(CreateUserDto newUser);
    Task<Result<UserDto>> GetUserByEmailAsync(string email);
    Task<Result<UserDto>> GetUserByIdAsync(string userId);
    Task<Result<List<UserDto>>> GetUsersByRole(string userRole, string? teamId = null);
    Task<Result<UserDto>> UpdateUserAsync(UpdateUserDto updatedUser);
    Task<Result<UserDto>> DeleteUserAsync(string userId);
}
