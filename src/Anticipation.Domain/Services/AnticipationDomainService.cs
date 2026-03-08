using Anticipation.Domain.Entities;
using Anticipation.Domain.ValueObjects;

namespace Anticipation.Domain.Services;

public sealed class AnticipationDomainService
{
    public AnticipationRequest Create(string creatorId, decimal requestedAmount, DateTime requestDate)
    {
        var money = new Money(requestedAmount, "BRL");
        return new AnticipationRequest(creatorId, money, requestDate);
    }

    public void Approve(AnticipationRequest request)
    {
        request.Approve();
    }

    public void Reject(AnticipationRequest request)
    {
        request.Reject();
    }
}