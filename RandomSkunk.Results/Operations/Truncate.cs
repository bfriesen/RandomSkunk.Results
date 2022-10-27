namespace RandomSkunk.Results;

/// <content> Defines the <c>Truncate</c> method. </content>
public partial struct Result<T>
{
    /// <summary>
    /// Truncates the <see cref="Result{T}"/> into an equivalent <see cref="Result"/>. If it is a <c>Success</c> result, then its
    /// value is ignored and a valueless <c>Success</c> result is returned. Otherwise, a <c>Fail</c> result with the same error
    /// as is returned.
    /// </summary>
    /// <returns>An equivalent <see cref="Result"/>.</returns>
    public Result Truncate() =>
        SelectMany(_ => Result.Success());
}

/// <content> Defines the <c>Truncate</c> method. </content>
public partial struct Maybe<T>
{
    /// <summary>
    /// Truncates the <see cref="Maybe{T}"/> into an equivalent <see cref="Result"/>. If it is a <c>Success</c> result, then its
    /// value is ignored and a valueless <c>Success</c> result is returned. If it is a <c>None</c> result, then a <c>Fail</c>
    /// result with error code <see cref="ErrorCodes.NoneResult"/> is returned. Otherwise, a <c>Fail</c> result with the same
    /// error as is returned.
    /// </summary>
    /// <returns>An equivalent <see cref="Result"/>.</returns>
    public Result Truncate() =>
        SelectMany(_ => Result.Success());
}

/// <content> Defines the <c>Truncate</c> extension methods. </content>
public static partial class ResultExtensions
{
    /// <summary>
    /// Truncates the <see cref="Result{T}"/> into an equivalent <see cref="Result"/>. If it is a <c>Success</c> result, then its
    /// value is ignored and a valueless <c>Success</c> result is returned. Otherwise, a <c>Fail</c> result with the same error
    /// as is returned.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <returns>An equivalent <see cref="Result"/>.</returns>
    public static async Task<Result> Truncate<T>(this Task<Result<T>> sourceResult) =>
        (await sourceResult).Truncate();

    /// <summary>
    /// Truncates the <see cref="Maybe{T}"/> into an equivalent <see cref="Result"/>. If it is a <c>Success</c> result, then its
    /// value is ignored and a valueless <c>Success</c> result is returned. If it is a <c>None</c> result, then a <c>Fail</c>
    /// result with error code <see cref="ErrorCodes.NoneResult"/> is returned. Otherwise, a <c>Fail</c> result with the same
    /// error as is returned.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <returns>An equivalent <see cref="Result"/>.</returns>
    public static async Task<Result> Truncate<T>(this Task<Maybe<T>> sourceResult) =>
        (await sourceResult).Truncate();
}
