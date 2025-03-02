using Mapster;
using MapsterMapper;
using MatchMaker.Core.Profiles;
using MatchMaker.Data.Interfaces;
using MatchMaker.Data.Repositories;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Entities;

namespace MatchMaker.Api.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddProjectServices(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
    {
        string dbName = configuration["MongoDB:DatabaseName"] ?? throw new ArgumentNullException("MongoDB:DatabaseName is missing in configuration.");
        string connectionString = configuration["MongoDb:ConnectionString"] ?? throw new ArgumentNullException("MongoDB:ConnectionString is missing in configuration.");

        DB.InitAsync(dbName, connectionString).GetAwaiter().GetResult();

        var conventionPack = new ConventionPack { new CamelCaseElementNameConvention() };
        ConventionRegistry.Register("CamelCaseConvention", conventionPack, _ => true);

        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(typeof(UserMappingProfile).Assembly);

        services.AddMapster();

        services.AddScoped<IUserRepo, UserRepo>();

        services.AddSingleton(config);

        return services;
    }
}
