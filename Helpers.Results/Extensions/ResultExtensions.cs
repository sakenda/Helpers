using Helpers.Results.Abstractions;
using Microsoft.Extensions.Logging;
using Helpers.Exceptions;

namespace Helpers.Results.Extensions;

public static class ResultExtensions
{
    public static void ToLog(this Result result, ILogger logger, LogLevel logLevel = LogLevel.Information)
    {
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentEnumException.ThrowIfOutOfRange<LogLevel>(logLevel);

        if (result.IsFailure)
        {
            var failure = result.GetFailureOrDefault();
            if (failure is null)
            {
                logger.Log(logLevel, "Failure: null");
                return;
            }

            logger.Log(logLevel, failure.EventId, failure.Exception, failure.Message, failure.MessageParams);
            return;
        }

        var successValue = result.GetSuccessOrDefault();
        if (successValue is null)
        {
            logger.Log(logLevel, "Success: null");
            return;
        }

        logger.Log(logLevel, successValue.EventId, result.ToString());
    }

    public static void ToLog<T>(this Result<T> result, ILogger logger, LogLevel logLevel = LogLevel.Information)
    {
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentEnumException.ThrowIfOutOfRange<LogLevel>(logLevel);

        if (result.IsFailure)
        {
            var failure = result.GetFailureOrDefault();
            if (failure is null)
            {
                logger.Log(logLevel, "Failure: null");
                return;
            }

            logger.Log(logLevel, failure.EventId, failure.Exception, failure.Message, failure.MessageParams);
            return;
        }

        var resultValue = result.GetValueOrDefault();
        if (resultValue is null)
        {
            logger.Log(logLevel, "Success: null");
            return;
        }

        var successValue = result.GetSuccessOrDefault();
        if (successValue is null)
        {
            logger.Log(logLevel, "Success: null");
            return;
        }

        logger.Log(logLevel, successValue.EventId, result.ToString());
    }

}
