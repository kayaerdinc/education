namespace EducationPlatform.Application.DTOs.Exams;

public record InstructorStudentSummaryDto(
    Guid UserId,
    string FullName,
    string Email,
    int TotalCorrect,
    int TotalIncorrect,
    int TotalAnswered,
    DateTime? CompletedAt);
