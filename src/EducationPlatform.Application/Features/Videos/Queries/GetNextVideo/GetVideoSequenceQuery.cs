using EducationPlatform.Application.DTOs.Courses;
using MediatR;

namespace EducationPlatform.Application.Features.Videos.Queries.GetNextVideo;

public record GetVideoSequenceQuery(Guid UserId) : IRequest<IReadOnlyCollection<VideoProgressDto>>;
