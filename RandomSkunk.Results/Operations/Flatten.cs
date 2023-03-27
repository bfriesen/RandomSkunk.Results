using static RandomSkunk.Results.AwaitSettings;

namespace RandomSkunk.Results;

/// <content> Defines the <c>Flatten</c> extension methods. </content>
public static partial class ResultExtensions
{
    /// <summary>
    /// Flattens the nested result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <returns>The flattened result.</returns>
    public static Result<T> Flatten<T>(this Result<Result<T>> sourceResult) =>
        sourceResult.SelectMany(nestedResult => nestedResult);

    /// <summary>
    /// Flattens the nested result.
    /// </summary>
    /// <param name="sourceResult">The source result.</param>
    /// <returns>The flattened result.</returns>
    public static Result Flatten(this Result<Result> sourceResult) =>
        sourceResult.SelectMany(nestedResult => nestedResult);

    /// <summary>
    /// Flattens the nested result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <returns>The flattened result.</returns>
    public static Maybe<T> Flatten<T>(this Result<Maybe<T>> sourceResult) =>
        sourceResult.SelectMany(nestedResult => nestedResult);

    /// <summary>
    /// Flattens the nested result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <returns>The flattened result.</returns>
    public static Maybe<T> Flatten<T>(this Maybe<Maybe<T>> sourceResult) =>
        sourceResult.SelectMany(nestedResult => nestedResult);

    /// <summary>
    /// Flattens the nested result.
    /// </summary>
    /// <param name="sourceResult">The source result.</param>
    /// <returns>The flattened result.</returns>
    public static Result Flatten(this Maybe<Result> sourceResult) =>
        sourceResult.SelectMany(nestedResult => nestedResult);

    /// <summary>
    /// Flattens the nested result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <returns>The flattened result.</returns>
    public static Result<T> Flatten<T>(this Maybe<Result<T>> sourceResult) =>
        sourceResult.SelectMany(nestedResult => nestedResult);

    /// <summary>
    /// Flattens the nested result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <returns>The flattened result.</returns>
    public static async Task<Result<T>> Flatten<T>(this Task<Result<Result<T>>> sourceResult) =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).Flatten();

    /// <summary>
    /// Flattens the nested result.
    /// </summary>
    /// <param name="sourceResult">The source result.</param>
    /// <returns>The flattened result.</returns>
    public static async Task<Result> Flatten(this Task<Result<Result>> sourceResult) =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).Flatten();

    /// <summary>
    /// Flattens the nested result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <returns>The flattened result.</returns>
    public static async Task<Maybe<T>> Flatten<T>(this Task<Result<Maybe<T>>> sourceResult) =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).Flatten();

    /// <summary>
    /// Flattens the nested result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <returns>The flattened result.</returns>
    public static async Task<Maybe<T>> Flatten<T>(this Task<Maybe<Maybe<T>>> sourceResult) =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).Flatten();

    /// <summary>
    /// Flattens the nested result.
    /// </summary>
    /// <param name="sourceResult">The source result.</param>
    /// <returns>The flattened result.</returns>
    public static async Task<Result> Flatten(this Task<Maybe<Result>> sourceResult) =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).Flatten();

    /// <summary>
    /// Flattens the nested result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <returns>The flattened result.</returns>
    public static async Task<Result<T>> Flatten<T>(this Task<Maybe<Result<T>>> sourceResult) =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).Flatten();
}
