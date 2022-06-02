using static RandomSkunk.Results.Exceptions;
using static RandomSkunk.Results.MaybeType;

namespace RandomSkunk.Results;

/// <content>
/// Defines the <c>Map</c> and <c>MapAsync</c> methods.
/// </content>
public partial struct Maybe<T>
{
    /// <summary>
    /// Maps the current result to a new result using the specified <paramref name="onSome"/>
    /// function. The map function is evaluated if and only if the source is a <c>Some</c> result,
    /// and the <see cref="Maybe{T}.Type"/> of the new result will always be the same as the source
    /// result.
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
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="onSome"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// If <paramref name="onSome"/> returns <see langword="null"/> when evaluated.
    /// </exception>
    public Maybe<TReturn> Map<TReturn>(
        Func<T, TReturn> onSome,
        Func<Error, Error>? onFail = null)
    {
        if (onSome is null) throw new ArgumentNullException(nameof(onSome));

        return _type switch
        {
            Some => (onSome(_value!) ?? throw FunctionMustNotReturnNull(nameof(onSome))).ToMaybe(),
            None => Maybe<TReturn>.Create.None(),
            _ => Maybe<TReturn>.Create.Fail(onFail.Evaluate(Error())),
        };
    }

    /// <summary>
    /// Maps the current result to a new result using the specified <paramref name="onSomeAsync"/>
    /// function. The map function is evaluated if and only if the source is a <c>Some</c> result,
    /// and the <see cref="Maybe{T}.Type"/> of the new result will always be the same as the source
    /// result.
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
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="onSomeAsync"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// If <paramref name="onSomeAsync"/> returns <see langword="null"/> when evaluated.
    /// </exception>
    public async Task<Maybe<TReturn>> MapAsync<TReturn>(
        Func<T, Task<TReturn>> onSomeAsync,
        Func<Error, Error>? onFail = null)
    {
        if (onSomeAsync is null) throw new ArgumentNullException(nameof(onSomeAsync));

        return _type switch
        {
            Some => (await onSomeAsync(_value!).ConfigureAwait(false) ?? throw FunctionMustNotReturnNull(nameof(onSomeAsync))).ToMaybe(),
            None => Maybe<TReturn>.Create.None(),
            _ => Maybe<TReturn>.Create.Fail(onFail.Evaluate(Error())),
        };
    }
}
