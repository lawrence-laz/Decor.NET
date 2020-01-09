using Decor.Internal;

namespace Decor
{
    public interface IDecorator : IBaseDecorator
    {
        void OnBefore(CallInfo callInfo);
        void OnAfter(CallInfo callInfo);
    }
}
