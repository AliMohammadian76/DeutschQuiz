export type Language = "fa" | "en";

/** Set to true when the FA/EN header switcher should be visible again. */
export const SHOW_LANGUAGE_SWITCHER = false;

/** Public UI language while the switcher is hidden. EN strings stay fully implemented. */
export const ACTIVE_UI_LANGUAGE: Language = "fa";

type Messages = {
  requestFailed: string;
  logout: string;
  login: string;
  register: string;
  heroHeadline: string;
  heroSubcopy: string;
  themeToLight: string;
  themeToDark: string;
  startQuiz: string;
  preparing: string;
  progress: string;
  translator: string;
  questionsReady: string;
  pickLesson: string;
  levelLabel: (level: string) => string;
  averageWith: (avg: number) => string;
  statAverage: string;
  statBest: string;
  statCorrect: string;
  statTime: string;
  secondsShort: (n: number) => string;
  progressHeading: (book: string, level: string) => string;
  progressChartHeading: string;
  progressChartEyebrow: string;
  chartScore: string;
  chartAverage: string;
  chartBest: string;
  chartEmpty: string;
  lessonCount: (n: number) => string;
  noProgressInLevel: string;
  attemptsCount: (n: number) => string;
  bestScore: (n: number) => string;
  historyHeading: string;
  refresh: string;
  loading: string;
  noAttemptsYet: string;
  fetching: string;
  categoryVocabulary: string;
  categoryGrammar: string;
  categoryMixed: string;
  dateUnknown: string;
  timeLabel: string;
  pickBook: string;
  pickLevelHeading: string;
  pickLessonHeading: string;
  stepBack: string;
  lessonsMeta: (count: number, book: string, level: string) => string;
  quizTypeHeading: string;
  start: string;
  continueQuiz: string;
  inProgress: string;
  exitQuiz: string;
  resumeHint: string;
  quizModes: {
    Vocabulary: { title: string; description: string };
    Grammar: { title: string; description: string };
    Mixed: { title: string; description: string };
  };
  resultCorrectSummary: (correct: number, total: number, seconds: number) => string;
  yourAnswer: string;
  correctLabel: string;
  back: string;
  questionOf: (index: number, total: number) => string;
  cancel: string;
  next: string;
  submitQuiz: string;
  submitting: string;
  displayNamePlaceholder: string;
  emailPlaceholder: string;
  passwordPlaceholder: string;
  authSubmitting: string;
  noAccountRegister: string;
  haveAccountLogin: string;
  questionsUnavailable: string;
  noQuestionsForMode: string;
  fetchQuestionsFailed: string;
  loginToSaveResult: string;
  submitResultFailed: string;
  lessonsUnavailable: string;
  fetchLessonsFailed: string;
  genericError: string;
  translatorHeading: string;
  translatorSubcopy: string;
  translatorInputLabel: string;
  translatorOutputLabel: string;
  translatorInputPlaceholder: string;
  translatorOutputPlaceholder: string;
  translateAction: string;
  translating: string;
  translationDirectionLabel: string;
  translationDirectionDeToFa: string;
  translationDirectionFaToDe: string;
  translationTextRequired: string;
  translationFailed: string;
};

