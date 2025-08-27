using Microsoft.Extensions.Logging;

namespace Helpers.Results.Abstractions;

public readonly record struct Result
{
    public object? Value { get; }
    public bool IsSuccess => Value is Success;
    public bool IsFailure => Value is Failure;

    private Result(Success success) => Value = success;
    private Result(Failure failure) => Value = failure;

    public Success? GetSuccessOrDefault() => IsSuccess ? (Success)Value! : null;
    public Failure? GetFailureOrDefault() => IsFailure ? (Failure)Value! : null;

    public override string ToString() =>  IsSuccess ? "Success" : $"Failure: {GetFailureOrDefault()?.Message}";

    public static Result Success(EventId? eventId = null) => new Result(Abstractions.Success.Create(eventId));
    public static Result Failure(Failure failure) => new Result(failure);

}

public readonly record struct Result<T>
{
    public object? Value { get; }
    public bool IsSuccess => Value is Success<T?>;
    public bool HasValue => IsSuccess && ((Success<T?>)Value!).Value is not null;
    public bool IsFailure => Value is Failure;

    private Result(Success<T?> success) => Value = success;
    private Result(Failure failure) => Value = failure;

    public T? GetValueOrDefault() => IsSuccess ? ((Success<T?>)Value!).Value : default;

    public Success<T?>? GetSuccessOrDefault() => IsSuccess ? (Success<T?>)Value! : null;
    public Failure? GetFailureOrDefault() => IsFailure ? (Failure)Value! : null;

    public override string ToString() => IsSuccess ? $"Success: {GetValueOrDefault()}" : $"Failure: {GetFailureOrDefault()?.Message}";

    public static Result<T> Success(T? value = default, EventId? eventId = null) => new Result<T>(Success<T?>.Create(value, eventId));
    public static Result<T> Failure(Failure failure) => new Result<T>(failure);

}
