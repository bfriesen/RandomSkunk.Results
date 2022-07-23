namespace RandomSkunk.Results;

/// <content> Defines the <c>OnSuccess</c> and <c>OnSuccessAsync</c> methods. </content>
public partial struct Result<T>
{
    /// <summary>
    /// Invokes the <paramref name="onSuccessCallback"/> function if the current result is a <c>Success</c> result.
    /// </summary>
    /// <param name="onSuccessCallback">A callback function to invoke if this is a <c>Success</c> result.</param>
    /// <returns>The current result.</returns>
    public Result<T> OnSuccess(Action<T> onSuccessCallback)
    {
        if (onSuccessCallback is null) throw new ArgumentNullException(nameof(onSuccessCallback));

        if (_type == ResultType.Success)
            onSuccessCallback(_value!);

        return this;
    }

    /// <summary>
    /// Invokes the <paramref name="onSuccessCallback"/> function if the current result is a <c>Success</c> result.
    /// </summary>
    /// <param name="onSuccessCallback">A callback function to invoke if this is a <c>Success</c> result.</param>
    /// <returns>The current result.</returns>
    public async Task<Result<T>> OnSuccessAsync(Func<T, Task> onSuccessCallback)
    {
        if (onSuccessCallback is null) throw new ArgumentNullException(nameof(onSuccessCallback));

        if (_type == ResultType.Success)
            await onSuccessCallback(_value!).ConfigureAwait(false);

        return this;
    }
}
