using DeutschQuiz.Domain;

namespace DeutschQuiz.Api.Contracts;

public sealed record QuizQuestionResponse(
    Guid Id,
    Guid LessonId,
    QuizCategory Category,
    QuestionType Type,
    string Prompt,
    IReadOnlyList<string> Options);
