using static RandomSkunk.Results.AwaitSettings;

namespace RandomSkunk.Results;

/// <content> Defines the <c>Else</c> methods. </content>
public partial struct Result
{
    /// <summary>
    /// Returns the current result if it is a <c>Success</c> result, else returns the specified fallback result.
    /// </summary>
    /// <param name="fallbackResult">The fallback result if the result is not <c>Success</c>.</param>
    /// <returns>Either the current result or <paramref name="fallbackResult"/>.</returns>
    public Result Else(Result fallbackResult)
    {
        return _outcome == Outcome.Success ? this : fallbackResult;
    }

    /// <summary>
    /// Returns the current result if it is a <c>Success</c> result, else returns the result from evaluating the
    /// <paramref name="getFallbackResult"/> function.
    /// </summary>
    /// <param name="getFallbackResult">A function that returns the fallback result if the result is not <c>Success</c>.</param>
    /// <returns>Either the current result or the value returned from <paramref name="getFallbackResult"/>.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="getFallbackResult"/> is <see langword="null"/>.</exception>
    public Result Else(Func<Result> getFallbackResult)
    {
        if (getFallbackResult is null) throw new ArgumentNullException(nameof(getFallbackResult));

        return _outcome == Outcome.Success ? this : getFallbackResult();
    }
}

/// <content> Defines the <c>Else</c> methods. </content>
public partial struct Result<T>
{
    /// <summary>
    /// Returns the current result if it is a <c>Success</c> result, else returns the specified fallback result.
    /// </summary>
    /// <param name="fallbackResult">The fallback result if the result is not <c>Success</c>.</param>
    /// <returns>Either the current result or <paramref name="fallbackResult"/>.</returns>
    public Result<T> Else(Result<T> fallbackResult)
    {
        return _outcome == Outcome.Success ? this : fallbackResult;
    }

    /// <summary>
    /// Returns the current result if it is a <c>Success</c> result, else returns the result from evaluating the
    /// <paramref name="getFallbackResult"/> function.
    /// </summary>
    /// <param name="getFallbackResult">A function that returns the fallback result if the result is not <c>Success</c>.</param>
    /// <returns>Either the current result or the value returned from <paramref name="getFallbackResult"/>.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="getFallbackResult"/> is <see langword="null"/>.</exception>
    public Result<T> Else(Func<Result<T>> getFallbackResult)
    {
        if (getFallbackResult is null) throw new ArgumentNullException(nameof(getFallbackResult));

        return _outcome == Outcome.Success ? this : getFallbackResult();
    }
}

/// <content> Defines the <c>Else</c> methods. </content>
public partial struct Maybe<T>
{
    /// <summary>
    /// Returns the current result if it is a <c>Success</c> result, else returns the specified fallback result.
    /// </summary>
    /// <param name="fallbackResult">The fallback result if the result is not <c>Success</c>.</param>
    /// <returns>Either the current result or <paramref name="fallbackResult"/>.</returns>
    public Maybe<T> Else(Maybe<T> fallbackResult)
    {
        return _outcome == Outcome.Success ? this : fallbackResult;
    }

    /// <summary>
    /// Returns the current result if it is a <c>Success</c> result, else returns the result from evaluating the
    /// <paramref name="getFallbackResult"/> function.
    /// </summary>
    /// <param name="getFallbackResult">A function that returns the fallback result if the result is not <c>Success</c>.</param>
    /// <returns>Either the current result or the value returned from <paramref name="getFallbackResult"/>.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="getFallbackResult"/> is <see langword="null"/>.</exception>
    public Maybe<T> Else(Func<Maybe<T>> getFallbackResult)
    {
        if (getFallbackResult is null) throw new ArgumentNullException(nameof(getFallbackResult));

        return _outcome == Outcome.Success ? this : getFallbackResult();
    }
}

/// <content> Defines the <c>Else</c> extension methods. </content>
public static partial class ResultExtensions
{
    /// <summary>
    /// Returns <paramref name="sourceResult"/> if it is a <c>Success</c> result, else returns the specified fallback result.
    /// </summary>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="fallbackResult">The fallback result if the result is not <c>Success</c>.</param>
    /// <returns>Either <paramref name="sourceResult"/> or <paramref name="fallbackResult"/>.</returns>
    public static async Task<Result> Else(this Task<Result> sourceResult, Result fallbackResult) =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).Else(fallbackResult);

    /// <summary>
    /// Returns <paramref name="sourceResult"/> if it is a <c>Success</c> result, else returns the result from evaluating the
    /// <paramref name="getFallbackResult"/> function.
    /// </summary>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="getFallbackResult">A function that returns the fallback result if the result is not <c>Success</c>.</param>
    /// <returns>Either <paramref name="sourceResult"/> or the value returned from <paramref name="getFallbackResult"/>.
    ///     </returns>
    /// <exception cref="ArgumentNullException">If <paramref name="getFallbackResult"/> is <see langword="null"/>.</exception>
    public static async Task<Result> Else(this Task<Result> sourceResult, Func<Result> getFallbackResult) =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).Else(getFallbackResult);

    /// <summary>
    /// Returns <paramref name="sourceResult"/> if it is a <c>Success</c> result, else returns the specified fallback result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="fallbackResult">The fallback result if the result is not <c>Success</c>.</param>
    /// <returns>Either <paramref name="sourceResult"/> or <paramref name="fallbackResult"/>.</returns>
    public static async Task<Result<T>> Else<T>(this Task<Result<T>> sourceResult, Result<T> fallbackResult) =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).Else(fallbackResult);

    /// <summary>
    /// Returns <paramref name="sourceResult"/> if it is a <c>Success</c> result, else returns the result from evaluating the
    /// <paramref name="getFallbackResult"/> function.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="getFallbackResult">A function that returns the fallback result if the result is not <c>Success</c>.</param>
    /// <returns>Either <paramref name="sourceResult"/> or the value returned from <paramref name="getFallbackResult"/>.
    ///     </returns>
    /// <exception cref="ArgumentNullException">If <paramref name="getFallbackResult"/> is <see langword="null"/>.</exception>
    public static async Task<Result<T>> Else<T>(this Task<Result<T>> sourceResult, Func<Result<T>> getFallbackResult) =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).Else(getFallbackResult);

    /// <summary>
    /// Returns <paramref name="sourceResult"/> if it is a <c>Success</c> result, else returns the specified fallback result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="fallbackResult">The fallback result if the result is not <c>Success</c>.</param>
    /// <returns>Either <paramref name="sourceResult"/> or <paramref name="fallbackResult"/>.</returns>
    public static async Task<Maybe<T>> Else<T>(this Task<Maybe<T>> sourceResult, Maybe<T> fallbackResult) =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).Else(fallbackResult);

    /// <summary>
    /// Returns <paramref name="sourceResult"/> if it is a <c>Success</c> result, else returns the result from evaluating the
    /// <paramref name="getFallbackResult"/> function.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="getFallbackResult">A function that returns the fallback result if the result is not <c>Success</c>.</param>
    /// <returns>Either <paramref name="sourceResult"/> or the value returned from <paramref name="getFallbackResult"/>.
    ///     </returns>
    /// <exception cref="ArgumentNullException">If <paramref name="getFallbackResult"/> is <see langword="null"/>.</exception>
    public static async Task<Maybe<T>> Else<T>(this Task<Maybe<T>> sourceResult, Func<Maybe<T>> getFallbackResult) =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).Else(getFallbackResult);
}
