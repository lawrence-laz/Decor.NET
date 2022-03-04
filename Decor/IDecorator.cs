using System.Threading.Tasks;

// TODO: write to https://github.com/YairHalberstadt/stronginject ask for feedback
// TODO: how to add this to NuGet "This is a prerelease version of StrongInject". https://www.nuget.org/packages/StrongInject/0.0.1-CI-20200911-095011

namespace Decor
{
    /// <summary>
    /// Describes a decorator used for intercepting methods according to <see cref="DecorateAttribute"/>.
    /// </summary>
    public interface IDecorator
    {
        /// <summary>
        /// <para>Additional logic to be executed when a decorated method is intercepted.</para>
        /// <para>Use 'await call.Next()' to continue execution.</para>
        /// </summary>
        /// <param name="invocation">Representation of the intercepted method call.</param>
        /// TODO: Update
        Task OnInvoke(Invocation invocation);
    }
}
