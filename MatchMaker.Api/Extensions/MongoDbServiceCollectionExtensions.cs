using MatchMaker.Api.Configurations;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using MongoDB.Entities;

namespace MatchMaker.Api.Extensions;

public static class MongoDbServiceCollectionExtensions
{
    public static IServiceCollection AddMongoDb(this IServiceCollection services, IConfiguration configuration)
    {
        var mongoDbSettings = configuration.GetSection("MongoDbSettings").Get<MongoDbSettings>();

        services.AddSingleton<IMongoClient>(serviceProvider =>
        {
            var mongoUrl = new MongoUrl(mongoDbSettings.ConnectionString);
            return new MongoClient(mongoUrl);
        });

        services.AddSingleton(serviceProvider =>
        {
            var client = serviceProvider.GetRequiredService<IMongoClient>();
            var database = client.GetDatabase(mongoDbSettings.DatabaseName);

            return database;
        });

        RegisterConventions();

        return services;
    }

    private static void RegisterConventions()
    {
        var conventionPack = new ConventionPack
        {
            new CamelCaseElementNameConvention()
        };

        ConventionRegistry.Register("CamelCaseConvention",
            conventionPack, _ => true
            );
    }
}

