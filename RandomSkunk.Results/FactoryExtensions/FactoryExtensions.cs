namespace RandomSkunk.Results.FactoryExtensions;

/// <summary>
/// Defines extension methods for creating result objects from arbitrary types.
/// </summary>
public static class FactoryExtensions
{
    /// <summary>
    /// Creates a <c>Success</c> result with the specified non-null value. If the value is <see langword="null"/>, then a
    /// <c>Fail</c> result is returned instead.
    /// </summary>
    /// <typeparam name="T">The type of the result value.</typeparam>
    /// <param name="sourceValue">The value. Can be <see langword="null"/>.</param>
    /// <param name="getNullValueError">An optional function that creates the <see cref="Error"/> of the <c>Fail</c> result when
    ///     the <paramref name="sourceValue"/> parameter is <see langword="null"/>. When <see langword="null"/> or not provided,
    ///     a function that returns an error with error code <see cref="ErrorCodes.BadRequest"/> and a message indicating that
    ///     the value cannot be null is used instead.</param>
    /// <returns>A <c>Success</c> result if <paramref name="sourceValue"/> is not <see langword="null"/>; otherwise, a
    ///     <c>Fail</c> result with a generated stack trace.</returns>
    public static Result<T> ToResult<T>(this T? sourceValue, Func<Error>? getNullValueError = null) =>
        Result<T>.FromValue(sourceValue, getNullValueError);

    /// <summary>
    /// Creates a maybe from the specified value.
    /// </summary>
    /// <typeparam name="T">The type of the result.</typeparam>
    /// <param name="sourceValue">The value. Can be <see langword="null"/>.</param>
    /// <returns>A <c>Success</c> result if <paramref name="sourceValue"/> is not null; otherwise, a <c>None</c> result.
    ///     </returns>
    public static Maybe<T> ToMaybe<T>(this T? sourceValue) =>
        Maybe<T>.FromValue(sourceValue);
}
