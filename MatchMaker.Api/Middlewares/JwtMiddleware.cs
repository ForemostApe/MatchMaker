using MatchMaker.Core.Services;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MatchMaker.Api.Middlewares;

public class JwtMiddleware(ILogger<JwtMiddleware> logger, RequestDelegate next, JwtOptions jwtOptions)
{
    private readonly ILogger<JwtMiddleware> _logger = logger;
    private readonly RequestDelegate _next = next;
    private readonly JwtOptions _jwtOptions = jwtOptions;

    public async Task Invoke(HttpContext context)
    {
        var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();

        if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
        {
            try
            {
                var token = authHeader.Substring(7);

                var principal = ValidateToken(token);

                if (principal != null)
                {
                    context.User = principal;
                }
                else
                {
                    _logger.LogWarning("JWT validation failed. Setting response to 401.");
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Unauthorized: Invalid token.");
                    return;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "JWT validation failed.");
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Unauthorized: Invalid token.");
                return;
            }
        }

        await _next(context);
    }

    private ClaimsPrincipal? ValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuer = _jwtOptions.Issuer,
            ValidAudience = _jwtOptions.Audience,
            IssuerSigningKey = _jwtOptions.SigningKey,
            ValidateIssuerSigningKey = true
        };

        try
        {
            return tokenHandler.ValidateToken(token, validationParameters, out _);
        }
        catch (SecurityTokenException ex)
        {
            _logger.LogWarning(ex, "JWT validation failed: SecurityTokenException.");
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while validating JWT.");
            return null;
        }
    }
}