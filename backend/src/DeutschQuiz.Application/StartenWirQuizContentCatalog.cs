using DeutschQuiz.Domain;

namespace DeutschQuiz.Application;

/// <summary>
/// An independent starter catalogue for Starten wir! A1.
/// The prompts are authored for DeutschQuiz and are not copied from the textbook.
/// </summary>
public static class StartenWirQuizContentCatalog
{
    public static IReadOnlyList<QuizContentLesson> Lessons { get; } =
    [
        CreateLesson(1, "Hallo und Kennenlernen",
            new VerbSeed("heißen", "heiße", "heißt", "heißt", "heißen", "heißt", "heißen"),
            V("Wie ___ du?", "heißt", "wohnst", "lernst"),
            V("Eine passende Begrüßung am Morgen ist ...", "Guten Morgen!", "Gute Nacht!", "Tschüss!"),
            V("Auf die Frage „Wie geht es dir?“ passt ...", "Gut, danke.", "In Berlin.", "Am Montag."),
            V("„Ich komme ___ Spanien.“", "aus", "am", "um"),
            V("Nach dem Namen fragt man mit ...", "Wie heißt du?", "Wo wohnst du?", "Was machst du?"),
            V("Eine Sprache auf A1 ist ...", "Deutsch", "Dienstag", "Deutschland"),
            V("Zum Abschied sagt man ...", "Auf Wiedersehen!", "Guten Appetit!", "Guten Morgen!"),
            V("„Wo wohnst du?“ fragt nach ...", "dem Wohnort", "der Uhrzeit", "dem Beruf"),
            V("Eine höfliche Anrede ist ...", "Sie", "du", "ihr"),
            V("„Das ist Anna.“ – „___, Anna!“", "Hallo", "Gute Nacht", "Bis morgen")),

        CreateLesson(2, "Familie und Personen",
            new VerbSeed("haben", "habe", "hast", "hat", "haben", "habt", "haben"),
            V("Der Bruder von deiner Mutter ist dein ...", "Onkel", "Sohn", "Vater"),
            V("Die Mutter von deinem Vater ist deine ...", "Großmutter", "Schwester", "Tochter"),
            V("Der Sohn von deinen Eltern ist dein ...", "Bruder", "Onkel", "Großvater"),
            V("Die Tochter von deinen Eltern ist deine ...", "Schwester", "Tante", "Mutter"),
            V("Der Vater und die Mutter sind die ...", "Eltern", "Kinder", "Freunde"),
            V("„Wer ist das?“ – „Das ist ...“", "meine Schwester", "am Dienstag", "sehr gut"),
            V("Eine Person ohne Geschwister ist ...", "Einzelkind", "Nachbar", "Lehrer"),
            V("Der Mann von deiner Tante ist dein ...", "Onkel", "Bruder", "Sohn"),
            V("„Das ist ___ Vater.“", "mein", "meine", "meinen"),
            V("Mit Freunden kann man ...", "zusammen lachen", "eine Adresse trinken", "einen Bruder wohnen")),

        CreateLesson(3, "Alltag und Uhrzeit",
            new VerbSeed("machen", "mache", "machst", "macht", "machen", "macht", "machen"),
            V("Um 7 Uhr beginnt oft der ...", "Morgen", "Abend", "Sonntag"),
            V("Nach dem Aufstehen putzt man die ...", "Zähne", "Wohnung", "Uhr"),
            V("Um 12 Uhr ist ...", "Mittag", "Mitternacht", "Morgen"),
            V("„Wie spät ist es?“ – „Es ist ___ Uhr.“", "acht", "blau", "Berlin"),
            V("Am Ende des Tages kommt die ...", "Nacht", "Woche", "Pause"),
            V("Ein Termin ist eine feste ...", "Zeit", "Farbe", "Familie"),
            V("Montag, Dienstag und Mittwoch sind ...", "Wochentage", "Monate", "Jahreszeiten"),
            V("„Um halb neun“ bedeutet ...", "8:30", "9:30", "8:15"),
            V("Am Wochenende hat man oft mehr ...", "Zeit", "Hunger", "Adresse"),
            V("Ein Wecker klingelt am ...", "Morgen", "Mittagessen", "Bahnhof")),

        CreateLesson(4, "Essen und Einkaufen",
            new VerbSeed("essen", "esse", "isst", "isst", "essen", "esst", "essen"),
            V("Zum Frühstück trinkt man oft ...", "Kaffee", "Schuhe", "Seife"),
            V("Brot kauft man in der ...", "Bäckerei", "Apotheke", "Bibliothek"),
            V("Obst und Gemüse gibt es auf dem ...", "Markt", "Bahnhof", "Spielplatz"),
            V("„Was kostet das?“ fragt nach dem ...", "Preis", "Namen", "Wetter"),
            V("Mit Messer und Gabel ... man.", "isst", "wohnt", "fährt"),
            V("Ein Apfel ist ein ...", "Obst", "Getränk", "Möbel"),
            V("Wasser ist ein ...", "Getränk", "Kleid", "Zimmer"),
            V("Im Restaurant bestellt man ein ...", "Essen", "Ticket", "Bett"),
            V("Die Rechnung bezahlt man nach dem ...", "Essen", "Aufstehen", "Anrufen"),
            V("„Ich möchte ein Kilo ...“", "Tomaten", "Uhr", "Jacken")),

        CreateLesson(5, "Wohnen",
            new VerbSeed("wohnen", "wohne", "wohnst", "wohnt", "wohnen", "wohnt", "wohnen"),
            V("In der Küche kocht man ...", "Essen", "Schuhe", "Post"),
            V("Im Schlafzimmer steht ein ...", "Bett", "Herd", "Bus"),
            V("Im Badezimmer gibt es eine ...", "Dusche", "Lampe", "Tasche"),
            V("Eine Wohnung hat oft mehrere ...", "Zimmer", "Wochentage", "Sprachen"),
            V("Die Tür macht man mit einem ... auf.", "Schlüssel", "Löffel", "Koffer"),
            V("Am Fenster hängt oft ein ...", "Vorhang", "Fahrplan", "Pass"),
            V("Auf dem Tisch steht eine ...", "Lampe", "Wohnung", "Adresse"),
            V("Ein Haus mit Garten liegt oft außerhalb der ...", "Stadt", "Uhr", "Küche"),
            V("„Wo wohnst du?“ – „Ich wohne ...“", "in Köln", "um acht Uhr", "sehr gern"),
            V("Miete bezahlt man für eine ...", "Wohnung", "Farbe", "Familie")),

        CreateLesson(6, "Freizeit und Hobbys",
            new VerbSeed("spielen", "spiele", "spielst", "spielt", "spielen", "spielt", "spielen"),
            V("Am Wochenende kann man einen Film ...", "sehen", "essen", "wohnen"),
            V("Fußball spielt man mit einem ...", "Ball", "Bett", "Brief"),
            V("Ein Buch liest man in der ...", "Freizeit", "Küche", "Adresse"),
            V("Musik hört man mit ...", "Kopfhörern", "Gabeln", "Schlüsseln"),
            V("Wer gern malt, hat ein kreatives ...", "Hobby", "Ticket", "Zimmer"),
            V("Im Park kann man ...", "spazieren gehen", "einchecken", "einkaufen bezahlen"),
            V("Ein Sportverein ist ein Ort für ...", "Sport", "Schlaf", "Post"),
            V("„Was machst du gern?“ fragt nach ...", "einem Hobby", "einer Adresse", "einem Preis"),
            V("Samstag und Sonntag sind das ...", "Wochenende", "Frühstück", "Jahr"),
            V("Zusammen mit Freunden macht Freizeit mehr ...", "Spaß", "Miete", "Wetter")),

        CreateLesson(7, "Arbeit und Termine",
            new VerbSeed("arbeiten", "arbeite", "arbeitest", "arbeitet", "arbeiten", "arbeitet", "arbeiten"),
            V("Ein Arzt arbeitet oft in einem ...", "Krankenhaus", "Supermarkt", "Bahnhof"),
            V("Eine Lehrerin arbeitet in der ...", "Schule", "Bäckerei", "Garage"),
            V("Ein Termin steht im ...", "Kalender", "Koffer", "Kühlschrank"),
            V("Um einen Termin zu vereinbaren, kann man ...", "anrufen", "schwimmen", "schlafen"),
            V("Der Chef ist eine Person bei der ...", "Arbeit", "Familie", "Reise"),
            V("Am Arbeitsplatz braucht man manchmal einen ...", "Computer", "Regenschirm", "Fahrstuhl"),
            V("„Wann hast du Zeit?“ fragt nach einem ...", "Termin", "Namen", "Getränk"),
            V("Eine Person mit einem Beruf ist ein ...", "Mitarbeiter", "Wochentag", "Zimmer"),
            V("Der Arbeitstag beginnt am ...", "Morgen", "Mitternacht", "Wochenende"),
            V("Nach der Arbeit hat man ...", "Feierabend", "Frühstück", "Unterricht")),

        CreateLesson(8, "Kleidung und Farben",
            new VerbSeed("tragen", "trage", "trägst", "trägt", "tragen", "tragt", "tragen"),
            V("Im Winter trägt man oft eine ...", "Jacke", "Sandale", "Badehose"),
            V("An den Füßen trägt man ...", "Schuhe", "Handschuhe", "Mützen"),
            V("Eine Hose ist ein Kleidungsstück für die ...", "Beine", "Ohren", "Hände"),
            V("Rot, blau und grün sind ...", "Farben", "Getränke", "Berufe"),
            V("Ein T-Shirt trägt man am ...", "Oberkörper", "Fuß", "Kopf"),
            V("Eine Mütze trägt man auf dem ...", "Kopf", "Arm", "Rücken"),
            V("„Welche Farbe hat die Jacke?“ – „Sie ist ...“", "schwarz", "gestern", "klein Uhr"),
            V("Im Sommer ist leichte ... angenehm.", "Kleidung", "Miete", "Adresse"),
            V("Im Geschäft probiert man Kleidung in der ... an.", "Umkleidekabine", "Küche", "Apotheke"),
            V("Ein Pullover ist meistens ... als ein T-Shirt.", "wärmer", "schneller", "lauter")),

        CreateLesson(9, "Körper und Gesundheit",
            new VerbSeed("fühlen", "fühle", "fühlst", "fühlt", "fühlen", "fühlt", "fühlen"),
            V("Mit den Augen kann man ...", "sehen", "hören", "schmecken"),
            V("Mit den Ohren kann man ...", "hören", "sehen", "laufen"),
            V("Wenn man krank ist, geht man zum ...", "Arzt", "Bäcker", "Fahrer"),
            V("Bei Zahnschmerzen hilft der ...", "Zahnarzt", "Friseur", "Lehrer"),
            V("Wasser trinken ist ...", "gesund", "laut", "teuer"),
            V("Bei Fieber misst man die ...", "Temperatur", "Adresse", "Uhrzeit"),
            V("Eine Tablette nimmt man mit ...", "Wasser", "Schuhen", "Käse"),
            V("„Wie fühlst du dich?“ – „Nicht so ...“", "gut", "blau", "spät"),
            V("Bei einer Erkältung hat man oft ...", "Husten", "Hunger nach Möbeln", "einen Bahnhof"),
            V("Ruhe und Schlaf sind gut für die ...", "Gesundheit", "Kleidung", "Reise")),

        CreateLesson(10, "Unterwegs in der Stadt",
            new VerbSeed("fahren", "fahre", "fährst", "fährt", "fahren", "fahrt", "fahren"),
            V("Mit dem Bus fährt man durch die ...", "Stadt", "Küche", "Familie"),
            V("Am Bahnhof wartet der ...", "Zug", "Arzt", "Schrank"),
            V("Für den Bus braucht man ein ...", "Ticket", "Kissen", "Rezept"),
            V("Die Straße überquert man an der ...", "Ampel", "Dusche", "Tafel"),
            V("Ein Taxi bringt dich an dein ...", "Ziel", "Frühstück", "Zimmer"),
            V("Eine Karte zeigt den Weg und die ...", "Straßen", "Farben", "Berufe"),
            V("„Wie komme ich zum Zentrum?“ ist eine Frage nach dem ...", "Weg", "Preis", "Namen"),
            V("Die U-Bahn fährt unter der ...", "Erde", "Jacke", "Tasse"),
            V("Man wartet an der Haltestelle auf den ...", "Bus", "Koffer", "Schlüssel"),
            V("Zu Fuß gehen ist eine Art der ...", "Fortbewegung", "Begrüßung", "Bezahlung")),

        CreateLesson(11, "Wetter und Jahreszeiten",
            new VerbSeed("regnen", "regne", "regnest", "regnet", "regnen", "regnet", "regnen"),
            V("Wenn Wasser vom Himmel fällt, ... es.", "regnet", "schneit immer", "scheint"),
            V("Die Sonne ... am Himmel.", "scheint", "wohnt", "kauft"),
            V("Im Winter kann es ...", "schneien", "kochen", "telefonieren"),
            V("Frühling, Sommer, Herbst und Winter sind ...", "Jahreszeiten", "Wochentage", "Zimmer"),
            V("Bei schlechtem Wetter braucht man einen ...", "Regenschirm", "Koffer", "Löffel"),
            V("Im Sommer ist es oft ...", "warm", "kalt wie Eis immer", "geschlossen"),
            V("Im Herbst fallen die ...", "Blätter", "Tickets", "Zähne"),
            V("„Wie ist das Wetter?“ – „Es ist ...“", "sonnig", "am Bahnhof", "mein Bruder"),
            V("Bei starkem Wind ist es ...", "windig", "hungrig", "teuer"),
            V("Eine Wettervorhersage zeigt das Wetter für ...", "die nächsten Tage", "die Familie", "den Preis")),

        CreateLesson(12, "Reisen und Pläne",
            new VerbSeed("gehen", "gehe", "gehst", "geht", "gehen", "geht", "gehen"),
            V("Für eine Reise packt man einen ...", "Koffer", "Herd", "Kalender"),
            V("Im Hotel braucht man eine ...", "Reservierung", "Jahreszeit", "Farbe"),
            V("Am Flughafen muss man ...", "einchecken", "duschen im Zug", "eine Farbe kaufen"),
            V("Der Reisepass zeigt deine ...", "Identität", "Uhrzeit", "Lieblingsfarbe"),
            V("Ein Reiseziel ist zum Beispiel ...", "München", "Dienstag", "Frühstück"),
            V("Vor der Abfahrt wartet man auf den ...", "Zug", "Schlüssel", "Arzt"),
            V("In den Koffer packt man ...", "Kleidung", "eine Dusche", "einen Bahnhof"),
            V("„Wohin fährst du?“ – „Ich fahre ...“", "nach Berlin", "um acht Uhr", "sehr nett"),
            V("Eine Reise für zwei Tage ist ein ...", "Kurztrip", "Wochentag", "Arbeitsplatz"),
            V("Für morgen macht man einen ...", "Plan", "Schuh", "Kühlschrank"))
    ];

