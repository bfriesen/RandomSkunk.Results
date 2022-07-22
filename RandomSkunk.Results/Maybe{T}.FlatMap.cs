namespace RandomSkunk.Results;

/// <content> Defines the <c>FlatMap</c> and <c>FlatMapAsync</c> methods. </content>
public partial struct Maybe<T>
{
    /// <summary>
    /// If this is a <c>Success</c> result, then return the result from evaluating the <paramref name="onSuccess"/> function.
    /// Else if this is a <c>Fail</c> result, return a <c>Fail</c> result with an equivalent error. Otherwise, if this is a
    /// <c>None</c> result, return a <c>None</c> result.
    /// </summary>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="onSuccess">A function that maps the value of the incoming result to the value of the outgoing result.
    ///     Evaluated only if this is a <c>Success</c> result.</param>
    /// <param name="onFail">An optional function that maps a <c>Fail</c> result's error to the returned result's error. If
    ///     <see langword="null"/>, no transformation takes place - a <c>Fail</c> result's error is used for the returned result.
    ///     Evaluated only if this is a <c>Fail</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccess"/> is <see langword="null"/>.</exception>
    public Maybe<TReturn> FlatMap<TReturn>(
        Func<T, Maybe<TReturn>> onSuccess,
        Func<Error, Error>? onFail = null)
    {
        if (onSuccess is null) throw new ArgumentNullException(nameof(onSuccess));

        return _type switch
        {
            MaybeType.Success => onSuccess(_value!),
            MaybeType.None => Maybe<TReturn>.None(),
            _ => Maybe<TReturn>.Fail(onFail.Evaluate(Error())),
        };
    }

    /// <summary>
    /// If this is a <c>Success</c> result, then return the result from evaluating the <paramref name="onSuccessAsync"/>
    /// function. Else if this is a <c>Fail</c> result, return a <c>Fail</c> result with an equivalent error. Otherwise, if this
    /// is a <c>None</c> result, return a <c>None</c> result.
    /// </summary>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="onSuccessAsync">A function that maps the value of the incoming result to the value of the outgoing result.
    ///     Evaluated only if this is a <c>Success</c> result.</param>
    /// <param name="onFail">An optional function that maps a <c>Fail</c> result's error to the returned result's error. If
    ///     <see langword="null"/>, no transformation takes place - a <c>Fail</c> result's error is used for the returned result.
    ///     Evaluated only if this is a <c>Fail</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessAsync"/> is <see langword="null"/>.</exception>
    public async Task<Maybe<TReturn>> FlatMapAsync<TReturn>(
        Func<T, Task<Maybe<TReturn>>> onSuccessAsync,
        Func<Error, Error>? onFail = null)
    {
        if (onSuccessAsync is null) throw new ArgumentNullException(nameof(onSuccessAsync));

        return _type switch
        {
            MaybeType.Success => await onSuccessAsync(_value!).ConfigureAwait(false),
            MaybeType.None => Maybe<TReturn>.None(),
            _ => Maybe<TReturn>.Fail(onFail.Evaluate(Error())),
        };
    }

    /// <summary>
    /// If this is a <c>Success</c> result, then return the result from evaluating the <paramref name="onSuccess"/> function.
    /// Else if this is a <c>Fail</c> result, return a <c>Fail</c> result with an equivalent error. Otherwise, if this is a
    /// <c>None</c> result, return a <c>Fail</c> result with an error indicating that there was no value.
    /// </summary>
    /// <param name="onSuccess">A function that maps the value of the incoming result to the outgoing result. Evaluated only if
    ///     this is a <c>Success</c> result.</param>
    /// <param name="onNone">An optional function that maps a <c>None</c> result to the return result's error. If
    ///     <see langword="null"/>, the error returned from <see cref="ResultExtensions.DefaultOnNoneCallback"/> is used instead.
    ///     Evaluated only if this is a <c>None</c> result.</param>
    /// <param name="onFail">An optional function that maps a <c>Fail</c> result's error to the returned result's error. If
    ///     <see langword="null"/>, no transformation takes place - a <c>Fail</c> result's error is used for the returned result.
    ///     Evaluated only if this is a <c>Fail</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccess"/> is <see langword="null"/>.</exception>
    public Result FlatMap(
        Func<T, Result> onSuccess,
        Func<Error>? onNone = null,
        Func<Error, Error>? onFail = null)
    {
        if (onSuccess is null) throw new ArgumentNullException(nameof(onSuccess));

        return _type switch
        {
            MaybeType.Success => onSuccess(_value!),
            MaybeType.None => Result.Fail(onNone.EvaluateOnNone()),
            _ => Result.Fail(onFail.Evaluate(Error())),
        };
    }

