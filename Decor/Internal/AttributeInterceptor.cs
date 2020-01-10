using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Decor.Internal
{
    public class AttributeInterceptor : AsyncInterceptor
    {
        private IBaseDecorator Decorator 
            => _decorator ?? (_decorator = _decoratorProvider.Get(_decoratorType));

        private IBaseDecorator _decorator;
        private readonly IDecoratorProvider _decoratorProvider;
        private readonly Type _decoratorType;

        public AttributeInterceptor(IDecoratorProvider decoratorProvider, Type decoratorType)
        {
            _decoratorProvider = decoratorProvider;
            _decoratorType = decoratorType;
        }

        protected async override Task BeforeInvocation(CallInfo callInfo)
        {
            if (!IsInTargetMethods(callInfo.Invocation.MethodInvocationTarget, callInfo.Invocation.Method))
            {
                return;
            }

            if (Decorator is IDecorator syncDecorator)
            {
                syncDecorator.OnBefore(callInfo);
            }
            else if (Decorator is IDecoratorAsync asyncDecorator)
            {
                await asyncDecorator.OnBefore(callInfo);
            }
        }

        protected async override Task AfterInvocation(CallInfo callInfo)
        {
            if (!IsInTargetMethods(callInfo.Invocation.MethodInvocationTarget, callInfo.Invocation.Method))
            {
                return;
            }

            if (Decorator is IDecorator syncDecorator)
            {
                syncDecorator.OnAfter(callInfo);
            }
            else if (Decorator is IDecoratorAsync asyncDecorator)
            {
                await asyncDecorator.OnAfter(callInfo);
            }
        }

        private bool IsInTargetMethods(params MethodInfo[] methods)
        {
            foreach (var method in methods)
            {
                var attributes = method.GetCustomAttributes<DecorateAttribute>(false);

                if (attributes.Any(attribute => attribute.DecoratorType == _decoratorType))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
