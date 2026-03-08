using Anticipation.Application.Commands;
using Anticipation.Application.DTOs;

namespace Anticipation.Application.Interfaces;

public interface IAnticipationService
{
    Task<AnticipationResponse> CreateAsync(CreateAnticipationCommand command, CancellationToken cancellationToken = default);
    Task<AnticipationResponse> ApproveAsync(ApproveAnticipationCommand command, CancellationToken cancellationToken = default);
    Task<AnticipationResponse> RejectAsync(RejectAnticipationCommand command, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<AnticipationResponse>> GetByCreatorAsync(string creatorId, CancellationToken cancellationToken = default);
}