using MatchMaker.Data.Interfaces;
using MatchMaker.Domain.Entities;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace MatchMaker.Data.Repositories;

public class UserRepo(ILogger<UserRepo> logger, IMongoDatabase database) : IUserRepo
{
    private readonly ILogger<UserRepo> _logger = logger;
    private readonly IMongoCollection<UserEntity> _userCollection = database.GetCollection<UserEntity>("users");

    public async Task<bool> CreateUserAsync(UserEntity newUser)
    {
        try
        {
            await _userCollection.InsertOneAsync(newUser);
            return true;
        }
        catch (MongoWriteException ex)
        {
            _logger.LogError(ex, "An unexpected error occured while trying to create account for {newUser}", newUser.Email);
            throw;
        }
    }

    public async Task<UserEntity?> GetUserByEmailAsync(string email)
    {
        try
        {
            var user = await _userCollection.Find(e => e.Email == email).FirstOrDefaultAsync();
            return user;
            
        }
        catch (MongoException ex)
        {
            _logger.LogError(ex, "An unexpected error occured while trying to find account for {email}", email);
            throw;
        }
    }

    public async Task<UserEntity?> GetUserByIdAsync(string userId)
    {
        try
        {
            var user = await _userCollection.Find(i => i.ID == userId).FirstOrDefaultAsync();
            return user;
        }
        catch (MongoException ex)
        {
            _logger.LogError(ex, "An unexpected error occured while trying to find account for user {id}", userId);
            throw;
        }
    }

    public async Task<bool> UpdateUserAsync(UserEntity updatedUser)
    {
        try
        {
            var filter = Builders<UserEntity>.Filter.Eq(u => u.ID, updatedUser.ID);
            var update = Builders<UserEntity>.Update
                .Set(u => u.Password, updatedUser.Password)
                .Set(u => u.Email, updatedUser.Email)
                .Set(u => u.FirstName, updatedUser.FirstName)
                .Set(u => u.LastName, updatedUser.LastName);

            var result = await _userCollection.UpdateOneAsync(filter, update);

            return result.ModifiedCount > 0;

        }
        catch (MongoWriteException ex)
        {
            _logger.LogError(ex, "An unexpected error occured while trying to update account for user {id}", updatedUser.ID);
            throw;
        }
    }

    public async Task<bool> DeleteUserAsync(string userId)
    {
        try
        {
            var result = await _userCollection.DeleteOneAsync(u => u.ID.Equals(userId));
            return result.DeletedCount > 0;
        }
        catch (MongoWriteException ex)
        {
            _logger.LogError(ex, "An unexpected error occured while trying to delete account for user {id}", userId);
            throw;
        }
    }
}
