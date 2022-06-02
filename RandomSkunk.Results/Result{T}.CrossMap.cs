namespace RandomSkunk.Results;

/// <content>
/// Defines the <c>CrossMap</c> and <c>CrossMapAsync</c> methods.
/// </content>
public partial struct Result<T>
{
    /// <summary>
    /// Maps the <see cref="Result{T}"/> to a <see cref="Result"/> using the specified
    /// <paramref name="onSuccess"/> function. The cross map function is evaluated if and only if
    /// the source is a <c>Success</c> result. If the source is a <c>Fail</c> result, the error is
    /// propagated to the returned <c>Fail</c> result.
    /// </summary>
    /// <param name="onSuccess">
    /// A function that maps the value of the incoming result to the outgoing result. Evaluated
    /// only if the source is a <c>Success</c> result.
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
    /// Maps the <see cref="Result{T}"/> to a <see cref="Result"/> using the specified
    /// <paramref name="onSuccessAsync"/> function. The cross map function is evaluated if and only if
    /// the source is a <c>Success</c> result. If the source is a <c>Fail</c> result, the error is
    /// propagated to the returned <c>Fail</c> result.
    /// </summary>
    /// <param name="onSuccessAsync">
    /// A function that maps the value of the incoming result to the outgoing result. Evaluated
    /// only if the source is a <c>Success</c> result.
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
    /// Maps the <see cref="Result{T}"/> to a <see cref="Maybe{T}"/> using the specified
    /// <paramref name="onSuccess"/> function. The cross map function is evaluated if and only if
    /// the source is a <c>Success</c> result. If the source is a <c>Fail</c> result, the error is
    /// propagated to the returned <c>Fail</c> result.
    /// </summary>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="onSuccess">
    /// A function that maps the value of the incoming result to the outgoing result. Evaluated
    /// only if the source is a <c>Success</c> result.
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
    public Maybe<TReturn> CrossMap<TReturn>(
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
    /// Maps the <see cref="Result{T}"/> to a <see cref="Maybe{T}"/> using the specified
    /// <paramref name="onSuccessAsync"/> function. The cross map function is evaluated if and only if
    /// the source is a <c>Success</c> result. If the source is a <c>Fail</c> result, the error is
    /// propagated to the returned <c>Fail</c> result.
    /// </summary>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="onSuccessAsync">
    /// A function that maps the value of the incoming result to the outgoing result. Evaluated
    /// only if the source is a <c>Success</c> result.
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
    public async Task<Maybe<TReturn>> CrossMapAsync<TReturn>(
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
