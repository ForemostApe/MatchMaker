using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace MatchMaker.Data.Repositories;

public class RepositoryBase<T>(ILogger<RepositoryBase<T>> logger, IMongoDatabase database, string collectionName) where T : class
{
    protected readonly ILogger<RepositoryBase<T>> _logger = logger;
    protected readonly IMongoCollection<T> _coillection = database.GetCollection<T>(collectionName);

    protected async Task HandleMongoOperation(Func<Task> operation, string errorMessage)
    {
        try
        {
            await operation();
        }
        catch (MongoException ex)
        {
            _logger.LogError(ex, errorMessage);
            throw;
        }
    }
}
