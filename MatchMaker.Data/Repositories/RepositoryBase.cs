using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace MatchMaker.Data.Repositories;

public class RepositoryBase<T>(ILogger<RepositoryBase<T>> logger, IMongoDatabase database, string collectionName) where T : class
{
    protected readonly ILogger<RepositoryBase<T>> _logger = logger;
    protected readonly IMongoCollection<T> _collection = database.GetCollection<T>(collectionName);

    protected async Task HandleMongoOperationAsync(Func<Task> operation, string errorMessage, CancellationToken cancellationToken = default)
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
    protected async Task<TResult> HandleMongoOperationAsync<TResult>(Func<Task<TResult>> operation, string errorMessage, CancellationToken cancellationToken = default)
    {
        try
        {
            return await operation();
        }
        catch (MongoException ex)
        {
            _logger.LogError(ex, errorMessage);
            throw; 
        }
    }
    public async Task InsertOneAsync(T document, CancellationToken cancellationToken = default)
    {
        await HandleMongoOperationAsync(() => _collection.InsertOneAsync(document, cancellationToken: cancellationToken), $"An error occurred while inserting a document into the {_collection.CollectionNamespace.CollectionName} collection.");
    }

    public async Task<T> FindOneAsync(FilterDefinition<T> filter, CancellationToken cancellationToken = default)
    {
        return await HandleMongoOperationAsync(() => _collection.Find(filter).FirstOrDefaultAsync(cancellationToken), $"An unexpected error occured when trying to find a document in {_collection.CollectionNamespace.CollectionName}.");
    }

    public async Task UpdateOneAsync(FilterDefinition<T> filter, UpdateDefinition<T> update, CancellationToken cancellationToken = default)
    {
        await HandleMongoOperationAsync(() => _collection.UpdateOneAsync(filter, update, cancellationToken: cancellationToken), $"An error occurred while updating a document in the {_collection.CollectionNamespace.CollectionName} collection.");
    }

    public async Task DeleteOneAsync(FilterDefinition<T> filter, CancellationToken cancellationToken = default)
    {
        await HandleMongoOperationAsync(() => _collection.DeleteOneAsync(filter, cancellationToken: cancellationToken), $"An error occurred while deleting a document from the {_collection.CollectionNamespace.CollectionName} collection.");
    }
}
