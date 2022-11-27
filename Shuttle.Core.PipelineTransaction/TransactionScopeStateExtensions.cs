using Shuttle.Core.Contract;
using Shuttle.Core.Pipelines;
using Shuttle.Core.Transactions;

namespace Shuttle.Core.PipelineTransaction
{
    public static class TransactionScopeStateExtensions
    {
        public static ITransactionScope GetTransactionScope(this IState state)
        {
            return Guard.AgainstNull(state, nameof(state)).Get<ITransactionScope>("TransactionScope");
        }

        public static void SetTransactionScope(this IState state, ITransactionScope scope)
        {
            Guard.AgainstNull(state, nameof(state)).Replace("TransactionScope", Guard.AgainstNull(scope, nameof(scope)));
        }

        public static bool GetTransactionComplete(this IState state)
        {
            return Guard.AgainstNull(state, nameof(state)).Get<bool>("TransactionComplete");
        }

        public static void SetTransactionComplete(this IState state)
        {
            Guard.AgainstNull(state, nameof(state)).Replace("TransactionComplete", true);
        }
    }
}