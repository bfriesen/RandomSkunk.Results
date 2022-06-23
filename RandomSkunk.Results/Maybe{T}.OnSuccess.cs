namespace RandomSkunk.Results;

/// <content>
/// Defines the <c>OnSuccess</c> and <c>OnSuccessAsync</c> methods.
/// </content>
public partial struct Maybe<T>
{
    /// <summary>
    /// Invokes the <paramref name="onSuccess"/> function if the current result is a <c>Success</c> result.
    /// </summary>
    /// <param name="onSuccess">A callback function to invoke if the source is a <c>Success</c> result.</param>
    /// <returns>The current result.</returns>
    public Maybe<T> OnSuccess(Action<T> onSuccess)
    {
        if (_type == MaybeType.Success)
            onSuccess(_value!);

        return this;
    }

    /// <summary>
    /// Invokes the <paramref name="onSuccess"/> function if the current result is a <c>Success</c> result.
    /// </summary>
    /// <param name="onSuccess">A callback function to invoke if the source is a <c>Success</c> result.</param>
    /// <returns>The current result.</returns>
    public async Task<Maybe<T>> OnSuccessAsync(Func<T, Task> onSuccess)
    {
        if (_type == MaybeType.Success)
            await onSuccess(_value!).ConfigureAwait(false);

        return this;
    }
}
