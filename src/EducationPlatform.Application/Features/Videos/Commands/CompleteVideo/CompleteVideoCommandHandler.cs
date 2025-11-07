using EducationPlatform.Application.Abstractions.Persistence;
using EducationPlatform.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EducationPlatform.Application.Features.Videos.Commands.CompleteVideo;

public class CompleteVideoCommandHandler : IRequestHandler<CompleteVideoCommand>
{
    private readonly IApplicationDbContext _dbContext;

    public CompleteVideoCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Handle(CompleteVideoCommand request, CancellationToken cancellationToken)
    {
        var video = await _dbContext.Videos
            .AsNoTracking()
            .FirstOrDefaultAsync(v => v.Id == request.VideoId, cancellationToken);

        if (video is null)
        {
            throw new KeyNotFoundException("Video bulunamadı.");
        }

        var alreadyCompleted = await _dbContext.StudentVideoProgresses
            .AnyAsync(p => p.UserId == request.UserId && p.VideoId == request.VideoId, cancellationToken);

        if (alreadyCompleted)
        {
            return;
        }

        if (video.OrderIndex > 1)
        {
            var precedingVideos = await _dbContext.Videos
                .Where(v => v.OrderIndex < video.OrderIndex)
                .Select(v => v.Id)
                .ToListAsync(cancellationToken);

            var completedPreceding = await _dbContext.StudentVideoProgresses
                .Where(p => p.UserId == request.UserId && precedingVideos.Contains(p.VideoId))
                .Select(p => p.VideoId)
                .ToListAsync(cancellationToken);

            var missing = precedingVideos.Except(completedPreceding).ToList();
            if (missing.Any())
            {
                throw new InvalidOperationException("Önceki videolar tamamlanmadan ilerleme kaydedilemez.");
            }
        }

        var progress = new StudentVideoProgress
        {
            UserId = request.UserId,
            VideoId = request.VideoId,
            CompletedAt = DateTime.UtcNow
        };

        _dbContext.StudentVideoProgresses.Add(progress);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
