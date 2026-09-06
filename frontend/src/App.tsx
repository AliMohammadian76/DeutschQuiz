import { FormEvent, useEffect, useState } from "react";
import {
  ACTIVE_UI_LANGUAGE,
  Language,
  SHOW_LANGUAGE_SWITCHER,
  dirFor,
  getMessages,
  localeFor,
} from "./i18n";
import { UserProgressChart } from "./UserProgressChart";

const apiBaseUrl =
  import.meta.env.VITE_API_BASE_URL ?? "http://localhost:5083/api";

const quizModeMeta = [
  {
    category: "Vocabulary",
    subtitle: "Wortschatz",
    accent: "bg-de-black text-white",
    card: "border-de-black/10 bg-gradient-to-br from-surface to-de-mist",
  },
  {
    category: "Grammar",
    subtitle: "Grammatik",
    accent: "bg-de-red text-white",
    card: "border-de-red/15 bg-gradient-to-br from-surface to-surface-rose",
  },
  {
    category: "Mixed",
    subtitle: "Komplett",
    accent: "bg-de-gold text-de-black",
    card: "border-de-gold/40 bg-gradient-to-br from-surface to-surface-warm",
  },
] as const;

type AuthMode = "login" | "register";
type AppPage = "quizzes" | "progress" | "history";
type QuizCategory = (typeof quizModeMeta)[number]["category"];
type AuthResult = { accessToken: string; user: { displayName: string } };
type QuizQuestion = {
  id: string;
  lessonId: string;
  category: QuizCategory;
  type: string;
  prompt: string;
  options: string[];
};
type Lesson = {
  id: string;
  book: string;
  level: string;
  number: number;
  title: string;
};
type BookOption = {
  name: string;
  levels: string[];
};
type AttemptResult = {
  totalQuestions: number;
  correctAnswers: number;
  score: number;
  totalTimeMs: number;
  answers: AttemptAnswerResult[];
};
type AttemptAnswerResult = {
  questionId: string;
  prompt: string;
  selectedAnswer: string;
  correctAnswer: string;
  isCorrect: boolean;
  explanation: string;
  responseTimeMs: number;
};
type ProgressSummary = {
  attemptsCount: number;
  averageScore: number;
  bestScore: number;
  totalQuestionsAnswered: number;
  totalCorrectAnswers: number;
  totalTimeMs: number;
  lessons: ProgressLessonSummary[];
};
type ProgressLessonSummary = {
  lessonId: string;
  book: string;
  level: string;
  lessonNumber: number;
  title: string;
  attemptsCount: number;
  averageScore: number;
  bestScore: number;
  totalQuestionsAnswered: number;
  totalCorrectAnswers: number;
  totalTimeMs: number;
  lastAttemptAtUtc: string | null;
};
type AttemptHistoryItem = {
  attemptId: string;
  lessonId: string;
  book: string;
  level: string;
  lessonNumber: number;
  title: string;
  category: QuizCategory;
  totalQuestions: number;
  correctAnswers: number;
  score: number;
  totalTimeMs: number;
  completedAtUtc: string | null;
};

const levelOrder = ["A1", "A1.1", "A1.2", "A2", "A2.1", "A2.2", "B1", "B1.1", "B1.2"];

function sortLevels(levels: string[]) {
  return [...levels].sort(
    (a, b) =>
      (levelOrder.indexOf(a) === -1 ? 999 : levelOrder.indexOf(a)) -
      (levelOrder.indexOf(b) === -1 ? 999 : levelOrder.indexOf(b)),
  );
}

const defaultLessonId = "11111111-1111-1111-1111-111111111111";

async function getError(response: Response, fallback: string) {
  try {
    const body = await response.json();
    return body.message ?? fallback;
  } catch {
    return fallback;
  }
}

