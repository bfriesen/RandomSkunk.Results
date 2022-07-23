namespace RandomSkunk.Results;

/// <content> Defines the <c>FlatMap</c> and <c>FlatMapAsync</c> methods. </content>
public partial struct Result
{
    /// <summary>
    /// If this is a <c>Success</c> result, then return the result from evaluating the <paramref name="onSuccessSelector"/>
    /// function. Otherwise, if this is a <c>Fail</c> result, return a <c>Fail</c> result with an equivalent error.
    /// </summary>
    /// <param name="onSuccessSelector">A function that maps the value of the incoming result to the value of the outgoing
    ///     result. Evaluated only if this is a <c>Success</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public Result FlatMap(Func<Result> onSuccessSelector)
    {
        if (onSuccessSelector is null) throw new ArgumentNullException(nameof(onSuccessSelector));

        return _type switch
        {
            ResultType.Success => onSuccessSelector(),
            _ => Fail(Error()),
        };
    }

    /// <summary>
    /// If this is a <c>Success</c> result, then return the result from evaluating the <paramref name="onSuccessSelector"/>
    /// function. Otherwise, if this is a <c>Fail</c> result, return a <c>Fail</c> result with an equivalent error.
    /// </summary>
    /// <param name="onSuccessSelector">A function that maps the value of the incoming result to the value of the outgoing
    ///     result. Evaluated only if this is a <c>Success</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public async Task<Result> FlatMapAsync(Func<Task<Result>> onSuccessSelector)
    {
        if (onSuccessSelector is null) throw new ArgumentNullException(nameof(onSuccessSelector));

        return _type switch
        {
            ResultType.Success => await onSuccessSelector().ConfigureAwait(false),
            _ => Fail(Error()),
        };
    }

    /// <summary>
    /// If this is a <c>Success</c> result, then return the result from evaluating the <paramref name="onSuccessSelector"/>
    /// function. Otherwise, if this is a <c>Fail</c> result, return a <c>Fail</c> result with an equivalent error.
    /// </summary>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="onSuccessSelector">A function that maps the value of the incoming result to the value of the outgoing
    ///     result. Evaluated only if this is a <c>Success</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public Result<TReturn> FlatMap<TReturn>(Func<Result<TReturn>> onSuccessSelector)
    {
        if (onSuccessSelector is null) throw new ArgumentNullException(nameof(onSuccessSelector));

        return _type switch
        {
            ResultType.Success => onSuccessSelector(),
            _ => Result<TReturn>.Fail(Error()),
        };
    }

    /// <summary>
    /// If this is a <c>Success</c> result, then return the result from evaluating the <paramref name="onSuccessSelector"/>
    /// function. Otherwise, if this is a <c>Fail</c> result, return a <c>Fail</c> result with an equivalent error.
    /// </summary>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="onSuccessSelector">A function that maps the value of the incoming result to the value of the outgoing
    ///     result. Evaluated only if this is a <c>Success</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public async Task<Result<TReturn>> FlatMapAsync<TReturn>(Func<Task<Result<TReturn>>> onSuccessSelector)
    {
        if (onSuccessSelector is null) throw new ArgumentNullException(nameof(onSuccessSelector));

        return _type switch
        {
            ResultType.Success => await onSuccessSelector().ConfigureAwait(false),
            _ => Result<TReturn>.Fail(Error()),
        };
    }

    /// <summary>
    /// If this is a <c>Success</c> result, then return the result from evaluating the <paramref name="onSuccessSelector"/>
    /// function. Otherwise, if this is a <c>Fail</c> result, return a <c>Fail</c> result with an equivalent error. A <c>None</c>
    /// result is never returned.
    /// </summary>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="onSuccessSelector">A function that maps the value of the incoming result to the value of the outgoing
    ///     result. Evaluated only if this is a <c>Success</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public Maybe<TReturn> FlatMap<TReturn>(Func<Maybe<TReturn>> onSuccessSelector)
    {
        if (onSuccessSelector is null) throw new ArgumentNullException(nameof(onSuccessSelector));

        return _type switch
        {
            ResultType.Success => onSuccessSelector(),
            _ => Maybe<TReturn>.Fail(Error()),
        };
    }

    /// <summary>
    /// If this is a <c>Success</c> result, then return the result from evaluating the <paramref name="onSuccessSelector"/>
    /// function. Otherwise, if this is a <c>Fail</c> result, return a <c>Fail</c> result with an equivalent error. A <c>None</c>
    /// result is never returned.
    /// </summary>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="onSuccessSelector">A function that maps the value of the incoming result to the value of the outgoing
    ///     result. Evaluated only if this is a <c>Success</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public async Task<Maybe<TReturn>> FlatMapAsync<TReturn>(Func<Task<Maybe<TReturn>>> onSuccessSelector)
    {
        if (onSuccessSelector is null) throw new ArgumentNullException(nameof(onSuccessSelector));

        return _type switch
        {
            ResultType.Success => await onSuccessSelector().ConfigureAwait(false),
            _ => Maybe<TReturn>.Fail(Error()),
        };
    }
}
