using DeutschQuiz.Application;
using DeutschQuiz.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DeutschQuiz.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DeutschQuiz");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                "The PostgreSQL connection string is missing. Set ConnectionStrings__DeutschQuiz.");
        }

        services.AddDbContext<Persistence.QuizDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<IQuizService, EfQuizService>();
        services.AddSingleton<JwtTokenService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IProgressService, ProgressService>();

        return services;
    }

    public static IServiceCollection AddUnavailableAccountServices(
        this IServiceCollection services)
    {
        services.AddSingleton<IAuthService, UnavailableAuthService>();
        services.AddSingleton<IProgressService, UnavailableProgressService>();
        return services;
    }
}
