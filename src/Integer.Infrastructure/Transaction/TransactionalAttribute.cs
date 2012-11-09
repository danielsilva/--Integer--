using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using PostSharp.Aspects;

namespace Integer.Infrastructure.Transaction
{
    [Serializable]
    public class TransactionalAttribute : OnMethodBoundaryAspect
    {
        private readonly TransactionScopeOption option;

        public TransactionalAttribute()
        {
        }

        public TransactionalAttribute(TransactionScopeOption option)
        {
            this.option = option;
        }

        public override void OnEntry(MethodExecutionArgs args)
        {
            args.MethodExecutionTag = new TransactionScope(this.option);
        }

        public override void OnExit(MethodExecutionArgs args)
        {
            ((TransactionScope)args.MethodExecutionTag).Dispose();
        }

        public override void OnSuccess(MethodExecutionArgs args)
        {
            ((TransactionScope)args.MethodExecutionTag).Complete();
        }
    }
}
