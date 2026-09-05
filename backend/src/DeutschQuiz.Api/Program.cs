using DeutschQuiz.Application;
using DeutschQuiz.Infrastructure;
using DeutschQuiz.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json.Serialization;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

var connectionString = builder.Configuration.GetConnectionString("DeutschQuiz");
var databaseEnabled = !string.IsNullOrWhiteSpace(connectionString);
var jwtSigningKey = builder.Configuration["Jwt:SigningKey"];
var authenticationEnabled = databaseEnabled && !string.IsNullOrWhiteSpace(jwtSigningKey);

if (databaseEnabled && (string.IsNullOrWhiteSpace(jwtSigningKey) || jwtSigningKey.Length < 32))
{
    throw new InvalidOperationException(
        "Jwt:SigningKey must be configured with at least 32 characters when PostgreSQL is enabled.");
}

if (databaseEnabled)
{
    builder.Services.AddInfrastructure(builder.Configuration);
}
else
{
    builder.Services.AddSingleton<IQuizService, InMemoryQuizService>();
    builder.Services.AddUnavailableAccountServices();
}

if (authenticationEnabled)
{
    var signingKey = jwtSigningKey!;

    builder.Services
        .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(signingKey)),
                ValidateIssuer = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"] ?? "DeutschQuiz.Api",
                ValidateAudience = true,
                ValidAudience = builder.Configuration["Jwt:Audience"] ?? "DeutschQuiz.Web",
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromSeconds(30)
            };
        });
}

builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy.WithOrigins(
                "http://localhost:5173",
                "https://localhost:5173",
                "http://localhost:3000",
                "https://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

if (databaseEnabled)
{
    await using var scope = app.Services.CreateAsyncScope();
    var db = scope.ServiceProvider.GetRequiredService<QuizDbContext>();
    await QuizDbSeeder.SeedAsync(db);
}

app.UseHttpsRedirection();
app.UseCors("Frontend");

if (authenticationEnabled)
{
    app.UseAuthentication();
    app.UseAuthorization();
}

app.MapControllers();

app.Run();
