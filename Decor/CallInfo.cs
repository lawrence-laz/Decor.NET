using Castle.DynamicProxy;
using System;
using System.Reflection;

namespace Decor
{
    public sealed class CallInfo
    {
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
    }
}
