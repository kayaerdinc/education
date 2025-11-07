using EducationPlatform.Application.Abstractions.Persistence;
using EducationPlatform.Application.DTOs.Exams;
using EducationPlatform.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EducationPlatform.Application.Features.Instructor.Queries.GetStudentSummaries;

public class GetStudentSummariesQueryHandler : IRequestHandler<GetStudentSummariesQuery, IReadOnlyCollection<InstructorStudentSummaryDto>>
{
    private readonly IApplicationDbContext _dbContext;

    public GetStudentSummariesQueryHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyCollection<InstructorStudentSummaryDto>> Handle(GetStudentSummariesQuery request, CancellationToken cancellationToken)
    {
        var studentAttempts = await _dbContext.ExamAttempts
            .Include(a => a.User)
            .Where(a => a.User != null && a.User.Role == UserRole.Student && a.TotalCorrect.HasValue && a.TotalIncorrect.HasValue)
            .OrderByDescending(a => a.CompletedAt)
            .ToListAsync(cancellationToken);

        var summaries = studentAttempts
            .GroupBy(a => a.UserId)
            .Select(g =>
            {
                var latestAttempt = g.OrderByDescending(x => x.CompletedAt).First();
                var totalAnswered = (latestAttempt.TotalCorrect ?? 0) + (latestAttempt.TotalIncorrect ?? 0);
                return new InstructorStudentSummaryDto(
                    latestAttempt.UserId,
                    latestAttempt.User!.FullName,
                    latestAttempt.User.Email,
                    latestAttempt.TotalCorrect ?? 0,
                    latestAttempt.TotalIncorrect ?? 0,
                    totalAnswered,
                    latestAttempt.CompletedAt);
            })
            .OrderBy(s => s.FullName)
            .ToList();

        return summaries;
    }
}
