using Castle.DynamicProxy;
using System;
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

            try
            {
                decorators[0].OnInvoke(call).Wait();
            }
            catch (AggregateException e) when (e.InnerException != null)
            {
                e.InnerException.Rethrow();
            }

            invocation.ReturnValue = call.ReturnValue;
        }

        public void InterceptAsynchronous(IInvocation invocation)
        {
            var targetMethod = invocation.MethodInvocationTarget.IsGenericMethod
                ? invocation.MethodInvocationTarget.GetGenericMethodDefinition()
                : invocation.MethodInvocationTarget;

            if (!MethodDecoratorMap.TryGetValue(targetMethod, out var decorators)
                || decorators == null || decorators.Length == 0)
            {
                // Method is not decorated.
                invocation.Proceed();
            }
            else
            {
                var decoratedTask = WrapInvocationInTask(invocation, decorators);

                if (decoratedTask.IsFaulted)
                {
                    (decoratedTask.Exception?.InnerException ?? decoratedTask.Exception).Rethrow();
                }

                if (!decoratedTask.IsCompleted)
                {
                    invocation.ReturnValue = decoratedTask;
                }
            }
        }

        public void InterceptAsynchronous<TResult>(IInvocation invocation)
        {
            var targetMethod = invocation.MethodInvocationTarget.IsGenericMethod
                ? invocation.MethodInvocationTarget.GetGenericMethodDefinition()
                : invocation.MethodInvocationTarget;

            if (!MethodDecoratorMap.TryGetValue(targetMethod, out var decorators)
                || decorators == null || decorators.Length == 0)
            {
                // Method is not decorated.
                invocation.Proceed();
            }
            else
            {
                invocation.ReturnValue = WrapInvocationInTaskWithResult<TResult>(invocation, decorators);
            }
        }

        private static async Task WrapInvocationInTask(IInvocation invocation, IDecorator[] decorators)
        {
            var call = new Call(invocation, decorators);

            await decorators[0].OnInvoke(call).ConfigureAwait(false);
        }

        private static async Task<TResult> WrapInvocationInTaskWithResult<TResult>(IInvocation invocation, IDecorator[] decorators)
        {
            var call = new Call(invocation, decorators);

            await decorators[0].OnInvoke(call).ConfigureAwait(false);

            return (TResult)call.ReturnValue;
        }
    }
}
