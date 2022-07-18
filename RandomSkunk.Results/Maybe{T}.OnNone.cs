namespace RandomSkunk.Results;

/// <content> Defines the <c>OnNone</c> and <c>OnNoneAsync</c> methods. </content>
public partial struct Maybe<T>
{
    /// <summary>
    /// Invokes the <paramref name="onNone"/> function if the current result is a <c>None</c> result.
    /// </summary>
    /// <param name="onNone">A callback function to invoke if this is a <c>None</c> result.</param>
    /// <returns>The current result.</returns>
    public Maybe<T> OnNone(Action onNone)
    {
        if (onNone is null) throw new ArgumentNullException(nameof(onNone));

        if (_type == MaybeType.None)
            onNone();

        return this;
    }

    /// <summary>
    /// Invokes the <paramref name="onNone"/> function if the current result is a <c>None</c> result.
    /// </summary>
    /// <param name="onNone">A callback function to invoke if this is a <c>None</c> result.</param>
    /// <returns>The current result.</returns>
    public async Task<Maybe<T>> OnNoneAsync(Func<Task> onNone)
    {
        if (onNone is null) throw new ArgumentNullException(nameof(onNone));

        if (_type == MaybeType.None)
            await onNone().ConfigureAwait(false);

        return this;
    }
}
