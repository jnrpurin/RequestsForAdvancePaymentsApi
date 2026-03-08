using Anticipation.Application.Commands;
using Anticipation.Application.Handlers;
using Anticipation.Domain.Entities;
using Anticipation.Domain.Repositories;
using Anticipation.Domain.Services;
using Microsoft.Extensions.Logging;

namespace Anticipation.Tests.Application.Tests;

public class CreateAnticipationHandlerTests
{
    [Fact]
    public async Task HandleAsync_Should_Throw_When_Creator_Has_Pending_Request()
    {
        var repository = new FakeAnticipationRepository { HasPending = true };
        var handler = new CreateAnticipationHandler(
            repository,
            new AnticipationDomainService(),
            LoggerFactory.Create(builder => { }).CreateLogger<CreateAnticipationHandler>());

        var command = new CreateAnticipationCommand("creator-1", 1500m, DateTime.UtcNow);

        await Assert.ThrowsAsync<InvalidOperationException>(() => handler.HandleAsync(command));
    }

    [Fact]
    public async Task HandleAsync_Should_Create_Request_When_Creator_Has_No_Pending()
    {
        var repository = new FakeAnticipationRepository { HasPending = false };
        var handler = new CreateAnticipationHandler(
            repository,
            new AnticipationDomainService(),
            LoggerFactory.Create(builder => { }).CreateLogger<CreateAnticipationHandler>());

        var command = new CreateAnticipationCommand("creator-1", 1500m, DateTime.UtcNow);
        var response = await handler.HandleAsync(command);

        Assert.Equal(1, repository.AddedCount);
        Assert.Equal(1, repository.SavedCount);
        Assert.Equal("creator-1", response.creator_id);
        Assert.Equal("pendentef", response.status);
    }

    private sealed class FakeAnticipationRepository : IAnticipationRepository
    {
        public bool HasPending { get; init; }
        public int AddedCount { get; private set; }
        public int SavedCount { get; private set; }

        public Task AddAsync(AnticipationRequest anticipation, CancellationToken cancellationToken = default)
        {
            AddedCount++;
            return Task.CompletedTask;
        }

        public Task<AnticipationRequest?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
            => Task.FromResult<AnticipationRequest?>(null);

        public Task<bool> HasPendingByCreatorAsync(string creatorId, CancellationToken cancellationToken = default)
            => Task.FromResult(HasPending);

        public Task<(IReadOnlyList<AnticipationRequest> Items, int TotalCount)> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default)
            => Task.FromResult(((IReadOnlyList<AnticipationRequest>)new List<AnticipationRequest>(), 0));

        public Task<IReadOnlyList<AnticipationRequest>> GetByCreatorAsync(string creatorId, CancellationToken cancellationToken = default)
            => Task.FromResult((IReadOnlyList<AnticipationRequest>)new List<AnticipationRequest>());

        public Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            SavedCount++;
            return Task.CompletedTask;
        }
    }
}