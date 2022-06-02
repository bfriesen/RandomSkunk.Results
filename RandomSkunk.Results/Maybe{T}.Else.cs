namespace RandomSkunk.Results;

/// <content>
/// Defines the <c>Else</c> methods.
/// </content>
public partial struct Maybe<T>
{
    /// <summary>
    /// Returns the current result if it is a <c>Some</c> result, else returns the specified fallback result.
    /// </summary>
    /// <param name="fallbackResult">The fallback result if the result is not <c>Some</c>.</param>
    /// <returns>Either the current result or <paramref name="fallbackResult"/>.</returns>
    public Maybe<T> Else(Maybe<T> fallbackResult)
    {
        return _type == MaybeType.Some ? this : fallbackResult;
    }

    /// <summary>
    /// Returns the current result if it is a <c>Some</c> result, else returns the result from evaluating the
    /// <paramref name="getFallbackResult"/> function.
    /// </summary>
    /// <param name="getFallbackResult">
    /// A function that returns the fallback result if the result is not <c>Some</c>.
    /// </param>
    /// <returns>
    /// Either the current result or the value returned from <paramref name="getFallbackResult"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="getFallbackResult"/> is <see langword="null"/>.
    /// </exception>
    public Maybe<T> Else(Func<Maybe<T>> getFallbackResult)
    {
        if (getFallbackResult is null) throw new ArgumentNullException(nameof(getFallbackResult));

        return _type == MaybeType.Some ? this : getFallbackResult();
    }
}
