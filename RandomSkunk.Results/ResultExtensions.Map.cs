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
    /// function. The map function is only evaluated if the source is a <c>Success</c> result, and
    /// the <see cref="Result{T}.Type"/> of the new result will always be the same as the source
    /// result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="map">
    /// A function that maps the value of the incoming result to the value of the outgoing result.
    /// </param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="map"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// If <paramref name="map"/> returns <see langword="null"/> when evaluated.
    /// </exception>
    public static Result<TReturn> Map<T, TReturn>(this Result<T> source, Func<T, TReturn> map)
    {
        if (map is null) throw new ArgumentNullException(nameof(map));

        return source.Type switch
        {
            Success => Result<TReturn>.Create.Success(map(source.Value())
                ?? throw Exceptions.FunctionMustNotReturnNull(nameof(map))),
            _ => Result<TReturn>.Create.Fail(source.Error()),
        };
    }

    /// <summary>
    /// Maps <paramref name="source"/> to a new result using the specified <paramref name="mapAsync"/>
    /// function. The map function is only evaluated if the source is a <c>Success</c> result, and
    /// the <see cref="Result{T}.Type"/> of the new result will always be the same as the source
    /// result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="mapAsync">
    /// A function that maps the value of the incoming result to the value of the outgoing result.
    /// </param>
    /// <param name="cancellationToken">
    /// The token to monitor for cancellation requests. The default value is
    /// <see cref="CancellationToken.None"/>.
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
        Func<T, CancellationToken, Task<TReturn>> mapAsync,
        CancellationToken cancellationToken = default)
    {
        if (mapAsync is null) throw new ArgumentNullException(nameof(mapAsync));

        return source.Type switch
        {
            Success => Result<TReturn>.Create.Success((await mapAsync(source.Value(), cancellationToken))
                ?? throw Exceptions.FunctionMustNotReturnNull(nameof(mapAsync))),
            _ => Result<TReturn>.Create.Fail(source.Error()),
        };
    }

    /// <summary>
    /// Maps <paramref name="source"/> to a new result using the specified <paramref name="mapAsync"/>
    /// function. The map function is only evaluated if the source is a <c>Success</c> result, and
    /// the <see cref="Result{T}.Type"/> the new result will always be the same as the source
    /// result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="mapAsync">
    /// A function that maps the value of the incoming result to the value of the outgoing result.
    /// </param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If if <paramref name="mapAsync"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// If <paramref name="mapAsync"/> returns <see langword="null"/> when evaluated.
    /// </exception>
    public static Task<Result<TReturn>> MapAsync<T, TReturn>(
        this Result<T> source,
        Func<T, Task<TReturn>> mapAsync)
    {
        if (mapAsync is null) throw new ArgumentNullException(nameof(mapAsync));

        return source.MapAsync((value, _) => mapAsync(value), default);
    }

    /// <summary>
    /// Maps <paramref name="source"/> to a new result using the specified <paramref name="map"/>
    /// function. The map function is only evaluated if the source is a <c>Some</c> result, and
    /// the <see cref="Maybe{T}.Type"/> of the new result will always be the same as the source
    /// result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="map">
    /// A function that maps the value of the incoming result to the value of the outgoing result.
    /// </param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="map"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// If <paramref name="map"/> returns <see langword="null"/> when evaluated.
    /// </exception>
    public static Maybe<TReturn> Map<T, TReturn>(this Maybe<T> source, Func<T, TReturn> map)
    {
        if (map is null) throw new ArgumentNullException(nameof(map));

        return source.Type switch
        {
            Some => Maybe<TReturn>.Create.Some(map(source.Value())
                ?? throw Exceptions.FunctionMustNotReturnNull(nameof(map))),
            None => Maybe<TReturn>.Create.None(),
            _ => Maybe<TReturn>.Create.Fail(source.Error()),
        };
    }

    /// <summary>
    /// Maps <paramref name="source"/> to a new result using the specified <paramref name="mapAsync"/>
    /// function. The map function is only evaluated if the source is a <c>Some</c> result, and
    /// the <see cref="Maybe{T}.Type"/> of the new result will always be the same as the source
    /// result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="mapAsync">
    /// A function that maps the value of the incoming result to the value of the outgoing result.
    /// </param>
    /// <param name="cancellationToken">
    /// The token to monitor for cancellation requests. The default value is
    /// <see cref="CancellationToken.None"/>.
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
        Func<T, CancellationToken, Task<TReturn>> mapAsync,
        CancellationToken cancellationToken = default)
    {
        if (mapAsync is null) throw new ArgumentNullException(nameof(mapAsync));

        return source.Type switch
        {
            Some => Maybe<TReturn>.Create.Some((await mapAsync(source.Value(), cancellationToken))
                ?? throw Exceptions.FunctionMustNotReturnNull(nameof(mapAsync))),
            None => Maybe<TReturn>.Create.None(),
            _ => Maybe<TReturn>.Create.Fail(source.Error()),
        };
    }

    /// <summary>
    /// Maps <paramref name="source"/> to a new result using the specified <paramref name="mapAsync"/>
    /// function. The map function is only evaluated if the source is a <c>Some</c> result, and
    /// the <see cref="Maybe{T}.Type"/> of the new result will always be the same as the source
    /// result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="mapAsync">
    /// A function that maps the value of the incoming result to the value of the outgoing result.
    /// </param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="mapAsync"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// If <paramref name="mapAsync"/> returns <see langword="null"/> when evaluated.
    /// </exception>
    public static Task<Maybe<TReturn>> MapAsync<T, TReturn>(
        this Maybe<T> source,
        Func<T, Task<TReturn>> mapAsync)
    {
        if (mapAsync is null) throw new ArgumentNullException(nameof(mapAsync));

        return source.MapAsync((value, _) => mapAsync(value), default);
    }
}
