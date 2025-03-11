using MatchMaker.Domain.Entities;

namespace MatchMaker.Data.Interfaces
{
    public interface IUserRepo
    {
        Task<bool> CreateUserAsync(User newUser);
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> GetUserByIdAsync(string userId);
        Task<bool> UpdateUserAsync(User updatedUser);
        Task<bool> DeleteUserAsync(string userId);
    }
}
