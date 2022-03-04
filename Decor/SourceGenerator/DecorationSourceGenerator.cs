using System.ComponentModel.Design.Serialization;
using System.Threading;
using Decor.SourceGenerator;
using Decor.SourceGenerator.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq.Expressions;

// TODO: Attributes should be just one of the strategies on how to decorate. There should be a low-level api underneath open for extending.

// TODO: allow decorate attribute on class?

// TODO Formatter.Format(newRoot, new AdhocWorkspace());

// TODO: Analyzer to check if target method is virtual (interface or explicitly virtual)

namespace Decor
{
    public static class Decorateee
    {
        public static DecorateType<T> Type<T>()
        {
            return new DecorateType<T>();
        }
    }

    public class DecorateType<T>
    {
        public DecorateType<T> Method()
        {
            return this;
        }

        public DecorateType<T> Method(Expression<Action<T>> methodSelector)
        {
            return this;
        }

        // generated code
        public DecorateType<T> Method(Expression<Func<T, Action<string, int>>> methodSelector)
        {
            return this;
        }

        // // generated code
        // public DecorateType<T> Method(Expression<Func<T, Action<string, string>>> methodSelector)
        // {
        //     return this;
        // }

        public void With<T>() where T : IDecorator
        {

        }
    }
}

namespace Decor.Internal
{
    [Generator]
    public sealed class DecorationSourceGenerator : ISourceGenerator
    {
        private INamedTypeSymbol _taskWithoutReturnType;

