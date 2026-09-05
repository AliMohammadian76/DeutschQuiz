using DeutschQuiz.Domain;

namespace DeutschQuiz.Application;

public static class OptionOrder
{
    public static IReadOnlyList<string> Shuffle(IReadOnlyList<string> options)
    {
        var shuffled = options.ToList();
        for (var i = shuffled.Count - 1; i > 0; i--)
        {
            var j = Random.Shared.Next(i + 1);
            (shuffled[i], shuffled[j]) = (shuffled[j], shuffled[i]);
        }

        return shuffled;
    }

    public static QuizQuestion WithShuffledOptions(QuizQuestion question) =>
        question with { Options = Shuffle(question.Options) };
}
