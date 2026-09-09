import { FormEvent, useEffect, useMemo, useState } from "react";
import {
  ACTIVE_UI_LANGUAGE,
  Language,
  SHOW_LANGUAGE_SWITCHER,
  dirFor,
  getMessages,
  localeFor,
} from "./i18n";
import {
  clearDraft,
  draftMatches,
  loadDraft,
  saveDraft,
  type QuizCategory,
  type QuizDraft,
  type QuizQuestion,
} from "./quizDraft";
import { UserProgressChart } from "./UserProgressChart";
import {
  applyTheme,
  getStoredTheme,
  type Theme,
} from "./theme";

const apiBaseUrl =
  import.meta.env.VITE_API_BASE_URL ?? "http://localhost:5083/api";

const quizModeMeta = [
  {
    category: "Vocabulary",
    subtitle: "Wortschatz",
    accent: "bg-surface-ink text-white",
    card: "border-line bg-surface",
  },
  {
    category: "Grammar",
    subtitle: "Grammatik",
    accent: "bg-de-red text-white",
    card: "border-de-red/25 bg-surface",
  },
  {
    category: "Mixed",
    subtitle: "Komplett",
    accent: "bg-de-gold text-de-black",
    card: "border-de-gold/35 bg-surface",
  },
] as const;

