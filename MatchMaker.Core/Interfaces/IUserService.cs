using MatchMaker.Domain.DTOs;

namespace MatchMaker.Core.Interfaces;

public interface IUserService
{
    Task<UserResultDTO> CreateUserAsync(UserDTO newUser);
}
