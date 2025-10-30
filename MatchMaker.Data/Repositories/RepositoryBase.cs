using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace MatchMaker.Data.Repositories;

public class RepositoryBase<T>(ILogger<RepositoryBase<T>> logger, IMongoDatabase database, string collectionName) where T : class
{
    protected readonly ILogger<RepositoryBase<T>> Logger = logger;
    protected readonly IMongoCollection<T> Collection = database.GetCollection<T>(collectionName);

    protected async Task HandleMongoOperationAsync(Func<Task> operation, string errorMessage, CancellationToken cancellationToken = default)
    {
        try
        {
            await operation();
        }
        catch (MongoException ex)
        {
            Logger.LogError(ex, errorMessage);
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
            Logger.LogError(ex, errorMessage);
            throw; 
        }
    }
    public async Task<T> InsertOneAsync(T document, CancellationToken cancellationToken = default)
    {
        await HandleMongoOperationAsync(() => Collection.InsertOneAsync(document, cancellationToken: cancellationToken), $"An error occurred while inserting a document into the {Collection.CollectionNamespace.CollectionName} collection.");
        return document;
    }

    public async Task<T> FindOneAsync(FilterDefinition<T> filter, CancellationToken cancellationToken = default)
    {
        return await HandleMongoOperationAsync(() => Collection.Find(filter).FirstOrDefaultAsync(cancellationToken), $"An unexpected error occured when trying to find a document in {Collection.CollectionNamespace.CollectionName}.");
    }

    public async Task<List<T>> FindAllAsync(CancellationToken cancellationToken = default)
    {
        return await HandleMongoOperationAsync(() => Collection.Find(FilterDefinition<T>.Empty).ToListAsync(cancellationToken), $"An error occurred while retrieving all documents from the {Collection.CollectionNamespace.CollectionName} collection.");
    }

    public async Task<List<T>> FindAllAsync(FilterDefinition<T> filter, CancellationToken cancellationToken = default)
    {
        return await HandleMongoOperationAsync(() => Collection.Find(filter).ToListAsync(cancellationToken), $"An error occurred while retrieving filtered documents from the {Collection.CollectionNamespace.CollectionName} collection.");
    }

    public async Task<UpdateResult> UpdateOneAsync(FilterDefinition<T> filter, UpdateDefinition<T> update, CancellationToken cancellationToken = default)
    {
        return await HandleMongoOperationAsync(() => Collection.UpdateOneAsync(filter, update, cancellationToken: cancellationToken), $"An error occurred while updating a document in the {Collection.CollectionNamespace.CollectionName} collection.");
    }

    public async Task<DeleteResult> DeleteOneAsync(FilterDefinition<T> filter, CancellationToken cancellationToken = default)
    {
        return await HandleMongoOperationAsync(() => Collection.DeleteOneAsync(filter, cancellationToken: cancellationToken), $"An error occurred while deleting a document from the {Collection.CollectionNamespace.CollectionName} collection.");
    }
}
