using static RandomSkunk.Results.Exceptions;
using static RandomSkunk.Results.ResultType;

namespace RandomSkunk.Results;

/// <content>
/// Defines the <c>Map</c> and <c>MapAsync</c> methods.
/// </content>
public partial struct Result<T>
{
    /// <summary>
    /// Maps the current result to a new result using the specified <paramref name="onSuccess"/>
    /// function. The map function is evaluated if and only if the source is a <c>Success</c>
    /// result, and the <see cref="Result{T}.Type"/> of the new result will always be the same as
    /// the source result.
    /// </summary>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="onSuccess">
    /// A function that maps the value of the incoming result to the value of the outgoing result.
    /// Evaluated only if the source is a <c>Success</c> result.
    /// </param>
    /// <param name="onFail">
    /// An optional function that maps a <c>Fail</c> result's error to the returned result's error.
    /// If <see langword="null"/>, no transformation takes place - a <c>Fail</c> result's error is
    /// used for the returned result. Evaluated only if the source is a <c>Fail</c> result.
    /// </param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="onSuccess"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// If <paramref name="onSuccess"/> returns <see langword="null"/> when evaluated.
    /// </exception>
    public Result<TReturn> Map<TReturn>(
        Func<T, TReturn> onSuccess,
        Func<Error, Error>? onFail = null)
    {
        if (onSuccess is null) throw new ArgumentNullException(nameof(onSuccess));

        return _type switch
        {
            Success => (onSuccess(_value!) ?? throw FunctionMustNotReturnNull(nameof(onSuccess))).ToResult(),
            _ => Result<TReturn>.Create.Fail(onFail.Evaluate(Error())),
        };
    }

    /// <summary>
    /// Maps the current result to a new result using the specified <paramref name="onSuccessAsync"/>
    /// function. The map function is evaluated if and only if the source is a <c>Success</c>
    /// result, and the <see cref="Result{T}.Type"/> of the new result will always be the same as
    /// the source result.
    /// </summary>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="onSuccessAsync">
    /// A function that maps the value of the incoming result to the value of the outgoing result.
    /// Evaluated only if the source is a <c>Success</c> result.
    /// </param>
    /// <param name="onFail">
    /// An optional function that maps a <c>Fail</c> result's error to the returned result's error.
    /// If <see langword="null"/>, no transformation takes place - a <c>Fail</c> result's error is
    /// used for the returned result. Evaluated only if the source is a <c>Fail</c> result.
    /// </param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="onSuccessAsync"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// If <paramref name="onSuccessAsync"/> returns <see langword="null"/> when evaluated.
    /// </exception>
    public async Task<Result<TReturn>> MapAsync<TReturn>(
        Func<T, Task<TReturn>> onSuccessAsync,
        Func<Error, Error>? onFail = null)
    {
        if (onSuccessAsync is null) throw new ArgumentNullException(nameof(onSuccessAsync));

        return _type switch
        {
            Success => (await onSuccessAsync(_value!).ConfigureAwait(false) ?? throw FunctionMustNotReturnNull(nameof(onSuccessAsync))).ToResult(),
            _ => Result<TReturn>.Create.Fail(onFail.Evaluate(Error())),
        };
    }
}
