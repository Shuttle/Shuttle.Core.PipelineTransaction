using System;
using System.Reflection;
using Shuttle.Core.Contract;
using Shuttle.Core.Pipelines;
using Shuttle.Core.Transactions;

namespace Shuttle.Core.PipelineTransaction
{
    public class TransactionScopeObserver :
        IPipelineObserver<OnStartTransactionScope>,
        IPipelineObserver<OnCompleteTransactionScope>,
        IPipelineObserver<OnDisposeTransactionScope>,
        IPipelineObserver<OnAbortPipeline>,
        IPipelineObserver<OnPipelineException>
    {
        private readonly ITransactionScopeFactory _transactionScopeFactory;

        public TransactionScopeObserver(ITransactionScopeFactory transactionScopeFactory)
        {
            Guard.AgainstNull(transactionScopeFactory, nameof(transactionScopeFactory));

            _transactionScopeFactory = transactionScopeFactory;
        }

        public void Execute(OnAbortPipeline pipelineEvent)
        {
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
        }

        public void Execute(OnCompleteTransactionScope pipelineEvent)
        {
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
        }

        public void Execute(OnDisposeTransactionScope pipelineEvent)
        {
            var state = pipelineEvent.Pipeline.State;
            var scope = state.GetTransactionScope();

            if (scope == null)
            {
                return;
            }

            scope.Dispose();

            state.SetTransactionScope(null);
        }

        public void Execute(OnPipelineException pipelineEvent)
        {
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
        }

        public void Execute(OnStartTransactionScope pipelineEvent)
        {
            var state = pipelineEvent.Pipeline.State;
            var scope = state.GetTransactionScope();

            if (scope != null)
            {
                throw new InvalidOperationException(
                    string.Format(Resources.TransactionAlreadyStartedException, GetType().FullName,
                        MethodBase.GetCurrentMethod().Name));
            }

            scope = _transactionScopeFactory.Create();

            state.SetTransactionScope(scope);
        }
    }
}