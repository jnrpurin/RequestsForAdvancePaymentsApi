using Anticipation.Application.DTOs;
using Anticipation.Application.Queries;
using Anticipation.Domain.Services;

namespace Anticipation.Application.Handlers;

public sealed class SimulateAnticipationHandler
{
    private const decimal FeePercentage = 5.00m;

    private readonly AnticipationDomainService _domainService;

    public SimulateAnticipationHandler(AnticipationDomainService domainService)
    {
        _domainService = domainService;
    }

    public Task<AnticipationSimulationResponse> HandleAsync(SimulateAnticipationQuery query, CancellationToken cancellationToken = default)
    {
        var simulatedRequest = _domainService.Create(query.CreatorId, query.RequestedAmount, query.RequestDate);
        var feeValue = decimal.Round(simulatedRequest.RequestedAmount.Amount - simulatedRequest.NetAmount, 2, MidpointRounding.AwayFromZero);

        var response = new AnticipationSimulationResponse(
            simulatedRequest.CreatorId,
            simulatedRequest.RequestedAmount.Amount,
            FeePercentage,
            feeValue,
            simulatedRequest.NetAmount,
            simulatedRequest.RequestDate,
            "pendentef");

        return Task.FromResult(response);
    }
}