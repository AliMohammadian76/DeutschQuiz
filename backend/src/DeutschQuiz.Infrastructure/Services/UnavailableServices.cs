using DeutschQuiz.Application;

namespace DeutschQuiz.Infrastructure.Services;

public sealed class UnavailableAuthService : IAuthService
{
    public Task<AuthResult> RegisterAsync(
        RegisterUserRequest request,
        CancellationToken cancellationToken = default) =>
        throw new NotSupportedException(
            "Account features require a configured PostgreSQL database and JWT signing key.");

    public Task<AuthResult?> LoginAsync(
        LoginRequest request,
        CancellationToken cancellationToken = default) =>
        throw new NotSupportedException(
            "Account features require a configured PostgreSQL database and JWT signing key.");
}

public sealed class UnavailableProgressService : IProgressService
{
    public Task<AttemptResult?> SubmitAsync(
        Guid userId,
        SubmitAttemptRequest request,
        CancellationToken cancellationToken = default) =>
        throw new NotSupportedException(
            "Progress features require a configured PostgreSQL database and JWT signing key.");

    public Task<ProgressSummary> GetSummaryAsync(
        Guid userId,
        CancellationToken cancellationToken = default) =>
        throw new NotSupportedException(
            "Progress features require a configured PostgreSQL database and JWT signing key.");

    public Task<IReadOnlyList<AttemptHistoryItem>> GetHistoryAsync(
        Guid userId,
        int limit = 20,
        CancellationToken cancellationToken = default) =>
        throw new NotSupportedException(
            "Progress features require a configured PostgreSQL database and JWT signing key.");
}
