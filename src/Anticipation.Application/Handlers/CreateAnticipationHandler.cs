using Anticipation.Application.Commands;
using Anticipation.Application.DTOs;
using Anticipation.Domain.Repositories;
using Anticipation.Domain.Services;

namespace Anticipation.Application.Handlers;

public sealed class CreateAnticipationHandler
{
    private readonly IAnticipationRepository _repository;
    private readonly AnticipationDomainService _domainService;

    public CreateAnticipationHandler(IAnticipationRepository repository, AnticipationDomainService domainService)
    {
        _repository = repository;
        _domainService = domainService;
    }

    public async Task<AnticipationResponse> HandleAsync(CreateAnticipationCommand command, CancellationToken cancellationToken = default)
    {
        var request = _domainService.Create(command.CreatorId, command.RequestedAmount, command.RequestDate);
        await _repository.AddAsync(request, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return AnticipationResponse.FromDomain(request);
    }
}