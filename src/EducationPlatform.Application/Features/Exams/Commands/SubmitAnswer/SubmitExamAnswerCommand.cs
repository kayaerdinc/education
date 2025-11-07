using MediatR;

namespace EducationPlatform.Application.Features.Exams.Commands.SubmitAnswer;

public record SubmitExamAnswerCommand(Guid UserId, Guid AttemptId, Guid QuestionId, Guid ChoiceId) : IRequest;
