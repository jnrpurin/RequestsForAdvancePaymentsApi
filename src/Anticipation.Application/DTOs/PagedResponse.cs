namespace Anticipation.Application.DTOs;

public sealed record PagedResponse<T>(
    int page,
    int pageSize,
    int totalItems,
    int totalPages,
    IReadOnlyList<T> items);