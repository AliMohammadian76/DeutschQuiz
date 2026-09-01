using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DeutschQuiz.Infrastructure.Persistence;

public sealed class QuizDbContextFactory : IDesignTimeDbContextFactory<QuizDbContext>
{
    public QuizDbContext CreateDbContext(string[] args)
    {
        var connectionString =
            Environment.GetEnvironmentVariable("ConnectionStrings__DeutschQuiz") ??
            "Host=localhost;Port=5432;Database=deutschquiz;Username=deutschquiz";

        var options = new DbContextOptionsBuilder<QuizDbContext>()
            .UseNpgsql(connectionString)
            .Options;

        return new QuizDbContext(options);
    }
}
