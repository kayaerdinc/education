using System;
using System.Collections.Generic;
using EducationPlatform.Domain.Common;
using EducationPlatform.Domain.Enums;

namespace EducationPlatform.Domain.Entities;

public class User : BaseEntity
{
    public string Email { get; set; } = string.Empty;
    public byte[] PasswordHash { get; set; } = Array.Empty<byte>();
    public byte[] PasswordSalt { get; set; } = Array.Empty<byte>();
    public UserRole Role { get; set; }
    public string FullName { get; set; } = string.Empty;
    public bool IsActive { get; set; }

    public ICollection<StudentVideoProgress> VideoProgresses { get; set; } = new List<StudentVideoProgress>();
    public ICollection<ExamAttempt> ExamAttempts { get; set; } = new List<ExamAttempt>();
}
