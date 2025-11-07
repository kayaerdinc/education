namespace EducationPlatform.Application.DTOs.Exams;

public record ExamQuestionDto(
    Guid QuestionId,
    string Body,
    int Order,
    IReadOnlyCollection<ExamChoiceDto> Choices);
