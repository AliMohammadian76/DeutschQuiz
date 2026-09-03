using DeutschQuiz.Domain;

namespace DeutschQuiz.Application;

public sealed record QuizContentLesson(
    Lesson Lesson,
    IReadOnlyList<QuizQuestion> Questions);

public static class QuizContentCatalog
{
    public static IReadOnlyList<QuizContentLesson> Lessons { get; } =
    [
        CreateLesson(1, "Hallo! Ich bin ...",
            ("Wie geht es dir?", ["Wie geht's?", "Wo wohnst du?", "Wie heißt du?"], "Wie geht's?", "Diese Frage fragt nach dem Befinden."),
            ("Ergänze: Ich ___ Ali.", ["bin", "bist", "sind"], "bin", "Mit ich verwenden wir die Form bin."),
            ("___ heißt du?", ["Wie", "Wo", "Was"], "Wie", "Die Frage nach dem Namen beginnt mit Wie."),
            ("Ich ___ aus dem Iran.", ["komme", "kommst", "kommen"], "komme", "Die Form von kommen für ich ist komme.")),
        CreateLesson(2, "Familie und Freunde",
            ("Das ist meine ___.", ["Schwester", "Montag", "Wohnung"], "Schwester", "Schwester ist ein Familienwort."),
            ("Mein Bruder ___ in Berlin.", ["wohnt", "wohnen", "wohnst"], "wohnt", "Bei er verwenden wir wohnt."),
            ("Das ist ___ Mutter.", ["meine", "mein", "meinen"], "meine", "Mutter ist feminin: meine Mutter."),
            ("___ du Geschwister?", ["Hast", "Hat", "Haben"], "Hast", "Die Frage mit du beginnt mit Hast.")),
        CreateLesson(3, "Zahlen und Alltag",
            ("Welche Zahl kommt nach neun?", ["zehn", "acht", "zwölf"], "zehn", "Nach neun kommt zehn."),
            ("Der Kurs beginnt um ___.", ["acht Uhr", "rot", "Brot"], "acht Uhr", "Eine Uhrzeit passt in den Satz."),
            ("Ich ___ jeden Tag Deutsch.", ["lerne", "lernst", "lernen"], "lerne", "Bei ich lautet die Form lerne."),
            ("Wann ___ du?", ["arbeitest", "arbeitet", "arbeiten"], "arbeitest", "Bei du verwenden wir arbeitest.")),
        CreateLesson(4, "Essen und Trinken",
            ("Was trinkst du am Morgen?", ["Kaffee", "Stuhl", "Jacke"], "Kaffee", "Kaffee ist ein Getränk."),
            ("Ich möchte ein ___.", ["Wasser", "Fenster", "Buch"], "Wasser", "Wasser passt als Getränk."),
            ("Wir ___ heute zusammen.", ["essen", "isst", "esst"], "essen", "Bei wir lautet die Form essen."),
            ("Du ___ gern Tee.", ["trinkst", "trinkt", "trinken"], "trinkst", "Bei du lautet die Form trinkst.")),
        CreateLesson(5, "Wohnen",
            ("Wo schläfst du?", ["im Schlafzimmer", "im Supermarkt", "im Bus"], "im Schlafzimmer", "Das Schlafzimmer ist ein Raum in der Wohnung."),
            ("In der Küche steht ein ___.", ["Tisch", "Schuh", "Sommer"], "Tisch", "Ein Tisch kann in der Küche stehen."),
            ("Die Wohnung ___ zwei Zimmer.", ["hat", "haben", "hast"], "hat", "Bei die Wohnung verwenden wir hat."),
            ("Wir ___ in einer Wohnung.", ["wohnen", "wohnt", "wohnst"], "wohnen", "Bei wir lautet die Form wohnen.")),
        CreateLesson(6, "Freizeit",
            ("Was machst du am Wochenende?", ["Ich spiele Fußball.", "Ich bin ein Tisch.", "Ich trinke eine Lampe."], "Ich spiele Fußball.", "Das ist eine sinnvolle Freizeitaktivität."),
            ("Ein Hobby ist ___.", ["Musik hören", "die Adresse", "der Schlüssel"], "Musik hören", "Musik hören kann ein Hobby sein."),
            ("Er ___ gern Rad.", ["fährt", "fahren", "fährst"], "fährt", "Bei er lautet die Form fährt."),
            ("___ ihr heute Tennis?", ["Spielt", "Spielen", "Spielst"], "Spielt", "Bei ihr lautet die Frage: Spielt ihr ...?")),
        CreateLesson(7, "Arbeit und Termine",
            ("Wann hast du einen Termin?", ["am Dienstag", "im Fenster", "mit Brot"], "am Dienstag", "Ein Wochentag passt zu einem Termin."),
            ("Meine Kollegin arbeitet im ___.", ["Büro", "Kuchen", "Winter"], "Büro", "Im Büro arbeitet man."),
            ("Ich ___ um neun Uhr an.", ["fange", "fängt", "fangen"], "fange", "Die Form von anfangen für ich ist fange an."),
            ("Wir ___ morgen einen Termin.", ["haben", "hat", "hast"], "haben", "Bei wir verwenden wir haben.")),
        CreateLesson(8, "Kleidung und Farben",
            ("Was trägt man an den Füßen?", ["Schuhe", "Hemd", "Hut"], "Schuhe", "Schuhe trägt man an den Füßen."),
            ("Welche Farbe hat Gras?", ["grün", "blau", "schwarz"], "grün", "Gras ist normalerweise grün."),
            ("Die Jacke ___ blau.", ["ist", "sind", "bist"], "ist", "Bei die Jacke verwenden wir ist."),
            ("Ich ___ heute eine Hose.", ["trage", "trägt", "tragen"], "trage", "Bei ich lautet die Form trage.")),
        CreateLesson(9, "Gesundheit",
            ("Was sagt man beim Arzt?", ["Ich habe Schmerzen.", "Ich bin ein Zimmer.", "Ich kaufe einen Tisch."], "Ich habe Schmerzen.", "Das ist eine passende Aussage beim Arzt."),
            ("Bei Fieber braucht man ein ___.", ["Thermometer", "Fahrrad", "Kissen"], "Thermometer", "Mit einem Thermometer misst man die Temperatur."),
            ("Mein Kopf ___.", ["tut weh", "tun weh", "tust weh"], "tut weh", "Kopf ist Singular: tut weh."),
            ("Du ___ viel Wasser trinken.", ["sollst", "soll", "sollen"], "sollst", "Bei du lautet die Form sollst.")),
        CreateLesson(10, "Unterwegs",
            ("Womit fährst du in die Stadt?", ["mit dem Bus", "mit dem Bett", "mit dem Teller"], "mit dem Bus", "Ein Bus ist ein Verkehrsmittel."),
            ("Wo kauft man eine Fahrkarte?", ["am Bahnhof", "im Badezimmer", "im Garten"], "am Bahnhof", "Am Bahnhof bekommt man Fahrkarten."),
            ("Der Zug ___ um zehn Uhr ab.", ["fährt", "fahren", "fährst"], "fährt", "Bei der Zug verwenden wir fährt ab."),
            ("Wir ___ an der nächsten Station aus.", ["steigen", "steigt", "steigst"], "steigen", "Bei wir lautet die Form steigen aus.")),
        CreateLesson(11, "Wetter und Jahreszeiten",
            ("Wie ist das Wetter heute?", ["Es ist sonnig.", "Es ist ein Schuh.", "Es sind drei Uhr."], "Es ist sonnig.", "Das ist eine Wetterbeschreibung."),
            ("Im Winter ist es oft ___.", ["kalt", "schnell", "teuer"], "kalt", "Winter ist normalerweise kalt."),
            ("Heute ___ es.", ["regnet", "regnen", "regnest"], "regnet", "Bei es lautet die Form regnet."),
            ("Im Sommer ___ wir im Park.", ["sitzen", "sitzt", "sitze"], "sitzen", "Bei wir lautet die Form sitzen.")),
        CreateLesson(12, "Reisen und Pläne",
            ("Was nimmt man auf eine Reise mit?", ["einen Koffer", "eine Lampe", "einen Herd"], "einen Koffer", "In einen Koffer packt man Kleidung."),
            ("Wohin möchtest du fahren?", ["nach München", "nach gestern", "nach blau"], "nach München", "München ist ein Reiseziel."),
            ("Nächste Woche ___ ich nach Köln.", ["fahre", "fährt", "fahren"], "fahre", "Bei ich lautet die Form fahre."),
            ("___ du im Hotel?", ["Übernachtest", "Übernachten", "Übernachtet"], "Übernachtest", "Bei du lautet die Form übernachtest."))
    ];

