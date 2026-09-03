using System.Security.Claims;
using DeutschQuiz.Application;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeutschQuiz.Api.Controllers;

[ApiController]
[Authorize]
[Route("api")]
public sealed class ProgressController(IProgressService progressService) : ControllerBase
{
    [HttpPost("attempts")]
    public async Task<ActionResult<AttemptResult>> SubmitAttempt(
        SubmitAttemptRequest request,
        CancellationToken cancellationToken)
    {
        if (!TryGetUserId(out var userId))
        {
            return Unauthorized();
        }

        var result = await progressService.SubmitAsync(
            userId,
            request,
            cancellationToken);

        return result is null
            ? BadRequest(new { message = "The attempt data is invalid." })
            : Ok(result);
    }

    [HttpGet("progress/summary")]
    public async Task<ActionResult<ProgressSummary>> Summary(
        CancellationToken cancellationToken)
    {
        if (!TryGetUserId(out var userId))
        {
            return Unauthorized();
        }

        return Ok(await progressService.GetSummaryAsync(userId, cancellationToken));
    }

    [HttpGet("progress/history")]
    public async Task<ActionResult<IReadOnlyList<AttemptHistoryItem>>> History(
        [FromQuery] int limit = 20,
        CancellationToken cancellationToken = default)
    {
        if (!TryGetUserId(out var userId))
        {
            return Unauthorized();
        }

        return Ok(await progressService.GetHistoryAsync(
            userId,
            limit,
            cancellationToken));
    }

    private bool TryGetUserId(out Guid userId) =>
        Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out userId);
}
