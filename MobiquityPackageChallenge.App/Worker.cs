using MediatR;
using MobiquityPackageChallenge.Application.Packer;
using Serilog;
using ILogger = Serilog.ILogger;

namespace MobiquityPackageChallenge.App
{
    public class Worker : BackgroundService
    {
        private readonly IMediator _Mediator;
        private static readonly ILogger _Logger = Log.ForContext<Worker>();

        public Worker(IMediator mediator)
        {
            _Mediator = mediator;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _Logger.Information("Pleas: {time}", DateTimeOffset.Now);
                Console.WriteLine("Please provide path to file and press enter.");
                var path = Console.ReadLine();

                try
                {
                    var file = await File.ReadAllBytesAsync(path, stoppingToken);
                    var command = new PackCommand(file);
                    var result = await _Mediator.Send(command, stoppingToken);
                    Console.WriteLine("\n\nResults:\n{0}",result);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Failed to process file.\n {0}", e.Message);
                }

                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}