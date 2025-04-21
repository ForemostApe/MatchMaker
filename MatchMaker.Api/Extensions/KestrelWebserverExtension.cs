using Microsoft.AspNetCore.Server.Kestrel.Core;
using System.Net;

namespace MatchMaker.Api.Extensions;

public static class KestrelWebserverExtension
{
    public static IWebHostBuilder ConfigureKestrelServer(this IWebHostBuilder webBuilder)
    {
        return webBuilder.ConfigureKestrel(serverOptions =>
        {
            serverOptions.ConfigureEndpointDefaults(listenOptions =>
            {
                listenOptions.Protocols = HttpProtocols.Http1AndHttp2;
            });

            serverOptions.Listen(IPAddress.Loopback, 5000);
            serverOptions.Listen(IPAddress.Loopback, 5001, options =>
            {
                options.UseHttps();
            });

            serverOptions.AddServerHeader = false;
            serverOptions.AllowResponseHeaderCompression = true;
            serverOptions.Limits.MaxRequestBodySize = 10 * 1024 * 1024;
            serverOptions.ConfigureHttpsDefaults(httpsOptions =>
            {
                httpsOptions.SslProtocols = System.Security.Authentication.SslProtocols.Tls12 | System.Security.Authentication.SslProtocols.Tls13;
            });
        });
    }
}
