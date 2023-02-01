namespace RandomSkunk.Results;

/// <content> Defines the <c>AsMaybe</c> method. </content>
public partial struct Result<T>
{
    /// <summary>
    /// Converts this <see cref="Result{T}"/> to an equivalent <see cref="Maybe{T}"/>. If this is a <c>Success</c> result, then a
    /// <c>Success</c> result with the same value is returned. Otherwise, if the <c>Fail</c> result's error has error code
    /// <see cref="ErrorCodes.NoValue"/>, then a <c>None</c> result is returned. For any other error code, a new <c>Fail</c>
    /// result with the same error is returned.
    /// </summary>
    /// <returns>The equivalent <see cref="Result{T}"/>.</returns>
    public Maybe<T> AsMaybe() =>
        SelectMany(Maybe<T>.Success);
}

/// <content> Defines the <c>AsMaybe</c> extension method. </content>
public static partial class ResultExtensions
{
    /// <summary>
    /// Converts this <see cref="Result{T}"/> to an equivalent <see cref="Maybe{T}"/>. If this is a <c>Success</c> result, then a
    /// <c>Success</c> result with the same value is returned. Otherwise, if the <c>Fail</c> result's error has error code
    /// <see cref="ErrorCodes.NoValue"/>, then a <c>None</c> result is returned. For any other error code, a new <c>Fail</c>
    /// result with the same error is returned.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <returns>The equivalent <see cref="Result{T}"/>.</returns>
    public static async Task<Maybe<T>> AsMaybe<T>(this Task<Result<T>> sourceResult) =>
        (await sourceResult.ConfigureAwait(false)).AsMaybe();
}
