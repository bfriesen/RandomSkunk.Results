namespace RandomSkunk.Results;

/// <content> Defines the <c>GetValueOr</c> methods. </content>
public partial struct Result<T>
{
    /// <summary>
    /// Gets the value of the <c>Success</c> result, or the specified fallback value if it is a <c>Fail</c> result.
    /// </summary>
    /// <param name="fallbackValue">The fallback value to return if this is not a <c>Success</c> result.</param>
    /// <returns>The value of this result if this is a <c>Success</c> result; otherwise, <paramref name="fallbackValue"/>.
    ///     </returns>
    public T? GetValueOr(T? fallbackValue)
    {
        return _type == ResultType.Success ? _value! : fallbackValue;
    }

    /// <summary>
    /// Gets the value of the <c>Success</c> result, or the specified fallback value if it is a <c>Fail</c> result.
    /// </summary>
    /// <param name="getFallbackValue">A function that creates the fallback value to return if this is not a <c>Success</c>
    ///     result.</param>
    /// <returns>The value of this result if this is a <c>Success</c> result; otherwise, the value returned by the
    ///     <paramref name="getFallbackValue"/> function.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="getFallbackValue"/> is <see langword="null"/>.</exception>
    public T? GetValueOr(Func<T?> getFallbackValue)
    {
        if (getFallbackValue is null) throw new ArgumentNullException(nameof(getFallbackValue));

        return _type == ResultType.Success ? _value! : getFallbackValue();
    }
}
