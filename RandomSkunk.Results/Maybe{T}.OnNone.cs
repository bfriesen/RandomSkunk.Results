namespace RandomSkunk.Results;

/// <content> Defines the <c>OnNone</c> and <c>OnNoneAsync</c> methods. </content>
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

        if (_type == MaybeType.None)
            onNoneCallback();

        return this;
    }

    /// <summary>
    /// Invokes the <paramref name="onNoneCallback"/> function if the current result is a <c>None</c> result.
    /// </summary>
    /// <param name="onNoneCallback">A callback function to invoke if this is a <c>None</c> result.</param>
    /// <returns>The current result.</returns>
    public async Task<Maybe<T>> OnNoneAsync(Func<Task> onNoneCallback)
    {
        if (onNoneCallback is null) throw new ArgumentNullException(nameof(onNoneCallback));

        if (_type == MaybeType.None)
            await onNoneCallback().ConfigureAwait(false);

        return this;
    }
}
