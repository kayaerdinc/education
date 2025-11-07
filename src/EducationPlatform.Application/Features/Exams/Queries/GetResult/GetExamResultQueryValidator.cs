using FluentValidation;

namespace EducationPlatform.Application.Features.Exams.Queries.GetResult;

public class GetExamResultQueryValidator : AbstractValidator<GetExamResultQuery>
{
    public GetExamResultQueryValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
    }
}
