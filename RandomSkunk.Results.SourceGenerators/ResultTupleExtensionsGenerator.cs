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
        var source = $@"// Auto-generated code

#nullable enable

namespace RandomSkunk.Results;

/// <summary>
/// Defines extension methods for value tuples of results.
/// </summary>
public static class ResultTupleExtensions
{{
{GetMethods()}
}}
";
        context.AddSource("ResultTupleExtensions.g.cs", source);
    }

    /// <inheritdoc/>
    public void Initialize(GeneratorInitializationContext context)
    {
    }

    private static string GetMethods()
    {
        var sb = new StringBuilder();

        for (int tupleCount = 2; tupleCount <= _maxTupleCount; tupleCount++)
        {
            sb.AppendLine($"    #region Tuple {tupleCount}").AppendLine();
            GenerateGenericOnAllSuccess(sb, tupleCount);
            GenerateGenericOnAllSuccessAsync(sb, tupleCount);
            GenerateNonGenericOnAllSuccess(sb, tupleCount);
            GenerateNonGenericOnAllSuccessAsync(sb, tupleCount);
            GenerateOnAnyNonSuccess(sb, tupleCount);
            GenerateOnAnyNonSuccessAsync(sb, tupleCount);
            GenerateGenericMatchAll(sb, tupleCount);
            GenerateGenericMatchAllAsync(sb, tupleCount);
            GenerateNonGenericMatchAll(sb, tupleCount);
            GenerateNonGenericMatchAllAsync(sb, tupleCount);
            sb.AppendLine("    #endregion").AppendLine();
        }

        GenerateGetNonSuccessErrorMethod(sb);
        return sb.ToString();
    }

    private static void GenerateGenericOnAllSuccess(StringBuilder sb, int tupleCount)
    {
        sb.Append(@"    /// <summary>
    /// Invokes the <paramref name=""onAllSuccess""/> function if all results in the tuple are <c>Success</c> results.
    /// </summary>
    /// <typeparam name=""T1"">The type of the first result.</typeparam>");
        GenerateTypeParamDocs(sb, tupleCount);
        sb.Append($@"    /// <param name=""results"">A tuple of results.</param>
    /// <param name=""onAllSuccess"">A callback function to invoke if all results in the tuple are <c>Success</c> results.</param>
    /// <returns>The same tuple of results.</returns>
    /// <exception cref=""ArgumentNullException"">If <paramref name=""onAllSuccess""/> is <see langword=""null""/>.</exception>
    public static ");
        GenerateGenericTupleDefinition(sb, tupleCount);
        sb.Append(" OnAllSuccess");
        GenerateTypeDefinition(sb, tupleCount);
        sb.Append(@"(
        this ");
        GenerateGenericTupleDefinition(sb, tupleCount);
        sb.Append(@" results,
        Action");
        GenerateTypeDefinition(sb, tupleCount);
        sb.Append(@" onAllSuccess)
    {
        if (onAllSuccess is null) throw new ArgumentNullException(nameof(onAllSuccess));

        if (results.Item1.IsSuccess");
        GenerateAndItemNIsSuccessClause(sb, tupleCount);
        sb.Append(@")
        {
            onAllSuccess(
                results.Item1.GetSuccessValue()");
        GenerateItemNGetSuccessValueParameter(sb, tupleCount);
        sb.Append(@");
        }

        return results;
    }

");
    }

    private static void GenerateGenericOnAllSuccessAsync(StringBuilder sb, int tupleCount)
    {
        sb.Append(@"    /// <summary>
    /// Invokes the <paramref name=""onAllSuccess""/> function if all results in the tuple are <c>Success</c> results.
    /// </summary>
    /// <typeparam name=""T1"">The type of the first result.</typeparam>");
        GenerateTypeParamDocs(sb, tupleCount);
        sb.Append(@"    /// <param name=""results"">A tuple of results.</param>
    /// <param name=""onAllSuccess"">A callback function to invoke if all results in the tuple are <c>Success</c> results.</param>
    /// <returns>The same tuple of results.</returns>
    /// <exception cref=""ArgumentNullException"">If <paramref name=""onAllSuccess""/> is <see langword=""null""/>.</exception>
    public static async Task<");
        GenerateGenericTupleDefinition(sb, tupleCount);
        sb.Append("> OnAllSuccessAsync");
        GenerateTypeDefinition(sb, tupleCount);
        sb.Append(@"(
        this ");
        GenerateGenericTupleDefinition(sb, tupleCount);
        sb.Append(@" results,
        Func<");
        GenerateTypeDefinitionArguments(sb, tupleCount);
        sb.Append(@", Task> onAllSuccess)
    {
        if (onAllSuccess is null) throw new ArgumentNullException(nameof(onAllSuccess));

        if (results.Item1.IsSuccess");
        GenerateAndItemNIsSuccessClause(sb, tupleCount);
        sb.Append(@")
        {
            await onAllSuccess(
                results.Item1.GetSuccessValue()");
        GenerateItemNGetSuccessValueParameter(sb, tupleCount);
        sb.Append(@").ConfigureAwait(false);
        }

        return results;
    }

");
    }

    private static void GenerateNonGenericOnAllSuccess(StringBuilder sb, int tupleCount)
    {
        sb.Append(@"    /// <summary>
    /// Invokes the <paramref name=""onAllSuccess""/> function if all results in the tuple are <c>Success</c> results.
    /// </summary>
    /// <param name=""results"">A tuple of results.</param>
    /// <param name=""onAllSuccess"">A callback function to invoke if all results in the tuple are <c>Success</c> results.</param>
    /// <returns>The same tuple of results.</returns>
    /// <exception cref=""ArgumentNullException"">If <paramref name=""onAllSuccess""/> is <see langword=""null""/>.</exception>
    public static ");
        GenerateNonGenericTupleDefinition(sb, tupleCount);
        sb.Append(@" OnAllSuccess(
        this ");
        GenerateNonGenericTupleDefinition(sb, tupleCount);
        sb.Append(@" results,
        Action<");
        GenerateObjectParameters(sb, tupleCount);
        sb.Append(@"> onAllSuccess)
    {
        if (onAllSuccess is null) throw new ArgumentNullException(nameof(onAllSuccess));

        if (results.Item1.IsSuccess");
        GenerateAndItemNIsSuccessClause(sb, tupleCount);
        sb.Append(@")
        {
            onAllSuccess(
                results.Item1.GetSuccessValue()");
        GenerateItemNGetSuccessValueParameter(sb, tupleCount);
        sb.Append(@");
        }

        return results;
    }

");
    }

    private static void GenerateNonGenericOnAllSuccessAsync(StringBuilder sb, int tupleCount)
    {
        sb.Append(@"    /// <summary>
    /// Invokes the <paramref name=""onAllSuccess""/> function if all results in the tuple are <c>Success</c> results.
    /// </summary>
    /// <param name=""results"">A tuple of results.</param>
    /// <param name=""onAllSuccess"">A callback function to invoke if all results in the tuple are <c>Success</c> results.</param>
    /// <returns>The same tuple of results.</returns>
    /// <exception cref=""ArgumentNullException"">If <paramref name=""onAllSuccess""/> is <see langword=""null""/>.</exception>
    public static async Task<");
        GenerateNonGenericTupleDefinition(sb, tupleCount);
        sb.Append(@"> OnAllSuccessAsync(
        this ");
        GenerateNonGenericTupleDefinition(sb, tupleCount);
        sb.Append(@" results,
        Func<");
        GenerateObjectParameters(sb, tupleCount);
        sb.Append(@", Task> onAllSuccess)
    {
        if (onAllSuccess is null) throw new ArgumentNullException(nameof(onAllSuccess));

        if (results.Item1.IsSuccess");
        GenerateAndItemNIsSuccessClause(sb, tupleCount);
        sb.Append(@")
        {
            await onAllSuccess(
                results.Item1.GetSuccessValue()");
        GenerateItemNGetSuccessValueParameter(sb, tupleCount);
        sb.Append(@").ConfigureAwait(false);
        }

        return results;
    }

");
    }

    private static void GenerateOnAnyNonSuccess(StringBuilder sb, int tupleCount)
    {
        sb.Append(@"    /// <summary>
    /// Invokes the <paramref name=""onAnyNonSuccess""/> function if any results in the tuple are <c>non-Success</c> results.
    /// </summary>
    /// <param name=""results"">A tuple of results.</param>
    /// <param name=""onAnyNonSuccess"">A callback function to invoke if any results in the tuple are <c>non-Success</c> results.
    ///     </param>
    /// <param name=""getNoneError"">An optional function that creates the <see cref=""Error""/> for any <c>None</c> results
    ///     (otherwise not applicable). If <see langword=""null""/> (and applicable), a function that returns an error with message
    ///     ""Not Found"" and error code <see cref=""ErrorCodes.NotFound""/> is used instead.</param>
    /// <returns>The same tuple of results.</returns>
    /// <exception cref=""ArgumentNullException"">If <paramref name=""onAnyNonSuccess""/> is <see langword=""null""/>.</exception>
    public static ");
        GenerateNonGenericTupleDefinition(sb, tupleCount);
        sb.Append(@" OnAnyNonSuccess(
        this ");
        GenerateNonGenericTupleDefinition(sb, tupleCount);
        sb.Append(@" results,
        Action<Error> onAnyNonSuccess,
        Func<Error>? getNoneError = null)
    {
        if (onAnyNonSuccess is null) throw new ArgumentNullException(nameof(onAnyNonSuccess));

        if (!results.Item1.IsSuccess");
        GenerateOrItemNIsNotSuccessClause(sb, tupleCount);
        sb.Append(@")
        {
            var error = GetNonSuccessError(
                getNoneError,
                results.Item1");
        GenerateItemNParameters(sb, tupleCount);
        sb.Append(@");
            onAnyNonSuccess(error);
        }

        return results;
    }

");
    }

    private static void GenerateOnAnyNonSuccessAsync(StringBuilder sb, int tupleCount)
    {
        sb.Append(@"    /// <summary>
    /// Invokes the <paramref name=""onAnyNonSuccess""/> function if any results in the tuple are <c>non-Success</c> results.
    /// </summary>
    /// <param name=""results"">A tuple of results.</param>
    /// <param name=""onAnyNonSuccess"">A callback function to invoke if any results in the tuple are <c>non-Success</c> results.
    ///     </param>
    /// <param name=""getNoneError"">An optional function that creates the <see cref=""Error""/> for any <c>None</c> results
    ///     (otherwise not applicable). If <see langword=""null""/> (and applicable), a function that returns an error with message
    ///     ""Not Found"" and error code <see cref=""ErrorCodes.NotFound""/> is used instead.</param>
    /// <returns>The same tuple of results.</returns>
    /// <exception cref=""ArgumentNullException"">If <paramref name=""onAnyNonSuccess""/> is <see langword=""null""/>.</exception>
    public static async Task<");
        GenerateNonGenericTupleDefinition(sb, tupleCount);
        sb.Append(@"> OnAnyNonSuccessAsync(
        this ");
        GenerateNonGenericTupleDefinition(sb, tupleCount);
        sb.Append(@" results,
        Func<Error, Task> onAnyNonSuccess,
        Func<Error>? getNoneError = null)
    {
        if (onAnyNonSuccess is null) throw new ArgumentNullException(nameof(onAnyNonSuccess));

        if (!results.Item1.IsSuccess");
        GenerateOrItemNIsNotSuccessClause(sb, tupleCount);
        sb.Append(@")
        {
            var error = GetNonSuccessError(
                getNoneError,
                results.Item1");
        GenerateItemNParameters(sb, tupleCount);
        sb.Append(@");
            await onAnyNonSuccess(error).ConfigureAwait(false);
        }

        return results;
    }

");
    }

    private static void GenerateGenericMatchAll(StringBuilder sb, int tupleCount)
    {
        sb.Append(@"    /// <summary>
    /// Evaluates either the <paramref name=""onAllSuccess""/> or <paramref name=""onAnyNonSuccess""/> function depending on
    /// whether all results in the tuple are <c>Success</c> or not.
    /// </summary>
    /// <typeparam name=""T1"">The type of the first result.</typeparam>");
        GenerateTypeParamDocs(sb, tupleCount);
        sb.Append(@"    /// <typeparam name=""TReturn"">The return type of the match all method.</typeparam>
    /// <param name=""results"">A tuple of results.</param>
    /// <param name=""onAllSuccess"">The function to evaluate if all results are <c>Success</c>. The non-null values of each of the
    ///     <c>Success</c> results are passed to this function.</param>
    /// <param name=""onAnyNonSuccess"">The function to evaluate if any results are <c>non-Success</c>. The error passed to this
    ///     function depends on how many results are <c>non-Success</c>. If only one is <c>non-Success</c>, then its error is
    ///     passed to this function. If more than one result is <c>non-Success</c>, then a <see cref=""CompositeError""/> is
    ///     returned containing the error of each <c>non-Success</c> result.</param>
    /// <param name=""getNoneError"">An optional function that creates the <see cref=""Error""/> for any <c>None</c> results
    ///     (otherwise not applicable). If <see langword=""null""/> (and applicable), a function that returns an error with message
    ///     ""Not Found"" and error code <see cref=""ErrorCodes.NotFound""/> is used instead.</param>
    /// <returns>The result of the matching function evaluation.</returns>
    /// <exception cref=""ArgumentNullException"">If <paramref name=""onAllSuccess""/> or <paramref name=""onAnyNonSuccess""/> is
    ///     <see langword=""null""/>.</exception>
    public static TReturn MatchAll<");
        GenerateTypeDefinitionArguments(sb, tupleCount);
        sb.Append(@", TReturn>(
        this ");
        GenerateGenericTupleDefinition(sb, tupleCount);
        sb.Append(@" results,
        Func<");
        GenerateTypeDefinitionArguments(sb, tupleCount);
        sb.Append(@", TReturn> onAllSuccess,
        Func<Error, TReturn> onAnyNonSuccess,
        Func<Error>? getNoneError = null)
    {
        if (onAllSuccess is null) throw new ArgumentNullException(nameof(onAllSuccess));
        if (onAnyNonSuccess is null) throw new ArgumentNullException(nameof(onAnyNonSuccess));

        if (results.Item1.IsSuccess");
        GenerateAndItemNIsSuccessClause(sb, tupleCount);
        sb.Append(@")
        {
            return onAllSuccess(
                results.Item1.GetSuccessValue()");
        GenerateItemNGetSuccessValueParameter(sb, tupleCount);
        sb.Append(@");
        }
        else
        {
            var error = GetNonSuccessError(
                getNoneError,
                results.Item1");
        GenerateItemNParameters(sb, tupleCount);
        sb.Append(@");
            return onAnyNonSuccess(error);
        }
    }

");
    }

    private static void GenerateGenericMatchAllAsync(StringBuilder sb, int tupleCount)
    {
        sb.Append(@"    /// <summary>
    /// Evaluates either the <paramref name=""onAllSuccess""/> or <paramref name=""onAnyNonSuccess""/> function depending on
    /// whether all results in the tuple are <c>Success</c> or not.
    /// </summary>
    /// <typeparam name=""T1"">The type of the first result.</typeparam>");
        GenerateTypeParamDocs(sb, tupleCount);
        sb.Append(@"    /// <typeparam name=""TReturn"">The return type of the match all method.</typeparam>
    /// <param name=""results"">A tuple of results.</param>
    /// <param name=""onAllSuccess"">The function to evaluate if all results are <c>Success</c>. The non-null values of each of the
    ///     <c>Success</c> results are passed to this function.</param>
    /// <param name=""onAnyNonSuccess"">The function to evaluate if any results are <c>non-Success</c>. The error passed to this
    ///     function depends on how many results are <c>non-Success</c>. If only one is <c>non-Success</c>, then its error is
    ///     passed to this function. If more than one result is <c>non-Success</c>, then a <see cref=""CompositeError""/> is
    ///     returned containing the error of each <c>non-Success</c> result.</param>
    /// <param name=""getNoneError"">An optional function that creates the <see cref=""Error""/> for any <c>None</c> results
    ///     (otherwise not applicable). If <see langword=""null""/> (and applicable), a function that returns an error with message
    ///     ""Not Found"" and error code <see cref=""ErrorCodes.NotFound""/> is used instead.</param>
    /// <returns>The result of the matching function evaluation.</returns>
    /// <exception cref=""ArgumentNullException"">If <paramref name=""onAllSuccess""/> or <paramref name=""onAnyNonSuccess""/> is
    ///     <see langword=""null""/>.</exception>
    public static Task<TReturn> MatchAllAsync<");
        GenerateTypeDefinitionArguments(sb, tupleCount);
        sb.Append(@", TReturn>(
        this ");
        GenerateGenericTupleDefinition(sb, tupleCount);
        sb.Append(@" results,
        Func<");
        GenerateTypeDefinitionArguments(sb, tupleCount);
        sb.Append(@", Task<TReturn>> onAllSuccess,
        Func<Error, Task<TReturn>> onAnyNonSuccess,
        Func<Error>? getNoneError = null)
    {
        if (onAllSuccess is null) throw new ArgumentNullException(nameof(onAllSuccess));
        if (onAnyNonSuccess is null) throw new ArgumentNullException(nameof(onAnyNonSuccess));

        if (results.Item1.IsSuccess");
        GenerateAndItemNIsSuccessClause(sb, tupleCount);
        sb.Append(@")
        {
            return onAllSuccess(
                results.Item1.GetSuccessValue()");
        GenerateItemNGetSuccessValueParameter(sb, tupleCount);
        sb.Append(@");
        }
        else
        {
            var error = GetNonSuccessError(
                getNoneError,
                results.Item1");
        GenerateItemNParameters(sb, tupleCount);
        sb.Append(@");
            return onAnyNonSuccess(error);
        }
    }

");
    }

    private static void GenerateNonGenericMatchAll(StringBuilder sb, int tupleCount)
    {
        sb.Append(@"    /// <summary>
    /// Evaluates either the <paramref name=""onAllSuccess""/> or <paramref name=""onAnyNonSuccess""/> function depending on
    /// whether all results in the tuple are <c>Success</c> or not.
    /// </summary>
    /// <typeparam name=""TReturn"">The return type of the match all method.</typeparam>
    /// <param name=""results"">A tuple of results.</param>
    /// <param name=""onAllSuccess"">The function to evaluate if all results are <c>Success</c>. The non-null values of each of the
    ///     <c>Success</c> results are passed to this function.</param>
    /// <param name=""onAnyNonSuccess"">The function to evaluate if any results are <c>non-Success</c>. The error passed to this
    ///     function depends on how many results are <c>non-Success</c>. If only one is <c>non-Success</c>, then its error is
    ///     passed to this function. If more than one result is <c>non-Success</c>, then a <see cref=""CompositeError""/> is
    ///     returned containing the error of each <c>non-Success</c> result.</param>
    /// <param name=""getNoneError"">An optional function that creates the <see cref=""Error""/> for any <c>None</c> results
    ///     (otherwise not applicable). If <see langword=""null""/> (and applicable), a function that returns an error with message
    ///     ""Not Found"" and error code <see cref=""ErrorCodes.NotFound""/> is used instead.</param>
    /// <returns>The result of the matching function evaluation.</returns>
    /// <exception cref=""ArgumentNullException"">If <paramref name=""onAllSuccess""/> or <paramref name=""onAnyNonSuccess""/> is
    ///     <see langword=""null""/>.</exception>
    public static TReturn MatchAll<TReturn>(
        this ");
        GenerateNonGenericTupleDefinition(sb, tupleCount);
        sb.Append(@" results,
        Func<");
        GenerateObjectParameters(sb, tupleCount);
        sb.Append(@", TReturn> onAllSuccess,
        Func<Error, TReturn> onAnyNonSuccess,
        Func<Error>? getNoneError = null)
    {
        if (onAllSuccess is null) throw new ArgumentNullException(nameof(onAllSuccess));
        if (onAnyNonSuccess is null) throw new ArgumentNullException(nameof(onAnyNonSuccess));

        if (results.Item1.IsSuccess");
        GenerateAndItemNIsSuccessClause(sb, tupleCount);
        sb.Append(@")
        {
            return onAllSuccess(
                results.Item1.GetSuccessValue()");
        GenerateItemNGetSuccessValueParameter(sb, tupleCount);
        sb.Append(@");
        }
        else
        {
            var error = GetNonSuccessError(
                getNoneError,
                results.Item1");
        GenerateItemNParameters(sb, tupleCount);
        sb.Append(@");
            return onAnyNonSuccess(error);
        }
    }

");
    }

    private static void GenerateNonGenericMatchAllAsync(StringBuilder sb, int tupleCount)
    {
        sb.Append(@"    /// <summary>
    /// Evaluates either the <paramref name=""onAllSuccess""/> or <paramref name=""onAnyNonSuccess""/> function depending on
    /// whether all results in the tuple are <c>Success</c> or not.
    /// </summary>
    /// <typeparam name=""TReturn"">The return type of the match all method.</typeparam>
    /// <param name=""results"">A tuple of results.</param>
    /// <param name=""onAllSuccess"">The function to evaluate if all results are <c>Success</c>. The non-null values of each of the
    ///     <c>Success</c> results are passed to this function.</param>
    /// <param name=""onAnyNonSuccess"">The function to evaluate if any results are <c>non-Success</c>. The error passed to this
    ///     function depends on how many results are <c>non-Success</c>. If only one is <c>non-Success</c>, then its error is
    ///     passed to this function. If more than one result is <c>non-Success</c>, then a <see cref=""CompositeError""/> is
    ///     returned containing the error of each <c>non-Success</c> result.</param>
    /// <param name=""getNoneError"">An optional function that creates the <see cref=""Error""/> for any <c>None</c> results
    ///     (otherwise not applicable). If <see langword=""null""/> (and applicable), a function that returns an error with message
    ///     ""Not Found"" and error code <see cref=""ErrorCodes.NotFound""/> is used instead.</param>
    /// <returns>The result of the matching function evaluation.</returns>
    /// <exception cref=""ArgumentNullException"">If <paramref name=""onAllSuccess""/> or <paramref name=""onAnyNonSuccess""/> is
    ///     <see langword=""null""/>.</exception>
    public static Task<TReturn> MatchAllAsync<TReturn>(
        this ");
        GenerateNonGenericTupleDefinition(sb, tupleCount);
        sb.Append(@" results,
        Func<");
        GenerateObjectParameters(sb, tupleCount);
        sb.Append(@", Task<TReturn>> onAllSuccess,
        Func<Error, Task<TReturn>> onAnyNonSuccess,
        Func<Error>? getNoneError = null)
    {
        if (onAllSuccess is null) throw new ArgumentNullException(nameof(onAllSuccess));
        if (onAnyNonSuccess is null) throw new ArgumentNullException(nameof(onAnyNonSuccess));

        if (results.Item1.IsSuccess");
        GenerateAndItemNIsSuccessClause(sb, tupleCount);
        sb.Append(@")
        {
            return onAllSuccess(
                results.Item1.GetSuccessValue()");
        GenerateItemNGetSuccessValueParameter(sb, tupleCount);
        sb.Append(@");
        }
        else
        {
            var error = GetNonSuccessError(
                getNoneError,
                results.Item1");
        GenerateItemNParameters(sb, tupleCount);
        sb.Append(@");
            return onAnyNonSuccess(error);
        }
    }

");
    }

    private static void GenerateTypeParamDocs(StringBuilder sb, int tupleCount)
    {
        for (int tupleNumber = 2; tupleNumber <= tupleCount; tupleNumber++)
        {
            sb.Append($@"
    /// <typeparam name=""T{tupleNumber}"">The type of the {GetOrdinal(tupleNumber)} result in the tuple.</typeparam>");
        }

        sb.AppendLine();
    }

    private static void GenerateGenericTupleDefinition(StringBuilder sb, int tupleCount)
    {
        sb.Append("(IResult<T1>, ");

        for (int tupleNumber = 2; tupleNumber <= tupleCount; tupleNumber++)
        {
            sb.Append($"IResult<T{tupleNumber}>");
            if (tupleNumber != tupleCount)
                sb.Append(", ");
        }

        sb.Append(")");
    }

    private static void GenerateNonGenericTupleDefinition(StringBuilder sb, int tupleCount)
    {
        sb.Append("(IResult, ");

        for (int tupleNumber = 2; tupleNumber <= tupleCount; tupleNumber++)
        {
            sb.Append("IResult");
            if (tupleNumber != tupleCount)
                sb.Append(", ");
        }

        sb.Append(")");
    }

    private static void GenerateTypeDefinition(StringBuilder sb, int tupleCount)
    {
        sb.Append("<");
        GenerateTypeDefinitionArguments(sb, tupleCount);
        sb.Append(">");
    }

    private static void GenerateTypeDefinitionArguments(StringBuilder sb, int tupleCount)
    {
        sb.Append("T1, ");

        for (int tupleNumber = 2; tupleNumber <= tupleCount; tupleNumber++)
        {
            sb.Append($"T{tupleNumber}");
            if (tupleNumber != tupleCount)
                sb.Append(", ");
        }
    }

    private static void GenerateObjectParameters(StringBuilder sb, int tupleCount)
    {
        sb.Append("object, ");

        for (int tupleNumber = 2; tupleNumber <= tupleCount; tupleNumber++)
        {
            sb.Append("object");
            if (tupleNumber != tupleCount)
                sb.Append(", ");
        }
    }

    private static void GenerateAndItemNIsSuccessClause(StringBuilder sb, int tupleCount)
    {
        for (int tupleNumber = 2; tupleNumber <= tupleCount; tupleNumber++)
        {
            sb.Append($@"
            && results.Item{tupleNumber}.IsSuccess");
        }
    }

    private static void GenerateOrItemNIsNotSuccessClause(StringBuilder sb, int tupleCount)
    {
        for (int tupleNumber = 2; tupleNumber <= tupleCount; tupleNumber++)
        {
            sb.Append($@"
            || !results.Item{tupleNumber}.IsSuccess");
        }
    }

    private static void GenerateItemNGetSuccessValueParameter(StringBuilder sb, int tupleCount)
    {
        for (int tupleNumber = 2; tupleNumber <= tupleCount; tupleNumber++)
        {
            sb.Append($@",
                results.Item{tupleNumber}.GetSuccessValue()");
        }
    }

    private static void GenerateItemNParameters(StringBuilder sb, int tupleCount)
    {
        for (int tupleNumber = 2; tupleNumber <= tupleCount; tupleNumber++)
        {
            sb.Append($@",
                results.Item{tupleNumber}");
        }
    }

    private static void GenerateGetNonSuccessErrorMethod(StringBuilder sb)
    {
        sb.Append(@"    private static Error GetNonSuccessError(Func<Error>? getNoneError, params IResult[] results)
    {
        var errors = results
            .Where(r => !r.IsSuccess)
            .Select(r => r.GetNonSuccessError(getNoneError));
        return CompositeError.Create(errors);
    }");
    }

    private static string GetOrdinal(int tupleNumber)
    {
        return tupleNumber switch
        {
            2 => "second",
            3 => "third",
            4 => "fourth",
            5 => "fifth",
            6 => "sixth",
            7 => "seventh",
            8 => "eighth",
            9 => "ninth",
            10 => "tenth",
            11 => "eleventh",
            12 => "twelfth",
            13 => "thirteenth",
            14 => "fourteenth",
            15 => "fifteenth",
            16 => "sixteenth",
            _ => (tupleNumber % 10) switch
            {
                1 => tupleNumber + "st",
                2 => tupleNumber + "nd",
                3 => tupleNumber + "rd",
                _ => tupleNumber + "th",
            },
        };
    }
}
