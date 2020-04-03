using Castle.DynamicProxy;
using Decor.Internal;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Decor
{
    public sealed class Call
    {
        /// <summary>
        /// Gets the <see cref="MethodInfo"/> representing the method at call site.
        /// It is different from <see cref="MethodImplementation"/> property only when a call originates from an interface.
        /// </summary>
        public MethodInfo Method => _invocation.Method;

        /// <summary>
        /// Gets the <see cref="MethodInfo"/> representing the called method's implementation.
        /// It is different from <see cref="Method"/> property only when a call originates from an interface.
        /// </summary>
        public MethodInfo MethodImplementation => _invocation.MethodInvocationTarget;

        /// <summary>
        /// Gets the collection of arguments passed to the target method.
        /// Arguments can be modified inside <see cref="IDecorator"/>.
        /// </summary>
        public object[] Arguments => _invocation.Arguments;

        /// <summary>
        /// Gets or sets the return value provided by call target or other <see cref="IDecorator"/> instances.
        /// </summary>
        public object ReturnValue { get; set; }

        /// <summary>
        /// Gets the generic arguments of the method. 
        /// Returns empty if method is not generic.
        /// </summary>
        public Type[] GenericArguments => _invocation.GenericArguments ?? Array.Empty<Type>();

        /// <summary>
        /// Gets the object on which call was performed. 
        /// </summary>
        public object Object => _invocation.InvocationTarget;

        private readonly IInvocation _invocation;
        private readonly IInvocationProceedInfo _proceedInfo;
        private readonly IDecorator[] _decorators;
        private int _callIndex = 1;

        internal Call(IInvocation invocation, IDecorator[] decorators)
        {
            _invocation = invocation;
            _decorators = decorators;
            _proceedInfo = invocation.CaptureProceedInfo();
        }

        /// <summary>
        /// Executes the next <see cref="IDecorator.OnInvoke(Call)"/> and eventually the called method itself.
        /// </summary>
        public async Task Next()
        {
            var currentCallIndex = _callIndex;
            try
            {
                if (_decorators != null && _callIndex < _decorators.Length)
                {
                    await _decorators[_callIndex++].OnInvoke(this).ConfigureAwait(false);
                }
                else
                {
                    _proceedInfo.Invoke();

                    if (_invocation.ReturnValue is Task task && MethodImplementation.IsAsync())
                    {
                        if (!task.IsCompleted)
                        {
                            // Async methods are executed within interception.
                            // Non-async method returned task is treated as a result 
                            // and is not executed within interception.
                            await task.ConfigureAwait(false);
                        }

                        // Runtime might return Task<T> derived type here.
                        // Discussed in dotnet/runtime#26312 and microsoft/vs-streamjsonrpc#116.
                        if (task.GetType().GetTypeInfo().TryGetGenericTaskType(out var genericTaskType))
                        {
                            if (genericTaskType.IsTaskWithVoidTaskResult())
                            {
                                return;
                            }

                            var resultProperty = genericTaskType.GetDeclaredProperty("Result");
                            if (resultProperty == null)
                            {
                                throw new InvalidOperationException(
                                    $"Object of type '{genericTaskType}' was expected to contain a property 'Result'.");
                            }

                            ReturnValue = resultProperty.GetValue(task);
                        }
                    }
                    else
                    {
                        ReturnValue = _invocation.ReturnValue;
                    }
                }
            }
            catch (Exception)
            {
                // Call index is reset to position before the exception, so that
                // the current decorator could call Next() again if needed.
                _callIndex = currentCallIndex;
                throw;
            }
        }
    }
}
