using DeutschQuiz.Domain;

namespace DeutschQuiz.Application;

public interface IQuizService
{
    IReadOnlyList<Lesson> GetLessons();
    IReadOnlyList<QuizQuestion> GetQuestions(Guid lessonId, QuizCategory? category = null);
}

public sealed class InMemoryQuizService : IQuizService
{
    private static readonly Guid LessonId = Guid.Parse("11111111-1111-1111-1111-111111111111");

    private static readonly Lesson Lesson = new(
        LessonId,
        "Menschen",
        "A1.1",
        1,
        "Hallo! Ich bin ...");

    private static readonly IReadOnlyList<QuizQuestion> Questions =
    [
        new(Guid.Parse("20000000-0000-0000-0000-000000000001"), LessonId, QuizCategory.Vocabulary, QuestionType.MultipleChoice, "Wie geht es dir?", ["Wie geht's?", "Wo wohnst du?", "Wie heißt du?"], "Wie geht's?", "این عبارت برای پرسیدن حال استفاده می‌شود."),
        new(Guid.Parse("20000000-0000-0000-0000-000000000002"), LessonId, QuizCategory.Vocabulary, QuestionType.MultipleChoice, "Ergänze: Ich ___ Ali.", ["bin", "bist", "sind"], "bin", "برای ich از bin استفاده می‌کنیم."),
        new(Guid.Parse("20000000-0000-0000-0000-000000000003"), LessonId, QuizCategory.Grammar, QuestionType.MultipleChoice, "___ heißt du?", ["Wie", "Wo", "Was"], "Wie", "ساختار درست سؤال: Wie heißt du?"),
        new(Guid.Parse("20000000-0000-0000-0000-000000000004"), LessonId, QuizCategory.Grammar, QuestionType.MultipleChoice, "Ich ___ aus dem Iran.", ["komme", "kommst", "kommen"], "komme", "صرف فعل kommen برای ich برابر komme است.")
    ];

    public IReadOnlyList<Lesson> GetLessons() => [Lesson];

    public IReadOnlyList<QuizQuestion> GetQuestions(Guid lessonId, QuizCategory? category = null) =>
        Questions.Where(question => question.LessonId == lessonId && (category is null || question.Category == category || category == QuizCategory.Mixed)).ToList();
}
