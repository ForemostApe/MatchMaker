using Mapster;
using MatchMaker.Api.Configurations;
using MatchMaker.Core.Interfaces;
using MatchMaker.Core.Profiles;
using MatchMaker.Core.Services;
using MatchMaker.Data.Interfaces;
using MatchMaker.Data.Repositories;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using MongoDB.Entities;

namespace MatchMaker.Api.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddProjectServices(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
    {



        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(typeof(UserMappingProfile).Assembly);

        services.AddMapster();

        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IUserRepo, UserRepo>();

        services.AddSingleton(config);

        services.AddControllers();
        services.AddEndpointsApiExplorer();


        return services;
    }
}
