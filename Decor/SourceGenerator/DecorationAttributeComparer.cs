using System.Collections.Generic;

namespace Decor.SourceGenerator
{
    internal class DecorationAttributeComparer : IEqualityComparer<DecorationAttribute>
    {
        public bool Equals(DecorationAttribute x, DecorationAttribute y)
        {
            return x.DecoratorFullName == y.DecoratorFullName;
        }

        public int GetHashCode(DecorationAttribute obj)
        {
            return obj.DecoratorFullName.GetHashCode();
        }
    }
}
