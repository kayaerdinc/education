using FluentValidation;

namespace EducationPlatform.Application.Features.Videos.Commands.CompleteVideo;

public class CompleteVideoCommandValidator : AbstractValidator<CompleteVideoCommand>
{
    public CompleteVideoCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("Kullanıcı bilgisi zorunludur.");

        RuleFor(x => x.VideoId)
            .NotEmpty().WithMessage("Video bilgisi zorunludur.");
    }
}
