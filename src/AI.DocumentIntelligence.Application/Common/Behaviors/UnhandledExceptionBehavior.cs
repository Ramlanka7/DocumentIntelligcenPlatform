using AI.DocumentIntelligence.Domain.Common;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AI.DocumentIntelligence.Application.Common.Behaviors;

/// <summary>
/// MediatR pipeline behavior that converts an unexpected exception thrown while handling a request
/// into a failed <see cref="Result"/>, keeping the Result pattern intact even for unforeseen errors.
/// The exception is logged; expected failures should still be modelled as <see cref="Error"/> values.
/// </summary>
/// <typeparam name="TRequest">The request type.</typeparam>
/// <typeparam name="TResponse">The response type, constrained to <see cref="Result"/>.</typeparam>
public sealed class UnhandledExceptionBehavior<TRequest, TResponse>(
    ILogger<UnhandledExceptionBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
    where TResponse : Result
{
    /// <inheritdoc />
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        try
        {
            return await next();
        }
        catch (Exception exception)
        {
            string requestName = typeof(TRequest).Name;
            logger.LogError(exception, "Unhandled exception while processing {RequestName}", requestName);

            var error = Error.Failure(
                "General.Unhandled",
                "An unexpected error occurred while processing the request.");

            return ResultFactory.Failure<TResponse>(error);
        }
    }
}
