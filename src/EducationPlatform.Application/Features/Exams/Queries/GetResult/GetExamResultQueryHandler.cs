using EducationPlatform.Application.Abstractions.Persistence;
using EducationPlatform.Application.DTOs.Exams;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EducationPlatform.Application.Features.Exams.Queries.GetResult;

public class GetExamResultQueryHandler : IRequestHandler<GetExamResultQuery, ExamResultDto>
{
    private readonly IApplicationDbContext _dbContext;

    public GetExamResultQueryHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ExamResultDto> Handle(GetExamResultQuery request, CancellationToken cancellationToken)
    {
        var attempt = await _dbContext.ExamAttempts
            .Where(a => a.UserId == request.UserId && a.CompletedAt != null)
            .OrderByDescending(a => a.CompletedAt)
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new KeyNotFoundException("Tamamlanmış sınav sonucu bulunamadı.");

        var totalQuestions = await _dbContext.Questions.CountAsync(cancellationToken);

        return new ExamResultDto(
            attempt.Id,
            totalQuestions,
            attempt.TotalCorrect ?? 0,
            attempt.TotalIncorrect ?? 0,
            attempt.CompletedAt);
    }
}
