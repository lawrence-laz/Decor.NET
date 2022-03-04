using Microsoft.CodeAnalysis;

namespace Decor.SourceGenerator
{
    internal class DecorationAttribute
    {
        public AttributeData AttributeData { get; set; }
        public string DecoratorFullName => ((ITypeSymbol)AttributeData.ConstructorArguments[0].Value).ToString();
        public string DecoratorName => ((ITypeSymbol)AttributeData.ConstructorArguments[0].Value).Name;

        public DecorationAttribute(AttributeData attributeData)
        {
            AttributeData = attributeData;
        }

        public override bool Equals(object other) 
            => other is DecorationAttribute otherAttribute && otherAttribute.AttributeData.Equals(AttributeData);

        public override int GetHashCode() => AttributeData.GetHashCode();

        public static bool operator ==(DecorationAttribute a, DecorationAttribute b) => a.Equals(b);

        public static bool operator !=(DecorationAttribute a, DecorationAttribute b) => !(a == b);
    }
}
