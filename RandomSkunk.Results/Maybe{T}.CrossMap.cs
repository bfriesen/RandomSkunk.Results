using static RandomSkunk.Results.MaybeType;

namespace RandomSkunk.Results;

/// <content>
/// Defines the <c>CrossMap</c> and <c>CrossMapAsync</c> methods.
/// </content>
public partial struct Maybe<T>
{
    /// <summary>
    /// Maps the <see cref="Maybe{T}"/> to a <see cref="Result"/> using the specified
    /// <paramref name="crossMap"/> function. The cross map function is evaluated if and only if
    /// the source is a <c>Some</c> result. If the source is a <c>Fail</c> result, the error is
    /// propagated to the returned <c>Fail</c> result. If the source is a <c>None</c> result, the
    /// error returned by <paramref name="getNoneError"/> is used for the returned <c>Fail</c>
    /// result.
    /// </summary>
    /// <param name="crossMap">
    /// A function that maps the value of the incoming result to the outgoing result. Evaluated
    /// only if the source is a <c>Some</c> result.
    /// </param>
    /// <param name="getNoneError">
    /// An optional function that maps a <c>None</c> result to the return result's error. If
    /// <see langword="null"/>, the error returned from <see cref="ResultExtensions.DefaultGetNoneError"/>
    /// is used instead. Evaluated only if the source is a <c>None</c> result.
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
        Func<Error>? getNoneError = null,
        Func<Error, Error>? getError = null)
    {
        if (crossMap is null) throw new ArgumentNullException(nameof(crossMap));

        return _type switch
        {
            Some => crossMap(_value!),
            None => Result.Create.Fail(getNoneError.Evaluate()),
            _ => Result.Create.Fail(getError.Evaluate(Error())),
        };
    }

    /// <summary>
    /// Maps the <see cref="Maybe{T}"/> to a <see cref="Result"/> using the specified
    /// <paramref name="crossMapAsync"/> function. The cross map function is evaluated if and only if
    /// the source is a <c>Some</c> result. If the source is a <c>Fail</c> result, the error is
    /// propagated to the returned <c>Fail</c> result. If the source is a <c>None</c> result, the
    /// error returned by <paramref name="getNoneError"/> is used for the returned <c>Fail</c>
    /// result.
    /// </summary>
    /// <param name="crossMapAsync">
    /// A function that maps the value of the incoming result to the outgoing result. Evaluated
    /// only if the source is a <c>Some</c> result.
    /// </param>
    /// <param name="getNoneError">
    /// An optional function that maps a <c>None</c> result to the return result's error. If
    /// <see langword="null"/>, the error returned from <see cref="ResultExtensions.DefaultGetNoneError"/>
    /// is used instead. Evaluated only if the source is a <c>None</c> result.
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
        Func<Error>? getNoneError = null,
        Func<Error, Error>? getError = null)
    {
        if (crossMapAsync is null) throw new ArgumentNullException(nameof(crossMapAsync));

        return _type switch
        {
            Some => await crossMapAsync(_value!).ConfigureAwait(false),
            None => Result.Create.Fail(getNoneError.Evaluate()),
            _ => Result.Create.Fail(getError.Evaluate(Error())),
        };
    }

    /// <summary>
    /// Maps the <see cref="Maybe{T}"/> to a <see cref="Result{T}"/> using the specified
    /// <paramref name="crossMap"/> function. The cross map function is evaluated if and only if
    /// the source is a <c>Some</c> result. If the source is a <c>Fail</c> result, the error is
    /// propagated to the returned <c>Fail</c> result. If the source is a <c>None</c> result, the
    /// error returned by <paramref name="getNoneError"/> is used for the returned <c>Fail</c>
    /// result.
    /// </summary>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="crossMap">
    /// A function that maps the value of the incoming result to the outgoing result. Evaluated
    /// only if the source is a <c>Some</c> result.
    /// </param>
    /// <param name="getNoneError">
    /// An optional function that maps a <c>None</c> result to the return result's error. If
    /// <see langword="null"/>, the error returned from <see cref="ResultExtensions.DefaultGetNoneError"/> is used
    /// instead. Evaluated only if the source is a <c>None</c> result.
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
    public Result<TReturn> CrossMap<TReturn>(
        Func<T, Result<TReturn>> crossMap,
        Func<Error>? getNoneError = null,
        Func<Error, Error>? getError = null)
    {
        if (crossMap is null) throw new ArgumentNullException(nameof(crossMap));

        return _type switch
        {
            Some => crossMap(_value!),
            None => Result<TReturn>.Create.Fail(getNoneError.Evaluate()),
            _ => Result<TReturn>.Create.Fail(getError.Evaluate(Error())),
        };
    }

    /// <summary>
    /// Maps the <see cref="Maybe{T}"/> to a <see cref="Result{T}"/> using the specified
    /// <paramref name="crossMapAsync"/> function. The cross map function is evaluated if and only if
    /// the source is a <c>Some</c> result. If the source is a <c>Fail</c> result, the error is
    /// propagated to the returned <c>Fail</c> result. If the source is a <c>None</c> result, the
    /// error returned by <paramref name="getNoneError"/> is used for the returned <c>Fail</c>
    /// result.
    /// </summary>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="crossMapAsync">
    /// A function that maps the value of the incoming result to the outgoing result. Evaluated
    /// only if the source is a <c>Some</c> result.
    /// </param>
    /// <param name="getNoneError">
    /// An optional function that maps a <c>None</c> result to the return result's error. If
    /// <see langword="null"/>, the error returned from <see cref="ResultExtensions.DefaultGetNoneError"/> is used
    /// instead. Evaluated only if the source is a <c>None</c> result.
    /// </param>
    /// <param name="getError">
    /// An optional function that maps a <c>Fail</c> result's error to the returned result's error.
    /// If
    /// <see langword="null"/>, no transformation takes place - a <c>Fail</c> result's error is
    /// used for the returned result. Evaluated only if the source is a <c>Fail</c> result.
    /// </param>
    /// <returns>The cross mapped result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="crossMapAsync"/> is <see langword="null"/>.
    /// </exception>
    public async Task<Result<TReturn>> CrossMapAsync<TReturn>(
        Func<T, Task<Result<TReturn>>> crossMapAsync,
        Func<Error>? getNoneError = null,
        Func<Error, Error>? getError = null)
    {
        if (crossMapAsync is null) throw new ArgumentNullException(nameof(crossMapAsync));

        return _type switch
        {
            Some => await crossMapAsync(_value!).ConfigureAwait(false),
            None => Result<TReturn>.Create.Fail(getNoneError.Evaluate()),
            _ => Result<TReturn>.Create.Fail(getError.Evaluate(Error())),
        };
    }
}
