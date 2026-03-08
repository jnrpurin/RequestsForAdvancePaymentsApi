using Anticipation.Application.DTOs;
using Anticipation.Application.Logging;
using Anticipation.Application.Queries;
using Anticipation.Domain.Services;
using Microsoft.Extensions.Logging;

namespace Anticipation.Application.Handlers;

public sealed class SimulateAnticipationHandler
{
    private const decimal FeePercentage = 5.00m;

    private readonly AnticipationDomainService _domainService;
    private readonly ILogger<SimulateAnticipationHandler> _logger;

    public SimulateAnticipationHandler(
        AnticipationDomainService domainService,
        ILogger<SimulateAnticipationHandler> logger)
    {
        _domainService = domainService;
        _logger = logger;
    }

    public Task<AnticipationSimulationResponse> HandleAsync(SimulateAnticipationQuery query, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation(ApplicationLogEvents.Simulate, "Simulating anticipation for creator {CreatorId} and requested amount {RequestedAmount}", query.CreatorId, query.RequestedAmount);

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