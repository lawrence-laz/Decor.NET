using Castle.DynamicProxy;
using System.Threading.Tasks;

namespace Decor.Internal
{
    public abstract class AsyncInterceptor : IAsyncInterceptor
    {
        public void InterceptSynchronous(IInvocation invocation)
        {
            BeforeInvocation(invocation).Wait();

            invocation.Proceed();

            AfterInvocation(invocation).Wait();
        }

        public void InterceptAsynchronous(IInvocation invocation)
        {
            invocation.ReturnValue = WrapInvocationInTask(invocation);
        }

        public void InterceptAsynchronous<TResult>(IInvocation invocation)
        {
            invocation.ReturnValue = WrapInvocationInTaskWithResult<TResult>(invocation);
        }

        private async Task WrapInvocationInTask(IInvocation invocation)
        {
            var proceed = invocation.CaptureProceedInfo();

            await BeforeInvocation(invocation);

            proceed.Invoke();

            await ((Task)invocation.ReturnValue).ConfigureAwait(false);

            await AfterInvocation(invocation);
        }

        private async Task<TResult> WrapInvocationInTaskWithResult<TResult>(IInvocation invocation)
        {
            var proceed = invocation.CaptureProceedInfo();

            await BeforeInvocation(invocation);

            proceed.Invoke();

            TResult result = await ((Task<TResult>)invocation.ReturnValue).ConfigureAwait(false);

            await AfterInvocation(invocation);

            return result;
        }

        protected virtual Task BeforeInvocation(IInvocation invocation) => Task.CompletedTask;

        protected virtual Task AfterInvocation(IInvocation invocation) => Task.CompletedTask;
    }
}
