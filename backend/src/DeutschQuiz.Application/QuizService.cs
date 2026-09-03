using DeutschQuiz.Domain;

namespace DeutschQuiz.Application;

public interface IQuizService
{
    Task<IReadOnlyList<Lesson>> GetLessonsAsync(
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<QuizQuestion>> GetQuestionsAsync(
        Guid lessonId,
        QuizCategory? category = null,
        CancellationToken cancellationToken = default);
}

public sealed class InMemoryQuizService : IQuizService
{
    public Task<IReadOnlyList<Lesson>> GetLessonsAsync(
        CancellationToken cancellationToken = default) =>
        Task.FromResult(QuizContentCatalog.GetLessons());

    public Task<IReadOnlyList<QuizQuestion>> GetQuestionsAsync(
        Guid lessonId,
        QuizCategory? category = null,
        CancellationToken cancellationToken = default) =>
        Task.FromResult(QuizContentCatalog.GetQuestions(lessonId, category));
}

public sealed record RegisterUserRequest(
    string Email,
    string Password,
    string DisplayName);

public sealed record LoginRequest(
    string Email,
    string Password);

public sealed record AuthUser(
    Guid Id,
    string Email,
    string DisplayName);

public sealed record AuthResult(
    string AccessToken,
    DateTime ExpiresAtUtc,
    AuthUser User);

public interface IAuthService
{
    Task<AuthResult> RegisterAsync(
        RegisterUserRequest request,
        CancellationToken cancellationToken = default);

    Task<AuthResult?> LoginAsync(
        LoginRequest request,
        CancellationToken cancellationToken = default);
}

public sealed record SubmitAnswerRequest(
    Guid QuestionId,
    string SelectedAnswer,
    int ResponseTimeMs);

public sealed record SubmitAttemptRequest(
    Guid LessonId,
    QuizCategory Category,
    DateTime? StartedAtUtc,
    IReadOnlyList<SubmitAnswerRequest> Answers);

public sealed record AttemptResult(
    Guid AttemptId,
    int TotalQuestions,
    int CorrectAnswers,
    decimal Score,
    int TotalTimeMs,
    DateTime CompletedAtUtc,
    IReadOnlyList<AttemptAnswerResult> Answers);

public sealed record AttemptAnswerResult(
    Guid QuestionId,
    string Prompt,
    string SelectedAnswer,
    string CorrectAnswer,
    bool IsCorrect,
    string Explanation,
    int ResponseTimeMs);

public sealed record ProgressSummary(
    int AttemptsCount,
    decimal AverageScore,
    int BestScore,
    int TotalQuestionsAnswered,
    int TotalCorrectAnswers,
    int TotalTimeMs,
    IReadOnlyList<ProgressLessonSummary> Lessons);

public sealed record ProgressLessonSummary(
    Guid LessonId,
    string Book,
    string Level,
    int LessonNumber,
    string Title,
    int AttemptsCount,
    decimal AverageScore,
    int BestScore,
    int TotalQuestionsAnswered,
    int TotalCorrectAnswers,
    int TotalTimeMs,
    DateTime? LastAttemptAtUtc);

public sealed record AttemptHistoryItem(
    Guid AttemptId,
    Guid LessonId,
    string Book,
    string Level,
    int LessonNumber,
    string Title,
    QuizCategory Category,
    int TotalQuestions,
    int CorrectAnswers,
    decimal Score,
    int TotalTimeMs,
    DateTime? CompletedAtUtc);

public interface IProgressService
{
    Task<AttemptResult?> SubmitAsync(
        Guid userId,
        SubmitAttemptRequest request,
        CancellationToken cancellationToken = default);

    Task<ProgressSummary> GetSummaryAsync(
        Guid userId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AttemptHistoryItem>> GetHistoryAsync(
        Guid userId,
        int limit = 20,
        CancellationToken cancellationToken = default);
}
