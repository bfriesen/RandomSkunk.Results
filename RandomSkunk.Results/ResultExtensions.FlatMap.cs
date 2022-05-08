using static RandomSkunk.Results.MaybeType;
using static RandomSkunk.Results.ResultType;

namespace RandomSkunk.Results;

/// <summary>
/// Defines extension methods for result objects.
/// </summary>
public static partial class ResultExtensions
{
    /// <summary>
    /// Maps <paramref name="source"/> to a another result using the specified
    /// <paramref name="flatMap"/> function. The flat map function is only evaluated if the source
    /// is a <c>Success</c> result. If the source is a <c>Fail</c> result, the error is propagated
    /// to the returned <c>Fail</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="flatMap">
    /// A function that maps the value of the incoming result to the value of the outgoing result.
    /// </param>
    /// <returns>The flat mapped result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="flatMap"/> is <see langword="null"/>.
    /// </exception>
    public static Result<TReturn> FlatMap<T, TReturn>(this Result<T> source, Func<T, Result<TReturn>> flatMap)
    {
        if (flatMap is null) throw new ArgumentNullException(nameof(flatMap));

        return source.Type switch
        {
            Success => flatMap(source.Value),
            _ => Result<TReturn>.Create.Fail(source.Error),
        };
    }

    /// <summary>
    /// Maps <paramref name="source"/> to a another result using the specified
    /// <paramref name="flatMapAsync"/> function. The flat map function is only evaluated if the
    /// source is a <c>Success</c> result. If the source is a <c>Fail</c> result, the error is
    /// propagated to the returned <c>Fail</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="flatMapAsync">
    /// A function that maps the value of the incoming result to the value of the outgoing result.
    /// </param>
    /// <param name="cancellationToken">
    /// The token to monitor for cancellation requests. The default value is
    /// <see cref="CancellationToken.None"/>.
    /// </param>
    /// <returns>The flat mapped result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="flatMapAsync"/> is <see langword="null"/>.
    /// </exception>
    public static async Task<Result<TReturn>> FlatMapAsync<T, TReturn>(
        this Result<T> source,
        Func<T, CancellationToken, Task<Result<TReturn>>> flatMapAsync,
        CancellationToken cancellationToken = default)
    {
        if (flatMapAsync is null) throw new ArgumentNullException(nameof(flatMapAsync));

        return source.Type switch
        {
            Success => await flatMapAsync(source.Value, cancellationToken),
            _ => Result<TReturn>.Create.Fail(source.Error),
        };
    }

    /// <summary>
    /// Maps <paramref name="source"/> to a another result using the specified
    /// <paramref name="flatMapAsync"/> function. The flat map function is only evaluated if the
    /// source is a <c>Success</c> result. If the source is a <c>Fail</c> result, the error is
    /// propagated to the returned <c>Fail</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="flatMapAsync">
    /// A function that maps the value of the incoming result to the value of the outgoing result.
    /// </param>
    /// <returns>The flat mapped result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="flatMapAsync"/> is <see langword="null"/>.
    /// </exception>
    public static Task<Result<TReturn>> FlatMapAsync<T, TReturn>(
        this Result<T> source,
        Func<T, Task<Result<TReturn>>> flatMapAsync)
    {
        if (flatMapAsync is null) throw new ArgumentNullException(nameof(flatMapAsync));

        return source.FlatMapAsync((value, _) => flatMapAsync(value), default);
    }

    /// <summary>
    /// Maps <paramref name="source"/> to a another result using the specified
    /// <paramref name="flatMap"/> function. The flat map function is only evaluated if the source
    /// is a <c>Some</c> result. If the source is a <c>Fail</c> result, the error is propagated
    /// to the returned <c>Fail</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="flatMap">
    /// A function that maps the value of the incoming result to the value of the outgoing result.
    /// </param>
    /// <returns>The flat mapped result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="flatMap"/> is <see langword="null"/>.
    /// </exception>
    public static Maybe<TReturn> FlatMap<T, TReturn>(this Maybe<T> source, Func<T, Maybe<TReturn>> flatMap)
    {
        if (flatMap is null) throw new ArgumentNullException(nameof(flatMap));

        return source.Type switch
        {
            Some => flatMap(source.Value),
            None => Maybe<TReturn>.Create.None(),
            _ => Maybe<TReturn>.Create.Fail(source.Error),
        };
    }

    /// <summary>
    /// Maps <paramref name="source"/> to a another result using the specified
    /// <paramref name="flatMapAsync"/> function. The flat map function is only evaluated if the
    /// source is a <c>Some</c> result. If the source is a <c>Fail</c> result, the error is
    /// propagated to the returned <c>Fail</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="flatMapAsync">
    /// A function that maps the value of the incoming result to the value of the outgoing result.
    /// </param>
    /// <param name="cancellationToken">
    /// The token to monitor for cancellation requests. The default value is
    /// <see cref="CancellationToken.None"/>.
    /// </param>
    /// <returns>The flat mapped result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="flatMapAsync"/> is <see langword="null"/>.
    /// </exception>
    public static async Task<Maybe<TReturn>> FlatMapAsync<T, TReturn>(
        this Maybe<T> source,
        Func<T, CancellationToken, Task<Maybe<TReturn>>> flatMapAsync,
        CancellationToken cancellationToken)
    {
        if (flatMapAsync is null) throw new ArgumentNullException(nameof(flatMapAsync));

        return source.Type switch
        {
            Some => await flatMapAsync(source.Value, cancellationToken),
            None => Maybe<TReturn>.Create.None(),
            _ => Maybe<TReturn>.Create.Fail(source.Error),
        };
    }

    /// <summary>
    /// Maps <paramref name="source"/> to a another result using the specified
    /// <paramref name="flatMapAsync"/> function. The flat map function is only evaluated if the
    /// source is a <c>Some</c> result. If the source is a <c>Fail</c> result, the error is
    /// propagated to the returned <c>Fail</c> result.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="source">The source result.</param>
    /// <param name="flatMapAsync">
    /// A function that maps the value of the incoming result to the value of the outgoing result.
    /// </param>
    /// <returns>The flat mapped result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="flatMapAsync"/> is <see langword="null"/>.
    /// </exception>
    public static Task<Maybe<TReturn>> FlatMapAsync<T, TReturn>(
        this Maybe<T> source,
        Func<T, Task<Maybe<TReturn>>> flatMapAsync)
    {
        if (flatMapAsync is null) throw new ArgumentNullException(nameof(flatMapAsync));

        return source.FlatMapAsync((value, _) => flatMapAsync(value), default);
    }
}
