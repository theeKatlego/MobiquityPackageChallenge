using MobiquityPackageChallenge.App;
using MobiquityPackageChallenge.Application.Packer;
using MobiquityPackageChallenge.Infrastructure.DependencyInjection;
using MobiquityPackageChallenge.Infrastructure.Logging;
using Serilog;

var _AppName = "MobiquityPackageChallenge";
string EnvironmentName = null;
string LogglyCustomerToken = null;
string InstrumentationKey = null;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration(
        configBuilder =>
        {
            var configuration = configBuilder.AddEnvironmentVariables().Build();

            EnvironmentName = configuration["EnvironmentName"];
            LogglyCustomerToken = configuration["LogglyCustomerToken"];
            InstrumentationKey = configuration["APPINSIGHTS_INSTRUMENTATIONKEY"];
            
            LoggingConfiguration.ConfigureLogging(
                 appName: _AppName,
                 environment: EnvironmentName,
                 customerToken: LogglyCustomerToken,
                 instrumentationKey: InstrumentationKey
             );
        })
    .ConfigureServices(services =>
    {
        services.AddApplication(typeof(PackCommand).Assembly);
        services.AddInfrastructure();
        
        services.AddHostedService<Worker>();
    })
    .UseSerilog()
    .Build();

await host.RunAsync();
