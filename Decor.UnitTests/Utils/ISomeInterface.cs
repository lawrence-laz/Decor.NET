using System.Diagnostics;
using System.Threading.Tasks;

namespace Decor.UnitTests.Utils
{
    public interface ISomeInterface
    {
        void AttributeInClassMethod();

        Task<T> AsyncAttributeInClassWithReturnMethod<T>(T value);

        T AttributeInClassWithReturnMethod<T>(T value);

        [Decorate(typeof(SomeDecorator))]
        void AttributeInInterfaceMethod();

        void NoAttributeMethod();

        [Decorate(typeof(SomeDecorator))]
        Task<long> AsyncAttributeInInterfaceMethod(Stopwatch stopwatch);

        [Decorate(typeof(SomeDecorator))]
        void AttributeInInterfaceAndClassMethod();
    }
}
