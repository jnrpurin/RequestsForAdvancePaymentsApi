using Anticipation.Application.Commands;
using Anticipation.Application.DTOs;
using Anticipation.Application.Logging;
using Anticipation.Domain.Repositories;
using Anticipation.Domain.Services;
using Microsoft.Extensions.Logging;

namespace Anticipation.Application.Handlers;

public sealed class CreateAnticipationHandler
{
    private readonly IAnticipationRepository _repository;
    private readonly AnticipationDomainService _domainService;
    private readonly ILogger<CreateAnticipationHandler> _logger;

    public CreateAnticipationHandler(
        IAnticipationRepository repository,
        AnticipationDomainService domainService,
        ILogger<CreateAnticipationHandler> logger)
    {
        _repository = repository;
        _domainService = domainService;
        _logger = logger;
    }

    public async Task<AnticipationResponse> HandleAsync(CreateAnticipationCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation(ApplicationLogEvents.CreateStarted, "Creating anticipation for creator {CreatorId} and requested amount {RequestedAmount}", command.CreatorId, command.RequestedAmount);

        var hasPendingRequest = await _repository.HasPendingByCreatorAsync(command.CreatorId, cancellationToken);
        if (hasPendingRequest)
        {
            _logger.LogWarning(ApplicationLogEvents.CreateDeniedPending, "Create denied because creator {CreatorId} already has a pending anticipation", command.CreatorId);
            throw new InvalidOperationException("O creator ja possui uma solicitacao pendente.");
        }

        var request = _domainService.Create(command.CreatorId, command.RequestedAmount, command.RequestDate);
        await _repository.AddAsync(request, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation(ApplicationLogEvents.CreateSucceeded, "Anticipation {AnticipationId} created for creator {CreatorId}", request.Id, request.CreatorId);

        return AnticipationResponse.FromDomain(request);
    }
}