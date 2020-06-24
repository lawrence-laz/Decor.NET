using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace SourceGeneratorSamples
{
    [Generator]
    public class HelloWorldGenerator : ISourceGenerator
    {
        public void Execute(SourceGeneratorContext context)
        {


            StringBuilder builder = new StringBuilder();
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

        public void Initialize(InitializationContext context)
        {
            //throw new NotImplementedException();
        }
    }
}
