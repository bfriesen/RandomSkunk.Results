namespace RandomSkunk.Results;

/// <content> Defines the <c>Then</c> and <c>ThenAsync</c> methods. </content>
public partial struct Result<T>
{
    /// <summary>
    /// If this is a <c>Success</c> result, then return the result from evaluating the <paramref name="onSuccess"/> function.
    /// Otherwise, if this is a <c>Fail</c> result, return a <c>Fail</c> result with an equivalent error.
    /// </summary>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="onSuccess">A function that maps the value of the incoming result to the value of the outgoing result.
    ///     Evaluated only if this is a <c>Success</c> result.</param>
    /// <param name="onFail">An optional function that maps a <c>Fail</c> result's error to the returned result's error. If
    ///     <see langword="null"/>, no transformation takes place - a <c>Fail</c> result's error is used for the returned result.
    ///     Evaluated only if this is a <c>Fail</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccess"/> is <see langword="null"/>.</exception>
    public Result<TReturn> Then<TReturn>(
        Func<T, Result<TReturn>> onSuccess,
        Func<Error, Error>? onFail = null)
    {
        if (onSuccess is null) throw new ArgumentNullException(nameof(onSuccess));

        return _type switch
        {
            ResultType.Success => onSuccess(_value!),
            _ => Result<TReturn>.Fail(onFail.Evaluate(Error())),
        };
    }

    /// <summary>
    /// If this is a <c>Success</c> result, then return the result from evaluating the <paramref name="onSuccessAsync"/>
    /// function. Otherwise, if this is a <c>Fail</c> result, return a <c>Fail</c> result with an equivalent error.
    /// </summary>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="onSuccessAsync">A function that maps the value of the incoming result to the value of the outgoing result.
    ///     Evaluated only if this is a <c>Success</c> result.</param>
    /// <param name="onFail">An optional function that maps a <c>Fail</c> result's error to the returned result's error. If
    ///     <see langword="null"/>, no transformation takes place - a <c>Fail</c> result's error is used for the returned result.
    ///     Evaluated only if this is a <c>Fail</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessAsync"/> is <see langword="null"/>.</exception>
    public async Task<Result<TReturn>> ThenAsync<TReturn>(
        Func<T, Task<Result<TReturn>>> onSuccessAsync,
        Func<Error, Error>? onFail = null)
    {
        if (onSuccessAsync is null) throw new ArgumentNullException(nameof(onSuccessAsync));

        return _type switch
        {
            ResultType.Success => await onSuccessAsync(_value!).ConfigureAwait(false),
            _ => Result<TReturn>.Fail(onFail.Evaluate(Error())),
        };
    }

    /// <summary>
    /// If this is a <c>Success</c> result, then return the result from evaluating the <paramref name="onSuccess"/> function.
    /// Otherwise, if this is a <c>Fail</c> result, return a <c>Fail</c> result with an equivalent error.
    /// </summary>
    /// <param name="onSuccess">A function that maps the value of the incoming result to the outgoing result. Evaluated only if
    ///     this is a <c>Success</c> result.</param>
    /// <param name="onFail">An optional function that maps a <c>Fail</c> result's error to the returned result's error. If
    ///     <see langword="null"/>, no transformation takes place - a <c>Fail</c> result's error is used for the returned result.
    ///     Evaluated only if this is a <c>Fail</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccess"/> is <see langword="null"/>.</exception>
    public Result Then(
        Func<T, Result> onSuccess,
        Func<Error, Error>? onFail = null)
    {
        if (onSuccess is null) throw new ArgumentNullException(nameof(onSuccess));

        return _type switch
        {
            ResultType.Success => onSuccess(_value!),
            _ => Result.Fail(onFail.Evaluate(Error())),
        };
    }

    /// <summary>
    /// If this is a <c>Success</c> result, then return the result from evaluating the <paramref name="onSuccessAsync"/>
    /// function. Otherwise, if this is a <c>Fail</c> result, return a <c>Fail</c> result with an equivalent error.
    /// </summary>
    /// <param name="onSuccessAsync">A function that maps the value of the incoming result to the outgoing result. Evaluated only
    ///     if this is a <c>Success</c> result.</param>
    /// <param name="onFail">An optional function that maps a <c>Fail</c> result's error to the returned result's error. If
    ///     <see langword="null"/>, no transformation takes place - a <c>Fail</c> result's error is used for the returned result.
    ///     Evaluated only if this is a <c>Fail</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessAsync"/> is <see langword="null"/>.</exception>
    public async Task<Result> ThenAsync(
        Func<T, Task<Result>> onSuccessAsync,
        Func<Error, Error>? onFail = null)
    {
        if (onSuccessAsync is null) throw new ArgumentNullException(nameof(onSuccessAsync));

        return _type switch
        {
            ResultType.Success => await onSuccessAsync(_value!).ConfigureAwait(false),
            _ => Result.Fail(onFail.Evaluate(Error())),
        };
    }

    /// <summary>
    /// If this is a <c>Success</c> result, then return the result from evaluating the <paramref name="onSuccess"/> function.
    /// Otherwise, if this is a <c>Fail</c> result, return a <c>Fail</c> result with an equivalent error. A <c>None</c> result is
    /// never returned.
    /// </summary>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="onSuccess">A function that maps the value of the incoming result to the outgoing result. Evaluated only if
    ///     this is a <c>Success</c> result.</param>
    /// <param name="onFail">An optional function that maps a <c>Fail</c> result's error to the returned result's error. If
    ///     <see langword="null"/>, no transformation takes place - a <c>Fail</c> result's error is used for the returned result.
    ///     Evaluated only if this is a <c>Fail</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccess"/> is <see langword="null"/>.</exception>
    public Maybe<TReturn> Then<TReturn>(
        Func<T, Maybe<TReturn>> onSuccess,
        Func<Error, Error>? onFail = null)
    {
        if (onSuccess is null) throw new ArgumentNullException(nameof(onSuccess));

        return _type switch
        {
            ResultType.Success => onSuccess(_value!),
            _ => Maybe<TReturn>.Fail(onFail.Evaluate(Error())),
        };
    }

    /// <summary>
    /// If this is a <c>Success</c> result, then return the result from evaluating the <paramref name="onSuccessAsync"/>
    /// function. Otherwise, if this is a <c>Fail</c> result, return a <c>Fail</c> result with an equivalent error. A <c>None</c>
    /// result is never returned.
    /// </summary>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="onSuccessAsync">A function that maps the value of the incoming result to the outgoing result. Evaluated only
    ///     if this is a <c>Success</c> result.</param>
    /// <param name="onFail">An optional function that maps a <c>Fail</c> result's error to the returned result's error. If
    ///     <see langword="null"/>, no transformation takes place - a <c>Fail</c> result's error is used for the returned result.
    ///     Evaluated only if this is a <c>Fail</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessAsync"/> is <see langword="null"/>.</exception>
    public async Task<Maybe<TReturn>> ThenAsync<TReturn>(
        Func<T, Task<Maybe<TReturn>>> onSuccessAsync,
        Func<Error, Error>? onFail = null)
    {
        if (onSuccessAsync is null) throw new ArgumentNullException(nameof(onSuccessAsync));

        return _type switch
        {
            ResultType.Success => await onSuccessAsync(_value!).ConfigureAwait(false),
            _ => Maybe<TReturn>.Fail(onFail.Evaluate(Error())),
        };
    }
}
