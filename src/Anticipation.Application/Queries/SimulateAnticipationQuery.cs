namespace Anticipation.Application.Queries;

public sealed record SimulateAnticipationQuery(
    string CreatorId,
    decimal RequestedAmount,
    DateTime RequestDate);