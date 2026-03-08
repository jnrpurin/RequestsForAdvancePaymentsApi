namespace Anticipation.Application.Commands;

public sealed record CreateAnticipationCommand(
    string CreatorId,
    decimal RequestedAmount,
    DateTime RequestDate);