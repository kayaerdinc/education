using EducationPlatform.Application.Abstractions.Persistence;
using EducationPlatform.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EducationPlatform.Application.Features.Exams.Commands.SubmitAnswer;

public class SubmitExamAnswerCommandHandler : IRequestHandler<SubmitExamAnswerCommand>
{
    private readonly IApplicationDbContext _dbContext;

    public SubmitExamAnswerCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Handle(SubmitExamAnswerCommand request, CancellationToken cancellationToken)
    {
        var attempt = await _dbContext.ExamAttempts
            .Include(a => a.Answers)
            .FirstOrDefaultAsync(a => a.Id == request.AttemptId && a.UserId == request.UserId, cancellationToken);

        if (attempt is null || attempt.CompletedAt is not null)
        {
            throw new InvalidOperationException("Geçersiz veya tamamlanmış bir sınav denemesi.");
        }

        var question = await _dbContext.Questions
            .Include(q => q.Choices)
            .FirstOrDefaultAsync(q => q.Id == request.QuestionId, cancellationToken)
            ?? throw new KeyNotFoundException("Soru bulunamadı.");

        var choice = question.Choices.FirstOrDefault(c => c.Id == request.ChoiceId)
                     ?? throw new InvalidOperationException("Seçilen şık mevcut soruya ait değil.");

        var previousQuestionsAnswered = attempt.Answers.Count;
        if (question.DisplayOrder > previousQuestionsAnswered + 1)
        {
            throw new InvalidOperationException("Sıradaki soruya geçmeden cevap verilemez.");
        }

        var existingAnswer = attempt.Answers.FirstOrDefault(a => a.QuestionId == request.QuestionId);
        if (existingAnswer is not null)
        {
            existingAnswer.ChoiceId = request.ChoiceId;
            existingAnswer.AnsweredAt = DateTime.UtcNow;
        }
        else
        {
            var answer = new ExamAnswer
            {
                AttemptId = attempt.Id,
                QuestionId = request.QuestionId,
                ChoiceId = request.ChoiceId,
                AnsweredAt = DateTime.UtcNow
            };
            _dbContext.ExamAnswers.Add(answer);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
