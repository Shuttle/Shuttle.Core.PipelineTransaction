using System;
using Microsoft.Extensions.DependencyInjection;
using Shuttle.Core.Contract;
using Shuttle.Core.Pipelines;

namespace Shuttle.Core.PipelineTransaction
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPipelineTransaction(this IServiceCollection services)
        {
            Guard.AgainstNull(services, nameof(services));

            services.AddPipelineObservers(typeof(TransactionScopeObserver).Assembly);

            return services;
        }
    }
}