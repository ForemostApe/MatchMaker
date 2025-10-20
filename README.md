# MatchMaker

This application started out as the final assignment in vocational-school and is now an ongoing experiment with React, .NET and MongoDb.

MatchMaker is a .NET 9 Web API that provides backend services for matchmaking and game management for little league-football. It includes authentication (JWT), role-based authorization, Swagger API documentation, MongoDB persistence, Serilog logging, rate limiting, SMTP support for outgoing mail, and static file hosting (SPA fallback). The API exposes health checks and uses a JwtMiddleware for custom token handling.

## Key features
- .NET 9 Web API using minimal-host pattern
- JWT authentication and role-based authorization
- MongoDB data storage
- Swagger / OpenAPI UI (Development)
- Serilog logging
- Rate limiting and CORS policies
- SMTP integration for email
- Session support and static files with SPA fallback (index.html)
- Health check endpoint

## Tech stack
- .NET 9
- MongoDB.Driver
- Serilog
- Swagger / Swashbuckle
- Mapster (mapping)
- SMTP (configurable)
- Rate limiting (ASP.NET Core Rate Limiting)
- Project layout: Api, Domain, Core, Data layers

## Repository
Clone the repository and open the solution in Visual Studio 2022 or use the CLI.

Example:
- Remote: https://github.com/ForemostApe/MatchMaker
- Branch: upload-images

## Prerequisites
- .NET 9 SDK
- MongoDB instance (local, container, or managed)
- Optional: SMTP server or credentials for outgoing mail (Papercut SMTP-configuration used for local testing)
- Optional: VS 2022 for debugging and the __https__ launch profile

## Configuration
The application reads configuration from:
- appsettings.json
- appsettings.{Environment}.json
- Environment variables
- In Development, user secrets via AddUserSecrets<Program>()

Important configuration sections (examples — adapt to your environment):
- ConnectionStrings: MongoDB connection string
- Jwt: Issuer, Key, Expires (minutes)
- Smtp: Host, Port, Username, Password (if sending email)
- Cors: AllowedOrigins (frontend origins)
- RateLimiting: limits and windows

Example minimal appsettings snippet:

{ 
  "Smtp": { 
  "Host": "smtp.example.com", 
  "Port": 587, 
  "User": "user@example.com", 
  "Password": "secret" 
  }, 
  "Cors": { 
  "AllowedOrigins": [ "http://localhost:5173" ] 
  } 
}

Use user secrets for development:
- dotnet user-secrets init
- dotnet user-secrets set "Jwt:Key" "your-secret"

User-secrets settings snippet:

MongoDbSettings": {
    "DatabaseName": "ExampleDataBaseName",
    "ConnectionString": "ExampleMongoDbConnectionString"
  },
  "JwtSettings": {
    "Issuer": "ExampleIssuer",
    "Audience": "ExampleAudience",
    "AccessTokenExpirationMinutes": 120,
    "RefreshTokenExpirationDays": 2,
    "SigningKey": "ExamleBase64EncodedSigningKey",
    "EncryptionKey": "ExampleBase64EncodedEncryptionKey"

## Running locally

Using Visual Studio 2022
- Open the solution.
- Select the __https__ launch profile (defined in MatchMaker.Api\Properties\launchSettings.json).
- Press F5 or use __Debug__ > Start Debugging.

Using dotnet CLI
- cd to MatchMaker.Api directory
- dotnet restore
- dotnet run
- By default launchSettings maps to https://localhost:5001 and http://localhost:5000 for Development.

Swagger UI (Development only)
- When running in Development, Swagger is available at:
  - https://localhost:5001/swagger
  - (or the configured applicationUrl set in __launchSettings.json__)

Health checks
- GET /health

Static files & SPA
- The API serves static files and falls back to index.html for unknown routes (suitable for SPA hosting).

## Authentication & Authorization
- JWT-based authentication is configured (AddJwtAuthentication).
- The code contains a JwtMiddleware to augment authentication.
- Role-based authorization is used; e.g., the GameController requires the "Coach" role for creating games.

Example protected endpoint (from GameController):
- POST /api/game (Requires role: Coach)
- GET /api/game - list games (authenticated)

Errors are returned using ProblemDetails with appropriate HTTP status codes and logging for unexpected exceptions.

## Development notes
- Service registration occurs in Program.cs (MongoDb, core services, JWT, SMTP, Swagger, rate limiting).
- Serilog is used for structured logging.
- CORS policies are registered for Development and Staging using allowed origins from configuration.
- Rate limiting is applied with app.UseRateLimiter().

## Diagnostics
- Logs are emitted via Serilog.
- Health checks route: /health
- In Development, IdentityModelEventSource.ShowPII = true is enabled to ease debugging token issues.

## License
- Check the repository root for a LICENSE file. If none, contact the maintainers to confirm licensing.

## Contact
- See repository remote: https://github.com/ForemostApe/MatchMaker
