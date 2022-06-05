namespace RandomSkunk.Results.FactoryExtensions;

/// <summary>
/// Defines extension methods for creating result objects from arbitrary types.
/// </summary>
public static class FactoryExtensions
{
    internal const string _defaultNullValueErrorMessage = "Value cannot be null.";
    internal const int _defaultNullValueErrorCode = 400;

    /// <summary>
    /// Creates a <c>Success</c> result with the specified non-null value. If the value is <see langword="null"/>, then a
    /// <c>Fail</c> result is returned instead.
    /// </summary>
    /// <typeparam name="T">The type of the result value.</typeparam>
    /// <param name="value">The value. Can be <see langword="null"/>.</param>
    /// <param name="nullValueErrorMessage">
    /// The error message to use if <paramref name="value"/> is <see langword="null"/>.
    /// </param>
    /// <param name="nullValueErrorCode">The error code to use if <paramref name="value"/> is <see langword="null"/>.</param>
    /// <param name="nullValueErrorIdentifier">
    /// The error identifier to use if <paramref name="value"/> is <see langword="null"/>.
    /// </param>
    /// <param name="nullValueErrorType">The error type to use if <paramref name="value"/> is <see langword="null"/>.</param>
    /// <returns>
    /// A <c>Success</c> result if <paramref name="value"/> is not <see langword="null"/>; otherwise, a <c>Fail</c> result with a
    /// generated stack trace.
    /// </returns>
    public static Result<T> ToResult<T>(
        this T? value,
        string nullValueErrorMessage = _defaultNullValueErrorMessage,
        int nullValueErrorCode = _defaultNullValueErrorCode,
        string? nullValueErrorIdentifier = null,
        string? nullValueErrorType = null) =>
        value is not null
            ? Result<T>.Success(value)
            : Result<T>.Fail(
                string.IsNullOrEmpty(nullValueErrorMessage) ? _defaultNullValueErrorMessage : nullValueErrorMessage,
                new StackTrace().ToString(),
                nullValueErrorCode,
                nullValueErrorIdentifier,
                nullValueErrorType);

    /// <summary>
    /// Creates a maybe from the specified value.
    /// </summary>
    /// <typeparam name="T">The type of the result.</typeparam>
    /// <param name="value">The value. Can be <see langword="null"/>.</param>
    /// <returns>A <c>Some</c> result if <paramref name="value"/> is not null; otherwise, a <c>None</c> result.</returns>
    public static Maybe<T> ToMaybe<T>(this T? value) =>
        Maybe<T>.FromValue(value);
}
