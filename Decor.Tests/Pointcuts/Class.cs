using System.Threading.Tasks;

namespace Decor.Tests.Pointcuts
{
    // TODO: by namespace, by assembly
    // TODO: method supply param types for overload specification

    public class MethodDecoration<T>
    {
        private readonly string _methodName;

        public MethodDecoration(string methodName)
        {
            _methodName = methodName;
        }

        public void With<TDecorator>()
        {

        }
    }

    public class ClassDecoration<T>
    {
        public MethodDecoration<T> Method(string methodName)
        {
            return new MethodDecoration<T>(methodName);
        }
    }

    public static class Decorate
    {
        public static ClassDecoration<T> Class<T>()
        {
            return new ClassDecoration<T>();
        }
    }

    public class MyDecorator : IDecorator
    {
        public Task OnInvoke(Invocation invocation)
        {
            throw new System.NotImplementedException();
        }
    }

    public class MyClass
    {
        public void MyMethod()
        {

        }
    }

    public class Class
    {
        public void Foo()
        {
            Decorate.Class<MyClass>()
                .Method(nameof(MyClass.MyMethod))
                .With<MyDecorator>();
                // .As<MyClassDecorated>(); // This is to specify other names if needed

            // Decorate.Assembly(assembly)...
            // Decorate.Namespace(string)...
            
        }
    }
}
