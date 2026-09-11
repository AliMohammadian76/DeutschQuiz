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

        var hasGenerated = await query.AnyAsync(question => question.IsGenerated, cancellationToken);
        if (hasGenerated)
            query = query.Where(question => question.IsGenerated);

        var questions = await query
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

        return questions
            .Select(OptionOrder.WithShuffledOptions)
            .ToList();
    }

    public async Task AddGeneratedQuestionsAsync(
        IReadOnlyList<QuizQuestion> questions,
        CancellationToken cancellationToken = default)
    {
        if (questions.Count == 0) return;

        var lessonId = questions[0].LessonId;
        var category = questions[0].Category;
        var previous = await db.Questions
            .Where(question => question.LessonId == lessonId && question.Category == category && question.IsGenerated)
            .ToListAsync(cancellationToken);
        db.Questions.RemoveRange(previous);

        foreach (var question in questions)
        {
            var entity = new QuizQuestionEntity
            {
                Id = question.Id,
                LessonId = question.LessonId,
                Category = question.Category,
                Type = question.Type,
                Prompt = question.Prompt,
                CorrectAnswer = question.CorrectAnswer,
                Explanation = question.Explanation,
                IsActive = true,
                IsGenerated = true,
                Options = question.Options.Select((text, index) => new QuestionOptionEntity
                {
                    Id = Guid.NewGuid(),
                    SortOrder = index,
                    Text = text,
                }).ToList(),
            };
            db.Questions.Add(entity);
        }

        await db.SaveChangesAsync(cancellationToken);
    }
}
