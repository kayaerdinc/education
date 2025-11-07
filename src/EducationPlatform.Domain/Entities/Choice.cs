using System;
using EducationPlatform.Domain.Common;

namespace EducationPlatform.Domain.Entities;

public class Choice : BaseEntity
{
    public Guid QuestionId { get; set; }
    public char ChoiceLabel { get; set; }
    public string Body { get; set; } = string.Empty;
    public bool IsCorrect { get; set; }

    public Question? Question { get; set; }
}
