using System;
using System.Collections.Generic;
using EducationPlatform.Domain.Common;

namespace EducationPlatform.Domain.Entities;

public class ExamAttempt : BaseEntity
{
    public Guid UserId { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public int? TotalCorrect { get; set; }
    public int? TotalIncorrect { get; set; }

    public User? User { get; set; }
    public ICollection<ExamAnswer> Answers { get; set; } = new List<ExamAnswer>();
    public Survey? Survey { get; set; }
}
