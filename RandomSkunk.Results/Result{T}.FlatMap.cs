using static RandomSkunk.Results.ResultType;

namespace RandomSkunk.Results;

/// <content>
/// Defines the <c>FlatMap</c> and <c>FlatMapAsync</c> methods.
/// </content>
public partial struct Result<T>
{
    /// <summary>
    /// Maps the current result to a another result using the specified
    /// <paramref name="flatMap"/> function. The flat map function is evaluated if and only if the
    /// source is a <c>Success</c> result. If the source is a <c>Fail</c> result, the error is
    /// propagated to the returned <c>Fail</c> result.
    /// </summary>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="flatMap">
    /// A function that maps the value of the incoming result to the value of the outgoing result.
    /// Evaluated only if the source is a <c>Success</c> result.
    /// </param>
    /// <param name="getError">
    /// An optional function that maps a <c>Fail</c> result's error to the returned result's error.
    /// If <see langword="null"/>, no transformation takes place - a <c>Fail</c> result's error is
    /// used for the returned result. Evaluated only if the source is a <c>Fail</c> result.
    /// </param>
    /// <returns>The flat mapped result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="flatMap"/> is <see langword="null"/>.
    /// </exception>
    public Result<TReturn> FlatMap<TReturn>(
        Func<T, Result<TReturn>> flatMap,
        Func<Error, Error>? getError = null)
    {
        if (flatMap is null) throw new ArgumentNullException(nameof(flatMap));

        return _type switch
        {
            Success => flatMap(_value!),
            _ => Result<TReturn>.Create.Fail(getError.Evaluate(Error())),
        };
    }

    /// <summary>
    /// Maps the current result to a another result using the specified
    /// <paramref name="flatMapAsync"/> function. The flat map function is evaluated if and only if
    /// the source is a <c>Success</c> result. If the source is a <c>Fail</c> result, the error is
    /// propagated to the returned <c>Fail</c> result.
    /// </summary>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="flatMapAsync">
    /// A function that maps the value of the incoming result to the value of the outgoing result.
    /// Evaluated only if the source is a <c>Success</c> result.
    /// </param>
    /// <param name="getError">
    /// An optional function that maps a <c>Fail</c> result's error to the returned result's error.
    /// If <see langword="null"/>, no transformation takes place - a <c>Fail</c> result's error is
    /// used for the returned result. Evaluated only if the source is a <c>Fail</c> result.
    /// </param>
    /// <returns>The flat mapped result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="flatMapAsync"/> is <see langword="null"/>.
    /// </exception>
    public async Task<Result<TReturn>> FlatMapAsync<TReturn>(
        Func<T, Task<Result<TReturn>>> flatMapAsync,
        Func<Error, Error>? getError = null)
    {
        if (flatMapAsync is null) throw new ArgumentNullException(nameof(flatMapAsync));

        return _type switch
        {
            Success => await flatMapAsync(_value!),
            _ => Result<TReturn>.Create.Fail(getError.Evaluate(Error())),
        };
    }
}
