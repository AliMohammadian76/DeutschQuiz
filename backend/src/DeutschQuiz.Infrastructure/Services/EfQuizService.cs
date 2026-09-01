using DeutschQuiz.Application;
using DeutschQuiz.Domain;
using DeutschQuiz.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DeutschQuiz.Infrastructure.Services;

public sealed class EfQuizService(QuizDbContext db) : IQuizService
{
    public async Task<IReadOnlyList<Lesson>> GetLessonsAsync(
        CancellationToken cancellationToken = default)
    {
        return await db.Lessons
            .AsNoTracking()
            .OrderBy(lesson => lesson.Book.Name)
            .ThenBy(lesson => lesson.Book.Level)
            .ThenBy(lesson => lesson.Number)
            .Select(lesson => new Lesson(
                lesson.Id,
                lesson.Book.Name,
                lesson.Book.Level,
                lesson.Number,
                lesson.Title))
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<QuizQuestion>> GetQuestionsAsync(
        Guid lessonId,
        QuizCategory? category = null,
        CancellationToken cancellationToken = default)
    {
        var query = db.Questions
            .AsNoTracking()
            .Where(question => question.LessonId == lessonId && question.IsActive);

        if (category is not null && category != QuizCategory.Mixed)
        {
            query = query.Where(question => question.Category == category);
        }

        return await query
            .OrderBy(question => question.Id)
            .Select(question => new QuizQuestion(
                question.Id,
                question.LessonId,
                question.Category,
                question.Type,
                question.Prompt,
                question.Options
                    .OrderBy(option => option.SortOrder)
                    .Select(option => option.Text)
                    .ToList(),
                question.CorrectAnswer,
                question.Explanation))
            .ToListAsync(cancellationToken);
    }
}
