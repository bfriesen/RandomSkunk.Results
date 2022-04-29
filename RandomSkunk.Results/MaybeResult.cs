namespace RandomSkunk.Results;

/// <summary>
/// Defines factory methods for creating instances of <see cref="MaybeResult{T}"/>.
/// </summary>
public static class MaybeResult
{
    /// <summary>
    /// Creates a <c>some</c> result for an operation with a return value.
    /// </summary>
    /// <typeparam name="T">The type of the return value of the operation.</typeparam>
    /// <param name="value">
    /// The value of the <c>some</c> result. Must not be <see langword="null"/>.
    /// </param>
    /// <returns>A <c>some</c> result.</returns>
    public static MaybeResult<T> Some<T>([DisallowNull] T value) => MaybeResult<T>.Some(value);

    /// <summary>
    /// Creates a <c>none</c> result for an operation with a return value.
    /// </summary>
    /// <typeparam name="T">The type of the return value of the operation.</typeparam>
    /// <returns>A <c>none</c> result.</returns>
    public static MaybeResult<T> None<T>() => MaybeResult<T>.None();

    /// <summary>
    /// Creates a <c>fail</c> result for an operation with a return value.
    /// </summary>
    /// <typeparam name="T">The type of the return value of the operation.</typeparam>
    /// <param name="error">The optional error that describes the failure.</param>
    /// <returns>A <c>fail</c> result.</returns>
    public static MaybeResult<T> Fail<T>(Error? error = null) => MaybeResult<T>.Fail(error);

    /// <summary>
    /// Creates a <c>fail</c> result for an operation <em>with</em> a return value.
    /// </summary>
    /// <typeparam name="T">The type of the return value of the operation.</typeparam>
    /// <param name="exception">The exception that caused the failure.</param>
    /// <param name="messagePrefix">An optional prefix for the exception message.</param>
    /// <param name="errorCode">The optional error code.</param>
    /// <param name="identifier">The optional identifier of the error.</param>
    /// <returns>A <c>fail</c> result.</returns>
    public static MaybeResult<T> Fail<T>(
        Exception exception,
        string? messagePrefix = null,
        int? errorCode = null,
        string? identifier = null) =>
        MaybeResult<T>.Fail(Error.FromException(exception, messagePrefix, errorCode, identifier));

    /// <summary>
    /// Creates a <c>fail</c> result for an operation with a return value.
    /// </summary>
    /// <typeparam name="T">The type of the return value of the operation.</typeparam>
    /// <param name="errorMessage">The error message that describes the failure.</param>
    /// <param name="stackTrace">The optional stack trace that describes the failure.</param>
    /// <param name="errorCode">The optional error code that describes the failure.</param>
    /// <param name="identifier">The optional identifier of the error.</param>
    /// <returns>A <c>fail</c> result.</returns>
    public static MaybeResult<T> Fail<T>(
        string errorMessage,
        string? stackTrace = null,
        int? errorCode = null,
        string? identifier = null) =>
        MaybeResult<T>.Fail(errorMessage, stackTrace, errorCode, identifier);
}
