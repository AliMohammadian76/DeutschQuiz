# DeutschQuiz

وب‌اپ آزمون زبان آلمانی برای `Menschen A1.1`.

## ساختار

- `backend/` — API مستقل با ASP.NET Core و Clean Architecture
- `frontend/` — رابط کاربری مستقل با Next.js، TypeScript و Tailwind CSS

## اجرای بک‌اند

```bash
dotnet run --project backend/src/DeutschQuiz.Api
```

APIهای اولیه:

- `GET /api/health`
- `GET /api/lessons`
- `GET /api/lessons/{lessonId}/questions?category=Vocabulary`

## اجرای فرانت‌اند

```bash
cd frontend
npm install
npm run dev
```

فرانت‌اند روی `http://localhost:3000` اجرا می‌شود.

محتوای سؤال‌های نمونه باید原创 و مستقل از متن و تمرین‌های دارای حق نشر کتاب طراحی شود.
