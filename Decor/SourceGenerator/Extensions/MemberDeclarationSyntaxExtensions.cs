using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;

namespace Decor.SourceGenerator.Extensions
{
    internal static class MemberDeclarationSyntaxExtensions
    {
        public static SyntaxToken GetModifierOrDefault(this MemberDeclarationSyntax memberDeclarationSyntax, SyntaxKind syntaxKind)
        {
            var modifier = memberDeclarationSyntax
                .Modifiers
                .FirstOrDefault(x => x.IsKind(syntaxKind));

            return modifier;
        }

        public static bool TryGetModifier(this MemberDeclarationSyntax memberDeclarationSyntax, SyntaxKind syntaxKind, out SyntaxToken modifier)
        {
            modifier = memberDeclarationSyntax.GetModifierOrDefault(syntaxKind);

            return modifier != null;
        }

        //public static bool TryGetAttribute(this MemberDeclarationSyntax memberDeclarationSyntax, out SyntaxToken attribute)
        //{
        //    //memberDeclarationSyntax.AttributeLists.FirstOrDefault(x => x.)
        //}
    }
}
