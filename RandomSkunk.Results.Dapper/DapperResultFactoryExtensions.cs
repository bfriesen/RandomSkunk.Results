using RandomSkunk.Results.Dapper;

namespace RandomSkunk.Results;

/// <summary>
/// Defines extension methods for result factories.
/// </summary>
public static class DapperResultFactoryExtensions
{
    /// <summary>
    /// Creates a <c>Fail</c> result with a <see cref="DbError"/>.
    /// </summary>
    /// <param name="source">The source factory.</param>
    /// <param name="exception">The exception that caused the failure.</param>
    /// <param name="errorMessage">The optional error message.</param>
    /// <param name="errorCode">The optional error code.</param>
    /// <param name="errorIdentifier">The optional identifier of the error.</param>
    /// <returns>A <c>Fail</c> result.</returns>
    public static Result DbException(
        this ResultFailFactory source,
        DbException exception,
        string? errorMessage = null,
        int? errorCode = null,
        string? errorIdentifier = null) =>
        source.Error(DbError.FromDbException(exception, errorMessage, errorCode, errorIdentifier));

    /// <summary>
    /// Creates a <c>Fail</c> result with a <see cref="DbError"/>.
    /// </summary>
    /// <typeparam name="T">The type of the result.</typeparam>
    /// <param name="source">The source factory.</param>
    /// <param name="exception">The exception that caused the failure.</param>
    /// <param name="errorMessage">The optional error message.</param>
    /// <param name="errorCode">The optional error code.</param>
    /// <param name="errorIdentifier">The optional identifier of the error.</param>
    /// <returns>A <c>Fail</c> result.</returns>
    public static Result<T> DbException<T>(
        this ResultFailFactory<T> source,
        DbException exception,
        string? errorMessage = null,
        int? errorCode = null,
        string? errorIdentifier = null) =>
        source.Error(DbError.FromDbException(exception, errorMessage, errorCode, errorIdentifier));

    /// <summary>
    /// Creates a <c>Fail</c> result with a <see cref="DbError"/>.
    /// </summary>
    /// <typeparam name="T">The type of the result.</typeparam>
    /// <param name="source">The source factory.</param>
    /// <param name="exception">The exception that caused the failure.</param>
    /// <param name="errorMessage">The optional error message.</param>
    /// <param name="errorCode">The optional error code.</param>
    /// <param name="errorIdentifier">The optional identifier of the error.</param>
    /// <returns>A <c>Fail</c> result.</returns>
    public static Maybe<T> DbException<T>(
        this MaybeFailFactory<T> source,
        DbException exception,
        string? errorMessage = null,
        int? errorCode = null,
        string? errorIdentifier = null) =>
        source.Error(DbError.FromDbException(exception, errorMessage, errorCode, errorIdentifier));
}
