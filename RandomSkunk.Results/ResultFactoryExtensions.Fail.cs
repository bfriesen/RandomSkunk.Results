using static RandomSkunk.Results.Error;

namespace RandomSkunk.Results;

/// <content>
/// Defines the <c>Fail</c> extension methods.
/// </content>
public static partial class ResultFactoryExtensions
{
    /// <summary>
    /// Creates a <c>Fail</c> result from the specified exception.
    /// </summary>
    /// <param name="source">The source factory.</param>
    /// <param name="exception">The exception that caused the failure.</param>
    /// <param name="errorMessage">The optional error message.</param>
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
    /// <returns>A <c>Fail</c> result.</returns>
    public static Result Exception(
        this ResultFactory source,
        Exception exception,
        string? errorMessage = null,
        int? errorCode = null,
        string? errorIdentifier = null,
        string? errorType = null,
        Error? innerError = null) =>
        source.Error(FromException(exception, errorMessage, errorCode, errorIdentifier, errorType, innerError));

    /// <summary>
    /// Creates a <c>Fail</c> result from the specified exception.
    /// </summary>
    /// <typeparam name="T">The type of the result.</typeparam>
    /// <param name="source">The source factory.</param>
    /// <param name="exception">The exception that caused the failure.</param>
    /// <param name="errorMessage">The optional error message.</param>
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
    /// <returns>A <c>Fail</c> result.</returns>
    public static Result<T> Exception<T>(
        this ResultFactory<T> source,
        Exception exception,
        string? errorMessage = null,
        int? errorCode = null,
        string? errorIdentifier = null,
        string? errorType = null,
        Error? innerError = null) =>
        source.Error(FromException(exception, errorMessage, errorCode, errorIdentifier, errorType, innerError));

    /// <summary>
    /// Creates a <c>Fail</c> result from the specified exception.
    /// </summary>
    /// <typeparam name="T">The type of the result.</typeparam>
    /// <param name="source">The source factory.</param>
    /// <param name="exception">The exception that caused the failure.</param>
    /// <param name="errorMessage">The optional error message.</param>
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
    /// <returns>A <c>Fail</c> result.</returns>
    public static Maybe<T> Exception<T>(
        this MaybeFactory<T> source,
        Exception exception,
        string? errorMessage = null,
        int? errorCode = null,
        string? errorIdentifier = null,
        string? errorType = null,
        Error? innerError = null) =>
        source.Error(FromException(exception, errorMessage, errorCode, errorIdentifier, errorType, innerError));

    /// <summary>
    /// Creates a <c>Fail</c> result.
    /// </summary>
    /// <param name="source">The source factory.</param>
    /// <param name="errorMessage">The error message.</param>
    /// <param name="stackTrace">The optional stack trace.</param>
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
    /// <returns>A <c>Fail</c> result.</returns>
    public static Result Error(
        this ResultFactory source,
        string errorMessage,
        string? stackTrace = null,
        int? errorCode = null,
        string? errorIdentifier = null,
        string? errorType = null,
        Error? innerError = null) =>
        source.Error(new Error(errorMessage, errorType)
        {
            StackTrace = stackTrace,
            ErrorCode = errorCode,
            Identifier = errorIdentifier,
            InnerError = innerError,
        });

    /// <summary>
    /// Creates a <c>Fail</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the result.</typeparam>
    /// <param name="source">The source factory.</param>
    /// <param name="errorMessage">The error message.</param>
    /// <param name="stackTrace">The optional stack trace.</param>
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
    /// <returns>A <c>Fail</c> result.</returns>
    public static Result<T> Error<T>(
        this ResultFactory<T> source,
        string errorMessage,
        string? stackTrace = null,
        int? errorCode = null,
        string? errorIdentifier = null,
        string? errorType = null,
        Error? innerError = null) =>
        source.Error(new Error(errorMessage, errorType)
        {
            StackTrace = stackTrace,
            ErrorCode = errorCode,
            Identifier = errorIdentifier,
            InnerError = innerError,
        });

    /// <summary>
    /// Creates a <c>Fail</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the result.</typeparam>
    /// <param name="source">The source factory.</param>
    /// <param name="errorMessage">The error message.</param>
    /// <param name="stackTrace">The optional stack trace.</param>
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
    /// <returns>A <c>Fail</c> result.</returns>
    public static Maybe<T> Error<T>(
        this MaybeFactory<T> source,
        string errorMessage,
        string? stackTrace = null,
        int? errorCode = null,
        string? errorIdentifier = null,
        string? errorType = null,
        Error? innerError = null) =>
        source.Error(new Error(errorMessage, errorType)
        {
            StackTrace = stackTrace,
            ErrorCode = errorCode,
            Identifier = errorIdentifier,
            InnerError = innerError,
        });
}
