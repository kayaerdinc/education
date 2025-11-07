namespace EducationPlatform.Application.DTOs.Exams;

public record InstructorSurveySummaryDto(
    Guid AttemptId,
    Guid UserId,
    string StudentName,
    bool Liked,
    byte ContentClarityRating,
    byte InstructorSupportRating,
    string? AdditionalFeedback,
    DateTime SubmittedAt);
