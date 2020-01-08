namespace Decor
{
    public interface IDecorator
    {
        void OnBefore(CallInfo callInfo);
        void OnAfter(CallInfo callInfo);
    }
}
