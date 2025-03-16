using MatchMaker.Data.Interfaces;
using MatchMaker.Domain.Entities;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace MatchMaker.Data.Repositories;

public class UserRepo(ILogger<UserRepo> logger, IMongoDatabase database) : RepositoryBase<User>(logger, database, "users"), IUserRepo
{
    public async Task CreateUserAsync(User newUser)
    {
        await InsertOneAsync(newUser);
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        var filter = Builders<User>.Filter.Eq(u => u.Email, email);
        return await FindOneAsync(filter);
    }

    public async Task<User?> GetUserByIdAsync(string userId)
    {
        var filter = Builders<User>.Filter.Eq(u => u.ID, userId);
        return await FindOneAsync(filter);
    }

    public async Task UpdateUserAsync(User updatedUser)
    {
        var filter = Builders<User>.Filter.Eq(u => u.ID, updatedUser.ID);
        var update = Builders<User>.Update
            .Set(u => u.PasswordHash, updatedUser.PasswordHash)
            .Set(u => u.Email, updatedUser.Email)
            .Set(u => u.FirstName, updatedUser.FirstName)
            .Set(u => u.LastName, updatedUser.LastName)
            .Set(u => u.UserRole, updatedUser.UserRole);

        await UpdateOneAsync(filter, update);
    }

    public async Task DeleteUserAsync(string userId)
    {
        var filter = Builders<User>.Filter.Eq(u => u.ID, userId);
        await DeleteOneAsync(filter);
    }
}
