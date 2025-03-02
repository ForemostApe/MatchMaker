using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Entities;

namespace MatchMaker.Api.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddProjectServices(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
    {
        string dbName = configuration["MongoDB:DatabaseName"] ?? throw new ArgumentNullException("MongoDB:DatabaseName is missing in configuration");

        DB.InitAsync(dbName);

        var conventionPack = new ConventionPack { new CamelCaseElementNameConvention() };
        ConventionRegistry.Register("CamelCaseConvention", conventionPack, _ => true);

        return services;
    }
}
