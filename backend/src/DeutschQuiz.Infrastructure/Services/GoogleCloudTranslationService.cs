using System.Net;
using System.Text.Json;
using DeutschQuiz.Application;
using Microsoft.Extensions.Configuration;

namespace DeutschQuiz.Infrastructure.Services;

public sealed class GoogleCloudTranslationService(
    HttpClient httpClient,
    IConfiguration configuration) : ITranslationService
{
    private readonly HttpClient _httpClient = httpClient;
    private readonly string? _apiKey = configuration["GoogleTranslate:ApiKey"];

    public async Task<TranslateResult> TranslateAsync(
        TranslateRequest request,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(_apiKey))
        {
            throw new InvalidOperationException(
                "Google Translate API key is not configured. Set GoogleTranslate:ApiKey.");
        }

        using var body = new FormUrlEncodedContent(
            new Dictionary<string, string>
            {
                ["q"] = request.Text,
                ["source"] = request.SourceLang,
                ["target"] = request.TargetLang,
                ["format"] = "text"
            });

        using var response = await _httpClient.PostAsync(
            $"https://translation.googleapis.com/language/translate/v2?key={Uri.EscapeDataString(_apiKey)}",
            body,
            cancellationToken);

        var payload = await response.Content.ReadAsStringAsync(cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException(
                $"Google Translate request failed: {(int)response.StatusCode} {response.ReasonPhrase}. {payload}");
        }

        using var document = JsonDocument.Parse(payload);
        var translated = document.RootElement
            .GetProperty("data")
            .GetProperty("translations")[0]
            .GetProperty("translatedText")
            .GetString();

        if (string.IsNullOrWhiteSpace(translated))
        {
            throw new InvalidOperationException("Google Translate returned an empty translation.");
        }

        return new TranslateResult(WebUtility.HtmlDecode(translated));
    }
}
