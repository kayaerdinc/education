using EducationPlatform.Application.Abstractions.Persistence;
using EducationPlatform.Application.DTOs.Exams;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EducationPlatform.Application.Features.Exams.Commands.CompleteExam;

public class CompleteExamCommandHandler : IRequestHandler<CompleteExamCommand, ExamResultDto>
{
    private readonly IApplicationDbContext _dbContext;

    public CompleteExamCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ExamResultDto> Handle(CompleteExamCommand request, CancellationToken cancellationToken)
    {
        var attempt = await _dbContext.ExamAttempts
            .Include(a => a.Answers)
            .FirstOrDefaultAsync(a => a.Id == request.AttemptId && a.UserId == request.UserId, cancellationToken)
            ?? throw new InvalidOperationException("Sınav denemesi bulunamadı.");

        if (attempt.CompletedAt is not null)
        {
            return new ExamResultDto(
                attempt.Id,
                await _dbContext.Questions.CountAsync(cancellationToken),
                attempt.TotalCorrect ?? 0,
                attempt.TotalIncorrect ?? 0,
                attempt.CompletedAt);
        }

        var totalQuestions = await _dbContext.Questions.CountAsync(cancellationToken);
        if (attempt.Answers.Count < totalQuestions)
        {
            throw new InvalidOperationException("Tüm sorular cevaplanmadan sınav tamamlanamaz.");
        }

        var correctAnswerIds = await _dbContext.Choices
            .Where(c => c.IsCorrect)
            .Select(c => new { c.QuestionId, c.Id })
            .ToListAsync(cancellationToken);

        var correctLookup = correctAnswerIds.ToDictionary(x => x.QuestionId, x => x.Id);

        var totalCorrect = attempt.Answers.Count(answer =>
            correctLookup.TryGetValue(answer.QuestionId, out var correctChoiceId) &&
            correctChoiceId == answer.ChoiceId);

        var totalIncorrect = totalQuestions - totalCorrect;

        attempt.TotalCorrect = totalCorrect;
        attempt.TotalIncorrect = totalIncorrect;
        attempt.CompletedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new ExamResultDto(attempt.Id, totalQuestions, totalCorrect, totalIncorrect, attempt.CompletedAt);
    }
}
