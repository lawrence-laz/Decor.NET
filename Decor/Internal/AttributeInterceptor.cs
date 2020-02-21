using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Decor.Internal
{
    public class AttributeInterceptor : AsyncInterceptor
    {
        private Lazy<IBaseDecorator> _decorator;
        private readonly IDecoratorProvider _decoratorProvider;
        private readonly Type _decoratorType;

        public AttributeInterceptor(IDecoratorProvider decoratorProvider, Type decoratorType)
        {
            _decoratorProvider = decoratorProvider;
            _decoratorType = decoratorType;

            _decorator = new Lazy<IBaseDecorator>(() => _decoratorProvider.Get(_decoratorType), true);
        }

        protected async override Task BeforeInvocation(CallInfo callInfo)
        {
            if (!IsInTargetMethods(callInfo.Invocation.MethodInvocationTarget, callInfo.Invocation.Method))
            {
                return;
            }

            if (_decorator.Value is IDecorator syncDecorator)
            {
                syncDecorator.OnBefore(callInfo);
            }
            else if (_decorator.Value is IDecoratorAsync asyncDecorator)
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

            if (_decorator.Value is IDecorator syncDecorator)
            {
                syncDecorator.OnAfter(callInfo);
            }
            else if (_decorator.Value is IDecoratorAsync asyncDecorator)
            {
                await asyncDecorator.OnAfter(callInfo);
            }
        }

        private bool IsInTargetMethods(params MethodInfo[] methods)
        {
            foreach (var method in methods)
            {
                // TODO: Could be cached for faster calls.
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
