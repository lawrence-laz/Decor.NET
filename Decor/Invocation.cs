using System;
using System.Threading.Tasks;

// TODO: Solve warnings
// TODO: MethodInfo Method?
// TODO: MethodInfo MethodImplementation
// TODO: Type[] GenericArguments
// TODO: TargetObject
namespace Decor
{
    public sealed class Invocation
    {
        internal IDecorator[] _decorators;
        internal int _decoratorIndex;
        internal Func<object[], object> _targetMethod;
        internal Func<object[], Task<object>> _targetMethodAsync;

        // TODO: Add type validation?
        public object[] Arguments { get; set; }
        // TODO: Add type validation?
        public object ReturnValue { get; set; }
        
        public Invocation(Func<object[], Task<object>> targetMethodAsync, object[] arguments, 
            IDecorator[] decorators)
        {
            _targetMethodAsync = targetMethodAsync;
            _decorators = decorators;
            Arguments = arguments;
        }

        public Invocation(Func<object[], object> targetMethod, object[] arguments, IDecorator[] decorators)
        {
            _targetMethod = targetMethod;
            _decorators = decorators;
            Arguments = arguments;
        }

        public async Task Next()
        {
            if (_decorators == null)
            {
                throw new System.Exception(); // TODO:
            }

            var decoratorIndexAtStart = _decoratorIndex;

            if (_decoratorIndex < _decorators.Length)
            {
                try
                {
                    var nextDecorator = _decorators[_decoratorIndex];
                    ++_decoratorIndex;
                    await nextDecorator.OnInvoke(this);
                }
                catch (Exception)
                {
                    // Decorator index is reset to position before the exception,
                    // so as to enable the current decorator to call Next() again.
                    _decoratorIndex = decoratorIndexAtStart;
                    throw;
                }
            }
            else if (_targetMethodAsync != null)
            {
                ReturnValue = await _targetMethodAsync.Invoke(Arguments);
            }
            else if (_targetMethod != null)
            {
                ReturnValue = _targetMethod?.Invoke(Arguments);
            }
            else
            {
                throw new Exception(); // TODO:
            }
        }
    }
}
