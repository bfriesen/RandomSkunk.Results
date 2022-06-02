namespace RandomSkunk.Results;

/// <content>
/// Defines the <c>OnFail</c> and <c>OnFailAsync</c> methods.
/// </content>
public partial struct Maybe<T>
{
    /// <summary>
    /// Invokes the <paramref name="onFail"/> function if the current result is a <c>Fail</c> result.
    /// </summary>
    /// <param name="onFail">A callback function to invoke if the source is a <c>Fail</c> result.</param>
    /// <returns>The current result.</returns>
    public Maybe<T> OnFail(Action<Error> onFail)
    {
        if (_type == MaybeType.Fail)
            onFail(Error());

        return this;
    }

    /// <summary>
    /// Invokes the <paramref name="onFail"/> function if the current result is a <c>Fail</c> result.
    /// </summary>
    /// <param name="onFail">A callback function to invoke if the source is a <c>Fail</c> result.</param>
    /// <returns>The current result.</returns>
    public async Task<Maybe<T>> OnFailAsync(Func<Error, Task> onFail)
    {
        if (_type == MaybeType.Fail)
            await onFail(Error()).ConfigureAwait(false);

        return this;
    }
}
