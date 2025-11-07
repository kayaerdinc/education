using FluentValidation;

namespace EducationPlatform.Application.Features.Exams.Commands.CompleteExam;

public class CompleteExamCommandValidator : AbstractValidator<CompleteExamCommand>
{
    public CompleteExamCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.AttemptId).NotEmpty();
    }
}
