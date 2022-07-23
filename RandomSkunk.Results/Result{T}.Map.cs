using static RandomSkunk.Results.Exceptions;

namespace RandomSkunk.Results;

/// <content> Defines the <c>Map</c> and <c>MapAsync</c> methods. </content>
public partial struct Result<T>
{
    /// <summary>
    /// Transforms the current result - if <c>Success</c> - into a new <c>Success</c> result using the specified
    /// <paramref name="onSuccessSelector"/> function. Otherwise, if the current result is <c>Fail</c>, it is transformed into a
    /// new <c>Fail</c> result with the same error.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Map</c> and <c>FlatMap</c> is in the return value of their <c>onSuccessSelector</c>
    /// function. The selector for <c>Map</c> returns a regular (non-result) value, which is the value of the returned
    /// <c>Success</c> result. The selector for <c>FlatMap</c> returns a result value, which is itself the returned result (and
    /// might not be <c>Success</c>).
    /// </remarks>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="onSuccessSelector">A function that maps the value of the incoming result to the value of the outgoing
    ///     result. Evaluated only if this is a <c>Success</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">If <paramref name="onSuccessSelector"/> returns <see langword="null"/> when
    ///     evaluated.</exception>
    public Result<TReturn> Map<TReturn>(Func<T, TReturn> onSuccessSelector)
    {
        if (onSuccessSelector is null) throw new ArgumentNullException(nameof(onSuccessSelector));

        return _type switch
        {
            ResultType.Success => (onSuccessSelector(_value!) ?? throw FunctionMustNotReturnNull(nameof(onSuccessSelector))).ToResult(),
            _ => Result<TReturn>.Fail(Error()),
        };
    }

    /// <summary>
    /// Transforms the current result - if <c>Success</c> - into a new <c>Success</c> result using the specified
    /// <paramref name="onSuccessSelector"/> function. Otherwise, if the current result is <c>Fail</c>, it is transformed into a
    /// new <c>Fail</c> result with the same error.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Map</c> and <c>FlatMap</c> is in the return value of their <c>onSuccessSelector</c>
    /// function. The selector for <c>Map</c> returns a regular (non-result) value, which is the value of the returned
    /// <c>Success</c> result. The selector for <c>FlatMap</c> returns a result value, which is itself the returned result (and
    /// might not be <c>Success</c>).
    /// </remarks>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="onSuccessSelector">A function that maps the value of the incoming result to the value of the outgoing
    ///     result. Evaluated only if this is a <c>Success</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException">If <paramref name="onSuccessSelector"/> returns <see langword="null"/> when
    ///     evaluated.</exception>
    public async Task<Result<TReturn>> MapAsync<TReturn>(Func<T, Task<TReturn>> onSuccessSelector)
    {
        if (onSuccessSelector is null) throw new ArgumentNullException(nameof(onSuccessSelector));

        return _type switch
        {
            ResultType.Success => (await onSuccessSelector(_value!).ConfigureAwait(false) ?? throw FunctionMustNotReturnNull(nameof(onSuccessSelector))).ToResult(),
            _ => Result<TReturn>.Fail(Error()),
        };
    }
}
