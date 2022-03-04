using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;

namespace Decor.SourceGenerator
{
    internal class MethodDecoration
    {
        public MethodDeclarationSyntax MethodSyntax { get; set; }
        public IMethodSymbol MethodSymbol { get; set; }
        public IEnumerable<DecorationAttribute> DecorateAttributes { get; set; }
    }
}
