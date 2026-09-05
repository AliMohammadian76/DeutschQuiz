using DeutschQuiz.Domain;

namespace DeutschQuiz.Application;

/// <summary>
/// Independent quiz catalogue for Starten wir! A2 (12 Lektionen).
/// Prompts are original for DeutschQuiz and are not copied from the textbook.
/// </summary>
public static class StartenWirA2QuizContentCatalog
{
    public static IReadOnlyList<QuizContentLesson> Lessons { get; } =
    [
        CreateLesson(1, "Alltag und Gewohnheiten",
            new VerbSeed("aufstehen", "stehe auf", "stehst auf", "steht auf", "stehen auf", "steht auf", "stehen auf"),
            V("Eine Gewohnheit macht man ...", "regelmäßig", "nur einmal im Leben", "nie"),
            V("Am Morgen ___ viele Leute früh auf.", "stehen", "kaufen", "schreiben"),
            V("Vor der Arbeit trinkt man oft ...", "Kaffee", "Benzin", "Sand"),
            V("„Täglich“ bedeutet ...", "jeden Tag", "einmal im Jahr", "nur sonntags"),
            V("Nach der Arbeit hat man oft ...", "Feierabend", "Frühstück", "Schulbeginn"),
            V("Am Wochenende ist der Alltag oft ...", "anders", "genau wie Montag immer", "nur Grammatik"),
            V("Zähne putzen gehört zur ...", "Hygiene", "Adresse", "Fahrkarte"),
            V("Wer den Haushalt macht, ...", "räumt auf", "fliegt ab", "parked nur"),
            V("Eine Pause braucht man, um ...", "sich zu erholen", "einen Pass zu essen", "nur zu schreien"),
            V("„Wie oft?“ fragt nach der ...", "Häufigkeit", "Farbe", "Adresse")),

        CreateLesson(2, "Wohnen und Nachbarn",
            new VerbSeed("einziehen", "ziehe ein", "ziehst ein", "zieht ein", "ziehen ein", "zieht ein", "ziehen ein"),
            V("Wenn man in eine neue Wohnung kommt, ___ man ein.", "zieht", "kocht", "singt"),
            V("Ein Nachbar wohnt ...", "nebenan", "im Flugzeug", "nur im Pass"),
            V("Die Miete bezahlt man für ...", "die Wohnung", "das Wetter", "die Farbe"),
            V("Im Mehrfamilienhaus teilt man oft den ...", "Flur", "Pass", "Himmel"),
            V("Ruhige Nachbarn sind oft ...", "angenehm", "laut wie ein Konzert immer", "nur teuer"),
            V("„Darf ich laut Musik hören?“ ist eine Frage an die ...", "Nachbarn", "Ampel", "Zeitung"),
            V("Eine Kaution zahlt man oft beim ...", "Einzug", "Frühstück", "Spaziergang"),
            V("Der Hausmeister hilft bei Problemen im ...", "Haus", "Meer", "Pass"),
            V("Mülltrennung ist wichtig für die ...", "Umwelt", "Jacke", "Uhrzeit"),
            V("Ein Balkon ist gut zum ...", "Sitzen draußen", "Schwimmen im Keller", "Fliegen")),

        CreateLesson(3, "Essen und Ernährung",
            new VerbSeed("kochen", "koche", "kochst", "kocht", "kochen", "kocht", "kochen"),
            V("Gesunde Ernährung enthält oft ...", "Obst und Gemüse", "nur Süßigkeiten", "nur Softdrinks"),
            V("Vegetarisch bedeutet ohne ...", "Fleisch", "Wasser", "Brot"),
            V("Im Restaurant bestellt man von der ...", "Speisekarte", "Landkarte", "Visitenkarte"),
            V("„Ich bin allergisch gegen Nüsse“ warnt vor ...", "Nahrungsmitteln", "Farben", "Wochentagen"),
            V("Zu viel Zucker ist oft ...", "ungesund", "sehr sportlich", "nötig für Ampeln"),
            V("Ein Rezept erklärt, wie man etwas ...", "kocht", "fährt", "mietet"),
            V("Bio-Lebensmittel kommen oft aus ...", "ökologischer Landwirtschaft", "nur dem Flughafen", "nur dem Pass"),
            V("Zum Abendessen isst man oft ...", "warm", "nur Schuhe", "nur Tickets"),
            V("„Guten Appetit!“ sagt man vor dem ...", "Essen", "Schlafen im Bus", "Parken"),
            V("Wer diätet, achtet auf die ...", "Kalorien", "Fahrpläne", "Postleitzahlen")),

        CreateLesson(4, "Arbeit und Ausbildung",
            new VerbSeed("lernen", "lerne", "lernst", "lernt", "lernen", "lernt", "lernen"),
            V("Eine Ausbildung bereitet auf einen ... vor.", "Beruf", "Urlaub nur", "Pass"),
            V("Ein Praktikum hilft, ... zu sammeln.", "Erfahrung", "Schnee", "Ampeln"),
            V("Die Bewerbung schickt man für eine ...", "Stelle", "Pizza", "Jacke"),
            V("Im Büro arbeitet man oft am ...", "Computer", "Herd", "Strand"),
            V("Ein Kollege arbeitet im gleichen ...", "Team", "Bett", "Koffer"),
            V("Gehalt ist die ... für Arbeit.", "Bezahlung", "Farbe", "Jahreszeit"),
            V("Wer studiert, ist oft an einer ...", "Universität", "Bäckerei nur", "Ampel"),
            V("Eine Weiterbildung verbessert die ...", "Qualifikation", "Wohnungsnummer", "Schuhgröße"),
            V("„Was machst du beruflich?“ fragt nach dem ...", "Job", "Wetter", "Frühstück"),
            V("Arbeitslos bedeutet ...", "ohne Job", "sehr reich", "immer im Urlaub")),

        CreateLesson(5, "Freizeit und Kultur",
            new VerbSeed("besuchen", "besuche", "besuchst", "besucht", "besuchen", "besucht", "besuchen"),
            V("Im Museum kann man ...", "Kunst anschauen", "Autos tanken", "Zähne ziehen"),
            V("Ein Konzert ist eine ... Veranstaltung.", "musikalische", "nur gesetzliche", "nur postale"),
            V("Im Kino sieht man einen ...", "Film", "Pass", "Herd"),
            V("Ein Hobby macht man in der ...", "Freizeit", "Arbeitszeit nur zwangsweise", "Prüfung immer"),
            V("Ein Festival dauert oft mehrere ...", "Tage", "Sekunden nur", "Passwörter"),
            V("Theaterstücke sieht man im ...", "Theater", "Kühlschrank", "Briefkasten"),
            V("Wer gern liest, geht in die ...", "Bibliothek", "Tankstelle als Lesesaal", "Ampel"),
            V("„Hast du Lust auf ...?“ schlägt Freizeit vor.", "etwas Gemeinsames", "nur Miete", "nur Formulare"),
            V("Ein Stadtfest ist oft ...", "kostenlos oder günstig", "nur im Pass", "nur nachts im Keller ohne Licht immer"),
            V("Kultur verbindet oft ...", "Menschen", "nur Schuhe", "nur Tickets ohne Leute")),

        CreateLesson(6, "Reisen und Verkehr",
            new VerbSeed("ankommen", "komme an", "kommst an", "kommt an", "kommen an", "kommt an", "kommen an"),
            V("Am Bahnhof ___ der Zug an.", "kommt", "kocht", "mietet"),
            V("Für die Bahn braucht man oft ein ...", "Ticket", "Kissen als Fahrkarte", "Rezept"),
            V("Verspätung bedeutet: der Zug kommt ...", "später", "früher immer", "nie wieder pünktlich garantiert immer"),
            V("Am Flughafen muss man vor dem Flug ...", "einchecken", "duschen im Cockpit", "den Pass essen"),
            V("Öffentliche Verkehrsmittel sind z. B. ...", "Bus und Bahn", "nur Privatejets", "nur Fahrräder im Meer"),
            V("Eine Umleitung ändert den ...", "Weg", "Namen", "Geburtstag"),
            V("Im Stau steht man mit dem ...", "Auto", "Löffel", "Pass"),
            V("„Wann kommt der Bus?“ fragt nach der ...", "Abfahrt", "Farbe der Jacke", "Miete"),
            V("Eine Fahrkarte gilt für eine ...", "Strecke oder Zeit", "nur Farbe", "nur Familie ohne Fahrt"),
            V("Zu Fuß gehen ist umweltfreundlich und ...", "gesund", "nur teuer immer", "nur laut")),

        CreateLesson(7, "Gesundheit und Sport",
            new VerbSeed("trainieren", "trainiere", "trainierst", "trainiert", "trainieren", "trainiert", "trainieren"),
            V("Wer Sport macht, ___ oft im Studio.", "trainiert", "kocht nur Formulare", "parkt nur"),
            V("Bei starken Schmerzen geht man zum ...", "Arzt", "Bäcker als Therapie", "Fahrplan"),
            V("Ausdauer trainiert man z. B. durch ...", "Joggen", "Nur Fernsehen", "Nur Sitzen"),
            V("Aufwärmen macht man ... dem Training.", "vor", "nur nach dem Schlafen im Pass", "statt"),
            V("Eine Verletzung braucht oft ...", "Ruhe", "mehr Lärm", "weniger Wasser immer falsch"),
            V("Gesund leben heißt oft: ...", "sich bewegen und gut essen", "nur Süßigkeiten", "nie schlafen"),
            V("Im Fitnessstudio gibt es oft ...", "Geräte", "nur Ampeln", "nur Fahrpläne"),
            V("Dehnen hilft den ...", "Muskeln", "Tickets", "Postleitzahlen"),
            V("„Ich fühle mich fit“ bedeutet: ich bin in ...", "guter Form", "schlechter Laune nur", "nur dem Pass"),
            V("Team sport macht man mit ...", "anderen", "niemandem je", "nur dem Kühlschrank")),

        CreateLesson(8, "Einkaufen und Dienstleistungen",
            new VerbSeed("bezahlen", "bezahle", "bezahlst", "bezahlt", "bezahlen", "bezahlt", "bezahlen"),
            V("An der Kasse ___ man die Ware.", "bezahlt", "wohnt", "singt"),
            V("Eine Dienstleistung ist z. B. ein ...", "Haarschnitt", "Apfel", "Stein"),
            V("Im Online-Shop bestellt man oft mit ...", "Karte oder Rechnung", "nur Schnee", "nur Ampeln"),
            V("„Haben Sie das in Größe M?“ fragt nach ...", "Kleidung", "Fahrplänen", "Wetter"),
            V("Ein Umtausch ist möglich mit dem ...", "Beleg", "Passwort der Nachbarn", "Frühstück"),
            V("Der Kundenservice hilft bei ...", "Problemen", "nur dem Wetter ändern", "nur dem Mond"),
            V("Bargeld sind ...", "Münzen und Scheine", "nur Apps ohne Geld", "nur Tickets"),
            V("Eine Reparatur macht man, wenn etwas ...", "kaputt ist", "neu und perfekt ist immer", "nur grün ist"),
            V("Im Salon wäscht man oft die ...", "Haare", "Fahrkarten", "Mieten"),
            V("„Das ist zu teuer“ spricht über den ...", "Preis", "Namen", "Wochentag")),

        CreateLesson(9, "Medien und Kommunikation",
            new VerbSeed("schreiben", "schreibe", "schreibst", "schreibt", "schreiben", "schreibt", "schreiben"),
            V("Eine Nachricht ___ man per Chat.", "schreibt", "kocht", "mietet"),
            V("Soziale Medien dienen oft zum ...", "Teilen und Kommentieren", "Nur Schlafen", "Nur Parken"),
            V("Ein Podcast ist eine ... Datei.", "Audio-", "nur steinerne", "nur postale ohne Ton"),
            V("Fake News sind ... Nachrichten.", "falsche", "immer wahre", "nur wetterbezogene garantiert"),
            V("Per E-Mail schickt man oft ...", "Dokumente", "Betten", "Ampeln"),
            V("Videoanrufe braucht man für ...", "Gespräche auf Distanz", "nur Kochen", "nur Müll"),
            V("Ein Emoticon zeigt oft eine ...", "Stimmung", "Adresse", "Hausnummer nur"),
            V("„Bist du online?“ fragt nach der ...", "Erreichbarkeit", "Schuhgröße", "Miete"),
            V("Datenschutz schützt private ...", "Informationen", "Bananen", "Jackenfarben ohne Sinn"),
            V("Ein Blogbeitrag ist ein Text im ...", "Internet", "Kühlschrank", "Briefkasten aus Stein")),

        CreateLesson(10, "Natur und Umwelt",
            new VerbSeed("schützen", "schütze", "schützt", "schützt", "schützen", "schützt", "schützen"),
            V("Wir sollten die Umwelt ...", "schützen", "zerstören absichtlich", "ignorieren immer"),
            V("Recycling bedeutet: Abfall ...", "wiederverwerten", "überall hinwerfen", "essen"),
            V("Erneuerbare Energie kommt z. B. von der ...", "Sonne", "nur vom Pass", "nur von Ampeln"),
            V("Plastikmüll ist schlecht für die ...", "Meere", "Grammatikregeln", "Fahrpläne"),
            V("Wer den Wasserhahn zudreht, spart ...", "Wasser", "Farben", "Namen"),
            V("Ein Nationalpark schützt die ...", "Natur", "nur Einkaufszentren", "nur Parkplätze"),
            V("CO₂ entsteht oft durch ...", "Verkehr und Industrie", "nur Lesen", "nur Lachen"),
            V("Nachhaltig leben heißt: ...", "Ressourcen schonen", "alles wegwerfen", "nur laut sein"),
            V("Bäume produzieren ...", "Sauerstoff", "Tickets", "Miete"),
            V("„Bitte nicht littering“ bedeutet: keinen Müll ...", "wegwerfen", "sortieren richtig", "recyceln")),

        CreateLesson(11, "Feste und Traditionen",
            new VerbSeed("feiern", "feiere", "feierst", "feiert", "feiern", "feiert", "feiern"),
            V("Geburtstag ___ man oft mit Freunden.", "feiert", "parkt", "tanked"),
            V("Zu Weihnachten schenkt man oft ...", "Geschenke", "Ampeln", "Fahrpläne"),
            V("Eine Tradition wiederholt man ...", "regelmäßig über Jahre", "nur einmal und nie wieder zwingend", "nur im Pass"),
            V("Silvester ist am ...", "31. Dezember", "1. Mai nur", "Ostermontag immer"),
            V("Auf einer Hochzeit sind oft viele ...", "Gäste", "Züge", "Rezepte ohne Leute"),
            V("Karneval ist in manchen Regionen sehr ...", "bunt", "leise wie eine Bibliothek immer", "nur digital ohne Menschen"),
            V("Ein Festessen ist ein besonderes ...", "Mahl", "Ticket", "Passwort"),
            V("Glückwünsche sagt man zum ...", "Geburtstag", "Müll trennen", "Parken"),
            V("Ein Feiertag ist oft ...", "arbeitsfrei", "nur im Keller", "nur nachts ohne Datum"),
            V("Familienfeste stärken oft die ...", "Beziehungen", "Staus", "Drucker")),

        CreateLesson(12, "Pläne und Wünsche",
            new VerbSeed("planen", "plane", "planst", "plant", "planen", "plant", "planen"),
            V("Für den Urlaub ___ man Termine und Geld.", "plant", "kocht ohne Sinn", "mietet nur den Himmel"),
            V("Ein Wunsch ist etwas, das man ...", "gern hätte", "schon hasst", "nie braucht und trotzdem hasst"),
            V("„Ich möchte ...“ drückt einen ... aus.", "Wunsch", "Pass", "Fahrplan ohne Sinn"),
            V("Ziele setzt man sich für die ...", "Zukunft", "Vergangenheit nur", "Jackenfarbe"),
            V("Eine To-do-Liste hilft beim ...", "Organisieren", "Vergessen absichtlich", "nur Schreien"),
            V("Wenn etwas klappt, war der Plan ...", "erfolgreich", "nutzlos immer", "nur nass"),
            V("Träume kann man manchmal ...", "verwirklichen", "essen", "parken"),
            V("„Was machst du am Wochenende?“ fragt nach ...", "Plänen", "der Schuhgröße", "nur dem Pass"),
            V("Flexibel sein heißt: Pläne ... können.", "ändern", "nie anpassen", "nur zerstören"),
            V("Ein Termin im Kalender ist ein fester ...", "Plan", "Baum", "Song ohne Datum"))
    ];

    private static QuizContentLesson CreateLesson(
        int number,
        string title,
        VerbSeed verb,
        params VocabSeed[] vocabulary)
    {
        var lessonId = Guid.Parse($"42424242-4242-4242-4242-{number:000000000000}");
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
            new Lesson(lessonId, "Starten wir!", "A2", number, title),
            questions.Select((question, index) => new QuizQuestion(
                Guid.Parse($"31000000-0000-0000-0000-{baseQuestionId + index:000000000000}"),
                lessonId,
                question.Category,
                QuestionType.MultipleChoice,
                question.Prompt,
                question.Options,
                question.CorrectAnswer,
                question.Explanation)).ToList());
    }

    private sealed record VocabSeed(string Prompt, string Answer, string DistractorOne, string DistractorTwo);
    private sealed record VerbSeed(string Infinitive, string Ich, string Du, string Er, string Wir, string Ihr, string Sie);
    private sealed record QuestionSeed(QuizCategory Category, string Prompt, string[] Options, string CorrectAnswer, string Explanation);

    private static VocabSeed V(string prompt, string answer, string distractorOne, string distractorTwo) =>
        new(prompt, answer, distractorOne, distractorTwo);
}
