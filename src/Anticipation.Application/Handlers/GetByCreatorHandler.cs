using Anticipation.Application.DTOs;
using Anticipation.Application.Queries;
using Anticipation.Domain.Repositories;

namespace Anticipation.Application.Handlers;

public sealed class GetByCreatorHandler
{
    private readonly IAnticipationRepository _repository;

    public GetByCreatorHandler(IAnticipationRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<AnticipationResponse>> HandleAsync(GetByCreatorQuery query, CancellationToken cancellationToken = default)
    {
        var requests = await _repository.GetByCreatorAsync(query.CreatorId, cancellationToken);

        return requests
            .Select(AnticipationResponse.FromDomain)
            .ToList();
    }
}