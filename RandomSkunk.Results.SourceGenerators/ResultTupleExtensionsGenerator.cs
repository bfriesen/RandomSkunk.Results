using Microsoft.CodeAnalysis;
using System.Text;

namespace RandomSkunk.Results.SourceGenerators;

/// <summary>
/// Defines a source generator that creates extension methods for result tuples.
/// </summary>
[Generator]
public class ResultTupleExtensionsGenerator : ISourceGenerator
{
    private const int _maxTupleCount = 16;

    /// <inheritdoc/>
    public void Execute(GeneratorExecutionContext context)
    {
        var code = new StringBuilder(400000)
            .AppendBeginClassDefinition();

        for (int tupleCount = 2; tupleCount <= _maxTupleCount; tupleCount++)
        {
            code.AppendBeginRegion(tupleCount)
                .AppendOnAllSuccessMethod(tupleCount)
                .AppendAsyncOnAllSuccessMethod(tupleCount)
                .AppendOnAnyNonSuccessMethod(tupleCount)
                .AppendAsyncOnAnyNonSuccessMethod(tupleCount)
                .AppendMatchMethod(tupleCount)
                .AppendAsyncMatchMethod(tupleCount)
                .AppendSelectMethod(tupleCount)
                .AppendAsyncSelectMethod(tupleCount)
                .AppendSelectManyResultMethod(tupleCount)
                .AppendAsyncSelectManyResultMethod(tupleCount)
                .AppendSelectManyResultOfTMethod(tupleCount)
                .AppendAsyncSelectManyResultOfTMethod(tupleCount)
                .AppendSelectManyMaybeOfTMethod(tupleCount)
                .AppendAsyncSelectManyMaybeOfTMethod(tupleCount)
                .AppendEndRegion();
        }

        code.AppendGetNonSuccessErrorMethod()
            .AppendEndClassDefinition();

        context.AddSource("ResultTupleExtensions.g.cs", code.ToString());
    }

    /// <inheritdoc/>
    public void Initialize(GeneratorInitializationContext context)
    {
    }
}
