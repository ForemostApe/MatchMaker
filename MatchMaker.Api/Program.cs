using MatchMaker.Api.Configurations;
using MatchMaker.Api.Extensions;
using MongoDB.Driver.Core.Configuration;
using MongoDB.Driver;

namespace MatchMaker.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddMongoDb(builder.Configuration);

            builder.Services.AddProjectServices(builder.Configuration, builder.Environment);

            var app = builder.Build();

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
