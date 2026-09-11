using DeutschQuiz.Application;
using DeutschQuiz.Api.Contracts;
using DeutschQuiz.Domain;
using DeutschQuiz.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace DeutschQuiz.Api.Controllers;

[ApiController]
[Route("api")]
public sealed class QuizController(IQuizService quizService, IAiQuizGenerator aiQuizGenerator) : ControllerBase
{
    [HttpGet("health")]
    public IActionResult Health() => Ok(new { status = "ok", service = "DeutschQuiz.Api" });

    [HttpGet("lessons")]
    public async Task<ActionResult<IReadOnlyList<Lesson>>> Lessons(
        CancellationToken cancellationToken)
    {
        var lessons = await quizService.GetLessonsAsync(cancellationToken);
        return Ok(lessons);
    }

    [HttpGet("lessons/{lessonId:guid}/questions")]
    public async Task<ActionResult<IReadOnlyList<QuizQuestionResponse>>> Questions(
        Guid lessonId,
        [FromQuery] QuizCategory? category = null,
        CancellationToken cancellationToken = default)
    {
        var questions = await quizService.GetQuestionsAsync(lessonId, category, cancellationToken);
        return questions.Count == 0
            ? NotFound()
            : Ok(questions.Select(question => new QuizQuestionResponse(
                question.Id,
                question.LessonId,
                question.Category,
                question.Type,
                question.Prompt,
                question.Options)));
    }

    [Authorize]
    [HttpPost("lessons/{lessonId:guid}/questions/generate")]
    public async Task<ActionResult<IReadOnlyList<QuizQuestionResponse>>> GenerateQuestions(
        Guid lessonId,
        [FromQuery] QuizCategory category = QuizCategory.Mixed,
        [FromQuery] int count = 10,
        CancellationToken cancellationToken = default)
    {
        var lesson = (await quizService.GetLessonsAsync(cancellationToken))
            .FirstOrDefault(item => item.Id == lessonId);
        if (lesson is null) return NotFound();

        try
        {
            var questions = await aiQuizGenerator.GenerateAsync(lesson, category, Math.Clamp(count, 1, 20), cancellationToken);
            await quizService.AddGeneratedQuestionsAsync(questions, cancellationToken);
            return Ok(questions.Select(question => new QuizQuestionResponse(
                question.Id, question.LessonId, question.Category, question.Type,
                question.Prompt, question.Options)));
        }
        catch (InvalidOperationException error)
        {
            return StatusCode(StatusCodes.Status503ServiceUnavailable, new { message = error.Message });
        }
    }
}
