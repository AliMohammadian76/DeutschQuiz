export type Theme = "dark" | "light" | "system";

export const THEME_STORAGE_KEY = "deutschquiz.theme";

export function getStoredTheme(): Theme {
  try {
    const saved = localStorage.getItem(THEME_STORAGE_KEY);
    return saved === "light" || saved === "system" ? saved : "dark";
  } catch {
    return "dark";
  }
}

export function applyTheme(theme: Theme) {
  const prefersLight = window.matchMedia("(prefers-color-scheme: light)").matches;
  document.documentElement.classList.toggle(
    "light",
    theme === "light" || (theme === "system" && prefersLight),
  );
  try {
    localStorage.setItem(THEME_STORAGE_KEY, theme);
  } catch {
    /* ignore quota / private mode */
  }
}

