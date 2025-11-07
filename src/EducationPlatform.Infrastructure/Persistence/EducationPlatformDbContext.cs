using EducationPlatform.Application.Abstractions.Persistence;
using EducationPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EducationPlatform.Infrastructure.Persistence;

public class EducationPlatformDbContext : DbContext, IApplicationDbContext
{
    public EducationPlatformDbContext(DbContextOptions<EducationPlatformDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Video> Videos => Set<Video>();
    public DbSet<StudentVideoProgress> StudentVideoProgresses => Set<StudentVideoProgress>();
    public DbSet<Question> Questions => Set<Question>();
    public DbSet<Choice> Choices => Set<Choice>();
    public DbSet<ExamAttempt> ExamAttempts => Set<ExamAttempt>();
    public DbSet<ExamAnswer> ExamAnswers => Set<ExamAnswer>();
    public DbSet<Survey> Surveys => Set<Survey>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(EducationPlatformDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}
