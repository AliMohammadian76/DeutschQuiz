using DeutschQuiz.Application;
using Microsoft.AspNetCore.Mvc;

namespace DeutschQuiz.Api.Controllers;

[ApiController]
[Route("api")]
public sealed class TranslationController(ITranslationService translationService) : ControllerBase
{
    [HttpPost("translate")]
    public async Task<ActionResult<TranslateResult>> Translate(
        TranslateRequest request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Text))
        {
            return BadRequest(new { message = "Text is required." });
        }

        if (!IsSupportedLanguage(request.SourceLang) || !IsSupportedLanguage(request.TargetLang))
        {
            return BadRequest(new { message = "Only 'de' and 'fa' languages are supported." });
        }

        try
        {
            var result = await translationService.TranslateAsync(request, cancellationToken);
            return Ok(result);
        }
        catch (InvalidOperationException exception)
        {
            return StatusCode(StatusCodes.Status503ServiceUnavailable, new { message = exception.Message });
        }
    }

    private static bool IsSupportedLanguage(string lang) =>
        lang is "de" or "fa";
}
