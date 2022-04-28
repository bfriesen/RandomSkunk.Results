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
    /// <param name="memberName">The compiler-provided name of the member where the call originated.</param>
    /// <param name="filePath">The compiler-provided path to the source file where the call originated.</param>
    /// <param name="lineNumber">The compiler-provided line number where the call originated.</param>
    /// <returns>A <c>some</c> result.</returns>
    public static MaybeResult<T> Some<T>(
        [DisallowNull] T value,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int lineNumber = 0) =>
        MaybeResult<T>.Some(value, memberName, filePath, lineNumber);

    /// <summary>
    /// Creates a <c>none</c> result for an operation with a return value.
    /// </summary>
    /// <typeparam name="T">The type of the return value of the operation.</typeparam>
    /// <param name="memberName">The compiler-provided name of the member where the call originated.</param>
    /// <param name="filePath">The compiler-provided path to the source file where the call originated.</param>
    /// <param name="lineNumber">The compiler-provided line number where the call originated.</param>
    /// <returns>A <c>none</c> result.</returns>
    public static MaybeResult<T> None<T>(
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int lineNumber = 0) =>
        MaybeResult<T>.None(memberName, filePath, lineNumber);

    /// <summary>
    /// Creates a <c>fail</c> result for an operation with a return value.
    /// </summary>
    /// <typeparam name="T">The type of the return value of the operation.</typeparam>
    /// <param name="error">The optional error that describes the failure.</param>
    /// <param name="memberName">The compiler-provided name of the member where the call originated.</param>
    /// <param name="filePath">The compiler-provided path to the source file where the call originated.</param>
    /// <param name="lineNumber">The compiler-provided line number where the call originated.</param>
    /// <returns>A <c>fail</c> result.</returns>
    public static MaybeResult<T> Fail<T>(
        Error? error = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int lineNumber = 0) =>
        MaybeResult<T>.Fail(error, memberName, filePath, lineNumber);

    /// <summary>
    /// Creates a <c>fail</c> result for an operation <em>with</em> a return value.
    /// </summary>
    /// <typeparam name="T">The type of the return value of the operation.</typeparam>
    /// <param name="exception">The exception that caused the failure.</param>
    /// <param name="messagePrefix">An optional prefix for the exception message.</param>
    /// <param name="errorCode">The optional error code.</param>
    /// <param name="identifier">The optional identifier of the error.</param>
    /// <param name="memberName">The compiler-provided name of the member where the call originated.</param>
    /// <param name="filePath">The compiler-provided path to the source file where the call originated.</param>
    /// <param name="lineNumber">The compiler-provided line number where the call originated.</param>
    /// <returns>A <c>fail</c> result.</returns>
    public static MaybeResult<T> Fail<T>(
        Exception exception,
        string? messagePrefix = null,
        int? errorCode = null,
        string? identifier = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int lineNumber = 0) =>
        MaybeResult<T>.Fail(Error.FromException(exception, messagePrefix, errorCode, identifier), memberName, filePath, lineNumber);

    /// <summary>
    /// Creates a <c>fail</c> result for an operation with a return value.
    /// </summary>
    /// <typeparam name="T">The type of the return value of the operation.</typeparam>
    /// <param name="errorMessage">The error message that describes the failure.</param>
    /// <param name="stackTrace">The optional stack trace that describes the failure.</param>
    /// <param name="errorCode">The optional error code that describes the failure.</param>
    /// <param name="identifier">The optional identifier of the error.</param>
    /// <param name="memberName">The compiler-provided name of the member where the call originated.</param>
    /// <param name="filePath">The compiler-provided path to the source file where the call originated.</param>
    /// <param name="lineNumber">The compiler-provided line number where the call originated.</param>
    /// <returns>A <c>fail</c> result.</returns>
    public static MaybeResult<T> Fail<T>(
        string errorMessage,
        string? stackTrace = null,
        int? errorCode = null,
        string? identifier = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int lineNumber = 0) =>
        MaybeResult<T>.Fail(errorMessage, stackTrace, errorCode, identifier, memberName, filePath, lineNumber);
}