export default function App() {
  const [language, setLanguage] = useState<Language>(ACTIVE_UI_LANGUAGE);
  const uiLanguage = SHOW_LANGUAGE_SWITCHER ? language : ACTIVE_UI_LANGUAGE;
  const t = getMessages(uiLanguage);
  const [authMode, setAuthMode] = useState<AuthMode>("login");
  const [authOpen, setAuthOpen] = useState(false);
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [displayName, setDisplayName] = useState("");
  const [authError, setAuthError] = useState("");
  const [authLoading, setAuthLoading] = useState(false);
  const [token, setToken] = useState<string | null>(null);
  const [userName, setUserName] = useState("");
  const [progress, setProgress] = useState<ProgressSummary | null>(null);
  const [progressLoading, setProgressLoading] = useState(false);
  const [lessons, setLessons] = useState<Lesson[]>([]);
  const [selectedLessonId, setSelectedLessonId] = useState(defaultLessonId);
  const [selectedBook, setSelectedBook] = useState("Menschen");
  const [selectedLevel, setSelectedLevel] = useState("A1.1");
  const [lessonsLoading, setLessonsLoading] = useState(false);
  const [lessonsError, setLessonsError] = useState("");
  const [history, setHistory] = useState<AttemptHistoryItem[]>([]);
  const [historyLoading, setHistoryLoading] = useState(false);
  const [quizOpen, setQuizOpen] = useState(false);
  const [quizCategory, setQuizCategory] = useState<QuizCategory>("Mixed");
  const [quizQuestions, setQuizQuestions] = useState<QuizQuestion[]>([]);
  const [quizIndex, setQuizIndex] = useState(0);
  const [quizAnswers, setQuizAnswers] = useState<Record<string, string>>({});
  const [quizTimes, setQuizTimes] = useState<Record<string, number>>({});
  const [quizStartedAt, setQuizStartedAt] = useState("");
  const [questionStartedAt, setQuestionStartedAt] = useState(0);
  const [quizLoading, setQuizLoading] = useState(false);
  const [quizSubmitting, setQuizSubmitting] = useState(false);
  const [quizError, setQuizError] = useState("");
  const [quizResult, setQuizResult] = useState<AttemptResult | null>(null);
  const [activePage, setActivePage] = useState<AppPage>("quizzes");

  useEffect(() => {
    document.documentElement.lang = uiLanguage;
    document.documentElement.dir = dirFor(uiLanguage);
  }, [uiLanguage]);

  async function loadProgress(accessToken: string) {
    setProgressLoading(true);
    try {
      const response = await fetch(`${apiBaseUrl}/progress/summary`, {
        headers: { Authorization: `Bearer ${accessToken}` },
      });
      if (response.ok) setProgress(await response.json());
      else if (response.status === 401) {
        localStorage.removeItem("deutschquiz.accessToken");
        setToken(null);
        setProgress(null);
      }
    } finally {
      setProgressLoading(false);
    }
  }

  async function loadHistory(accessToken: string) {
    setHistoryLoading(true);
    try {
      const response = await fetch(`${apiBaseUrl}/progress/history?limit=500`, {
        headers: { Authorization: `Bearer ${accessToken}` },
      });
      if (response.ok) setHistory(await response.json());
      else if (response.status === 401) {
        localStorage.removeItem("deutschquiz.accessToken");
        setToken(null);
        setHistory([]);
      }
    } finally {
      setHistoryLoading(false);
    }
  }

  async function startQuiz(category: QuizCategory) {
    setQuizCategory(category);
    setQuizLoading(true);
    setQuizError("");
    setQuizResult(null);
    try {
      const response = await fetch(
        `${apiBaseUrl}/lessons/${selectedLessonId}/questions?category=${category}`,
      );
      if (!response.ok) throw new Error(t.questionsUnavailable);
      const questions = (await response.json()) as QuizQuestion[];
      if (!questions.length) {
        throw new Error(t.noQuestionsForMode);
      }
      setQuizQuestions(questions);
      setQuizAnswers({});
      setQuizTimes({});
      setQuizIndex(0);
      setQuizStartedAt(new Date().toISOString());
      setQuestionStartedAt(Date.now());
      setQuizOpen(true);
    } catch (error) {
      setQuizError(
        error instanceof Error ? error.message : t.fetchQuestionsFailed,
      );
    } finally {
      setQuizLoading(false);
    }
  }

  function selectAnswer(answer: string) {
    const question = quizQuestions[quizIndex];
    if (!question) return;
    setQuizAnswers((current) => ({ ...current, [question.id]: answer }));
    setQuizTimes((current) =>
      current[question.id] !== undefined
        ? current
        : {
            ...current,
            [question.id]: Math.max(0, Date.now() - questionStartedAt),
          },
    );
  }

  function nextQuestion() {
    if (quizIndex >= quizQuestions.length - 1) return;
    setQuizIndex((current) => current + 1);
    setQuestionStartedAt(Date.now());
  }

  async function submitQuiz() {
    if (!token) {
      setQuizError(t.loginToSaveResult);
      openAuth("login");
      return;
    }

    setQuizSubmitting(true);
    setQuizError("");
    try {
      const answers = quizQuestions.map((question) => ({
        questionId: question.id,
        selectedAnswer: quizAnswers[question.id] ?? "",
        responseTimeMs:
          quizTimes[question.id] ??
          Math.max(0, Date.now() - questionStartedAt),
      }));
      const response = await fetch(`${apiBaseUrl}/attempts`, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${token}`,
        },
        body: JSON.stringify({
          lessonId: selectedLessonId,
          category: quizCategory,
          startedAtUtc: quizStartedAt,
          answers,
        }),
      });
      if (response.status === 401) {
        localStorage.removeItem("deutschquiz.accessToken");
        localStorage.removeItem("deutschquiz.displayName");
        setToken(null);
        setUserName("");
        setQuizError(
          uiLanguage === "fa"
            ? "جلسهٔ ورود منقضی شد؛ لطفاً دوباره وارد شوید."
            : "Your session expired. Please log in again.",
        );
        setQuizOpen(false);
        openAuth("login");
        return;
      }
      if (!response.ok) throw new Error(await getError(response, t.requestFailed));
      const result = (await response.json()) as AttemptResult;
      setQuizResult(result);
      await loadProgress(token);
      await loadHistory(token);
    } catch (error) {
      setQuizError(
        error instanceof Error ? error.message : t.submitResultFailed,
      );
    } finally {
      setQuizSubmitting(false);
    }
  }

  useEffect(() => {
    const timer = window.setTimeout(() => {
      setLessonsLoading(true);
      setLessonsError("");
      void fetch(`${apiBaseUrl}/lessons`)
        .then(async (response) => {
          if (!response.ok) throw new Error(t.lessonsUnavailable);
          return (await response.json()) as Lesson[];
        })
        .then((result) => {
          setLessons(result);
          const initialLesson =
            result.find((lesson) => lesson.id === defaultLessonId) ?? result[0];
          if (initialLesson) {
            setSelectedLessonId(initialLesson.id);
            setSelectedBook(initialLesson.book);
            setSelectedLevel(initialLesson.level);
          }
        })
        .catch((error: unknown) => {
          setLessonsError(
            error instanceof Error ? error.message : t.fetchLessonsFailed,
          );
        })
        .finally(() => setLessonsLoading(false));
    }, 0);

    return () => window.clearTimeout(timer);
  }, [t.fetchLessonsFailed, t.lessonsUnavailable]);

  useEffect(() => {
    const savedToken = localStorage.getItem("deutschquiz.accessToken");
    const savedName = localStorage.getItem("deutschquiz.displayName");
    if (!savedToken) return;

    const timer = window.setTimeout(() => {
      setToken(savedToken);
      setUserName(savedName ?? "");
      void loadProgress(savedToken);
      void loadHistory(savedToken);
    }, 0);

    return () => window.clearTimeout(timer);
  }, []);

  async function submitAuth(event: FormEvent<HTMLFormElement>) {
    event.preventDefault();
    setAuthError("");
    setAuthLoading(true);
    try {
      const response = await fetch(
        `${apiBaseUrl}/auth/${authMode === "login" ? "login" : "register"}`,
        {
          method: "POST",
          headers: { "Content-Type": "application/json" },
          body: JSON.stringify(
            authMode === "login"
              ? { email, password }
              : { email, password, displayName },
          ),
        },
      );
      if (!response.ok) throw new Error(await getError(response, t.requestFailed));
      const result = (await response.json()) as AuthResult;
      localStorage.setItem("deutschquiz.accessToken", result.accessToken);
      localStorage.setItem("deutschquiz.displayName", result.user.displayName);
      setToken(result.accessToken);
      setUserName(result.user.displayName);
      setAuthOpen(false);
      setPassword("");
      await loadProgress(result.accessToken);
      await loadHistory(result.accessToken);
    } catch (error) {
      setAuthError(error instanceof Error ? error.message : t.genericError);
    } finally {
      setAuthLoading(false);
    }
  }

  function logout() {
    localStorage.removeItem("deutschquiz.accessToken");
    localStorage.removeItem("deutschquiz.displayName");
    setToken(null);
    setUserName("");
    setProgress(null);
    setHistory([]);
  }

  function openAuth(mode: AuthMode) {
    setAuthMode(mode);
    setAuthError("");
    setAuthOpen(true);
  }

  const activeQuestion = quizQuestions[quizIndex];
  const selectedLesson = lessons.find((lesson) => lesson.id === selectedLessonId);
  const bookOptions: BookOption[] = Array.from(
    lessons.reduce((books, lesson) => {
      const levels = books.get(lesson.book) ?? [];
      if (!levels.includes(lesson.level)) levels.push(lesson.level);
      books.set(lesson.book, levels);
      return books;
    }, new Map<string, string[]>()),
  ).map(([name, levels]) => ({
    name,
    levels: sortLevels(levels),
  }));
  const selectedBookLevels =
    bookOptions.find((book) => book.name === selectedBook)?.levels ?? [];
  const bookLessons = lessons
    .filter(
      (lesson) =>
        lesson.book === selectedBook && lesson.level === selectedLevel,
    )
    .sort((a, b) => a.number - b.number);
  const selectedBookProgress = progress?.lessons.filter(
    (lesson) =>
      lesson.book === selectedBook && lesson.level === selectedLevel,
  ) ?? [];
  function hasCompletedSection(lessonId: string, category: QuizCategory) {
    return history.some(
      (attempt) => attempt.lessonId === lessonId && attempt.category === category,
    );
  }

  const isLessonComplete = (lessonId: string) =>
    quizModeMeta.every((mode) => hasCompletedSection(lessonId, mode.category));
  const completedLessonIds = new Set(
    lessons.filter((lesson) => isLessonComplete(lesson.id)).map((lesson) => lesson.id),
  );
  const completedBookNames = new Set(
    bookOptions
      .filter((book) => {
        const bookLessonIds = lessons.filter((lesson) => lesson.book === book.name).map((lesson) => lesson.id);
        return bookLessonIds.length > 0 && bookLessonIds.every((lessonId) => completedLessonIds.has(lessonId));
      })
      .map((book) => book.name),
  );

  function selectBook(bookName: string) {
    setSelectedBook(bookName);
    const levels = bookOptions.find((book) => book.name === bookName)?.levels ?? [];
    const nextLevel = levels.includes(selectedLevel) ? selectedLevel : levels[0] ?? selectedLevel;
    setSelectedLevel(nextLevel);
    const firstLesson = lessons.find(
      (lesson) => lesson.book === bookName && lesson.level === nextLevel,
    );
    if (firstLesson) setSelectedLessonId(firstLesson.id);
  }

  function selectLevel(level: string) {
    setSelectedLevel(level);
    const firstLesson = lessons.find(
      (lesson) => lesson.book === selectedBook && lesson.level === level,
    );
    if (firstLesson) setSelectedLessonId(firstLesson.id);
  }

  function categoryLabel(category: QuizCategory) {
    if (category === "Vocabulary") return t.categoryVocabulary;
    if (category === "Grammar") return t.categoryGrammar;
    return t.categoryMixed;
  }

  const textAlign = uiLanguage === "en" ? "text-left" : "text-right";
  const pageLabels = uiLanguage === "fa"
    ? { quizzes: "آزمون‌ها", progress: "پیشرفت", history: "تاریخچه آزمون‌ها" }
    : { quizzes: "Quizzes", progress: "Progress", history: "Quiz history" };

  return (
    <main className="min-h-screen">
      <div className="de-flag h-2 w-full animate-flag rounded-b-2xl" aria-hidden>
        <span /><span /><span />
      </div>

      <div className="mx-auto max-w-6xl px-5 py-6 sm:px-8">
        <header className="animate-rise flex items-center justify-between rounded-3xl border border-line bg-surface/90 px-4 py-3 shadow-sm backdrop-blur sm:px-5">
          <div className="flex items-center gap-3">
            <div className="de-flag h-11 w-8 shrink-0 rounded-xl shadow-md" aria-hidden>
              <span /><span /><span />
            </div>
            <div>
              <p className="font-display text-xl font-extrabold tracking-tight text-de-black">
                DeutschQuiz
              </p>
              <p className="text-xs text-muted">Deutsch lernen · Schritt für Schritt</p>
            </div>
          </div>
          <div className="flex items-center gap-2">
            <nav className="flex items-center gap-1 rounded-full border border-line bg-de-cream p-1" aria-label="Main navigation">
              {(["quizzes", "progress", "history"] as const).map((page) => (
                <button
                  key={page}
                  onClick={() => setActivePage(page)}
                  className={`rounded-full px-3 py-2 text-xs font-bold transition ${activePage === page ? "bg-de-black text-white" : "text-muted hover:bg-white"}`}
                >
                  {pageLabels[page]}
                </button>
              ))}
            </nav>
            {SHOW_LANGUAGE_SWITCHER && (
              <button
                onClick={() => setLanguage(language === "fa" ? "en" : "fa")}
                className="rounded-full border border-line bg-de-cream px-3 py-2 text-xs font-semibold text-muted transition hover:border-de-gold hover:bg-de-gold/30"
              >
                {language === "fa" ? "EN" : "FA"}
              </button>
            )}
            {token ? (
              <div className="flex items-center gap-2">
                <span className="hidden text-sm font-semibold text-muted sm:inline">
                  {userName}
                </span>
                <button
                  onClick={logout}
                  className="rounded-full border border-line bg-surface px-4 py-2 text-sm font-semibold text-de-black hover:bg-de-mist"
                >
                  {t.logout}
                </button>
              </div>
            ) : (
              <button
                onClick={() => openAuth("login")}
                className="rounded-full bg-de-black px-5 py-2.5 text-sm font-semibold text-white shadow-md transition hover:bg-de-red"
              >
                {t.login}
              </button>
            )}
          </div>
        </header>

        <section className="mt-10 grid items-stretch gap-8 lg:grid-cols-[1.15fr_0.85fr]">
          <div
            className="animate-rise relative z-10 self-center rounded-[2rem] border border-line bg-gradient-to-br from-surface via-de-cream to-surface-warm p-6 shadow-sm sm:p-8"
            style={{ animationDelay: "80ms" }}
          >
            <p className="font-display text-5xl font-extrabold leading-none tracking-tight text-de-black sm:text-7xl">
              Deutsch<span className="text-de-red">Quiz</span>
            </p>
            <h1 className="mt-5 max-w-xl text-2xl font-bold leading-10 text-de-black sm:text-3xl">
              {t.heroHeadline}
            </h1>
          </div>

          <div
            className="animate-flag relative flex min-h-[280px] items-center justify-center overflow-hidden rounded-[2rem] border border-line bg-gradient-to-br from-de-mist via-surface to-de-cream p-6 shadow-xl shadow-de-black/10 lg:min-h-[360px]"
            style={{ animationDelay: "200ms" }}
          >
            <img
              src="/germany-flag-map.png"
              alt="Deutschland"
              className="max-h-[300px] w-auto max-w-full object-contain drop-shadow-lg lg:max-h-[340px]"
            />
          </div>
        </section>

        {activePage !== "quizzes" && progress && (
          <section className="mt-12 grid gap-3 sm:grid-cols-4">
            {(
              [
                [t.statAverage, `${Math.round(progress.averageScore)}٪`, "bg-surface-warm border-de-gold/40 text-de-black"],
                [t.statBest, `${progress.bestScore}٪`, "bg-surface-rose border-de-rose/30 text-de-red"],
                [t.statCorrect, `${progress.totalCorrectAnswers}/${progress.totalQuestionsAnswered}`, "bg-de-mist border-line text-de-black"],
                [t.statTime, t.secondsShort(Math.round(progress.totalTimeMs / 1000)), "bg-surface border-line text-de-black"],
              ] as const
            ).map(([label, value, tone]) => (
              <div key={label} className={`rounded-3xl border px-4 py-5 shadow-sm ${tone}`}>
                <p className="text-xs text-muted">{label}</p>
                <p className="font-display mt-2 text-2xl font-bold">{value}</p>
              </div>
            ))}
          </section>
        )}

        {activePage === "progress" && token && progress && (
          <section className="mt-12 rounded-[2rem] border border-line bg-surface p-5 shadow-sm sm:p-7">
            <div className="flex flex-col gap-2 sm:flex-row sm:items-end sm:justify-between">
              <div>
                <p className="text-xs font-bold uppercase tracking-wider text-de-red">
                  {t.progressChartEyebrow}
                </p>
                <h2 className="mt-2 font-display text-2xl font-bold text-de-black">
                  {t.progressChartHeading}
                </h2>
              </div>
              <span className="rounded-full bg-de-gold/40 px-3 py-1 text-xs font-bold text-de-black">
                {selectedBook} {selectedLevel}
              </span>
            </div>
            <div className="mt-6">
              <div className="grid gap-4 lg:grid-cols-3">
                {quizModeMeta.map((mode) => {
                  const categoryAttempts = history.filter(
                    (attempt) =>
                      attempt.book === selectedBook &&
                      attempt.level === selectedLevel &&
                      attempt.category === mode.category,
                  );
                  return (
                    <article key={mode.category} className="rounded-3xl border border-line bg-gradient-to-br from-white to-de-mist p-3">
                      <div className="flex items-center justify-between px-2 pt-1">
                        <h3 className="font-display text-lg font-bold text-de-black">{categoryLabel(mode.category)}</h3>
                        <span className="text-xs text-muted">{categoryAttempts.length}</span>
                      </div>
                      <UserProgressChart
                        attempts={categoryAttempts}
                        lessons={[]}
                        scoreLabel={t.chartScore}
                        averageLabel={t.chartAverage}
                        bestLabel={t.chartBest}
                        emptyLabel={t.chartEmpty}
                        locale={localeFor(uiLanguage)}
                        rtl={uiLanguage === "fa"}
                      />
                    </article>
                  );
                })}
              </div>
            </div>
          </section>
        )}

        {activePage === "progress" && token && progress && (
          <section className="mt-12 rounded-[2rem] border border-line bg-surface p-5 shadow-sm sm:p-7">
            <div className="flex flex-col gap-2 sm:flex-row sm:items-end sm:justify-between">
              <div>
                <p className="text-xs font-bold uppercase tracking-wider text-de-red">Fortschritt</p>
                <h2 className="mt-2 font-display text-2xl font-bold text-de-black">
                  {t.progressHeading(selectedBook, selectedLevel)}
                </h2>
              </div>
              <span className="rounded-full bg-de-gold/40 px-3 py-1 text-xs font-bold text-de-black">
                {t.lessonCount(selectedBookProgress.length)}
              </span>
            </div>
            {selectedBookProgress.length === 0 ? (
              <p className="mt-6 rounded-3xl border border-dashed border-de-amber/50 bg-surface-warm px-4 py-6 text-center text-sm text-muted">
                {t.noProgressInLevel}
              </p>
            ) : (
              <div className="mt-6 grid gap-3 md:grid-cols-2">
                {selectedBookProgress.map((lesson) => (
                  <div
                    key={lesson.lessonId}
                    className="rounded-3xl border border-line bg-gradient-to-br from-white to-de-mist p-4 shadow-sm"
                  >
                    <div className="flex items-start justify-between gap-3">
                      <div>
                        <span className="text-xs font-bold text-de-red">
                          Lektion {lesson.lessonNumber}
                        </span>
                        <p className="mt-1 text-sm font-bold text-de-black" dir="ltr">
                          {lesson.title}
                        </p>
                      </div>
                      <span className="rounded-full bg-de-gold px-3 py-1 text-xs font-bold text-de-black">
                        {Math.round(lesson.averageScore)}٪
                      </span>
                    </div>
                    <div className="mt-4 h-2 overflow-hidden rounded-full bg-line">
                      <div
                        className="h-full rounded-full bg-gradient-to-l from-de-red to-de-rose"
                        style={{ width: `${lesson.averageScore}%` }}
                      />
                    </div>
                    <div className="mt-3 flex gap-4 text-xs text-muted">
                      <span>{t.attemptsCount(lesson.attemptsCount)}</span>
                      <span>{t.bestScore(lesson.bestScore)}</span>
                      <span>{t.secondsShort(Math.round(lesson.totalTimeMs / 1000))}</span>
                    </div>
                  </div>
                ))}
              </div>
            )}
          </section>
        )}

        {activePage === "history" && token && (
          <section className="mt-12 rounded-[2rem] border border-line bg-surface p-5 shadow-sm sm:p-7">
            <div className="flex items-center justify-between gap-4">
              <div>
                <p className="text-xs font-bold uppercase tracking-wider text-de-red">Verlauf</p>
                <h2 className="mt-2 font-display text-2xl font-bold text-de-black">
                  {t.historyHeading}
                </h2>
              </div>
              <button
                onClick={() => void loadHistory(token)}
                disabled={historyLoading}
                className="rounded-full border border-line bg-de-cream px-4 py-2 text-xs font-semibold text-muted disabled:opacity-50"
              >
                {historyLoading ? t.loading : t.refresh}
              </button>
            </div>
            {history.length === 0 ? (
              <p className="mt-6 rounded-3xl border border-dashed border-line bg-de-mist px-4 py-6 text-center text-sm text-muted">
                {historyLoading ? t.fetching : t.noAttemptsYet}
              </p>
            ) : (
              <div className="mt-6 space-y-3">
                {history.map((attempt) => (
                  <div
                    key={attempt.attemptId}
                    className="flex flex-col gap-3 rounded-3xl border border-line bg-gradient-to-l from-surface-warm/40 to-white px-4 py-4 sm:flex-row sm:items-center sm:justify-between"
                  >
                    <div>
                      <div className="flex flex-wrap items-center gap-2">
                        <span className="rounded-full bg-de-red/10 px-2.5 py-1 text-sm font-bold text-de-red">
                          {categoryLabel(attempt.category)}
                        </span>
                        <span className="text-xs text-muted">
                          {attempt.book} {attempt.level} · Lektion {attempt.lessonNumber}
                        </span>
                      </div>
                      <p className="mt-1 text-xs text-muted">
                        {attempt.completedAtUtc
                          ? new Date(attempt.completedAtUtc).toLocaleString(
                              localeFor(uiLanguage),
                              {
                                dateStyle: "medium",
                                timeStyle: "short",
                              },
                            )
                          : t.dateUnknown}
                      </p>
                    </div>
                    <div className="flex items-center gap-5 text-left">
                      <div>
                        <span className="block font-display text-lg font-bold text-de-red">
                          {Math.round(attempt.score)}٪
                        </span>
                        <span className="text-[11px] text-muted">
                          {attempt.correctAnswers}/{attempt.totalQuestions}
                        </span>
                      </div>
                      <div>
                        <span className="block text-sm font-bold text-de-black">
                          {t.secondsShort(Math.round(attempt.totalTimeMs / 1000))}
                        </span>
                        <span className="text-[11px] text-muted">{t.timeLabel}</span>
                      </div>
                    </div>
                  </div>
                ))}
              </div>
            )}
          </section>
        )}

        {activePage === "quizzes" && <section className="mt-12 rounded-[2rem] border border-line bg-surface p-5 shadow-sm sm:p-7">
          <p className="text-xs font-bold uppercase tracking-wider text-de-red">Lehrwerk</p>
          <h2 className="mt-2 font-display text-2xl font-bold text-de-black">{t.pickBook}</h2>
          <div className="mt-6 grid gap-3 sm:grid-cols-2">
            {bookOptions.map((book) => (
              <button
                key={book.name}
                onClick={() => selectBook(book.name)}
                className={`rounded-3xl border p-5 shadow-sm transition ${textAlign} ${
                  completedBookNames.has(book.name)
                    ? selectedBook === book.name
                      ? "border-emerald-600 bg-gradient-to-br from-emerald-700 to-emerald-900 text-white"
                      : "border-emerald-300 bg-gradient-to-br from-emerald-50 to-white text-de-black hover:border-emerald-500"
                    : selectedBook === book.name
                    ? "border-de-black bg-gradient-to-br from-de-black to-surface-ink text-white"
                    : "border-line bg-gradient-to-br from-white to-de-cream text-de-black hover:border-de-red"
                }`}
              >
                <p className="font-display text-xl font-bold" dir="ltr">
                  {completedBookNames.has(book.name) ? "✓ " : ""}{book.name}
                </p>
                <p
                  className={`mt-2 text-xs ${selectedBook === book.name ? "text-de-gold" : "text-muted"}`}
                >
                  {book.levels.join(" · ")}
                </p>
              </button>
            ))}
          </div>
          {selectedBookLevels.length > 1 && (
            <div className="mt-5 flex flex-wrap gap-2">
              {selectedBookLevels.map((level) => (
                <button
                  key={level}
                  onClick={() => selectLevel(level)}
                  className={`rounded-full px-4 py-2 text-xs font-bold transition ${
                    selectedLevel === level
                      ? "bg-de-gold text-de-black shadow-md shadow-de-gold/40"
                      : "border border-line bg-white text-muted hover:border-de-amber hover:bg-surface-warm"
                  }`}
                  dir="ltr"
                >
                  {level}
                </button>
              ))}
            </div>
          )}

          <div className="mt-10 flex flex-col gap-2 sm:flex-row sm:items-end sm:justify-between">
            <div>
              <p className="text-xs font-bold uppercase tracking-wider text-de-red">Lektion</p>
              <h2 className="mt-2 font-display text-2xl font-bold text-de-black">
                {t.pickLessonHeading}
              </h2>
            </div>
            <span className="rounded-full bg-de-mist px-3 py-1 text-xs text-muted">
              {lessonsLoading
                ? t.fetching
                : t.lessonsMeta(bookLessons.length, selectedBook, selectedLevel)}
            </span>
          </div>
          {lessonsError && (
            <p className="mt-4 rounded-2xl border border-de-red/30 bg-surface-rose px-3 py-2 text-sm font-semibold text-de-red">
              {lessonsError}
            </p>
          )}
          <div className="mt-6 grid gap-3 sm:grid-cols-2 lg:grid-cols-3">
            {bookLessons.map((lesson) => (
              <button
                key={lesson.id}
                onClick={() => setSelectedLessonId(lesson.id)}
                className={`rounded-3xl border p-4 shadow-sm transition ${textAlign} ${
                  completedLessonIds.has(lesson.id)
                    ? selectedLessonId === lesson.id
                      ? "border-emerald-600 bg-gradient-to-br from-emerald-100 to-white shadow-emerald-900/10"
                      : "border-emerald-300 bg-gradient-to-br from-emerald-50 to-white hover:border-emerald-500"
                    : selectedLessonId === lesson.id
                      ? "border-de-red bg-gradient-to-br from-surface-rose to-white shadow-de-red/10"
                      : "border-line bg-white hover:border-de-gold hover:bg-surface-warm"
                }`}
              >
                <span className={`text-xs font-bold ${completedLessonIds.has(lesson.id) ? "text-emerald-700" : "text-de-red"}`}>
                  {completedLessonIds.has(lesson.id) ? "✓ " : ""}Lektion {lesson.number}
                </span>
                <p className="mt-2 text-sm font-bold text-de-black" dir="ltr">
                  {lesson.title}
                </p>
              </button>
            ))}
          </div>
        </section>}

        {activePage === "quizzes" && <section className="mt-12 pb-16">
          <div className="mb-6 flex items-end justify-between gap-4">
            <div>
              <p className="text-xs font-bold uppercase tracking-wider text-de-red">Quiz</p>
              <h2 className="mt-2 font-display text-2xl font-bold text-de-black">
                {t.quizTypeHeading}
              </h2>
            </div>
            <span className="rounded-full bg-de-gold/50 px-3 py-1 text-xs font-bold text-de-black">
              Lektion {selectedLesson?.number ?? 1}
            </span>
          </div>
          <div className="grid gap-4 md:grid-cols-3">
            {quizModeMeta.map((mode) => {
              const copy = t.quizModes[mode.category];
              const sectionCompleted = token && hasCompletedSection(selectedLessonId, mode.category);
              return (
                <button
                  key={mode.category}
                  onClick={() => void startQuiz(mode.category)}
                  disabled={quizLoading}
                  className={`group rounded-[1.75rem] border p-5 shadow-sm transition hover:-translate-y-0.5 hover:shadow-lg disabled:cursor-wait ${textAlign} ${sectionCompleted ? "border-emerald-400 bg-gradient-to-br from-emerald-50 to-white" : mode.card}`}
                >
                  <span
                    className={`inline-block rounded-full px-3 py-1 text-[10px] font-bold uppercase tracking-wider ${sectionCompleted ? "bg-emerald-600 text-white" : mode.accent}`}
                  >
                    {sectionCompleted ? `✓ ${mode.subtitle}` : mode.subtitle}
                  </span>
                  <h3 className="mt-4 text-lg font-bold text-de-black">{copy.title}</h3>
                  <p className="mt-2 text-sm leading-7 text-muted">{copy.description}</p>
                  <div className="mt-5 text-sm font-bold text-de-red group-hover:underline">
                    {t.start}
                  </div>
                </button>
              );
            })}
          </div>
        </section>}
      </div>

      {quizOpen && activeQuestion && (
        <div
          className="fixed inset-0 z-40 grid place-items-center bg-de-black/50 px-5 py-6 backdrop-blur-sm"
          onMouseDown={() => !quizSubmitting && setQuizOpen(false)}
        >
          <div
            className="max-h-[90vh] w-full max-w-2xl overflow-y-auto rounded-[2rem] border border-line bg-surface p-6 shadow-2xl sm:p-8"
            onMouseDown={(event) => event.stopPropagation()}
          >
            {quizResult ? (
              <div className="text-center">
                <div className="de-flag mx-auto h-16 w-12 rounded-2xl shadow-md" aria-hidden>
                  <span /><span /><span />
                </div>
                <p className="mt-5 text-xs font-bold uppercase tracking-wider text-de-red">
                  Ergebnis
                </p>
                <h2 className="font-display mt-2 text-4xl font-extrabold text-de-black">
                  {Math.round(quizResult.score)}٪
                </h2>
                <p className="mt-3 text-sm text-muted">
                  {t.resultCorrectSummary(
                    quizResult.correctAnswers,
                    quizResult.totalQuestions,
                    Math.round(quizResult.totalTimeMs / 1000),
                  )}
                </p>
                <div className={`mt-7 space-y-2 ${textAlign}`}>
                  {quizResult.answers.map((answer, index) => (
                    <div
                      key={answer.questionId}
                      className={`rounded-3xl border p-4 ${
                        answer.isCorrect
                          ? "border-de-gold/60 bg-surface-warm"
                          : "border-de-rose/40 bg-surface-rose"
                      }`}
                    >
                      <p className="text-sm font-bold text-de-black" dir="ltr">
                        {index + 1}. {answer.prompt}
                      </p>
                      <p className="mt-2 text-xs text-muted" dir="ltr">
                        {t.yourAnswer}{" "}
                        <span className="font-bold">{answer.selectedAnswer}</span>
                        {!answer.isCorrect && (
                          <>
                            {" "}
                            · {t.correctLabel}{" "}
                            <span className="font-bold text-de-black">{answer.correctAnswer}</span>
                          </>
                        )}
                      </p>
                      {answer.explanation && (
                        <p className="mt-2 text-xs leading-6 text-muted">{answer.explanation}</p>
                      )}
                    </div>
                  ))}
                </div>
                <button
                  onClick={() => setQuizOpen(false)}
                  className="mt-7 rounded-2xl bg-de-black px-6 py-3 text-sm font-bold text-white"
                >
                  {t.back}
                </button>
              </div>
            ) : (
              <>
                <div className="flex items-start justify-between gap-4">
                  <div>
                    <p className="text-xs font-bold uppercase tracking-wider text-de-red">
                      {quizCategory === "Vocabulary"
                        ? "Wortschatz"
                        : quizCategory === "Grammar"
                          ? "Grammatik"
                          : "Komplett"}
                    </p>
                    <h2 className="mt-2 font-display text-2xl font-bold text-de-black">
                      {t.questionOf(quizIndex + 1, quizQuestions.length)}
                    </h2>
                  </div>
                  <button onClick={() => setQuizOpen(false)} className="text-xl text-muted">
                    ×
                  </button>
                </div>
                <div className="mt-5 h-2 overflow-hidden rounded-full bg-line">
                  <div
                    className="h-full rounded-full bg-gradient-to-l from-de-red to-de-gold transition-all"
                    style={{
                      width: `${((quizIndex + 1) / quizQuestions.length) * 100}%`,
                    }}
                  />
                </div>
                <div className="mt-8 rounded-[1.75rem] border border-line bg-gradient-to-b from-de-cream to-background p-5 sm:p-7">
                  <p
                    className="text-center text-xl font-bold leading-9 text-de-black"
                    dir="ltr"
                  >
                    {activeQuestion.prompt}
                  </p>
                  <div className="mt-7 grid gap-2">
                    {activeQuestion.options.map((option) => {
                      const selected = quizAnswers[activeQuestion.id] === option;
                      return (
                        <button
                          key={option}
                          onClick={() => selectAnswer(option)}
                          className={`rounded-2xl border px-4 py-3.5 text-center text-base font-semibold transition ${
                            selected
                              ? "border-de-black bg-de-black text-white shadow-md"
                              : "border-line bg-white text-de-black hover:border-de-red hover:bg-surface-rose"
                          }`}
                          dir="ltr"
                        >
                          {option}
                        </button>
                      );
                    })}
                  </div>
                </div>
                {quizError && (
                  <p className="mt-4 rounded-2xl border border-de-red/30 bg-surface-rose px-3 py-2 text-center text-xs font-semibold text-de-red">
                    {quizError}
                  </p>
                )}
                <div className="mt-6 flex items-center justify-between gap-3">
                  <button
                    onClick={() => setQuizOpen(false)}
                    className="rounded-2xl border border-line px-4 py-3 text-sm font-semibold text-muted"
                  >
                    {t.cancel}
                  </button>
                  {quizIndex < quizQuestions.length - 1 ? (
                    <button
                      onClick={nextQuestion}
                      disabled={!quizAnswers[activeQuestion.id]}
                      className="rounded-2xl bg-de-red px-5 py-3 text-sm font-bold text-white shadow-md shadow-de-red/20 disabled:opacity-40"
                    >
                      {t.next}
                    </button>
                  ) : (
                    <button
                      onClick={() => void submitQuiz()}
                      disabled={!quizAnswers[activeQuestion.id] || quizSubmitting}
                      className="rounded-2xl bg-de-black px-5 py-3 text-sm font-bold text-white disabled:opacity-40"
                    >
                      {quizSubmitting ? t.submitting : t.submitQuiz}
                    </button>
                  )}
                </div>
              </>
            )}
          </div>
        </div>
      )}

      {authOpen && (
        <div
          className="fixed inset-0 z-50 grid place-items-center bg-de-black/45 px-5 backdrop-blur-sm"
          onMouseDown={() => setAuthOpen(false)}
        >
          <div
            className="w-full max-w-md rounded-[2rem] border border-line bg-surface p-6 shadow-2xl"
            onMouseDown={(event) => event.stopPropagation()}
          >
            <div className="de-flag mb-5 h-2 w-full rounded-full" aria-hidden>
              <span /><span /><span />
            </div>
            <div className="flex items-start justify-between">
              <div>
                <p className="font-display text-sm font-bold text-de-black">DeutschQuiz</p>
                <h2 className="mt-2 text-2xl font-bold text-de-black">
                  {authMode === "login" ? t.login : t.register}
                </h2>
              </div>
              <button onClick={() => setAuthOpen(false)} className="text-xl text-muted">
                ×
              </button>
            </div>
            <form onSubmit={submitAuth} className="mt-6 space-y-3">
              {authMode === "register" && (
                <input
                  required
                  value={displayName}
                  onChange={(event) => setDisplayName(event.target.value)}
                  placeholder={t.displayNamePlaceholder}
                  className="w-full rounded-2xl border border-line bg-de-cream px-4 py-3 text-sm outline-none focus:border-de-gold"
                />
              )}
              <input
                required
                type="email"
                value={email}
                onChange={(event) => setEmail(event.target.value)}
                placeholder={t.emailPlaceholder}
                className="w-full rounded-2xl border border-line bg-de-cream px-4 py-3 text-sm outline-none focus:border-de-gold"
              />
              <input
                required
                minLength={8}
                type="password"
                value={password}
                onChange={(event) => setPassword(event.target.value)}
                placeholder={t.passwordPlaceholder}
                className="w-full rounded-2xl border border-line bg-de-cream px-4 py-3 text-sm outline-none focus:border-de-gold"
              />
              {authError && (
                <p className="rounded-2xl border border-de-red/30 bg-surface-rose px-3 py-2 text-xs font-semibold text-de-red">
                  {authError}
                </p>
              )}
              <button
                disabled={authLoading}
                className="w-full rounded-2xl bg-de-red px-4 py-3.5 text-sm font-bold text-white shadow-md shadow-de-red/25 disabled:opacity-60"
              >
                {authLoading
                  ? t.authSubmitting
                  : authMode === "login"
                    ? t.login
                    : t.register}
              </button>
            </form>
            <button
              onClick={() => {
                setAuthMode(authMode === "login" ? "register" : "login");
                setAuthError("");
              }}
              className="mt-4 w-full text-center text-xs font-semibold text-muted"
            >
              {authMode === "login" ? t.noAccountRegister : t.haveAccountLogin}
            </button>
          </div>
        </div>
      )}
    </main>
  );
}
