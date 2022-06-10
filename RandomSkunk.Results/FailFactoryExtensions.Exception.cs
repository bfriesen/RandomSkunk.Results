using static RandomSkunk.Results.Error;

namespace RandomSkunk.Results;

/// <content>
/// Defines the <c>Exception</c> extension methods.
/// </content>
public static partial class FailFactoryExtensions
{
    /// <summary>
    /// Creates a <c>Fail</c> result from the specified exception.
    /// </summary>
    /// <param name="source">The source factory.</param>
    /// <param name="exception">The exception that caused the failure.</param>
    /// <param name="errorMessage">The optional error message.</param>
    /// <param name="errorCode">The optional error code.</param>
    /// <param name="errorIdentifier">The optional identifier of the error.</param>
    /// <returns>A <c>Fail</c> result.</returns>
    public static Result Exception(
        this ResultFailFactory source,
        Exception exception,
        string? errorMessage = null,
        int? errorCode = null,
        string? errorIdentifier = null) =>
        source.Error(FromException(exception, errorMessage, errorCode, errorIdentifier));

    /// <summary>
    /// Creates a <c>Fail</c> result from the specified exception.
    /// </summary>
    /// <typeparam name="T">The type of the result.</typeparam>
    /// <param name="source">The source factory.</param>
    /// <param name="exception">The exception that caused the failure.</param>
    /// <param name="errorMessage">The optional error message.</param>
    /// <param name="errorCode">The optional error code.</param>
    /// <param name="errorIdentifier">The optional identifier of the error.</param>
    /// <returns>A <c>Fail</c> result.</returns>
    public static Result<T> Exception<T>(
        this ResultFailFactory<T> source,
        Exception exception,
        string? errorMessage = null,
        int? errorCode = null,
        string? errorIdentifier = null) =>
        source.Error(FromException(exception, errorMessage, errorCode, errorIdentifier));

    /// <summary>
    /// Creates a <c>Fail</c> result from the specified exception.
    /// </summary>
    /// <typeparam name="T">The type of the result.</typeparam>
    /// <param name="source">The source factory.</param>
    /// <param name="exception">The exception that caused the failure.</param>
    /// <param name="errorMessage">The optional error message.</param>
    /// <param name="errorCode">The optional error code.</param>
    /// <param name="errorIdentifier">The optional identifier of the error.</param>
    /// <returns>A <c>Fail</c> result.</returns>
    public static Maybe<T> Exception<T>(
        this MaybeFailFactory<T> source,
        Exception exception,
        string? errorMessage = null,
        int? errorCode = null,
        string? errorIdentifier = null) =>
        source.Error(FromException(exception, errorMessage, errorCode, errorIdentifier));
}
