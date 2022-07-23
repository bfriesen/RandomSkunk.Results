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
                .AppendOnAllSuccessAsyncMethod(tupleCount)
                .AppendOnAnyNonSuccessMethod(tupleCount)
                .AppendOnAnyNonSuccessAsyncMethod(tupleCount)
                .AppendMatchAllMethod(tupleCount)
                .AppendMatchAllAsyncMethod(tupleCount)
                .AppendMapAllMethod(tupleCount)
                .AppendMapAllAsyncMethod(tupleCount)
                .AppendFlatMapAllResultMethod(tupleCount)
                .AppendFlatMapAllAsyncResultMethod(tupleCount)
                .AppendFlatMapAllResultOfTMethod(tupleCount)
                .AppendFlatMapAllAsyncResultOfTMethod(tupleCount)
                .AppendFlatMapAllMaybeOfTMethod(tupleCount)
                .AppendFlatMapAllAsyncMaybeOfTMethod(tupleCount)
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
