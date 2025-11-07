using EducationPlatform.Application.Abstractions.Persistence;
using EducationPlatform.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EducationPlatform.Application.Features.Surveys.Commands.SubmitSurvey;

public class SubmitSurveyCommandHandler : IRequestHandler<SubmitSurveyCommand>
{
    private readonly IApplicationDbContext _dbContext;

    public SubmitSurveyCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Handle(SubmitSurveyCommand request, CancellationToken cancellationToken)
    {
        var attempt = await _dbContext.ExamAttempts
            .Include(a => a.Survey)
            .FirstOrDefaultAsync(a => a.Id == request.AttemptId && a.UserId == request.UserId, cancellationToken)
            ?? throw new InvalidOperationException("Sınav denemesi bulunamadı.");

        if (attempt.CompletedAt is null)
        {
            throw new InvalidOperationException("Sınav tamamlanmadan anket doldurulamaz.");
        }

        if (attempt.Survey is not null)
        {
            throw new InvalidOperationException("Bu deneme için anket zaten doldurulmuş.");
        }

        if (request.ContentClarityRating is < 1 or > 5 ||
            request.InstructorSupportRating is < 1 or > 5)
        {
            throw new InvalidOperationException("Puanlamalar 1-5 arasında olmalıdır.");
        }

        var survey = new Survey
        {
            Id = Guid.NewGuid(),
            AttemptId = request.AttemptId,
            Liked = request.Liked,
            ContentClarityRating = request.ContentClarityRating,
            InstructorSupportRating = request.InstructorSupportRating,
            AdditionalFeedback = request.AdditionalFeedback,
            SubmittedAt = DateTime.UtcNow
        };

        await _dbContext.Surveys.AddAsync(survey, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