    /// <summary>
    /// If this is a <c>Success</c> result, then return the result from evaluating the <paramref name="onSuccessAsync"/>
    /// function. Else if this is a <c>Fail</c> result, return a <c>Fail</c> result with an equivalent error. Otherwise, if this
    /// is a <c>None</c> result, return a <c>Fail</c> result with an error indicating that there was no value.
    /// </summary>
    /// <param name="onSuccessAsync">A function that maps the value of the incoming result to the outgoing result. Evaluated only
    ///     if this is a <c>Success</c> result.</param>
    /// <param name="onNone">An optional function that maps a <c>None</c> result to the return result's error. If
    ///     <see langword="null"/>, the error returned from <see cref="ResultExtensions.DefaultOnNoneCallback"/> is used instead.
    ///     Evaluated only if this is a <c>None</c> result.</param>
    /// <param name="onFail">An optional function that maps a <c>Fail</c> result's error to the returned result's error. If
    ///     <see langword="null"/>, no transformation takes place - a <c>Fail</c> result's error is used for the returned result.
    ///     Evaluated only if this is a <c>Fail</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessAsync"/> is <see langword="null"/>.</exception>
    public async Task<Result> FlatMapAsync(
        Func<T, Task<Result>> onSuccessAsync,
        Func<Error>? onNone = null,
        Func<Error, Error>? onFail = null)
    {
        if (onSuccessAsync is null) throw new ArgumentNullException(nameof(onSuccessAsync));

        return _type switch
        {
            MaybeType.Success => await onSuccessAsync(_value!).ConfigureAwait(false),
            MaybeType.None => Result.Fail(onNone.EvaluateOnNone()),
            _ => Result.Fail(onFail.Evaluate(Error())),
        };
    }

    /// <summary>
    /// If this is a <c>Success</c> result, then return the result from evaluating the <paramref name="onSuccess"/> function.
    /// Else if this is a <c>Fail</c> result, return a <c>Fail</c> result with an equivalent error. Otherwise, if this is a
    /// <c>None</c> result, return a <c>Fail</c> result with an error indicating that there was no value.
    /// </summary>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="onSuccess">A function that maps the value of the incoming result to the outgoing result. Evaluated only if
    ///     this is a <c>Success</c> result.</param>
    /// <param name="onNone">An optional function that maps a <c>None</c> result to the return result's error. If
    ///     <see langword="null"/>, the error returned from <see cref="ResultExtensions.DefaultOnNoneCallback"/> is used instead.
    ///     Evaluated only if this is a <c>None</c> result.</param>
    /// <param name="onFail">An optional function that maps a <c>Fail</c> result's error to the returned result's error. If
    ///     <see langword="null"/>, no transformation takes place - a <c>Fail</c> result's error is used for the returned result.
    ///     Evaluated only if this is a <c>Fail</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccess"/> is <see langword="null"/>.</exception>
    public Result<TReturn> FlatMap<TReturn>(
        Func<T, Result<TReturn>> onSuccess,
        Func<Error>? onNone = null,
        Func<Error, Error>? onFail = null)
    {
        if (onSuccess is null) throw new ArgumentNullException(nameof(onSuccess));

        return _type switch
        {
            MaybeType.Success => onSuccess(_value!),
            MaybeType.None => Result<TReturn>.Fail(onNone.EvaluateOnNone()),
            _ => Result<TReturn>.Fail(onFail.Evaluate(Error())),
        };
    }

    /// <summary>
    /// If this is a <c>Success</c> result, then return the result from evaluating the <paramref name="onSuccessAsync"/>
    /// function. Else if this is a <c>Fail</c> result, return a <c>Fail</c> result with an equivalent error. Otherwise, if this
    /// is a <c>None</c> result, return a <c>Fail</c> result with an error indicating that there was no value.
    /// </summary>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="onSuccessAsync">A function that maps the value of the incoming result to the outgoing result. Evaluated only
    ///     if this is a <c>Success</c> result.</param>
    /// <param name="onNone">An optional function that maps a <c>None</c> result to the return result's error. If
    ///     <see langword="null"/>, the error returned from <see cref="ResultExtensions.DefaultOnNoneCallback"/> is used instead.
    ///     Evaluated only if this is a <c>None</c> result.</param>
    /// <param name="onFail">An optional function that maps a <c>Fail</c> result's error to the returned result's error. If
    ///     <see langword="null"/>, no transformation takes place - a <c>Fail</c> result's error is used for the returned result.
    ///     Evaluated only if this is a <c>Fail</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessAsync"/> is <see langword="null"/>.</exception>
    public async Task<Result<TReturn>> FlatMapAsync<TReturn>(
        Func<T, Task<Result<TReturn>>> onSuccessAsync,
        Func<Error>? onNone = null,
        Func<Error, Error>? onFail = null)
    {
        if (onSuccessAsync is null) throw new ArgumentNullException(nameof(onSuccessAsync));

        return _type switch
        {
            MaybeType.Success => await onSuccessAsync(_value!).ConfigureAwait(false),
            MaybeType.None => Result<TReturn>.Fail(onNone.EvaluateOnNone()),
            _ => Result<TReturn>.Fail(onFail.Evaluate(Error())),
        };
    }
}
