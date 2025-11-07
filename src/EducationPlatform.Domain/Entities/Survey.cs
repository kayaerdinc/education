using System;
using EducationPlatform.Domain.Common;

namespace EducationPlatform.Domain.Entities;

public class Survey : BaseEntity
{
    public Guid AttemptId { get; set; }
    public bool Liked { get; set; }
    public byte ContentClarityRating { get; set; }
    public byte InstructorSupportRating { get; set; }
    public string? AdditionalFeedback { get; set; }
    public DateTime SubmittedAt { get; set; }

    public ExamAttempt? Attempt { get; set; }
}
