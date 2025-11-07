namespace EducationPlatform.Application.DTOs.Exams;

public class SurveyRequestDto
{
    public Guid AttemptId { get; init; }
    public bool Liked { get; init; }
    public byte ContentClarityRating { get; init; }
    public byte InstructorSupportRating { get; init; }
    public string? AdditionalFeedback { get; init; }
}
