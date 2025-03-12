using MatchMaker.Api.Configurations;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using MongoDB.Entities;

namespace MatchMaker.Api.Extensions;

public static class MongoDbServiceCollectionExtensions
{
    public static IServiceCollection AddMongoDb(this IServiceCollection services, IConfiguration configuration)
    {
        var mongoDbSettings = configuration.GetSection("MongoDbSettings").Get<MongoDbSettings>() ?? throw new Exception("MongoDbSettings configuration is missing.");


        services.AddSingleton<IMongoClient>(serviceProvider =>
        {
            MongoUrl mongoUrl = new MongoUrl(mongoDbSettings.ConnectionString) ?? throw new Exception("Connection-string missing.");

            return new MongoClient(mongoUrl);
        });

        services.AddSingleton(serviceProvider =>
        {
            var client = serviceProvider.GetRequiredService<IMongoClient>();
            var database = client.GetDatabase(mongoDbSettings.DatabaseName) ?? throw new Exception("Database-name missing.");

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

