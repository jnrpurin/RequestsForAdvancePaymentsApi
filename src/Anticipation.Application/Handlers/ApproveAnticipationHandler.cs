using Anticipation.Application.Commands;
using Anticipation.Application.DTOs;
using Anticipation.Domain.Repositories;
using Anticipation.Domain.Services;

namespace Anticipation.Application.Handlers;

public sealed class ApproveAnticipationHandler
{
    private readonly IAnticipationRepository _repository;
    private readonly AnticipationDomainService _domainService;

    public ApproveAnticipationHandler(IAnticipationRepository repository, AnticipationDomainService domainService)
    {
        _repository = repository;
        _domainService = domainService;
    }

    public async Task<AnticipationResponse> HandleAsync(ApproveAnticipationCommand command, CancellationToken cancellationToken = default)
    {
        var request = await _repository.GetByIdAsync(command.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"Anticipation request '{command.Id}' was not found.");

        _domainService.Approve(request);
        await _repository.SaveChangesAsync(cancellationToken);

        return AnticipationResponse.FromDomain(request);
    }
}