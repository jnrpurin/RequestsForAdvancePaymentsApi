using Anticipation.Domain.Entities;
using Anticipation.Domain.Enums;
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

    public Task<bool> HasPendingByCreatorAsync(string creatorId, CancellationToken cancellationToken = default)
    {
        return _dbContext.AnticipationRequests
            .AsNoTracking()
            .AnyAsync(
                request => request.CreatorId == creatorId && request.Status == RequestStatus.Pending,
                cancellationToken);
    }

    public async Task<(IReadOnlyList<AnticipationRequest> Items, int TotalCount)> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = _dbContext.AnticipationRequests
            .AsNoTracking()
            .OrderByDescending(request => request.RequestDate);

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
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