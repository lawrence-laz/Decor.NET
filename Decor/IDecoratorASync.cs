using Decor.Internal;
using System.Threading.Tasks;

namespace Decor
{
    public interface IDecoratorAsync : IBaseDecorator
    {
        Task OnBefore(CallInfo callInfo);
        Task OnAfter(CallInfo callInfo);
    }
}