const fa: Messages = {
  requestFailed: "درخواست انجام نشد.",
  logout: "خروج",
  login: "ورود",
  register: "ثبت‌نام",
  heroHeadline: "آلمانی را با آزمون‌های کوتاه و دقیق تمرین کن.",
  heroSubcopy: "کتاب را انتخاب کن، درس را بزن، و در چند دقیقه پیشرفت کن.",
  themeToLight: "حالت روشن",
  themeToDark: "حالت تاریک",
  startQuiz: "شروع آزمون",
  preparing: "آماده‌سازی...",
  progress: "پیشرفت",
  translator: "مترجم",
  questionsReady: "۲۰ سؤال آماده",
  pickLesson: "درس را انتخاب کن",
  levelLabel: (level) => `سطح ${level}`,
  averageWith: (avg) => ` · میانگین ${avg}٪`,
  statAverage: "میانگین",
  statBest: "بهترین",
  statCorrect: "درست",
  statTime: "زمان",
  secondsShort: (n) => `${n} ثانیه`,
  progressHeading: (book, level) => `پیشرفت · ${book} ${level}`,
  progressChartHeading: "نمودار پیشرفت کاربر",
  progressChartEyebrow: "User Progress Chart",
  chartScore: "نمره",
  chartAverage: "میانگین",
  chartBest: "بهترین",
  chartEmpty: "هنوز داده‌ای برای رسم نمودار نیست.",
  lessonCount: (n) => `${n} درس`,
  noProgressInLevel: "هنوز در این سطح آزمونی ثبت نکرده‌ای.",
  attemptsCount: (n) => `${n} آزمون`,
  bestScore: (n) => `بهترین ${n}٪`,
  historyHeading: "تاریخچه‌ی آزمون‌ها",
  refresh: "به‌روزرسانی",
  loading: "...",
  noAttemptsYet: "هنوز آزمونی ثبت نکرده‌ای.",
  fetching: "در حال دریافت...",
  categoryVocabulary: "واژگان",
  categoryGrammar: "گرامر",
  categoryMixed: "جامع",
  dateUnknown: "تاریخ نامشخص",
  timeLabel: "زمان",
  pickBook: "انتخاب کتاب",
  pickLevelHeading: "انتخاب سطح",
  pickLessonHeading: "انتخاب درس",
  stepBack: "قبلی",
  lessonsMeta: (count, book, level) => `${count} درس · ${book} ${level}`,
  quizTypeHeading: "نوع آزمون",
  start: "شروع",
  continueQuiz: "ادامه آزمون",
  inProgress: "نیمه‌تمام",
  exitQuiz: "خروج از آزمون",
  resumeHint: "می‌توانی بعداً از همین‌جا ادامه دهی.",
  quizModes: {
    Vocabulary: {
      title: "واژگان",
      description: "کلمات کلیدی درس را مرور کن.",
    },
    Grammar: {
      title: "گرامر",
      description: "ساختارهای پایه و جمله‌سازی را بسنج.",
    },
    Mixed: {
      title: "آزمون جامع",
      description: "ترکیبی از واژگان و گرامر.",
    },
  },
  resultCorrectSummary: (correct, total, seconds) =>
    `${correct} از ${total} درست · ${seconds} ثانیه`,
  yourAnswer: "پاسخ تو:",
  correctLabel: "درست:",
  back: "بازگشت",
  questionOf: (index, total) => `سؤال ${index} از ${total}`,
  cancel: "انصراف",
  next: "بعدی",
  submitQuiz: "ثبت آزمون",
  submitting: "ثبت...",
  displayNamePlaceholder: "نام نمایشی",
  emailPlaceholder: "ایمیل",
  passwordPlaceholder: "رمز عبور (حداقل ۸ کاراکتر)",
  authSubmitting: "در حال ارسال...",
  noAccountRegister: "حساب نداری؟ ثبت‌نام کن",
  haveAccountLogin: "قبلاً حساب ساخته‌ای؟ وارد شو",
  questionsUnavailable: "سؤال‌های این آزمون در دسترس نیست.",
  noQuestionsForMode: "برای این حالت هنوز سؤالی ثبت نشده است.",
  fetchQuestionsFailed: "دریافت سؤال‌ها انجام نشد.",
  loginToSaveResult: "برای ثبت نتیجه، ابتدا وارد حساب کاربری شو.",
  submitResultFailed: "ثبت نتیجه انجام نشد.",
  lessonsUnavailable: "فهرست درس‌ها در دسترس نیست.",
  fetchLessonsFailed: "دریافت درس‌ها انجام نشد.",
  genericError: "خطایی رخ داد.",
  translatorHeading: "مترجم آلمانی/فارسی",
  translatorSubcopy: "متن دلخواه را وارد کن و ترجمه‌ی سریع بگیر.",
  translatorInputLabel: "متن ورودی",
  translatorOutputLabel: "ترجمه",
  translatorInputPlaceholder: "مثلاً: Guten Morgen یا سلام، حالت چطوره؟",
  translatorOutputPlaceholder: "خروجی ترجمه اینجا نمایش داده می‌شود.",
  translateAction: "ترجمه",
  translating: "در حال ترجمه...",
  translationDirectionLabel: "جهت ترجمه",
  translationDirectionDeToFa: "آلمانی → فارسی",
  translationDirectionFaToDe: "فارسی → آلمانی",
  translationTextRequired: "لطفاً متن ورودی را وارد کن.",
  translationFailed: "ترجمه انجام نشد.",
};

