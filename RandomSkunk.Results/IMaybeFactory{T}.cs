namespace RandomSkunk.Results;

/// <summary>
/// Defines methods for creating instances of <see cref="Maybe{T}"/>.
/// </summary>
/// <typeparam name="T">The type of the result value.</typeparam>
public interface IMaybeFactory<T>
{
    /// <summary>
    /// Creates a <c>Some</c> result with the specified value.
    /// </summary>
    /// <param name="value">
    /// The value of the <c>Some</c> result. Must not be <see langword="null"/>.
    /// </param>
    /// <returns>A <c>Some</c> result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="value"/> is <see langword="null"/>.
    /// </exception>
    Maybe<T> Some([DisallowNull] T value);

    /// <summary>
    /// Creates a <c>None</c> result.
    /// </summary>
    /// <returns>A <c>None</c> result.</returns>
    Maybe<T> None();

    /// <summary>
    /// Creates a <c>Fail</c> result with the specified error.
    /// </summary>
    /// <param name="error">
    /// An error that describes the failure. If <see langword="null"/>, a default error is
    /// used.
    /// </param>
    /// <returns>A <c>Fail</c> result.</returns>
    Maybe<T> Fail(Error? error = null);
}
