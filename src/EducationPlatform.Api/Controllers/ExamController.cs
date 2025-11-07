using EducationPlatform.Api.Extensions;
using EducationPlatform.Application.DTOs.Exams;
using EducationPlatform.Application.Features.Exams.Commands.CompleteExam;
using EducationPlatform.Application.Features.Exams.Commands.SubmitAnswer;
using EducationPlatform.Application.Features.Exams.Queries.GetQuestion;
using EducationPlatform.Application.Features.Exams.Queries.GetResult;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EducationPlatform.Api.Controllers;

[ApiController]
[Authorize(Roles = "Student")]
[Route("api/[controller]")]
public class ExamController : ControllerBase
{
    private readonly IMediator _mediator;

    public ExamController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("questions/{orderIndex:int}")]
    [ProducesResponseType(typeof(ExamQuestionResponseDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetQuestion(int orderIndex, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();
        var query = new GetExamQuestionQuery(userId, orderIndex);
        var response = await _mediator.Send(query, cancellationToken);
        return Ok(response);
    }

    public record SubmitAnswerRequest(Guid AttemptId, Guid QuestionId, Guid ChoiceId);

    [HttpPost("answers")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> SubmitAnswer([FromBody] SubmitAnswerRequest request, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();
        var command = new SubmitExamAnswerCommand(userId, request.AttemptId, request.QuestionId, request.ChoiceId);
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }

    public record CompleteExamRequest(Guid AttemptId);

    [HttpPost("complete")]
    [ProducesResponseType(typeof(ExamResultDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> CompleteExam([FromBody] CompleteExamRequest request, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();
        var command = new CompleteExamCommand(userId, request.AttemptId);
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    [HttpGet("result")]
    [ProducesResponseType(typeof(ExamResultDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetResult(CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();
        var query = new GetExamResultQuery(userId);
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }
}
