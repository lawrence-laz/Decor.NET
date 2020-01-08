using System.Diagnostics;
using System.Threading.Tasks;

namespace Decor.UnitTests.Data
{
    public class SomeClass : ISomeInterface
    {
        [Decorate(typeof(SomeDecorator))]
        public void AttributeInClassMethod() { }

        [Decorate(typeof(SomeDecorator))]
        public T AttributeInClassWithReturnMethod<T>(T value) => value;

        public void AttributeInInterfaceMethod() { }

        [Decorate(typeof(SomeDecorator))]
        [Decorate(typeof(AnotherDecorator))]
        public virtual void MultiAttributeMethod() { }

        public void NoAttributeMethod() { }

        public async Task<long> AsyncAttributeInInterfaceMethod(Stopwatch stopwatch)
        {
            await Task.Delay(50);
            var elapsed = stopwatch.ElapsedMilliseconds;
            await Task.Delay(50);

            return elapsed;
        }

        [Decorate(typeof(SomeDecorator))]
        public void AttributeInInterfaceAndClassMethod() { }
    }
}
