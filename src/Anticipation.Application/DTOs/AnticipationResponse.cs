using Anticipation.Domain.Enums;

namespace Anticipation.Application.DTOs;

public sealed record AnticipationResponse(
    Guid Id,
    string CreatorId,
    decimal Amount,
    string Currency,
    RequestStatus Status,
    DateTime CreatedAtUtc,
    DateTime? DecidedAtUtc,
    string? RejectionReason);