using MatchMaker.Domain.Configurations;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Configuration;
using System.Threading.RateLimiting;

namespace MatchMaker.Api.Extensions
{
    public static class RateLimitingServiceExtension
    {
        public static IServiceCollection AddRateLimiting(this IServiceCollection services, IConfiguration configuration)
        {
            var settings = configuration.GetSection("RateLimiting").Get<RateLimitSettings>() ?? throw new ArgumentNullException(nameof(configuration), "Can't bind settings for rate-limiting.");

            services.AddRateLimiter(options =>
            {
                options.AddFixedWindowLimiter("EmailVerificationPolicy", options =>
                {
                    options.PermitLimit = settings.PermitLimit;
                    options.Window = TimeSpan.FromSeconds(settings.WindowInSeconds);
                    options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                    options.QueueLimit = settings.QueueLimit;
                });

                options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
                    RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: context.User.Identity?.Name ?? context.Request.Headers.Host.ToString(),
                        factory: _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 100,
                            Window = TimeSpan.FromMinutes(1)
                        }));
            });

            return services;
        }
    }
}
