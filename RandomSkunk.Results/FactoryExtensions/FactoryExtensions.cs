namespace RandomSkunk.Results.FactoryExtensions;

/// <summary>
/// Defines extension methods for creating result objects from arbitrary types.
/// </summary>
public static class FactoryExtensions
{
    /// <summary>
    /// Creates a <c>Success</c> result with the specified value.
    /// </summary>
    /// <typeparam name="T">The type of the result value.</typeparam>
    /// <param name="value">The value of the <c>Success</c> result. Must not be <see langword="null"/>.</param>
    /// <returns>A <c>Success</c> result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="value"/> is <see langword="null"/>.</exception>
    public static Result<T> ToResult<T>([DisallowNull] this T value) =>
        Result<T>.Success(value);

    /// <summary>
    /// Creates a maybe from the specified value.
    /// </summary>
    /// <typeparam name="T">The type of the result.</typeparam>
    /// <param name="value">The value. Can be <see langword="null"/>.</param>
    /// <returns>A <c>Some</c> result if <paramref name="value"/> is not null; otherwise, a <c>None</c> result.</returns>
    public static Maybe<T> ToMaybe<T>(this T? value) =>
        Maybe<T>.FromValue(value);
}
