namespace RandomSkunk.Results;

/// <summary>
/// Defines methods for creating instances of <see cref="MaybeResult{T}"/>.
/// </summary>
/// <typeparam name="T">The type of the result value.</typeparam>
public interface IMaybeResultFactory<T>
{
    /// <summary>
    /// Creates a <c>Some</c> result for an operation with an optional return value.
    /// </summary>
    /// <param name="value">
    /// The value of the <c>Some</c> result. Must not be <see langword="null"/>.
    /// </param>
    /// <returns>A <c>Some</c> result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="value"/> is <see langword="null"/>.
    /// </exception>
    MaybeResult<T> Some([DisallowNull] T value);

    /// <summary>
    /// Creates a <c>None</c> result for an operation with an optional return value.
    /// </summary>
    /// <returns>A <c>None</c> result.</returns>
    MaybeResult<T> None();

    /// <summary>
    /// Creates a <c>Fail</c> result for an operation with an optional return value.
    /// </summary>
    /// <param name="error">The optional error that describes the failure.</param>
    /// <returns>A <c>Fail</c> result.</returns>
    MaybeResult<T> Fail(Error? error = null);
}
