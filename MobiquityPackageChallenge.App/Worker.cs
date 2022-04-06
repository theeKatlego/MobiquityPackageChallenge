using Serilog;
using ILogger = Serilog.ILogger;

namespace MobiquityPackageChallenge.App
{
    public class Worker : BackgroundService
    {
        private static readonly ILogger _Logger = Log.ForContext<Worker>();

        public Worker() { }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _Logger.Information("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}