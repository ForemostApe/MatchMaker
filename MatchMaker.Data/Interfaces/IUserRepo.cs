using MatchMaker.Domain.Entities;

namespace MatchMaker.Data.Interfaces
{
    public interface IUserRepo
    {
        Task CreateUserAsync(User newUser);
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> GetUserByIdAsync(string userId);
        Task UpdateUserAsync(User updatedUser);
        Task DeleteUserAsync(string userId);
    }
}
