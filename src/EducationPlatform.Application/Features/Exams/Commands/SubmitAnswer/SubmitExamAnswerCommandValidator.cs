
using FluentValidation;

namespace EducationPlatform.Application.Features.Exams.Commands.SubmitAnswer;

public class SubmitExamAnswerCommandValidator : AbstractValidator<SubmitExamAnswerCommand>
{
    public SubmitExamAnswerCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.AttemptId).NotEmpty();
        RuleFor(x => x.QuestionId).NotEmpty();
        RuleFor(x => x.ChoiceId).NotEmpty();
    }
}
