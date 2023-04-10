namespace RandomSkunk.Results;

/// <content> Defines the <c>OnNone</c> methods. </content>
public partial struct Maybe<T>
{
    /// <summary>
    /// Invokes the <paramref name="onNoneCallback"/> function if the current result is a <c>None</c> result.
    /// </summary>
    /// <param name="onNoneCallback">A callback function to invoke if this is a <c>None</c> result.</param>
    /// <returns>The current result.</returns>
    public Maybe<T> OnNone(Action onNoneCallback)
    {
        if (onNoneCallback is null) throw new ArgumentNullException(nameof(onNoneCallback));

        if (_outcome == Outcome.None)
        {
            try
            {
                onNoneCallback();
            }
            catch (TaskCanceledException ex)
            {
                return Errors.Canceled(ex);
            }
            catch (Exception ex)
            {
                return Fail(ex, Error.GetMessageForExceptionThrownInCallback(nameof(onNoneCallback)));
            }
        }

        return this;
    }

    /// <summary>
    /// Invokes the <paramref name="onNoneCallback"/> function if the current result is a <c>None</c> result.
    /// </summary>
    /// <param name="onNoneCallback">A callback function to invoke if this is a <c>None</c> result.</param>
    /// <returns>The current result.</returns>
    public async Task<Maybe<T>> OnNone(Func<Task> onNoneCallback)
    {
        if (onNoneCallback is null) throw new ArgumentNullException(nameof(onNoneCallback));

        if (_outcome == Outcome.None)
        {
            try
            {
                await onNoneCallback().ConfigureAwait(ContinueOnCapturedContext);
            }
            catch (TaskCanceledException ex)
            {
                return Errors.Canceled(ex);
            }
            catch (Exception ex)
            {
                return Fail(ex, Error.GetMessageForExceptionThrownInCallback(nameof(onNoneCallback)));
            }
        }

        return this;
    }
}

/// <content> Defines the <c>OnNone</c> extension methods. </content>
public static partial class ResultExtensions
{
    /// <summary>
    /// Invokes the <paramref name="onNoneCallback"/> function if <paramref name="sourceResult"/> is a <c>None</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onNoneCallback">A callback function to invoke if the source is a <c>None</c> result.</param>
    /// <returns>The <paramref name="sourceResult"/> result.</returns>
    public static async Task<Maybe<T>> OnNone<T>(this Task<Maybe<T>> sourceResult, Action onNoneCallback) =>
        (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).OnNone(onNoneCallback);

    /// <summary>
    /// Invokes the <paramref name="onNoneCallback"/> function if <paramref name="sourceResult"/> is a <c>None</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onNoneCallback">A callback function to invoke if the source is a <c>None</c> result.</param>
    /// <returns>The <paramref name="sourceResult"/> result.</returns>
    public static async Task<Maybe<T>> OnNone<T>(this Task<Maybe<T>> sourceResult, Func<Task> onNoneCallback) =>
        await (await sourceResult.ConfigureAwait(ContinueOnCapturedContext)).OnNone(onNoneCallback).ConfigureAwait(ContinueOnCapturedContext);
}
