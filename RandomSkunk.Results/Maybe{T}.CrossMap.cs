namespace RandomSkunk.Results;

/// <content>
/// Defines the <c>CrossMap</c> and <c>CrossMapAsync</c> methods.
/// </content>
public partial struct Maybe<T>
{
    /// <summary>
    /// Maps the <see cref="Maybe{T}"/> to a <see cref="Result"/> using the specified
    /// <paramref name="onSuccess"/> function. The cross map function is evaluated if and only if
    /// the source is a <c>Success</c> result. If the source is a <c>Fail</c> result, the error is
    /// propagated to the returned <c>Fail</c> result. If the source is a <c>None</c> result, the
    /// error returned by <paramref name="onNone"/> is used for the returned <c>Fail</c>
    /// result.
    /// </summary>
    /// <param name="onSuccess">
    /// A function that maps the value of the incoming result to the outgoing result. Evaluated
    /// only if the source is a <c>Success</c> result.
    /// </param>
    /// <param name="onNone">
    /// An optional function that maps a <c>None</c> result to the return result's error. If
    /// <see langword="null"/>, the error returned from <see cref="ResultExtensions.DefaultOnNoneCallback"/>
    /// is used instead. Evaluated only if the source is a <c>None</c> result.
    /// </param>
    /// <param name="onFail">
    /// An optional function that maps a <c>Fail</c> result's error to the returned result's error.
    /// If <see langword="null"/>, no transformation takes place - a <c>Fail</c> result's error is
    /// used for the returned result. Evaluated only if the source is a <c>Fail</c> result.
    /// </param>
    /// <returns>The cross mapped result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="onSuccess"/> is <see langword="null"/>.
    /// </exception>
    public Result CrossMap(
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
    /// Maps the <see cref="Maybe{T}"/> to a <see cref="Result"/> using the specified
    /// <paramref name="onSuccessAsync"/> function. The cross map function is evaluated if and only if
    /// the source is a <c>Success</c> result. If the source is a <c>Fail</c> result, the error is
    /// propagated to the returned <c>Fail</c> result. If the source is a <c>None</c> result, the
    /// error returned by <paramref name="onNone"/> is used for the returned <c>Fail</c>
    /// result.
    /// </summary>
    /// <param name="onSuccessAsync">
    /// A function that maps the value of the incoming result to the outgoing result. Evaluated
    /// only if the source is a <c>Success</c> result.
    /// </param>
    /// <param name="onNone">
    /// An optional function that maps a <c>None</c> result to the return result's error. If
    /// <see langword="null"/>, the error returned from <see cref="ResultExtensions.DefaultOnNoneCallback"/>
    /// is used instead. Evaluated only if the source is a <c>None</c> result.
    /// </param>
    /// <param name="onFail">
    /// An optional function that maps a <c>Fail</c> result's error to the returned result's error.
    /// If <see langword="null"/>, no transformation takes place - a <c>Fail</c> result's error is
    /// used for the returned result. Evaluated only if the source is a <c>Fail</c> result.
    /// </param>
    /// <returns>The cross mapped result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="onSuccessAsync"/> is <see langword="null"/>.
    /// </exception>
    public async Task<Result> CrossMapAsync(
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
    /// Maps the <see cref="Maybe{T}"/> to a <see cref="Result{T}"/> using the specified
    /// <paramref name="onSuccess"/> function. The cross map function is evaluated if and only if
    /// the source is a <c>Success</c> result. If the source is a <c>Fail</c> result, the error is
    /// propagated to the returned <c>Fail</c> result. If the source is a <c>None</c> result, the
    /// error returned by <paramref name="onNone"/> is used for the returned <c>Fail</c>
    /// result.
    /// </summary>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="onSuccess">
    /// A function that maps the value of the incoming result to the outgoing result. Evaluated
    /// only if the source is a <c>Success</c> result.
    /// </param>
    /// <param name="onNone">
    /// An optional function that maps a <c>None</c> result to the return result's error. If
    /// <see langword="null"/>, the error returned from <see cref="ResultExtensions.DefaultOnNoneCallback"/> is used
    /// instead. Evaluated only if the source is a <c>None</c> result.
    /// </param>
    /// <param name="onFail">
    /// An optional function that maps a <c>Fail</c> result's error to the returned result's error.
    /// If <see langword="null"/>, no transformation takes place - a <c>Fail</c> result's error is
    /// used for the returned result. Evaluated only if the source is a <c>Fail</c> result.
    /// </param>
    /// <returns>The cross mapped result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="onSuccess"/> is <see langword="null"/>.
    /// </exception>
    public Result<TReturn> CrossMap<TReturn>(
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
    /// Maps the <see cref="Maybe{T}"/> to a <see cref="Result{T}"/> using the specified
    /// <paramref name="onSuccessAsync"/> function. The cross map function is evaluated if and only if
    /// the source is a <c>Success</c> result. If the source is a <c>Fail</c> result, the error is
    /// propagated to the returned <c>Fail</c> result. If the source is a <c>None</c> result, the
    /// error returned by <paramref name="onNone"/> is used for the returned <c>Fail</c>
    /// result.
    /// </summary>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="onSuccessAsync">
    /// A function that maps the value of the incoming result to the outgoing result. Evaluated
    /// only if the source is a <c>Success</c> result.
    /// </param>
    /// <param name="onNone">
    /// An optional function that maps a <c>None</c> result to the return result's error. If
    /// <see langword="null"/>, the error returned from <see cref="ResultExtensions.DefaultOnNoneCallback"/> is used
    /// instead. Evaluated only if the source is a <c>None</c> result.
    /// </param>
    /// <param name="onFail">
    /// An optional function that maps a <c>Fail</c> result's error to the returned result's error.
    /// If
    /// <see langword="null"/>, no transformation takes place - a <c>Fail</c> result's error is
    /// used for the returned result. Evaluated only if the source is a <c>Fail</c> result.
    /// </param>
    /// <returns>The cross mapped result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="onSuccessAsync"/> is <see langword="null"/>.
    /// </exception>
    public async Task<Result<TReturn>> CrossMapAsync<TReturn>(
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
