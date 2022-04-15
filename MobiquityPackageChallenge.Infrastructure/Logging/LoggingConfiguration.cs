using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Loggly;
using Loggly.Config;
using Serilog;
using Serilog.Core.Enrichers;
using Serilog.Enrichers;
using Serilog.Events;
using Serilog.Sinks.ApplicationInsights.Sinks.ApplicationInsights.TelemetryConverters;

namespace MobiquityPackageChallenge.Infrastructure.Logging
{
    public static class LoggingConfiguration
    {
        public static void ConfigureLogging(string appName, string environment, string customerToken, string instrumentationKey)
        {
            SetupLogglyConfiguration(appName, customerToken);

            var loggerConfig = new LoggerConfiguration()
                .MinimumLevel.Debug()

                //Add enrichers
                .Enrich.FromLogContext()
                .Enrich.WithProcessId()
                .Enrich.WithThreadId()
                .Enrich.With(new EnvironmentUserNameEnricher())
                .Enrich.With(new MachineNameEnricher())
                .Enrich.With(new PropertyEnricher("Environment", environment))

                //Add sinks
                .WriteTo.Async(s => s.Loggly())
                .WriteTo.Async(configure: s => s.ApplicationInsights(instrumentationKey: instrumentationKey, telemetryConverter: new EventTelemetryConverter(), restrictedToMinimumLevel: LogEventLevel.Information));

            Log.Logger = loggerConfig.CreateLogger();
        }

        static void SetupLogglyConfiguration(string appName, string customerToken)
        {
            //Configure Loggly
            var config = LogglyConfig.Instance;
            config.CustomerToken = customerToken;
            config.ApplicationName = appName;

            config.Transport = new TransportConfiguration()
            {
                EndpointHostname = "logs-01.loggly.com",
                EndpointPort = 443,
                LogTransport = LogTransport.Https
            };

            config.ThrowExceptions = true;

            //use the new Transport property that hides IP as of loggly-csharp 4.6.1.76
            config.Transport.ForwardedForIp = "0.0.0.0";

            //Define Tags sent to Loggly
            config.TagConfig.Tags.AddRange(
                new ITag[]
                {
                    new ApplicationNameTag { Formatter = "application-{0}" },
                    new HostnameTag { Formatter = "host-{0}" }
                }
            );
        }
    }
}
