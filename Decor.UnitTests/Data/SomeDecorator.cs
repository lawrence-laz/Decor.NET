using System.Diagnostics;
using System.Linq;

namespace Decor.UnitTests.Data
{
    public class SomeDecorator : IDecorator
    {
        public int OnAfterCallCount = 0;
        public int OnBeforeCallCount = 0;
        public CallInfo CallInfoAfter;
        public CallInfo CallInfoBefore;

        public void OnAfter(CallInfo callInfo)
        {
            (callInfo.Arguments.FirstOrDefault() as Stopwatch)?.Stop();

            OnAfterCallCount++;
            CallInfoAfter = callInfo;
        }

        public void OnBefore(CallInfo callInfo)
        {
            OnBeforeCallCount++;
            CallInfoBefore = callInfo;
        }
    }
}
