# DeutschQuiz

وب‌اپ آزمون زبان آلمانی برای `Menschen A1.1`.

## ساختار

- `backend/` — API مستقل با ASP.NET Core و Clean Architecture
- `frontend/` — رابط کاربری مستقل با Next.js، TypeScript و Tailwind CSS

## وضعیت فعلی

- آزمون‌های واژگان، گرامر و جامع برای `Menschen A1.1 / Lektion 1`
- ثبت‌نام، ورود با JWT و نمایش پیشرفت کاربر
- ثبت نتیجه‌ی آزمون و زمان پاسخ‌گویی به هر سؤال

## اجرای بک‌اند

```bash
dotnet run --project backend/src/DeutschQuiz.Api
```

APIهای اولیه:

- `GET /api/health`
- `GET /api/lessons`
- `GET /api/lessons/{lessonId}/questions?category=Vocabulary`

## حساب کاربری و پیشرفت

با تنظیم `ConnectionStrings__DeutschQuiz` و `Jwt__SigningKey`، endpointهای زیر فعال می‌شوند:

- `POST /api/auth/register` — ساخت حساب با `email`، `password` و `displayName`
- `POST /api/auth/login` — دریافت JWT
- `POST /api/attempts` — ثبت نتیجه‌ی آزمون و زمان پاسخ هر سؤال (نیازمند Bearer token)
- `GET /api/progress/summary` — خلاصه‌ی نمره، بهترین نتیجه، زمان و پیشرفت درس‌ها

نمونه‌ی بدنه‌ی ثبت نتیجه:

```json
{
  "lessonId": "11111111-1111-1111-1111-111111111111",
  "category": "Mixed",
  "startedAtUtc": "2026-09-01T10:00:00Z",
  "answers": [
    {
      "questionId": "20000000-0000-0000-0000-000000000001",
      "selectedAnswer": "Wie geht's?",
      "responseTimeMs": 4200
    },
    {
      "questionId": "20000000-0000-0000-0000-000000000002",
      "selectedAnswer": "bin",
      "responseTimeMs": 3100
    },
    {
      "questionId": "20000000-0000-0000-0000-000000000003",
      "selectedAnswer": "Wie",
      "responseTimeMs": 2800
    },
    {
      "questionId": "20000000-0000-0000-0000-000000000004",
      "selectedAnswer": "komme",
      "responseTimeMs": 3500
    }
  ]
}
```

برای آزمون‌های `Vocabulary` یا `Grammar` فقط سؤال‌های همان دسته را ارسال کنید.
برای `Mixed` همه‌ی سؤال‌های فعال درس باید در `answers` وجود داشته باشند؛ پاسخ ناقص
ثبت نمی‌شود.

## دیتابیس PostgreSQL

دیتابیس اصلی پروژه PostgreSQL است و EF Core در لایه‌ی Infrastructure قرار دارد.
در حالت توسعه، اگر connection string تنظیم نشده باشد، API برای راحتی با داده‌ی موقت in-memory اجرا می‌شود.

1. فایل `.env.example` را به `.env` کپی کنید و مقدار رمز محلی را تغییر دهید.
2. PostgreSQL را بالا بیاورید:

```bash
docker compose --env-file .env up -d postgres
```

3. قبل از اجرای API، connection string را در محیط تنظیم کنید:

PowerShell:

```powershell
$env:ConnectionStrings__DeutschQuiz = "Host=localhost;Port=5432;Database=deutschquiz;Username=deutschquiz;Password=YOUR_LOCAL_PASSWORD"
dotnet run --project backend/src/DeutschQuiz.Api
```

در اولین اجرای API، migrationها و داده‌ی نمونه‌ی `Menschen A1.1 / Lektion 1` به صورت خودکار اعمال می‌شوند.
از این مرحله به بعد، تغییرات schema با EF Core migration نسخه‌بندی می‌شوند.

## اجرای فرانت‌اند

```bash
cd frontend
npm install
npm run dev
```

فرانت‌اند روی `http://localhost:3000` اجرا می‌شود.
در صورت نیاز، مقدار `NEXT_PUBLIC_API_BASE_URL` را مطابق `frontend/.env.example` تنظیم کن.

محتوای سؤال‌های نمونه باید原创 و مستقل از متن و تمرین‌های دارای حق نشر کتاب طراحی شود.
