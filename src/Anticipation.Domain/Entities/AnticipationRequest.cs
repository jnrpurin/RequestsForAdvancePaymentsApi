using Anticipation.Domain.Enums;
using Anticipation.Domain.ValueObjects;

namespace Anticipation.Domain.Entities;

public sealed class AnticipationRequest
{
    public Guid Id { get; private set; }
    public string CreatorId { get; private set; } = string.Empty;
    public Money Amount { get; private set; } = null!;
    public RequestStatus Status { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime? DecidedAtUtc { get; private set; }
    public string? RejectionReason { get; private set; }

    private AnticipationRequest()
    {
    }

    public AnticipationRequest(string creatorId, Money amount)
    {
        if (string.IsNullOrWhiteSpace(creatorId))
        {
            throw new ArgumentException("CreatorId is required.", nameof(creatorId));
        }

        Id = Guid.NewGuid();
        CreatorId = creatorId.Trim();
        Amount = amount;
        Status = RequestStatus.Pending;
        CreatedAtUtc = DateTime.UtcNow;
    }

    public void Approve()
    {
        EnsurePending();
        Status = RequestStatus.Approved;
        DecidedAtUtc = DateTime.UtcNow;
        RejectionReason = null;
    }

    public void Reject(string reason)
    {
        EnsurePending();

        if (string.IsNullOrWhiteSpace(reason))
        {
            throw new ArgumentException("Rejection reason is required.", nameof(reason));
        }

        Status = RequestStatus.Rejected;
        DecidedAtUtc = DateTime.UtcNow;
        RejectionReason = reason.Trim();
    }

    private void EnsurePending()
    {
        if (Status != RequestStatus.Pending)
        {
            throw new InvalidOperationException("Only pending requests can be decided.");
        }
    }
}