    public static IReadOnlyList<Lesson> GetLessons() =>
        Lessons.Select(content => content.Lesson).ToList();

    public static IReadOnlyList<QuizQuestion> GetQuestions(
        Guid lessonId,
        QuizCategory? category = null) =>
        Lessons
            .Where(content => content.Lesson.Id == lessonId)
            .SelectMany(content => content.Questions)
            .Where(question =>
                category is null ||
                category == QuizCategory.Mixed ||
                question.Category == category)
            .ToList();

    private static QuizContentLesson CreateLesson(
        int number,
        string title,
        (string Prompt, string[] Options, string CorrectAnswer, string Explanation) vocabularyOne,
        (string Prompt, string[] Options, string CorrectAnswer, string Explanation) vocabularyTwo,
        (string Prompt, string[] Options, string CorrectAnswer, string Explanation) grammarOne,
        (string Prompt, string[] Options, string CorrectAnswer, string Explanation) grammarTwo)
    {
        var lessonId = number == 1
            ? Guid.Parse("11111111-1111-1111-1111-111111111111")
            : Guid.Parse($"11111111-1111-1111-1111-{number:000000000000}");
        var baseQuestionId = number == 1 ? 1 : number * 100 + 1;

        return new QuizContentLesson(
            new Lesson(lessonId, "Menschen", "A1.1", number, title),
            [
                CreateQuestion(baseQuestionId, lessonId, QuizCategory.Vocabulary, vocabularyOne),
                CreateQuestion(baseQuestionId + 1, lessonId, QuizCategory.Vocabulary, vocabularyTwo),
                CreateQuestion(baseQuestionId + 2, lessonId, QuizCategory.Grammar, grammarOne),
                CreateQuestion(baseQuestionId + 3, lessonId, QuizCategory.Grammar, grammarTwo)
            ]);
    }

    private static QuizQuestion CreateQuestion(
        int number,
        Guid lessonId,
        QuizCategory category,
        (string Prompt, string[] Options, string CorrectAnswer, string Explanation) content) =>
        new(
            Guid.Parse($"20000000-0000-0000-0000-{number:000000000000}"),
            lessonId,
            category,
            QuestionType.MultipleChoice,
            content.Prompt,
            content.Options,
            content.CorrectAnswer,
            content.Explanation);
}
