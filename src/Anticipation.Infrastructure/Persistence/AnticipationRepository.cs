using Anticipation.Domain.Entities;
using Anticipation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Anticipation.Infrastructure.Persistence;

public sealed class AnticipationRepository : IAnticipationRepository
{
    private readonly AppDbContext _dbContext;

    public AnticipationRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(AnticipationRequest anticipation, CancellationToken cancellationToken = default)
    {
        await _dbContext.AnticipationRequests.AddAsync(anticipation, cancellationToken);
    }

    public Task<AnticipationRequest?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _dbContext.AnticipationRequests.FirstOrDefaultAsync(request => request.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<AnticipationRequest>> GetByCreatorAsync(string creatorId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.AnticipationRequests
            .AsNoTracking()
            .Where(request => request.CreatorId == creatorId)
            .OrderByDescending(request => request.RequestDate)
            .ToListAsync(cancellationToken);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }
}