# Hello World Source Generator

This project demonstrates the basic usage of C# source generators. 

## How to run

1. Navigate to `console-app/`
1. Execute `dotnet run`

## Notable points

1. The initial source generator interface [has been deprecated](https://github.com/dotnet/roslyn/blob/main/docs/features/source-generators.md) in favor of the [incremental generator interface](https://github.com/dotnet/roslyn/blob/main/docs/features/incremental-generators.md).
1. When referencing source generator projects make sure that you specify `OutputItemType` and `ReferenceOutputAssembly` attributes as follows: `<ProjectReference Include="..\source-generator\source-generator.csproj" OutputItemType="Analyzer" ReferenceOutputAssembly="false" />`

## References

1. [Incremental Generators design documentation](https://github.com/dotnet/roslyn/blob/main/docs/features/incremental-generators.md)
1. [Incremental Generators cookbook](https://github.com/dotnet/roslyn/blob/main/docs/features/incremental-generators.cookbook.md)