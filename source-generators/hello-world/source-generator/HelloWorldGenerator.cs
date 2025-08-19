using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System.Text;

namespace source_generator;

[Generator]
public class HelloWorldGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        // Register a source output that generates the HelloWorld class
        context.RegisterSourceOutput(
            context.CompilationProvider,
            (sourceProductionContext, compilation) =>
            {
                // Define the source code for the HelloWorld class
                string sourceCode = @"
using System;

namespace GeneratedCode
{
    public class HelloWorld
    {
        public void SayHello(string name)
        {
            Console.WriteLine($""Hello {name}!"");
        }
    }
}";

                // Add the source code to the compilation
                sourceProductionContext.AddSource("HelloWorld.g.cs", SourceText.From(sourceCode, Encoding.UTF8));
            });
    }
}