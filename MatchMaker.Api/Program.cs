using Mapster;
using MatchMaker.Api.Extensions;

namespace MatchMaker.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddProjectServices(builder.Configuration, builder.Environment);

            var app = builder.Build();
            
            app.Run();
        }
    }
}
