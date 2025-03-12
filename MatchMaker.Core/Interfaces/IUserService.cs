using MatchMaker.Domain.DTOs;

namespace MatchMaker.Core.Interfaces;

public interface IUserService
{
    Task<Result<UserDTO>> CreateUserAsync(CreateUserDTO newUser);
    Task<Result<UserDTO>> GetUserByEmailAsync(string email);
    Task<Result<UserDTO>> GetUserByIdAsync(string userId);
    
}
