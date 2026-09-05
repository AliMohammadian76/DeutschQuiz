import { FormEvent, useEffect, useState } from "react";

const apiBaseUrl =
  import.meta.env.VITE_API_BASE_URL ?? "http://localhost:5083/api";

const quizModes = [
  {
    category: "Vocabulary",
    title: "واژگان",
    subtitle: "Wortschatz",
    description: "کلمات کلیدی درس را مرور کن.",
    accent: "bg-de-black text-white",
  },
  {
    category: "Grammar",
    title: "گرامر",
    subtitle: "Grammatik",
    description: "ساختارهای پایه و جمله‌سازی را بسنج.",
    accent: "bg-de-red text-white",
  },
  {
    category: "Mixed",
    title: "آزمون جامع",
    subtitle: "Komplett",
    description: "ترکیبی از واژگان و گرامر.",
    accent: "bg-de-gold text-de-black",
  },
] as const;

type AuthMode = "login" | "register";
type QuizCategory = (typeof quizModes)[number]["category"];
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

const defaultLessonId = "11111111-1111-1111-1111-111111111111";

async function getError(response: Response) {
  try {
    const body = await response.json();
    return body.message ?? "درخواست انجام نشد.";
  } catch {
    return "درخواست انجام نشد.";
  }
}

export default function App() {
  const [language, setLanguage] = useState("fa");
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

  async function loadProgress(accessToken: string) {
    setProgressLoading(true);
    try {
      const response = await fetch(`${apiBaseUrl}/progress/summary`, {
        headers: { Authorization: `Bearer ${accessToken}` },
      });
      if (response.ok) setProgress(await response.json());
    } finally {
      setProgressLoading(false);
    }
  }

  async function loadHistory(accessToken: string) {
    setHistoryLoading(true);
    try {
      const response = await fetch(`${apiBaseUrl}/progress/history?limit=10`, {
        headers: { Authorization: `Bearer ${accessToken}` },
      });
      if (response.ok) setHistory(await response.json());
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
      if (!response.ok) throw new Error("سؤال‌های این آزمون در دسترس نیست.");
      const questions = (await response.json()) as QuizQuestion[];
      if (!questions.length) {
        throw new Error("برای این حالت هنوز سؤالی ثبت نشده است.");
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
        error instanceof Error ? error.message : "دریافت سؤال‌ها انجام نشد.",
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
      setQuizError("برای ثبت نتیجه، ابتدا وارد حساب کاربری شو.");
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
      if (!response.ok) throw new Error(await getError(response));
      const result = (await response.json()) as AttemptResult;
      setQuizResult(result);
      await loadProgress(token);
      await loadHistory(token);
    } catch (error) {
      setQuizError(
        error instanceof Error ? error.message : "ثبت نتیجه انجام نشد.",
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
          if (!response.ok) throw new Error("فهرست درس‌ها در دسترس نیست.");
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
            error instanceof Error
              ? error.message
              : "دریافت درس‌ها انجام نشد.",
          );
        })
        .finally(() => setLessonsLoading(false));
    }, 0);

    return () => window.clearTimeout(timer);
  }, []);

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
      if (!response.ok) throw new Error(await getError(response));
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
      setAuthError(error instanceof Error ? error.message : "خطایی رخ داد.");
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
    levels: [...levels].sort(),
  }));
  const selectedBookLevels =
    bookOptions.find((book) => book.name === selectedBook)?.levels ?? [];
  const bookLessons = lessons.filter(
    (lesson) =>
      lesson.book === selectedBook && lesson.level === selectedLevel,
  );
  const selectedBookProgress = progress?.lessons.filter(
    (lesson) =>
      lesson.book === selectedBook && lesson.level === selectedLevel,
  ) ?? [];

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

  return (
    <main className="min-h-screen">
      <div className="de-flag h-1.5 w-full animate-flag" aria-hidden>
        <span /><span /><span />
      </div>

      <div className="mx-auto max-w-6xl px-5 py-6 sm:px-8">
        <header className="animate-rise flex items-center justify-between">
          <div className="flex items-center gap-3">
            <div className="de-flag h-10 w-7 shrink-0 rounded-sm shadow-sm" aria-hidden>
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
            <button
              onClick={() => setLanguage(language === "fa" ? "en" : "fa")}
              className="border border-line bg-surface px-3 py-2 text-xs font-semibold text-muted transition hover:border-de-black"
            >
              {language === "fa" ? "EN" : "FA"}
            </button>
            {token ? (
              <div className="flex items-center gap-2">
                <span className="hidden text-sm font-semibold text-muted sm:inline">
                  {userName}
                </span>
                <button
                  onClick={logout}
                  className="border border-line bg-surface px-4 py-2 text-sm font-semibold text-de-black"
                >
                  خروج
                </button>
              </div>
            ) : (
              <button
                onClick={() => openAuth("login")}
                className="bg-de-black px-4 py-2 text-sm font-semibold text-white transition hover:bg-de-red"
              >
                ورود
              </button>
            )}
          </div>
        </header>

        <section className="mt-12 grid items-stretch gap-10 lg:grid-cols-[1.15fr_0.85fr]">
          <div className="animate-rise relative z-10 self-center" style={{ animationDelay: "80ms" }}>
            <p className="font-display text-5xl font-extrabold leading-none tracking-tight text-de-black sm:text-7xl">
              DeutschQuiz
            </p>
            <h1 className="mt-5 max-w-xl text-2xl font-bold leading-10 text-de-black sm:text-3xl">
              آلمانی را با آزمون‌های کوتاه و دقیق تمرین کن.
            </h1>
            <p className="mt-4 max-w-md text-sm leading-7 text-muted">
              {selectedLesson?.book ?? "Menschen"} · {selectedLesson?.level ?? "A1.1"} ·
              Lektion {selectedLesson?.number ?? 1}
            </p>
            <div className="mt-8 flex flex-wrap gap-3">
              <button
                onClick={() => void startQuiz("Mixed")}
                disabled={quizLoading}
                className="bg-de-red px-6 py-3.5 text-sm font-bold text-white transition hover:brightness-95 disabled:opacity-60"
              >
                {quizLoading ? "آماده‌سازی..." : "شروع آزمون"}
              </button>
              <button
                onClick={() => (token ? void loadProgress(token) : openAuth("login"))}
                className="border border-de-black bg-transparent px-6 py-3.5 text-sm font-bold text-de-black transition hover:bg-de-black hover:text-white"
              >
                پیشرفت
              </button>
            </div>
          </div>

          <div
            className="animate-flag relative min-h-[280px] overflow-hidden lg:min-h-[360px]"
            style={{ animationDelay: "200ms" }}
          >
            <div className="de-flag absolute inset-0" aria-hidden>
              <span /><span /><span />
            </div>
            <div className="absolute inset-0 flex flex-col justify-end bg-gradient-to-t from-black/50 to-transparent p-6 text-white">
              <p className="font-display text-sm font-bold uppercase tracking-[0.2em]">
                Bundesrepublik
              </p>
              <p className="mt-2 text-lg font-bold">
                {selectedLesson ? "۲۰ سؤال آماده" : "درس را انتخاب کن"}
              </p>
              <p className="mt-1 text-sm text-white/80">
                سطح {selectedLesson?.level ?? "A1.1"}
                {progress ? ` · میانگین ${Math.round(progress.averageScore)}٪` : ""}
              </p>
            </div>
          </div>
        </section>

        {progress && (
          <section className="mt-14 grid gap-px overflow-hidden border border-line bg-line sm:grid-cols-4">
            {(
              [
                ["میانگین", `${Math.round(progress.averageScore)}٪`],
                ["بهترین", `${progress.bestScore}٪`],
                ["درست", `${progress.totalCorrectAnswers}/${progress.totalQuestionsAnswered}`],
                ["زمان", `${Math.round(progress.totalTimeMs / 1000)}ث`],
              ] as const
            ).map(([label, value]) => (
              <div key={label} className="bg-surface px-4 py-5">
                <p className="text-xs text-muted">{label}</p>
                <p className="font-display mt-2 text-2xl font-bold text-de-black">{value}</p>
              </div>
            ))}
          </section>
        )}

        {token && progress && (
          <section className="mt-14 border-t border-line pt-10">
            <div className="flex flex-col gap-2 sm:flex-row sm:items-end sm:justify-between">
              <div>
                <p className="text-xs font-bold uppercase tracking-wider text-de-red">Fortschritt</p>
                <h2 className="mt-2 font-display text-2xl font-bold text-de-black">
                  پیشرفت · {selectedBook} {selectedLevel}
                </h2>
              </div>
              <span className="text-xs text-muted">{selectedBookProgress.length} درس</span>
            </div>
            {selectedBookProgress.length === 0 ? (
              <p className="mt-6 border border-dashed border-line bg-surface px-4 py-6 text-center text-sm text-muted">
                هنوز در این سطح آزمونی ثبت نکرده‌ای.
              </p>
            ) : (
              <div className="mt-6 grid gap-3 md:grid-cols-2">
                {selectedBookProgress.map((lesson) => (
                  <div key={lesson.lessonId} className="border border-line bg-surface p-4">
                    <div className="flex items-start justify-between gap-3">
                      <div>
                        <span className="text-xs font-bold text-de-red">
                          Lektion {lesson.lessonNumber}
                        </span>
                        <p className="mt-1 text-sm font-bold text-de-black" dir="ltr">
                          {lesson.title}
                        </p>
                      </div>
                      <span className="bg-de-gold px-2 py-1 text-xs font-bold text-de-black">
                        {Math.round(lesson.averageScore)}٪
                      </span>
                    </div>
                    <div className="mt-4 h-1 overflow-hidden bg-line">
                      <div
                        className="h-full bg-de-red"
                        style={{ width: `${lesson.averageScore}%` }}
                      />
                    </div>
                    <div className="mt-3 flex gap-4 text-xs text-muted">
                      <span>{lesson.attemptsCount} آزمون</span>
                      <span>بهترین {lesson.bestScore}٪</span>
                      <span>{Math.round(lesson.totalTimeMs / 1000)}ث</span>
                    </div>
                  </div>
                ))}
              </div>
            )}
          </section>
        )}

        {token && (
          <section className="mt-14 border-t border-line pt-10">
            <div className="flex items-center justify-between gap-4">
              <div>
                <p className="text-xs font-bold uppercase tracking-wider text-de-red">Verlauf</p>
                <h2 className="mt-2 font-display text-2xl font-bold text-de-black">
                  تاریخچه‌ی آزمون‌ها
                </h2>
              </div>
              <button
                onClick={() => void loadHistory(token)}
                disabled={historyLoading}
                className="border border-line px-3 py-2 text-xs font-semibold text-muted disabled:opacity-50"
              >
                {historyLoading ? "..." : "به‌روزرسانی"}
              </button>
            </div>
            {history.length === 0 ? (
              <p className="mt-6 border border-dashed border-line bg-surface px-4 py-6 text-center text-sm text-muted">
                {historyLoading ? "در حال دریافت..." : "هنوز آزمونی ثبت نکرده‌ای."}
              </p>
            ) : (
              <div className="mt-6 divide-y divide-line border border-line bg-surface">
                {history.map((attempt) => (
                  <div
                    key={attempt.attemptId}
                    className="flex flex-col gap-3 px-4 py-4 sm:flex-row sm:items-center sm:justify-between"
                  >
                    <div>
                      <div className="flex flex-wrap items-center gap-2">
                        <span className="text-sm font-bold text-de-black">
                          {attempt.category === "Vocabulary"
                            ? "واژگان"
                            : attempt.category === "Grammar"
                              ? "گرامر"
                              : "جامع"}
                        </span>
                        <span className="text-xs text-muted">
                          {attempt.book} {attempt.level} · Lektion {attempt.lessonNumber}
                        </span>
                      </div>
                      <p className="mt-1 text-xs text-muted">
                        {attempt.completedAtUtc
                          ? new Date(attempt.completedAtUtc).toLocaleString("fa-IR", {
                              dateStyle: "medium",
                              timeStyle: "short",
                            })
                          : "تاریخ نامشخص"}
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
                          {Math.round(attempt.totalTimeMs / 1000)}ث
                        </span>
                        <span className="text-[11px] text-muted">زمان</span>
                      </div>
                    </div>
                  </div>
                ))}
              </div>
            )}
          </section>
        )}

        <section className="mt-16 border-t border-line pt-10">
          <p className="text-xs font-bold uppercase tracking-wider text-de-red">Lehrwerk</p>
          <h2 className="mt-2 font-display text-2xl font-bold text-de-black">انتخاب کتاب</h2>
          <div className="mt-6 grid gap-3 sm:grid-cols-2">
            {bookOptions.map((book) => (
              <button
                key={book.name}
                onClick={() => selectBook(book.name)}
                className={`border p-5 text-right transition ${
                  selectedBook === book.name
                    ? "border-de-black bg-de-black text-white"
                    : "border-line bg-surface text-de-black hover:border-de-red"
                }`}
              >
                <p className="font-display text-xl font-bold" dir="ltr">
                  {book.name}
                </p>
                <p
                  className={`mt-2 text-xs ${selectedBook === book.name ? "text-white/70" : "text-muted"}`}
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
                  className={`px-4 py-2 text-xs font-bold transition ${
                    selectedLevel === level
                      ? "bg-de-gold text-de-black"
                      : "border border-line bg-surface text-muted hover:border-de-black"
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
              <h2 className="mt-2 font-display text-2xl font-bold text-de-black">انتخاب درس</h2>
            </div>
            <span className="text-xs text-muted">
              {lessonsLoading
                ? "در حال دریافت..."
                : `${bookLessons.length} درس · ${selectedBook} ${selectedLevel}`}
            </span>
          </div>
          {lessonsError && (
            <p className="mt-4 border border-de-red/30 bg-de-red/5 px-3 py-2 text-sm font-semibold text-de-red">
              {lessonsError}
            </p>
          )}
          <div className="mt-6 grid gap-2 sm:grid-cols-2 lg:grid-cols-3">
            {bookLessons.map((lesson) => (
              <button
                key={lesson.id}
                onClick={() => setSelectedLessonId(lesson.id)}
                className={`border p-4 text-right transition ${
                  selectedLessonId === lesson.id
                    ? "border-de-red bg-de-red/5"
                    : "border-line bg-surface hover:border-de-black"
                }`}
              >
                <span className="text-xs font-bold text-de-red">Lektion {lesson.number}</span>
                <p className="mt-2 text-sm font-bold text-de-black" dir="ltr">
                  {lesson.title}
                </p>
              </button>
            ))}
          </div>
        </section>

        <section className="mt-14 border-t border-line pb-16 pt-10">
          <div className="mb-6 flex items-end justify-between gap-4">
            <div>
              <p className="text-xs font-bold uppercase tracking-wider text-de-red">Quiz</p>
              <h2 className="mt-2 font-display text-2xl font-bold text-de-black">نوع آزمون</h2>
            </div>
            <span className="text-xs text-muted">Lektion {selectedLesson?.number ?? 1}</span>
          </div>
          <div className="grid gap-3 md:grid-cols-3">
            {quizModes.map((mode) => (
              <button
                key={mode.category}
                onClick={() => void startQuiz(mode.category)}
                disabled={quizLoading}
                className="group border border-line bg-surface p-5 text-right transition hover:border-de-black disabled:cursor-wait"
              >
                <span
                  className={`inline-block px-2 py-1 text-[10px] font-bold uppercase tracking-wider ${mode.accent}`}
                >
                  {mode.subtitle}
                </span>
                <h3 className="mt-4 text-lg font-bold text-de-black">{mode.title}</h3>
                <p className="mt-2 text-sm leading-7 text-muted">{mode.description}</p>
                <div className="mt-5 text-sm font-bold text-de-red group-hover:underline">
                  شروع
                </div>
              </button>
            ))}
          </div>
        </section>
      </div>

      {quizOpen && activeQuestion && (
        <div
          className="fixed inset-0 z-40 grid place-items-center bg-de-black/50 px-5 py-6 backdrop-blur-sm"
          onMouseDown={() => !quizSubmitting && setQuizOpen(false)}
        >
          <div
            className="max-h-[90vh] w-full max-w-2xl overflow-y-auto border border-line bg-surface p-6 shadow-2xl sm:p-8"
            onMouseDown={(event) => event.stopPropagation()}
          >
            {quizResult ? (
              <div className="text-center">
                <div className="de-flag mx-auto h-16 w-12" aria-hidden>
                  <span /><span /><span />
                </div>
                <p className="mt-5 text-xs font-bold uppercase tracking-wider text-de-red">
                  Ergebnis
                </p>
                <h2 className="font-display mt-2 text-4xl font-extrabold text-de-black">
                  {Math.round(quizResult.score)}٪
                </h2>
                <p className="mt-3 text-sm text-muted">
                  {quizResult.correctAnswers} از {quizResult.totalQuestions} درست ·{" "}
                  {Math.round(quizResult.totalTimeMs / 1000)} ثانیه
                </p>
                <div className="mt-7 space-y-2 text-right">
                  {quizResult.answers.map((answer, index) => (
                    <div
                      key={answer.questionId}
                      className={`border p-4 ${
                        answer.isCorrect
                          ? "border-de-gold/60 bg-de-gold/10"
                          : "border-de-red/30 bg-de-red/5"
                      }`}
                    >
                      <p className="text-sm font-bold text-de-black" dir="ltr">
                        {index + 1}. {answer.prompt}
                      </p>
                      <p className="mt-2 text-xs text-muted" dir="ltr">
                        پاسخ تو: <span className="font-bold">{answer.selectedAnswer}</span>
                        {!answer.isCorrect && (
                          <>
                            {" "}
                            · درست:{" "}
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
                  className="mt-7 bg-de-black px-6 py-3 text-sm font-bold text-white"
                >
                  بازگشت
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
                      سؤال {quizIndex + 1} از {quizQuestions.length}
                    </h2>
                  </div>
                  <button onClick={() => setQuizOpen(false)} className="text-xl text-muted">
                    ×
                  </button>
                </div>
                <div className="mt-5 h-1 overflow-hidden bg-line">
                  <div
                    className="h-full bg-de-red transition-all"
                    style={{
                      width: `${((quizIndex + 1) / quizQuestions.length) * 100}%`,
                    }}
                  />
                </div>
                <div className="mt-8 border border-line bg-background p-5 sm:p-7">
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
                          className={`border px-4 py-3.5 text-center text-base font-semibold transition ${
                            selected
                              ? "border-de-black bg-de-black text-white"
                              : "border-line bg-surface text-de-black hover:border-de-red"
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
                  <p className="mt-4 border border-de-red/30 bg-de-red/5 px-3 py-2 text-center text-xs font-semibold text-de-red">
                    {quizError}
                  </p>
                )}
                <div className="mt-6 flex items-center justify-between gap-3">
                  <button
                    onClick={() => setQuizOpen(false)}
                    className="border border-line px-4 py-3 text-sm font-semibold text-muted"
                  >
                    انصراف
                  </button>
                  {quizIndex < quizQuestions.length - 1 ? (
                    <button
                      onClick={nextQuestion}
                      disabled={!quizAnswers[activeQuestion.id]}
                      className="bg-de-red px-5 py-3 text-sm font-bold text-white disabled:opacity-40"
                    >
                      بعدی
                    </button>
                  ) : (
                    <button
                      onClick={() => void submitQuiz()}
                      disabled={!quizAnswers[activeQuestion.id] || quizSubmitting}
                      className="bg-de-black px-5 py-3 text-sm font-bold text-white disabled:opacity-40"
                    >
                      {quizSubmitting ? "ثبت..." : "ثبت آزمون"}
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
            className="w-full max-w-md border border-line bg-surface p-6 shadow-2xl"
            onMouseDown={(event) => event.stopPropagation()}
          >
            <div className="de-flag mb-5 h-1.5 w-full" aria-hidden>
              <span /><span /><span />
            </div>
            <div className="flex items-start justify-between">
              <div>
                <p className="font-display text-sm font-bold text-de-black">DeutschQuiz</p>
                <h2 className="mt-2 text-2xl font-bold text-de-black">
                  {authMode === "login" ? "ورود" : "ثبت‌نام"}
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
                  placeholder="نام نمایشی"
                  className="w-full border border-line bg-background px-4 py-3 text-sm outline-none focus:border-de-black"
                />
              )}
              <input
                required
                type="email"
                value={email}
                onChange={(event) => setEmail(event.target.value)}
                placeholder="ایمیل"
                className="w-full border border-line bg-background px-4 py-3 text-sm outline-none focus:border-de-black"
              />
              <input
                required
                minLength={8}
                type="password"
                value={password}
                onChange={(event) => setPassword(event.target.value)}
                placeholder="رمز عبور (حداقل ۸ کاراکتر)"
                className="w-full border border-line bg-background px-4 py-3 text-sm outline-none focus:border-de-black"
              />
              {authError && (
                <p className="border border-de-red/30 bg-de-red/5 px-3 py-2 text-xs font-semibold text-de-red">
                  {authError}
                </p>
              )}
              <button
                disabled={authLoading}
                className="w-full bg-de-red px-4 py-3.5 text-sm font-bold text-white disabled:opacity-60"
              >
                {authLoading
                  ? "در حال ارسال..."
                  : authMode === "login"
                    ? "ورود"
                    : "ثبت‌نام"}
              </button>
            </form>
            <button
              onClick={() => {
                setAuthMode(authMode === "login" ? "register" : "login");
                setAuthError("");
              }}
              className="mt-4 w-full text-center text-xs font-semibold text-muted"
            >
              {authMode === "login"
                ? "حساب نداری؟ ثبت‌نام کن"
                : "قبلاً حساب ساخته‌ای؟ وارد شو"}
            </button>
          </div>
        </div>
      )}
    </main>
  );
}
