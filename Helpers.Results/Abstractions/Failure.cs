using Microsoft.Extensions.Logging;

namespace Helpers.Results.Abstractions;

public record Failure : IResultType
{
    public EventId EventId { get; }
    public string Message { get; }
    public object[] MessageParams { get; }
    public Exception? Exception { get; }

    private Failure(EventId eventId, Exception? exception, string message, params object[] messageParams)
    {
        Message = message;
        MessageParams = messageParams;
        EventId = eventId;
        Exception = exception;
    }

    public static Failure Create(EventId eventId, string message, params object[] messageParams)
        => new(eventId, null, message, messageParams);

    public static Failure Create(EventId eventId, Exception? exception, string message, params object[] messageParams)
        => new(eventId, exception, message, messageParams);

}
