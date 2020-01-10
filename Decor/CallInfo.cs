using Castle.DynamicProxy;
using System;
using System.Reflection;

namespace Decor
{
    public sealed class CallInfo
    {
        private object _state;

        public MethodInfo Method => Invocation.Method;
        public object[] Arguments => Invocation.Arguments;
        public object ReturnValue => Invocation.ReturnValue;
        public Type[] GenericArguments => Invocation.GenericArguments;
        public object Object => Invocation.InvocationTarget;

        internal IInvocation Invocation { get; set; }

        internal CallInfo(IInvocation invocation)
        {
            Invocation = invocation;
        }

        /// <summary>
        /// Save a state inside <see cref="IDecorator.OnBefore(CallInfo)"/> to be later 
        /// accessed in <see cref="IDecorator.OnAfter(CallInfo)"/>.
        /// </summary>
        /// <param name="state">State can be any object needed by a decorator.</param>
        public void SetState(object state) => _state = state;

        /// <summary>
        /// Get a state in <see cref="IDecorator.OnAfter(CallInfo)"/>, which was
        /// previously saved in <see cref="IDecorator.OnBefore(CallInfo)"/>.
        /// </summary>
        /// <typeparam name="T">Type has to match to what was provided to <see cref="SetState(object)"/>.</typeparam>
        /// <returns>State object previously saved with <see cref="SetState(object)"/>.</returns>
        public T GetState<T>() => (T)_state;
    }
}
