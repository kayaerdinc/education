namespace EducationPlatform.Application.DTOs.Exams;

public record ExamQuestionResponseDto(
    Guid AttemptId,
    ExamQuestionDto Question,
    bool IsLastQuestion);
