using MatchMaker.Core.Interfaces;
using MatchMaker.Core.Services;
using MatchMaker.Core.Utilities;

namespace MatchMaker.Api.Extensions
{
    public static class FileManagementExtension
    {
        public static IServiceCollection AddFileManagement(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<FileValidationOptions>(configuration.GetSection("LogoUploadRestrictions"));

            services.AddScoped<IFileValidationService, FileValidationService>();
            services.AddScoped<IFileStorageService, FileStorageService>();

            return services;        
        }
    }
}