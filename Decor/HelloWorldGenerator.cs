using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace SourceGeneratorSamples
{
    /// <summary>
    /// 
    /// </summary>
    [Generator]
    public class HelloWorldGenerator : ISourceGenerator
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void Execute(SourceGeneratorContext context)
        {


            var builder = new StringBuilder();
            builder.Append(@"
using System;
namespace HelloWorldGenerated
{
    public static class HelloWorld
    {
        public static void SayHello() 
        {
            Console.WriteLine(""Hello from generated code!"");
        }
    }
}
");
            context.AddSource("helloWorldGenerated", SourceText.From(builder.ToString(), Encoding.UTF8));

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void Initialize(InitializationContext context)
        {
            //throw new NotImplementedException();
        }
    }
}



//var decorateAttributeType = context.Compilation.GetTypeByMetadataName(typeof(Decorate3Attribute).FullName);


//foreach (var typeName in context.Compilation.Assembly.TypeNames)
//{
//    var type = context.Compilation.Assembly.GetTypeByMetadataName(typeName);

//    if (true/*type.GetAttributes().Any(x => SymbolEqualityComparer.Default.Equals(x.AttributeClass, decorateAttributeType))*/)
//    {
//        // Generate decorator class dynamically

//    }
//}