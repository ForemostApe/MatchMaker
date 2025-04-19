using MatchMaker.Api.Extensions;
using MatchMaker.Domain.Extensions;
using MatchMaker.Domain.Middlewares;
using Microsoft.IdentityModel.Logging;

namespace MatchMaker.Domain
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Host.UseDefaultServiceProvider((context, options) =>
            {
                options.ValidateOnBuild = context.HostingEnvironment.IsDevelopment();
                options.ValidateOnBuild = true;
            });

            builder.Services.AddMongoDb(builder.Configuration);
            builder.Services.AddCoreServices(builder.Configuration, builder.Environment);
            builder.Services.AddJwtAuthentication(builder.Configuration);
            builder.Services.AddSmtpServices(builder.Configuration);
            builder.Services.AddSwagger();
            builder.Services.AddRateLimiting(builder.Configuration);


            #if DEBUG
                IdentityModelEventSource.ShowPII = true;
            #endif

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "MatchMaker API v1");
                });
            }
            else
            {
                app.UseExceptionHandler("/error"); 
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseCors();
            app.UseSession();
            app.UseMiddleware<JwtMiddleware>();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapHealthChecks("/health"); //Check what this does.
            app.MapControllers();
            app.UseRateLimiter();

            app.Run();
        }
    }
}
