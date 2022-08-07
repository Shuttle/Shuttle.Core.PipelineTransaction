using Shuttle.Core.Contract;
using Shuttle.Core.Pipelines;

namespace Shuttle.Core.PipelineTransaction
{
    public static class PipelineProcessingBuilderExtensions
    {
        public static PipelineProcessingBuilder AddTransactions(this PipelineProcessingBuilder builder)
        {
            Guard.AgainstNull(builder, nameof(builder));

            builder.AddAssembly(typeof(TransactionScopeObserver).Assembly);

            return builder;
        }
    }
}