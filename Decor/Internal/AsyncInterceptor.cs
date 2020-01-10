using Castle.DynamicProxy;
using System.Threading.Tasks;

namespace Decor.Internal
{
    public abstract class AsyncInterceptor : IAsyncInterceptor
    {
        public void InterceptSynchronous(IInvocation invocation)
        {
            var callInfo = new CallInfo(invocation);

            BeforeInvocation(callInfo).Wait();

            invocation.Proceed();

            AfterInvocation(callInfo).Wait();
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
            var callInfo = new CallInfo(invocation);

            await BeforeInvocation(callInfo);

            proceed.Invoke();

            await ((Task)invocation.ReturnValue).ConfigureAwait(false);

            await AfterInvocation(callInfo);
        }

        private async Task<TResult> WrapInvocationInTaskWithResult<TResult>(IInvocation invocation)
        {
            var proceed = invocation.CaptureProceedInfo();
            var callInfo = new CallInfo(invocation);

            await BeforeInvocation(callInfo);

            proceed.Invoke();

            TResult result = await ((Task<TResult>)invocation.ReturnValue).ConfigureAwait(false);

            await AfterInvocation(callInfo);

            return result;
        }

        protected virtual Task BeforeInvocation(CallInfo callInfo) => Task.CompletedTask;

        protected virtual Task AfterInvocation(CallInfo callInfo) => Task.CompletedTask;
    }
}
