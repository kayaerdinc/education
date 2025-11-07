using EducationPlatform.Api.Extensions;
using EducationPlatform.Application.DTOs.Exams;
using EducationPlatform.Application.Features.Surveys.Commands.SubmitSurvey;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EducationPlatform.Api.Controllers;

[ApiController]
[Authorize(Roles = "Student")]
[Route("api/[controller]")]
public class SurveyController : ControllerBase
{
    private readonly IMediator _mediator;

    public SurveyController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Submit([FromBody] SurveyRequestDto requestDto, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();
        var command = new SubmitSurveyCommand(
            userId,
            requestDto.AttemptId,
            requestDto.Liked,
            requestDto.ContentClarityRating,
            requestDto.InstructorSupportRating,
            requestDto.AdditionalFeedback);

        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }
}
