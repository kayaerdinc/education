namespace EducationPlatform.Application.DTOs.Exams;

public record ExamResultDto(
    Guid AttemptId,
    int TotalQuestions,
    int TotalCorrect,
    int TotalIncorrect,
    DateTime? CompletedAt);
