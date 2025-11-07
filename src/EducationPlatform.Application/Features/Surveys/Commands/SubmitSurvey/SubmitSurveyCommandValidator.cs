using FluentValidation;

namespace EducationPlatform.Application.Features.Surveys.Commands.SubmitSurvey;

public class SubmitSurveyCommandValidator : AbstractValidator<SubmitSurveyCommand>
{
    public SubmitSurveyCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.AttemptId).NotEmpty();
        RuleFor(x => x.ContentClarityRating)
            .InclusiveBetween((byte)1, (byte)5);
        RuleFor(x => x.InstructorSupportRating)
            .InclusiveBetween((byte)1, (byte)5);
        RuleFor(x => x.AdditionalFeedback)
            .MaximumLength(1000);
    }
}
