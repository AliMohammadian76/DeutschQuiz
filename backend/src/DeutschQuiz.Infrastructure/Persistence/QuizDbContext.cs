using DeutschQuiz.Domain;
using Microsoft.EntityFrameworkCore;

namespace DeutschQuiz.Infrastructure.Persistence;

public sealed class QuizDbContext(DbContextOptions<QuizDbContext> options) : DbContext(options)
{
    public DbSet<BookEntity> Books => Set<BookEntity>();
    public DbSet<LessonEntity> Lessons => Set<LessonEntity>();
    public DbSet<QuizQuestionEntity> Questions => Set<QuizQuestionEntity>();
    public DbSet<QuestionOptionEntity> QuestionOptions => Set<QuestionOptionEntity>();
    public DbSet<UserEntity> Users => Set<UserEntity>();
    public DbSet<QuizAttemptEntity> QuizAttempts => Set<QuizAttemptEntity>();
    public DbSet<QuizAttemptAnswerEntity> QuizAttemptAnswers => Set<QuizAttemptAnswerEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserEntity>(entity =>
        {
            entity.ToTable("users");
            entity.HasKey(user => user.Id);
            entity.Property(user => user.Email).HasMaxLength(320).IsRequired();
            entity.Property(user => user.DisplayName).HasMaxLength(120).IsRequired();
            entity.Property(user => user.PasswordHash).HasMaxLength(500).IsRequired();
            entity.Property(user => user.CreatedAtUtc).IsRequired();
            entity.HasIndex(user => user.Email).IsUnique();
        });

        modelBuilder.Entity<BookEntity>(entity =>
        {
            entity.ToTable("books");
            entity.HasKey(book => book.Id);
            entity.Property(book => book.Name).HasMaxLength(120).IsRequired();
            entity.Property(book => book.Level).HasMaxLength(20).IsRequired();
            entity.Property(book => book.Publisher).HasMaxLength(120);
            entity.HasIndex(book => new { book.Name, book.Level }).IsUnique();
        });

