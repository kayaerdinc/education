using System;

namespace EducationPlatform.Domain.Entities;

public class StudentVideoProgress
{
    public Guid UserId { get; set; }
    public Guid VideoId { get; set; }
    public DateTime CompletedAt { get; set; }

    public User? User { get; set; }
    public Video? Video { get; set; }
}
