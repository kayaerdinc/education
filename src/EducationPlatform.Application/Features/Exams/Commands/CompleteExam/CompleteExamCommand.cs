using EducationPlatform.Application.DTOs.Exams;
using MediatR;

namespace EducationPlatform.Application.Features.Exams.Commands.CompleteExam;

public record CompleteExamCommand(Guid UserId, Guid AttemptId) : IRequest<ExamResultDto>;
