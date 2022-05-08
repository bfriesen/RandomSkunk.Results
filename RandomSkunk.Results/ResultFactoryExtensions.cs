using System.Reflection;

namespace RandomSkunk.Results;

/// <summary>
/// Defines extension methods for result factories.
/// </summary>
public static class ResultFactoryExtensions
{
    /// <summary>
    /// Creates a maybe from the specified value.
    /// </summary>
    /// <typeparam name="T">The type of the result.</typeparam>
    /// <param name="source">The source factory.</param>
    /// <param name="value">The value. Can be <see langword="null"/>.</param>
    /// <returns>
    /// A <c>Some</c> result if <paramref name="value"/> is not null; otherwise, a <c>None</c>
    /// result.
    /// </returns>
    public static Maybe<T> FromValue<T>(
        this IMaybeFactory<T> source, T? value) =>
        value is not null
            ? source.Some(value)
            : source.None();

    /// <summary>
    /// Creates a <c>Fail</c> result.
    /// </summary>
    /// <param name="source">The source factory.</param>
    /// <param name="exception">The exception that caused the failure.</param>
    /// <param name="errorMessage">The optional error message.</param>
    /// <param name="errorCode">The optional error code.</param>
    /// <param name="identifier">The optional identifier of the error.</param>
    /// <param name="type">
    /// The optional type of the error. If <see langword="null"/>, then the
    /// <see cref="MemberInfo.Name"/> of the <see cref="Type"/> of the current instance
    /// is used instead.
    /// </param>
    /// <param name="innerError">
    /// The optional error that is the cause of the current error.
    /// </param>
    /// <returns>A <c>Fail</c> result.</returns>
    public static Result Fail(
        this IResultFactory source,
        Exception exception,
        string? errorMessage = null,
        int? errorCode = null,
        string? identifier = null,
        string? type = null,
        Error? innerError = null) =>
        source.Fail(Error.FromException(exception, errorMessage, errorCode, identifier, type, innerError));

    /// <summary>
    /// Creates a <c>Fail</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the result.</typeparam>
    /// <param name="source">The source factory.</param>
    /// <param name="exception">The exception that caused the failure.</param>
    /// <param name="errorMessage">The optional error message.</param>
    /// <param name="errorCode">The optional error code.</param>
    /// <param name="identifier">The optional identifier of the error.</param>
    /// <param name="type">
    /// The optional type of the error. If <see langword="null"/>, then the
    /// <see cref="MemberInfo.Name"/> of the <see cref="Type"/> of the current instance
    /// is used instead.
    /// </param>
    /// <param name="innerError">
    /// The optional error that is the cause of the current error.
    /// </param>
    /// <returns>A <c>Fail</c> result.</returns>
    public static Result<T> Fail<T>(
        this IResultFactory<T> source,
        Exception exception,
        string? errorMessage = null,
        int? errorCode = null,
        string? identifier = null,
        string? type = null,
        Error? innerError = null) =>
        source.Fail(Error.FromException(exception, errorMessage, errorCode, identifier, type, innerError));

    /// <summary>
    /// Creates a <c>Fail</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the result.</typeparam>
    /// <param name="source">The source factory.</param>
    /// <param name="exception">The exception that caused the failure.</param>
    /// <param name="errorMessage">The optional error message.</param>
    /// <param name="errorCode">The optional error code.</param>
    /// <param name="identifier">The optional identifier of the error.</param>
    /// <param name="type">
    /// The optional type of the error. If <see langword="null"/>, then the
    /// <see cref="MemberInfo.Name"/> of the <see cref="Type"/> of the current instance
    /// is used instead.
    /// </param>
    /// <param name="innerError">
    /// The optional error that is the cause of the current error.
    /// </param>
    /// <returns>A <c>Fail</c> result.</returns>
    public static Maybe<T> Fail<T>(
        this IMaybeFactory<T> source,
        Exception exception,
        string? errorMessage = null,
        int? errorCode = null,
        string? identifier = null,
        string? type = null,
        Error? innerError = null) =>
        source.Fail(Error.FromException(exception, errorMessage, errorCode, identifier, type, innerError));

    /// <summary>
    /// Creates a <c>Fail</c> result.
    /// </summary>
    /// <param name="source">The source factory.</param>
    /// <param name="errorMessage">The error message.</param>
    /// <param name="stackTrace">The optional stack trace.</param>
    /// <param name="errorCode">The optional error code.</param>
    /// <param name="identifier">The optional identifier of the error.</param>
    /// <param name="type">
    /// The optional type of the error. If <see langword="null"/>, then the
    /// <see cref="MemberInfo.Name"/> of the <see cref="Type"/> of the current instance
    /// is used instead.
    /// </param>
    /// <param name="innerError">
    /// The optional error that is the cause of the current error.
    /// </param>
    /// <returns>A <c>Fail</c> result.</returns>
    public static Result Fail(
        this IResultFactory source,
        string errorMessage,
        string? stackTrace = null,
        int? errorCode = null,
        string? identifier = null,
        string? type = null,
        Error? innerError = null) =>
        source.Fail(new Error(errorMessage, stackTrace, errorCode, identifier, type, innerError));

    /// <summary>
    /// Creates a <c>Fail</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the result.</typeparam>
    /// <param name="source">The source factory.</param>
    /// <param name="errorMessage">The error message.</param>
    /// <param name="stackTrace">The optional stack trace.</param>
    /// <param name="errorCode">The optional error code.</param>
    /// <param name="identifier">The optional identifier of the error.</param>
    /// <param name="type">
    /// The optional type of the error. If <see langword="null"/>, then the
    /// <see cref="MemberInfo.Name"/> of the <see cref="Type"/> of the current instance
    /// is used instead.
    /// </param>
    /// <param name="innerError">
    /// The optional error that is the cause of the current error.
    /// </param>
    /// <returns>A <c>Fail</c> result.</returns>
    public static Result<T> Fail<T>(
        this IResultFactory<T> source,
        string errorMessage,
        string? stackTrace = null,
        int? errorCode = null,
        string? identifier = null,
        string? type = null,
        Error? innerError = null) =>
        source.Fail(new Error(errorMessage, stackTrace, errorCode, identifier, type, innerError));

    /// <summary>
    /// Creates a <c>Fail</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the result.</typeparam>
    /// <param name="source">The source factory.</param>
    /// <param name="errorMessage">The error message.</param>
    /// <param name="stackTrace">The optional stack trace.</param>
    /// <param name="errorCode">The optional error code.</param>
    /// <param name="identifier">The optional identifier of the error.</param>
    /// <param name="type">
    /// The optional type of the error. If <see langword="null"/>, then the
    /// <see cref="MemberInfo.Name"/> of the <see cref="Type"/> of the current instance
    /// is used instead.
    /// </param>
    /// <param name="innerError">
    /// The optional error that is the cause of the current error.
    /// </param>
    /// <returns>A <c>Fail</c> result.</returns>
    public static Maybe<T> Fail<T>(
        this IMaybeFactory<T> source,
        string errorMessage,
        string? stackTrace = null,
        int? errorCode = null,
        string? identifier = null,
        string? type = null,
        Error? innerError = null) =>
        source.Fail(new Error(errorMessage, stackTrace, errorCode, identifier, type, innerError));
}
