namespace EducationPlatform.Application.DTOs.Courses;

public record VideoProgressDto(
    Guid VideoId,
    string Title,
    string? Description,
    string Url,
    int OrderIndex,
    bool IsCompleted,
    bool IsLocked,
    int? DurationSeconds,
    string? ThumbnailUrl);