        /// <summary>
        /// Register Decor syntax receivers.
        /// </summary>
        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new DecorationSyntaxReceiver());
        }

        public void Execute(GeneratorExecutionContext context)
        {
            if (context.SyntaxReceiver is not DecorationSyntaxReceiver decorReceiver)
            {
                return;
            }

            // DebugBuddy.WaitForDebugger();

            // context.Compilation.GetSymbolsWithName();

            // Microsoft.CodeAnalysis.FindSymbols.SymbolFinder.FindDeclarationsAsync();

            // Find invocations
            // var invocationSyntax = SyntaxFactory.InvocationExpression(
                
            // );
            // var semanticModel = context.Compilation.GetSemanticModel(invocationSyntax.SyntaxTree);
            // var mapToSymbol = semanticModel.GetSymbolInfo(invocationSyntax.Expression).Symbol as IMethodSymbol;
            // var convertToType = mapToSymbol.ReturnType;

            // var invocations = string.Join(Environment.NewLine, decorReceiver.MethodsToDecorate.Select(x => x.GetText().ToString()));
            // var invocations = string.Join(Environment.NewLine, decorReceiver.MethodsToDecorate.Select(x => (x.Expression as MemberAccessExpressionSyntax)?.Name.ToString()));
            var methodSymbols = new List<IMethodSymbol>();
            var chainsAsString = new StringBuilder(Environment.NewLine);
            foreach (var decorateChain in decorReceiver.DecorationBuilders)
            {
                var invocations = decorateChain
                    .Reverse()
                    .Select(invocation =>
                    {
                        var semantic = context.Compilation.GetSemanticModel(invocation.SyntaxTree);
                        return semantic.GetSymbolInfo(invocation, context.CancellationToken).Symbol;
                    })
                    .OfType<IMethodSymbol>()
                    .Select(invocation => invocation.Name)
                    .ToList();

                chainsAsString.Append(string.Join(" -> ", invocations) + Environment.NewLine);
                // methodSymbol.GetAttributes().Any(x => x.AttributeClass)
            }

            var a = string.Join(Environment.NewLine, methodSymbols.Select(x => x.ToDisplayString()));
            
            context.AddSource(
                "DecorMarker.cs",
                $"public class DecorMarker {{ public static string Hello => @\"Hello from Decor! {chainsAsString.ToString()}\"; }}");

            if (!(context.SyntaxReceiver is DecorationSyntaxReceiver syntaxReceiver))
                return;

            _taskWithoutReturnType = context.Compilation
                .GetTypeByMetadataName($"{nameof(System)}.{nameof(System.Threading)}.{nameof(System.Threading.Tasks)}.{nameof(Task)}");

            var decorateAttribute = context.Compilation
                .GetTypeByMetadataName($"{nameof(Decor)}.{nameof(DecorateAttribute)}");

            var methodDecorations = syntaxReceiver
                .MethodsWithAttributes
                .Select(method =>
                {
                    var methodModel = context.Compilation.GetSemanticModel(method.SyntaxTree);
                    var methodSymbol = methodModel.GetDeclaredSymbol(method);
                    var decorationAttributes = methodSymbol
                        .GetAttributes()
                        .Where(attributeData => attributeData.AttributeClass.Equals(decorateAttribute, SymbolEqualityComparer.Default))
                        .Select(attributeData => new DecorationAttribute(attributeData));

                    var methodDecoration = decorationAttributes.Any()
                        ? new MethodDecoration
                        {
                            MethodSyntax = method,
                            MethodSymbol = methodSymbol,
                            DecorateAttributes = decorationAttributes
                        }
                        : default;

                    return methodDecoration;
                })
                .Where(x => x != default)
                .ToArray();

            var classDecorations = methodDecorations
                .GroupBy(x => (ITypeSymbol)x.MethodSymbol.ContainingType)
                .Select(x => new ClassDecoration
                {
                    ClassSyntax = (ClassDeclarationSyntax)x.First().MethodSyntax.Parent,
                    ClassSymbol = x.Key,
                    MethodDecorations = x.ToArray()
                })
                .ToArray();

            foreach (var decoration in classDecorations)
            {
                var usingsSyntax = decoration
                    .ClassSyntax
                    .SyntaxTree.GetRoot()
                    .DescendantNodes()
                    .OfType<UsingDirectiveSyntax>();
                // TODO: make sure local variables are appended with decor_ to avoid conflicts
                var decoratedClassSource = $@"
{string.Join(Environment.NewLine, usingsSyntax)}

// TODO: Add description of something like this class is an automatically generated class by Decor.
namespace {decoration.ClassSymbol.ContainingNamespace}
{{
    {decoration.ClassSyntax.Modifiers} class {decoration.ClassSyntax.Identifier}Decorated : {decoration.ClassSymbol}
    {{
        {GetDecoratorArrayFields(decoration)}

        {GetConstructors(decoration)}

        {GetMethodOverrides(context, decoration)}
    }}
}}
";

                context.AddSource($"{decoration.ClassSyntax.Identifier}Decorated", SourceText.From(decoratedClassSource, Encoding.UTF8));
            }
        }

        private string GetMethodOverrides(GeneratorExecutionContext context, ClassDecoration decoration)
        {
            return decoration
                .MethodDecorations
                .Select(method => GetMethodOverride(method, context))
                .JoinStrings(Environment.NewLine);
        }

        private string GetDecoratorArrayFields(ClassDecoration decoration)
        {
            return decoration
                .MethodDecorations
                .Select(GetDecoratorArrayField)
                .JoinStrings(Environment.NewLine);
        }

        private string GetDecoratorArrayField(MethodDecoration decoration)
        {
            // TODO: Overloads with same name?
            var source = $"public {nameof(IDecorator)}[] {decoration.MethodSyntax.Identifier.ValueText}Decorators;";

            return source;
        }

        private string GetConstructors(ClassDecoration decoration)
        {
            var source = new StringBuilder();

            //Debugger.Launch();
            var decoratorParameters = decoration
                .MethodDecorations
                .SelectMany(method => method.DecorateAttributes)
                .Distinct(new DecorationAttributeComparer())
                .Select(attribute => SyntaxFactory.Parameter(
                    default,
                    default,
                    SyntaxFactory.ParseTypeName(attribute.DecoratorFullName),
                    SyntaxFactory.Identifier(attribute.DecoratorName.AsParam()),
                    default
                    )
                )
                .ToArray();

            var methodListAssignments = decoration
                .MethodDecorations
                .Select(methodDecoration =>
                {
                    if (!methodDecoration.DecorateAttributes.Any())
                    {
                        return string.Empty;
                    }

                    var source = $@"
            {methodDecoration.MethodSyntax.Identifier.ValueText}Decorators = new [] 
            {{
                {methodDecoration.DecorateAttributes.Select(x => x.DecoratorName.AsParam()).JoinStrings(", ")}
            }};
";

                    return source;
                })
                .JoinStrings(Environment.NewLine);

            var constructors = decoration
                .ClassSyntax
                .Members
                .OfType<ConstructorDeclarationSyntax>();

            if (!constructors.Any())
            {
                constructors = new[] {
                    SyntaxFactory.ConstructorDeclaration(
                        attributeLists: default,
                        modifiers: SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.PublicKeyword)),
                        identifier: SyntaxFactory.Identifier(decoration.ClassSyntax.Identifier.ValueText),
                        parameterList: SyntaxFactory.ParameterList(),
                        initializer: default,
                        expressionBody: default
                    )
                };
            }

            foreach (var constructor in constructors)
            {
                var decoratedParameterList = constructor.ParameterList.AddParameters(decoratorParameters);

                var baseParameterNames = constructor
                    .ParameterList
                    .Parameters
                    .Select(x => x.Identifier.Text);

                var constructorSource = $@"
        {constructor.Modifiers} {constructor.Identifier.Text}Decorated{decoratedParameterList}
            : base({baseParameterNames.JoinStrings(", ")})
        {{
            {methodListAssignments}
        }}
";

                source.Append(constructorSource);
            }

            return source.ToString();
        }

        // TODO: maybe shouldn't build string, but rather modify body? WOuld be easier to catch all cases like async not async and generics and generic arg constraints, nullable types, etc.
        // remove virutal, add override
        private string GetMethodOverride(MethodDecoration decoration, GeneratorExecutionContext context)
        {
            // TODO: if decorated.
            if (TryCatchDynamicParametersMethod(decoration, context))
            {
                // TODO: Should this show error?
                return string.Empty;
            }

            var methodOverride = decoration.MethodSyntax;

            if (!ReplaceVirtualWithOverrideModifier(methodOverride, out methodOverride))
            {
                return string.Empty;
            }

            methodOverride = CopyAttributesExceptDecorateAttribute(methodOverride);

            //var parametersSource = decoration
            //    .MethodSymbol
            //    .Parameters
            //    .Select(parameter => $"{parameter.Type} {parameter.Name}")
            //    .JoinStrings(", ");

            methodOverride = ReplaceBody(decoration, methodOverride);

            // TODO: Handle generics

            return methodOverride.ToFullString();

            //            return $@"
            //        {decoration.MethodSymbol.ReturnType} {decoration.MethodSyntax.Identifier}({parametersSource})
            //        {{
            //            var invocation = new {nameof(Invocation)}(
            //                {invocationTargetLambda},
            //                {argumentsSource},
            //                {decoration.MethodSyntax.Identifier.ValueText}Decorators
            //            );

            //            {invocationCallSource}

            //            // TODO: Throw a nice exception when return value was not specified for a nonvoid/Task async method.
            //            {returnSource}
            //        }}
            //";
        }

        private MethodDeclarationSyntax ReplaceBody(MethodDecoration decoration, MethodDeclarationSyntax methodOverride)
        {
            var body = GetBody(decoration);

            methodOverride = methodOverride
                .WithExpressionBody(null)
                .WithSemicolonToken(default)
                .WithBody(body);

            return methodOverride;
        }

        private BlockSyntax GetBody(MethodDecoration decoration)
        {
            var bodyOverride = $@"
            var invocation = new {nameof(Invocation)}(
                {GetInvocationTargetLambda(decoration)},
                {GetInvocationArguments(decoration)},
                {decoration.MethodSyntax.Identifier.ValueText}Decorators
            );

            {GetInvocationCall(decoration)}

            // TODO: Throw a nice exception when return value was not specified for a nonvoid/Task async method.
            {GetReturn(decoration)}
";

            var bodySyntax = SyntaxFactory.ParseStatement(bodyOverride);
            var blockSyntax = SyntaxFactory.Block(bodySyntax);
            return blockSyntax;
        }

        private string GetReturn(MethodDecoration decoration)
        {
            string returnSource;
            if (decoration.MethodSymbol.IsAsync)
            {
                if (decoration.MethodSymbol.ReturnType.Equals(_taskWithoutReturnType, SymbolEqualityComparer.Default))
                {
                    returnSource = string.Empty;
                }
                else if (decoration.MethodSymbol.ReturnsVoid)
                {
                    returnSource = string.Empty;
                }
                else
                {
                    var returnType = ((INamedTypeSymbol)decoration.MethodSymbol.ReturnType).TypeArguments.First();
                    returnSource = $"return ({returnType})invocation.{nameof(Invocation.ReturnValue)};";
                }
            }
            else
            {
                if (decoration.MethodSymbol.ReturnsVoid)
                {
                    returnSource = string.Empty;
                }
                else
                {
                    returnSource = $"return ({decoration.MethodSymbol.ReturnType})invocation.{nameof(Invocation.ReturnValue)};";
                }
            }

            return returnSource;
        }

        private static string GetInvocationArgumentsFromObjectArray(MethodDecoration decoration)
        {
            return decoration
                .MethodSymbol
                .Parameters
                .Select((parameter, index) => $"({parameter.Type})arguments[{index}]")
                .JoinStrings(", ");
        }

        private static string GetInvocationCall(MethodDecoration decoration)
        {
            return decoration.MethodSymbol.IsAsync
                ? "await invocation.Next();"
                : $@"try
            {{
                invocation.Next().Wait();
            }}
            catch({nameof(System)}.{nameof(AggregateException)} e)
            {{
                    {nameof(System)}
                    .{nameof(System.Runtime)}
                    .{nameof(System.Runtime.ExceptionServices)}
                    .{nameof(ExceptionDispatchInfo)}
                    .{nameof(ExceptionDispatchInfo.Capture)}(e.{nameof(AggregateException.InnerException)})
                    .{nameof(ExceptionDispatchInfo.Throw)}();
            }}";
        }

        private static string GetInvocationArguments(MethodDecoration decoration)
        {
            return decoration.MethodSymbol.Parameters.Any()
                ? decoration
                    .MethodSymbol
                    .Parameters
                    .Select(parameter => $"{parameter.Name}")
                    .JoinStrings(", ")
                    .ModifyString(arguments => $"new object[] {{ {arguments} }}")
                : "System.Array.Empty<object>()";
        }

        private string GetInvocationTargetLambda(MethodDecoration decoration)
        {
            var arguments = GetInvocationArgumentsFromObjectArray(decoration);
            string invocationTargetLambda;
            if (decoration.MethodSymbol.IsAsync)
            {
                if (decoration.MethodSymbol.ReturnType.Equals(_taskWithoutReturnType, SymbolEqualityComparer.Default))
                {
                    invocationTargetLambda =
                        $@"targetMethodAsync: async arguments => 
                {{ 
                    await base.{decoration.MethodSymbol.Name}({arguments}); 
                    return default; 
                }}";
                }
                else if (decoration.MethodSymbol.ReturnsVoid)
                {
                    invocationTargetLambda =
                        $"targetMethodAsync: arguments => {{ base.{decoration.MethodSymbol.Name}({arguments}); return default; }}";
                }
                else
                {
                    invocationTargetLambda =
                        $"targetMethodAsync: async arguments => await base.{decoration.MethodSymbol.Name}({arguments})";
                }
            }
            else
            {
                if (decoration.MethodSymbol.ReturnsVoid)
                {
                    invocationTargetLambda =
                        $"targetMethod: arguments => {{ base.{decoration.MethodSymbol.Name}({arguments}); return default; }}";
                }
                else
                {
                    invocationTargetLambda =
                        $"targetMethod: arguments => base.{decoration.MethodSymbol.Name}({arguments})";
                }
            }

            return invocationTargetLambda;
        }

        private static MethodDeclarationSyntax CopyAttributesExceptDecorateAttribute(MethodDeclarationSyntax methodOverride)
        {
            if (methodOverride.AttributeLists.Any())
            {
                methodOverride = methodOverride.WithAttributeLists(
                    SyntaxFactory.List(methodOverride.AttributeLists
                        .Select(attributeList => attributeList.WithAttributes(
                            SyntaxFactory.SeparatedList(attributeList.Attributes
                                .Where(attribute => !attribute.Name.ToString().Contains("Decorate"))))) // TODO: Add proper type check.
                        .Where(attributeList => attributeList.Attributes.Any()))); // TODO: Check by type, not name.
            }

            return methodOverride;
        }

        private static bool ReplaceVirtualWithOverrideModifier(MethodDeclarationSyntax current, out MethodDeclarationSyntax updated)
        {
            if (current.TryGetModifier(SyntaxKind.VirtualKeyword, out var virtualModifier))
            {
                var modifiers = current
                    .Modifiers
                    .Remove(virtualModifier)
                    .Add(SyntaxFactory.ParseToken("override "));

                updated = current.WithModifiers(modifiers);
                return true;
            }

            updated = default;
            // TODO: Check if method is virtual. Cannot decorate non-virtual methods.
            return false;
        }

        private bool TryCatchDynamicParametersMethod(MethodDecoration decoration, GeneratorExecutionContext context)
        {
            bool methodContainsDynamicParameters = decoration
                .MethodSymbol
                .Parameters
                .Any(parameter => parameter.Type.TypeKind == TypeKind.Dynamic);

            if (methodContainsDynamicParameters)
            {
                context.ReportDiagnostic(Diagnostic.Create(
                    new DiagnosticDescriptor(
                        "CS1971",
                        "Decorating methods with dynamic parameters is not supported.",
                        "Cannot decorate method {0}.{1} because it contains dynamic parameters.",
                        "Decor",
                        DiagnosticSeverity.Error,
                        isEnabledByDefault: true),
                    Location.Create(decoration.MethodSyntax.SyntaxTree, decoration.MethodSyntax.Span),
                    (decoration.MethodSyntax.Parent as TypeDeclarationSyntax).Identifier.ValueText,
                    decoration.MethodSyntax.Identifier.ValueText));
            }

            return methodContainsDynamicParameters;
        }
    }
}
