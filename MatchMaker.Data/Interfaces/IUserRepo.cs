using MatchMaker.Domain.Entities;

namespace MatchMaker.Data.Interfaces
{
    public interface IUserRepo
    {
        Task<bool> CreateUserAsync(UserEntity newUser);
        Task<UserEntity?> GetUserByEmailAsync(string email);
        Task<UserEntity?> GetUserByIdAsync(string userId);
        Task<bool> UpdateUserAsync(UserEntity updatedUser);
        Task<bool> DeleteUserAsync(string userId);
    }
}
