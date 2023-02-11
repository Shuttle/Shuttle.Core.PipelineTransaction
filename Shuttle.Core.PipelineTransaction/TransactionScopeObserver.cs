using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Shuttle.Core.Contract;
using Shuttle.Core.Pipelines;
using Shuttle.Core.Transactions;

namespace Shuttle.Core.PipelineTransaction
{
    public interface ITransactionScopeObserver : 
        IPipelineObserver<OnStartTransactionScope>,
        IPipelineObserver<OnCompleteTransactionScope>, 
        IPipelineObserver<OnDisposeTransactionScope>,
        IPipelineObserver<OnAbortPipeline>, 
        IPipelineObserver<OnPipelineException>
    {
    }

    public class TransactionScopeObserver : ITransactionScopeObserver
    {
        private readonly ITransactionScopeFactory _transactionScopeFactory;
        private readonly TransactionScopeOptions _transactionScopeOptions;

        public TransactionScopeObserver(IOptions<TransactionScopeOptions> transactionScopeOptions, ITransactionScopeFactory transactionScopeFactory)
        {
            Guard.AgainstNull(transactionScopeOptions, nameof(transactionScopeOptions));
            
            _transactionScopeOptions = Guard.AgainstNull(transactionScopeOptions.Value, nameof(transactionScopeOptions.Value));
            _transactionScopeFactory = Guard.AgainstNull(transactionScopeFactory, nameof(transactionScopeFactory));
        }

        public async Task Execute(OnAbortPipeline pipelineEvent)
        {
            Guard.AgainstNull(pipelineEvent, nameof(pipelineEvent));

            var state = pipelineEvent.Pipeline.State;
            var scope = state.GetTransactionScope();

            if (scope == null)
            {
                return;
            }

            if (state.GetTransactionComplete())
            {
                scope.Complete();
            }

            scope.Dispose();

            state.SetTransactionScope(null);

            await Task.CompletedTask;
        }

        public async Task Execute(OnCompleteTransactionScope pipelineEvent)
        {
            Guard.AgainstNull(pipelineEvent, nameof(pipelineEvent));

            var state = pipelineEvent.Pipeline.State;
            var scope = state.GetTransactionScope();

            if (scope == null)
            {
                return;
            }

            if (pipelineEvent.Pipeline.Exception == null || state.GetTransactionComplete())
            {
                scope.Complete();
            }

            await Task.CompletedTask;
        }

        public async Task Execute(OnDisposeTransactionScope pipelineEvent)
        {
            Guard.AgainstNull(pipelineEvent, nameof(pipelineEvent));

            var state = pipelineEvent.Pipeline.State;
            var scope = state.GetTransactionScope();

            if (scope == null)
            {
                return;
            }

            scope.Dispose();

            state.SetTransactionScope(null);

            await Task.CompletedTask;
        }

        public async Task Execute(OnPipelineException pipelineEvent)
        {
            Guard.AgainstNull(pipelineEvent, nameof(pipelineEvent));

            var state = pipelineEvent.Pipeline.State;
            var scope = state.GetTransactionScope();

            if (scope == null)
            {
                return;
            }

            if (state.GetTransactionComplete())
            {
                scope.Complete();
            }

            scope.Dispose();

            state.SetTransactionScope(null);

            await Task.CompletedTask;
        }

        public async Task Execute(OnStartTransactionScope pipelineEvent)
        {
            Guard.AgainstNull(pipelineEvent, nameof(pipelineEvent));

            var state = pipelineEvent.Pipeline.State;
            var scope = state.GetTransactionScope();

            if (scope != null)
            {
                throw new InvalidOperationException(
                    string.Format(Resources.TransactionAlreadyStartedException, GetType().FullName,
                        MethodBase.GetCurrentMethod()?.Name ?? Resources.MethodNameNotFound));
            }

            scope = _transactionScopeFactory.Create(_transactionScopeOptions.IsolationLevel, _transactionScopeOptions.Timeout);

            state.SetTransactionScope(scope);

            await Task.CompletedTask;
        }
    }
}