using Anticipation.Application.Commands;
using Anticipation.Application.DTOs;
using Anticipation.Application.Logging;
using Anticipation.Domain.Repositories;
using Anticipation.Domain.Services;
using Microsoft.Extensions.Logging;

namespace Anticipation.Application.Handlers;

public sealed class ApproveAnticipationHandler
{
    private readonly IAnticipationRepository _repository;
    private readonly AnticipationDomainService _domainService;
    private readonly ILogger<ApproveAnticipationHandler> _logger;

    public ApproveAnticipationHandler(
        IAnticipationRepository repository,
        AnticipationDomainService domainService,
        ILogger<ApproveAnticipationHandler> logger)
    {
        _repository = repository;
        _domainService = domainService;
        _logger = logger;
    }

    public async Task<AnticipationResponse> HandleAsync(ApproveAnticipationCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation(ApplicationLogEvents.ApproveStarted, "Approving anticipation {AnticipationId}", command.Id);

        var request = await _repository.GetByIdAsync(command.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"Anticipation request '{command.Id}' was not found.");

        _domainService.Approve(request);
        await _repository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(ApplicationLogEvents.ApproveSucceeded, "Anticipation {AnticipationId} approved", request.Id);

        return AnticipationResponse.FromDomain(request);
    }
}