namespace RandomSkunk.Results.Dapper;

/// <summary>
/// Defines extension methods for the results of execute database query operations.
/// </summary>
public static class ExecuteResultExtensions
{
    /// <summary>
    /// Returns <paramref name="sourceResult"/> if it is a <c>Success</c> result and its value is one; otherwise, returns a
    /// <c>Fail</c> result.
    /// </summary>
    /// <param name="sourceResult">A result whose value represents the number of rows affected by a database query.</param>
    /// <param name="errorIdentifier">The optional error identifier to use for a <c>Fail</c> result.</param>
    /// <param name="errorCode">The optional error code to use for a <c>Fail</c> result.</param>
    /// <returns>A result representing a database query that affected one row.</returns>
    public static Result<int> EnsureOneRowAffected(
        this Result<int> sourceResult,
        string? errorIdentifier = null,
        int? errorCode = null) =>
        sourceResult.EnsureNRowsAffected(1, errorIdentifier, errorCode);

    /// <summary>
    /// Returns <paramref name="sourceResult"/> if it is a <c>Success</c> result and its value is one; otherwise, returns a
    /// <c>Fail</c> result.
    /// </summary>
    /// <param name="sourceResult">A result whose value represents the number of rows affected by a database query.</param>
    /// <param name="errorIdentifier">The optional error identifier to use for a <c>Fail</c> result.</param>
    /// <param name="errorCode">The optional error code to use for a <c>Fail</c> result.</param>
    /// <returns>A result representing a database query that affected one row.</returns>
    public static async Task<Result<int>> EnsureOneRowAffected(
        this Task<Result<int>> sourceResult,
        string? errorIdentifier = null,
        int? errorCode = null) =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).EnsureOneRowAffected(errorIdentifier, errorCode);

    /// <summary>
    /// Returns <paramref name="sourceResult"/> if it is a <c>Success</c> result and its value is equal to
    /// <paramref name="affectedRows"/>; otherwise, returns a <c>Fail</c> result.
    /// </summary>
    /// <param name="sourceResult">A result whose value represents the number of rows affected by a database query.</param>
    /// <param name="affectedRows">The expected number of affected rows.</param>
    /// <param name="errorIdentifier">The optional error identifier to use for a <c>Fail</c> result.</param>
    /// <param name="errorCode">The optional error code to use for a <c>Fail</c> result.</param>
    /// <returns>A result representing a database query that affected N rows.</returns>
    public static Result<int> EnsureNRowsAffected(
        this Result<int> sourceResult,
        int affectedRows,
        string? errorIdentifier = null,
        int? errorCode = null) =>
        sourceResult.ToFailIf(
            affectedRowCount => affectedRowCount == affectedRows,
            affectedRowCount => new Error
            {
                Message = $"Expected {affectedRows} row{(affectedRows != 1 ? "s" : null)} to be affected, but was {affectedRowCount}.",
                Identifier = errorIdentifier,
                ErrorCode = errorCode,
            });

    /// <summary>
    /// Returns <paramref name="sourceResult"/> if it is a <c>Success</c> result and its value is equal to
    /// <paramref name="affectedRows"/>; otherwise, returns a <c>Fail</c> result.
    /// </summary>
    /// <param name="sourceResult">A result whose value represents the number of rows affected by a database query.</param>
    /// <param name="affectedRows">The expected number of affected rows.</param>
    /// <param name="errorIdentifier">The optional error identifier to use for a <c>Fail</c> result.</param>
    /// <param name="errorCode">The optional error code to use for a <c>Fail</c> result.</param>
    /// <returns>A result representing a database query that affected N rows.</returns>
    public static async Task<Result<int>> EnsureNRowsAffected(
        this Task<Result<int>> sourceResult,
        int affectedRows,
        string? errorIdentifier = null,
        int? errorCode = null) =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).EnsureNRowsAffected(affectedRows, errorIdentifier, errorCode);
}
