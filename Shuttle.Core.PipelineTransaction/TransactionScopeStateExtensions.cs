using Shuttle.Core.Pipelines;
using Shuttle.Core.Transactions;

namespace Shuttle.Core.PipelineTransaction
{
    public static class TransactionScopeStateExtensions
    {
        public static ITransactionScope GetTransactionScope(this IState state)
        {
            return state.Get<ITransactionScope>("TransactionScope");
        }

        public static void SetTransactionScope(this IState state, ITransactionScope scope)
        {
            state.Replace("TransactionScope", scope);
        }

        public static bool GetTransactionComplete(this IState state)
        {
            return state.Get<bool>("TransactionComplete");
        }

        public static void SetTransactionComplete(this IState state)
        {
            state.Replace("TransactionComplete", true);
        }
    }
}