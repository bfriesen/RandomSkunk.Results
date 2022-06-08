namespace RandomSkunk.Results;

/// <content>
/// Defines the <c>Error</c> extension methods.
/// </content>
public static partial class FailFactoryExtensions
{
    /// <summary>
    /// Creates a <c>Fail</c> result with a generated stack trace.
    /// </summary>
    /// <param name="source">The source factory.</param>
    /// <param name="errorMessage">The error message.</param>
    /// <param name="errorCode">The optional error code.</param>
    /// <param name="errorIdentifier">The optional identifier of the error.</param>
    /// <param name="errorType">
    /// The optional type of the error. If <see langword="null"/>, then the
    /// <see cref="MemberInfo.Name"/> of the <see cref="Type"/> of the current instance
    /// is used instead.
    /// </param>
    /// <param name="innerError">
    /// The optional error that is the cause of the current error.
    /// </param>
    /// <param name="stackTraceSkipFrames">
    /// The number of frames up the stack from which to start the generated stack trace.
    /// </param>
    /// <returns>A <c>Fail</c> result.</returns>
    public static Result Error(
        this ResultFailFactory source,
        string errorMessage,
        int? errorCode = null,
        string? errorIdentifier = null,
        string? errorType = null,
        Error? innerError = null,
        byte stackTraceSkipFrames = 2) =>
        source.Error(new Error(errorMessage, errorType)
        {
            StackTrace = new StackTrace(stackTraceSkipFrames).ToString(),
            ErrorCode = errorCode,
            Identifier = errorIdentifier,
            InnerError = innerError,
        });

    /// <summary>
    /// Creates a <c>Fail</c> result with a generated stack trace.
    /// </summary>
    /// <typeparam name="T">The type of the result.</typeparam>
    /// <param name="source">The source factory.</param>
    /// <param name="errorMessage">The error message.</param>
    /// <param name="errorCode">The optional error code.</param>
    /// <param name="errorIdentifier">The optional identifier of the error.</param>
    /// <param name="errorType">
    /// The optional type of the error. If <see langword="null"/>, then the
    /// <see cref="MemberInfo.Name"/> of the <see cref="Type"/> of the current instance
    /// is used instead.
    /// </param>
    /// <param name="innerError">
    /// The optional error that is the cause of the current error.
    /// </param>
    /// <param name="stackTraceSkipFrames">
    /// The number of frames up the stack from which to start the generated stack trace.
    /// </param>
    /// <returns>A <c>Fail</c> result.</returns>
    public static Result<T> Error<T>(
        this ResultFailFactory<T> source,
        string errorMessage,
        int? errorCode = null,
        string? errorIdentifier = null,
        string? errorType = null,
        Error? innerError = null,
        byte stackTraceSkipFrames = 2) =>
        source.Error(new Error(errorMessage, errorType)
        {
            StackTrace = new StackTrace(stackTraceSkipFrames).ToString(),
            ErrorCode = errorCode,
            Identifier = errorIdentifier,
            InnerError = innerError,
        });

    /// <summary>
    /// Creates a <c>Fail</c> result with a generated stack trace.
    /// </summary>
    /// <typeparam name="T">The type of the result.</typeparam>
    /// <param name="source">The source factory.</param>
    /// <param name="errorMessage">The error message.</param>
    /// <param name="errorCode">The optional error code.</param>
    /// <param name="errorIdentifier">The optional identifier of the error.</param>
    /// <param name="errorType">
    /// The optional type of the error. If <see langword="null"/>, then the
    /// <see cref="MemberInfo.Name"/> of the <see cref="Type"/> of the current instance
    /// is used instead.
    /// </param>
    /// <param name="innerError">
    /// The optional error that is the cause of the current error.
    /// </param>
    /// <param name="stackTraceSkipFrames">
    /// The number of frames up the stack from which to start the generated stack trace.
    /// </param>
    /// <returns>A <c>Fail</c> result.</returns>
    public static Maybe<T> Error<T>(
        this MaybeFailFactory<T> source,
        string errorMessage,
        int? errorCode = null,
        string? errorIdentifier = null,
        string? errorType = null,
        Error? innerError = null,
        byte stackTraceSkipFrames = 2) =>
        source.Error(new Error(errorMessage, errorType)
        {
            StackTrace = new StackTrace(stackTraceSkipFrames).ToString(),
            ErrorCode = errorCode,
            Identifier = errorIdentifier,
            InnerError = innerError,
        });
}