    private static QuizContentLesson CreateLesson(
        int number,
        string title,
        VerbSeed verb,
        params VocabSeed[] vocabulary)
    {
        var lessonId = Guid.Parse($"22222222-2222-2222-2222-{number:000000000000}");
        var baseQuestionId = number * 100 + 1;
        var questions = vocabulary
            .Select(item => new QuestionSeed(
                QuizCategory.Vocabulary,
                item.Prompt,
                [item.Answer, item.DistractorOne, item.DistractorTwo],
                item.Answer,
                "این گزینه با معنی و کاربرد جمله سازگار است."))
            .Concat(
            [
                new(QuizCategory.Grammar, $"„Ich ___ .“ – Form von {verb.Infinitive}:", [verb.Ich, verb.Du, verb.Wir], verb.Ich, $"با «ich» شکل درست فعل {verb.Infinitive}، «{verb.Ich}» است."),
                new(QuizCategory.Grammar, $"„Du ___ .“ – Form von {verb.Infinitive}:", [verb.Du, verb.Ich, verb.Wir], verb.Du, $"با «du» شکل درست فعل {verb.Infinitive}، «{verb.Du}» است."),
                new(QuizCategory.Grammar, $"„Er ___ .“ – Form von {verb.Infinitive}:", [verb.Er, verb.Ich, verb.Wir], verb.Er, $"با «er» شکل درست فعل {verb.Infinitive}، «{verb.Er}» است."),
                new(QuizCategory.Grammar, $"„Wir ___ .“ – Form von {verb.Infinitive}:", [verb.Wir, verb.Er, verb.Du], verb.Wir, $"با «wir» شکل درست فعل {verb.Infinitive}، «{verb.Wir}» است."),
                new(QuizCategory.Grammar, $"„Ihr ___ .“ – Form von {verb.Infinitive}:", [verb.Ihr, verb.Wir, verb.Ich], verb.Ihr, $"با «ihr» شکل درست فعل {verb.Infinitive}، «{verb.Ihr}» است."),
                new(QuizCategory.Grammar, $"„Sie ___ .“ – Form von {verb.Infinitive}:", [verb.Sie, verb.Ich, verb.Du], verb.Sie, $"با «Sie» شکل درست فعل {verb.Infinitive}، «{verb.Sie}» است."),
                new(QuizCategory.Grammar, $"Frage mit „du“: ___ du?", [verb.Du, verb.Ich, verb.Wir], verb.Du, "در پرسش بله/خیر، فعل قبل از فاعل می‌آید."),
                new(QuizCategory.Grammar, $"Frage mit „ihr“: ___ ihr?", [verb.Ihr, verb.Ich, verb.Wir], verb.Ihr, "در پرسش بله/خیر، فعل قبل از «ihr» می‌آید."),
                new(QuizCategory.Grammar, $"„Am Samstag ___ ich .“ – Form von {verb.Infinitive}:", [verb.Ich, verb.Du, verb.Er], verb.Ich, "فاعل «ich» به شکل اول شخص فعل نیاز دارد."),
                new(QuizCategory.Grammar, $"„Meine Freundin ___ .“ – Form von {verb.Infinitive}:", [verb.Er, verb.Ich, verb.Sie], verb.Er, "«Meine Freundin» سوم شخص مفرد است و با شکل «er/sie» می‌آید.")
            ])
            .ToList();

        return new QuizContentLesson(
            new Lesson(lessonId, "Starten wir!", "A1", number, title),
            questions.Select((question, index) => new QuizQuestion(
                Guid.Parse($"30000000-0000-0000-0000-{baseQuestionId + index:000000000000}"),
                lessonId,
                question.Category,
                QuestionType.MultipleChoice,
                question.Prompt,
                question.Options,
                question.CorrectAnswer,
                question.Explanation)).ToList());
    }

    private sealed record VocabSeed(
        string Prompt,
        string Answer,
        string DistractorOne,
        string DistractorTwo);

    private sealed record VerbSeed(
        string Infinitive,
        string Ich,
        string Du,
        string Er,
        string Wir,
        string Ihr,
        string Sie)
    {
    }

    private sealed record QuestionSeed(
        QuizCategory Category,
        string Prompt,
        string[] Options,
        string CorrectAnswer,
        string Explanation);

    private static VocabSeed V(
        string prompt,
        string answer,
        string distractorOne,
        string distractorTwo) =>
        new(prompt, answer, distractorOne, distractorTwo);
}
