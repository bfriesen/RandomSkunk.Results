namespace RandomSkunk.Results;

/// <content> Defines the <c>OnNonSuccess</c> and <c>OnNonSuccessAsync</c> extension methods. </content>
public static partial class ResultExtensions
{
    /// <summary>
    /// Invokes the <paramref name="onNonSuccess"/> function if the current result is a <c>non-Success</c> result.
    /// </summary>
    /// <typeparam name="TResult">The type of result.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onNonSuccess">A callback function to invoke if this is a <c>non-Success</c> result.</param>
    /// <returns>The current result.</returns>
    public static TResult OnNonSuccess<TResult>(
        this TResult sourceResult,
        Action<Error> onNonSuccess)
        where TResult : IResult
    {
        if (onNonSuccess is null) throw new ArgumentNullException(nameof(onNonSuccess));

        if (!sourceResult.IsSuccess)
        {
            var error = sourceResult.GetNonSuccessError();
            onNonSuccess(error);
        }

        return sourceResult;
    }

    /// <summary>
    /// Invokes the <paramref name="onNonSuccess"/> function if the current result is a <c>non-Success</c> result.
    /// </summary>
    /// <typeparam name="TResult">The type of result.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onNonSuccess">A callback function to invoke if this is a <c>non-Success</c> result.</param>
    /// <returns>The current result.</returns>
    public static async Task<TResult> OnNonSuccessAsync<TResult>(
        this TResult sourceResult,
        Func<Error, Task> onNonSuccess)
        where TResult : IResult
    {
        if (onNonSuccess is null) throw new ArgumentNullException(nameof(onNonSuccess));

        if (!sourceResult.IsSuccess)
        {
            var error = sourceResult.GetNonSuccessError();
            await onNonSuccess(error).ConfigureAwait(false);
        }

        return sourceResult;
    }

    /// <summary>
    /// Invokes the <paramref name="onNonSuccessCallback"/> function if the current result is a <c>non-Success</c> result.
    /// </summary>
    /// <typeparam name="TResult">The type of result.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onNonSuccessCallback">A callback function to invoke if the source is a <c>non-Success</c> result.</param>
    /// <returns>The current result.</returns>
    public static async Task<TResult> OnNonSuccess<TResult>(
        this Task<TResult> sourceResult,
        Action<Error> onNonSuccessCallback)
        where TResult : IResult =>
        (await sourceResult.ConfigureAwait(false)).OnNonSuccess(onNonSuccessCallback);

    /// <summary>
    /// Invokes the <paramref name="onNonSuccess"/> function if the current result is a <c>non-Success</c> result.
    /// </summary>
    /// <typeparam name="TResult">The type of result.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onNonSuccess">A callback function to invoke if the source is a <c>non-Success</c> result.</param>
    /// <returns>The current result.</returns>
    public static async Task<TResult> OnNonSuccessAsync<TResult>(
        this Task<TResult> sourceResult,
        Func<Error, Task> onNonSuccess)
        where TResult : IResult =>
        await (await sourceResult.ConfigureAwait(false)).OnNonSuccessAsync(onNonSuccess);
}
