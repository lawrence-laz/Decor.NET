using System;
using System.Runtime.ExceptionServices;

namespace Decor.Internal
{
    internal static class ExceptionExtensions
    {
        public static void Rethrow(this Exception exception) 
            => ExceptionDispatchInfo.Capture(exception).Throw();
    }
}
