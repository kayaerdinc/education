using EducationPlatform.Application.DTOs.Exams;
using MediatR;

namespace EducationPlatform.Application.Features.Instructor.Queries.GetSurveySummaries;

public record GetSurveySummariesQuery : IRequest<IReadOnlyCollection<InstructorSurveySummaryDto>>;
