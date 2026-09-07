# DeutschQuiz

DeutschQuiz is a full-stack German learning application built around short, lesson-based quizzes. It helps learners practise vocabulary and grammar, track quiz performance over time, complete lessons progressively, and maintain a daily learning streak.

## Highlights

- Lesson-based content for **Menschen** (`A1.1` through `B1.2`) and **Starten wir!** (`A1`, `A2`, and `B1`)
- Three quiz modes per lesson: **Vocabulary**, **Grammar**, and **Mixed**
- User registration and sign-in with JWT authentication
- PostgreSQL-backed quiz attempts, answer timing, and progress history
- ECharts-powered progress charts, separated by quiz category
- Completion indicators for quiz sections, lessons, and books
- Daily learning streak with current streak, best streak, and the last seven days of activity

## Technology Stack

| Area | Technologies |
| --- | --- |
| Frontend | React, TypeScript, Vite, Tailwind CSS, ECharts |
| Backend | ASP.NET Core, Entity Framework Core, JWT Bearer Authentication |
| Database | PostgreSQL |
| Local infrastructure | Docker Compose |

## Quick Start

### Prerequisites

- .NET SDK 10 or later
- Node.js 20 or later
- Docker Desktop

### Start the database

```powershell
Copy-Item .env.example .env
docker compose --env-file .env up -d postgres
```

### Start the API

```powershell
dotnet run --project backend/src/DeutschQuiz.Api --launch-profile http
```

The API runs at `http://localhost:5083`. It applies migrations and seeds quiz content on startup.

### Start the frontend

```powershell
cd frontend
npm install
npm run dev
```

Open `http://localhost:5173` in a browser.

## Configuration

The API uses PostgreSQL and JWT settings from `.env` or environment variables.

```dotenv
ConnectionStrings__DeutschQuiz=Host=127.0.0.1;Port=5432;Database=deutschquiz;Username=deutschquiz;Password=YOUR_PASSWORD;SSL Mode=Disable;Timeout=30
Jwt__SigningKey=replace-with-a-long-development-secret
Jwt__Issuer=DeutschQuiz.Api
Jwt__Audience=DeutschQuiz.Web
```

For a non-default API address, set this in `frontend/.env`:

```dotenv
VITE_API_BASE_URL=http://localhost:5083/api
```

## API Overview

| Method | Endpoint | Description |
| --- | --- | --- |
| `GET` | `/api/health` | Service health check |
| `GET` | `/api/lessons` | List lessons |
| `GET` | `/api/lessons/{lessonId}/questions?category=Vocabulary\|Grammar\|Mixed` | Get lesson questions |
| `POST` | `/api/auth/register` | Create an account |
| `POST` | `/api/auth/login` | Receive a JWT |
| `POST` | `/api/attempts` | Save a quiz attempt |
| `GET` | `/api/progress/summary` | Get aggregate and per-lesson progress |
| `GET` | `/api/progress/history?limit=500` | Get attempt history |

Protected endpoints require:

```http
Authorization: Bearer <access-token>
```

## Development Commands

```powershell
# Frontend
cd frontend
npm run build
npm run lint

# Backend
dotnet build DeutschQuiz.slnx
```

## Progress Rules

- A quiz section is complete after its matching quiz mode has been saved.
- A lesson is complete after Vocabulary, Grammar, and Mixed are all complete.
- A book is complete after every lesson in that book is complete.
- A streak is a consecutive run of calendar days with at least one saved quiz attempt.

## Contributing

Use `develop` for active work and reserve `master` for release-ready code. See [CONTRIBUTING.md](CONTRIBUTING.md) for the collaboration workflow.

## Content Notice

Quiz content is original practice material and is not a reproduction of copyrighted textbook exercises.