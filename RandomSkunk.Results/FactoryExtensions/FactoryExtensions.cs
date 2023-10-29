namespace RandomSkunk.Results.FactoryExtensions;

/// <summary>
/// Defines extension methods for creating result objects from arbitrary types.
/// </summary>
public static class FactoryExtensions
{
    /// <summary>
    /// Creates a <c>Success</c> result with the specified value. If the value is <see langword="null"/>, then a <c>Fail</c>
    /// result with error code <see cref="ErrorCodes.BadRequest"/> is returned instead.
    /// </summary>
    /// <typeparam name="T">The type of the result value.</typeparam>
    /// <param name="sourceValue">The value. Can be <see langword="null"/>.</param>
    /// <returns>A <c>Success</c> result if <paramref name="sourceValue"/> is not <see langword="null"/>; otherwise, a
    ///     <c>Fail</c> result.</returns>
    public static Result<T> ToResult<T>(this T? sourceValue) => sourceValue;
}
