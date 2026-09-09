namespace DeutschQuiz.Application;

public sealed record TranslateRequest(
    string Text,
    string SourceLang,
    string TargetLang);

public sealed record TranslateResult(string TranslatedText);

public interface ITranslationService
{
    Task<TranslateResult> TranslateAsync(
        TranslateRequest request,
        CancellationToken cancellationToken = default);
}
