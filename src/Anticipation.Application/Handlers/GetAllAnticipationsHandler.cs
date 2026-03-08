using Anticipation.Application.DTOs;
using Anticipation.Application.Logging;
using Anticipation.Application.Queries;
using Anticipation.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Anticipation.Application.Handlers;

public sealed class GetAllAnticipationsHandler
{
    private readonly IAnticipationRepository _repository;
    private readonly ILogger<GetAllAnticipationsHandler> _logger;

    public GetAllAnticipationsHandler(IAnticipationRepository repository, ILogger<GetAllAnticipationsHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<PagedResponse<AnticipationResponse>> HandleAsync(GetAllAnticipationsQuery query, CancellationToken cancellationToken = default)
    {
        var (items, totalCount) = await _repository.GetPagedAsync(query.Page, query.PageSize, cancellationToken);
        var mappedItems = items.Select(AnticipationResponse.FromDomain).ToList();
        var totalPages = totalCount == 0 ? 0 : (int)Math.Ceiling(totalCount / (double)query.PageSize);

        _logger.LogInformation(
            ApplicationLogEvents.ListPaged,
            "Listed anticipations page {Page} with page size {PageSize}. Returned {ReturnedItems} items from total {TotalItems}",
            query.Page,
            query.PageSize,
            mappedItems.Count,
            totalCount);

        return new PagedResponse<AnticipationResponse>(
            query.Page,
            query.PageSize,
            totalCount,
            totalPages,
            mappedItems);
    }
}