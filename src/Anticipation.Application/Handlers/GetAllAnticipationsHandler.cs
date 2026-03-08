using Anticipation.Application.DTOs;
using Anticipation.Application.Queries;
using Anticipation.Domain.Repositories;

namespace Anticipation.Application.Handlers;

public sealed class GetAllAnticipationsHandler
{
    private readonly IAnticipationRepository _repository;

    public GetAllAnticipationsHandler(IAnticipationRepository repository)
    {
        _repository = repository;
    }

    public async Task<PagedResponse<AnticipationResponse>> HandleAsync(GetAllAnticipationsQuery query, CancellationToken cancellationToken = default)
    {
        var (items, totalCount) = await _repository.GetPagedAsync(query.Page, query.PageSize, cancellationToken);
        var mappedItems = items.Select(AnticipationResponse.FromDomain).ToList();
        var totalPages = totalCount == 0 ? 0 : (int)Math.Ceiling(totalCount / (double)query.PageSize);

        return new PagedResponse<AnticipationResponse>(
            query.Page,
            query.PageSize,
            totalCount,
            totalPages,
            mappedItems);
    }
}