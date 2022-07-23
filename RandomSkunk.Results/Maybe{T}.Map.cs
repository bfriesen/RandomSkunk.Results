using static RandomSkunk.Results.Exceptions;

namespace RandomSkunk.Results;

/// <content> Defines the <c>Map</c> and <c>MapAsync</c> methods. </content>
public partial struct Maybe<T>
{
    /// <summary>
    /// Maps the current result to a new result using the specified <paramref name="onSuccessSelector"/> function. The map
    /// function is evaluated if and only if this is a <c>Success</c> result, and the <see cref="Maybe{T}.Type"/> of the new
    /// result will always be the same as this result.
    /// </summary>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="onSuccessSelector">A function that maps the value of the incoming result to the value of the outgoing
    ///     result. Evaluated only if this is a <c>Success</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">If <paramref name="onSuccessSelector"/> returns <see langword="null"/> when
    ///     evaluated.</exception>
    public Maybe<TReturn> Map<TReturn>(Func<T, TReturn> onSuccessSelector)
    {
        if (onSuccessSelector is null) throw new ArgumentNullException(nameof(onSuccessSelector));

        return _type switch
        {
            MaybeType.Success => (onSuccessSelector(_value!) ?? throw FunctionMustNotReturnNull(nameof(onSuccessSelector))).ToMaybe(),
            MaybeType.None => Maybe<TReturn>.None(),
            _ => Maybe<TReturn>.Fail(Error()),
        };
    }

    /// <summary>
    /// Maps the current result to a new result using the specified <paramref name="onSuccessSelector"/> function. The map
    /// function is evaluated if and only if this is a <c>Success</c> result, and the <see cref="Maybe{T}.Type"/> of the new
    /// result will always be the same as this result.
    /// </summary>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="onSuccessSelector">A function that maps the value of the incoming result to the value of the outgoing
    ///     result. Evaluated only if this is a <c>Success</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">If <paramref name="onSuccessSelector"/> returns <see langword="null"/> when
    ///     evaluated.</exception>
    public async Task<Maybe<TReturn>> MapAsync<TReturn>(Func<T, Task<TReturn>> onSuccessSelector)
    {
        if (onSuccessSelector is null) throw new ArgumentNullException(nameof(onSuccessSelector));

        return _type switch
        {
            MaybeType.Success => (await onSuccessSelector(_value!).ConfigureAwait(false) ?? throw FunctionMustNotReturnNull(nameof(onSuccessSelector))).ToMaybe(),
            MaybeType.None => Maybe<TReturn>.None(),
            _ => Maybe<TReturn>.Fail(Error()),
        };
    }
}
