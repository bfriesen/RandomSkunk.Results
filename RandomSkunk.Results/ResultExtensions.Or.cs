namespace RandomSkunk.Results;

/// <content>
/// Defines the <c>Or</c> extension methods.
/// </content>
public static partial class ResultExtensions
{
    /// <summary>
    /// Returns <paramref name="source"/> if it is a <c>Success</c> result; otherwise, returns a
    /// new <c>Success</c> result with the specified fallback value.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="fallbackValue">The fallback value if the result is not <c>Success</c>.</param>
    /// <returns>A <c>Success</c> result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="fallbackValue"/> is <see langword="null"/>.
    /// </exception>
    public static Result<T> Or<T>(this Result<T> source, [DisallowNull] T fallbackValue)
    {
        if (fallbackValue is null) throw new ArgumentNullException(nameof(fallbackValue));

        return source.IsSuccess ? source : Result<T>.Create.Success(fallbackValue);
    }

    /// <summary>
    /// Returns <paramref name="source"/> if it is a <c>Success</c> result; otherwise, returns a
    /// new <c>Success</c> result with the specified fallback value.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="getFallbackValue">
    /// A function that returns the fallback value if the result is not <c>Success</c>.
    /// </param>
    /// <returns>A <c>Success</c> result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="getFallbackValue"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// If <paramref name="getFallbackValue"/> returns <see langword="null"/> when evaluated.
    /// </exception>
    public static Result<T> Or<T>(this Result<T> source, Func<T> getFallbackValue)
    {
        if (getFallbackValue is null) throw new ArgumentNullException(nameof(getFallbackValue));

        if (source.IsSuccess)
            return source;

        var fallbackValue = getFallbackValue()
             ?? throw Exceptions.FunctionMustNotReturnNull(nameof(getFallbackValue));

        return Result<T>.Create.Success(fallbackValue);
    }

    /// <summary>
    /// Returns <paramref name="source"/> if it is a <c>Some</c> result; otherwise, returns a new
    /// <c>Some</c> result with the specified fallback value.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="fallbackValue">The fallback value if the result is not <c>Some</c>.</param>
    /// <returns>A <c>Some</c> result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="fallbackValue"/> is <see langword="null"/>.
    /// </exception>
    public static Maybe<T> Or<T>(this Maybe<T> source, [DisallowNull] T fallbackValue)
    {
        if (fallbackValue is null) throw new ArgumentNullException(nameof(fallbackValue));

        return source.IsSome ? source : Maybe<T>.Create.Some(fallbackValue);
    }

    /// <summary>
    /// Returns <paramref name="source"/> if it is a <c>Some</c> result; otherwise, returns a new
    /// <c>Some</c> result with its value from evaluating the <paramref name="getFallbackValue"/>
    /// function.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="getFallbackValue">
    /// A function that returns the fallback value if the result is not <c>Some</c>.
    /// </param>
    /// <returns>A <c>Some</c> result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="getFallbackValue"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// If <paramref name="getFallbackValue"/> returns <see langword="null"/> when evaluated.
    /// </exception>
    public static Maybe<T> Or<T>(this Maybe<T> source, Func<T> getFallbackValue)
    {
        if (getFallbackValue is null) throw new ArgumentNullException(nameof(getFallbackValue));

        if (source.IsSome)
            return source;

        var fallbackValue = getFallbackValue()
             ?? throw Exceptions.FunctionMustNotReturnNull(nameof(getFallbackValue));

        return Maybe<T>.Create.Some(fallbackValue);
    }
}