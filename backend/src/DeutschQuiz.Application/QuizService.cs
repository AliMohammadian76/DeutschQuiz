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
    private static readonly Guid LessonId =
        Guid.Parse("11111111-1111-1111-1111-111111111111");

    private static readonly Lesson Lesson = new(
        LessonId,
        "Menschen",
        "A1.1",
        1,
        "Hallo! Ich bin ...");

    private static readonly IReadOnlyList<QuizQuestion> Questions =
    [
        new(
            Guid.Parse("20000000-0000-0000-0000-000000000001"),
            LessonId,
            QuizCategory.Vocabulary,
            QuestionType.MultipleChoice,
            "Wie geht es dir?",
            ["Wie geht's?", "Wo wohnst du?", "Wie heißt du?"],
            "Wie geht's?",
            "Diese Frage fragt nach dem Befinden."),
        new(
            Guid.Parse("20000000-0000-0000-0000-000000000002"),
            LessonId,
            QuizCategory.Vocabulary,
            QuestionType.MultipleChoice,
            "Ergänze: Ich ___ Ali.",
            ["bin", "bist", "sind"],
            "bin",
            "Mit ich verwenden wir die Form bin."),
        new(
            Guid.Parse("20000000-0000-0000-0000-000000000003"),
            LessonId,
            QuizCategory.Grammar,
            QuestionType.MultipleChoice,
            "___ heißt du?",
            ["Wie", "Wo", "Was"],
            "Wie",
            "Die richtige Frage lautet: Wie heißt du?"),
        new(
            Guid.Parse("20000000-0000-0000-0000-000000000004"),
            LessonId,
            QuizCategory.Grammar,
            QuestionType.MultipleChoice,
            "Ich ___ aus dem Iran.",
            ["komme", "kommst", "kommen"],
            "komme",
            "Die Form von kommen für ich ist komme.")
    ];

    public Task<IReadOnlyList<Lesson>> GetLessonsAsync(
        CancellationToken cancellationToken = default) =>
        Task.FromResult<IReadOnlyList<Lesson>>([Lesson]);

    public Task<IReadOnlyList<QuizQuestion>> GetQuestionsAsync(
        Guid lessonId,
        QuizCategory? category = null,
        CancellationToken cancellationToken = default) =>
        Task.FromResult<IReadOnlyList<QuizQuestion>>(
            Questions
                .Where(question => question.LessonId == lessonId &&
                    (category is null ||
                     category == QuizCategory.Mixed ||
                     question.Category == category))
                .ToList());
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
    DateTime CompletedAtUtc);

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
    DateTime? LastAttemptAtUtc);

public interface IProgressService
{
    Task<AttemptResult?> SubmitAsync(
        Guid userId,
        SubmitAttemptRequest request,
        CancellationToken cancellationToken = default);

    Task<ProgressSummary> GetSummaryAsync(
        Guid userId,
        CancellationToken cancellationToken = default);
}
