using DeutschQuiz.Domain;

namespace DeutschQuiz.Application;

public sealed record QuizContentLesson(
    Lesson Lesson,
    IReadOnlyList<QuizQuestion> Questions);

public sealed record QuizContentBook(
    Guid Id,
    string Name,
    string Level,
    string Publisher,
    IReadOnlyList<QuizContentLesson> Lessons);

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
            G("Wir ___ Deutsch.", ["lernen", "lernt", "lernst"], "lernen", "Bei wir endet das Verb auf -en."),
            V("Welche Frage passt zur Antwort: Ich heiße Amir.", ["Wie heißt du?", "Woher kommst du?", "Wie geht es dir?"], "Wie heißt du?", "Mit Wie heißt du? fragt man nach dem Namen."),
            V("Was sagt man beim Abschied?", ["Auf Wiedersehen!", "Guten Morgen!", "Guten Appetit!"], "Auf Wiedersehen!", "Auf Wiedersehen sagt man beim Abschied."),
            V("Was passt? – Ich komme aus Spanien. – ___.", ["Ah, interessant.", "Um acht Uhr.", "Im Kurs."], "Ah, interessant.", "Das ist eine passende Reaktion auf die Herkunft."),
            V("Wer ist Frau Kaya?", ["Sie ist Lehrerin.", "Sie sind Lehrer.", "Er ist Lehrerin."], "Sie ist Lehrerin.", "Frau Kaya ist eine Person; die passende Antwort nutzt sie."),
            V("Welche Sprache lernst du?", ["Deutsch.", "In Berlin.", "Sehr gut."], "Deutsch.", "Eine Sprache passt als Antwort."),
            G("Ich ___ Maria.", ["heiße", "heißt", "heißen"], "heiße", "Bei ich lautet heißen: heiße."),
            G("Er ___ aus Deutschland.", ["kommt", "komme", "kommen"], "kommt", "Bei er lautet kommen: kommt."),
            G("___ du Deutsch?", ["Lernst", "Lernen", "Lernt"], "Lernst", "Bei du lautet die Frage Lernst du ...?"),
            G("Wir ___ aus Teheran.", ["kommen", "kommt", "kommst"], "kommen", "Bei wir lautet kommen: kommen."),
            G("Sie ___ Deutsch.", ["sprechen", "spricht", "sprichst"], "sprechen", "Bei Sie lautet sprechen: sprechen.")),

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
            G("Meine Freundin ___ sehr gut Deutsch.", ["spricht", "sprechen", "sprichst"], "spricht", "Bei sie lautet sprechen: spricht."),
            V("Wie heißt die Mutter von deinem Vater?", ["Großmutter", "Schwester", "Tochter"], "Großmutter", "Die Mutter des Vaters ist die Großmutter."),
            V("Der Bruder meiner Mutter ist mein ___.", ["Onkel", "Sohn", "Großvater"], "Onkel", "Der Bruder der Mutter ist der Onkel."),
            V("Anna und Sara sind Töchter von Frau Braun. Sie sind ___.", ["Schwestern", "Mütter", "Freundinnen"], "Schwestern", "Zwei Töchter derselben Eltern sind Schwestern."),
            V("Wen triffst du am Samstag?", ["Meine Freunde.", "Meine Wohnung.", "Meine Uhr."], "Meine Freunde.", "Freunde kann man treffen."),
            V("Welche Frage passt? – Das ist mein Bruder.", ["Wer ist das?", "Wann ist das?", "Wo ist das?"], "Wer ist das?", "Wer fragt nach einer Person."),
            G("Das ist ___ Bruder.", ["mein", "meine", "meinen"], "mein", "Bruder ist maskulin: mein Bruder."),
            G("Ich habe ___ Schwester.", ["eine", "einen", "ein"], "eine", "Schwester ist feminin: eine Schwester."),
            G("Meine Eltern ___ in Isfahan.", ["wohnen", "wohnt", "wohnst"], "wohnen", "Bei meine Eltern verwenden wir wohnen."),
            G("___ ihr Kinder?", ["Habt", "Hat", "Hast"], "Habt", "Bei ihr lautet haben: habt."),
            G("Der Vater ___ zu Hause.", ["ist", "sind", "bist"], "ist", "Der Vater ist Singular.")),

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
            G("Wir ___ um sieben Uhr auf.", ["stehen", "steht", "stehst"], "stehen", "Bei wir lautet aufstehen: stehen ... auf."),
            V("Wie viel ist sieben plus drei?", ["zehn", "zwölf", "sechzehn"], "zehn", "Sieben plus drei ist zehn."),
            V("Welcher Tag kommt nach Mittwoch?", ["Donnerstag", "Dienstag", "Sonntag"], "Donnerstag", "Nach Mittwoch kommt Donnerstag."),
            V("Wann gehst du ins Bett?", ["Um elf Uhr.", "Aus dem Iran.", "Mit dem Bus."], "Um elf Uhr.", "Eine Uhrzeit passt als Antwort."),
            V("Welche Zahl ist 20?", ["zwanzig", "zwölf", "zwei"], "zwanzig", "20 heißt zwanzig."),
            V("Was steht in einem Kalender?", ["Termine", "Getränke", "Schuhe"], "Termine", "In einem Kalender stehen Termine."),
            G("Am Montag ___ ich Deutsch.", ["lerne", "lernt", "lernst"], "lerne", "Bei ich lautet lernen: lerne."),
            G("___ beginnt der Kurs?", ["Wann", "Woher", "Wer"], "Wann", "Wann fragt nach der Zeit."),
            G("Der Kurs beginnt ___ acht Uhr.", ["um", "aus", "mit"], "um", "Bei Uhrzeiten verwenden wir um."),
            G("Er ___ um sechs Uhr auf.", ["steht", "stehen", "stehst"], "steht", "Bei er lautet aufstehen: steht ... auf."),
            G("Am Samstag ___ ich frei.", ["habe", "hat", "haben"], "habe", "Bei ich lautet haben: habe.")),

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
            G("Wir ___ heute nicht zu Hause.", ["essen", "isst", "esst"], "essen", "Bei wir bleibt die Verbform essen."),
            V("Welches Wort ist Gemüse?", ["Kartoffel", "Apfel", "Kaffee"], "Kartoffel", "Eine Kartoffel ist Gemüse."),
            V("Was kaufst du beim Bäcker?", ["Brot", "Milch", "Fisch"], "Brot", "Beim Bäcker kauft man Brot."),
            V("Was passt zum Frühstück?", ["Brot und Käse", "Bus und Bahnhof", "Hemd und Schuhe"], "Brot und Käse", "Brot und Käse sind typische Lebensmittel."),
            V("Was brauchst du für Kaffee?", ["eine Tasse", "einen Schrank", "eine Jacke"], "eine Tasse", "Kaffee trinkt man aus einer Tasse."),
            V("Welche Frage passt im Café?", ["Was möchten Sie?", "Wo wohnen Sie?", "Wie alt sind Sie?"], "Was möchten Sie?", "Im Café fragt man nach der Bestellung."),
            G("Er ___ einen Apfel.", ["isst", "essen", "esse"], "isst", "Bei er lautet essen: isst."),
            G("Ich ___ gern Wasser.", ["trinke", "trinkst", "trinken"], "trinke", "Bei ich lautet trinken: trinke."),
            G("Möchtest du ___ Saft?", ["einen", "eine", "ein"], "einen", "Saft ist maskulin: einen Saft."),
            G("___ ihr Brot?", ["Esst", "Isst", "Essen"], "Esst", "Bei ihr lautet essen: esst."),
            G("Die Suppe ___ gut.", ["schmeckt", "schmecken", "schmeckst"], "schmeckt", "Die Suppe ist Singular: schmeckt.")),

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
            G("In meinem Zimmer ___ ein Bett.", ["steht", "stehen", "stehst"], "steht", "Ein Bett ist Singular: steht."),
            V("Wo kocht man?", ["in der Küche", "im Schlafzimmer", "auf dem Balkon"], "in der Küche", "In der Küche kocht man."),
            V("Was steht oft im Wohnzimmer?", ["ein Sofa", "eine Dusche", "ein Herd"], "ein Sofa", "Ein Sofa gehört oft ins Wohnzimmer."),
            V("Was macht man an der Tür?", ["aufmachen", "trinken", "fahren"], "aufmachen", "Eine Tür kann man aufmachen."),
            V("Was ist ein Raum?", ["das Schlafzimmer", "der Dienstag", "die Rechnung"], "das Schlafzimmer", "Das Schlafzimmer ist ein Raum."),
            V("Was braucht man zum Putzen?", ["einen Staubsauger", "eine Fahrkarte", "einen Regenschirm"], "einen Staubsauger", "Mit einem Staubsauger putzt man."),
            G("In der Küche ___ ein Tisch.", ["steht", "stehen", "stehst"], "steht", "Ein Tisch ist Singular."),
            G("Die Zimmer ___ hell.", ["sind", "ist", "bist"], "sind", "Zimmer steht hier im Plural."),
            G("Wir haben ___ Küche.", ["eine", "einen", "ein"], "eine", "Küche ist feminin: eine Küche."),
            G("___ du allein?", ["Wohnst", "Wohnen", "Wohnt"], "Wohnst", "Bei du lautet wohnen: wohnst."),
            G("Das Bad ist ___ Flur.", ["neben dem", "nach die", "um den"], "neben dem", "Neben dem Flur beschreibt den Ort.")),

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
            G("Am Sonntag ___ ich nicht.", ["arbeite", "arbeitet", "arbeiten"], "arbeite", "Bei ich lautet arbeiten: arbeite."),
            V("Was machst du mit einem Buch?", ["lesen", "fahren", "kochen"], "lesen", "Ein Buch liest man."),
            V("Welche Aktivität ist draußen?", ["Rad fahren", "fernsehen", "schlafen"], "Rad fahren", "Rad fahren kann man draußen."),
            V("Was macht man im Kino?", ["einen Film sehen", "eine Suppe kochen", "einen Bus nehmen"], "einen Film sehen", "Im Kino sieht man einen Film."),
            V("Was passt zu Musik?", ["tanzen", "wohnen", "schreiben eine Adresse"], "tanzen", "Zu Musik kann man tanzen."),
            V("Wann hast du Zeit für ein Hobby?", ["Am Samstag.", "Im Kühlschrank.", "Aus Berlin."], "Am Samstag.", "Ein Wochentag passt als Zeitangabe."),
            G("Sie ___ gern Musik.", ["hört", "hören", "hörst"], "hört", "Bei sie lautet hören: hört."),
            G("___ du gern Bücher?", ["Liest", "Lesen", "Lest"], "Liest", "Bei du lautet lesen: liest."),
            G("Wir ___ am Abend fern.", ["sehen", "sieht", "siehst"], "sehen", "Bei wir lautet fernsehen: sehen ... fern."),
            G("Er ___ am Samstag Fußball.", ["spielt", "spielen", "spielst"], "spielt", "Bei er lautet spielen: spielt."),
            G("Ich ___ gern schwimmen.", ["gehe", "geht", "gehen"], "gehe", "Bei ich lautet gehen: gehe.")),

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
            G("___ Sie am Freitag Zeit?", ["Haben", "Hast", "Hat"], "Haben", "Die höfliche Form mit Sie ist Haben Sie ...?"),
            V("Wer arbeitet im Büro?", ["eine Kollegin", "ein Apfel", "ein Fahrplan"], "eine Kollegin", "Eine Kollegin kann im Büro arbeiten."),
            V("Was steht im Kalender?", ["ein Termin", "ein Schuh", "ein Getränk"], "ein Termin", "Im Kalender steht ein Termin."),
            V("Was sagt man bei einer Verspätung?", ["Ich komme später.", "Ich bin ein Büro.", "Ich esse um Berlin."], "Ich komme später.", "So informiert man über eine Verspätung."),
            V("Was braucht man bei einem Meeting?", ["einen Notizblock", "eine Badehose", "einen Koffer"], "einen Notizblock", "Ein Notizblock ist für Notizen nützlich."),
            V("Was bedeutet pünktlich?", ["zur richtigen Zeit", "sehr hungrig", "in einer Wohnung"], "zur richtigen Zeit", "Pünktlich heißt zur richtigen Zeit."),
            G("Der Chef ___ um acht Uhr.", ["beginnt", "beginnen", "beginnst"], "beginnt", "Bei der Chef lautet beginnen: beginnt."),
            G("Ich ___ heute bis sechs.", ["arbeite", "arbeitet", "arbeiten"], "arbeite", "Bei ich lautet arbeiten: arbeite."),
            G("Wir ___ am Montag frei.", ["haben", "hat", "hast"], "haben", "Bei wir lautet haben: haben."),
            G("___ du morgen einen Termin?", ["Hast", "Hat", "Haben"], "Hast", "Bei du lautet haben: hast."),
            G("Sie ___ um zehn Uhr an.", ["fängt", "fangen", "fängst"], "fängt", "Bei sie lautet anfangen: fängt ... an.")),

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
            G("Welche Farbe ___ dein T-Shirt?", ["hat", "haben", "hast"], "hat", "Bei das T-Shirt lautet haben: hat."),
            V("Was trägt man am Kopf?", ["eine Mütze", "eine Hose", "Schuhe"], "eine Mütze", "Eine Mütze trägt man am Kopf."),
            V("Was ist ein Kleidungsstück?", ["ein Kleid", "ein Bahnhof", "ein Medikament"], "ein Kleid", "Ein Kleid ist ein Kleidungsstück."),
            V("Welche Farbe hat eine Zitrone?", ["gelb", "schwarz", "grau"], "gelb", "Eine Zitrone ist normalerweise gelb."),
            V("Was sagt man im Kleidungsgeschäft?", ["Wie viel kostet das?", "Wo ist der Bahnhof?", "Wie geht es dem Wetter?"], "Wie viel kostet das?", "Im Geschäft fragt man nach dem Preis."),
            V("Was zieht man bei Regen an?", ["einen Regenmantel", "eine Badehose", "Sandalen"], "einen Regenmantel", "Ein Regenmantel schützt bei Regen."),
            G("Das Kleid ___ schön.", ["ist", "sind", "bist"], "ist", "Das Kleid ist Singular."),
            G("Wir ___ warme Jacken.", ["tragen", "trägt", "trägst"], "tragen", "Bei wir lautet tragen: tragen."),
            G("___ du den blauen Pullover?", ["Trägst", "Tragen", "Trägt"], "Trägst", "Bei du lautet tragen: trägst."),
            G("Die Hose ___ 30 Euro.", ["kostet", "kosten", "kostest"], "kostet", "Die Hose ist Singular: kostet."),
            G("Ich brauche ___ neue Schuhe.", ["—", "einen", "eine"], "—", "Schuhe steht im Plural; hier steht kein Artikel.")),

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
            G("___ Sie Schmerzen?", ["Haben", "Hast", "Hat"], "Haben", "Die höfliche Frage lautet Haben Sie Schmerzen?"),
            V("Welches Körperteil ist am Fuß?", ["der Zeh", "der Kopf", "der Hals"], "der Zeh", "Ein Zeh ist ein Körperteil am Fuß."),
            V("Was braucht man bei Kopfschmerzen?", ["eine Tablette", "eine Fahrkarte", "einen Koffer"], "eine Tablette", "Eine Tablette kann gegen Schmerzen helfen."),
            V("Was ist ein Symptom?", ["Husten", "ein Bahnhof", "ein Pullover"], "Husten", "Husten kann ein Symptom sein."),
            V("Was sagt der Arzt?", ["Machen Sie den Mund auf.", "Trinken Sie den Fahrplan.", "Schlafen Sie im Bus."], "Machen Sie den Mund auf.", "Das ist eine einfache Anweisung beim Arzt."),
            V("Was ist wichtig für die Gesundheit?", ["Schlaf", "nur Zucker", "keine Bewegung"], "Schlaf", "Genug Schlaf ist wichtig."),
            G("Mein Bauch ___ weh.", ["tut", "tun", "tust"], "tut", "Bauch ist Singular: tut weh."),
            G("Ihr ___ heute viel trinken.", ["sollt", "soll", "sollen"], "sollt", "Bei ihr lautet sollen: sollt."),
            G("Er ___ zum Arzt gehen.", ["muss", "müssen", "musst"], "muss", "Bei er lautet müssen: muss."),
            G("Ich ___ keine Schmerzen.", ["habe", "hast", "haben"], "habe", "Bei ich lautet haben: habe."),
            G("Die Kinder ___ gesund.", ["sind", "ist", "bist"], "sind", "Kinder steht im Plural.")),

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
            G("Ich ___ die Adresse nicht.", ["finde", "findest", "finden"], "finde", "Bei ich lautet finden: finde."),
            V("Was sieht man am Bahnhof?", ["einen Fahrplan", "eine Dusche", "ein Rezept"], "einen Fahrplan", "Ein Fahrplan zeigt die Abfahrtszeiten."),
            V("Was braucht man für den Bus?", ["eine Fahrkarte", "einen Arzt", "ein Bett"], "eine Fahrkarte", "Mit einer Fahrkarte fährt man Bus."),
            V("Wo wartet man auf den Zug?", ["am Bahnsteig", "im Schlafzimmer", "in der Apotheke"], "am Bahnsteig", "Am Bahnsteig wartet man auf den Zug."),
            V("Was bedeutet rechts?", ["diese Richtung", "ein Getränk", "ein Termin"], "diese Richtung", "Rechts beschreibt eine Richtung."),
            V("Wie fragst du höflich nach dem Weg?", ["Entschuldigung, wo ist der Bahnhof?", "Gib mir den Montag!", "Trinkst du die Straße?"], "Entschuldigung, wo ist der Bahnhof?", "Entschuldigung macht die Frage höflich."),
            G("Der Bus ___ an der Haltestelle.", ["hält", "halten", "hältst"], "hält", "Bei der Bus lautet halten: hält."),
            G("Wir ___ geradeaus.", ["gehen", "geht", "gehst"], "gehen", "Bei wir lautet gehen: gehen."),
            G("___ Sie links ab?", ["Biegen", "Biegt", "Biegst"], "Biegen", "Bei Sie lautet die Frage Biegen Sie ...?"),
            G("Ich ___ mit dem Zug nach Bonn.", ["fahre", "fährt", "fahren"], "fahre", "Bei ich lautet fahren: fahre."),
            G("Die Fahrkarten ___ im Automaten.", ["sind", "ist", "bist"], "sind", "Fahrkarten steht im Plural.")),

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
            G("Im Herbst ___ die Blätter gelb.", ["werden", "wird", "wirst"], "werden", "Blätter steht im Plural: werden."),
            V("Was sieht man am Himmel?", ["Wolken", "Schuhe", "Teller"], "Wolken", "Wolken sieht man am Himmel."),
            V("Was braucht man bei Sonne?", ["eine Sonnenbrille", "einen Regenschirm", "einen Schal"], "eine Sonnenbrille", "Eine Sonnenbrille schützt die Augen."),
            V("Welche Jahreszeit ist oft heiß?", ["der Sommer", "der Winter", "der Herbst"], "der Sommer", "Im Sommer ist es oft heiß."),
            V("Was macht man bei Schnee?", ["einen Schneemann bauen", "eine Fahrkarte kaufen", "im Café arbeiten"], "einen Schneemann bauen", "Bei Schnee kann man einen Schneemann bauen."),
            V("Wie ist der Himmel bei Regen?", ["grau", "süß", "klein"], "grau", "Bei Regen ist der Himmel oft grau."),
            G("Es ___ heute kalt.", ["ist", "sind", "bist"], "ist", "Es ist kalt ist die richtige Form."),
            G("Im Frühling ___ die Blumen.", ["blühen", "blüht", "blühst"], "blühen", "Blumen steht im Plural: blühen."),
            G("___ es morgen?", ["Regnet", "Regnen", "Regnest"], "Regnet", "Bei es lautet regnen: regnet."),
            G("Wir ___ im Sommer oft draußen.", ["sind", "ist", "bist"], "sind", "Bei wir lautet sein: sind."),
            G("Der Wind ___ stark.", ["weht", "wehen", "wehst"], "weht", "Der Wind ist Singular: weht.")),

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
            G("___ ihr schon einen Reiseplan?", ["Habt", "Hat", "Haben"], "Habt", "Bei ihr lautet haben: habt."),
            V("Was bucht man für eine Reise?", ["ein Hotelzimmer", "einen Wochentag", "eine Farbe"], "ein Hotelzimmer", "Für eine Reise bucht man ein Hotelzimmer."),
            V("Was zeigt der Reisepass?", ["deine Identität", "das Wetter", "den Stundenplan"], "deine Identität", "Der Reisepass ist ein persönliches Dokument."),
            V("Was macht man am Bahnhof vor der Abfahrt?", ["auf den Zug warten", "eine Suppe kochen", "eine Jacke waschen"], "auf den Zug warten", "Vor der Abfahrt wartet man auf den Zug."),
            V("Was packst du in den Koffer?", ["Kleidung", "eine Dusche", "einen Bahnhof"], "Kleidung", "Kleidung nimmt man auf die Reise mit."),
            V("Welche Frage passt im Hotel?", ["Haben Sie ein Zimmer frei?", "Wie ist dein Bruder?", "Was kostet der Dienstag?"], "Haben Sie ein Zimmer frei?", "Im Hotel fragt man nach einem freien Zimmer."),
            G("Morgen ___ wir nach Hamburg.", ["fahren", "fährt", "fährst"], "fahren", "Bei wir lautet fahren: fahren."),
            G("Sie ___ ein Zimmer.", ["bucht", "buchen", "buchst"], "bucht", "Bei sie lautet buchen: bucht."),
            G("___ du den Koffer?", ["Packst", "Packen", "Packt"], "Packst", "Bei du lautet packen: packst."),
            G("Wir ___ im Hotel.", ["übernachten", "übernachtet", "übernachtest"], "übernachten", "Bei wir lautet übernachten: übernachten."),
            G("Der Zug ___ morgen früh ab.", ["fährt", "fahren", "fährst"], "fährt", "Bei der Zug lautet fahren: fährt.")),
    ];

    public static IReadOnlyList<QuizContentBook> Books { get; } =
    [
        new(
            Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
            "Menschen",
            "A1.1",
            "Hueber",
            Lessons),
        new(
            Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"),
            "Menschen",
            "A1.2",
            "Hueber",
            MenschenA12QuizContentCatalog.Lessons),
        new(
            Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd"),
            "Menschen",
            "A2.1",
            "Hueber",
            MenschenA21QuizContentCatalog.Lessons),
        new(
            Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"),
            "Menschen",
            "A2.2",
            "Hueber",
            MenschenA22QuizContentCatalog.Lessons),
        new(
            Guid.Parse("f1f1f1f1-f1f1-f1f1-f1f1-f1f1f1f1f1f1"),
            "Menschen",
            "B1.1",
            "Hueber",
            MenschenB11QuizContentCatalog.Lessons),
        new(
            Guid.Parse("f2f2f2f2-f2f2-f2f2-f2f2-f2f2f2f2f2f2"),
            "Menschen",
            "B1.2",
            "Hueber",
            MenschenB12QuizContentCatalog.Lessons),
        new(
            Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
            "Starten wir!",
            "A1.1",
            "Hueber",
            StartenWirQuizContentCatalog.Lessons)
    ];

    public static IReadOnlyList<QuizContentLesson> AllLessons { get; } =
        Books.SelectMany(book => book.Lessons).ToList();

    public static IReadOnlyList<Lesson> GetLessons() =>
        AllLessons.Select(content => content.Lesson).ToList();

    public static IReadOnlyList<QuizQuestion> GetQuestions(
        Guid lessonId,
        QuizCategory? category = null) =>
        AllLessons
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
