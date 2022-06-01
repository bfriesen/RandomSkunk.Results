namespace RandomSkunk.Results;

/// <content>
/// Defines the <c>OnSuccess</c> and <c>OnSuccessAsync</c> methods.
/// </content>
public partial struct Result
{
    /// <summary>
    /// Invokes the <paramref name="onSuccess"/> function if the current result is a <c>Success</c> result.
    /// </summary>
    /// <param name="onSuccess">A callback function to invoke if the source is a <c>Success</c> result.</param>
    /// <returns>The current result.</returns>
    public Result OnSuccess(Action onSuccess)
    {
        if (IsSuccess)
            onSuccess();

        return this;
    }

    /// <summary>
    /// Invokes the <paramref name="onSuccess"/> function if the current result is a <c>Success</c> result.
    /// </summary>
    /// <param name="onSuccess">A callback function to invoke if the source is a <c>Success</c> result.</param>
    /// <returns>The current result.</returns>
    public async Task<Result> OnSuccessAsync(Func<Task> onSuccess)
    {
        if (IsSuccess)
            await onSuccess().ConfigureAwait(false);

        return this;
    }
}