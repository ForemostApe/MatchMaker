using MatchMaker.Domain.DTOs;

namespace MatchMaker.Core.Interfaces;

public interface IUserService
{
    Task<UserResultDTO> CreateUserAsync(CreateUserDTO newUser);
    Task<UserResultDTO> GetUserByIdAsync(string userId);
}
