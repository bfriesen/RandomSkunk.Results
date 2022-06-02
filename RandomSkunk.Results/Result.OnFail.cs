namespace RandomSkunk.Results;

/// <content>
/// Defines the <c>OnFail</c> and <c>OnFailAsync</c> methods.
/// </content>
public partial struct Result
{
    /// <summary>
    /// Invokes the <paramref name="onFail"/> function if the current result is a <c>Fail</c> result.
    /// </summary>
    /// <param name="onFail">A callback function to invoke if the source is a <c>Fail</c> result.</param>
    /// <returns>The current result.</returns>
    public Result OnFail(Action<Error> onFail)
    {
        if (_type == ResultType.Fail)
            onFail(Error());

        return this;
    }

    /// <summary>
    /// Invokes the <paramref name="onFail"/> function if the current result is a <c>Fail</c> result.
    /// </summary>
    /// <param name="onFail">A callback function to invoke if the source is a <c>Fail</c> result.</param>
    /// <returns>The current result.</returns>
    public async Task<Result> OnFailAsync(Func<Error, Task> onFail)
    {
        if (_type == ResultType.Fail)
            await onFail(Error()).ConfigureAwait(false);

        return this;
    }
}
