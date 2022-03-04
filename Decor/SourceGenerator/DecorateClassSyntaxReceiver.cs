using System.Diagnostics;
using Decor.SourceGenerator;
using Decor.SourceGenerator.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace Decor.Internal
{
    internal class DecorateClassSyntaxReceiver : ISyntaxReceiver
    {
        public List<MethodDeclarationSyntax> MethodsWithAttributes { get; }
            = new List<MethodDeclarationSyntax>();

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            if (!(syntaxNode is InvocationExpressionSyntax invocation))
            {
                return;
            }

            Debug.WriteLine(invocation.GetText().ToString());
            throw new Exception();

            System.Diagnostics.Debugger.Launch();
        }
    }
}
