using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Serilog;
using Serilog.Context;
using Serilog.Events;

namespace MobiquityPackageChallenge.Infrastructure.Logging
{
    public class LoggingPipelineStep<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly Type[] _WarningExceptions = new[]
        {
            typeof(ValidationException),
        };

        private readonly LoggingContext _LoggingContext;

        public LoggingPipelineStep(LoggingContext loggingContext)
        {
            _LoggingContext = loggingContext;
        }
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            // Set the query name on LoggingContext
            _LoggingContext.SetCommandOrQuery(request);

            // Add properties to all log events
            using (LogContext.PushProperty("CorrelationId", _LoggingContext.CorrelationId))
                using (LogContext.PushProperty("Command", _LoggingContext.CommandOrQuery))
                {
                    var sw = Stopwatch.StartNew();

                    try
                    {
                        var result = await next();

                        sw.Stop();
                        Log.Information("Handled request {Command} - {RequestDuration} ms", _LoggingContext.CommandOrQuery, sw.ElapsedMilliseconds);
                        return result;
                    }
                    catch (Exception e)
                    {
                        sw.Stop();
                        var level = _WarningExceptions.Contains(e.GetType())
                            ? LogEventLevel.Warning
                            : LogEventLevel.Error;
                        Log.Write(level, e, "Error handling request {Command} - {RequestDuration} ms: " + e.Message, _LoggingContext.CommandOrQuery, sw.ElapsedMilliseconds);
                        throw;
                    }
                }
        }
    }
}
