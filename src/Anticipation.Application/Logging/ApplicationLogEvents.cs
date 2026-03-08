using Microsoft.Extensions.Logging;

namespace Anticipation.Application.Logging;

public static class ApplicationLogEvents
{
    public static readonly EventId CreateStarted = new(1000, nameof(CreateStarted));
    public static readonly EventId CreateDeniedPending = new(1001, nameof(CreateDeniedPending));
    public static readonly EventId CreateSucceeded = new(1002, nameof(CreateSucceeded));

    public static readonly EventId ApproveStarted = new(1100, nameof(ApproveStarted));
    public static readonly EventId ApproveSucceeded = new(1101, nameof(ApproveSucceeded));

    public static readonly EventId RejectStarted = new(1200, nameof(RejectStarted));
    public static readonly EventId RejectSucceeded = new(1201, nameof(RejectSucceeded));

    public static readonly EventId ListPaged = new(1300, nameof(ListPaged));
    public static readonly EventId Simulate = new(1400, nameof(Simulate));
}