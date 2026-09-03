using DeutschQuiz.Application;
using DeutschQuiz.Domain;
using Microsoft.EntityFrameworkCore;

namespace DeutschQuiz.Infrastructure.Persistence;

public static class QuizDbSeeder
{
    private static readonly Guid BookId =
        Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");

    public static async Task SeedAsync(
        QuizDbContext db,
        CancellationToken cancellationToken = default)
    {
        await db.Database.MigrateAsync(cancellationToken);

        var book = await db.Books
            .Include(item => item.Lessons)
            .SingleOrDefaultAsync(item => item.Id == BookId, cancellationToken);

        if (book is null)
        {
            book = new BookEntity
            {
                Id = BookId,
                Name = "Menschen",
                Level = "A1.1",
                Publisher = "Hueber"
            };
            db.Books.Add(book);
        }

        foreach (var content in QuizContentCatalog.Lessons)
        {
            var lesson = book.Lessons.SingleOrDefault(item => item.Id == content.Lesson.Id);
            if (lesson is null)
            {
                lesson = new LessonEntity
                {
                    Id = content.Lesson.Id,
                    BookId = book.Id,
                    Number = content.Lesson.Number,
                    Title = content.Lesson.Title,
                    Book = book
                };
                db.Lessons.Add(lesson);
            }

            var existingQuestionIds = await db.Questions
                .Where(question => question.LessonId == lesson.Id)
                .Select(question => question.Id)
                .ToListAsync(cancellationToken);

            foreach (var question in content.Questions.Where(question =>
                         !existingQuestionIds.Contains(question.Id)))
            {
                var entity = new QuizQuestionEntity
                {
                    Id = question.Id,
                    LessonId = lesson.Id,
                    Category = question.Category,
                    Type = question.Type,
                    Prompt = question.Prompt,
                    CorrectAnswer = question.CorrectAnswer,
                    Explanation = question.Explanation,
                    Lesson = lesson
                };

                for (var index = 0; index < question.Options.Count; index++)
                {
                    entity.Options.Add(new QuestionOptionEntity
                    {
                        Id = Guid.NewGuid(),
                        SortOrder = index,
                        Text = question.Options[index],
                        Question = entity
                    });
                }

                db.Questions.Add(entity);
            }
        }

        await db.SaveChangesAsync(cancellationToken);
    }
}