type AuthMode = "login" | "register";
type AppPage = "quizzes" | "progress" | "history" | "quiz" | "translator";
type QuizPickerStep = "book" | "level" | "lesson" | "mode";
type TranslationDirection = "de-fa" | "fa-de";
type AuthResult = { accessToken: string; user: { displayName: string } };
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
  const [quizDraft, setQuizDraft] = useState<QuizDraft | null>(() => loadDraft());
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
  const [pickerStep, setPickerStep] = useState<QuizPickerStep>("book");
  const [theme, setTheme] = useState<Theme>(() => getStoredTheme());
  const [settingsOpen, setSettingsOpen] = useState(false);
  const [justFinishedScore, setJustFinishedScore] = useState<number | null>(null);
  const [translationInput, setTranslationInput] = useState("");
  const [translationOutput, setTranslationOutput] = useState("");
  const [translationDirection, setTranslationDirection] =
    useState<TranslationDirection>("de-fa");
  const [translationLoading, setTranslationLoading] = useState(false);
  const [translationError, setTranslationError] = useState("");

  useEffect(() => {
    document.documentElement.lang = uiLanguage;
    document.documentElement.dir = dirFor(uiLanguage);
  }, [uiLanguage]);

  useEffect(() => {
    applyTheme(theme);
  }, [theme]);

  useEffect(() => {
    if (theme !== "system") return;
    const media = window.matchMedia("(prefers-color-scheme: light)");
    const syncTheme = () => applyTheme("system");
    media.addEventListener("change", syncTheme);
    return () => media.removeEventListener("change", syncTheme);
  }, [theme]);

  useEffect(() => {
    if (activePage !== "quiz" || quizResult || !quizQuestions.length || !quizStartedAt) {
      return;
    }
    const draft: QuizDraft = {
      lessonId: selectedLessonId,
      category: quizCategory,
      questions: quizQuestions,
      answers: quizAnswers,
      times: quizTimes,
      quizIndex,
      startedAtUtc: quizStartedAt,
    };
    saveDraft(draft);
    setQuizDraft(draft);
  }, [
    activePage,
    quizResult,
    quizQuestions,
    quizAnswers,
    quizTimes,
    quizIndex,
    quizStartedAt,
    quizCategory,
    selectedLessonId,
  ]);

  function applyDraft(draft: QuizDraft) {
    const lesson = lessons.find((item) => item.id === draft.lessonId);
    if (lesson) {
      setSelectedBook(lesson.book);
      setSelectedLevel(lesson.level);
    }
    setSelectedLessonId(draft.lessonId);
    setQuizCategory(draft.category);
    setQuizQuestions(draft.questions);
    setQuizAnswers(draft.answers);
    setQuizTimes(draft.times);
    setQuizIndex(draft.quizIndex);
    setQuizStartedAt(draft.startedAtUtc);
    setQuestionStartedAt(Date.now());
    setQuizResult(null);
    setQuizError("");
    setQuizDraft(draft);
  }

  function exitQuizPage() {
    setActivePage("quizzes");
  }

  function goToHome() {
    clearDraft();
    setQuizDraft(null);
    setQuizResult(null);
    setQuizQuestions([]);
    setQuizAnswers({});
    setQuizTimes({});
    setQuizIndex(0);
    setQuizStartedAt("");
    setQuizError("");
    setPickerStep("book");
    setActivePage("quizzes");
  }

  function finishQuizSession() {
    goToHome();
  }

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
      const startedAtUtc = new Date().toISOString();
      setQuizQuestions(questions);
      setQuizAnswers({});
      setQuizTimes({});
      setQuizIndex(0);
      setQuizStartedAt(startedAtUtc);
      setQuestionStartedAt(Date.now());
      const draft: QuizDraft = {
        lessonId: selectedLessonId,
        category,
        questions,
        answers: {},
        times: {},
        quizIndex: 0,
        startedAtUtc,
      };
      saveDraft(draft);
      setQuizDraft(draft);
      setActivePage("quiz");
    } catch (error) {
      setQuizError(
        error instanceof Error ? error.message : t.fetchQuestionsFailed,
      );
    } finally {
      setQuizLoading(false);
    }
  }

  function openOrResumeQuiz(category: QuizCategory) {
    const draft = loadDraft();
    if (draftMatches(draft, selectedLessonId, category) && draft) {
      applyDraft(draft);
      setActivePage("quiz");
      return;
    }
    void startQuiz(category);
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
        openAuth("login");
        return;
      }
      if (!response.ok) throw new Error(await getError(response, t.requestFailed));
      const result = (await response.json()) as AttemptResult;
      setJustFinishedScore(Math.round(result.score));
      goToHome();
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

  async function translateText() {
    const input = translationInput.trim();
    if (!input) {
      setTranslationError(t.translationTextRequired);
      return;
    }

    const sourceLang = translationDirection === "de-fa" ? "de" : "fa";
    const targetLang = translationDirection === "de-fa" ? "fa" : "de";
    setTranslationLoading(true);
    setTranslationError("");

    try {
      const response = await fetch(`${apiBaseUrl}/translate`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({
          text: input,
          sourceLang,
          targetLang,
        }),
      });
      if (!response.ok) throw new Error(await getError(response, t.translationFailed));
      const result = (await response.json()) as { translatedText: string };
      setTranslationOutput(result.translatedText ?? "");
    } catch (error) {
      setTranslationError(
        error instanceof Error ? error.message : t.translationFailed,
      );
    } finally {
      setTranslationLoading(false);
    }
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
    setPickerStep(levels.length > 1 ? "level" : "lesson");
  }

  function selectLevel(level: string) {
    setSelectedLevel(level);
    const firstLesson = lessons.find(
      (lesson) => lesson.book === selectedBook && lesson.level === level,
    );
    if (firstLesson) setSelectedLessonId(firstLesson.id);
    setPickerStep("lesson");
  }

  function selectLesson(lessonId: string) {
    setSelectedLessonId(lessonId);
    setPickerStep("mode");
  }

  function goToPickerStep(step: QuizPickerStep) {
    setPickerStep(step);
  }

  function stepBack() {
    if (pickerStep === "mode") {
      setPickerStep("lesson");
      return;
    }
    if (pickerStep === "lesson") {
      const levels =
        bookOptions.find((book) => book.name === selectedBook)?.levels ?? [];
      setPickerStep(levels.length > 1 ? "level" : "book");
      return;
    }
    if (pickerStep === "level") {
      setPickerStep("book");
    }
  }

  function categoryLabel(category: QuizCategory) {
    if (category === "Vocabulary") return t.categoryVocabulary;
    if (category === "Grammar") return t.categoryGrammar;
    return t.categoryMixed;
  }

  const textAlign = uiLanguage === "en" ? "text-left" : "text-right";
  const pageLabels = uiLanguage === "fa"
    ? { quizzes: "آزمون‌ها", progress: "پیشرفت", history: "تاریخچه آزمون‌ها", translator: "مترجم" }
    : { quizzes: "Quizzes", progress: "Progress", history: "Quiz history", translator: "Translator" };

  function NavIcon({ page }: { page: "quizzes" | "progress" | "history" | "translator" }) {
    const common = { width: 28, height: 28, viewBox: "0 0 28 28", fill: "none", xmlns: "http://www.w3.org/2000/svg" };
    if (page === "quizzes") {
      return <svg {...common}><defs><linearGradient id="quizGlass" x1="4" y1="3" x2="24" y2="25"><stop stopColor="#fff" stopOpacity=".72"/><stop offset=".45" stopColor="#e7b84b"/><stop offset="1" stopColor="#e54858"/></linearGradient></defs><rect x="6.5" y="4" width="14" height="18" rx="3.5" stroke="url(#quizGlass)" strokeWidth="1.7"/><path d="m10 15 2.4 2.4L18 11.8" stroke="#f8d77d" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round"/><path d="M10 8.5h7" stroke="#fff" strokeOpacity=".7" strokeWidth="1.2" strokeLinecap="round"/></svg>;
    }
    if (page === "progress") {
      return <svg {...common}><defs><linearGradient id="progressGlass" x1="4" y1="24" x2="23" y2="4"><stop stopColor="#e54858"/><stop offset=".55" stopColor="#e7b84b"/><stop offset="1" stopColor="#fff" stopOpacity=".8"/></linearGradient></defs><path d="M5 23V5M5 23h18" stroke="#fff" strokeOpacity=".55" strokeWidth="1.2" strokeLinecap="round"/><rect x="8" y="15" width="3.5" height="6" rx="1.2" fill="url(#progressGlass)"/><rect x="13" y="11" width="3.5" height="10" rx="1.2" fill="url(#progressGlass)"/><rect x="18" y="7" width="3.5" height="14" rx="1.2" fill="url(#progressGlass)"/></svg>;
    }
    if (page === "history") {
      return <svg {...common}><defs><linearGradient id="historyGlass" x1="4" y1="22" x2="24" y2="5"><stop stopColor="#e54858"/><stop offset=".5" stopColor="#e7b84b"/><stop offset="1" stopColor="#fff" stopOpacity=".8"/></linearGradient></defs><path d="M7.2 10.2A8.2 8.2 0 1 1 6 17" stroke="url(#historyGlass)" strokeWidth="1.8" strokeLinecap="round"/><path d="M7.2 6.5v3.8H3.5" stroke="#fff" strokeOpacity=".75" strokeWidth="1.5" strokeLinecap="round" strokeLinejoin="round"/><path d="M14 9.2v5l3.2 1.8" stroke="#f8d77d" strokeWidth="1.6" strokeLinecap="round" strokeLinejoin="round"/></svg>;
    }
    return <svg {...common}><defs><linearGradient id="translateGlass" x1="4" y1="22" x2="24" y2="6"><stop stopColor="#e54858"/><stop offset=".5" stopColor="#e7b84b"/><stop offset="1" stopColor="#fff" stopOpacity=".8"/></linearGradient></defs><path d="M5 7.5A3.5 3.5 0 0 1 8.5 4h11A3.5 3.5 0 0 1 23 7.5v7a3.5 3.5 0 0 1-3.5 3.5h-6.2L8 22v-4H8.5A3.5 3.5 0 0 1 5 14.5v-7Z" stroke="url(#translateGlass)" strokeWidth="1.6"/><path d="M10 8.5h5M12.5 7v1.5M10 15l2.3-4 2.3 4M11 13.2h2.7" stroke="#fff" strokeOpacity=".8" strokeWidth="1.3" strokeLinecap="round" strokeLinejoin="round"/></svg>;
  }

  function formatDuration(totalTimeMs: number) {
    const totalSeconds = Math.max(0, Math.round(totalTimeMs / 1000));
    const minutes = Math.floor(totalSeconds / 60);
    const seconds = totalSeconds % 60;
    return `${String(minutes).padStart(2, "0")}:${String(seconds).padStart(2, "0")}`;
  }
  const streak = useMemo(() => {
    const dayKey = (date: Date) =>
      `${date.getFullYear()}-${String(date.getMonth() + 1).padStart(2, "0")}-${String(date.getDate()).padStart(2, "0")}`;
    const activeDays = new Set(
      history
        .filter((attempt) => attempt.completedAtUtc)
        .map((attempt) => dayKey(new Date(attempt.completedAtUtc!))),
    );
    const today = new Date();
    today.setHours(0, 0, 0, 0);
    const yesterday = new Date(today);
    yesterday.setDate(yesterday.getDate() - 1);
    let cursor = activeDays.has(dayKey(today)) ? today : activeDays.has(dayKey(yesterday)) ? yesterday : null;
    let current = 0;
    while (cursor && activeDays.has(dayKey(cursor))) {
      current += 1;
      const previous = new Date(cursor);
      previous.setDate(previous.getDate() - 1);
      cursor = previous;
    }
    let longest = 0;
    let running = 0;
    let previousKey = "";
    for (const key of [...activeDays].sort()) {
      const day = new Date(`${key}T00:00:00`);
      const previous = new Date(day);
      previous.setDate(previous.getDate() - 1);
      running = previousKey === dayKey(previous) ? running + 1 : 1;
      longest = Math.max(longest, running);
      previousKey = key;
    }
    const week = Array.from({ length: 7 }, (_, index) => {
      const day = new Date(today);
      day.setDate(today.getDate() - (6 - index));
      return {
        key: dayKey(day),
        label: day.toLocaleDateString(localeFor(uiLanguage), { weekday: "narrow" }),
        day: day.getDate(),
      };
    });
    return { current, longest, activeDays, week };
  }, [history, uiLanguage]);

  return (
    <main className="glass-page min-h-screen bg-background text-foreground">
      <div className="app-shell glass-frame mx-auto max-w-6xl gap-5 px-5 py-6 sm:px-8">
        <aside className="app-sidebar">
          <div className="app-sidebar__brand">
            <span className="text-xs font-bold uppercase tracking-[0.18em] text-de-gold">DeutschQuiz</span>
            <span className="mt-1 block text-[11px] text-muted">{uiLanguage === "fa" ? "فضای یادگیری" : "Learning space"}</span>
          </div>
          <div className="account-card">
            <span className="account-card__icon" aria-hidden>
              <svg width="20" height="20" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
                <circle cx="12" cy="8" r="3.2" stroke="currentColor" strokeWidth="1.5" />
                <path d="M5.8 19.2c.7-3.1 2.8-4.7 6.2-4.7s5.5 1.6 6.2 4.7" stroke="currentColor" strokeWidth="1.5" strokeLinecap="round" />
              </svg>
            </span>
            <span className="account-card__copy">
              <span className="account-card__label">حساب کاربری</span>
              <span className="account-card__name">{token ? userName || "کاربر" : "ورود به حساب"}</span>
            </span>
            <span className="account-card__chevron" aria-hidden>‹</span>
          </div>
          <nav className="app-sidebar__nav" aria-label="Main navigation">
            {(["quizzes", "progress", "history", "translator"] as const).map((page) => {
              const active = activePage === page || (page === "quizzes" && activePage === "quiz");
              return (
                <button
                  key={page}
                  onClick={() => {
                    setActivePage(page);
                    if (page === "quizzes") {
                      setPickerStep("book");
                      setQuizResult(null);
                    }
                  }}
                  className={`app-sidebar__item ${active ? "app-sidebar__item--active" : ""}`}
                >
                  <span className="app-sidebar__icon" aria-hidden><NavIcon page={page} /></span>
                  <span>{pageLabels[page]}</span>
                </button>
              );
            })}
          </nav>
        </aside>
        <div className="app-content">
        <header className="glass-header animate-rise flex flex-wrap items-center justify-between gap-3 border-b border-line pb-5">
          <div className="flex items-center gap-3">
            <div className="de-flag h-10 w-8 shrink-0 rounded-xl shadow-md" aria-hidden>
              <span /><span /><span />
            </div>
            <div>
              <p className="font-display text-xl font-extrabold tracking-tight text-foreground">
                DeutschQuiz
              </p>
              <p className="text-xs text-muted">Deutsch lernen · Schritt für Schritt</p>
            </div>
          </div>
          <div className="flex flex-wrap items-center gap-2">
            {token ? (
              <div className="flex items-center gap-2">
                <button
                  onClick={logout}
                  className="rounded-full border border-line bg-surface px-4 py-2 text-sm font-semibold text-foreground hover:bg-de-mist"
                >
                  {t.logout}
                </button>
              </div>
            ) : (
              <button
                onClick={() => openAuth("login")}
                className="rounded-full bg-de-red px-5 py-2.5 text-sm font-semibold text-white shadow-md shadow-de-red/20 transition hover:brightness-110"
              >
                {t.login}
              </button>
            )}
            <div className="relative">
              <button
                type="button"
                onClick={() => setSettingsOpen((open) => !open)}
                aria-label="تنظیمات نمایش"
                aria-expanded={settingsOpen}
                className="settings-trigger grid h-10 w-10 place-items-center text-foreground transition"
              >
                <svg className="settings-icon" width="26" height="26" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg" aria-hidden="true">
                  <defs>
                    <linearGradient id="settingsGlass" x1="4" y1="3" x2="20" y2="21" gradientUnits="userSpaceOnUse">
                      <stop stopColor="#FFF" stopOpacity=".9" />
                      <stop offset=".35" stopColor="#FF6B73" />
                      <stop offset="1" stopColor="#C92F3E" />
                    </linearGradient>
                  </defs>
                  <path d="m9.85 3.4.45 1.62a7.2 7.2 0 0 1 2.4 0l.45-1.62 2.08.86-.57 1.56a7.2 7.2 0 0 1 1.7 1.7l1.56-.57.86 2.08-1.62.45a7.2 7.2 0 0 1 0 2.4l1.62.45-.86 2.08-1.56-.57a7.2 7.2 0 0 1-1.7 1.7l.57 1.56-2.08.86-.45-1.62a7.2 7.2 0 0 1-2.4 0l-.45 1.62-2.08-.86.57-1.56a7.2 7.2 0 0 1-1.7-1.7l-1.56.57-.86-2.08 1.62-.45a7.2 7.2 0 0 1 0-2.4l-1.62-.45.86-2.08 1.56.57a7.2 7.2 0 0 1 1.7-1.7l-.57-1.56 2.08-.86Z" fill="url(#settingsGlass)" fillOpacity=".2" stroke="url(#settingsGlass)" strokeWidth="1.25" strokeLinejoin="round" />
                  <circle cx="12" cy="10.2" r="3.05" fill="rgb(35 20 22 / 72%)" stroke="#FF7A80" strokeWidth="1.25" />
                  <circle cx="12" cy="10.2" r="1.05" fill="#FFE1DF" fillOpacity=".95" />
                </svg>
              </button>
              {settingsOpen && (
                <div className="glass-settings absolute left-0 top-[calc(100%+0.7rem)] z-50 w-60 overflow-hidden rounded-[1.4rem] border border-line p-2 text-right shadow-2xl">
                  <p className="px-3 pb-2 pt-1 text-xs font-bold text-muted">تم</p>
                  {([
                    ["light", "☀", "روشن"],
                    ["dark", "☾", "تاریک"],
                    ["system", "▣", "سیستم"],
                  ] as const).map(([value, icon, label]) => (
                    <button
                      key={value}
                      type="button"
                      onClick={() => {
                        setTheme(value);
                        setSettingsOpen(false);
                      }}
                      className={`flex w-full items-center justify-between rounded-xl px-3 py-2.5 text-sm font-semibold transition ${
                        theme === value
                          ? "bg-de-gold/15 text-foreground"
                          : "text-muted hover:bg-de-mist hover:text-foreground"
                      }`}
                    >
                      <span className="text-base">{icon}</span>
                      <span>{label}</span>
                      <span className="decorative-ui w-4 text-center text-de-gold">{theme === value ? "✓" : ""}</span>
                    </button>
                  ))}
                  <div className="my-2 border-t border-line" />
                  <p className="px-3 pb-2 text-xs font-bold text-muted">زبان</p>
                  <button
                    type="button"
                    disabled={!SHOW_LANGUAGE_SWITCHER}
                    onClick={() => {
                      if (!SHOW_LANGUAGE_SWITCHER) return;
                      setLanguage(language === "fa" ? "en" : "fa");
                    }}
                    className="flex w-full items-center justify-between rounded-xl px-3 py-2.5 text-sm font-semibold text-foreground transition hover:bg-de-mist disabled:cursor-default disabled:hover:bg-transparent"
                  >
                    <span className="text-base">◎</span>
                    <span>{language === "fa" ? "فارسی" : "English"}</span>
                    <span className="decorative-ui w-4 text-center text-de-gold">✓</span>
                  </button>
                </div>
              )}
            </div>
          </div>
        </header>

        {justFinishedScore !== null &&
          activePage === "quizzes" &&
          pickerStep === "book" && (
          <div className="mt-6 flex flex-wrap items-center justify-between gap-3 rounded-[1.5rem] border border-de-gold/35 bg-surface-warm px-4 py-3">
            <p className="text-sm font-semibold text-foreground">
              {uiLanguage === "fa"
                ? `آزمون ثبت شد · نمره ${justFinishedScore}٪`
                : `Quiz saved · score ${justFinishedScore}%`}
            </p>
            <button
              type="button"
              onClick={() => setJustFinishedScore(null)}
              className="rounded-full border border-line bg-surface px-3 py-1.5 text-xs font-semibold text-muted"
            >
              {uiLanguage === "fa" ? "باشه" : "OK"}
            </button>
          </div>
        )}

        {((activePage === "quizzes" && pickerStep === "book") ||
          (activePage !== "quizzes" && activePage !== "quiz" && activePage !== "translator")) && (
        <section
          className="animate-rise mt-12 max-w-2xl"
          style={{ animationDelay: "80ms" }}
        >
          <h1 className="text-2xl font-bold leading-10 text-foreground sm:text-4xl sm:leading-12">
            {t.heroHeadline}
          </h1>
          <p className="mt-4 max-w-md text-sm leading-7 text-muted">
            {t.heroSubcopy}
          </p>
        </section>
        )}

        {activePage !== "quizzes" && activePage !== "quiz" && activePage !== "translator" && progress && (
          <section className="mt-12 grid gap-3 sm:grid-cols-4">
            {(
              [
                [t.statAverage, `${Math.round(progress.averageScore)}٪`, "bg-surface-warm border-de-gold/40 text-foreground"],
                [t.statBest, `${progress.bestScore}٪`, "bg-surface-rose border-de-rose/30 text-de-red"],
                [t.statCorrect, `${progress.totalCorrectAnswers}/${progress.totalQuestionsAnswered}`, "bg-de-mist border-line text-foreground"],
                [t.statTime, formatDuration(progress.totalTimeMs), "bg-surface border-line text-foreground"],
              ] as const
            ).map(([label, value, tone]) => (
              <div key={label} className={`rounded-3xl border px-4 py-5 ${tone}`}>
                <p className="text-xs text-muted">{label}</p>
                <p className="font-display mt-2 text-2xl font-bold">{value}</p>
              </div>
            ))}
          </section>
        )}

        {token && activePage === "quizzes" && (
          <section className="mt-8 overflow-hidden rounded-[2rem] border border-de-gold/25 bg-surface-warm p-5 sm:p-6">
            <div className="flex flex-col gap-5 sm:flex-row sm:items-center sm:justify-between">
              <div className="flex items-center gap-4">
                <div className="decorative-ui grid h-16 w-16 place-items-center rounded-3xl bg-de-red text-3xl text-white shadow-lg shadow-de-red/25" aria-hidden>🔥</div>
                <div>
                  <p className="text-xs font-bold uppercase tracking-wider text-de-red">{uiLanguage === "fa" ? "استریک یادگیری" : "Learning streak"}</p>
                  <p className="decorative-ui font-display mt-1 text-3xl font-extrabold text-foreground">{streak.current} {uiLanguage === "fa" ? "روز" : streak.current === 1 ? "day" : "days"}</p>
                  <p className="mt-1 text-xs text-muted">{uiLanguage === "fa" ? `بهترین رکورد: ${streak.longest} روز` : `Best streak: ${streak.longest} days`}</p>
                </div>
              </div>
              <div className="grid grid-cols-7 gap-2" dir="ltr">
                {streak.week.map((day) => {
                  const active = streak.activeDays.has(day.key);
                  return (
                    <div key={day.key} className="flex flex-col items-center gap-1">
                      <span className="decorative-ui text-[10px] font-bold text-muted">{day.label}</span>
                      <span className={`decorative-ui grid h-9 w-9 place-items-center rounded-full text-xs font-bold ${active ? "bg-de-gold text-de-black shadow-md" : "border border-line bg-surface text-muted"}`}>
                        {active ? "✓" : day.day}
                      </span>
                    </div>
                  );
                })}
              </div>
            </div>
            {streak.current === 0 && (
              <p className="mt-4 rounded-2xl border border-line bg-surface/70 px-3 py-2 text-xs text-muted">
                {uiLanguage === "fa" ? "امروز یک آزمون را ثبت کن تا استریکت شروع شود." : "Finish a quiz today to start your streak."}
              </p>
            )}
          </section>
        )}

        {activePage === "progress" && token && progress && (
          <section className="mt-12 rounded-[2rem] border border-line bg-surface p-5 sm:p-7">
            <div className="flex flex-col gap-2 sm:flex-row sm:items-end sm:justify-between">
              <div>
                <p className="text-xs font-bold uppercase tracking-wider text-de-red">
                  {t.progressChartEyebrow}
                </p>
                <h2 className="mt-2 font-display text-2xl font-bold text-foreground">
                  {t.progressChartHeading}
                </h2>
              </div>
              <span className="rounded-full bg-de-gold/30 px-3 py-1 text-xs font-bold text-foreground">
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
                    <article key={mode.category} className="rounded-3xl border border-line bg-de-mist/60 p-3">
                      <div className="flex items-center justify-between px-2 pt-1">
                        <h3 className="font-display text-lg font-bold text-foreground">{categoryLabel(mode.category)}</h3>
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
                        theme={theme}
                      />
                    </article>
                  );
                })}
              </div>
            </div>
          </section>
        )}

        {activePage === "progress" && token && progress && (
          <section className="mt-12 rounded-[2rem] border border-line bg-surface p-5 sm:p-7">
            <div className="flex flex-col gap-2 sm:flex-row sm:items-end sm:justify-between">
              <div>
                <p className="text-xs font-bold uppercase tracking-wider text-de-red">Fortschritt</p>
                <h2 className="mt-2 font-display text-2xl font-bold text-foreground">
                  {t.progressHeading(selectedBook, selectedLevel)}
                </h2>
              </div>
              <span className="rounded-full bg-de-gold/30 px-3 py-1 text-xs font-bold text-foreground">
                {t.lessonCount(selectedBookProgress.length)}
              </span>
            </div>
            {selectedBookProgress.length === 0 ? (
              <p className="mt-6 rounded-3xl border border-dashed border-de-amber/40 bg-surface-warm px-4 py-6 text-center text-sm text-muted">
                {t.noProgressInLevel}
              </p>
            ) : (
              <div className="mt-6 grid gap-3 md:grid-cols-2">
                {selectedBookProgress.map((lesson) => (
                  <div
                    key={lesson.lessonId}
                    className="rounded-3xl border border-line bg-de-mist/50 p-4"
                  >
                    <div className="flex items-start justify-between gap-3">
                      <div>
                        <span className="text-xs font-bold text-de-red">
                          Lektion {lesson.lessonNumber}
                        </span>
                        <p className="mt-1 text-sm font-bold text-foreground" dir="ltr">
                          {lesson.title}
                        </p>
                      </div>
                      <span className="rounded-full bg-de-gold px-3 py-1 text-xs font-bold text-de-black">
                        {Math.round(lesson.averageScore)}٪
                      </span>
                    </div>
                    <div className="mt-4 h-2 overflow-hidden rounded-full bg-line">
                      <div
                        className="h-full rounded-full bg-gradient-to-l from-de-red to-de-gold"
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
          <section className="mt-12 rounded-[2rem] border border-line bg-surface p-5 sm:p-7">
            <div className="flex items-center justify-between gap-4">
              <div>
                <p className="text-xs font-bold uppercase tracking-wider text-de-red">Verlauf</p>
                <h2 className="mt-2 font-display text-2xl font-bold text-foreground">
                  {t.historyHeading}
                </h2>
              </div>
              <button
                onClick={() => void loadHistory(token)}
                disabled={historyLoading}
                className="rounded-full border border-line bg-de-mist px-4 py-2 text-xs font-semibold text-muted disabled:opacity-50"
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
                    className="flex flex-col gap-3 rounded-3xl border border-line bg-de-mist/40 px-4 py-4 sm:flex-row sm:items-center sm:justify-between"
                  >
                    <div>
                      <div className="flex flex-wrap items-center gap-2">
                        <span className="rounded-full bg-de-red/15 px-2.5 py-1 text-sm font-bold text-de-red">
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
                        <span className="block text-sm font-bold text-foreground">
                          {formatDuration(attempt.totalTimeMs)}
                        </span>
                      </div>
                    </div>
                  </div>
                ))}
              </div>
            )}
          </section>
        )}

        {activePage === "translator" && (
          <section className="mt-12 overflow-hidden rounded-[2rem] border border-line bg-surface shadow-[var(--card-shadow)]">
            <div className="border-b border-line px-5 py-5 sm:px-7">
              <p className="text-xs font-bold uppercase tracking-wider text-de-red">{t.translator}</p>
              <div className="mt-2 flex flex-col gap-1 sm:flex-row sm:items-end sm:justify-between">
                <h2 className="font-display text-2xl font-bold text-foreground">{t.translatorHeading}</h2>
                <p className="text-sm text-muted">{t.translatorSubcopy}</p>
              </div>
            </div>

            <div className="p-4 sm:p-6">
              <div className="grid items-stretch gap-3 lg:grid-cols-[minmax(0,1fr)_auto_minmax(0,1fr)]">
                <div className="overflow-hidden rounded-2xl border border-line bg-de-mist/70">
                  <div className="flex items-center justify-between border-b border-line px-4 py-3">
                    <span className="text-sm font-bold text-foreground">
                      {translationDirection === "de-fa"
                        ? t.translationDirectionDeToFa.split(/ → | -> /)[0]
                        : t.translationDirectionFaToDe.split(/ → | -> /)[0]}
                    </span>
                    <span className="rounded-full bg-surface px-2.5 py-1 text-[11px] font-semibold text-muted">{t.translatorInputLabel}</span>
                  </div>
                  <textarea
                    value={translationInput}
                    onChange={(event) => setTranslationInput(event.target.value)}
                    rows={8}
                    placeholder={t.translatorInputPlaceholder}
                    className="min-h-[15rem] w-full resize-none border-0 bg-transparent px-4 py-4 text-base leading-8 text-foreground outline-none placeholder:text-muted focus:ring-0"
                    dir={translationDirection === "fa-de" ? "rtl" : "ltr"}
                  />
                  <div className="flex justify-end px-3 pb-3">
                    <span className="text-[11px] text-muted">{translationInput.length}</span>
                  </div>
                </div>

                <button
                  type="button"
                  aria-label={t.translationDirectionLabel}
                  onClick={() => {
                    setTranslationDirection((current) => (current === "de-fa" ? "fa-de" : "de-fa"));
                    setTranslationInput(translationOutput);
                    setTranslationOutput(translationInput);
                  }}
                  className="self-center rounded-full border border-line bg-surface p-3 text-lg text-muted transition hover:border-de-gold hover:bg-de-mist hover:text-de-gold"
                >
                  ⇄
                </button>

                <div className="overflow-hidden rounded-2xl border border-line bg-surface-ink/60">
                  <div className="flex items-center justify-between border-b border-line px-4 py-3">
                    <span className="text-sm font-bold text-foreground">
                      {translationDirection === "de-fa"
                        ? t.translationDirectionDeToFa.split(/ → | -> /)[1]
                        : t.translationDirectionFaToDe.split(/ → | -> /)[1]}
                    </span>
                    <span className="rounded-full bg-de-mist px-2.5 py-1 text-[11px] font-semibold text-muted">{t.translatorOutputLabel}</span>
                  </div>
                  <textarea
                    value={translationOutput}
                    readOnly
                    rows={8}
                    placeholder={t.translatorOutputPlaceholder}
                    className="min-h-[15rem] w-full resize-none border-0 bg-transparent px-4 py-4 text-base leading-8 text-foreground outline-none placeholder:text-muted focus:ring-0"
                    dir={translationDirection === "de-fa" ? "rtl" : "ltr"}
                  />
                </div>
              </div>

            {translationError && (
              <p className="mt-4 rounded-2xl border border-de-red/30 bg-surface-rose px-3 py-2 text-xs font-semibold text-de-red">
                {translationError}
              </p>
            )}
            <div className="mt-5 flex justify-end">
              <button
                type="button"
                onClick={() => void translateText()}
                disabled={translationLoading}
                className="rounded-xl bg-de-red px-6 py-3 text-sm font-bold text-white shadow-md shadow-de-red/20 transition hover:brightness-110 disabled:opacity-60"
              >
                {translationLoading ? t.translating : t.translateAction}
              </button>
            </div>
            </div>
          </section>
        )}

        {activePage === "quizzes" && (
          <section className="mt-12 pb-16">
            {pickerStep !== "book" && (
              <div className="mb-5 flex flex-wrap items-center gap-2">
                <button
                  onClick={stepBack}
                  className="rounded-full border border-line bg-surface px-4 py-2 text-xs font-semibold text-muted hover:bg-de-mist"
                >
                  {t.stepBack}
                </button>
                <nav
                  className="flex flex-wrap items-center gap-1 text-xs font-semibold text-muted"
                  aria-label="Quiz picker path"
                >
                  <button
                    onClick={() => goToPickerStep("book")}
                    className="rounded-full px-2 py-1 hover:bg-de-mist hover:text-foreground"
                    dir="ltr"
                  >
                    {selectedBook}
                  </button>
                  {(pickerStep === "level" ||
                    pickerStep === "lesson" ||
                    pickerStep === "mode") &&
                    selectedBookLevels.length > 1 && (
                      <>
                        <span aria-hidden>›</span>
                        <button
                          onClick={() => goToPickerStep("level")}
                          className="rounded-full px-2 py-1 hover:bg-de-mist hover:text-foreground"
                          dir="ltr"
                        >
                          {selectedLevel}
                        </button>
                      </>
                    )}
                  {(pickerStep === "lesson" || pickerStep === "mode") &&
                    selectedBookLevels.length <= 1 && (
                      <>
                        <span aria-hidden>›</span>
                        <span className="rounded-full px-2 py-1" dir="ltr">
                          {selectedLevel}
                        </span>
                      </>
                    )}
                  {(pickerStep === "lesson" || pickerStep === "mode") && (
                    <>
                      <span aria-hidden>›</span>
                      {pickerStep === "mode" ? (
                        <button
                          onClick={() => goToPickerStep("lesson")}
                          className="rounded-full px-2 py-1 hover:bg-de-mist hover:text-foreground"
                          dir="ltr"
                        >
                          Lektion {selectedLesson?.number ?? 1}
                        </button>
                      ) : (
                        <span className="rounded-full px-2 py-1 text-foreground">
                          {t.pickLessonHeading}
                        </span>
                      )}
                    </>
                  )}
                  {pickerStep === "mode" && (
                    <>
                      <span aria-hidden>›</span>
                      <span className="rounded-full px-2 py-1 text-foreground">
                        {t.quizTypeHeading}
                      </span>
                    </>
                  )}
                </nav>
              </div>
            )}

            {pickerStep === "book" && (
              <div id="book-picker" className="scroll-mt-8 rounded-[2rem] border border-line bg-surface p-5 sm:p-7">
                <p className="text-xs font-bold uppercase tracking-wider text-de-red">Lehrwerk</p>
                <h2 className="mt-2 font-display text-2xl font-bold text-foreground">{t.pickBook}</h2>
                {lessonsError && (
                  <p className="mt-4 rounded-2xl border border-de-red/30 bg-surface-rose px-3 py-2 text-sm font-semibold text-de-red">
                    {lessonsError}
                  </p>
                )}
                <div className="mt-6 grid gap-3 sm:grid-cols-2">
                  {bookOptions.map((book) => (
                    <button
                      key={book.name}
                      onClick={() => selectBook(book.name)}
                      className={`rounded-[1.75rem] border p-5 transition ${textAlign} ${
                        completedBookNames.has(book.name)
                          ? "border-success/40 bg-success-bg text-foreground hover:border-success"
                          : "border-line bg-de-mist/40 text-foreground hover:border-de-red"
                      }`}
                    >
                      <p className="font-display text-xl font-bold" dir="ltr">
                        <span className="decorative-ui">{completedBookNames.has(book.name) ? "✓ " : ""}</span>
                        {book.name}
                      </p>
                      <p className="mt-2 text-xs text-muted" dir="ltr">
                        {book.levels.join(" · ")}
                      </p>
                    </button>
                  ))}
                </div>
                {lessonsLoading && (
                  <p className="mt-4 text-center text-sm text-muted">{t.fetching}</p>
                )}
              </div>
            )}

            {pickerStep === "level" && (
              <div className="rounded-[2rem] border border-line bg-surface p-5 sm:p-7">
                <p className="text-xs font-bold uppercase tracking-wider text-de-red">Niveau</p>
                <h2 className="mt-2 font-display text-2xl font-bold text-foreground">
                  {t.pickLevelHeading}
                </h2>
                <div className="mt-6 grid gap-3 sm:grid-cols-2">
                  {selectedBookLevels.map((level) => (
                    <button
                      key={level}
                      onClick={() => selectLevel(level)}
                      className="rounded-[1.75rem] border border-line bg-de-mist/40 p-5 text-foreground transition hover:border-de-gold hover:bg-surface-warm"
                    >
                      <p className="font-display text-2xl font-bold" dir="ltr">
                        {level}
                      </p>
                      <p className="mt-2 text-xs text-muted">{t.levelLabel(level)}</p>
                    </button>
                  ))}
                </div>
              </div>
            )}

            {pickerStep === "lesson" && (
              <div className="rounded-[2rem] border border-line bg-surface p-5 sm:p-7">
                <div className="flex flex-col gap-2 sm:flex-row sm:items-end sm:justify-between">
                  <div>
                    <p className="text-xs font-bold uppercase tracking-wider text-de-red">Lektion</p>
                    <h2 className="mt-2 font-display text-2xl font-bold text-foreground">
                      {t.pickLessonHeading}
                    </h2>
                  </div>
                  <span className="rounded-full bg-de-mist px-3 py-1 text-xs text-muted">
                    {lessonsLoading
                      ? t.fetching
                      : t.lessonsMeta(bookLessons.length, selectedBook, selectedLevel)}
                  </span>
                </div>
                <div className="mt-6 grid gap-3 sm:grid-cols-2 lg:grid-cols-3">
                  {bookLessons.map((lesson) => (
                    <button
                      key={lesson.id}
                      onClick={() => selectLesson(lesson.id)}
                      className={`rounded-[1.5rem] border p-4 transition ${textAlign} ${
                        completedLessonIds.has(lesson.id)
                          ? "border-success/40 bg-success-bg hover:border-success"
                          : "border-line bg-de-mist/40 hover:border-de-gold hover:bg-surface-warm"
                      }`}
                    >
                      <span
                        className={`text-xs font-bold ${
                          completedLessonIds.has(lesson.id)
                            ? "text-success"
                            : "text-de-red"
                        }`}
                      >
                        <span className="decorative-ui">{completedLessonIds.has(lesson.id) ? "✓ " : ""}</span>
                        Lektion {lesson.number}
                      </span>
                      <p className="mt-2 text-sm font-bold text-foreground" dir="ltr">
                        {lesson.title}
                      </p>
                    </button>
                  ))}
                </div>
              </div>
            )}

            {pickerStep === "mode" && (
              <div>
                <div className="mb-6 flex items-end justify-between gap-4">
                  <div>
                    <p className="text-xs font-bold uppercase tracking-wider text-de-red">Quiz</p>
                    <h2 className="mt-2 font-display text-2xl font-bold text-foreground">
                      {t.quizTypeHeading}
                    </h2>
                  </div>
                  <span className="rounded-full bg-de-gold/30 px-3 py-1 text-xs font-bold text-foreground">
                    Lektion {selectedLesson?.number ?? 1}
                  </span>
                </div>
                <div className="grid gap-4 md:grid-cols-3">
                  {quizModeMeta.map((mode) => {
                    const copy = t.quizModes[mode.category];
                    const sectionCompleted =
                      token && hasCompletedSection(selectedLessonId, mode.category);
                    const sectionInProgress = draftMatches(
                      quizDraft,
                      selectedLessonId,
                      mode.category,
                    );
                    return (
                      <button
                        key={mode.category}
                        onClick={() => openOrResumeQuiz(mode.category)}
                        disabled={quizLoading}
                        className={`group rounded-[1.75rem] border p-5 transition hover:-translate-y-0.5 hover:shadow-lg disabled:cursor-wait ${textAlign} ${
                          sectionInProgress
                            ? "border-de-amber bg-surface-warm"
                            : sectionCompleted
                              ? "border-success/40 bg-success-bg"
                              : mode.card
                        }`}
                      >
                        <span
                          className={`inline-block rounded-full px-3 py-1 text-[10px] font-bold uppercase tracking-wider ${
                            sectionInProgress
                              ? "bg-de-amber text-de-black"
                              : sectionCompleted
                                ? "bg-success text-white"
                                : mode.accent
                          }`}
                        >
                          {sectionInProgress
                            ? t.inProgress
                            : sectionCompleted
                              ? `✓ ${mode.subtitle}`
                              : mode.subtitle}
                        </span>
                        <h3 className="mt-4 text-lg font-bold text-foreground">{copy.title}</h3>
                        <p className="mt-2 text-sm leading-7 text-muted">{copy.description}</p>
                        <div className="mt-5 text-sm font-bold text-de-red group-hover:underline">
                          {sectionInProgress ? t.continueQuiz : t.start}
                        </div>
                      </button>
                    );
                  })}
                </div>
                {quizError && (
                  <p className="mt-4 rounded-2xl border border-de-red/30 bg-surface-rose px-3 py-2 text-center text-xs font-semibold text-de-red">
                    {quizError}
                  </p>
                )}
              </div>
            )}
          </section>
        )}

        {activePage === "quiz" && (
          <section className="mt-10 pb-16">
            <div className="mx-auto max-w-2xl rounded-[2rem] border border-line bg-surface p-6 sm:p-8">
              {quizResult ? (
                <div className="text-center">
                  <div className="de-flag mx-auto h-16 w-12 rounded-2xl shadow-md" aria-hidden>
                    <span /><span /><span />
                  </div>
                  <p className="mt-5 text-xs font-bold uppercase tracking-wider text-de-red">
                    Ergebnis
                  </p>
                  <h2 className="font-display mt-2 text-4xl font-extrabold text-foreground">
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
                            ? "border-de-gold/50 bg-surface-warm"
                            : "border-de-rose/40 bg-surface-rose"
                        }`}
                      >
                        <p className="text-sm font-bold text-foreground" dir="ltr">
                          {index + 1}. {answer.prompt}
                        </p>
                        <p className="mt-2 text-xs text-muted" dir="ltr">
                          {t.yourAnswer}{" "}
                          <span className="font-bold">{answer.selectedAnswer}</span>
                          {!answer.isCorrect && (
                            <>
                              {" "}
                              · {t.correctLabel}{" "}
                              <span className="font-bold text-foreground">{answer.correctAnswer}</span>
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
                    onClick={finishQuizSession}
                    className="mt-7 rounded-2xl bg-de-red px-6 py-3 text-sm font-bold text-white"
                  >
                    {t.back}
                  </button>
                </div>
              ) : activeQuestion ? (
                <>
                  <div className="flex items-start gap-4">
                    <div>
                      <p className="text-xs font-bold uppercase tracking-wider text-de-red">
                        {quizCategory === "Vocabulary"
                          ? "Wortschatz"
                          : quizCategory === "Grammar"
                            ? "Grammatik"
                            : "Komplett"}
                      </p>
                      <h2 className="mt-2 font-display text-2xl font-bold text-foreground">
                        {t.questionOf(quizIndex + 1, quizQuestions.length)}
                      </h2>
                      <p className="mt-1 text-xs text-muted">{t.resumeHint}</p>
                    </div>
                  </div>
                  <div className="mt-5 h-2 overflow-hidden rounded-full bg-line">
                    <div
                      className="h-full rounded-full bg-gradient-to-l from-de-red to-de-gold transition-all"
                      style={{
                        width: `${((quizIndex + 1) / quizQuestions.length) * 100}%`,
                      }}
                    />
                  </div>
                  <div className="mt-8 rounded-[1.75rem] border border-line bg-de-mist/50 p-5 sm:p-7">
                    <p
                      className="text-center text-xl font-bold leading-9 text-foreground"
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
                                ? "border-de-red bg-de-red text-white shadow-md"
                                : "border-line bg-surface text-foreground hover:border-de-red hover:bg-surface-rose"
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
                      onClick={exitQuizPage}
                      disabled={quizSubmitting}
                      className="rounded-2xl border border-line px-4 py-3 text-sm font-semibold text-muted disabled:opacity-40"
                    >
                      {t.exitQuiz}
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
                        className="rounded-2xl bg-surface-ink px-5 py-3 text-sm font-bold text-white disabled:opacity-40"
                      >
                        {quizSubmitting ? t.submitting : t.submitQuiz}
                      </button>
                    )}
                  </div>
                </>
              ) : (
                <div className="text-center">
                  <p className="text-sm text-muted">{t.fetching}</p>
                  <button
                    onClick={exitQuizPage}
                    className="mt-6 rounded-2xl border border-line px-4 py-3 text-sm font-semibold text-muted"
                  >
                    {t.back}
                  </button>
                </div>
              )}
            </div>
          </section>
        )}
        </div>
      </div>

      {authOpen && (
        <div
          className="fixed inset-0 z-50 grid place-items-center bg-black/60 px-5 backdrop-blur-sm"
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
                <p className="font-display text-sm font-bold text-foreground">DeutschQuiz</p>
                <h2 className="mt-2 text-2xl font-bold text-foreground">
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
                  className="w-full rounded-2xl border border-line bg-de-mist px-4 py-3 text-sm text-foreground outline-none placeholder:text-muted focus:border-de-gold"
                />
              )}
              <input
                required
                type="email"
                value={email}
                onChange={(event) => setEmail(event.target.value)}
                placeholder={t.emailPlaceholder}
                className="w-full rounded-2xl border border-line bg-de-mist px-4 py-3 text-sm text-foreground outline-none placeholder:text-muted focus:border-de-gold"
              />
              <input
                required
                minLength={8}
                type="password"
                value={password}
                onChange={(event) => setPassword(event.target.value)}
                placeholder={t.passwordPlaceholder}
                className="w-full rounded-2xl border border-line bg-de-mist px-4 py-3 text-sm text-foreground outline-none placeholder:text-muted focus:border-de-gold"
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
