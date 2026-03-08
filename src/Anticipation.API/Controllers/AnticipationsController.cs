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
    private const int DefaultPage = 1;
    private const int DefaultPageSize = 20;
    private const int MaxPageSize = 100;

    private readonly IAnticipationService _service;

    public AnticipationsController(IAnticipationService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAnticipationRequest request, CancellationToken cancellationToken)
    {
        var response = await _service.CreateAsync(
            new CreateAnticipationCommand(request.creator_id, request.valor_solicitado, request.data_solicitacao),
            cancellationToken);

        return CreatedAtAction(nameof(GetByCreator), new { version = "1", creatorId = response.creator_id }, response);
    }

    [HttpGet("simulate")]
    public async Task<IActionResult> Simulate(
        [FromQuery(Name = "creator_id")] string creatorId,
        [FromQuery(Name = "valor_solicitado")] decimal requestedAmount,
        [FromQuery(Name = "data_solicitacao")] DateTime requestDate,
        CancellationToken cancellationToken)
    {
        var response = await _service.SimulateAsync(creatorId, requestedAmount, requestDate, cancellationToken);
        return Ok(response);
    }

    [HttpPut("{id:guid}/approve")]
    public async Task<IActionResult> Approve(Guid id, CancellationToken cancellationToken)
    {
        var response = await _service.ApproveAsync(new ApproveAnticipationCommand(id), cancellationToken);
        return Ok(response);
    }

    [HttpPut("{id:guid}/reject")]
    public async Task<IActionResult> Reject(Guid id, CancellationToken cancellationToken)
    {
        var response = await _service.RejectAsync(new RejectAnticipationCommand(id), cancellationToken);
        return Ok(response);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int page = DefaultPage, [FromQuery] int pageSize = DefaultPageSize, CancellationToken cancellationToken = default)
    {
        var normalizedPage = page < 1 ? DefaultPage : page;
        var normalizedPageSize = pageSize < 1 ? DefaultPageSize : Math.Min(pageSize, MaxPageSize);

        var response = await _service.GetAllAsync(normalizedPage, normalizedPageSize, cancellationToken);
        return Ok(response);
    }

    [HttpGet("creator/{creatorId}")]
    public async Task<IActionResult> GetByCreator(string creatorId, CancellationToken cancellationToken)
    {
        var response = await _service.GetByCreatorAsync(creatorId, cancellationToken);
        return Ok(response);
    }
}