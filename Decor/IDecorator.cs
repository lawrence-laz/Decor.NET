using System.Threading.Tasks;

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
        /// <param name="call">Representation of the intercepted method call.</param>
        Task OnInvoke(Call call);
    }
}
