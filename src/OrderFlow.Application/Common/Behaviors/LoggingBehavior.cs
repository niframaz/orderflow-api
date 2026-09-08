using MediatR;
using Microsoft.Extensions.Logging;

namespace OrderFlow.Application.Common.Behaviors
{
    public sealed class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger) : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
    {
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;

            logger.LogInformation("Handling {RequestName}", requestName);

            var stopWatch = System.Diagnostics.Stopwatch.StartNew();

            try
            {
                var response = await next(cancellationToken);
                stopWatch.Stop();
                logger.LogInformation("Handled {RequestName} in {ElapsedMs} ms", requestName, stopWatch.ElapsedMilliseconds);
                return response;

            }
            catch (Exception ex)
            {
                stopWatch.Stop();
                logger.LogError(ex, "Error handling {RequestName} after {ElapsedMs} ms", requestName, stopWatch.ElapsedMilliseconds);
                throw;
            }
        }
    }
}
