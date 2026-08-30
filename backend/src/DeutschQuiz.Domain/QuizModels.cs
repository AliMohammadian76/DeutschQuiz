namespace DeutschQuiz.Domain;

public enum QuizCategory
{
    Vocabulary,
    Grammar,
    Mixed
}

public enum QuestionType
{
    MultipleChoice,
    FillBlank,
    Matching,
    Essay
}

public sealed record Lesson(
    Guid Id,
    string Book,
    string Level,
    int Number,
    string Title);

public sealed record QuizQuestion(
    Guid Id,
    Guid LessonId,
    QuizCategory Category,
    QuestionType Type,
    string Prompt,
    IReadOnlyList<string> Options,
    string CorrectAnswer,
    string Explanation);
