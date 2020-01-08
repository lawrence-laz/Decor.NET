using System.Diagnostics;
using System.Threading.Tasks;

namespace Decor.UnitTests.Data
{
    public interface ISomeInterface
    {
        void AttributeInClassMethod();

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
