namespace Helpers.Results.Abstractions;

public interface IResultType
{
}

public interface IResultType<out T> : IResultType
{
    public T? Value { get; }

}