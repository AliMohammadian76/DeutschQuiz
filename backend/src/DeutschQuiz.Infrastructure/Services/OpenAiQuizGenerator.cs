using System.Net.Http.Json;
using System.Text.Json;
using DeutschQuiz.Domain;
using Microsoft.Extensions.Configuration;

namespace DeutschQuiz.Infrastructure.Services;

public interface IAiQuizGenerator
{
    Task<IReadOnlyList<QuizQuestion>> GenerateAsync(
        Lesson lesson,
        QuizCategory category,
        int count,
        CancellationToken cancellationToken = default);
}

public sealed class OllamaQuizGenerator(
    HttpClient httpClient,
    IConfiguration configuration) : IAiQuizGenerator
{
    public async Task<IReadOnlyList<QuizQuestion>> GenerateAsync(
        Lesson lesson,
        QuizCategory category,
        int count,
        CancellationToken cancellationToken = default)
    {
        var baseUrl = configuration["Ollama:BaseUrl"] ?? "http://localhost:11434";
        var model = configuration["Ollama:Model"] ?? "qwen2.5:7b";
        var safeCount = Math.Clamp(count, 1, 30);

        var payload = new
        {
            model,
            stream = false,
            format = "json",
            prompt = $"You are a careful German teacher. Create {safeCount} original German learning multiple-choice questions. Book: {lesson.Book}. Level: {lesson.Level}. Lesson {lesson.Number}: {lesson.Title}. Category: {category}. Use natural German appropriate for the level. Each question must have exactly four options and one correct answer. Explanations should be concise Persian. Return ONLY valid JSON in this exact shape: {{\"questions\":[{{\"prompt\":\"...\",\"options\":[\"...\",\"...\",\"...\",\"...\"],\"correctAnswer\":\"...\",\"explanation\":\"...\"}}]}}. Do not use markdown fences."
        };

        using var response = await httpClient.PostAsJsonAsync($"{baseUrl.TrimEnd('/')}/api/generate", payload, cancellationToken);
        var body = await response.Content.ReadAsStringAsync(cancellationToken);
        if (!response.IsSuccessStatusCode)
            throw new InvalidOperationException($"Ollama request failed ({(int)response.StatusCode}): {body}");

        using var document = JsonDocument.Parse(body);
        var json = document.RootElement.GetProperty("response").GetString() ?? string.Empty;
        var generated = JsonSerializer.Deserialize<GeneratedQuiz>(json, new JsonSerializerOptions(JsonSerializerDefaults.Web))
            ?? throw new InvalidOperationException("Ollama returned an empty quiz.");

        var result = generated.Questions
            .Where(question => question.Options is { Count: 4 } && question.Options.Contains(question.CorrectAnswer))
            .Select(question => new QuizQuestion(
                Guid.NewGuid(), lesson.Id, category, QuestionType.MultipleChoice,
                question.Prompt.Trim(), question.Options.Select(option => option.Trim()).ToArray(),
                question.CorrectAnswer.Trim(), question.Explanation?.Trim() ?? string.Empty))
            .Where(question => question.Prompt.Length > 0)
            .Take(safeCount)
            .ToList();

        if (result.Count == 0)
            throw new InvalidOperationException("Ollama returned no valid quiz questions.");
        return result;
    }

    private sealed record GeneratedQuiz(List<GeneratedQuestion> Questions);
    private sealed record GeneratedQuestion(string Prompt, List<string> Options, string CorrectAnswer, string? Explanation);
}
