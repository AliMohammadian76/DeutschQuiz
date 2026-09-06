import ReactECharts from "echarts-for-react";
import type { EChartsOption } from "echarts";

export type ProgressChartAttempt = {
  attemptId: string;
  book: string;
  level: string;
  lessonNumber: number;
  score: number;
  completedAtUtc: string | null;
};

export type ProgressChartLesson = {
  lessonId: string;
  lessonNumber: number;
  title: string;
  averageScore: number;
  bestScore: number;
};

type UserProgressChartProps = {
  attempts: ProgressChartAttempt[];
  lessons: ProgressChartLesson[];
  scoreLabel: string;
  averageLabel: string;
  bestLabel: string;
  emptyLabel: string;
  locale: string;
  rtl: boolean;
};

function formatAxisDate(value: string | null, locale: string) {
  if (!value) return "—";
  return new Date(value).toLocaleDateString(locale, {
    month: "short",
    day: "numeric",
  });
}

export function UserProgressChart({
  attempts,
  lessons,
  scoreLabel,
  averageLabel,
  bestLabel,
  emptyLabel,
  locale,
  rtl,
}: UserProgressChartProps) {
  const chronological = [...attempts]
    .filter((attempt) => attempt.completedAtUtc)
    .sort(
      (a, b) =>
        new Date(a.completedAtUtc!).getTime() -
        new Date(b.completedAtUtc!).getTime(),
    );

  const lessonBars = [...lessons].sort((a, b) => a.lessonNumber - b.lessonNumber);
  const hasData = chronological.length > 0 || lessonBars.length > 0;

  if (!hasData) {
    return (
      <p className="rounded-3xl border border-dashed border-line bg-de-mist px-4 py-10 text-center text-sm text-muted">
        {emptyLabel}
      </p>
    );
  }

  const scores = chronological.map((attempt) => Math.round(attempt.score));
  const averageScore =
    scores.length > 0
      ? Math.round(scores.reduce((sum, score) => sum + score, 0) / scores.length)
      : 0;

  const option: EChartsOption = {
    color: ["#e11d2e", "#f5c518"],
    textStyle: {
      fontFamily: "inherit",
      color: "#5c564c",
    },
    tooltip: {
      trigger: "axis",
      axisPointer: { type: "cross" },
      backgroundColor: "rgba(255,248,232,0.96)",
      borderColor: "#e8e0d0",
      textStyle: { color: "#141414" },
    },
    legend: {
      data: chronological.length
        ? [scoreLabel, ...(lessonBars.length ? [averageLabel, bestLabel] : [])]
        : [averageLabel, bestLabel],
      top: 0,
      textStyle: { color: "#5c564c" },
    },
    grid: [
      {
        left: rtl ? 48 : 40,
        right: rtl ? 40 : 48,
        top: chronological.length && lessonBars.length ? "12%" : "14%",
        height: chronological.length && lessonBars.length ? "34%" : "68%",
        containLabel: true,
      },
      ...(lessonBars.length && chronological.length
        ? [
            {
              left: rtl ? 48 : 40,
              right: rtl ? 40 : 48,
              top: "56%",
              height: "30%",
              containLabel: true,
            },
          ]
        : []),
    ],
    xAxis: [
      ...(chronological.length
        ? [
            {
              type: "category" as const,
              data: chronological.map((attempt) =>
                formatAxisDate(attempt.completedAtUtc, locale),
              ),
              boundaryGap: false,
              axisLine: { lineStyle: { color: "#e8e0d0" } },
              axisLabel: { color: "#5c564c", hideOverlap: true },
              axisTick: { show: false },
            },
          ]
        : []),
      ...(lessonBars.length
        ? [
            {
              type: "category" as const,
              gridIndex: chronological.length ? 1 : 0,
              data: lessonBars.map((lesson) => `L${lesson.lessonNumber}`),
              axisLine: { lineStyle: { color: "#e8e0d0" } },
              axisLabel: { color: "#5c564c" },
              axisTick: { show: false },
            },
          ]
        : []),
    ],
    yAxis: [
      ...(chronological.length
        ? [
            {
              type: "value" as const,
              min: 0,
              max: 100,
              axisLabel: { formatter: "{value}%", color: "#5c564c" },
              splitLine: { lineStyle: { color: "#efe8da", type: "dashed" as const } },
            },
          ]
        : []),
      ...(lessonBars.length
        ? [
            {
              type: "value" as const,
              gridIndex: chronological.length ? 1 : 0,
              min: 0,
              max: 100,
              axisLabel: { formatter: "{value}%", color: "#5c564c" },
              splitLine: { lineStyle: { color: "#efe8da", type: "dashed" as const } },
            },
          ]
        : []),
    ],
    series: [
      ...(chronological.length
        ? [
            {
              name: scoreLabel,
              type: "line" as const,
              smooth: true,
              symbol: "circle",
              symbolSize: 8,
              data: scores,
              areaStyle: {
                color: {
                  type: "linear" as const,
                  x: 0,
                  y: 0,
                  x2: 0,
                  y2: 1,
                  colorStops: [
                    { offset: 0, color: "rgba(225,29,46,0.35)" },
                    { offset: 1, color: "rgba(225,29,46,0.02)" },
                  ],
                },
              },
              lineStyle: { width: 3, color: "#e11d2e" },
              itemStyle: { color: "#e11d2e", borderColor: "#fff", borderWidth: 2 },
              markLine: {
                silent: true,
                symbol: "none",
                data: [
                  {
                    yAxis: averageScore,
                    label: {
                      formatter: `${averageLabel} ${averageScore}%`,
                      color: "#141414",
                      fontSize: 11,
                    },
                    lineStyle: { color: "#141414", type: "dashed" as const, width: 1.5 },
                  },
                ],
              },
              tooltip: {
                valueFormatter: (value: unknown) => `${value}%`,
              },
            },
          ]
        : []),
      ...(lessonBars.length
        ? [
            {
              name: averageLabel,
              type: "bar" as const,
              xAxisIndex: chronological.length ? 1 : 0,
              yAxisIndex: chronological.length ? 1 : 0,
              barMaxWidth: 28,
              data: lessonBars.map((lesson) => Math.round(lesson.averageScore)),
              itemStyle: {
                color: "#f5c518",
                borderRadius: [8, 8, 0, 0],
              },
              tooltip: {
                valueFormatter: (value: unknown) => `${value}%`,
              },
            },
            {
              name: bestLabel,
              type: "bar" as const,
              xAxisIndex: chronological.length ? 1 : 0,
              yAxisIndex: chronological.length ? 1 : 0,
              barMaxWidth: 28,
              data: lessonBars.map((lesson) => lesson.bestScore),
              itemStyle: {
                color: "#e11d2e",
                borderRadius: [8, 8, 0, 0],
              },
              tooltip: {
                valueFormatter: (value: unknown) => `${value}%`,
              },
            },
          ]
        : []),
    ],
  };

  return (
    <ReactECharts
      option={option}
      style={{ height: chronological.length && lessonBars.length ? 420 : 300, width: "100%" }}
      opts={{ renderer: "canvas" }}
      notMerge
    />
  );
}
