using static RandomSkunk.Results.MaybeType;

namespace RandomSkunk.Results;

/// <content>
/// Defines the <c>FlatMap</c> and <c>FlatMapAsync</c> methods.
/// </content>
public partial struct Maybe<T>
{
    /// <summary>
    /// Maps the current result to a another result using the specified
    /// <paramref name="onSome"/> function. The flat map function is evaluated if and only if the
    /// source is a <c>Some</c> result. If the source is a <c>Fail</c> result, the error is
    /// propagated to the returned <c>Fail</c> result. If the source is a <c>None</c> result, a
    /// <c>None</c> result is returned.
    /// </summary>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="onSome">
    /// A function that maps the value of the incoming result to the value of the outgoing result.
    /// Evaluated only if the source is a <c>Some</c> result.
    /// </param>
    /// <param name="onFail">
    /// An optional function that maps a <c>Fail</c> result's error to the returned result's error.
    /// If <see langword="null"/>, no transformation takes place - a <c>Fail</c> result's error is
    /// used for the returned result. Evaluated only if the source is a <c>Fail</c> result.
    /// </param>
    /// <returns>The flat mapped result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="onSome"/> is <see langword="null"/>.
    /// </exception>
    public Maybe<TReturn> FlatMap<TReturn>(
        Func<T, Maybe<TReturn>> onSome,
        Func<Error, Error>? onFail = null)
    {
        if (onSome is null) throw new ArgumentNullException(nameof(onSome));

        return _type switch
        {
            Some => onSome(_value!),
            None => Maybe<TReturn>.Create.None(),
            _ => Maybe<TReturn>.Create.Fail(onFail.Evaluate(Error())),
        };
    }

    /// <summary>
    /// Maps the current result to a another result using the specified
    /// <paramref name="onSomeAsync"/> function. The flat map function is evaluated if and only if
    /// the source is a <c>Some</c> result. If the source is a <c>Fail</c> result, the error is
    /// propagated to the returned <c>Fail</c> result. If the source is a <c>None</c> result, a
    /// <c>None</c> result is returned.
    /// </summary>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="onSomeAsync">
    /// A function that maps the value of the incoming result to the value of the outgoing result.
    /// Evaluated only if the source is a <c>Some</c> result.
    /// </param>
    /// <param name="onFail">
    /// An optional function that maps a <c>Fail</c> result's error to the returned result's error.
    /// If <see langword="null"/>, no transformation takes place - a <c>Fail</c> result's error is
    /// used for the returned result. Evaluated only if the source is a <c>Fail</c> result.
    /// </param>
    /// <returns>The flat mapped result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="onSomeAsync"/> is <see langword="null"/>.
    /// </exception>
    public async Task<Maybe<TReturn>> FlatMapAsync<TReturn>(
        Func<T, Task<Maybe<TReturn>>> onSomeAsync,
        Func<Error, Error>? onFail = null)
    {
        if (onSomeAsync is null) throw new ArgumentNullException(nameof(onSomeAsync));

        return _type switch
        {
            Some => await onSomeAsync(_value!).ConfigureAwait(false),
            None => Maybe<TReturn>.Create.None(),
            _ => Maybe<TReturn>.Create.Fail(onFail.Evaluate(Error())),
        };
    }
}
