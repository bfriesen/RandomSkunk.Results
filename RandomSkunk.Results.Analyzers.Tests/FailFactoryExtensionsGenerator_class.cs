using FluentAssertions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using RandomSkunk.Results;

namespace RandomSkunk.Results.Analyzers.Tests
{
    public class FailFactoryExtensionsGenerator_class
    {
        private static string GetGeneratedCode(string inputCode)
        {
            var syntaxTree = CSharpSyntaxTree.ParseText(inputCode);

            var references = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => !a.IsDynamic)
                .Select(a => (MetadataReference)MetadataReference.CreateFromFile(a.Location));

            var compilation = CSharpCompilation.Create(
                nameof(TryCatchGenerator_class),
                new[] { syntaxTree },
                references,
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            var generator = new FailFactoryExtensionsGenerator();

            CSharpGeneratorDriver.Create(generator)
                .RunGeneratorsAndUpdateCompilation(compilation, out var outputCompilation, out var diagnostics);

            outputCompilation.SyntaxTrees.Should().HaveCount(compilation.SyntaxTrees.Length + 1);

            var generatedCode = outputCompilation.SyntaxTrees.Last().ToString();
            return generatedCode;
        }

        [Fact]
        public void Foo()
        {
            const string inputCode = @"namespace RandomSkunk.Results;

[ErrorFactory]
public static class Errors
{
    internal const string BadRequestTitle = ""Bad Request"";
    internal const string BadRequestMessage = ""The operation cannot be processed due to a client error."";

    /// <summary>
    /// Creates an <see cref=""Error""/> indicating that the operation cannot be processed due to a client error.
    /// </summary>
    /// <param name=""errorMessage"">The error message.</param>
    /// <param name=""errorIdentifier"">The optional identifier of the error.</param>
    /// <param name=""isSensitive"">Whether the error contains sensitive information.</param>
    /// <param name=""extensions"">Additional properties for the error.</param>
    /// <param name=""innerError"">The optional <see cref=""Error""/> instance that caused the Bad Request error.</param>
    /// <returns>A Bad Request error.</returns>
    public static Error BadRequest(
        string errorMessage = BadRequestMessage,
        string? errorIdentifier = null,
        bool isSensitive = false,
        IReadOnlyDictionary<string, object>? extensions = null,
        Error? innerError = null) =>
        new()
        {
            Message = errorMessage ?? BadRequestMessage,
            Title = BadRequestTitle,
            Identifier = errorIdentifier,
            ErrorCode = ErrorCodes.BadRequest,
            IsSensitive = isSensitive,
            Extensions = extensions!,
            InnerError = innerError,
        };
}";

            const string expectedGeneratedCode = @"";

            var generatedCode = GetGeneratedCode(inputCode);

            generatedCode.Should().Be(expectedGeneratedCode);
        }
    }
}
