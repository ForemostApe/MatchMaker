using MatchMaker.Api.Extensions;
using MatchMaker.Domain.Extensions;
using MatchMaker.Domain.Middlewares;
using Microsoft.IdentityModel.Logging;

namespace MatchMaker.Api
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

            #if DEBUG
                builder.WebHost.ConfigureKestrelServer();
#endif

            builder.Host.UseSerilogLogging(builder.Configuration);

            builder.Services.AddMongoDb(builder.Configuration);
            builder.Services.AddCoreServices(builder.Configuration, builder.Environment);
            builder.Services.AddJwtAuthentication(builder.Configuration);
            builder.Services.AddSmtpServices(builder.Configuration);
            builder.Services.AddSwagger();
            builder.Services.AddRateLimiting(builder.Configuration);

            var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? new[] { "http://localhost:5173" };

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("Development", policy =>
                {
                    policy.WithOrigins(allowedOrigins)
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                          .AllowCredentials()
                          .SetPreflightMaxAge(TimeSpan.FromSeconds(86400));
                });
                options.AddPolicy("Staging", policy =>
                {
                    policy.WithOrigins(allowedOrigins)
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                          .AllowCredentials()
                          .SetPreflightMaxAge(TimeSpan.FromSeconds(86400));
                });
            });

            builder.Host.UseSerilogLogging(builder.Configuration);

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "MatchMaker API v1");
                });
                IdentityModelEventSource.ShowPII = true;
            }
            else
            {
                app.UseHsts();
                app.UseExceptionHandler("/error");
            }

            app.UseHttpsRedirection();

            //Check what this actually does. Tied to Kestrel and security-headers.
            //if (app.Environment.IsDevelopment()) app.Use(async (context, next) =>
            //{
            //    context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
            //    await next();
            //});

            app.UseRateLimiter();
            app.UseStaticFiles();
            app.MapHealthChecks("/health");
            app.UseRouting();
            app.UseSession();
            app.UseCors(app.Environment.IsDevelopment() ? "Development" : "Staging");
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMiddleware<JwtMiddleware>();
            app.MapControllers();
            app.MapFallbackToFile("index.html");

            app.Run();
        }
    }
}
