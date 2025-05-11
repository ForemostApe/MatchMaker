using MatchMaker.Data.Interfaces;
using MatchMaker.Domain.Entities;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace MatchMaker.Data.Repositories;

public class UserRepo(ILogger<UserRepo> logger, IMongoDatabase database) : RepositoryBase<User>(logger, database, "users"), IUserRepo
{
    public async Task<User> CreateUserAsync(User newUser)
    {
        try
        {
            return await InsertOneAsync(newUser);
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

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        try
        {
            var filter = Builders<User>.Filter.Regex(u => u.Email, new MongoDB.Bson.BsonRegularExpression($"^{email}$", "i"));
            return await FindOneAsync(filter);
        }
        catch
        {
            throw;
        }
    }

    public async Task<List<User>> GetUsersByRole(UserRole parsedRole, string? teamId = null)
    {
        try
        {
            var filters = new List<FilterDefinition<User>>
            {
                Builders<User>.Filter.Eq(u => u.UserRole, parsedRole)
            };

            if (!string.IsNullOrEmpty(teamId))
            {
                filters.Add(Builders<User>.Filter.Eq(u => u.TeamAffiliation, teamId));
            }

            var combinedFilter = Builders<User>.Filter.And(filters);

            return await FindAllAsync(combinedFilter);
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
                .Set(u => u.Email, updatedUser.Email)
                .Set(u => u.FirstName, updatedUser.FirstName)
                .Set(u => u.LastName, updatedUser.LastName)
                .Set(u => u.TeamAffiliation, updatedUser.TeamAffiliation)
                .Set(u => u.UserRole, updatedUser.UserRole)
                .Set(u => u.IsVerified, updatedUser.IsVerified);

            return await UpdateOneAsync(filter, update);
        }
        catch
        {
            throw;
        }
    }

    public async Task<UpdateResult> VerifyEmailAsync(User verifiedUser)
    {
        try
        {
            var filter = Builders<User>.Filter.Eq(u => u.Id, verifiedUser.Id);
            var update = Builders<User>.Update
                .Set(u => u.IsVerified, verifiedUser.IsVerified);

            return await UpdateOneAsync(filter, update);
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
