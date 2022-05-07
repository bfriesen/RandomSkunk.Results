namespace RandomSkunk.Results;

/// <summary>
/// Defines methods for creating instances of <see cref="Result{T}"/>.
/// </summary>
/// <typeparam name="T">The type of the result value.</typeparam>
public interface IResultFactory<T>
{
    /// <summary>
    /// Creates a <c>Success</c> result with the specified value.
    /// </summary>
    /// <param name="value">
    /// The value of the <c>Success</c> result. Must not be <see langword="null"/>.
    /// </param>
    /// <returns>A <c>Success</c> result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="value"/> is <see langword="null"/>.
    /// </exception>
    Result<T> Success([DisallowNull] T value);

    /// <summary>
    /// Creates a <c>Fail</c> result with the specified error.
    /// </summary>
    /// <param name="error">
    /// An error that describes the failure. If <see langword="null"/>, a default error is
    /// used.
    /// </param>
    /// <returns>A <c>Fail</c> result.</returns>
    Result<T> Fail(Error? error = null);
}
