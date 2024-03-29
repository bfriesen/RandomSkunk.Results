namespace RandomSkunk.Results.Dapper;

/// <summary>
/// Defines extension methods for the results of execute database query operations.
/// </summary>
public static class ExecuteResultExtensions
{
    /// <summary>
    /// Returns a <c>Success</c> result if <paramref name="sourceResult"/> is a <c>Success</c> result and its value is one;
    /// otherwise, returns a <c>Fail</c> result.
    /// </summary>
    /// <param name="sourceResult">A result whose value represents the number of rows affected by a database query.</param>
    /// <returns>A result representing a database query that affected one row.</returns>
    public static Result EnsureOneRowAffected(this Result<int> sourceResult) =>
        sourceResult.EnsureNRowsAffected(1);

    /// <summary>
    /// Returns a <c>Success</c> result if <paramref name="sourceResult"/> is a <c>Success</c> result and its value is one;
    /// otherwise, returns a <c>Fail</c> result.
    /// </summary>
    /// <param name="sourceResult">A result whose value represents the number of rows affected by a database query.</param>
    /// <returns>A result representing a database query that affected one row.</returns>
    public static async Task<Result> EnsureOneRowAffected(this Task<Result<int>> sourceResult) =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).EnsureOneRowAffected();

    /// <summary>
    /// Returns a <c>Success</c> result if <paramref name="sourceResult"/> is a <c>Success</c> result and its value is equal to
    /// <paramref name="affectedRows"/>; otherwise, returns a <c>Fail</c> result.
    /// </summary>
    /// <param name="sourceResult">A result whose value represents the number of rows affected by a database query.</param>
    /// <param name="affectedRows">The expected number of affected rows.</param>
    /// <returns>A result representing a database query that affected N rows.</returns>
    public static Result EnsureNRowsAffected(this Result<int> sourceResult, int affectedRows) =>
        sourceResult.SelectMany(affectedRowCount =>
            affectedRowCount == affectedRows
                ? Result.Success()
                : Result.Fail($"Expected {affectedRows} row{(affectedRows != 1 ? "s" : null)} to be affected, but was {affectedRowCount}."));

    /// <summary>
    /// Returns a <c>Success</c> result if <paramref name="sourceResult"/> is a <c>Success</c> result and its value is equal to
    /// <paramref name="affectedRows"/>; otherwise, returns a <c>Fail</c> result.
    /// </summary>
    /// <param name="sourceResult">A result whose value represents the number of rows affected by a database query.</param>
    /// <param name="affectedRows">The expected number of affected rows.</param>
    /// <returns>A result representing a database query that affected N rows.</returns>
    public static async Task<Result> EnsureNRowsAffected(this Task<Result<int>> sourceResult, int affectedRows) =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).EnsureNRowsAffected(affectedRows);
}
