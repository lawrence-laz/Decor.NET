using Castle.DynamicProxy;
using System.Reflection;

namespace Decor.UnitTests.Utils
{
    public static class ProxyUtils
    {
        public static  T UnwrapProxy<T>(this T proxy)
            => ProxyUtil.IsProxy(proxy)
                ? (T)proxy.GetType()
                    .GetField("__target", BindingFlags.NonPublic | BindingFlags.Instance)
                    .GetValue(proxy)
                : proxy;
    }
}
