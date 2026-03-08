using Asp.Versioning;
using Anticipation.API.Contracts;
using Anticipation.Application.Commands;
using Anticipation.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Anticipation.API.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public sealed class AnticipationsController : ControllerBase
{
    private readonly IAnticipationService _service;

    public AnticipationsController(IAnticipationService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAnticipationRequest request, CancellationToken cancellationToken)
    {
        var response = await _service.CreateAsync(
            new CreateAnticipationCommand(request.CreatorId, request.Amount, request.Currency),
            cancellationToken);

        return CreatedAtAction(nameof(GetByCreator), new { creatorId = response.CreatorId }, response);
    }

    [HttpPut("{id:guid}/approve")]
    public async Task<IActionResult> Approve(Guid id, CancellationToken cancellationToken)
    {
        var response = await _service.ApproveAsync(new ApproveAnticipationCommand(id), cancellationToken);
        return Ok(response);
    }

    [HttpPut("{id:guid}/reject")]
    public async Task<IActionResult> Reject(Guid id, [FromQuery] string reason, CancellationToken cancellationToken)
    {
        var response = await _service.RejectAsync(new RejectAnticipationCommand(id, reason), cancellationToken);
        return Ok(response);
    }

    [HttpGet("creator/{creatorId}")]
    public async Task<IActionResult> GetByCreator(string creatorId, CancellationToken cancellationToken)
    {
        var response = await _service.GetByCreatorAsync(creatorId, cancellationToken);
        return Ok(response);
    }
}