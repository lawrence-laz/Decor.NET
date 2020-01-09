using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Decor.UnitTests.Data
{
    public class SomeAsyncDecorator : IDecoratorAsync
    {
        public int OnAfterCallCount = 0;
        public int OnBeforeCallCount = 0;
        public CallInfo CallInfoAfter;
        public CallInfo CallInfoBefore;

        public async Task OnAfter(CallInfo callInfo)
        {
            (callInfo.Arguments.FirstOrDefault() as Stopwatch)?.Stop();
            await Task.Delay(50);
            OnAfterCallCount++;
            CallInfoAfter = callInfo;
        }

        public async Task OnBefore(CallInfo callInfo)
        {
            await Task.Delay(50);
            OnBeforeCallCount++;
            CallInfoBefore = callInfo;
        }
    }
}
