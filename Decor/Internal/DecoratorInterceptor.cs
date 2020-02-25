using Castle.DynamicProxy;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Threading.Tasks;

namespace Decor.Internal
{
    internal class DecoratorInterceptor : IAsyncInterceptor
    {
        public DecoratorInterceptor(ReadOnlyDictionary<MethodInfo, IDecorator[]> methodDecoratorMap)
        {
            MethodDecoratorMap = methodDecoratorMap;
        }

        public ReadOnlyDictionary<MethodInfo, IDecorator[]> MethodDecoratorMap { get; }

        public void InterceptSynchronous(IInvocation invocation)
        {
            var targetMethod = invocation.MethodInvocationTarget.IsGenericMethod
                ? invocation.MethodInvocationTarget.GetGenericMethodDefinition()
                : invocation.MethodInvocationTarget;

            if (!MethodDecoratorMap.TryGetValue(targetMethod, out var decorators)
                || decorators == null || decorators.Length == 0)
            {
                invocation.Proceed();

                return; // Method is not decorated.
            }

            var call = new Call(invocation, decorators);

            decorators[0].OnInvoke(call).Wait();
        }

        public void InterceptAsynchronous(IInvocation invocation)
        {
            var targetMethod = invocation.MethodInvocationTarget.IsGenericMethod
                ? invocation.MethodInvocationTarget.GetGenericMethodDefinition()
                : invocation.MethodInvocationTarget;

            if (MethodDecoratorMap.TryGetValue(targetMethod, out var decorators)
                || decorators == null || decorators.Length == 0)
            {
                var decoratedTask = WrapInvocationInTask(invocation, decorators);

                if (!decoratedTask.IsCompleted)
                {
                    invocation.ReturnValue = decoratedTask;
                }
            }
            else
            {
                invocation.Proceed();
            }
        }

        public void InterceptAsynchronous<TResult>(IInvocation invocation)
        {
            var targetMethod = invocation.MethodInvocationTarget.IsGenericMethod
                ? invocation.MethodInvocationTarget.GetGenericMethodDefinition()
                : invocation.MethodInvocationTarget;

            if (MethodDecoratorMap.TryGetValue(targetMethod, out var decorators)
                || decorators == null || decorators.Length == 0)
            {
                invocation.ReturnValue = WrapInvocationInTaskWithResult<TResult>(invocation, decorators);
            }
            else
            {
                invocation.Proceed();
            }
        }

        private async Task WrapInvocationInTask(IInvocation invocation, IDecorator[] decorators)
        {
            var call = new Call(invocation, decorators);

            await decorators[0].OnInvoke(call).ConfigureAwait(false);
        }

        private async Task<TResult> WrapInvocationInTaskWithResult<TResult>(IInvocation invocation, IDecorator[] decorators)
        {
            var call = new Call(invocation, decorators);

            await decorators[0].OnInvoke(call).ConfigureAwait(false);

            return (TResult)invocation.ReturnValue;
        }
    }
}
