using MatchMaker.Domain.Entities;
using MongoDB.Driver;

namespace MatchMaker.Data.Interfaces
{
    public interface IUserRepo
    {
        Task CreateUserAsync(User newUser);
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> GetUserByIdAsync(string userId);
        Task<UpdateResult> UpdateUserAsync(User updatedUser);
        Task VerifyEmailAsync(User verifiedUser);
        Task<DeleteResult> DeleteUserAsync(string userId);
    }
}
