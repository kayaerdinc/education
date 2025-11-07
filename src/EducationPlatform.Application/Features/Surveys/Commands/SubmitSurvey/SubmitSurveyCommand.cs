using MediatR;

namespace EducationPlatform.Application.Features.Surveys.Commands.SubmitSurvey;

public record SubmitSurveyCommand(
    Guid UserId,
    Guid AttemptId,
    bool Liked,
    byte ContentClarityRating,
    byte InstructorSupportRating,
    string? AdditionalFeedback) : IRequest;
