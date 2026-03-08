namespace Anticipation.API.Contracts;

public sealed record CreateAnticipationRequest(
    string CreatorId,
    decimal Amount,
    string Currency);