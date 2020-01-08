using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace Decor.UnitTests.Data
{
    public class ClassicDecorator : ISomeInterface
    {
        public void AttributeInInterfaceMethod() { }

        public Task<long> AsyncAttributeInInterfaceMethod(Stopwatch stopwatch) => throw new NotImplementedException();
        public void AttributeInClassMethod() => throw new NotImplementedException();
        public T AttributeInClassWithReturnMethod<T>(T value) => throw new NotImplementedException();
        public void AttributeInInterfaceAndClassMethod() => throw new NotImplementedException();
        public void NoAttributeMethod() => throw new NotImplementedException();
    }
}
