using Anticipation.Domain.Enums;
using Anticipation.Domain.ValueObjects;

namespace Anticipation.Domain.Entities;

public sealed class AnticipationRequest
{
    private const decimal FeeRate = 0.05m;

    public Guid Id { get; private set; }
    public string CreatorId { get; private set; } = string.Empty;
    public Money RequestedAmount { get; private set; } = null!;
    public RequestStatus Status { get; private set; }
    public DateTime RequestDate { get; private set; }
    public DateTime? DecidedAtUtc { get; private set; }
    public decimal NetAmount => decimal.Round(RequestedAmount.Amount * (1 - FeeRate), 2, MidpointRounding.AwayFromZero);

    private AnticipationRequest()
    {
    }

    public AnticipationRequest(string creatorId, Money requestedAmount, DateTime requestDate)
    {
        if (string.IsNullOrWhiteSpace(creatorId))
        {
            throw new ArgumentException("CreatorId is required.", nameof(creatorId));
        }

        if (requestDate == default)
        {
            throw new ArgumentException("Request date is required.", nameof(requestDate));
        }

        Id = Guid.NewGuid();
        CreatorId = creatorId.Trim();
        RequestedAmount = requestedAmount;
        RequestDate = requestDate;
        Status = RequestStatus.Pending;
    }

    public void Approve()
    {
        EnsurePending();
        Status = RequestStatus.Approved;
        DecidedAtUtc = DateTime.UtcNow;
    }

    public void Reject()
    {
        EnsurePending();

        Status = RequestStatus.Rejected;
        DecidedAtUtc = DateTime.UtcNow;
    }

    private void EnsurePending()
    {
        if (Status != RequestStatus.Pending)
        {
            throw new InvalidOperationException("Only pending requests can be decided.");
        }
    }
}