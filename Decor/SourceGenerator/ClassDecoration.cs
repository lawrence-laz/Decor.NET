using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Decor.SourceGenerator
{
    internal class ClassDecoration
    {
        public ClassDeclarationSyntax ClassSyntax { get; set; }
        public ITypeSymbol ClassSymbol { get; set; }
        public MethodDecoration[] MethodDecorations { get; set; }
    }
}
