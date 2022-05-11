using static RandomSkunk.Results.MaybeType;
using static RandomSkunk.Results.ResultType;

namespace RandomSkunk.Results;

/// <content>
/// Defines the <c>Else</c> extension methods.
/// </content>
public static partial class ResultExtensions
{
    /// <summary>
    /// Returns <paramref name="source"/> if it is a <c>Success</c> result, else returns the
    /// specified fallback result.
    /// </summary>
    /// <param name="source">The source result.</param>
    /// <param name="fallbackResult">The fallback result if the result is not <c>Success</c>.</param>
    /// <returns>Either <paramref name="source"/> or <paramref name="fallbackResult"/>.</returns>
    public static Result Else(this Result source, Result fallbackResult)
    {
        return source._type == Success ? source : fallbackResult;
    }

    /// <summary>
    /// Returns <paramref name="source"/> if it is a <c>Success</c> result, else returns the result
    /// from evaluating the <paramref name="getFallbackResult"/> function.
    /// </summary>
    /// <param name="source">The source result.</param>
    /// <param name="getFallbackResult">
    /// A function that returns the fallback result if the result is not <c>Success</c>.
    /// </param>
    /// <returns>
    /// Either <paramref name="source"/> or the value returned from
    /// <paramref name="getFallbackResult"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="getFallbackResult"/> is <see langword="null"/>.
    /// </exception>
    public static Result Else(this Result source, Func<Result> getFallbackResult)
    {
        if (getFallbackResult is null) throw new ArgumentNullException(nameof(getFallbackResult));

        return source._type == Success ? source : getFallbackResult();
    }

    /// <summary>
    /// Returns <paramref name="source"/> if it is a <c>Success</c> result, else returns the
    /// specified fallback result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="fallbackResult">The fallback result if the result is not <c>Success</c>.</param>
    /// <returns>Either <paramref name="source"/> or <paramref name="fallbackResult"/>.</returns>
    public static Result<T> Else<T>(this Result<T> source, Result<T> fallbackResult)
    {
        return source._type == Success ? source : fallbackResult;
    }

    /// <summary>
    /// Returns <paramref name="source"/> if it is a <c>Success</c> result, else returns the result
    /// from evaluating the <paramref name="getFallbackResult"/> function.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="getFallbackResult">
    /// A function that returns the fallback result if the result is not <c>Success</c>.
    /// </param>
    /// <returns>
    /// Either <paramref name="source"/> or the value returned from
    /// <paramref name="getFallbackResult"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="getFallbackResult"/> is <see langword="null"/>.
    /// </exception>
    public static Result<T> Else<T>(this Result<T> source, Func<Result<T>> getFallbackResult)
    {
        if (getFallbackResult is null) throw new ArgumentNullException(nameof(getFallbackResult));

        return source._type == Success ? source : getFallbackResult();
    }

    /// <summary>
    /// Returns <paramref name="source"/> if it is a <c>Some</c> result, else returns the
    /// specified fallback result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="fallbackResult">The fallback result if the result is not <c>Some</c>.</param>
    /// <returns>Either <paramref name="source"/> or <paramref name="fallbackResult"/>.</returns>
    public static Maybe<T> Else<T>(this Maybe<T> source, Maybe<T> fallbackResult)
    {
        return source._type == Some ? source : fallbackResult;
    }

    /// <summary>
    /// Returns <paramref name="source"/> if it is a <c>Some</c> result, else returns the result
    /// from evaluating the <paramref name="getFallbackResult"/> function.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="getFallbackResult">
    /// A function that returns the fallback result if the result is not <c>Some</c>.
    /// </param>
    /// <returns>
    /// Either <paramref name="source"/> or the value returned from
    /// <paramref name="getFallbackResult"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="getFallbackResult"/> is <see langword="null"/>.
    /// </exception>
    public static Maybe<T> Else<T>(this Maybe<T> source, Func<Maybe<T>> getFallbackResult)
    {
        if (getFallbackResult is null) throw new ArgumentNullException(nameof(getFallbackResult));

        return source._type == Some ? source : getFallbackResult();
    }
}
