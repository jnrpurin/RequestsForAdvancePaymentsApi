namespace Anticipation.Application.Commands;

public sealed record CreateAnticipationCommand(
    string CreatorId,
    decimal Amount,
    string Currency);