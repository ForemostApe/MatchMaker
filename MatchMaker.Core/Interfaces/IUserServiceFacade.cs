using MatchMaker.Domain.DTOs;
using MatchMaker.Domain.DTOs.Users;

namespace MatchMaker.Core.Interfaces;

public interface IUserServiceFacade
{
    Task<Result<UserDTO>> CreateUserAsync(CreateUserDTO newUser);
    Task<Result<UserDTO>> GetUserByEmailAsync(string email);
    Task<Result<UserDTO>> GetUserByIdAsync(string userId);
    Task<Result<UserDTO>> UpdateUserAsync(UpdateUserDTO updatedUser);
    Task<Result<bool>> DeleteUserAsync(string userId);
}
