# DeutschQuiz

وب‌اپ آزمون زبان آلمانی برای `Menschen A1.1` و `Starten wir! A1.1`.

## ساختار

- `backend/` — API مستقل با ASP.NET Core و Clean Architecture
- `frontend/` — رابط کاربری مستقل با Next.js، TypeScript و Tailwind CSS

## کار تیمی و شاخه‌ها

- شاخه‌ی پیش‌فرض: `master` (همیشه قابل انتشار)
- کار روزانه روی شاخه‌های کوتاه‌مدت (`frontend/…`، `backend/…`) و ادغام با PR به `master`
- جزئیات workflow و دعوت همکار: [CONTRIBUTING.md](CONTRIBUTING.md)

## وضعیت فعلی

- آزمون‌های درس‌به‌درس برای ۱۲ درس هر دو کتاب `Menschen A1.1` و `Starten wir! A1.1`
- سه حالت آزمون برای هر درس: واژگان (`Vocabulary`)، گرامر (`Grammar`) و جامع (`Mixed`)
- هر درس در حالت جامع ۲۰ سؤال دارد: ۱۰ واژگان و ۱۰ گرامر
- سؤال‌های درس‌های بالاتر، نکات پایه‌ی درس‌های قبلی را هم به‌صورت تجمعی مرور می‌کنند.
- انتخاب کتاب و درس از رابط کاربری و دریافت سؤال‌های همان درس از API
- ثبت‌نام، ورود با JWT و نمایش پیشرفت کاربر
- ثبت نتیجه‌ی آزمون و زمان پاسخ‌گویی به هر سؤال

## اجرای بک‌اند

```bash
dotnet run --project backend/src/DeutschQuiz.Api
```

APIهای اولیه:

- `GET /api/health`
- `GET /api/lessons`
- `GET /api/lessons/{lessonId}/questions?category=Vocabulary|Grammar|Mixed`

کاتالوگ فعلی `Menschen A1.1` شامل این درس‌هاست:

1. `Hallo! Ich bin ...`
2. `Familie und Freunde`
3. `Zahlen und Alltag`
4. `Essen und Trinken`
5. `Wohnen`
6. `Freizeit`
7. `Arbeit und Termine`
8. `Kleidung und Farben`
9. `Gesundheit`
10. `Unterwegs`
11. `Wetter und Jahreszeiten`
12. `Reisen und Pläne`

## حساب کاربری و پیشرفت

با تنظیم `ConnectionStrings__DeutschQuiz` و `Jwt__SigningKey`، endpointهای زیر فعال می‌شوند:

- `POST /api/auth/register` — ساخت حساب با `email`، `password` و `displayName`
- `POST /api/auth/login` — دریافت JWT
- `POST /api/attempts` — ثبت نتیجه‌ی آزمون و زمان پاسخ هر سؤال (نیازمند Bearer token)
- `GET /api/progress/summary` — خلاصه‌ی نمره، بهترین نتیجه، زمان و پیشرفت درس‌ها؛ برای هر درس میانگین، بهترین نمره، تعداد پاسخ درست و زمان کل هم برمی‌گردد
- `GET /api/progress/history?limit=10` — آخرین تلاش‌های کاربر با نوع آزمون، نمره، زمان و تاریخ

پاسخ `POST /api/attempts` علاوه بر نمره و زمان کل، آرایه‌ی `answers` را برمی‌گرداند.
هر آیتم شامل متن سؤال، پاسخ انتخاب‌شده، پاسخ درست، وضعیت صحیح/غلط، توضیح و زمان
پاسخ است؛ پاسخ درست فقط بعد از ثبت آزمون ارسال می‌شود.

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

در اولین اجرای API، migrationها و داده‌های نمونه‌ی هر ۱۲ درس هر دو کتاب به صورت خودکار اعمال می‌شوند. Seeder به شکل افزایشی کار می‌کند؛ بنابراین با اجرای مجدد، درس‌ها و سؤال‌های موجود دوباره درج نمی‌شوند.
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
