using FluentValidation;

namespace EducationPlatform.Application.Features.Exams.Queries.GetQuestion;

public class GetExamQuestionQueryValidator : AbstractValidator<GetExamQuestionQuery>
{
    public GetExamQuestionQueryValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("Kullanıcı bilgisi zorunludur.");
        RuleFor(x => x.OrderIndex)
            .GreaterThan(0).WithMessage("Soru sırası 0'dan büyük olmalıdır.");
    }
}
