using EducationPlatform.Application.DTOs.Exams;
using EducationPlatform.Application.Features.Instructor.Queries.GetStudentSummaries;
using EducationPlatform.Application.Features.Instructor.Queries.GetSurveySummaries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EducationPlatform.Api.Controllers;

[ApiController]
[Authorize(Roles = "Instructor")]
[Route("api/[controller]")]
public class InstructorController : ControllerBase
{
    private readonly IMediator _mediator;

    public InstructorController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("students")]
    [ProducesResponseType(typeof(IReadOnlyCollection<InstructorStudentSummaryDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetStudentSummaries(CancellationToken cancellationToken)
    {
        var query = new GetStudentSummariesQuery();
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpGet("surveys")]
    [ProducesResponseType(typeof(IReadOnlyCollection<InstructorSurveySummaryDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSurveySummaries(CancellationToken cancellationToken)
    {
        var query = new GetSurveySummariesQuery();
        var result = await _mediator.Send(query, cancellationToken);
        return Ok(result);
    }
}
