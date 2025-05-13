using MatchMaker.Domain.Entities;
using MongoDB.Driver;

namespace MatchMaker.Data.Interfaces
{
    public interface IUserRepo
    {
        Task<User> CreateUserAsync(User newUser);
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> GetUserByIdAsync(string userId);
        Task<List<User>> GetUsersByRole(UserRole parsedRole, string? teamId = null);
        Task<UpdateResult> UpdateUserAsync(User updatedUser);
        Task<UpdateResult> VerifyEmailAsync(User verifiedUser);
        Task<DeleteResult> DeleteUserAsync(string userId);
    }
}
