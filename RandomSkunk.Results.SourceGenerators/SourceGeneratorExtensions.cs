using System.Text;

namespace RandomSkunk.Results.SourceGenerators;

internal static class SourceGeneratorExtensions
{
    public static StringBuilder AppendBeginClassDefinition(this StringBuilder code) =>
        code.Append(@"// Auto-generated code

#nullable enable

namespace RandomSkunk.Results;

/// <summary>
/// Defines extension methods for value tuples of results.
/// </summary>
public static class ResultTupleExtensions
{
");

    public static StringBuilder AppendBeginRegion(this StringBuilder code, int tupleCount) =>
        code.AppendLine($"    #region {tupleCount}-tuple").AppendLine();

    public static StringBuilder AppendOnAllSuccessMethod(this StringBuilder code, int tupleCount) =>
        code.Append(@"    /// <summary>
    /// Invokes the <paramref name=""onAllSuccess""/> function if all results in the tuple are <c>Success</c> results.
    /// </summary>
    /// <typeparam name=""T1"">The type of the tuple's first result.</typeparam>")
            .AppendTypeParamDocsForT(tupleCount)
            .Append($@"    /// <param name=""sourceResults"">A tuple of results.</param>
    /// <param name=""onAllSuccess"">A callback function to invoke if all results in the tuple are <c>Success</c> results.</param>
    /// <returns>The same tuple of results.</returns>
    /// <exception cref=""ArgumentNullException"">If <paramref name=""onAllSuccess""/> is <see langword=""null""/> or if any of
    ///     the <paramref name=""sourceResults""/> tuple's items are <see langword=""null""/>.</exception>
    public static ")
            .AppendTupleDefinitionForT(tupleCount)
            .Append(" OnAllSuccess")
            .AppendTypeDefinitionForT(tupleCount)
            .Append(@"(
        this ")
            .AppendTupleDefinitionForT(tupleCount)
            .Append(@" sourceResults,
        Action")
            .AppendTypeDefinitionForT(tupleCount)
            .Append(@" onAllSuccess)
    {
        if (onAllSuccess is null) throw new ArgumentNullException(nameof(onAllSuccess));

        if (sourceResults.Item1 is null) throw new ArgumentNullException($""{nameof(sourceResults)}.Item1"");")
        .AppendItemNNullChecks(tupleCount)
        .Append(@"
        if (sourceResults.Item1.IsSuccess")
            .AppendAndItemNIsSuccessClauses(tupleCount)
            .Append(@")
        {
            onAllSuccess(
                sourceResults.Item1.GetSuccessValue()")
            .AppendItemNGetSuccessValueParameters(tupleCount)
            .Append(@");
        }

        return sourceResults;
    }

");

    public static StringBuilder AppendOnAllSuccessAsyncMethod(this StringBuilder code, int tupleCount) =>
        code.Append(@"    /// <summary>
    /// Invokes the <paramref name=""onAllSuccess""/> function if all results in the tuple are <c>Success</c> results.
    /// </summary>
    /// <typeparam name=""T1"">The type of the tuple's first result.</typeparam>")
            .AppendTypeParamDocsForT(tupleCount)
            .Append(@"    /// <param name=""sourceResults"">A tuple of results.</param>
    /// <param name=""onAllSuccess"">A callback function to invoke if all results in the tuple are <c>Success</c> results.</param>
    /// <returns>The same tuple of results.</returns>
    /// <exception cref=""ArgumentNullException"">If <paramref name=""onAllSuccess""/> is <see langword=""null""/> or if any of
    ///     the <paramref name=""sourceResults""/> tuple's items are <see langword=""null""/>.</exception>
    public static async Task<")
            .AppendTupleDefinitionForT(tupleCount)
            .Append("> OnAllSuccessAsync")
            .AppendTypeDefinitionForT(tupleCount)
            .Append(@"(
        this ")
            .AppendTupleDefinitionForT(tupleCount)
            .Append(@" sourceResults,
        Func<")
            .AppendTypeDefinitionArgumentsForT(tupleCount)
            .Append(@", Task> onAllSuccess)
    {
        if (onAllSuccess is null) throw new ArgumentNullException(nameof(onAllSuccess));

        if (sourceResults.Item1 is null) throw new ArgumentNullException($""{nameof(sourceResults)}.Item1"");")
        .AppendItemNNullChecks(tupleCount)
        .Append(@"
        if (sourceResults.Item1.IsSuccess")
            .AppendAndItemNIsSuccessClauses(tupleCount)
            .Append(@")
        {
            await onAllSuccess(
                sourceResults.Item1.GetSuccessValue()")
            .AppendItemNGetSuccessValueParameters(tupleCount)
            .Append(@").ConfigureAwait(false);
        }

        return sourceResults;
    }

");

    public static StringBuilder AppendOnAnyNonSuccessMethod(this StringBuilder code, int tupleCount) =>
        code.Append(@"    /// <summary>
    /// Invokes the <paramref name=""onAnyNonSuccess""/> function if any results in the tuple are <c>non-Success</c> results.
    /// </summary>
    /// <typeparam name=""TResult1"">The type of the tuple's first result.</typeparam>")
            .AppendTypeParamDocsForTResult(tupleCount)
            .Append($@"    /// <param name=""sourceResults"">A tuple of results.</param>
    /// <param name=""onAnyNonSuccess"">A callback function to invoke if any results in the tuple are <c>non-Success</c> results.
    ///     </param>
    /// <param name=""getNoneError"">An optional function that creates the <see cref=""Error""/> for a <c>None</c> result. If null
    ///     or not provided, a function that returns an error with error code <see cref=""ErrorCodes.NotFound""/> is used instead.
    ///     </param>
    /// <returns>The same tuple of results.</returns>
    /// <exception cref=""ArgumentNullException"">If <paramref name=""onAnyNonSuccess""/> is <see langword=""null""/> or if any of
    ///     the <paramref name=""sourceResults""/> tuple's items are <see langword=""null""/>.</exception>
    public static ")
            .AppendTupleDefinitionForTResult(tupleCount)
            .Append(@" OnAnyNonSuccess")
            .AppendTypeDefinitionForTResult(tupleCount)
            .Append(@"(
        this ")
            .AppendTupleDefinitionForTResult(tupleCount)
            .Append(@" sourceResults,
        Action<Error> onAnyNonSuccess,
        Func<Error>? getNoneError = null)
        where TResult1 : IResult")
            .AppendGenericConstraintsForTResult(tupleCount)
            .Append(@"
    {
        if (onAnyNonSuccess is null) throw new ArgumentNullException(nameof(onAnyNonSuccess));

        if (sourceResults.Item1 is null) throw new ArgumentNullException($""{nameof(sourceResults)}.Item1"");")
        .AppendItemNNullChecks(tupleCount)
        .Append(@"
        if (!sourceResults.Item1.IsSuccess")
            .AppendOrItemNIsNotSuccessClauses(tupleCount)
            .Append(@")
        {
            var error = GetNonSuccessError(
                getNoneError,
                sourceResults.Item1")
            .AppendItemNParameters(tupleCount)
            .Append(@");

            onAnyNonSuccess(error);
        }

        return sourceResults;
    }

");

    public static StringBuilder AppendOnAnyNonSuccessAsyncMethod(this StringBuilder code, int tupleCount)
    {
        return code.Append(@"    /// <summary>
    /// Invokes the <paramref name=""onAnyNonSuccess""/> function if any results in the tuple are <c>non-Success</c> results.
    /// </summary>
    /// <typeparam name=""TResult1"">The type of the tuple's first result.</typeparam>")
        .AppendTypeParamDocsForTResult(tupleCount)
            .Append($@"    /// <param name=""sourceResults"">A tuple of results.</param>
    /// <param name=""onAnyNonSuccess"">A callback function to invoke if any results in the tuple are <c>non-Success</c> results.
    ///     </param>
    /// <param name=""getNoneError"">An optional function that creates the <see cref=""Error""/> for a <c>None</c> result. If null
    ///     or not provided, a function that returns an error with error code <see cref=""ErrorCodes.NotFound""/> is used instead.
    ///     </param>
    /// <returns>The same tuple of results.</returns>
    /// <exception cref=""ArgumentNullException"">If <paramref name=""onAnyNonSuccess""/> is <see langword=""null""/> or if any of
    ///     the <paramref name=""sourceResults""/> tuple's items are <see langword=""null""/>.</exception>
    public static async Task<")
      .AppendTupleDefinitionForTResult(tupleCount)
            .Append(@"> OnAnyNonSuccessAsync")
      .AppendTypeDefinitionForTResult(tupleCount)
            .Append(@"(
        this ")
      .AppendTupleDefinitionForTResult(tupleCount)
            .Append(@" sourceResults,
        Func<Error, Task> onAnyNonSuccess,
        Func<Error>? getNoneError = null)
        where TResult1 : IResult")
      .AppendGenericConstraintsForTResult(tupleCount)
            .Append(@"
    {
        if (onAnyNonSuccess is null) throw new ArgumentNullException(nameof(onAnyNonSuccess));

        if (sourceResults.Item1 is null) throw new ArgumentNullException($""{nameof(sourceResults)}.Item1"");")
        .AppendItemNNullChecks(tupleCount)
        .Append(@"
        if (!sourceResults.Item1.IsSuccess")
      .AppendOrItemNIsNotSuccessClauses(tupleCount)
            .Append(@")
        {
            var error = GetNonSuccessError(
                getNoneError,
                sourceResults.Item1")
      .AppendItemNParameters(tupleCount)
            .Append(@");

            await onAnyNonSuccess(error).ConfigureAwait(false);
        }

        return sourceResults;
    }

");
    }

    public static StringBuilder AppendMatchAllMethod(this StringBuilder code, int tupleCount)
    {
        return code.Append(@"    /// <summary>
    /// Evaluates either the <paramref name=""onAllSuccess""/> or <paramref name=""onAnyNonSuccess""/> function depending on
    /// whether all results in the tuple are <c>Success</c> or not.
    /// </summary>
    /// <typeparam name=""T1"">The type of the tuple's first result.</typeparam>")
            .AppendTypeParamDocsForT(tupleCount)
            .Append(@"    /// <typeparam name=""TReturn"">The return type of the match all method.</typeparam>
    /// <param name=""sourceResults"">A tuple of results.</param>
    /// <param name=""onAllSuccess"">The function to evaluate if all results are <c>Success</c>. The non-null values of each of the
    ///     <c>Success</c> results are passed to this function.</param>
    /// <param name=""onAnyNonSuccess"">The function to evaluate if any results are <c>non-Success</c>. The error passed to this
    ///     function depends on how many results are <c>non-Success</c>. If only one is <c>non-Success</c>, then its error is
    ///     passed to this function. If more than one result is <c>non-Success</c>, then a <see cref=""CompositeError""/> is
    ///     returned containing the error of each <c>non-Success</c> result.</param>
    /// <param name=""getNoneError"">An optional function that creates the <see cref=""Error""/> for a <c>None</c> result. If null
    ///     or not provided, a function that returns an error with error code <see cref=""ErrorCodes.NotFound""/> is used instead.
    ///     </param>
    /// <returns>The result of the matching function evaluation.</returns>
    /// <exception cref=""ArgumentNullException"">If <paramref name=""onAllSuccess""/> or <paramref name=""onAnyNonSuccess""/> is
    ///     <see langword=""null""/> or if any of the <paramref name=""sourceResults""/> tuple's items are <see langword=""null""/>.
    ///     </exception>
    public static TReturn MatchAll<")
            .AppendTypeDefinitionArgumentsForT(tupleCount)
            .Append(@", TReturn>(
        this ")
            .AppendTupleDefinitionForT(tupleCount)
            .Append(@" sourceResults,
        Func<")
            .AppendTypeDefinitionArgumentsForT(tupleCount)
            .Append(@", TReturn> onAllSuccess,
        Func<Error, TReturn> onAnyNonSuccess,
        Func<Error>? getNoneError = null)
    {
        if (onAllSuccess is null) throw new ArgumentNullException(nameof(onAllSuccess));
        if (onAnyNonSuccess is null) throw new ArgumentNullException(nameof(onAnyNonSuccess));

        if (sourceResults.Item1 is null) throw new ArgumentNullException($""{nameof(sourceResults)}.Item1"");")
        .AppendItemNNullChecks(tupleCount)
        .Append(@"
        if (sourceResults.Item1.IsSuccess")
            .AppendAndItemNIsSuccessClauses(tupleCount)
            .Append(@")
        {
            return onAllSuccess(
                sourceResults.Item1.GetSuccessValue()")
            .AppendItemNGetSuccessValueParameters(tupleCount)
            .Append(@");
        }
        else
        {
            var error = GetNonSuccessError(
                getNoneError,
                sourceResults.Item1")
            .AppendItemNParameters(tupleCount)
            .Append(@");

            return onAnyNonSuccess(error);
        }
    }

");
    }

    public static StringBuilder AppendMatchAllAsyncMethod(this StringBuilder code, int tupleCount)
    {
            return code.Append(@"    /// <summary>
    /// Evaluates either the <paramref name=""onAllSuccess""/> or <paramref name=""onAnyNonSuccess""/> function depending on
    /// whether all results in the tuple are <c>Success</c> or not.
    /// </summary>
    /// <typeparam name=""T1"">The type of the tuple's first result.</typeparam>")
            .AppendTypeParamDocsForT(tupleCount)
            .Append(@"    /// <typeparam name=""TReturn"">The return type of the match all method.</typeparam>
    /// <param name=""sourceResults"">A tuple of results.</param>
    /// <param name=""onAllSuccess"">The function to evaluate if all results are <c>Success</c>. The non-null values of each of the
    ///     <c>Success</c> results are passed to this function.</param>
    /// <param name=""onAnyNonSuccess"">The function to evaluate if any results are <c>non-Success</c>. The error passed to this
    ///     function depends on how many results are <c>non-Success</c>. If only one is <c>non-Success</c>, then its error is
    ///     passed to this function. If more than one result is <c>non-Success</c>, then a <see cref=""CompositeError""/> is
    ///     returned containing the error of each <c>non-Success</c> result.</param>
    /// <param name=""getNoneError"">An optional function that creates the <see cref=""Error""/> for a <c>None</c> result. If null
    ///     or not provided, a function that returns an error with error code <see cref=""ErrorCodes.NotFound""/> is used instead.
    ///     </param>
    /// <returns>The result of the matching function evaluation.</returns>
    /// <exception cref=""ArgumentNullException"">If <paramref name=""onAllSuccess""/> or <paramref name=""onAnyNonSuccess""/> is
    ///     <see langword=""null""/> or if any of the <paramref name=""sourceResults""/> tuple's items are <see langword=""null""/>.
    ///     </exception>
    public static Task<TReturn> MatchAllAsync<")
            .AppendTypeDefinitionArgumentsForT(tupleCount)
            .Append(@", TReturn>(
        this ")
            .AppendTupleDefinitionForT(tupleCount)
            .Append(@" sourceResults,
        Func<")
            .AppendTypeDefinitionArgumentsForT(tupleCount)
            .Append(@", Task<TReturn>> onAllSuccess,
        Func<Error, Task<TReturn>> onAnyNonSuccess,
        Func<Error>? getNoneError = null)
    {
        if (onAllSuccess is null) throw new ArgumentNullException(nameof(onAllSuccess));
        if (onAnyNonSuccess is null) throw new ArgumentNullException(nameof(onAnyNonSuccess));

        if (sourceResults.Item1 is null) throw new ArgumentNullException($""{nameof(sourceResults)}.Item1"");")
        .AppendItemNNullChecks(tupleCount)
        .Append(@"
        if (sourceResults.Item1.IsSuccess")
            .AppendAndItemNIsSuccessClauses(tupleCount)
            .Append(@")
        {
            return onAllSuccess(
                sourceResults.Item1.GetSuccessValue()")
            .AppendItemNGetSuccessValueParameters(tupleCount)
            .Append(@");
        }
        else
        {
            var error = GetNonSuccessError(
                getNoneError,
                sourceResults.Item1")
            .AppendItemNParameters(tupleCount)
            .Append(@");

            return onAnyNonSuccess(error);
        }
    }

");
    }

    public static StringBuilder AppendEndRegion(this StringBuilder code) =>
        code.AppendLine("    #endregion").AppendLine();

    public static StringBuilder AppendGetNonSuccessErrorMethod(this StringBuilder code)
    {
        return code.Append(@"    private static Error GetNonSuccessError(Func<Error>? getNoneError, params IResult[] results)
    {
        var errors = results
            .Where(r => !r.IsSuccess)
            .Select(r => r.GetNonSuccessError(getNoneError));

        return CompositeError.Create(errors);
    }");
    }

    public static StringBuilder AppendEndClassDefinition(this StringBuilder code) =>
        code.Append(@"
}
");

    private static StringBuilder AppendTypeParamDocsForT(this StringBuilder code, int tupleCount)
    {
        for (int tupleNumber = 2; tupleNumber <= tupleCount; tupleNumber++)
        {
            code.Append($@"
    /// <typeparam name=""T{tupleNumber}"">The type of the tuple's {GetOrdinal(tupleNumber)} result.</typeparam>");
        }

        return code.AppendLine();
    }

    private static StringBuilder AppendTypeParamDocsForTResult(this StringBuilder code, int tupleCount)
    {
        for (int tupleNumber = 2; tupleNumber <= tupleCount; tupleNumber++)
        {
            code.Append($@"
    /// <typeparam name=""TResult{tupleNumber}"">The type of the tuple's {GetOrdinal(tupleNumber)} result.</typeparam>");
        }

        return code.AppendLine();
    }

    private static StringBuilder AppendTupleDefinitionForT(this StringBuilder code, int tupleCount)
    {
        code.Append("(IResult<T1>, ");

        for (int tupleNumber = 2; tupleNumber <= tupleCount; tupleNumber++)
        {
            code.Append($"IResult<T{tupleNumber}>");
            if (tupleNumber != tupleCount)
                code.Append(", ");
        }

        return code.Append(')');
    }

    private static StringBuilder AppendTupleDefinitionForTResult(this StringBuilder code, int tupleCount)
    {
        code.Append("(TResult1, ");

        for (int tupleNumber = 2; tupleNumber <= tupleCount; tupleNumber++)
        {
            code.Append($"TResult{tupleNumber}");
            if (tupleNumber != tupleCount)
                code.Append(", ");
        }

        return code.Append(')');
    }

    private static StringBuilder AppendTypeDefinitionForT(this StringBuilder code, int tupleCount)
    {
        code.Append('<');
        code.AppendTypeDefinitionArgumentsForT(tupleCount);
        return code.Append('>');
    }

    private static StringBuilder AppendTypeDefinitionForTResult(this StringBuilder code, int tupleCount)
    {
        code.Append("<TResult1, ");

        for (int tupleNumber = 2; tupleNumber <= tupleCount; tupleNumber++)
        {
            code.Append($"TResult{tupleNumber}");
            if (tupleNumber != tupleCount)
                code.Append(", ");
        }

        return code.Append('>');
    }

    private static StringBuilder AppendGenericConstraintsForTResult(this StringBuilder code, int tupleCount)
    {
        for (int tupleNumber = 2; tupleNumber <= tupleCount; tupleNumber++)
            code.AppendLine().Append($"        where TResult{tupleNumber} : IResult");

        return code;
    }

    private static StringBuilder AppendItemNNullChecks(this StringBuilder code, int tupleCount)
    {
        for (int tupleNumber = 2; tupleNumber <= tupleCount; tupleNumber++)
            code.AppendLine().Append(@$"        if (sourceResults.Item{tupleNumber} is null) throw new ArgumentNullException($""{{nameof(sourceResults)}}.Item{tupleNumber}"");");

        return code.AppendLine();
    }

    private static StringBuilder AppendTypeDefinitionArgumentsForT(this StringBuilder code, int tupleCount)
    {
        code.Append("T1, ");

        for (int tupleNumber = 2; tupleNumber <= tupleCount; tupleNumber++)
        {
            code.Append($"T{tupleNumber}");
            if (tupleNumber != tupleCount)
                code.Append(", ");
        }

        return code;
    }

    private static StringBuilder AppendAndItemNIsSuccessClauses(this StringBuilder code, int tupleCount)
    {
        for (int tupleNumber = 2; tupleNumber <= tupleCount; tupleNumber++)
        {
            code.Append($@"
            && sourceResults.Item{tupleNumber}.IsSuccess");
        }

        return code;
    }

    private static StringBuilder AppendOrItemNIsNotSuccessClauses(this StringBuilder code, int tupleCount)
    {
        for (int tupleNumber = 2; tupleNumber <= tupleCount; tupleNumber++)
        {
            code.Append($@"
            || !sourceResults.Item{tupleNumber}.IsSuccess");
        }

        return code;
    }

    private static StringBuilder AppendItemNGetSuccessValueParameters(this StringBuilder code, int tupleCount)
    {
        for (int tupleNumber = 2; tupleNumber <= tupleCount; tupleNumber++)
        {
            code.Append($@",
                sourceResults.Item{tupleNumber}.GetSuccessValue()");
        }

        return code;
    }

    private static StringBuilder AppendItemNParameters(this StringBuilder code, int tupleCount)
    {
        for (int tupleNumber = 2; tupleNumber <= tupleCount; tupleNumber++)
        {
            code.Append($@",
                sourceResults.Item{tupleNumber}");
        }

        return code;
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
