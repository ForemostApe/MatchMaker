using MatchMaker.Domain.Entities;

namespace MatchMaker.Data.Interfaces
{
    public interface IUserRepo
    {
        Task CreateUserAsync(User newUser);
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> GetUserByIdAsync(string userId);
        Task UpdateUserAsync(User updatedUser);
        Task VerifyEmailAsync(User verifiedUser);
        Task DeleteUserAsync(string userId);
    }
}
