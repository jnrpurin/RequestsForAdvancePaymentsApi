using Anticipation.Application.Commands;
using Anticipation.Application.DTOs;
using Anticipation.Domain.Repositories;
using Anticipation.Domain.Services;

namespace Anticipation.Application.Handlers;

public sealed class RejectAnticipationHandler
{
    private readonly IAnticipationRepository _repository;
    private readonly AnticipationDomainService _domainService;

    public RejectAnticipationHandler(IAnticipationRepository repository, AnticipationDomainService domainService)
    {
        _repository = repository;
        _domainService = domainService;
    }

    public async Task<AnticipationResponse> HandleAsync(RejectAnticipationCommand command, CancellationToken cancellationToken = default)
    {
        var request = await _repository.GetByIdAsync(command.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"Anticipation request '{command.Id}' was not found.");

        _domainService.Reject(request, command.Reason);
        await _repository.SaveChangesAsync(cancellationToken);

        return new AnticipationResponse(
            request.Id,
            request.CreatorId,
            request.Amount.Amount,
            request.Amount.Currency,
            request.Status,
            request.CreatedAtUtc,
            request.DecidedAtUtc,
            request.RejectionReason);
    }
}