namespace RandomSkunk.Results;

/// <content> Defines the <c>AsNonNullable</c> extension methods. </content>
public static partial class ResultExtensions
{
    /// <summary>
    /// Gets an equivalent result with a non-nullable type.
    /// </summary>
    /// <typeparam name="T">The nullable type of the source result.</typeparam>
    /// <param name="result">The source result.</param>
    /// <returns>The equivalant result.</returns>
    public static Result<T> AsNonNullable<T>(this Result<T?> result)
        where T : struct =>
        result.Select(value => value!.Value);

    /// <summary>
    /// Gets an equivalent result with a non-nullable type.
    /// </summary>
    /// <typeparam name="T">The nullable type of the source result.</typeparam>
    /// <param name="result">The source result.</param>
    /// <returns>The equivalant result.</returns>
    public static Maybe<T> AsNonNullable<T>(this Maybe<T?> result)
        where T : struct =>
        result.Select(value => value!.Value);

    /// <summary>
    /// Gets an equivalent result with a non-nullable type.
    /// </summary>
    /// <typeparam name="T">The nullable type of the source result.</typeparam>
    /// <param name="result">The source result.</param>
    /// <returns>The equivalant result.</returns>
    public static async Task<Result<T>> AsNonNullable<T>(this Task<Result<T?>> result)
        where T : struct =>
        (await result.ConfigureAwait(ContinueOnCapturedContext)).AsNonNullable();

    /// <summary>
    /// Gets an equivalent result with a non-nullable type.
    /// </summary>
    /// <typeparam name="T">The nullable type of the source result.</typeparam>
    /// <param name="result">The source result.</param>
    /// <returns>The equivalant result.</returns>
    public static async Task<Maybe<T>> AsNonNullable<T>(this Task<Maybe<T?>> result)
        where T : struct =>
        (await result.ConfigureAwait(ContinueOnCapturedContext)).AsNonNullable();
}
