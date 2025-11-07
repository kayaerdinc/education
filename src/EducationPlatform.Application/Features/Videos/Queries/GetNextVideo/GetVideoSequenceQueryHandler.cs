using EducationPlatform.Application.Abstractions.Persistence;
using EducationPlatform.Application.DTOs.Courses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EducationPlatform.Application.Features.Videos.Queries.GetNextVideo;

public class GetVideoSequenceQueryHandler : IRequestHandler<GetVideoSequenceQuery, IReadOnlyCollection<VideoProgressDto>>
{
    private readonly IApplicationDbContext _dbContext;

    public GetVideoSequenceQueryHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyCollection<VideoProgressDto>> Handle(GetVideoSequenceQuery request, CancellationToken cancellationToken)
    {
        var videos = await _dbContext.Videos
            .OrderBy(v => v.OrderIndex)
            .AsNoTracking()
            .Select(v => new
            {
                v.Id,
                v.Title,
                v.Description,
                v.Url,
                v.OrderIndex,
                v.DurationSeconds,
                v.ThumbnailUrl
            })
            .ToListAsync(cancellationToken);

        var progress = await _dbContext.StudentVideoProgresses
            .Where(p => p.UserId == request.UserId)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var orderedProgress = videos
            .Select(v =>
            {
                var isCompleted = progress.Any(p => p.VideoId == v.Id);
                return new VideoProgressDto(
                    v.Id,
                    v.Title,
                    v.Description,
                    v.Url,
                    v.OrderIndex,
                    isCompleted,
                    false,
                    v.DurationSeconds,
                    v.ThumbnailUrl);
            })
            .OrderBy(v => v.OrderIndex)
            .ToList();

        var lockFlag = false;
        for (var i = 0; i < orderedProgress.Count; i++)
        {
            if (lockFlag)
            {
                orderedProgress[i] = orderedProgress[i] with { IsLocked = true };
            }

            if (!orderedProgress[i].IsCompleted)
            {
                lockFlag = true;
            }
        }

        return orderedProgress;
    }
}
