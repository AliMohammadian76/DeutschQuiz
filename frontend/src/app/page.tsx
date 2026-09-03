"use client";

import { FormEvent, useEffect, useState } from "react";

const apiBaseUrl =
  process.env.NEXT_PUBLIC_API_BASE_URL ?? "http://localhost:5083/api";

const quizModes = [
  {
    category: "Vocabulary",
    title: "واژگان",
    subtitle: "Wortschatz",
    description: "کلمات کلیدی درس اول را مرور کن.",
    icon: "Aa",
    color: "from-amber-400 to-orange-500",
  },
  {
    category: "Grammar",
    title: "گرامر",
    subtitle: "Grammatik",
    description: "ساختارهای پایه و جمله‌سازی را بسنج.",
    icon: "文",
    color: "from-violet-500 to-indigo-600",
  },
  {
    category: "Mixed",
    title: "آزمون جامع",
    subtitle: "Komplett",
    description: "ترکیبی از واژگان و گرامر.",
    icon: "✦",
    color: "from-cyan-400 to-blue-600",
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
};

const lessonId = "11111111-1111-1111-1111-111111111111";

async function getError(response: Response) {
  try {
    const body = await response.json();
    return body.message ?? "درخواست انجام نشد.";
  } catch {
    return "درخواست انجام نشد.";
  }
}

export default function Home() {
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

  async function startQuiz(category: QuizCategory) {
    setQuizCategory(category);
    setQuizLoading(true);
    setQuizError("");
    setQuizResult(null);
    try {
      const response = await fetch(
        `${apiBaseUrl}/lessons/${lessonId}/questions?category=${category}`,
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
          lessonId,
          category: quizCategory,
          startedAtUtc: quizStartedAt,
          answers,
        }),
      });
      if (!response.ok) throw new Error(await getError(response));
      const result = (await response.json()) as AttemptResult;
      setQuizResult(result);
      await loadProgress(token);
    } catch (error) {
      setQuizError(
        error instanceof Error ? error.message : "ثبت نتیجه انجام نشد.",
      );
    } finally {
      setQuizSubmitting(false);
    }
  }

  useEffect(() => {
    const savedToken = localStorage.getItem("deutschquiz.accessToken");
    const savedName = localStorage.getItem("deutschquiz.displayName");
    if (!savedToken) return;

    const timer = window.setTimeout(() => {
      setToken(savedToken);
      setUserName(savedName ?? "");
      void loadProgress(savedToken);
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
  }

  function openAuth(mode: AuthMode) {
    setAuthMode(mode);
    setAuthError("");
    setAuthOpen(true);
  }

  const activeQuestion = quizQuestions[quizIndex];

  return (
    <main className="min-h-screen overflow-hidden">
      <div className="mx-auto max-w-7xl px-5 py-6 sm:px-8 lg:px-10">
        <header className="flex items-center justify-between">
          <div className="flex items-center gap-3">
            <div className="grid h-11 w-11 place-items-center rounded-2xl bg-[#172033] text-lg font-bold text-white shadow-lg shadow-slate-300">D</div>
            <div><p className="text-lg font-bold tracking-tight">DeutschQuiz</p><p className="text-xs text-slate-500">تمرین هوشمند آلمانی</p></div>
          </div>
          <div className="flex items-center gap-3">
            <button onClick={() => setLanguage(language === "fa" ? "en" : "fa")} className="rounded-full border border-slate-200 bg-white px-4 py-2 text-xs font-semibold text-slate-600 transition hover:border-slate-300">{language === "fa" ? "English" : "فارسی"}</button>
            {token ? <div className="flex items-center gap-2"><span className="hidden text-sm font-bold text-slate-600 sm:inline">{userName}</span><button onClick={logout} className="rounded-full border border-slate-200 bg-white px-4 py-2.5 text-sm font-semibold text-slate-700">خروج</button></div> : <button onClick={() => openAuth("login")} className="rounded-full bg-[#172033] px-5 py-2.5 text-sm font-semibold text-white">ورود / ثبت‌نام</button>}
          </div>
        </header>

        <section className="relative mt-14 grid items-center gap-12 lg:grid-cols-[1.1fr_0.9fr]">
          <div className="relative z-10">
            <span className="inline-flex rounded-full bg-cyan-50 px-4 py-2 text-xs font-bold text-cyan-700">Menschen · A1.1 · Lektion 1</span>
            <h1 className="mt-6 max-w-xl text-4xl font-black leading-[1.2] tracking-tight text-[#172033] sm:text-6xl">هر روز کمی بهتر،<span className="block text-cyan-600">یک سؤال در هر لحظه.</span></h1>
            <p className="mt-6 max-w-lg text-base leading-8 text-slate-500">آزمون‌های کوتاه و جذاب برای سنجش واژگان و گرامر آلمانی. پیشرفتت را ببین و با ریتم خودت جلو برو.</p>
            <div className="mt-8 flex flex-wrap gap-4"><button onClick={() => void startQuiz("Mixed")} disabled={quizLoading} className="rounded-2xl bg-cyan-600 px-6 py-3.5 text-sm font-bold text-white shadow-xl shadow-cyan-200 disabled:opacity-60">{quizLoading ? "در حال آماده‌سازی..." : "شروع آزمون رایگان ←"}</button><button onClick={() => token ? void loadProgress(token) : openAuth("login")} className="rounded-2xl border border-slate-200 bg-white px-6 py-3.5 text-sm font-bold text-slate-700">مشاهده‌ی پیشرفت</button></div>
            <div className="mt-10 flex items-center gap-7 text-sm text-slate-500"><div><span className="block text-2xl font-black text-[#172033]">۱۲</span>سؤال آماده</div><div className="h-9 w-px bg-slate-200" /><div><span className="block text-2xl font-black text-[#172033]">A1.1</span>سطح فعلی</div></div>
          </div>
          <div className="relative mx-auto w-full max-w-md"><div className="absolute -inset-6 rounded-[3rem] bg-gradient-to-br from-cyan-100 via-white to-violet-100 blur-2xl" /><div className="relative rounded-[2rem] border border-white/80 bg-white/90 p-6 shadow-2xl shadow-slate-200/80 backdrop-blur">
            <div className="flex items-center justify-between"><div><p className="text-xs text-slate-400">پیشرفت تو</p><p className="mt-1 text-2xl font-black">{progress ? `${Math.round(progress.averageScore)}٪` : "—"}</p></div><div className="grid h-12 w-12 place-items-center rounded-2xl bg-cyan-50 text-xl">↗</div></div>
            <div className="mt-6 flex h-32 items-end justify-between gap-2">{[34, 54, 42, 76, 61, 88, progress ? Math.max(8, Math.round(progress.averageScore)) : 18].map((height, index) => <div key={index} className="flex flex-1 flex-col items-center gap-2"><div className={`w-full rounded-t-lg ${index === 6 ? "bg-cyan-500" : "bg-cyan-100"}`} style={{ height: `${height}%` }} /><span className="text-[10px] text-slate-400">{["ش", "ی", "د", "س", "چ", "پ", "ج"][index]}</span></div>)}</div>
            <div className="mt-5 rounded-2xl bg-slate-50 p-4"><div className="flex items-center justify-between text-xs"><span className="font-bold text-slate-700">Lektion 1</span><span className="font-bold text-cyan-600">{progressLoading ? "در حال بارگذاری" : progress ? `${progress.attemptsCount} آزمون` : "هنوز شروع نشده"}</span></div><div className="mt-3 h-2 overflow-hidden rounded-full bg-slate-200"><div className="h-full rounded-full bg-cyan-500 transition-all" style={{ width: `${progress?.averageScore ?? 0}%` }} /></div></div>
          </div></div>
        </section>

        {progress && <section className="mt-12 grid gap-4 sm:grid-cols-4">{[["میانگین نمره", `${Math.round(progress.averageScore)}٪`], ["بهترین نمره", `${progress.bestScore}٪`], ["پاسخ درست", `${progress.totalCorrectAnswers} از ${progress.totalQuestionsAnswered}`], ["زمان پاسخ", `${Math.round(progress.totalTimeMs / 1000)} ثانیه`]].map(([label, value]) => <div key={label} className="rounded-2xl border border-slate-100 bg-white p-4 shadow-sm"><p className="text-xs text-slate-400">{label}</p><p className="mt-2 text-xl font-black text-[#172033]">{value}</p></div>)}</section>}

        <section className="mt-20 pb-12"><div className="mb-7 flex items-end justify-between"><div><p className="text-sm font-bold text-cyan-600">انتخاب کن و شروع کن</p><h2 className="mt-2 text-2xl font-black">نوع آزمون را انتخاب کن</h2></div><span className="text-sm text-slate-400">۳ حالت آزمون</span></div>
          <div className="grid gap-5 md:grid-cols-3">{quizModes.map((mode) => <button key={mode.category} onClick={() => void startQuiz(mode.category)} disabled={quizLoading} className="group text-right disabled:cursor-wait"><div className="h-full rounded-3xl border border-slate-100 bg-white p-6 shadow-sm transition duration-300 hover:-translate-y-1 hover:shadow-xl"><div className={`grid h-14 w-14 place-items-center rounded-2xl bg-gradient-to-br ${mode.color} text-xl font-black text-white shadow-lg`}>{mode.icon}</div><div className="mt-5 flex items-center gap-2"><h3 className="text-lg font-black">{mode.title}</h3><span className="text-xs text-slate-400">{mode.subtitle}</span></div><p className="mt-3 text-sm leading-7 text-slate-500">{mode.description}</p><div className="mt-6 text-sm font-bold text-cyan-600">شروع کن ←</div></div></button>)}</div>
        </section>
      </div>

      {quizOpen && activeQuestion && <div className="fixed inset-0 z-40 grid place-items-center bg-slate-950/40 px-5 py-6 backdrop-blur-sm" onMouseDown={() => !quizSubmitting && setQuizOpen(false)}>
        <div className="max-h-[90vh] w-full max-w-2xl overflow-y-auto rounded-3xl bg-white p-6 shadow-2xl sm:p-8" onMouseDown={(event) => event.stopPropagation()}>
          {quizResult ? <div className="text-center">
            <div className="mx-auto grid h-20 w-20 place-items-center rounded-full bg-emerald-50 text-3xl text-emerald-600">✓</div>
            <p className="mt-5 text-sm font-bold text-cyan-600">آزمون تمام شد</p>
            <h2 className="mt-2 text-3xl font-black text-[#172033]">{Math.round(quizResult.score)}٪</h2>
            <p className="mt-3 text-sm text-slate-500">{quizResult.correctAnswers} پاسخ درست از {quizResult.totalQuestions} سؤال · {Math.round(quizResult.totalTimeMs / 1000)} ثانیه</p>
            <div className="mt-7 space-y-3 text-right">
              {quizResult.answers.map((answer, index) => <div key={answer.questionId} className={`rounded-2xl border p-4 ${answer.isCorrect ? "border-emerald-100 bg-emerald-50/70" : "border-rose-100 bg-rose-50/70"}`}>
                <div className="flex items-start gap-3"><span className={`grid h-7 w-7 shrink-0 place-items-center rounded-full text-xs font-black ${answer.isCorrect ? "bg-emerald-500 text-white" : "bg-rose-500 text-white"}`}>{answer.isCorrect ? "✓" : "×"}</span><div className="min-w-0 flex-1"><p className="text-sm font-bold text-slate-800" dir="ltr">{index + 1}. {answer.prompt}</p><p className="mt-2 text-xs text-slate-600" dir="ltr">پاسخ تو: <span className="font-bold">{answer.selectedAnswer}</span>{!answer.isCorrect && <> · پاسخ درست: <span className="font-bold text-emerald-700">{answer.correctAnswer}</span></>}</p>{answer.explanation && <p className="mt-2 text-xs leading-6 text-slate-500">{answer.explanation}</p>}</div></div>
              </div>)}
            </div>
            <button onClick={() => setQuizOpen(false)} className="mt-7 rounded-2xl bg-cyan-600 px-6 py-3 text-sm font-bold text-white">بازگشت به صفحه اصلی</button>
          </div> : <>
            <div className="flex items-start justify-between gap-4">
              <div>
                <p className="text-xs font-bold text-cyan-600">{quizCategory === "Vocabulary" ? "Wortschatz" : quizCategory === "Grammar" ? "Grammatik" : "Komplett"}</p>
                <h2 className="mt-2 text-2xl font-black text-[#172033]">سؤال {quizIndex + 1} از {quizQuestions.length}</h2>
              </div>
              <button onClick={() => setQuizOpen(false)} className="text-xl text-slate-400">×</button>
            </div>
            <div className="mt-5 h-2 overflow-hidden rounded-full bg-slate-100"><div className="h-full rounded-full bg-cyan-500 transition-all" style={{ width: `${((quizIndex + 1) / quizQuestions.length) * 100}%` }} /></div>
            <div className="mt-9 rounded-3xl bg-slate-50 p-5 sm:p-7">
              <p className="text-center text-xl font-bold leading-9 text-[#172033]" dir="ltr">{activeQuestion.prompt}</p>
              <div className="mt-7 grid gap-3">
                {activeQuestion.options.map((option) => {
                  const selected = quizAnswers[activeQuestion.id] === option;
                  return <button key={option} onClick={() => selectAnswer(option)} className={`rounded-2xl border px-4 py-3.5 text-center text-base font-semibold transition ${selected ? "border-cyan-500 bg-cyan-50 text-cyan-700" : "border-slate-200 bg-white text-slate-700 hover:border-cyan-300"}`} dir="ltr">{option}</button>;
                })}
              </div>
            </div>
            {quizError && <p className="mt-4 rounded-xl bg-rose-50 px-3 py-2 text-center text-xs font-semibold text-rose-600">{quizError}</p>}
            <div className="mt-6 flex items-center justify-between gap-3">
              <button onClick={() => setQuizOpen(false)} className="rounded-2xl border border-slate-200 px-4 py-3 text-sm font-semibold text-slate-600">انصراف</button>
              {quizIndex < quizQuestions.length - 1 ? <button onClick={nextQuestion} disabled={!quizAnswers[activeQuestion.id]} className="rounded-2xl bg-cyan-600 px-5 py-3 text-sm font-bold text-white disabled:opacity-40">سؤال بعدی ←</button> : <button onClick={() => void submitQuiz()} disabled={!quizAnswers[activeQuestion.id] || quizSubmitting} className="rounded-2xl bg-emerald-600 px-5 py-3 text-sm font-bold text-white disabled:opacity-40">{quizSubmitting ? "در حال ثبت..." : "ثبت آزمون ✓"}</button>}
            </div>
          </>}
        </div>
      </div>}

      {authOpen && <div className="fixed inset-0 z-50 grid place-items-center bg-slate-950/35 px-5 backdrop-blur-sm" onMouseDown={() => setAuthOpen(false)}><div className="w-full max-w-md rounded-3xl bg-white p-6 shadow-2xl" onMouseDown={(event) => event.stopPropagation()}><div className="flex items-start justify-between"><div><p className="text-sm font-bold text-cyan-600">DeutschQuiz</p><h2 className="mt-2 text-2xl font-black">{authMode === "login" ? "خوش برگشتی" : "ساخت حساب کاربری"}</h2></div><button onClick={() => setAuthOpen(false)} className="text-xl text-slate-400">×</button></div><form onSubmit={submitAuth} className="mt-6 space-y-4">{authMode === "register" && <input required value={displayName} onChange={(event) => setDisplayName(event.target.value)} placeholder="نام نمایشی" className="w-full rounded-2xl border border-slate-200 px-4 py-3 text-sm outline-none focus:border-cyan-500" />}<input required type="email" value={email} onChange={(event) => setEmail(event.target.value)} placeholder="ایمیل" className="w-full rounded-2xl border border-slate-200 px-4 py-3 text-sm outline-none focus:border-cyan-500" /><input required minLength={8} type="password" value={password} onChange={(event) => setPassword(event.target.value)} placeholder="رمز عبور (حداقل ۸ کاراکتر)" className="w-full rounded-2xl border border-slate-200 px-4 py-3 text-sm outline-none focus:border-cyan-500" />{authError && <p className="rounded-xl bg-rose-50 px-3 py-2 text-xs font-semibold text-rose-600">{authError}</p>}<button disabled={authLoading} className="w-full rounded-2xl bg-cyan-600 px-4 py-3.5 text-sm font-bold text-white disabled:opacity-60">{authLoading ? "در حال ارسال..." : authMode === "login" ? "ورود" : "ثبت‌نام"}</button></form><button onClick={() => { setAuthMode(authMode === "login" ? "register" : "login"); setAuthError(""); }} className="mt-4 w-full text-center text-xs font-semibold text-slate-500">{authMode === "login" ? "حساب نداری؟ ثبت‌نام کن" : "قبلاً حساب ساخته‌ای؟ وارد شو"}</button></div></div>}
    </main>
  );
}
