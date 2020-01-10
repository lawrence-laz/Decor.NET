namespace Decor.UnitTests.Utils
{
    public class AnotherDecorator : IDecorator
    {
        public int OnAfterCallCount = 0;
        public int OnBeforeCallCount = 0;

        public void OnAfter(CallInfo callInfo)
        {
            OnAfterCallCount++;
        }

        public void OnBefore(CallInfo callInfo)
        {
            OnBeforeCallCount++;
        }
    }
}
