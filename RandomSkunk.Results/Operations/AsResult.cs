namespace RandomSkunk.Results;

/// <content> Defines the <c>AsResult</c> method. </content>
public partial struct Maybe<T>
{
    /// <summary>
    /// Converts this <see cref="Maybe{T}"/> to an equivalent <see cref="Result{T}"/>. If this is a <c>Success</c> result, then a
    /// <c>Success</c> result with the same value is returned. If this is a <c>Fail</c> result, then a <c>Fail</c> result with
    /// the same error is returned. Otherwise, if this is a <c>None</c> result, then a <c>Fail</c> result with error code
    /// <see cref="ErrorCodes.NoValue"/> is returned.
    /// </summary>
    /// <param name="onNoneSelector">An optional transform function to apply to a <c>None</c> result.</param>
    /// <returns>The equivalent <see cref="Result{T}"/>.</returns>
    public Result<T> AsResult(Func<Result<T>>? onNoneSelector) =>
        SelectMany(Result<T>.Success, onNoneSelector);

    /// <summary>
    /// Converts this <see cref="Maybe{T}"/> to an equivalent <see cref="Result{T}"/>. If this is a <c>Success</c> result, then a
    /// <c>Success</c> result with the same value is returned. If this is a <c>Fail</c> result, then a <c>Fail</c> result with
    /// the same error is returned. Otherwise, if this is a <c>None</c> result, then a <c>Fail</c> result with error code
    /// <see cref="ErrorCodes.NoValue"/> is returned.
    /// </summary>
    /// <returns>The equivalent <see cref="Result{T}"/>.</returns>
    public Result<T> AsResult() =>
        AsResult(null);
}

/// <content> Defines the <c>AsResult</c> extension method. </content>
public static partial class ResultExtensions
{
    /// <summary>
    /// Converts this <see cref="Maybe{T}"/> to an equivalent <see cref="Result{T}"/>. If this is a <c>Success</c> result, then a
    /// <c>Success</c> result with the same value is returned. If this is a <c>Fail</c> result, then a <c>Fail</c> result with
    /// the same error is returned. Otherwise, if this is a <c>None</c> result, then a <c>Fail</c> result with error code
    /// <see cref="ErrorCodes.NoValue"/> is returned.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onNoneSelector">An optional transform function to apply to a <c>None</c> result.</param>
    /// <returns>The equivalent <see cref="Result{T}"/>.</returns>
    public static async Task<Result<T>> AsResult<T>(this Task<Maybe<T>> sourceResult, Func<Result<T>>? onNoneSelector) =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).AsResult(onNoneSelector);

    /// <summary>
    /// Converts this <see cref="Maybe{T}"/> to an equivalent <see cref="Result{T}"/>. If this is a <c>Success</c> result, then a
    /// <c>Success</c> result with the same value is returned. If this is a <c>Fail</c> result, then a <c>Fail</c> result with
    /// the same error is returned. Otherwise, if this is a <c>None</c> result, then a <c>Fail</c> result with error code
    /// <see cref="ErrorCodes.NoValue"/> is returned.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <returns>The equivalent <see cref="Result{T}"/>.</returns>
    public static Task<Result<T>> AsResult<T>(this Task<Maybe<T>> sourceResult) =>
        sourceResult.AsResult(null);
}
