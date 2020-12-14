using System.Threading.Tasks;

namespace Decor.UnitTests.Common
{
    public class TestDecorator : IDecorator
    {
        public bool WasInvoked { get; set; }

        public async Task OnInvoke(Call call)
        {
            WasInvoked = true;
            await call.Next();
        }
    }
}
