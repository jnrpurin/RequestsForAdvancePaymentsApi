using Anticipation.Domain.Entities;
using Anticipation.Domain.ValueObjects;

namespace Anticipation.Domain.Services;

public sealed class AnticipationDomainService
{
    private const decimal MinimumRequestedAmount = 100.00m;

    public AnticipationRequest Create(string creatorId, decimal requestedAmount, DateTime requestDate)
    {
        if (requestedAmount <= MinimumRequestedAmount)
        {
            throw new ArgumentException("valor_solicitado deve ser maior que R$ 100,00.", nameof(requestedAmount));
        }

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