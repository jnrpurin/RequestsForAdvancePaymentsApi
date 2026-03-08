using Anticipation.Domain.Entities;
using Anticipation.Domain.ValueObjects;

namespace Anticipation.Domain.Services;

public sealed class AnticipationDomainService
{
    public AnticipationRequest Create(string creatorId, decimal amount, string currency)
    {
        var money = new Money(amount, currency);
        return new AnticipationRequest(creatorId, money);
    }

    public void Approve(AnticipationRequest request)
    {
        request.Approve();
    }

    public void Reject(AnticipationRequest request, string reason)
    {
        request.Reject(reason);
    }
}