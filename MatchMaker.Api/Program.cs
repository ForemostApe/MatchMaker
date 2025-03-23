using MatchMaker.Api.Extensions;
using MatchMaker.Api.Middlewares;

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

            builder.Services.AddMongoDb(builder.Configuration);
            builder.Services.AddCoreServices(builder.Configuration);
            builder.Services.AddJwtAuthentication(builder.Configuration);
            builder.Services.AddSmtpServices(builder.Configuration, builder.Environment);

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseSwagger();
                //app.UseSwaggerUI();
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
            app.MapHealthChecks("/health");
            app.MapControllers();

            app.Run();
        }
    }
}
