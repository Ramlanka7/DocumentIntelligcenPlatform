using AI.DocumentIntelligence.Domain.Common;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AI.DocumentIntelligence.Application.Common.Behaviors;

/// <summary>
/// MediatR pipeline behavior that logs the start and completion of every request, including whether
/// the resulting <see cref="Result"/> represented success or failure (and the failing error code).
/// </summary>
/// <typeparam name="TRequest">The request type.</typeparam>
/// <typeparam name="TResponse">The response type.</typeparam>
public sealed class LoggingBehavior<TRequest, TResponse>(
    ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    /// <inheritdoc />
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        string requestName = typeof(TRequest).Name;
        logger.LogInformation("Handling {RequestName}", requestName);

        TResponse response = await next();

        if (response is Result { IsFailure: true } failed)
        {
            logger.LogWarning(
                "{RequestName} failed with {ErrorCode}: {ErrorDescription}",
                requestName,
                failed.Error.Code,
                failed.Error.Description);
        }
        else
        {
            logger.LogInformation("Handled {RequestName}", requestName);
        }

        return response;
    }
}
