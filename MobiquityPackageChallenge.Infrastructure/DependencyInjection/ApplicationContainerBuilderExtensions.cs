using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.DependencyInjection;
using MobiquityPackageChallenge.Application.Packer;

namespace MaxDrive.Infrastructure.DependencyInjection
{
    public static class ApplicationContainerBuilderExtensions
    {
        public static void AddApplication(this IServiceCollection services, Assembly applicationAssembly)
        {
            services.AddValidatorsFromAssembly(applicationAssembly);
            services.AddMediatR(applicationAssembly);

            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(RequestPreProcessorBehavior<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(RequestPostProcessorBehavior<,>));
            AddApplicationServices(services);
        }
        private static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IExtractor, Extractor>();
        }
    }
}
