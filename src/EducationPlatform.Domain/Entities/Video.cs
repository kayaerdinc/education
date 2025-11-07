using System.Collections.Generic;
using EducationPlatform.Domain.Common;

namespace EducationPlatform.Domain.Entities;

public class Video : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Url { get; set; } = string.Empty;
    public int OrderIndex { get; set; }
    public int? DurationSeconds { get; set; }
    public string? ThumbnailUrl { get; set; }

    public ICollection<StudentVideoProgress> ProgressRecords { get; set; } = new List<StudentVideoProgress>();
}
