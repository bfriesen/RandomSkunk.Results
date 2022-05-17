using static RandomSkunk.Results.MaybeType;
using static RandomSkunk.Results.ResultType;

namespace RandomSkunk.Results;

/// <content>
/// Defines the <c>FlatMap</c> and <c>FlatMapAsync</c> extension methods.
/// </content>
public static partial class ResultExtensions
{
    /// <summary>
    /// Maps <paramref name="source"/> to a another result using the specified
    /// <paramref name="flatMap"/> function. The flat map function is evaluated if and only if the
    /// source is a <c>Success</c> result. If the source is a <c>Fail</c> result, the error is
    /// propagated to the returned <c>Fail</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="source">The source result.</param>
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
    public static Result<TReturn> FlatMap<T, TReturn>(
        this Result<T> source,
        Func<T, Result<TReturn>> flatMap,
        Func<Error, Error>? getError = null)
    {
        if (flatMap is null) throw new ArgumentNullException(nameof(flatMap));

        return source._type switch
        {
            Success => flatMap(source._value!),
            _ => Result<TReturn>.Create.Fail(getError.Evaluate(source.Error())),
        };
    }

    /// <summary>
    /// Maps <paramref name="source"/> to a another result using the specified
    /// <paramref name="flatMapAsync"/> function. The flat map function is evaluated if and only if
    /// the source is a <c>Success</c> result. If the source is a <c>Fail</c> result, the error is
    /// propagated to the returned <c>Fail</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="source">The source result.</param>
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
    public static async Task<Result<TReturn>> FlatMapAsync<T, TReturn>(
        this Result<T> source,
        Func<T, Task<Result<TReturn>>> flatMapAsync,
        Func<Error, Error>? getError = null)
    {
        if (flatMapAsync is null) throw new ArgumentNullException(nameof(flatMapAsync));

        return source._type switch
        {
            Success => await flatMapAsync(source._value!),
            _ => Result<TReturn>.Create.Fail(getError.Evaluate(source.Error())),
        };
    }

    /// <summary>
    /// Maps <paramref name="source"/> to a another result using the specified
    /// <paramref name="flatMap"/> function. The flat map function is evaluated if and only if the
    /// source is a <c>Some</c> result. If the source is a <c>Fail</c> result, the error is
    /// propagated to the returned <c>Fail</c> result. If the source is a <c>None</c> result, a
    /// <c>None</c> result is returned.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="flatMap">
    /// A function that maps the value of the incoming result to the value of the outgoing result.
    /// Evaluated only if the source is a <c>Some</c> result.
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
    public static Maybe<TReturn> FlatMap<T, TReturn>(
        this Maybe<T> source,
        Func<T, Maybe<TReturn>> flatMap,
        Func<Error, Error>? getError = null)
    {
        if (flatMap is null) throw new ArgumentNullException(nameof(flatMap));

        return source._type switch
        {
            Some => flatMap(source._value!),
            None => Maybe<TReturn>.Create.None(),
            _ => Maybe<TReturn>.Create.Fail(getError.Evaluate(source.Error())),
        };
    }

    /// <summary>
    /// Maps <paramref name="source"/> to a another result using the specified
    /// <paramref name="flatMapAsync"/> function. The flat map function is evaluated if and only if
    /// the source is a <c>Some</c> result. If the source is a <c>Fail</c> result, the error is
    /// propagated to the returned <c>Fail</c> result. If the source is a <c>None</c> result, a
    /// <c>None</c> result is returned.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="flatMapAsync">
    /// A function that maps the value of the incoming result to the value of the outgoing result.
    /// Evaluated only if the source is a <c>Some</c> result.
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
    public static async Task<Maybe<TReturn>> FlatMapAsync<T, TReturn>(
        this Maybe<T> source,
        Func<T, Task<Maybe<TReturn>>> flatMapAsync,
        Func<Error, Error>? getError = null)
    {
        if (flatMapAsync is null) throw new ArgumentNullException(nameof(flatMapAsync));

        return source._type switch
        {
            Some => await flatMapAsync(source._value!),
            None => Maybe<TReturn>.Create.None(),
            _ => Maybe<TReturn>.Create.Fail(getError.Evaluate(source.Error())),
        };
    }
}
