namespace RandomSkunk.Results;

/// <content> Defines the <c>OnSuccess</c> and <c>OnSuccessAsync</c> methods. </content>
public partial struct Result
{
    /// <summary>
    /// Invokes the <paramref name="onSuccessCallback"/> function if the current result is a <c>Success</c> result.
    /// </summary>
    /// <param name="onSuccessCallback">A callback function to invoke if this is a <c>Success</c> result.</param>
    /// <returns>The current result.</returns>
    public Result OnSuccess(Action onSuccessCallback)
    {
        if (onSuccessCallback is null) throw new ArgumentNullException(nameof(onSuccessCallback));

        if (_type == ResultType.Success)
            onSuccessCallback();

        return this;
    }

    /// <summary>
    /// Invokes the <paramref name="onSuccessCallback"/> function if the current result is a <c>Success</c> result.
    /// </summary>
    /// <param name="onSuccessCallback">A callback function to invoke if this is a <c>Success</c> result.</param>
    /// <returns>The current result.</returns>
    public async Task<Result> OnSuccessAsync(Func<Task> onSuccessCallback)
    {
        if (onSuccessCallback is null) throw new ArgumentNullException(nameof(onSuccessCallback));

        if (_type == ResultType.Success)
            await onSuccessCallback().ConfigureAwait(false);

        return this;
    }
}
