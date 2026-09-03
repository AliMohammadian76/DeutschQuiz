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
            V("Wie geht es dir?", ["Wie geht's?", "Wo wohnst du?", "Wie heißt du?"], "Wie geht's?", "Wie geht's? fragt nach dem Befinden."),
            V("Ergänze: Ich ___ Ali.", ["bin", "bist", "sind"], "bin", "Mit ich verwenden wir bin."),
            G("___ heißt du?", ["Wie", "Wo", "Was"], "Wie", "Die Frage nach dem Namen beginnt mit Wie."),
            G("Ich ___ aus dem Iran.", ["komme", "kommst", "kommen"], "komme", "Die Form von kommen für ich ist komme."),
            V("Welche Begrüßung passt am Morgen?", ["Guten Morgen!", "Gute Nacht!", "Bis morgen!"], "Guten Morgen!", "Guten Morgen sagt man am Anfang des Tages."),
            V("Was passt? – Wie geht es dir?", ["Gut, danke.", "Am Dienstag.", "In Köln."], "Gut, danke.", "So antwortet man auf die Frage nach dem Befinden."),
            V("Wo wohnst du?", ["In Teheran.", "Um acht Uhr.", "Sehr nett."], "In Teheran.", "Wo fragt nach einem Wohnort."),
            G("Du ___ sehr nett.", ["bist", "bin", "sind"], "bist", "Die Form von sein für du ist bist."),
            G("___ Sie Herr Meier?", ["Sind", "Bist", "Bin"], "Sind", "Die höfliche Frage mit Sie lautet Sind Sie ...?"),
            G("Wir ___ Deutsch.", ["lernen", "lernt", "lernst"], "lernen", "Bei wir endet das Verb auf -en.")),

        CreateLesson(2, "Familie und Freunde",
            V("Das ist meine ___.", ["Schwester", "Montag", "Wohnung"], "Schwester", "Schwester ist ein Familienwort."),
            V("Der Vater von meiner Mutter ist mein ___.", ["Großvater", "Bruder", "Sohn"], "Großvater", "Der Vater der Mutter ist der Großvater."),
            V("Wer ist die Tochter von deinen Eltern?", ["Meine Schwester", "Mein Onkel", "Mein Freund"], "Meine Schwester", "Die Tochter ist die Schwester oder man selbst."),
            V("Was passt zu Freundschaft?", ["zusammen lachen", "im Schrank schlafen", "eine Adresse essen"], "zusammen lachen", "Zusammen lachen ist eine Aktivität mit Freunden."),
            V("Wie fragst du nach einer Person?", ["Wer ist das?", "Wie viel Uhr?", "Wohin gehst du?"], "Wer ist das?", "Wer fragt nach einer Person."),
            G("Mein Bruder ___ in Berlin.", ["wohnt", "wohnen", "wohnst"], "wohnt", "Bei er lautet wohnen: wohnt."),
            G("Das ist ___ Mutter.", ["meine", "mein", "meinen"], "meine", "Mutter ist feminin: meine Mutter."),
            G("___ du Geschwister?", ["Hast", "Hat", "Haben"], "Hast", "Bei du lautet haben: hast."),
            G("Das sind ___ Eltern.", ["meine", "mein", "meinen"], "meine", "Eltern steht im Plural: meine Eltern."),
            G("Meine Freundin ___ sehr gut Deutsch.", ["spricht", "sprechen", "sprichst"], "spricht", "Bei sie lautet sprechen: spricht.")),

        CreateLesson(3, "Zahlen und Alltag",
            V("Welche Zahl kommt nach neun?", ["zehn", "acht", "zwölf"], "zehn", "Nach neun kommt zehn."),
            V("Der Kurs beginnt um ___.", ["acht Uhr", "rot", "Brot"], "acht Uhr", "Eine Uhrzeit passt in den Satz."),
            V("Was machst du am Montag?", ["Ich arbeite.", "Ich bin zwölf Uhr.", "Ich heiße Dienstag."], "Ich arbeite.", "Das ist eine passende Aktivität für einen Wochentag."),
            V("Welche Zahl ist 14?", ["vierzehn", "vierzig", "vier"], "vierzehn", "14 heißt auf Deutsch vierzehn."),
            V("Wann ist der Termin? – ___ Uhr.", ["Um zehn", "Aus Berlin", "Mit Anna"], "Um zehn", "Uhrzeiten beginnen oft mit um."),
            G("Ich ___ jeden Tag Deutsch.", ["lerne", "lernst", "lernen"], "lerne", "Bei ich lautet die Form lerne."),
            G("Wann ___ du?", ["arbeitest", "arbeitet", "arbeiten"], "arbeitest", "Bei du verwenden wir arbeitest."),
            G("Der Unterricht ___ um neun Uhr.", ["beginnt", "beginnen", "beginnst"], "beginnt", "Bei der Unterricht lautet beginnen: beginnt."),
            G("Heute ___ Dienstag.", ["ist", "sind", "bist"], "ist", "Heute ist ein einzelner Wochentag."),
            G("Wir ___ um sieben Uhr auf.", ["stehen", "steht", "stehst"], "stehen", "Bei wir lautet aufstehen: stehen ... auf.")),

        CreateLesson(4, "Essen und Trinken",
            V("Was trinkst du am Morgen?", ["Kaffee", "Stuhl", "Jacke"], "Kaffee", "Kaffee ist ein Getränk."),
            V("Ich möchte ein ___.", ["Wasser", "Fenster", "Buch"], "Wasser", "Wasser passt als Getränk."),
            V("Was bestellst du im Café?", ["einen Tee", "eine Hose", "ein Fahrrad"], "einen Tee", "Tee ist etwas zum Trinken."),
            V("Was ist Obst?", ["ein Apfel", "ein Tisch", "eine Tür"], "ein Apfel", "Ein Apfel ist Obst."),
            V("Was sagt man im Restaurant?", ["Die Rechnung, bitte.", "Die Adresse, schlafen.", "Der Montag, danke."], "Die Rechnung, bitte.", "So bittet man höflich um die Rechnung."),
            G("Wir ___ heute zusammen.", ["essen", "isst", "esst"], "essen", "Bei wir lautet essen: essen."),
            G("Du ___ gern Tee.", ["trinkst", "trinkt", "trinken"], "trinkst", "Bei du lautet trinken: trinkst."),
            G("Ich möchte ___ Kaffee.", ["einen", "eine", "ein"], "einen", "Kaffee ist maskulin: einen Kaffee."),
            G("Sie ___ kein Fleisch.", ["isst", "essen", "esse"], "isst", "Bei sie lautet essen: isst."),
            G("Wir ___ heute nicht zu Hause.", ["essen", "isst", "esst"], "essen", "Bei wir bleibt die Verbform essen.")),

        CreateLesson(5, "Wohnen",
            V("Wo schläfst du?", ["im Schlafzimmer", "im Supermarkt", "im Bus"], "im Schlafzimmer", "Das Schlafzimmer ist ein Raum in der Wohnung."),
            V("In der Küche steht ein ___.", ["Tisch", "Schuh", "Sommer"], "Tisch", "Ein Tisch kann in der Küche stehen."),
            V("Was gibt es im Badezimmer?", ["eine Dusche", "ein Sofa", "ein Fahrrad"], "eine Dusche", "Eine Dusche gehört ins Badezimmer."),
            V("Wie viele Zimmer hat die Wohnung?", ["Drei Zimmer.", "Drei Uhr.", "Drei Freunde."], "Drei Zimmer.", "Zimmer zählt die Räume einer Wohnung."),
            V("Was ist eine Adresse?", ["Straße und Hausnummer", "ein Getränk", "eine Jahreszeit"], "Straße und Hausnummer", "Eine Adresse zeigt, wo man wohnt."),
            G("Die Wohnung ___ zwei Zimmer.", ["hat", "haben", "hast"], "hat", "Bei die Wohnung verwenden wir hat."),
            G("Wir ___ in einer Wohnung.", ["wohnen", "wohnt", "wohnst"], "wohnen", "Bei wir lautet wohnen: wohnen."),
            G("Das Sofa steht ___ Wohnzimmer.", ["im", "am", "nach"], "im", "In dem Wohnzimmer wird zu im Wohnzimmer."),
            G("Ich habe ___ Balkon.", ["einen", "eine", "ein"], "einen", "Balkon ist maskulin: einen Balkon."),
            G("In meinem Zimmer ___ ein Bett.", ["steht", "stehen", "stehst"], "steht", "Ein Bett ist Singular: steht.")),

        CreateLesson(6, "Freizeit",
            V("Was machst du am Wochenende?", ["Ich spiele Fußball.", "Ich bin ein Tisch.", "Ich trinke eine Lampe."], "Ich spiele Fußball.", "Fußball spielen ist eine Freizeitaktivität."),
            V("Ein Hobby ist ___.", ["Musik hören", "die Adresse", "der Schlüssel"], "Musik hören", "Musik hören kann ein Hobby sein."),
            V("Was kann man im Park machen?", ["spazieren gehen", "eine Rechnung trinken", "im Kalender essen"], "spazieren gehen", "Im Park kann man spazieren gehen."),
            V("Was brauchst du für Tennis?", ["einen Schläger", "einen Kühlschrank", "eine Fahrkarte"], "einen Schläger", "Mit einem Schläger spielt man Tennis."),
            V("Welche Antwort passt? – Was machst du gern?", ["Ich lese gern.", "Ich bin aus Köln.", "Ich habe zwei Brüder."], "Ich lese gern.", "Die Frage fragt nach einer Lieblingsaktivität."),
            G("Er ___ gern Rad.", ["fährt", "fahren", "fährst"], "fährt", "Bei er lautet fahren: fährt."),
            G("___ ihr heute Tennis?", ["Spielt", "Spielen", "Spielst"], "Spielt", "Bei ihr lautet die Frage Spielt ihr ...?"),
            G("Ich ___ am Samstag Freunde.", ["treffe", "triffst", "treffen"], "treffe", "Bei ich lautet treffen: treffe."),
            G("Wir ___ gern Filme.", ["sehen", "sieht", "siehst"], "sehen", "Bei wir lautet sehen: sehen."),
            G("Am Sonntag ___ ich nicht.", ["arbeite", "arbeitet", "arbeiten"], "arbeite", "Bei ich lautet arbeiten: arbeite.")),

        CreateLesson(7, "Arbeit und Termine",
            V("Wann hast du einen Termin?", ["am Dienstag", "im Fenster", "mit Brot"], "am Dienstag", "Ein Wochentag passt zu einem Termin."),
            V("Meine Kollegin arbeitet im ___.", ["Büro", "Kuchen", "Winter"], "Büro", "Im Büro arbeitet man."),
            V("Was brauchst du für einen Arzttermin?", ["die Versichertenkarte", "einen Fußball", "eine Badehose"], "die Versichertenkarte", "Die Karte nimmt man zum Arzt mit."),
            V("Was sagt man am Telefon?", ["Guten Tag, hier ist Ali.", "Guten Tag, ich bin ein Tisch.", "Hallo, ich esse Montag."], "Guten Tag, hier ist Ali.", "So stellt man sich am Telefon vor."),
            V("Ein Termin um 15 Uhr ist um ___.", ["drei Uhr", "fünf Uhr", "zehn Uhr"], "drei Uhr", "15 Uhr ist drei Uhr nachmittags."),
            G("Ich ___ um neun Uhr an.", ["fange", "fängt", "fangen"], "fange", "Die Form für ich ist fange ... an."),
            G("Wir ___ morgen einen Termin.", ["haben", "hat", "hast"], "haben", "Bei wir verwenden wir haben."),
            G("Können wir den Termin ___?", ["verschieben", "verschiebt", "verschiebst"], "verschieben", "Nach können steht der Infinitiv verschieben."),
            G("Meine Kollegin ___ heute bis fünf.", ["arbeitet", "arbeiten", "arbeitest"], "arbeitet", "Bei sie lautet arbeiten: arbeitet."),
            G("___ Sie am Freitag Zeit?", ["Haben", "Hast", "Hat"], "Haben", "Die höfliche Form mit Sie ist Haben Sie ...?")),

        CreateLesson(8, "Kleidung und Farben",
            V("Was trägt man an den Füßen?", ["Schuhe", "Hemd", "Hut"], "Schuhe", "Schuhe trägt man an den Füßen."),
            V("Welche Farbe hat Gras?", ["grün", "blau", "schwarz"], "grün", "Gras ist normalerweise grün."),
            V("Was trägt man im Winter?", ["eine warme Jacke", "eine Badehose", "Sandalen"], "eine warme Jacke", "Im Winter braucht man warme Kleidung."),
            V("Welche Farbe ist eine dunkle Farbe?", ["schwarz", "gelb", "rosa"], "schwarz", "Schwarz ist eine dunkle Farbe."),
            V("Was passt im Geschäft?", ["Kann ich die Hose anprobieren?", "Kann ich den Montag trinken?", "Kann ich die Adresse essen?"], "Kann ich die Hose anprobieren?", "Im Kleidungsgeschäft probiert man Kleidung an."),
            G("Die Jacke ___ blau.", ["ist", "sind", "bist"], "ist", "Bei die Jacke verwenden wir ist."),
            G("Ich ___ heute eine Hose.", ["trage", "trägt", "tragen"], "trage", "Bei ich lautet tragen: trage."),
            G("Die Schuhe ___ neu.", ["sind", "ist", "bist"], "sind", "Schuhe steht im Plural: sind."),
            G("Ich möchte ___ roten Pullover.", ["einen", "eine", "ein"], "einen", "Pullover ist maskulin: einen Pullover."),
            G("Welche Farbe ___ dein T-Shirt?", ["hat", "haben", "hast"], "hat", "Bei das T-Shirt lautet haben: hat.")),

        CreateLesson(9, "Gesundheit",
            V("Was sagt man beim Arzt?", ["Ich habe Schmerzen.", "Ich bin ein Zimmer.", "Ich kaufe einen Tisch."], "Ich habe Schmerzen.", "Das ist eine passende Aussage beim Arzt."),
            V("Bei Fieber braucht man ein ___.", ["Thermometer", "Fahrrad", "Kissen"], "Thermometer", "Mit einem Thermometer misst man die Temperatur."),
            V("Was tut weh?", ["Der Kopf.", "Der Dienstag.", "Die Adresse."], "Der Kopf.", "Kopf kann als Körperteil wehtun."),
            V("Was ist gesund?", ["Obst und Wasser", "nur Süßigkeiten", "eine Fahrkarte"], "Obst und Wasser", "Obst und Wasser gehören zu einer gesunden Ernährung."),
            V("Was sagt man in der Apotheke?", ["Ich brauche ein Medikament.", "Ich brauche ein Schlafzimmer.", "Ich brauche einen Termin im Sommer."], "Ich brauche ein Medikament.", "In der Apotheke kauft man Medikamente."),
            G("Mein Kopf ___.", ["tut weh", "tun weh", "tust weh"], "tut weh", "Kopf ist Singular: tut weh."),
            G("Du ___ viel Wasser trinken.", ["sollst", "soll", "sollen"], "sollst", "Bei du lautet sollen: sollst."),
            G("Ich ___ heute zu Hause bleiben.", ["muss", "musst", "müssen"], "muss", "Bei ich lautet müssen: muss."),
            G("Wir ___ zum Arzt gehen.", ["müssen", "muss", "musst"], "müssen", "Bei wir lautet müssen: müssen."),
            G("___ Sie Schmerzen?", ["Haben", "Hast", "Hat"], "Haben", "Die höfliche Frage lautet Haben Sie Schmerzen?")),

        CreateLesson(10, "Unterwegs",
            V("Womit fährst du in die Stadt?", ["mit dem Bus", "mit dem Bett", "mit dem Teller"], "mit dem Bus", "Ein Bus ist ein Verkehrsmittel."),
            V("Wo kauft man eine Fahrkarte?", ["am Bahnhof", "im Badezimmer", "im Garten"], "am Bahnhof", "Am Bahnhof bekommt man Fahrkarten."),
            V("Wie fragst du nach dem Weg?", ["Wo ist der Bahnhof?", "Wie heißt deine Schwester?", "Was trinkst du?"], "Wo ist der Bahnhof?", "Wo ist ...? fragt nach einem Ort."),
            V("Was bedeutet links?", ["diese Richtung", "eine Mahlzeit", "eine Jahreszeit"], "diese Richtung", "Links beschreibt eine Richtung."),
            V("Was macht man an der Haltestelle?", ["auf den Bus warten", "eine Wohnung mieten", "einen Arzt untersuchen"], "auf den Bus warten", "An der Haltestelle wartet man auf den Bus."),
            G("Der Zug ___ um zehn Uhr ab.", ["fährt", "fahren", "fährst"], "fährt", "Bei der Zug verwenden wir fährt ab."),
            G("Wir ___ an der nächsten Station aus.", ["steigen", "steigt", "steigst"], "steigen", "Bei wir lautet aussteigen: steigen ... aus."),
            G("___ du mit dem Fahrrad?", ["Fährst", "Fahren", "Fährt"], "Fährst", "Bei du lautet fahren: fährst."),
            G("Gehen Sie ___ die Straße und dann rechts.", ["geradeaus", "gestern", "lecker"], "geradeaus", "Geradeaus beschreibt die Richtung."),
            G("Ich ___ die Adresse nicht.", ["finde", "findest", "finden"], "finde", "Bei ich lautet finden: finde.")),

        CreateLesson(11, "Wetter und Jahreszeiten",
            V("Wie ist das Wetter heute?", ["Es ist sonnig.", "Es ist ein Schuh.", "Es sind drei Uhr."], "Es ist sonnig.", "Das ist eine Wetterbeschreibung."),
            V("Im Winter ist es oft ___.", ["kalt", "schnell", "teuer"], "kalt", "Winter ist normalerweise kalt."),
            V("Welche Jahreszeit kommt nach dem Frühling?", ["Sommer", "Winter", "Herbst"], "Sommer", "Nach dem Frühling kommt der Sommer."),
            V("Was brauchst du bei Regen?", ["einen Regenschirm", "eine Sonnenbrille", "einen Kühlschrank"], "einen Regenschirm", "Ein Regenschirm schützt bei Regen."),
            V("Wie ist das Wetter bei 30 Grad?", ["warm", "eiskalt", "dunkel"], "warm", "30 Grad ist normalerweise warm."),
            G("Heute ___ es.", ["regnet", "regnen", "regnest"], "regnet", "Bei es lautet regnen: regnet."),
            G("Im Sommer ___ wir im Park.", ["sitzen", "sitzt", "sitze"], "sitzen", "Bei wir lautet sitzen: sitzen."),
            G("Morgen ___ die Sonne.", ["scheint", "scheinen", "scheinst"], "scheint", "Bei die Sonne lautet scheinen: scheint."),
            G("Wenn es kalt ist, ___ ich eine Jacke.", ["trage", "trägt", "tragen"], "trage", "Bei ich lautet tragen: trage."),
            G("Im Herbst ___ die Blätter gelb.", ["werden", "wird", "wirst"], "werden", "Blätter steht im Plural: werden.")),

        CreateLesson(12, "Reisen und Pläne",
            V("Was nimmt man auf eine Reise mit?", ["einen Koffer", "eine Lampe", "einen Herd"], "einen Koffer", "In einen Koffer packt man Kleidung."),
            V("Wohin möchtest du fahren?", ["nach München", "nach gestern", "nach blau"], "nach München", "München ist ein Reiseziel."),
            V("Was braucht man im Hotel?", ["eine Reservierung", "eine Jahreszeit", "einen Wochentag"], "eine Reservierung", "Mit einer Reservierung ist ein Zimmer gebucht."),
            V("Was macht man am Flughafen?", ["einchecken", "schlafen im Kühlschrank", "eine Farbe kaufen"], "einchecken", "Am Flughafen checkt man ein."),
            V("Welche Antwort passt? – Wie war die Reise?", ["Sie war schön.", "Sie ist ein Koffer.", "Sie hat Dienstag."], "Sie war schön.", "So beschreibt man eine vergangene Reise einfach."),
            G("Nächste Woche ___ ich nach Köln.", ["fahre", "fährt", "fahren"], "fahre", "Bei ich lautet fahren: fahre."),
            G("___ du im Hotel?", ["Übernachtest", "Übernachten", "Übernachtet"], "Übernachtest", "Bei du lautet übernachten: übernachtest."),
            G("Wir ___ am Samstag nach Berlin.", ["fahren", "fährt", "fährst"], "fahren", "Bei wir lautet fahren: fahren."),
            G("Ich ___ morgen früh aufstehen.", ["muss", "musst", "müssen"], "muss", "Bei ich lautet müssen: muss."),
            G("___ ihr schon einen Reiseplan?", ["Habt", "Hat", "Haben"], "Habt", "Bei ihr lautet haben: habt."))
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
        params (QuizCategory Category, string Prompt, string[] Options, string CorrectAnswer, string Explanation)[] questions)
    {
        var lessonId = number == 1
            ? Guid.Parse("11111111-1111-1111-1111-111111111111")
            : Guid.Parse($"11111111-1111-1111-1111-{number:000000000000}");
        var baseQuestionId = number == 1 ? 1 : number * 100 + 1;

        return new QuizContentLesson(
            new Lesson(lessonId, "Menschen", "A1.1", number, title),
            questions.Select((question, index) => new QuizQuestion(
                Guid.Parse($"20000000-0000-0000-0000-{baseQuestionId + index:000000000000}"),
                lessonId,
                question.Category,
                QuestionType.MultipleChoice,
                question.Prompt,
                question.Options,
                question.CorrectAnswer,
                question.Explanation)).ToList());
    }

    private static (QuizCategory Category, string Prompt, string[] Options, string CorrectAnswer, string Explanation) V(
        string prompt, string[] options, string correctAnswer, string explanation) =>
        (QuizCategory.Vocabulary, prompt, options, correctAnswer, explanation);

    private static (QuizCategory Category, string Prompt, string[] Options, string CorrectAnswer, string Explanation) G(
        string prompt, string[] options, string correctAnswer, string explanation) =>
        (QuizCategory.Grammar, prompt, options, correctAnswer, explanation);
}
