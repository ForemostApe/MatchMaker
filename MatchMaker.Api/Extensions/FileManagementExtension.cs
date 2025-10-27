using MatchMaker.Core.Interfaces;
using MatchMaker.Core.Services;
using MatchMaker.Core.Utilities;

namespace MatchMaker.Api.Extensions;

public class FileManagementExtension
{
    public static IServiceCollection AddCoreServices
    (
        IServiceCollection services, 
        IConfiguration configuration
    )

    {
        services.Configure<FileValidationOptions>(configuration.GetSection("LogoUploadRestrictions"));

        services.AddScoped<IFileValidationService, FileValidationService>();
        
        return services;
    }
}