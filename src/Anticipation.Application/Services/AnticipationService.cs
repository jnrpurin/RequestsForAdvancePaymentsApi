using Anticipation.Application.Commands;
using Anticipation.Application.DTOs;
using Anticipation.Application.Handlers;
using Anticipation.Application.Interfaces;
using Anticipation.Application.Queries;

namespace Anticipation.Application.Services;

public sealed class AnticipationService : IAnticipationService
{
    private readonly CreateAnticipationHandler _createHandler;
    private readonly ApproveAnticipationHandler _approveHandler;
    private readonly RejectAnticipationHandler _rejectHandler;
    private readonly GetByCreatorHandler _getByCreatorHandler;

    public AnticipationService(
        CreateAnticipationHandler createHandler,
        ApproveAnticipationHandler approveHandler,
        RejectAnticipationHandler rejectHandler,
        GetByCreatorHandler getByCreatorHandler)
    {
        _createHandler = createHandler;
        _approveHandler = approveHandler;
        _rejectHandler = rejectHandler;
        _getByCreatorHandler = getByCreatorHandler;
    }

    public Task<AnticipationResponse> CreateAsync(CreateAnticipationCommand command, CancellationToken cancellationToken = default)
        => _createHandler.HandleAsync(command, cancellationToken);

    public Task<AnticipationResponse> ApproveAsync(ApproveAnticipationCommand command, CancellationToken cancellationToken = default)
        => _approveHandler.HandleAsync(command, cancellationToken);

    public Task<AnticipationResponse> RejectAsync(RejectAnticipationCommand command, CancellationToken cancellationToken = default)
        => _rejectHandler.HandleAsync(command, cancellationToken);

    public Task<IReadOnlyList<AnticipationResponse>> GetByCreatorAsync(string creatorId, CancellationToken cancellationToken = default)
        => _getByCreatorHandler.HandleAsync(new GetByCreatorQuery(creatorId), cancellationToken);
}