using Anticipation.Application.Commands;
using Anticipation.Application.DTOs;
using Anticipation.Application.Handlers;
using Anticipation.Application.Interfaces;
using Anticipation.Application.Queries;

namespace Anticipation.Application.Services;

public sealed class AnticipationService : IAnticipationService
{
    private readonly CreateAnticipationHandler _createHandler;
    private readonly SimulateAnticipationHandler _simulateHandler;
    private readonly ApproveAnticipationHandler _approveHandler;
    private readonly RejectAnticipationHandler _rejectHandler;
    private readonly GetAllAnticipationsHandler _getAllAnticipationsHandler;
    private readonly GetByCreatorHandler _getByCreatorHandler;

    public AnticipationService(
        CreateAnticipationHandler createHandler,
        SimulateAnticipationHandler simulateHandler,
        ApproveAnticipationHandler approveHandler,
        RejectAnticipationHandler rejectHandler,
        GetAllAnticipationsHandler getAllAnticipationsHandler,
        GetByCreatorHandler getByCreatorHandler)
    {
        _createHandler = createHandler;
        _simulateHandler = simulateHandler;
        _approveHandler = approveHandler;
        _rejectHandler = rejectHandler;
        _getAllAnticipationsHandler = getAllAnticipationsHandler;
        _getByCreatorHandler = getByCreatorHandler;
    }

    public Task<AnticipationResponse> CreateAsync(CreateAnticipationCommand command, CancellationToken cancellationToken = default)
        => _createHandler.HandleAsync(command, cancellationToken);

    public Task<AnticipationSimulationResponse> SimulateAsync(string creatorId, decimal requestedAmount, DateTime requestDate, CancellationToken cancellationToken = default)
        => _simulateHandler.HandleAsync(new SimulateAnticipationQuery(creatorId, requestedAmount, requestDate), cancellationToken);

    public Task<AnticipationResponse> ApproveAsync(ApproveAnticipationCommand command, CancellationToken cancellationToken = default)
        => _approveHandler.HandleAsync(command, cancellationToken);

    public Task<AnticipationResponse> RejectAsync(RejectAnticipationCommand command, CancellationToken cancellationToken = default)
        => _rejectHandler.HandleAsync(command, cancellationToken);

    public Task<PagedResponse<AnticipationResponse>> GetAllAsync(int page, int pageSize, CancellationToken cancellationToken = default)
        => _getAllAnticipationsHandler.HandleAsync(new GetAllAnticipationsQuery(page, pageSize), cancellationToken);

    public Task<IReadOnlyList<AnticipationResponse>> GetByCreatorAsync(string creatorId, CancellationToken cancellationToken = default)
        => _getByCreatorHandler.HandleAsync(new GetByCreatorQuery(creatorId), cancellationToken);
}