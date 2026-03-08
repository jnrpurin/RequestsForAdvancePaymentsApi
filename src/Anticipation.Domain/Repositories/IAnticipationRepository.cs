using Anticipation.Domain.Entities;

namespace Anticipation.Domain.Repositories;

public interface IAnticipationRepository
{
    Task AddAsync(AnticipationRequest anticipation, CancellationToken cancellationToken = default);
    Task<AnticipationRequest?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<AnticipationRequest>> GetByCreatorAsync(string creatorId, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}