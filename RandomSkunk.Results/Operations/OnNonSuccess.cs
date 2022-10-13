namespace RandomSkunk.Results;

/// <content> Defines the <c>OnNonSuccess</c> extension methods. </content>
public static partial class ResultExtensions
{
    /// <summary>
    /// Invokes the <paramref name="onNonSuccessCallback"/> function if the current result is a <c>non-Success</c> result.
    /// </summary>
    /// <typeparam name="TResult">The type of result.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onNonSuccessCallback">A callback function to invoke if this is a <c>non-Success</c> result.</param>
    /// <returns>The current result.</returns>
    public static TResult OnNonSuccess<TResult>(
        this TResult sourceResult,
        Action<Error> onNonSuccessCallback)
        where TResult : IResult
    {
        if (onNonSuccessCallback is null) throw new ArgumentNullException(nameof(onNonSuccessCallback));

        if (!sourceResult.IsSuccess)
        {
            var error = sourceResult.GetNonSuccessError();
            onNonSuccessCallback(error);
        }

        return sourceResult;
    }

    /// <inheritdoc cref="OnNonSuccess{TResult}(TResult, Action{Error})"/>
    public static async Task<TResult> OnNonSuccess<TResult>(
        this TResult sourceResult,
        Func<Error, Task> onNonSuccessCallback)
        where TResult : IResult
    {
        if (onNonSuccessCallback is null) throw new ArgumentNullException(nameof(onNonSuccessCallback));

        if (!sourceResult.IsSuccess)
        {
            var error = sourceResult.GetNonSuccessError();
            await onNonSuccessCallback(error).ConfigureAwait(false);
        }

        return sourceResult;
    }

    /// <inheritdoc cref="OnNonSuccess{TResult}(TResult, Action{Error})"/>
    public static async Task<TResult> OnNonSuccess<TResult>(
        this Task<TResult> sourceResult,
        Action<Error> onNonSuccessCallback)
        where TResult : IResult =>
        (await sourceResult.ConfigureAwait(false)).OnNonSuccess(onNonSuccessCallback);

    /// <inheritdoc cref="OnNonSuccess{TResult}(TResult, Action{Error})"/>
    public static async Task<TResult> OnNonSuccess<TResult>(
        this Task<TResult> sourceResult,
        Func<Error, Task> onNonSuccessCallback)
        where TResult : IResult =>
        await (await sourceResult.ConfigureAwait(false)).OnNonSuccess(onNonSuccessCallback).ConfigureAwait(false);
}
