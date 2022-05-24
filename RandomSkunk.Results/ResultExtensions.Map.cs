using static RandomSkunk.Results.Exceptions;
using static RandomSkunk.Results.MaybeType;
using static RandomSkunk.Results.ResultType;

namespace RandomSkunk.Results;

/// <content>
/// Defines the <c>Map</c> and <c>MapAsync</c> extension methods.
/// </content>
public static partial class ResultExtensions
{
    /// <summary>
    /// Maps <paramref name="source"/> to a new result using the specified <paramref name="map"/>
    /// function. The map function is evaluated if and only if the source is a <c>Success</c>
    /// result, and the <see cref="Result{T}.Type"/> of the new result will always be the same as
    /// the source result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="source">The source result.</param>
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
    public static Result<TReturn> Map<T, TReturn>(
        this Result<T> source,
        Func<T, TReturn> map,
        Func<Error, Error>? getError = null)
    {
        if (map is null) throw new ArgumentNullException(nameof(map));

        return source._type switch
        {
            Success => Result<TReturn>.Create.Success(map(source._value!)
                ?? throw FunctionMustNotReturnNull(nameof(map))),
            _ => Result<TReturn>.Create.Fail(getError.Evaluate(source.Error())),
        };
    }

    /// <summary>
    /// Maps <paramref name="source"/> to a new result using the specified <paramref name="mapAsync"/>
    /// function. The map function is evaluated if and only if the source is a <c>Success</c>
    /// result, and the <see cref="Result{T}.Type"/> of the new result will always be the same as
    /// the source result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="source">The source result.</param>
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
    public static async Task<Result<TReturn>> MapAsync<T, TReturn>(
        this Result<T> source,
        Func<T, Task<TReturn>> mapAsync,
        Func<Error, Error>? getError = null)
    {
        if (mapAsync is null) throw new ArgumentNullException(nameof(mapAsync));

        return source._type switch
        {
            Success => Result<TReturn>.Create.Success(await mapAsync(source._value!)
                ?? throw FunctionMustNotReturnNull(nameof(mapAsync))),
            _ => Result<TReturn>.Create.Fail(getError.Evaluate(source.Error())),
        };
    }

    /// <summary>
    /// Maps <paramref name="source"/> to a new result using the specified <paramref name="map"/>
    /// function. The map function is evaluated if and only if the source is a <c>Some</c> result,
    /// and the <see cref="Maybe{T}.Type"/> of the new result will always be the same as the source
    /// result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="map">
    /// A function that maps the value of the incoming result to the value of the outgoing result.
    /// Evaluated only if the source is a <c>Some</c> result.
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
    public static Maybe<TReturn> Map<T, TReturn>(
        this Maybe<T> source,
        Func<T, TReturn> map,
        Func<Error, Error>? getError = null)
    {
        if (map is null) throw new ArgumentNullException(nameof(map));

        return source._type switch
        {
            Some => Maybe<TReturn>.Create.Some(map(source._value!)
                ?? throw FunctionMustNotReturnNull(nameof(map))),
            None => Maybe<TReturn>.Create.None(),
            _ => Maybe<TReturn>.Create.Fail(getError.Evaluate(source.Error())),
        };
    }

    /// <summary>
    /// Maps <paramref name="source"/> to a new result using the specified <paramref name="mapAsync"/>
    /// function. The map function is evaluated if and only if the source is a <c>Some</c> result,
    /// and the <see cref="Maybe{T}.Type"/> of the new result will always be the same as the source
    /// result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="mapAsync">
    /// A function that maps the value of the incoming result to the value of the outgoing result.
    /// Evaluated only if the source is a <c>Some</c> result.
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
    public static async Task<Maybe<TReturn>> MapAsync<T, TReturn>(
        this Maybe<T> source,
        Func<T, Task<TReturn>> mapAsync,
        Func<Error, Error>? getError = null)
    {
        if (mapAsync is null) throw new ArgumentNullException(nameof(mapAsync));

        return source._type switch
        {
            Some => Maybe<TReturn>.Create.Some(await mapAsync(source._value!)
                ?? throw FunctionMustNotReturnNull(nameof(mapAsync))),
            None => Maybe<TReturn>.Create.None(),
            _ => Maybe<TReturn>.Create.Fail(getError.Evaluate(source.Error())),
        };
    }

    /// <summary>
    /// Maps <paramref name="source"/> to a new result using the specified <paramref name="map"/>
    /// function. The map function is evaluated if and only if the source is a <c>Success</c>
    /// result, and the <see cref="Result{T}.Type"/> of the new result will always be the same as
    /// the source result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="source">The task returning the source result.</param>
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
    public static async Task<Result<TReturn>> Map<T, TReturn>(
        this Task<Result<T>> source,
        Func<T, TReturn> map,
        Func<Error, Error>? getError = null) =>
        (await source).Map(map, getError);

    /// <summary>
    /// Maps <paramref name="source"/> to a new result using the specified <paramref name="mapAsync"/>
    /// function. The map function is evaluated if and only if the source is a <c>Success</c>
    /// result, and the <see cref="Result{T}.Type"/> of the new result will always be the same as
    /// the source result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="source">The task returning the source result.</param>
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
    public static async Task<Result<TReturn>> MapAsync<T, TReturn>(
        this Task<Result<T>> source,
        Func<T, Task<TReturn>> mapAsync,
        Func<Error, Error>? getError = null) =>
        await (await source).MapAsync(mapAsync, getError);

    /// <summary>
    /// Maps <paramref name="source"/> to a new result using the specified <paramref name="map"/>
    /// function. The map function is evaluated if and only if the source is a <c>Some</c> result,
    /// and the <see cref="Maybe{T}.Type"/> of the new result will always be the same as the source
    /// result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="source">The task returning the source result.</param>
    /// <param name="map">
    /// A function that maps the value of the incoming result to the value of the outgoing result.
    /// Evaluated only if the source is a <c>Some</c> result.
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
    public static async Task<Maybe<TReturn>> Map<T, TReturn>(
        this Task<Maybe<T>> source,
        Func<T, TReturn> map,
        Func<Error, Error>? getError = null) =>
        (await source).Map(map, getError);

    /// <summary>
    /// Maps <paramref name="source"/> to a new result using the specified <paramref name="mapAsync"/>
    /// function. The map function is evaluated if and only if the source is a <c>Some</c> result,
    /// and the <see cref="Maybe{T}.Type"/> of the new result will always be the same as the source
    /// result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="source">The task returning the source result.</param>
    /// <param name="mapAsync">
    /// A function that maps the value of the incoming result to the value of the outgoing result.
    /// Evaluated only if the source is a <c>Some</c> result.
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
    public static async Task<Maybe<TReturn>> MapAsync<T, TReturn>(
        this Task<Maybe<T>> source,
        Func<T, Task<TReturn>> mapAsync,
        Func<Error, Error>? getError = null) =>
        await (await source).MapAsync(mapAsync, getError);
}
