using EducationPlatform.Application.Abstractions.Persistence;
using EducationPlatform.Application.DTOs.Exams;
using EducationPlatform.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EducationPlatform.Application.Features.Exams.Queries.GetQuestion;

public class GetExamQuestionQueryHandler : IRequestHandler<GetExamQuestionQuery, ExamQuestionResponseDto>
{
    private readonly IApplicationDbContext _dbContext;

    public GetExamQuestionQueryHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ExamQuestionResponseDto> Handle(GetExamQuestionQuery request, CancellationToken cancellationToken)
    {
        await EnsureAllVideosCompletedAsync(request.UserId, cancellationToken);

        var attempt = await _dbContext.ExamAttempts
            .FirstOrDefaultAsync(a => a.UserId == request.UserId && a.CompletedAt == null, cancellationToken);

        if (attempt is null)
        {
            attempt = new ExamAttempt
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                StartedAt = DateTime.UtcNow
            };

            await _dbContext.ExamAttempts.AddAsync(attempt, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        var totalQuestions = await _dbContext.Questions.CountAsync(cancellationToken);

        var question = await _dbContext.Questions
            .AsNoTracking()
            .Include(q => q.Choices)
            .FirstOrDefaultAsync(q => q.DisplayOrder == request.OrderIndex, cancellationToken);

        if (question is null)
        {
            throw new KeyNotFoundException("Soru bulunamadı.");
        }

        var dto = new ExamQuestionDto(
            question.Id,
            question.Body,
            question.DisplayOrder,
            question.Choices
                .OrderBy(c => c.ChoiceLabel)
                .Select(c => new ExamChoiceDto(c.Id, c.ChoiceLabel, c.Body))
                .ToList());

        var isLastQuestion = question.DisplayOrder == totalQuestions;

        return new ExamQuestionResponseDto(attempt.Id, dto, isLastQuestion);
    }

    private async Task EnsureAllVideosCompletedAsync(Guid userId, CancellationToken cancellationToken)
    {
        var totalVideos = await _dbContext.Videos.CountAsync(cancellationToken);
        var completedVideos = await _dbContext.StudentVideoProgresses
            .CountAsync(p => p.UserId == userId, cancellationToken);

        if (totalVideos == 0)
        {
            return;
        }

        if (completedVideos < totalVideos)
        {
            throw new InvalidOperationException("Tüm eğitim videoları tamamlanmadan sınava başlanamaz.");
        }
    }
}
