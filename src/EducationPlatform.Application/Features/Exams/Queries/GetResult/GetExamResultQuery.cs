using EducationPlatform.Application.DTOs.Exams;
using MediatR;

namespace EducationPlatform.Application.Features.Exams.Queries.GetResult;

public record GetExamResultQuery(Guid UserId) : IRequest<ExamResultDto>;
