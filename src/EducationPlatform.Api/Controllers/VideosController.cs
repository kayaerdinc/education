using EducationPlatform.Api.Extensions;
using EducationPlatform.Application.DTOs.Courses;
using EducationPlatform.Application.Features.Videos.Commands.CompleteVideo;
using EducationPlatform.Application.Features.Videos.Queries.GetNextVideo;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EducationPlatform.Api.Controllers;

[ApiController]
[Authorize(Roles = "Student")]
[Route("api/[controller]")]
public class VideosController : ControllerBase
{
    private readonly IMediator _mediator;

    public VideosController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyCollection<VideoProgressDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSequence(CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();
        var query = new GetVideoSequenceQuery(userId);
        var videos = await _mediator.Send(query, cancellationToken);
        return Ok(videos);
    }

    [HttpPost("{videoId:guid}/complete")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> CompleteVideo(Guid videoId, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();
        var command = new CompleteVideoCommand(userId, videoId);
        await _mediator.Send(command, cancellationToken);
        return NoContent();
    }
}
