using System.Linq;
using Decor.Internal;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Collections;

namespace Decor.SourceGenerator
{
    internal class DecorateChain : IEnumerable<InvocationExpressionSyntax>
    {
        private List<InvocationExpressionSyntax> _invocations = new();

        public void Add(InvocationExpressionSyntax invocation) => _invocations.Add(invocation);

        public IEnumerator<InvocationExpressionSyntax> GetEnumerator() => _invocations.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _invocations.GetEnumerator();
    }

    internal class DecorationSyntaxReceiver : ISyntaxReceiver
    {
        private List<InvocationExpressionSyntax> _decorationBuilderMethods = new();

        public List<MethodDeclarationSyntax> MethodsWithAttributes { get; } = new();
        public List<InvocationExpressionSyntax> MethodsToDecorate { get; } = new();
        public List<IDecorationBuilder> DecorationBuilders { get; } = new();

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {

            if (!(syntaxNode is InvocationExpressionSyntax invocation))
            {
                return;
            }

            // DebugBuddy.WaitForDebugger();

            if (!invocation.GetText().ToString().Contains(nameof(Decorateee))) // Add a nice check if this is decorate chain
            {
                return;
            }

            _decorationBuilderMethods.Add(invocation);

            // var isFinalChainInvocation = invocation.GetText().ToString().Trim().EndsWith(";"); // Not very reliable?
            // var isFinalChainInvocation = invocation.GetText().ToString().Trim().Contains("Type"); // Not very reliable?
            var isFinalChainInvocation = invocation
                .ChildNodes()
                .FirstOrDefault()
                ?.ChildNodes()
                ?.OfType<IdentifierNameSyntax>()
                ?.FirstOrDefault()
                ?.Identifier
                .Text == nameof(Decorateee); // Not very reliable?
            // var isFinalChainInvocation = invocation is InvocationExpressionSyntax; // Not very reliable?
            if (isFinalChainInvocation)
            {
                var methodName = (invocation.Expression as MemberAccessExpressionSyntax)?.Name?.ToString();
                IDecorationBuilder decorationBuilder = methodName switch
                {
                    nameof(Decorateee.Type) => new ClassDecorationBuilder(),
                    nameof(DecorateType<object>.Method) => new MethodDecorationBuilder(),
                    _ => throw new NotImplementedException() // What
                };

                // Console.WriteLine("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFf");
                // Debugger.Log(1, "fff", "FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF");
                DecorationBuilders.Add(decorationBuilder);
                _decorationBuilderMethods = new();
            }

            // var methodName = (invocation.Expression as MemberAccessExpressionSyntax)?.Name?.ToString();
            // if (methodName?.StartsWith("With") is true)
            // {
            //     MethodsToDecorate.Add(invocation);
            //     // Solution
            //     // invocation
            // }

            // throw new Exception(invocation.GetText().ToString());
            // Debug.WriteLine(invocation.GetText().ToString());

            // System.Diagnostics.Debugger.Launch();

            // if (!(syntaxNode is MethodDeclarationSyntax methodDeclarationSyntax))
            // {
            //     return;
            // }

            // if (methodDeclarationSyntax.AttributeLists.Count > 0)
            // {
            //     MethodsWithAttributes.Add(methodDeclarationSyntax);
            // }
        }
    }
}