        modelBuilder.Entity<LessonEntity>(entity =>
        {
            entity.ToTable("lessons");
            entity.HasKey(lesson => lesson.Id);
            entity.Property(lesson => lesson.Title).HasMaxLength(200).IsRequired();
            entity.HasOne(lesson => lesson.Book)
                .WithMany(book => book.Lessons)
                .HasForeignKey(lesson => lesson.BookId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasIndex(lesson => new { lesson.BookId, lesson.Number }).IsUnique();
        });

        modelBuilder.Entity<QuizQuestionEntity>(entity =>
        {
            entity.ToTable("questions");
            entity.HasKey(question => question.Id);
            entity.Property(question => question.Category).HasConversion<string>().HasMaxLength(30);
            entity.Property(question => question.Type).HasConversion<string>().HasMaxLength(30);
            entity.Property(question => question.Prompt).HasMaxLength(1000).IsRequired();
            entity.Property(question => question.CorrectAnswer).HasMaxLength(500).IsRequired();
            entity.Property(question => question.Explanation).HasMaxLength(2000);
            entity.HasOne(question => question.Lesson)
                .WithMany(lesson => lesson.Questions)
                .HasForeignKey(question => question.LessonId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasIndex(question => new { question.LessonId, question.Category, question.IsActive });
        });

        modelBuilder.Entity<QuestionOptionEntity>(entity =>
        {
            entity.ToTable("question_options");
            entity.HasKey(option => option.Id);
            entity.Property(option => option.Text).HasMaxLength(500).IsRequired();
            entity.HasOne(option => option.Question)
                .WithMany(question => question.Options)
                .HasForeignKey(option => option.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasIndex(option => new { option.QuestionId, option.SortOrder }).IsUnique();
        });

        modelBuilder.Entity<QuizAttemptEntity>(entity =>
        {
            entity.ToTable("quiz_attempts");
            entity.HasKey(attempt => attempt.Id);
            entity.Property(attempt => attempt.Category).HasConversion<string>().HasMaxLength(30);
            entity.Property(attempt => attempt.Score).HasPrecision(5, 2);
            entity.HasOne(attempt => attempt.Lesson)
                .WithMany()
                .HasForeignKey(attempt => attempt.LessonId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(attempt => attempt.User)
                .WithMany(user => user.Attempts)
                .HasForeignKey(attempt => attempt.UserId)
                .OnDelete(DeleteBehavior.SetNull);
            entity.HasIndex(attempt => new { attempt.UserId, attempt.StartedAtUtc });
        });

        modelBuilder.Entity<QuizAttemptAnswerEntity>(entity =>
        {
            entity.ToTable("quiz_attempt_answers");
            entity.HasKey(answer => answer.Id);
            entity.Property(answer => answer.SelectedAnswer).HasMaxLength(500).IsRequired();
            entity.HasOne(answer => answer.Attempt)
                .WithMany(attempt => attempt.Answers)
                .HasForeignKey(answer => answer.AttemptId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(answer => answer.Question)
                .WithMany()
                .HasForeignKey(answer => answer.QuestionId)
                .OnDelete(DeleteBehavior.Restrict);
            entity.HasIndex(answer => new { answer.AttemptId, answer.QuestionId }).IsUnique();
        });
    }
}

public sealed class UserEntity
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public DateTime CreatedAtUtc { get; set; }
    public ICollection<QuizAttemptEntity> Attempts { get; set; } = [];
}

public sealed class BookEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Level { get; set; } = string.Empty;
    public string? Publisher { get; set; }
    public ICollection<LessonEntity> Lessons { get; set; } = [];
}

public sealed class LessonEntity
{
    public Guid Id { get; set; }
    public Guid BookId { get; set; }
    public int Number { get; set; }
    public string Title { get; set; } = string.Empty;
    public BookEntity Book { get; set; } = null!;
    public ICollection<QuizQuestionEntity> Questions { get; set; } = [];
}

public sealed class QuizQuestionEntity
{
    public Guid Id { get; set; }
    public Guid LessonId { get; set; }
    public QuizCategory Category { get; set; }
    public QuestionType Type { get; set; }
    public string Prompt { get; set; } = string.Empty;
    public string CorrectAnswer { get; set; } = string.Empty;
    public string Explanation { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public LessonEntity Lesson { get; set; } = null!;
    public ICollection<QuestionOptionEntity> Options { get; set; } = [];
}

public sealed class QuestionOptionEntity
{
    public Guid Id { get; set; }
    public Guid QuestionId { get; set; }
    public int SortOrder { get; set; }
    public string Text { get; set; } = string.Empty;
    public QuizQuestionEntity Question { get; set; } = null!;
}

public sealed class QuizAttemptEntity
{
    public Guid Id { get; set; }
    public Guid? UserId { get; set; }
    public Guid LessonId { get; set; }
    public QuizCategory Category { get; set; }
    public DateTime StartedAtUtc { get; set; }
    public DateTime? CompletedAtUtc { get; set; }
    public decimal? Score { get; set; }
    public int TotalQuestions { get; set; }
    public int CorrectAnswers { get; set; }
    public int TotalTimeMs { get; set; }
    public UserEntity? User { get; set; }
    public LessonEntity Lesson { get; set; } = null!;
    public ICollection<QuizAttemptAnswerEntity> Answers { get; set; } = [];
}

public sealed class QuizAttemptAnswerEntity
{
    public Guid Id { get; set; }
    public Guid AttemptId { get; set; }
    public Guid QuestionId { get; set; }
    public string SelectedAnswer { get; set; } = string.Empty;
    public bool IsCorrect { get; set; }
    public int ResponseTimeMs { get; set; }
    public DateTime AnsweredAtUtc { get; set; }
    public QuizAttemptEntity Attempt { get; set; } = null!;
    public QuizQuestionEntity Question { get; set; } = null!;
}
