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
    /// <param name="value">The value. Can be <see langword="null"/>.</param>
    /// <param name="getNullValueError">
    /// An optional function that creates the <see cref="Error"/> of the <c>Fail</c> result when the <paramref name="value"/>
    /// parameter is <see langword="null"/>. If <see langword="null"/>, a function that returns an error with message "Value
    /// cannot be null." and error code 400 is used instead.
    /// </param>
    /// <returns>
    /// A <c>Success</c> result if <paramref name="value"/> is not <see langword="null"/>; otherwise, a <c>Fail</c> result with a
    /// generated stack trace.
    /// </returns>
    public static Result<T> ToResult<T>(this T? value, Func<Error>? getNullValueError = null) =>
        Result<T>.FromValue(value, getNullValueError);

    /// <summary>
    /// Creates a maybe from the specified value.
    /// </summary>
    /// <typeparam name="T">The type of the result.</typeparam>
    /// <param name="value">The value. Can be <see langword="null"/>.</param>
    /// <returns>A <c>Success</c> result if <paramref name="value"/> is not null; otherwise, a <c>None</c> result.</returns>
    public static Maybe<T> ToMaybe<T>(this T? value) =>
        Maybe<T>.FromValue(value);
}
