namespace RandomSkunk.Results;

/// <content> Defines the <c>AndAlso</c> and <c>AndAlsoAsync</c> methods. </content>
public partial struct Result
{
    /// <summary>
    /// Combines this result with another result if this is a <c>Success</c> result; otherwise, return this <c>Fail</c> result.
    /// </summary>
    /// <param name="onSuccess">A function returning another <see cref="Result"/> that is only evaluated if this is a
    ///     <c>Success</c> result.</param>
    /// <param name="onFail">An optional function for creating a custom error that is only evaluated if this is a <c>Fail</c>
    ///     result.</param>
    /// <returns>The combined result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccess"/> is <see langword="null"/>.</exception>
    public Result AndAlso(Func<Result> onSuccess, Func<Error, Error>? onFail = null)
    {
        if (onSuccess is null) throw new ArgumentNullException(nameof(onSuccess));

        return _type switch
        {
            ResultType.Success => onSuccess(),
            _ => onFail is null ? this : Fail(onFail(Error())),
        };
    }

    /// <summary>
    /// Combines this result with another result if this is a <c>Success</c> result; otherwise, return this <c>Fail</c> result.
    /// </summary>
    /// <param name="onSuccessAsync">A function returning another <see cref="Result"/> that is only evaluated if this is a
    ///     <c>Success</c> result.</param>
    /// <param name="onFail">An optional function for creating a custom error that is only evaluated if this is a <c>Fail</c>
    ///     result.</param>
    /// <returns>The combined result.</returns>
    public async Task<Result> AndAlsoAsync(Func<Task<Result>> onSuccessAsync, Func<Error, Error>? onFail = null)
    {
        if (onSuccessAsync is null) throw new ArgumentNullException(nameof(onSuccessAsync));

        return _type switch
        {
            ResultType.Success => await onSuccessAsync().ConfigureAwait(false),
            _ => onFail is null ? this : Fail(onFail(Error())),
        };
    }
}
