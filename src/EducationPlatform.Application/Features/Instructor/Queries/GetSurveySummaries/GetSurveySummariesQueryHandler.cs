using EducationPlatform.Application.Abstractions.Persistence;
using EducationPlatform.Application.DTOs.Exams;
using EducationPlatform.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EducationPlatform.Application.Features.Instructor.Queries.GetSurveySummaries;

public class GetSurveySummariesQueryHandler : IRequestHandler<GetSurveySummariesQuery, IReadOnlyCollection<InstructorSurveySummaryDto>>
{
    private readonly IApplicationDbContext _dbContext;

    public GetSurveySummariesQueryHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyCollection<InstructorSurveySummaryDto>> Handle(GetSurveySummariesQuery request, CancellationToken cancellationToken)
    {
        var summaries = await _dbContext.Surveys
            .Where(s => s.Attempt != null
                        && s.Attempt.User != null
                        && s.Attempt.User.Role == UserRole.Student)
            .OrderByDescending(s => s.SubmittedAt)
            .Select(s => new InstructorSurveySummaryDto(
                s.AttemptId,
                s.Attempt!.UserId,
                s.Attempt!.User!.FullName,
                s.Liked,
                s.ContentClarityRating,
                s.InstructorSupportRating,
                s.AdditionalFeedback,
                s.SubmittedAt))
            .ToListAsync(cancellationToken);

        return summaries;
    }
}
