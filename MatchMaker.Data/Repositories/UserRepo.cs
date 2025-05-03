using MatchMaker.Data.Interfaces;
using MatchMaker.Domain.Entities;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace MatchMaker.Data.Repositories;

public class UserRepo(ILogger<UserRepo> logger, IMongoDatabase database) : RepositoryBase<User>(logger, database, "users"), IUserRepo
{
    public async Task CreateUserAsync(User newUser)
    {
        try
        {
            await InsertOneAsync(newUser);
        }
        catch
        {
            throw;
        }
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        try
        {
            var filter = Builders<User>.Filter.Eq(u => u.Email, email);
            return await FindOneAsync(filter);
        }
        catch
        {
            throw;
        }
    }

    public async Task<User?> GetUserByIdAsync(string userId)
    {
        try
        {
            var filter = Builders<User>.Filter.Eq(u => u.Id, userId);
            return await FindOneAsync(filter);
        }
        catch
        {
            throw;
        }
    }

    public async Task<UpdateResult> UpdateUserAsync(User updatedUser)
    {
        try
        {
            var filter = Builders<User>.Filter.Eq(u => u.Id, updatedUser.Id);
            var update = Builders<User>.Update
                .Set(u => u.PasswordHash, updatedUser.PasswordHash)
                .Set(u => u.Email, updatedUser.Email)
                .Set(u => u.FirstName, updatedUser.FirstName)
                .Set(u => u.LastName, updatedUser.LastName)
                .Set(u => u.UserRole, updatedUser.UserRole)
                .Set(u => u.IsVerified, updatedUser.IsVerified);

            return await UpdateOneAsync(filter, update);
        }
        catch
        {
            throw;
        }
    }

    public async Task VerifyEmailAsync(User verifiedUser)
    {
        try
        {
            var filter = Builders<User>.Filter.Eq(u => u.Id, verifiedUser.Id);
            var update = Builders<User>.Update
                .Set(u => u.IsVerified, verifiedUser.IsVerified);

            await UpdateOneAsync(filter, update);
        }
        catch
        {
            throw;
        }
    }

    public async Task<DeleteResult> DeleteUserAsync(string userId)
    {
        try
        {
            var filter = Builders<User>.Filter.Eq(u => u.Id, userId);
            return await DeleteOneAsync(filter);
        }
        catch
        {
            throw;
        }
    }
}
