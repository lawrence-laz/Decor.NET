/* using Castle.DynamicProxy; */
/* using System; */
/* using System.Collections.ObjectModel; */
/* using System.Reflection; */
/* using System.Threading.Tasks; */

/* namespace Decor.Internal */
/* { */
/*     internal class DecoratorInterceptor : IAsyncInterceptor */
/*     { */
/*         public DecoratorInterceptor(ReadOnlyDictionary<MethodInfo, IDecorator[]> methodDecoratorMap) */
/*         { */
/*             MethodDecoratorMap = methodDecoratorMap; */
/*         } */

/*         public ReadOnlyDictionary<MethodInfo, IDecorator[]> MethodDecoratorMap { get; } */

/*         public void InterceptSynchronous(IInvocation invocation) */
/*         { */
/*             if (TryGetMethodDecorators(invocation, out var decorators)) */
/*             { */
/*                 var call = new Call(invocation, decorators); */

/*                 try */
/*                 { */
/*                     decorators[0].OnInvoke(call).Wait(); */
/*                 } */
/*                 catch (AggregateException e) when (e.InnerException != null) */
/*                 { */
/*                     e.InnerException.Rethrow(); */
/*                 } */

/*                 invocation.ReturnValue = call.ReturnValue; */
/*             } */
/*             else */
/*             { */
/*                 invocation.Proceed(); */
/*             } */
/*         } */

/*         public void InterceptAsynchronous(IInvocation invocation) */
/*         { */
/*             if (TryGetMethodDecorators(invocation, out var decorators)) */
/*             { */
/*                 var decoratedTask = WrapInvocationInTask(invocation, decorators); */

/*                 if (decoratedTask.IsFaulted) */
/*                 { */
/*                     (decoratedTask.Exception?.InnerException ?? decoratedTask.Exception).Rethrow(); */
/*                 } */

/*                 if (!decoratedTask.IsCompleted) */
/*                 { */
/*                     invocation.ReturnValue = decoratedTask; */
/*                 } */
/*             } */
/*             else */
/*             { */
/*                 invocation.Proceed(); */
/*             } */
/*         } */

/*         public void InterceptAsynchronous<TResult>(IInvocation invocation) */
/*         { */
/*             if (TryGetMethodDecorators(invocation, out var decorators)) */
/*             { */
/*                 invocation.ReturnValue = WrapInvocationInTaskWithResult<TResult>(invocation, decorators); */
/*             } */
/*             else */
/*             { */
/*                 invocation.Proceed(); */
/*             } */
/*         } */

/*         private bool TryGetMethodDecorators(IInvocation invocation, out IDecorator[] decorators) */
/*         { */
/*             var targetMethod = invocation.MethodInvocationTarget.IsGenericMethod */
/*                 ? invocation.MethodInvocationTarget.GetGenericMethodDefinition() */
/*                 : invocation.MethodInvocationTarget; */

/*             return MethodDecoratorMap.TryGetValue(targetMethod, out decorators) */
/*                 && decorators != null && decorators.Length != 0; */
/*         } */

/*         private static async Task WrapInvocationInTask(IInvocation invocation, IDecorator[] decorators) */
/*         { */
/*             var call = new Call(invocation, decorators); */

/*             await decorators[0].OnInvoke(call).ConfigureAwait(false); */
/*         } */

/*         private static async Task<TResult> WrapInvocationInTaskWithResult<TResult>(IInvocation invocation, IDecorator[] decorators) */
/*         { */
/*             var call = new Call(invocation, decorators); */

/*             try */
/*             { */
/*                 await decorators[0].OnInvoke(call).ConfigureAwait(false); */
/*             } */
/*             catch (AggregateException aggreggateException) */
/*             { */
/*                 aggreggateException.InnerException?.Rethrow(); */
/*                 aggreggateException.Rethrow(); */
/*             } */
/*             catch (TargetInvocationException targetInvocationException) */
/*             { */
/*                 if (targetInvocationException.InnerException is AggregateException aggreggateException) */
/*                 { */
/*                     aggreggateException.InnerException?.Rethrow(); */
/*                     aggreggateException.Rethrow(); */
/*                 } */

/*                 targetInvocationException.InnerException?.Rethrow(); */
/*                 targetInvocationException.Rethrow(); */
/*             } */

/*             return (TResult)call.ReturnValue; */
/*         } */
/*     } */
/* } */
