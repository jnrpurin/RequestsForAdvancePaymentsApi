using Anticipation.Application.Commands;
using Anticipation.Application.DTOs;
using Anticipation.Application.Logging;
using Anticipation.Domain.Repositories;
using Anticipation.Domain.Services;
using Microsoft.Extensions.Logging;

namespace Anticipation.Application.Handlers;

public sealed class RejectAnticipationHandler
{
    private readonly IAnticipationRepository _repository;
    private readonly AnticipationDomainService _domainService;
    private readonly ILogger<RejectAnticipationHandler> _logger;

    public RejectAnticipationHandler(
        IAnticipationRepository repository,
        AnticipationDomainService domainService,
        ILogger<RejectAnticipationHandler> logger)
    {
        _repository = repository;
        _domainService = domainService;
        _logger = logger;
    }

    public async Task<AnticipationResponse> HandleAsync(RejectAnticipationCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation(ApplicationLogEvents.RejectStarted, "Rejecting anticipation {AnticipationId}", command.Id);

        var request = await _repository.GetByIdAsync(command.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"Anticipation request '{command.Id}' was not found.");

        _domainService.Reject(request);
        await _repository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(ApplicationLogEvents.RejectSucceeded, "Anticipation {AnticipationId} rejected", request.Id);

        return AnticipationResponse.FromDomain(request);
    }
}