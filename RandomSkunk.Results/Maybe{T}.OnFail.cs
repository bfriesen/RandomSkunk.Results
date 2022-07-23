namespace RandomSkunk.Results;

/// <content> Defines the <c>OnFail</c> and <c>OnFailAsync</c> methods. </content>
public partial struct Maybe<T>
{
    /// <summary>
    /// Invokes the <paramref name="onFailCallback"/> function if the current result is a <c>Fail</c> result.
    /// </summary>
    /// <param name="onFailCallback">A callback function to invoke if this is a <c>Fail</c> result.</param>
    /// <returns>The current result.</returns>
    public Maybe<T> OnFail(Action<Error> onFailCallback)
    {
        if (onFailCallback is null) throw new ArgumentNullException(nameof(onFailCallback));

        if (_type == MaybeType.Fail)
            onFailCallback(Error());

        return this;
    }

    /// <summary>
    /// Invokes the <paramref name="onFailCallback"/> function if the current result is a <c>Fail</c> result.
    /// </summary>
    /// <param name="onFailCallback">A callback function to invoke if this is a <c>Fail</c> result.</param>
    /// <returns>The current result.</returns>
    public async Task<Maybe<T>> OnFailAsync(Func<Error, Task> onFailCallback)
    {
        if (onFailCallback is null) throw new ArgumentNullException(nameof(onFailCallback));

        if (_type == MaybeType.Fail)
            await onFailCallback(Error()).ConfigureAwait(false);

        return this;
    }
}
