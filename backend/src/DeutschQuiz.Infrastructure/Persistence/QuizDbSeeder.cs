using DeutschQuiz.Domain;
using Microsoft.EntityFrameworkCore;

namespace DeutschQuiz.Infrastructure.Persistence;

public static class QuizDbSeeder
{
    private static readonly Guid BookId =
        Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");

    private static readonly Guid LessonId =
        Guid.Parse("11111111-1111-1111-1111-111111111111");

    public static async Task SeedAsync(
        QuizDbContext db,
        CancellationToken cancellationToken = default)
    {
        await db.Database.MigrateAsync(cancellationToken);

        if (await db.Books.AnyAsync(cancellationToken))
        {
            return;
        }

        var book = new BookEntity
        {
            Id = BookId,
            Name = "Menschen",
            Level = "A1.1",
            Publisher = "Hueber"
        };

        var lesson = new LessonEntity
        {
            Id = LessonId,
            BookId = BookId,
            Number = 1,
            Title = "Hallo! Ich bin ...",
            Book = book
        };

        var questions = new[]
        {
            CreateQuestion(
                "20000000-0000-0000-0000-000000000001",
                QuizCategory.Vocabulary,
                "Wie geht es dir?",
                "Wie geht's?",
                "Diese Frage fragt nach dem Befinden.",
                "Wie geht's?",
                "Wo wohnst du?",
                "Wie heißt du?"),
            CreateQuestion(
                "20000000-0000-0000-0000-000000000002",
                QuizCategory.Vocabulary,
                "Ergänze: Ich ___ Ali.",
                "bin",
                "Mit ich verwenden wir die Form bin.",
                "bin",
                "bist",
                "sind"),
            CreateQuestion(
                "20000000-0000-0000-0000-000000000003",
                QuizCategory.Grammar,
                "___ heißt du?",
                "Wie",
                "Die richtige Frage lautet: Wie heißt du?",
                "Wie",
                "Wo",
                "Was"),
            CreateQuestion(
                "20000000-0000-0000-0000-000000000004",
                QuizCategory.Grammar,
                "Ich ___ aus dem Iran.",
                "komme",
                "Die Form von kommen für ich ist komme.",
                "komme",
                "kommst",
                "kommen")
        };

        foreach (var question in questions)
        {
            question.LessonId = LessonId;
            question.Lesson = lesson;
            lesson.Questions.Add(question);
        }

        db.Books.Add(book);
        db.Lessons.Add(lesson);
        db.Questions.AddRange(questions);
        await db.SaveChangesAsync(cancellationToken);
    }

    private static QuizQuestionEntity CreateQuestion(
        string id,
        QuizCategory category,
        string prompt,
        string correctAnswer,
        string explanation,
        params string[] options)
    {
        var question = new QuizQuestionEntity
        {
            Id = Guid.Parse(id),
            Category = category,
            Type = QuestionType.MultipleChoice,
            Prompt = prompt,
            CorrectAnswer = correctAnswer,
            Explanation = explanation
        };

        for (var index = 0; index < options.Length; index++)
        {
            question.Options.Add(new QuestionOptionEntity
            {
                Id = Guid.NewGuid(),
                SortOrder = index,
                Text = options[index],
                Question = question
            });
        }

        return question;
    }
}
