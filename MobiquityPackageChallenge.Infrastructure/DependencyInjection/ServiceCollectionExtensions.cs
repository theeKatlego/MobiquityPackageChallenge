using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MobiquityPackageChallenge.Infrastructure.Logging;

namespace MobiquityPackageChallenge.Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static void AddInfrastructure(this IServiceCollection services)
    {
        services.AddLogging();
    }

    private static void AddLogging(this IServiceCollection services)
    {
        services.AddSingleton<LoggingContext>();
    }
}
