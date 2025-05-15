using Serilog;

namespace MatchMaker.Api.Extensions
{
    public static class SerilogServiceExtension
    {
        public static IHostBuilder UseSerilogLogging(this IHostBuilder hostBuilder, IConfiguration configuration)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .WriteTo.Console()
                .WriteTo.File("Logs/app-.log", rollingInterval: RollingInterval.Day)
                .Enrich.FromLogContext()
                .CreateLogger();

            return hostBuilder.UseSerilog();
        }
    }
}
