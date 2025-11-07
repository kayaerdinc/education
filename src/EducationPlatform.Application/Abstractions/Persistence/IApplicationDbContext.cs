using EducationPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace EducationPlatform.Application.Abstractions.Persistence;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    DbSet<Video> Videos { get; }
    DbSet<StudentVideoProgress> StudentVideoProgresses { get; }
    DbSet<Question> Questions { get; }
    DbSet<Choice> Choices { get; }
    DbSet<ExamAttempt> ExamAttempts { get; }
    DbSet<ExamAnswer> ExamAnswers { get; }
    DbSet<Survey> Surveys { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
