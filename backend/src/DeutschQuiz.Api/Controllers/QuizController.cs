using DeutschQuiz.Application;
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
    public IReadOnlyList<Lesson> Lessons() => quizService.GetLessons();

    [HttpGet("lessons/{lessonId:guid}/questions")]
    public ActionResult<IReadOnlyList<QuizQuestion>> Questions(Guid lessonId, [FromQuery] QuizCategory? category = null)
    {
        var questions = quizService.GetQuestions(lessonId, category);
        return questions.Count == 0 ? NotFound() : Ok(questions);
    }
}
