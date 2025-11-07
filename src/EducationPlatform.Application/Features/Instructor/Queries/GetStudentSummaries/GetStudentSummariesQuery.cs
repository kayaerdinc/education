using EducationPlatform.Application.DTOs.Exams;
using MediatR;

namespace EducationPlatform.Application.Features.Instructor.Queries.GetStudentSummaries;

public record GetStudentSummariesQuery : IRequest<IReadOnlyCollection<InstructorStudentSummaryDto>>;
