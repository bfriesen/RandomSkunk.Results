namespace RandomSkunk.Results;

/// <content> Defines the <c>OnFail</c> and <c>OnFailAsync</c> methods. </content>
public partial struct Result<T>
{
    /// <summary>
    /// Invokes the <paramref name="onFail"/> function if the current result is a <c>Fail</c> result.
    /// </summary>
    /// <param name="onFail">A callback function to invoke if this is a <c>Fail</c> result.</param>
    /// <returns>The current result.</returns>
    public Result<T> OnFail(Action<Error> onFail)
    {
        if (onFail is null) throw new ArgumentNullException(nameof(onFail));

        if (_type == ResultType.Fail)
            onFail(Error());

        return this;
    }

    /// <summary>
    /// Invokes the <paramref name="onFail"/> function if the current result is a <c>Fail</c> result.
    /// </summary>
    /// <param name="onFail">A callback function to invoke if this is a <c>Fail</c> result.</param>
    /// <returns>The current result.</returns>
    public async Task<Result<T>> OnFailAsync(Func<Error, Task> onFail)
    {
        if (onFail is null) throw new ArgumentNullException(nameof(onFail));

        if (_type == ResultType.Fail)
            await onFail(Error()).ConfigureAwait(false);

        return this;
    }
}