const en: Messages = {
  requestFailed: "Request failed.",
  logout: "Log out",
  login: "Log in",
  register: "Sign up",
  heroHeadline: "Practice German with short, focused quizzes.",
  heroSubcopy: "Pick a book, choose a lesson, and make progress in minutes.",
  themeToLight: "Light mode",
  themeToDark: "Dark mode",
  startQuiz: "Start quiz",
  preparing: "Preparing...",
  progress: "Progress",
  translator: "Translator",
  questionsReady: "20 questions ready",
  pickLesson: "Pick a lesson",
  levelLabel: (level) => `Level ${level}`,
  averageWith: (avg) => ` · avg ${avg}%`,
  statAverage: "Average",
  statBest: "Best",
  statCorrect: "Correct",
  statTime: "Time",
  secondsShort: (n) => `${n}s`,
  progressHeading: (book, level) => `Progress · ${book} ${level}`,
  progressChartHeading: "User Progress Chart",
  progressChartEyebrow: "Fortschritt",
  chartScore: "Score",
  chartAverage: "Average",
  chartBest: "Best",
  chartEmpty: "No data yet to draw the chart.",
  lessonCount: (n) => `${n} lessons`,
  noProgressInLevel: "No attempts recorded for this level yet.",
  attemptsCount: (n) => `${n} attempts`,
  bestScore: (n) => `Best ${n}%`,
  historyHeading: "Quiz history",
  refresh: "Refresh",
  loading: "...",
  noAttemptsYet: "No attempts yet.",
  fetching: "Loading...",
  categoryVocabulary: "Vocabulary",
  categoryGrammar: "Grammar",
  categoryMixed: "Mixed",
  dateUnknown: "Unknown date",
  timeLabel: "Time",
  pickBook: "Choose a book",
  pickLevelHeading: "Choose a level",
  pickLessonHeading: "Choose a lesson",
  stepBack: "Back",
  lessonsMeta: (count, book, level) => `${count} lessons · ${book} ${level}`,
  quizTypeHeading: "Quiz type",
  start: "Start",
  continueQuiz: "Continue quiz",
  inProgress: "In progress",
  exitQuiz: "Exit quiz",
  resumeHint: "You can continue from here later.",
  quizModes: {
    Vocabulary: {
      title: "Vocabulary",
      description: "Review the key words from this lesson.",
    },
    Grammar: {
      title: "Grammar",
      description: "Check basic structures and sentence building.",
    },
    Mixed: {
      title: "Full quiz",
      description: "A mix of vocabulary and grammar.",
    },
  },
  resultCorrectSummary: (correct, total, seconds) =>
    `${correct} of ${total} correct · ${seconds}s`,
  yourAnswer: "Your answer:",
  correctLabel: "Correct:",
  back: "Back",
  questionOf: (index, total) => `Question ${index} of ${total}`,
  cancel: "Cancel",
  next: "Next",
  submitQuiz: "Submit quiz",
  submitting: "Saving...",
  displayNamePlaceholder: "Display name",
  emailPlaceholder: "Email",
  passwordPlaceholder: "Password (min. 8 characters)",
  authSubmitting: "Sending...",
  noAccountRegister: "No account? Sign up",
  haveAccountLogin: "Already have an account? Log in",
  questionsUnavailable: "Questions for this quiz are unavailable.",
  noQuestionsForMode: "No questions are registered for this mode yet.",
  fetchQuestionsFailed: "Could not load questions.",
  loginToSaveResult: "Log in to save your result.",
  submitResultFailed: "Could not save the result.",
  lessonsUnavailable: "Lesson list is unavailable.",
  fetchLessonsFailed: "Could not load lessons.",
  genericError: "Something went wrong.",
  translatorHeading: "German/Persian Translator",
  translatorSubcopy: "Enter any text and get a quick translation.",
  translatorInputLabel: "Input text",
  translatorOutputLabel: "Translation",
  translatorInputPlaceholder: "Example: Guten Morgen or سلام، حالت چطوره؟",
  translatorOutputPlaceholder: "Translated text appears here.",
  translateAction: "Translate",
  translating: "Translating...",
  translationDirectionLabel: "Direction",
  translationDirectionDeToFa: "German -> Persian",
  translationDirectionFaToDe: "Persian -> German",
  translationTextRequired: "Please enter text to translate.",
  translationFailed: "Could not translate the text.",
};

const catalogs: Record<Language, Messages> = { fa, en };

export function getMessages(language: Language): Messages {
  return catalogs[language];
}

export function localeFor(language: Language): string {
  return language === "en" ? "en-US" : "fa-IR";
}

export function dirFor(language: Language): "rtl" | "ltr" {
  return language === "en" ? "ltr" : "rtl";
}
