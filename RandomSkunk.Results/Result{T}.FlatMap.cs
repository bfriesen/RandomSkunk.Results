namespace RandomSkunk.Results;

/// <content>
/// Defines the <c>FlatMap</c> and <c>FlatMapAsync</c> methods.
/// </content>
public partial struct Result<T>
{
    /// <summary>
    /// Maps the current result to a another result using the specified
    /// <paramref name="onSuccess"/> function. The flat map function is evaluated if and only if the
    /// source is a <c>Success</c> result. If the source is a <c>Fail</c> result, the error is
    /// propagated to the returned <c>Fail</c> result.
    /// </summary>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="onSuccess">
    /// A function that maps the value of the incoming result to the value of the outgoing result.
    /// Evaluated only if the source is a <c>Success</c> result.
    /// </param>
    /// <param name="onFail">
    /// An optional function that maps a <c>Fail</c> result's error to the returned result's error.
    /// If <see langword="null"/>, no transformation takes place - a <c>Fail</c> result's error is
    /// used for the returned result. Evaluated only if the source is a <c>Fail</c> result.
    /// </param>
    /// <returns>The flat mapped result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="onSuccess"/> is <see langword="null"/>.
    /// </exception>
    public Result<TReturn> FlatMap<TReturn>(
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
    /// Maps the current result to a another result using the specified
    /// <paramref name="onSuccessAsync"/> function. The flat map function is evaluated if and only if
    /// the source is a <c>Success</c> result. If the source is a <c>Fail</c> result, the error is
    /// propagated to the returned <c>Fail</c> result.
    /// </summary>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="onSuccessAsync">
    /// A function that maps the value of the incoming result to the value of the outgoing result.
    /// Evaluated only if the source is a <c>Success</c> result.
    /// </param>
    /// <param name="onFail">
    /// An optional function that maps a <c>Fail</c> result's error to the returned result's error.
    /// If <see langword="null"/>, no transformation takes place - a <c>Fail</c> result's error is
    /// used for the returned result. Evaluated only if the source is a <c>Fail</c> result.
    /// </param>
    /// <returns>The flat mapped result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="onSuccessAsync"/> is <see langword="null"/>.
    /// </exception>
    public async Task<Result<TReturn>> FlatMapAsync<TReturn>(
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
}
