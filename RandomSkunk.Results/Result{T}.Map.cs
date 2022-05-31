using static RandomSkunk.Results.Exceptions;
using static RandomSkunk.Results.ResultType;

namespace RandomSkunk.Results;

/// <content>
/// Defines the <c>Map</c> and <c>MapAsync</c> methods.
/// </content>
public partial struct Result<T>
{
    /// <summary>
    /// Maps the current result to a new result using the specified <paramref name="map"/>
    /// function. The map function is evaluated if and only if the source is a <c>Success</c>
    /// result, and the <see cref="Result{T}.Type"/> of the new result will always be the same as
    /// the source result.
    /// </summary>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="map">
    /// A function that maps the value of the incoming result to the value of the outgoing result.
    /// Evaluated only if the source is a <c>Success</c> result.
    /// </param>
    /// <param name="getError">
    /// An optional function that maps a <c>Fail</c> result's error to the returned result's error.
    /// If <see langword="null"/>, no transformation takes place - a <c>Fail</c> result's error is
    /// used for the returned result. Evaluated only if the source is a <c>Fail</c> result.
    /// </param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="map"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// If <paramref name="map"/> returns <see langword="null"/> when evaluated.
    /// </exception>
    public Result<TReturn> Map<TReturn>(
        Func<T, TReturn> map,
        Func<Error, Error>? getError = null)
    {
        if (map is null) throw new ArgumentNullException(nameof(map));

        return _type switch
        {
            Success => (map(_value!) ?? throw FunctionMustNotReturnNull(nameof(map))).ToResult(),
            _ => Result<TReturn>.Create.Fail(getError.Evaluate(Error())),
        };
    }

    /// <summary>
    /// Maps the current result to a new result using the specified <paramref name="mapAsync"/>
    /// function. The map function is evaluated if and only if the source is a <c>Success</c>
    /// result, and the <see cref="Result{T}.Type"/> of the new result will always be the same as
    /// the source result.
    /// </summary>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="mapAsync">
    /// A function that maps the value of the incoming result to the value of the outgoing result.
    /// Evaluated only if the source is a <c>Success</c> result.
    /// </param>
    /// <param name="getError">
    /// An optional function that maps a <c>Fail</c> result's error to the returned result's error.
    /// If <see langword="null"/>, no transformation takes place - a <c>Fail</c> result's error is
    /// used for the returned result. Evaluated only if the source is a <c>Fail</c> result.
    /// </param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="mapAsync"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// If <paramref name="mapAsync"/> returns <see langword="null"/> when evaluated.
    /// </exception>
    public async Task<Result<TReturn>> MapAsync<TReturn>(
        Func<T, Task<TReturn>> mapAsync,
        Func<Error, Error>? getError = null)
    {
        if (mapAsync is null) throw new ArgumentNullException(nameof(mapAsync));

        return _type switch
        {
            Success => (await mapAsync(_value!).ConfigureAwait(false) ?? throw FunctionMustNotReturnNull(nameof(mapAsync))).ToResult(),
            _ => Result<TReturn>.Create.Fail(getError.Evaluate(Error())),
        };
    }
}
