using MatchMaker.Data.Interfaces;
using MatchMaker.Domain.Entities;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MatchMaker.Data.Repositories;

public class UserRepo(ILogger<UserRepo> logger, IMongoDatabase database) : RepositoryBase<User>(logger, database, "users"), IUserRepo
{
    public async Task<User> CreateUserAsync(User newUser)
    {
        ArgumentNullException.ThrowIfNull(newUser);

        try
        {
            return await InsertOneAsync(newUser);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occured in the repo-class trying to create user.");
            throw;
        }
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        ArgumentException.ThrowIfNullOrEmpty(email);

        try
        {
            var filter = Builders<User>.Filter.Regex(u => u.Email, new MongoDB.Bson.BsonRegularExpression($"^{email}$", "i"));
            return await FindOneAsync(filter);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occured in the repo-class trying to get user by email-address.");
            throw;
        }
    }

    public async Task<User?> GetUserByIdAsync(string userId)
    {
        ArgumentException.ThrowIfNullOrEmpty(userId);

        try
        {
            var filter = Builders<User>.Filter.Eq(u => u.Id, userId);
            return await FindOneAsync(filter);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occured in the repo-class trying to get user by UserId.");
            throw;
        }
    }

    public async Task<List<User>> GetUsersByRole(UserRole parsedRole, string? teamId = null)
    {
        ArgumentException.ThrowIfNullOrEmpty(parsedRole.ToString());

        try
        {
            var filters = new List<FilterDefinition<User>>
            {
                Builders<User>.Filter.Eq(u => u.UserRole, parsedRole)
            };
            if (!string.IsNullOrEmpty(teamId))
            {
                filters.Add(Builders<User>.Filter.Eq("TeamAffiliation", new ObjectId(teamId)));
            }

            var combinedFilter = Builders<User>.Filter.And(filters);
            return await FindAllAsync(combinedFilter);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occured in the repo-class trying to get user by UserRole");
            throw;
        }
    }

    public async Task<UpdateResult> UpdateUserAsync(User updatedUser)
    {
        ArgumentNullException.ThrowIfNull(updatedUser);

        try
        {
            var filter = Builders<User>.Filter.Eq(u => u.Id, updatedUser.Id);
            var update = Builders<User>.Update
                .Set(u => u.Email, updatedUser.Email)
                .Set(u => u.FirstName, updatedUser.FirstName)
                .Set(u => u.LastName, updatedUser.LastName)
                .Set(u => u.TeamAffiliation, updatedUser.TeamAffiliation)
                .Set(u => u.UserRole, updatedUser.UserRole)
                .Set(u => u.IsVerified, updatedUser.IsVerified);

            return await UpdateOneAsync(filter, update);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occured in the repo-class trying to update user.");
            throw;
        }
    }

    public async Task<UpdateResult> VerifyEmailAsync(User verifiedUser)
    {
        ArgumentNullException.ThrowIfNull(verifiedUser);

        try
        {
            var filter = Builders<User>.Filter.Eq(u => u.Id, verifiedUser.Id);
            var update = Builders<User>.Update
                .Set(u => u.IsVerified, verifiedUser.IsVerified);

            return await UpdateOneAsync(filter, update);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occured in the repo-class trying to verify user-email.");
            throw;
        }
    }

    public async Task<DeleteResult> DeleteUserAsync(string userId)
    {
        ArgumentNullException.ThrowIfNull(userId);

        try
        {
            var filter = Builders<User>.Filter.Eq(u => u.Id, userId);
            return await DeleteOneAsync(filter);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occured in the repo-class trying to delete user.");
            throw;
        }
    }
}
