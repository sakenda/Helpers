using Microsoft.Extensions.Logging;

namespace Helpers.Results.Abstractions;

public record Success : IResultType
{
    public EventId EventId { get; } = new();

    private Success(EventId? eventId = null) => EventId = eventId ?? new EventId();

    public static Success Create(EventId? eventId = null) => new(eventId);

}

public record Success<T> : IResultType<T>
{
    public T? Value { get; }
    public EventId EventId { get; } = new();

    private Success(T? value = default, EventId? eventId = default)
    {
        Value = value;
        EventId = eventId ?? new EventId();
    }

    public static Success<T> Create(T? value = default, EventId? eventId = null) => new(value, eventId);

}
