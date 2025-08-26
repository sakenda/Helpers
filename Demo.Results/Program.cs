using Helpers.Results.Abstractions;
using Microsoft.Extensions.Logging;
using Helpers.Results.Extensions;
using Microsoft.Extensions.Logging.Console;

namespace Demo.Results;

internal class Program
{
    public static ILogger<Program> Logger { get; set; } = default!;

    private static Failure CreateFailure(int id, string title, Exception? exception, string message, params object[] values)
        => Failure.Create(new EventId(id, title), exception, message, values);

    private static Result GetResult(bool isSuccess, Failure failure)
        => isSuccess ? Result.Success() : Result.Failure(failure);

    private static Result<int> GetIntegerResult(int value, Failure failure) 
        => value > 50 ? Result<int>.Success(value) : Result<int>.Failure(failure);

    private static IEnumerable<Result<Person>> GetPersonResult()
    {
        var persons = Person.CreateDemoPersons();

        foreach (var person in persons)
        {
            if (person.Email.Contains("othercompany.com"))
            {
                yield return Result<Person>.Failure(
                    CreateFailure(300, "Person Validation", null, "Person {Name} is not valid. Persons from 'othercompany' are not allowed. Email: {Email}", person.Name, person.Email));
                continue;
            }

            yield return Result<Person>.Success(person, new EventId(100, "Person Validation"));
        }
    }

    static void Main(string[] args)
    {
        Logger = LoggerFactory
            .Create(builder =>
            {
                builder.AddConsole();
                builder.SetMinimumLevel(LogLevel.Information);
                builder.AddSimpleConsole(options =>
                {
                    options.IncludeScopes = true;
                    options.SingleLine = true;
                    options.TimestampFormat = "HH:mm:ss ";
                    options.ColorBehavior = LoggerColorBehavior.Enabled;
                });
            })
            .CreateLogger<Program>();

        TestSimpleResult(true);
        TestSimpleResult(false);
        TestGenericResult(99);
        TestGenericResult(2);
        TestPersonResult();
    }

    private static void TestSimpleResult(bool isSuccess)
    {
        Logger.LogInformation("=== Starting simple result validation... ===");

        var failure = CreateFailure(1001, "Test Failure", null, "An error occurred while processing the request.");
        var result = GetResult(isSuccess, failure);

        if (result.IsFailure)
        {
            result.ToLog(Logger, LogLevel.Error);
            return;
        }

        result.ToLog(Logger, LogLevel.Information);
    }

    private static void TestGenericResult(int value)
    {
        Logger.LogInformation("=== Starting integer validation for value: {Value} ===", value);

        var failure = CreateFailure(1002, "Test Failure", null, "Value must be greater than 50. Value is {Value}", value);
        var result = GetIntegerResult(value, failure);
        
        if (result.IsFailure)
        {
            result.ToLog(Logger, LogLevel.Error);
            return;
        }

        result.ToLog(Logger, LogLevel.Information);
    }

    private static void TestPersonResult()
    {
        Logger.LogInformation("=== Starting person validation... ===");

        var personsResult = GetPersonResult();

        foreach (var result in personsResult)
        {
            if (result.IsFailure)
            {
                result.ToLog(Logger, LogLevel.Error);
                continue;
            }

            result.ToLog(Logger, LogLevel.Information);
        }
    }

}
