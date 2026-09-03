using DeutschQuiz.Application;
using DeutschQuiz.Domain;
using DeutschQuiz.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DeutschQuiz.Infrastructure.Services;

public sealed class ProgressService(QuizDbContext db) : IProgressService
{
    public async Task<AttemptResult?> SubmitAsync(
        Guid userId,
        SubmitAttemptRequest request,
        CancellationToken cancellationToken = default)
    {
        var answers = (request.Answers ?? [])
            .Where(answer => answer.QuestionId != Guid.Empty)
            .GroupBy(answer => answer.QuestionId)
            .Select(group => group.First())
            .ToList();

        if (request.LessonId == Guid.Empty || answers.Count == 0)
        {
            return null;
        }

        var questionsQuery = db.Questions
            .AsNoTracking()
            .Where(question =>
                question.LessonId == request.LessonId &&
                question.IsActive);

        if (request.Category != QuizCategory.Mixed)
        {
            questionsQuery = questionsQuery.Where(
                question => question.Category == request.Category);
        }

        var questions = await questionsQuery.ToListAsync(cancellationToken);
        if (questions.Count == 0 ||
            questions.Count != answers.Count ||
            questions.Select(question => question.Id)
                .Except(answers.Select(answer => answer.QuestionId))
                .Any())
        {
            return null;
        }

        var questionsById = questions.ToDictionary(question => question.Id);
        var completedAtUtc = DateTime.UtcNow;
        var startedAtUtc = request.StartedAtUtc?.ToUniversalTime() ?? completedAtUtc;
        var answerEntities = new List<QuizAttemptAnswerEntity>(answers.Count);
        var answerResults = new List<AttemptAnswerResult>(answers.Count);
        var correctAnswers = 0;

        foreach (var answer in answers)
        {
            var question = questionsById[answer.QuestionId];
            var selectedAnswer = answer.SelectedAnswer.Trim();
            var isCorrect = string.Equals(
                selectedAnswer,
                question.CorrectAnswer.Trim(),
                StringComparison.OrdinalIgnoreCase);

            if (isCorrect)
            {
                correctAnswers++;
            }

            answerEntities.Add(new QuizAttemptAnswerEntity
            {
                Id = Guid.NewGuid(),
                QuestionId = question.Id,
                SelectedAnswer = selectedAnswer,
                IsCorrect = isCorrect,
                ResponseTimeMs = Math.Max(0, answer.ResponseTimeMs),
                AnsweredAtUtc = completedAtUtc
            });

            answerResults.Add(new AttemptAnswerResult(
                question.Id,
                question.Prompt,
                selectedAnswer,
                question.CorrectAnswer,
                isCorrect,
                question.Explanation,
                Math.Max(0, answer.ResponseTimeMs)));
        }

        var totalQuestions = questions.Count;
        var score = Math.Round(
            correctAnswers * 100m / totalQuestions,
            2,
            MidpointRounding.AwayFromZero);
        var attempt = new QuizAttemptEntity
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            LessonId = request.LessonId,
            Category = request.Category,
            StartedAtUtc = startedAtUtc,
            CompletedAtUtc = completedAtUtc,
            Score = score,
            TotalQuestions = totalQuestions,
            CorrectAnswers = correctAnswers,
            TotalTimeMs = answerEntities.Sum(answer => answer.ResponseTimeMs),
            Answers = answerEntities
        };

        db.QuizAttempts.Add(attempt);
        await db.SaveChangesAsync(cancellationToken);

        return new AttemptResult(
            attempt.Id,
            totalQuestions,
            correctAnswers,
            score,
            attempt.TotalTimeMs,
            completedAtUtc,
            answerResults);
    }

    public async Task<ProgressSummary> GetSummaryAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var attempts = await db.QuizAttempts
            .AsNoTracking()
            .Where(attempt => attempt.UserId == userId)
            .Include(attempt => attempt.Lesson)
            .ThenInclude(lesson => lesson.Book)
            .OrderByDescending(attempt => attempt.CompletedAtUtc)
            .ToListAsync(cancellationToken);

        if (attempts.Count == 0)
        {
            return new ProgressSummary(0, 0, 0, 0, 0, 0, []);
        }

        var lessons = attempts
            .GroupBy(attempt => attempt.Lesson)
            .Select(group => new ProgressLessonSummary(
                group.Key.Id,
                group.Key.Book.Name,
                group.Key.Book.Level,
                group.Key.Number,
                group.Key.Title,
                group.Count(),
                Math.Round(group.Average(attempt => attempt.Score ?? 0m), 2),
                group.Max(attempt => attempt.CompletedAtUtc)))
            .OrderBy(summary => summary.Book)
            .ThenBy(summary => summary.Level)
            .ThenBy(summary => summary.LessonNumber)
            .ToList();

        return new ProgressSummary(
            attempts.Count,
            Math.Round(attempts.Average(attempt => attempt.Score ?? 0m), 2),
            (int)Math.Round(attempts.Max(attempt => attempt.Score ?? 0m)),
            attempts.Sum(attempt => attempt.TotalQuestions),
            attempts.Sum(attempt => attempt.CorrectAnswers),
            attempts.Sum(attempt => attempt.TotalTimeMs),
            lessons);
    }
}
