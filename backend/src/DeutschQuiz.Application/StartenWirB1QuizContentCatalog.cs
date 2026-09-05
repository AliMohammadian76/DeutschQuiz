using DeutschQuiz.Domain;

namespace DeutschQuiz.Application;

/// <summary>
/// Independent quiz catalogue for Starten wir! B1 (12 Lektionen).
/// Prompts are original for DeutschQuiz and are not copied from the textbook.
/// </summary>
public static class StartenWirB1QuizContentCatalog
{
    public static IReadOnlyList<QuizContentLesson> Lessons { get; } =
    [
        CreateLesson(1, "Biografie und Identität",
            new VerbSeed("erzählen", "erzähle", "erzählst", "erzählt", "erzählen", "erzählt", "erzählen"),
            V("Eine Biografie ___ das Leben einer Person.", "erzählt", "kocht", "parkt"),
            V("Identität beschreibt, wer man ...", "ist", "nur mietet", "nur tankt"),
            V("Herkunft meint oft das Land oder die ...", "Familie", "Ampel", "Fahrkarte"),
            V("Werte sind Dinge, die einem ... sind.", "wichtig", "egal und nutzlos immer", "nur teuer ohne Sinn"),
            V("Ein Lebenslauf listet Ausbildung und ...", "Berufserfahrung", "Lieblingsfarben nur", "Schuhgrößen ohne Job"),
            V("„Woher kommst du ursprünglich?“ fragt nach der ...", "Herkunft", "Uhrzeit", "Miete"),
            V("Mehrsprachigkeit bedeutet: mehrere ... sprechen.", "Sprachen", "Betten", "Ampeln"),
            V("Persönliche Stärken helfen bei der ...", "Selbstdarstellung", "Mülltonne", "Parkuhr"),
            V("Ein Porträt zeigt oft den ... einer Person.", "Charakter", "Fahrplan", "Postcode ohne Sinn"),
            V("Erinnerungen gehören zur eigenen ...", "Geschichte", "Jackengröße nur", "Benzinuhr")),

        CreateLesson(2, "Studium und Beruf",
            new VerbSeed("bewerben", "bewerbe", "bewirbst", "bewirbt", "bewerben", "bewerbt", "bewerben"),
            V("Um eine Stelle zu bekommen, ___ man sich.", "bewirbt", "kocht", "schwimmt nur"),
            V("Ein Studium findet oft an der ... statt.", "Universität", "Tankstelle als Campus", "Ampel"),
            V("Praktika verbessern die Chancen auf dem ...", "Arbeitsmarkt", "Wetterbericht", "Menü"),
            V("Soft Skills sind z. B. ...", "Kommunikation und Teamfähigkeit", "nur Schuhgröße", "nur Haarfarbe"),
            V("Ein Vorstellungsgespräch prüft die ...", "Eignung", "Parkgebühr", "Postleitzahl"),
            V("Weiterbildung hält Wissen ...", "aktuell", "absichtlich veraltet", "nur nass"),
            V("Homeoffice bedeutet Arbeit von ...", "zu Hause", "nur vom Meer aus zwingend", "nur vom Mond"),
            V("Eine Deadline ist ein fester ...", "Termin", "Baum", "Song"),
            V("Networking knüpft berufliche ...", "Kontakte", "Schleifen im Pass", "Ampeln"),
            V("Berufliche Ziele plant man langfristig für die ...", "Karriere", "Jackenwäsche nur", "Mittagspause ohne Plan")),

        CreateLesson(3, "Gesellschaft und Zusammenleben",
            new VerbSeed("respektieren", "respektiere", "respektierst", "respektiert", "respektieren", "respektiert", "respektieren"),
            V("Im Zusammenleben sollte man andere ...", "respektieren", "beleidigen absichtlich", "ignorieren immer hart"),
            V("Toleranz bedeutet: Vielfalt ...", "akzeptieren", "löschen", "bestrafen ohne Grund"),
            V("Nachbarn teilen oft den gleichen ...", "Wohnort", "Passwort", "Geburtstag zwingend"),
            V("Ehrenamt hilft der ...", "Gemeinschaft", "nur dem Stau", "nur dem Drucker"),
            V("Regeln im Haus schützen den ...", "Frieden", "Lärmrekord", "Chaosplan"),
            V("Integration braucht oft Sprache und ...", "Kontakt", "nur Isolation", "nur Fernsehen ohne Leute"),
            V("Konflikte löst man besser durch ...", "Gespräch", "Schreien ohne Pause", "Ignorieren ewig"),
            V("Gleichberechtigung fordert gleiche ...", "Rechte", "Schuhmarken", "Haarfarben"),
            V("Ein Verein organisiert gemeinsame ...", "Aktivitäten", "Einsamkeiten", "Staus"),
            V("Höflichkeit erleichtert den ...", "Alltag", "Krieg", "Lärm")),

        CreateLesson(4, "Medien und Meinung",
            new VerbSeed("diskutieren", "diskutiere", "diskutierst", "diskutiert", "diskutieren", "diskutiert", "diskutieren"),
            V("Über Nachrichten kann man kontrovers ...", "diskutieren", "schweigen ewig ohne Grund", "parken"),
            V("Eine Meinung ist eine persönliche ...", "Einschätzung", "Fahrkarte", "Wohnungsnummer"),
            V("Quellen prüfen schützt vor ...", "Desinformation", "gesunder Ernährung", "Sport"),
            V("Kommentare unter Artikeln zeigen oft ...", "Reaktionen", "nur Wetterdaten ohne Text", "nur Mieten"),
            V("Pressefreiheit ist wichtig für die ...", "Demokratie", "Kochrezepte nur", "Schuhmode"),
            V("Ein Interview stellt gezielte ...", "Fragen", "Betten", "Ampeln"),
            V("Hate Speech verletzt und sollte man ...", "melden", "teilen freudig", "ignorieren als Lob"),
            V("Fact-Checking prüft, ob etwas ... ist.", "wahr", "nur bunt", "nur laut"),
            V("Algorithmen filtern oft, was man ...", "sieht", "kocht", "mietet ohne Screen"),
            V("Eine Debatte braucht Argumente und ...", "Respekt", "nur Beleidigungen", "nur Lärm")),

        CreateLesson(5, "Konsum und Geld",
            new VerbSeed("sparen", "spare", "sparst", "spart", "sparen", "spart", "sparen"),
            V("Für größere Anschaffungen sollte man ...", "sparen", "alles sofort ausgeben sinnlos", "Schulden feiern"),
            V("Ein Budget plant Einnahmen und ...", "Ausgaben", "Farben", "Namen"),
            V("Konsum bedeutet oft: Dinge ...", "kaufen und nutzen", "nur anschauen ewig ohne Nutzen", "wegwerfen ungelesen"),
            V("Nachhaltiger Konsum achtet auf ...", "Qualität und Umwelt", "nur Billigstware immer", "nur Verpackungsmüll maximieren"),
            V("Zinsen zahlt man oft bei einem ...", "Kredit", "Spaziergang", "Lied"),
            V("Online-Shopping ist bequem, aber braucht ...", "Vorsicht", "keine Adresse je", "nur Glück"),
            V("Ein Konto verwaltet das ...", "Geld", "Wetter", "Frühstück"),
            V("Werbung will den ... anregen.", "Kauf", "Schlaf", "Müll sortieren"),
            V("„Das lohnt sich“ meint: es ist den ... wert.", "Preis", "Pass", "Wochentag"),
            V("Schuldner haben oft zu hohe ...", "Verbindlichkeiten", "Urlaubstage nur positiv", "Muskelkraft")),

        CreateLesson(6, "Umwelt und Nachhaltigkeit",
            new VerbSeed("vermeiden", "vermeide", "vermeidest", "vermeidet", "vermeiden", "vermeidet", "vermeiden"),
            V("Einwegplastik sollte man möglichst ...", "vermeiden", "sammeln als Hobby sinnlos", "essen"),
            V("Nachhaltigkeit denkt an heutige und ... Generationen.", "künftige", "nur vergangene", "nur digitale ohne Menschen"),
            V("Der ökologische Fußabdruck misst ...", "Umweltbelastung", "Schuhgröße wörtlich nur", "Parkgebühren"),
            V("Öffentliche Verkehrsmittel schonen oft das ...", "Klima", "Passwort", "Frühstück"),
            V("Mülltrennung ist ein Schritt zur ...", "Kreislaufwirtschaft", "Chaosparty", "Lautstärke"),
            V("Erneuerbare Energien ersetzen teilweise ...", "Fossilien", "Bücher", "Sport"),
            V("Greenwashing täuscht mit angeblich grünen ...", "Versprechen", "Bäumen die man pflanzt echt", "echten Maßnahmen immer"),
            V("Wasser sparen hilft in Zeiten der ...", "Knappheit", "Übersättigung mit Regen ewig", "Partys"),
            V("Lokale Produkte verkürzen oft Transport-...", "wege", "namen", "farben"),
            V("Klimaschutz braucht kollektives ...", "Handeln", "Schweigen absolut", "Ignorieren")),

        CreateLesson(7, "Kultur und Freizeit",
            new VerbSeed("genießen", "genieße", "genießt", "genießt", "genießen", "genießt", "genießen"),
            V("Freie Zeit sollte man bewusst ...", "genießen", "verschwenden absichtlich immer", "hassen"),
            V("Ein Kulturangebot kann Theater, Konzert oder ... sein.", "Ausstellung", "nur Stau", "nur Formular"),
            V("Work-Life-Balance sucht Ausgleich zwischen Job und ...", "Privatleben", "nur Überstunden", "nur E-Mails nachts"),
            V("Hobbys reduzieren oft ...", "Stress", "Freude", "Freunde"),
            V("Ein Lesekreis diskutiert gemeinsam ...", "Bücher", "Ampeln", "Parkuhren"),
            V("Streetfood gehört zur urbanen ...", "Kultur", "Steuererklärung", "Zahnmedizin"),
            V("Ein Festival verbindet Musik und ...", "Gemeinschaft", "Isolation", "Stille ohne Menschen immer"),
            V("Kreativität zeigt sich in Kunst, Design oder ...", "Schreiben", "nur Formularfeldern", "nur Stempelkarten"),
            V("Entspannungstechniken helfen bei ...", "Anspannung", "mehr Stress erzeugen", "Lautstärkerekorden"),
            V("Freizeitgestaltung ist oft eine Frage der ...", "Prioritäten", "Schuhmarke nur", "Haarfarbe")),

        CreateLesson(8, "Reisen und Mobilität",
            new VerbSeed("entdecken", "entdecke", "entdeckst", "entdeckt", "entdecken", "entdeckt", "entdecken"),
            V("Auf Reisen kann man neue Orte ...", "entdecken", "löschen", "vermieten als Pass"),
            V("Mobilität beschreibt, wie man sich ...", "fortbewegt", "nur hinsetzt ewig", "nur schläft"),
            V("Carsharing teilt ein ...", "Auto", "Frühstück", "Passwort der Bank"),
            V("Bahnreisen sind oft klimafreundlicher als ...", "Kurzstreckenflüge", "Spaziergänge", "Lesen"),
            V("Eine Packliste verhindert, etwas zu ...", "vergessen", "finden extra", "kaufen doppelt absichtlich sinnvoll"),
            V("Interrail ist ein Ticket für viele ...", "Länder", "Zimmer im Pass", "Ampeln"),
            V("Digitale Tickets speichert man oft im ...", "Smartphone", "Kühlschrank", "Briefkasten aus Stein"),
            V("Barrierefreiheit erleichtert Reisen für ...", "alle", "niemanden je", "nur Roboter"),
            V("Jetlag spürt man nach langen ...", "Flügen", "Spaziergängen um den Block", "E-Mails"),
            V("Nachhaltig reisen heißt: bewusst und ...", "ressourcenschonend", "maximal fliegen immer", "Müll hinterlassen")),

        CreateLesson(9, "Gesundheit und Lebensstil",
            new VerbSeed("achten", "achte", "achtest", "achtet", "achten", "achtet", "achten"),
            V("Auf die Gesundheit sollte man im Alltag ...", "achten", "pfeifen", "vergessen absichtlich"),
            V("Schlafqualität beeinflusst Stimmung und ...", "Konzentration", "nur Schuhfarbe", "nur Postleitzahl"),
            V("Mental health meint die ... Gesundheit.", "psychische", "nur zahnmedizinische ohne Kopf", "nur finanzielle immer"),
            V("Ausgewogene Ernährung liefert ...", "Nährstoffe", "nur Lärm", "nur Stress"),
            V("Bewegungsmangel erhöht langfristig ...", "Risiken", "Glück garantiert", "Urlaubstage magisch"),
            V("Vorsorgeuntersuchungen erkennt man früh ...", "Probleme", "Geschenke", "Partytermine"),
            V("Suchtprävention warnt vor schädlichen ...", "Gewohnheiten", "Hobbys die guttun", "Büchern"),
            V("Workaholism beschreibt übermäßiges ...", "Arbeiten", "Schlafen", "Lachen"),
            V("Entschleunigung bedeutet: Tempo ...", "reduzieren", "verdoppeln", "ignorieren und rasen"),
            V("Ein gesunder Lebensstil verbindet Körper und ...", "Geist", "Drucker", "Stau")),

        CreateLesson(10, "Technik und Digitalisierung",
            new VerbSeed("nutzen", "nutze", "nutzt", "nutzt", "nutzen", "nutzt", "nutzen"),
            V("Digitale Tools ___ wir täglich.", "nutzen", "kochen", "mieten als Wetter"),
            V("Künstliche Intelligenz analysiert oft große ...", "Datenmengen", "Bananenkisten nur", "Schuhregale"),
            V("Datenschutz schützt vor unerlaubtem ...", "Zugriff", "Lesen von Romanen", "Sport"),
            V("Ein Update verbessert Software oder schließt ...", "Sicherheitslücken", "Fenster im Haus wörtlich immer", "Freundeskreise"),
            V("Cloud-Dienste speichern Dateien ...", "online", "nur auf Papier steinzeitlich zwingend", "nur im Kühlschrank"),
            V("Phishing versucht, Zugangsdaten zu ...", "stehlen", "schenken ehrlich", "drucken dekorativ"),
            V("Smart Home steuert Geräte im ...", "Haushalt", "Wald ohne Strom", "Meer"),
            V("Digitale Kompetenz braucht man im ...", "Beruf und Alltag", "nur Mittelalter", "nur ohne Geräte"),
            V("Algorithmen entscheiden mit, was im Feed ...", "erscheint", "kocht", "regnet"),
            V("Offline-Zeiten helfen gegen digitale ...", "Überlastung", "Erholung maximal falsch", "Fitness ohne Pause")),

        CreateLesson(11, "Politik und Engagement",
            new VerbSeed("wählen", "wähle", "wählst", "wählt", "wählen", "wählt", "wählen"),
            V("In einer Demokratie darf man Parteien ...", "wählen", "verbieten ohne Grund privat", "essen"),
            V("Engagement zeigt sich in Protest, Petition oder ...", "Ehrenamt", "nur Schweigen absolut", "nur Konsum ohne Haltung"),
            V("Wahlbeteiligung misst, wie viele Menschen ...", "wählen gehen", "schlafen am Wahltag nur", "reisen ohne Wahlrecht nutzen"),
            V("Meinungsfreiheit schützt öffentliche ...", "Äußerungen", "Diebstähle", "Lügen als Pflicht"),
            V("Eine Petition sammelt ... für ein Anliegen.", "Unterschriften", "Steine", "Ampeln"),
            V("Kommunalpolitik betrifft die eigene ...", "Stadt oder Gemeinde", "Galaxie nur", "Tiefsee"),
            V("Rechte und Pflichten gehören zum ...", "Bürgerstatus", "Frühstücksmenü", "Parkschein ohne Recht"),
            V("Korruption schadet dem ...", "Gemeinwohl", "Wetter", "Sportrekord positiv"),
            V("Jugendliche können sich in Projekten ...", "einbringen", "ausschließen ewig", "wegducken als Ideal"),
            V("Medienkompetenz hilft, politische Infos zu ...", "einordnen", "löschen blind", "ignorieren komplett")),

        CreateLesson(12, "Zukunft und Ziele",
            new VerbSeed("erreichen", "erreiche", "erreichst", "erreicht", "erreichen", "erreicht", "erreichen"),
            V("Wer plant, will Ziele ...", "erreichen", "vermeiden absichtlich", "vergessen feiern"),
            V("Langfristige Ziele brauchen ...", "Ausdauer", "nur Glück ohne Arbeit", "nur Zufall ewig"),
            V("Ein Meilenstein markiert einen wichtigen ...", "Fortschritt", "Rückschritt geplant", "Stau"),
            V("Scheitern kann man als Chance zum ... sehen.", "Lernen", "Aufgeben ewig", "Schämen ohne Ende"),
            V("Visionen beschreiben eine gewünschte ...", "Zukunft", "Vergangenheit nur", "Parkuhr"),
            V("Priorisieren heißt: Wichtiges zuerst ...", "erledigen", "aufschieben ewig", "löschen"),
            V("Mentoren unterstützen auf dem ...", "Weg", "Sofa ohne Rat", "Mond ohne Funk"),
            V("Flexibilität hilft, wenn Pläne sich ...", "ändern", "nie bewegen", "in Stein meißeln müssen"),
            V("Erfolg misst jeder etwas ...", "anders", "identisch wie Roboter immer", "nur in Euro zwingend"),
            V("Hoffnung motiviert, weiter zu ...", "machen", "stoppen absolut", "aufgeben sofort"))
    ];

    private static QuizContentLesson CreateLesson(
        int number,
        string title,
        VerbSeed verb,
        params VocabSeed[] vocabulary)
    {
        var lessonId = Guid.Parse($"52525252-5252-5252-5252-{number:000000000000}");
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
            new Lesson(lessonId, "Starten wir!", "B1", number, title),
            questions.Select((question, index) => new QuizQuestion(
                Guid.Parse($"32000000-0000-0000-0000-{baseQuestionId + index:000000000000}"),
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
