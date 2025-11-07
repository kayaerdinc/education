using EducationPlatform.Application.DTOs.Exams;
using MediatR;

namespace EducationPlatform.Application.Features.Exams.Queries.GetQuestion;

public record GetExamQuestionQuery(Guid UserId, int OrderIndex) : IRequest<ExamQuestionResponseDto>;
