using DeutschQuiz.Application;
using DeutschQuiz.Domain;
using Microsoft.EntityFrameworkCore;

namespace DeutschQuiz.Infrastructure.Persistence;

public static class QuizDbSeeder
{
    public static async Task SeedAsync(
        QuizDbContext db,
        CancellationToken cancellationToken = default)
    {
        await db.Database.MigrateAsync(cancellationToken);

        foreach (var catalog in QuizContentCatalog.Books)
        {
            var book = await db.Books
                .Include(item => item.Lessons)
                .SingleOrDefaultAsync(item => item.Id == catalog.Id, cancellationToken);

            if (book is null)
            {
                book = new BookEntity
                {
                    Id = catalog.Id,
                    Name = catalog.Name,
                    Level = catalog.Level,
                    Publisher = catalog.Publisher
                };
                db.Books.Add(book);
                await db.SaveChangesAsync(cancellationToken);
            }
            else if (
                book.Name != catalog.Name ||
                book.Level != catalog.Level ||
                book.Publisher != catalog.Publisher)
            {
                book.Name = catalog.Name;
                book.Level = catalog.Level;
                book.Publisher = catalog.Publisher;
                await db.SaveChangesAsync(cancellationToken);
            }

            foreach (var content in catalog.Lessons)
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
                    book.Lessons.Add(lesson);
                    await db.SaveChangesAsync(cancellationToken);
                }

                var existingQuestions = await db.Questions
                    .Include(question => question.Options)
                    .Where(question => question.LessonId == lesson.Id)
                    .ToDictionaryAsync(question => question.Id, cancellationToken);

                foreach (var question in content.Questions)
                {
                    if (!existingQuestions.TryGetValue(question.Id, out var entity))
                    {
                        entity = new QuizQuestionEntity
                        {
                            Id = question.Id,
                            LessonId = lesson.Id,
                            Lesson = lesson
                        };
                        db.Questions.Add(entity);
                    }

                    entity.Category = question.Category;
                    entity.Type = question.Type;
                    entity.Prompt = question.Prompt;
                    entity.CorrectAnswer = question.CorrectAnswer;
                    entity.Explanation = question.Explanation;

                    for (var index = 0; index < question.Options.Count; index++)
                    {
                        var option = entity.Options
                            .SingleOrDefault(item => item.SortOrder == index);
                        if (option is null)
                        {
                            entity.Options.Add(new QuestionOptionEntity
                            {
                                Id = Guid.NewGuid(),
                                SortOrder = index,
                                Text = question.Options[index],
                                Question = entity
                            });
                        }
                        else
                        {
                            option.Text = question.Options[index];
                        }
                    }

                    foreach (var extraOption in entity.Options
                                 .Where(option => option.SortOrder >= question.Options.Count)
                                 .ToList())
                    {
                        db.QuestionOptions.Remove(extraOption);
                    }
                }

                await db.SaveChangesAsync(cancellationToken);
                db.ChangeTracker.Clear();

                book = await db.Books
                    .Include(item => item.Lessons)
                    .SingleAsync(item => item.Id == catalog.Id, cancellationToken);
            }
        }
    }
}
