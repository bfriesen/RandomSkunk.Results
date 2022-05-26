using static RandomSkunk.Results.ResultType;

namespace RandomSkunk.Results;

/// <content>
/// Defines the <c>CrossMap</c> and <c>CrossMapAsync</c> methods.
/// </content>
public partial struct Result<T>
{
    /// <summary>
    /// Maps the <see cref="Result{T}"/> to a <see cref="Result"/> using the specified
    /// <paramref name="crossMap"/> function. The cross map function is evaluated if and only if
    /// the source is a <c>Success</c> result. If the source is a <c>Fail</c> result, the error is
    /// propagated to the returned <c>Fail</c> result.
    /// </summary>
    /// <param name="crossMap">
    /// A function that maps the value of the incoming result to the outgoing result. Evaluated
    /// only if the source is a <c>Success</c> result.
    /// </param>
    /// <param name="getError">
    /// An optional function that maps a <c>Fail</c> result's error to the returned result's error.
    /// If <see langword="null"/>, no transformation takes place - a <c>Fail</c> result's error is
    /// used for the returned result. Evaluated only if the source is a <c>Fail</c> result.
    /// </param>
    /// <returns>The cross mapped result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="crossMap"/> is <see langword="null"/>.
    /// </exception>
    public Result CrossMap(
        Func<T, Result> crossMap,
        Func<Error, Error>? getError = null)
    {
        if (crossMap is null) throw new ArgumentNullException(nameof(crossMap));

        return _type switch
        {
            Success => crossMap(_value!),
            _ => Result.Create.Fail(getError.Evaluate(Error())),
        };
    }

    /// <summary>
    /// Maps the <see cref="Result{T}"/> to a <see cref="Result"/> using the specified
    /// <paramref name="crossMapAsync"/> function. The cross map function is evaluated if and only if
    /// the source is a <c>Success</c> result. If the source is a <c>Fail</c> result, the error is
    /// propagated to the returned <c>Fail</c> result.
    /// </summary>
    /// <param name="crossMapAsync">
    /// A function that maps the value of the incoming result to the outgoing result. Evaluated
    /// only if the source is a <c>Success</c> result.
    /// </param>
    /// <param name="getError">
    /// An optional function that maps a <c>Fail</c> result's error to the returned result's error.
    /// If <see langword="null"/>, no transformation takes place - a <c>Fail</c> result's error is
    /// used for the returned result. Evaluated only if the source is a <c>Fail</c> result.
    /// </param>
    /// <returns>The cross mapped result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="crossMapAsync"/> is <see langword="null"/>.
    /// </exception>
    public async Task<Result> CrossMapAsync(
        Func<T, Task<Result>> crossMapAsync,
        Func<Error, Error>? getError = null)
    {
        if (crossMapAsync is null) throw new ArgumentNullException(nameof(crossMapAsync));

        return _type switch
        {
            Success => await crossMapAsync(_value!),
            _ => Result.Create.Fail(getError.Evaluate(Error())),
        };
    }

    /// <summary>
    /// Maps the <see cref="Result{T}"/> to a <see cref="Maybe{T}"/> using the specified
    /// <paramref name="crossMap"/> function. The cross map function is evaluated if and only if
    /// the source is a <c>Success</c> result. If the source is a <c>Fail</c> result, the error is
    /// propagated to the returned <c>Fail</c> result.
    /// </summary>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="crossMap">
    /// A function that maps the value of the incoming result to the outgoing result. Evaluated
    /// only if the source is a <c>Success</c> result.
    /// </param>
    /// <param name="getError">
    /// An optional function that maps a <c>Fail</c> result's error to the returned result's error.
    /// If <see langword="null"/>, no transformation takes place - a <c>Fail</c> result's error is
    /// used for the returned result. Evaluated only if the source is a <c>Fail</c> result.
    /// </param>
    /// <returns>The cross mapped result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="crossMap"/> is <see langword="null"/>.
    /// </exception>
    public Maybe<TReturn> CrossMap<TReturn>(
        Func<T, Maybe<TReturn>> crossMap,
        Func<Error, Error>? getError = null)
    {
        if (crossMap is null) throw new ArgumentNullException(nameof(crossMap));

        return _type switch
        {
            Success => crossMap(_value!),
            _ => Maybe<TReturn>.Create.Fail(getError.Evaluate(Error())),
        };
    }

    /// <summary>
    /// Maps the <see cref="Result{T}"/> to a <see cref="Maybe{T}"/> using the specified
    /// <paramref name="crossMapAsync"/> function. The cross map function is evaluated if and only if
    /// the source is a <c>Success</c> result. If the source is a <c>Fail</c> result, the error is
    /// propagated to the returned <c>Fail</c> result.
    /// </summary>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="crossMapAsync">
    /// A function that maps the value of the incoming result to the outgoing result. Evaluated
    /// only if the source is a <c>Success</c> result.
    /// </param>
    /// <param name="getError">
    /// An optional function that maps a <c>Fail</c> result's error to the returned result's error.
    /// If <see langword="null"/>, no transformation takes place - a <c>Fail</c> result's error is
    /// used for the returned result. Evaluated only if the source is a <c>Fail</c> result.
    /// </param>
    /// <returns>The cross mapped result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="crossMapAsync"/> is <see langword="null"/>.
    /// </exception>
    public async Task<Maybe<TReturn>> CrossMapAsync<TReturn>(
        Func<T, Task<Maybe<TReturn>>> crossMapAsync,
        Func<Error, Error>? getError = null)
    {
        if (crossMapAsync is null) throw new ArgumentNullException(nameof(crossMapAsync));

        return _type switch
        {
            Success => await crossMapAsync(_value!),
            _ => Maybe<TReturn>.Create.Fail(getError.Evaluate(Error())),
        };
    }
}
