using Microsoft.Extensions.Logging;

namespace Anticipation.API.Logging;

public static class ApiLogEvents
{
    public static readonly EventId HandledClientException = new(2000, nameof(HandledClientException));
    public static readonly EventId UnhandledServerException = new(2001, nameof(UnhandledServerException));
}