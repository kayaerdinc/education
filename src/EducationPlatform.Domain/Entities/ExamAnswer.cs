using System;

namespace EducationPlatform.Domain.Entities;

public class ExamAnswer
{
    public Guid AttemptId { get; set; }
    public Guid QuestionId { get; set; }
    public Guid ChoiceId { get; set; }
    public DateTime AnsweredAt { get; set; }

    public ExamAttempt? Attempt { get; set; }
    public Question? Question { get; set; }
    public Choice? Choice { get; set; }
}
