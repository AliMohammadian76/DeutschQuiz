using DeutschQuiz.Application;
using DeutschQuiz.Api.Contracts;
using DeutschQuiz.Domain;
using Microsoft.AspNetCore.Mvc;

namespace DeutschQuiz.Api.Controllers;

[ApiController]
[Route("api")]
public sealed class QuizController(IQuizService quizService) : ControllerBase
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
}
