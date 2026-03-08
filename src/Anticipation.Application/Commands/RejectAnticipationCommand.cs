namespace Anticipation.Application.Commands;

public sealed record RejectAnticipationCommand(
    Guid Id,
    string Reason);