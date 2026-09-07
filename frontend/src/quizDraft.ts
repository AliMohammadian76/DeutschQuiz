export type QuizCategory = "Vocabulary" | "Grammar" | "Mixed";

export type QuizQuestion = {
  id: string;
  lessonId: string;
  category: QuizCategory;
  type: string;
  prompt: string;
  options: string[];
};

export type QuizDraft = {
  lessonId: string;
  category: QuizCategory;
  questions: QuizQuestion[];
  answers: Record<string, string>;
  times: Record<string, number>;
  quizIndex: number;
  startedAtUtc: string;
};

export const QUIZ_DRAFT_KEY = "deutschquiz.quizDraft";

export function loadDraft(): QuizDraft | null {
  try {
    const raw = localStorage.getItem(QUIZ_DRAFT_KEY);
    if (!raw) return null;
    const draft = JSON.parse(raw) as QuizDraft;
    if (
      !draft?.lessonId ||
      !draft?.category ||
      !Array.isArray(draft.questions) ||
      !draft.questions.length
    ) {
      return null;
    }
    return draft;
  } catch {
    return null;
  }
}

export function saveDraft(draft: QuizDraft) {
  localStorage.setItem(QUIZ_DRAFT_KEY, JSON.stringify(draft));
}

export function clearDraft() {
  localStorage.removeItem(QUIZ_DRAFT_KEY);
}

export function draftMatches(
  draft: QuizDraft | null,
  lessonId: string,
  category: QuizCategory,
) {
  return (
    draft !== null &&
    draft.lessonId === lessonId &&
    draft.category === category
  );
}
