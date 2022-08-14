namespace RandomSkunk.Results;

/// <content> Defines the <c>FlatMap</c> and <c>FlatMapAsync</c> methods. </content>
public partial struct Result
{
    /// <summary>
    /// Transforms the current result - if <c>Success</c> - into a new result using the specified
    /// <paramref name="onSuccessSelector"/> function. Otherwise, if the current result is <c>Fail</c>, it is transformed into a
    /// new <c>Fail</c> result with the same error.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Map</c> and <c>FlatMap</c> is in the return value of their <c>onSuccessSelector</c> function.
    /// The selector for <c>Map</c> returns a regular (non-result) value, which is the value of the returned <c>Success</c>
    /// result. The selector for <c>FlatMap</c> returns a result value, which is itself the returned result (and might not be
    /// <c>Success</c>).
    /// </remarks>
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
    /// Transforms the current result - if <c>Success</c> - into a new result using the specified
    /// <paramref name="onSuccessSelector"/> function. Otherwise, if the current result is <c>Fail</c>, it is transformed into a
    /// new <c>Fail</c> result with the same error.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Map</c> and <c>FlatMap</c> is in the return value of their <c>onSuccessSelector</c> function.
    /// The selector for <c>Map</c> returns a regular (non-result) value, which is the value of the returned <c>Success</c>
    /// result. The selector for <c>FlatMap</c> returns a result value, which is itself the returned result (and might not be
    /// <c>Success</c>).
    /// </remarks>
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
    /// Transforms the current result - if <c>Success</c> - into a new result using the specified
    /// <paramref name="onSuccessSelector"/> function. Otherwise, if the current result is <c>Fail</c>, it is transformed into a
    /// new <c>Fail</c> result with the same error.
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
    /// Transforms the current result - if <c>Success</c> - into a new result using the specified
    /// <paramref name="onSuccessSelector"/> function. Otherwise, if the current result is <c>Fail</c>, it is transformed into a
    /// new <c>Fail</c> result with the same error.
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
    /// Transforms the current result - if <c>Success</c> - into a new result using the specified
    /// <paramref name="onSuccessSelector"/> function. Otherwise, if the <c>Fail</c> result's error has error code
    /// <see cref="ErrorCodes.ResultIsNone"/>, then a <c>None</c> result is returned. For any other error code, a new <c>Fail</c>
    /// result with the same error is returned.
    /// </summary>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="onSuccessSelector">A function that maps the value of the incoming result to the value of the outgoing
    ///     result. Evaluated only if this is a <c>Success</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public Maybe<TReturn> FlatMap<TReturn>(Func<Maybe<TReturn>> onSuccessSelector)
    {
        if (onSuccessSelector is null) throw new ArgumentNullException(nameof(onSuccessSelector));

        if (_type == ResultType.Success)
            return onSuccessSelector();

        var error = Error();
        if (error.ErrorCode == ErrorCodes.ResultIsNone)
            return Maybe<TReturn>.None();

        return Maybe<TReturn>.Fail(error);
    }

    /// <summary>
    /// Transforms the current result - if <c>Success</c> - into a new result using the specified
    /// <paramref name="onSuccessSelector"/> function. Otherwise, if the <c>Fail</c> result's error has error code
    /// <see cref="ErrorCodes.ResultIsNone"/>, then a <c>None</c> result is returned. For any other error code, a new <c>Fail</c>
    /// result with the same error is returned.
    /// </summary>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="onSuccessSelector">A function that maps the value of the incoming result to the value of the outgoing
    ///     result. Evaluated only if this is a <c>Success</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public async Task<Maybe<TReturn>> FlatMapAsync<TReturn>(Func<Task<Maybe<TReturn>>> onSuccessSelector)
    {
        if (onSuccessSelector is null) throw new ArgumentNullException(nameof(onSuccessSelector));

        if (_type == ResultType.Success)
            return await onSuccessSelector().ConfigureAwait(false);

        var error = Error();
        if (error.ErrorCode == ErrorCodes.ResultIsNone)
            return Maybe<TReturn>.None();

        return Maybe<TReturn>.Fail(error);
    }
}

/// <content> Defines the <c>FlatMap</c> and <c>FlatMapAsync</c> methods. </content>
public partial struct Result<T>
{
    /// <summary>
    /// Transforms the current result - if <c>Success</c> - into a new result using the specified
    /// <paramref name="onSuccessSelector"/> function. Otherwise, if the current result is <c>Fail</c>, it is transformed into a
    /// new <c>Fail</c> result with the same error.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Map</c> and <c>FlatMap</c> is in the return value of their <c>onSuccessSelector</c> function.
    /// The selector for <c>Map</c> returns a regular (non-result) value, which is the value of the returned <c>Success</c>
    /// result. The selector for <c>FlatMap</c> returns a result value, which is itself the returned result (and might not be
    /// <c>Success</c>).
    /// </remarks>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="onSuccessSelector">A function that maps the value of the incoming result to the value of the outgoing
    ///     result. Evaluated only if this is a <c>Success</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public Result<TReturn> FlatMap<TReturn>(Func<T, Result<TReturn>> onSuccessSelector)
    {
        if (onSuccessSelector is null) throw new ArgumentNullException(nameof(onSuccessSelector));

        return _type switch
        {
            ResultType.Success => onSuccessSelector(_value!),
            _ => Result<TReturn>.Fail(Error()),
        };
    }

    /// <summary>
    /// Transforms the current result - if <c>Success</c> - into a new result using the specified
    /// <paramref name="onSuccessSelector"/> function. Otherwise, if the current result is <c>Fail</c>, it is transformed into a
    /// new <c>Fail</c> result with the same error.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Map</c> and <c>FlatMap</c> is in the return value of their <c>onSuccessSelector</c> function.
    /// The selector for <c>Map</c> returns a regular (non-result) value, which is the value of the returned <c>Success</c>
    /// result. The selector for <c>FlatMap</c> returns a result value, which is itself the returned result (and might not be
    /// <c>Success</c>).
    /// </remarks>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="onSuccessSelector">A function that maps the value of the incoming result to the value of the outgoing
    ///     result. Evaluated only if this is a <c>Success</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public async Task<Result<TReturn>> FlatMapAsync<TReturn>(Func<T, Task<Result<TReturn>>> onSuccessSelector)
    {
        if (onSuccessSelector is null) throw new ArgumentNullException(nameof(onSuccessSelector));

        return _type switch
        {
            ResultType.Success => await onSuccessSelector(_value!).ConfigureAwait(false),
            _ => Result<TReturn>.Fail(Error()),
        };
    }

    /// <summary>
    /// Transforms the current result - if <c>Success</c> - into a new result using the specified
    /// <paramref name="onSuccessSelector"/> function. Otherwise, if the current result is <c>Fail</c>, it is transformed into a
    /// new <c>Fail</c> result with the same error.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Map</c> and <c>FlatMap</c> is in the return value of their <c>onSuccessSelector</c> function.
    /// The selector for <c>Map</c> returns a regular (non-result) value, which is the value of the returned <c>Success</c>
    /// result. The selector for <c>FlatMap</c> returns a result value, which is itself the returned result (and might not be
    /// <c>Success</c>).
    /// </remarks>
    /// <param name="onSuccessSelector">A function that maps the value of the incoming result to the outgoing result. Evaluated
    ///     only if this is a <c>Success</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public Result FlatMap(Func<T, Result> onSuccessSelector)
    {
        if (onSuccessSelector is null) throw new ArgumentNullException(nameof(onSuccessSelector));

        return _type switch
        {
            ResultType.Success => onSuccessSelector(_value!),
            _ => Result.Fail(Error()),
        };
    }

    /// <summary>
    /// Transforms the current result - if <c>Success</c> - into a new result using the specified
    /// <paramref name="onSuccessSelector"/> function. Otherwise, if the current result is <c>Fail</c>, it is transformed into a
    /// new <c>Fail</c> result with the same error.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Map</c> and <c>FlatMap</c> is in the return value of their <c>onSuccessSelector</c> function.
    /// The selector for <c>Map</c> returns a regular (non-result) value, which is the value of the returned <c>Success</c>
    /// result. The selector for <c>FlatMap</c> returns a result value, which is itself the returned result (and might not be
    /// <c>Success</c>).
    /// </remarks>
    /// <param name="onSuccessSelector">A function that maps the value of the incoming result to the outgoing result. Evaluated
    ///     only if this is a <c>Success</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public async Task<Result> FlatMapAsync(Func<T, Task<Result>> onSuccessSelector)
    {
        if (onSuccessSelector is null) throw new ArgumentNullException(nameof(onSuccessSelector));

        return _type switch
        {
            ResultType.Success => await onSuccessSelector(_value!).ConfigureAwait(false),
            _ => Result.Fail(Error()),
        };
    }

    /// <summary>
    /// Transforms the current result - if <c>Success</c> - into a new result using the specified
    /// <paramref name="onSuccessSelector"/> function. Otherwise, if the <c>Fail</c> result's error has error code
    /// <see cref="ErrorCodes.ResultIsNone"/>, then a <c>None</c> result is returned. For any other error code, a new <c>Fail</c>
    /// result with the same error is returned.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Map</c> and <c>FlatMap</c> is in the return value of their <c>onSuccessSelector</c> function.
    /// The selector for <c>Map</c> returns a regular (non-result) value, which is the value of the returned <c>Success</c>
    /// result. The selector for <c>FlatMap</c> returns a result value, which is itself the returned result (and might not be
    /// <c>Success</c>).
    /// </remarks>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="onSuccessSelector">A function that maps the value of the incoming result to the outgoing result. Evaluated
    ///     only if this is a <c>Success</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public Maybe<TReturn> FlatMap<TReturn>(Func<T, Maybe<TReturn>> onSuccessSelector)
    {
        if (onSuccessSelector is null) throw new ArgumentNullException(nameof(onSuccessSelector));

        if (_type == ResultType.Success)
            return onSuccessSelector(_value!);

        var error = Error();
        if (error.ErrorCode == ErrorCodes.ResultIsNone)
            return Maybe<TReturn>.None();

        return Maybe<TReturn>.Fail(error);
    }

    /// <summary>
    /// Transforms the current result - if <c>Success</c> - into a new result using the specified
    /// <paramref name="onSuccessSelector"/> function. Otherwise, if the <c>Fail</c> result's error has error code
    /// <see cref="ErrorCodes.ResultIsNone"/>, then a <c>None</c> result is returned. For any other error code, a new <c>Fail</c>
    /// result with the same error is returned.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Map</c> and <c>FlatMap</c> is in the return value of their <c>onSuccessSelector</c> function.
    /// The selector for <c>Map</c> returns a regular (non-result) value, which is the value of the returned <c>Success</c>
    /// result. The selector for <c>FlatMap</c> returns a result value, which is itself the returned result (and might not be
    /// <c>Success</c>).
    /// </remarks>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="onSuccessSelector">A function that maps the value of the incoming result to the outgoing result. Evaluated
    ///     only if this is a <c>Success</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public async Task<Maybe<TReturn>> FlatMapAsync<TReturn>(Func<T, Task<Maybe<TReturn>>> onSuccessSelector)
    {
        if (onSuccessSelector is null) throw new ArgumentNullException(nameof(onSuccessSelector));

        if (_type == ResultType.Success)
            return await onSuccessSelector(_value!).ConfigureAwait(false);

        var error = Error();
        if (error.ErrorCode == ErrorCodes.ResultIsNone)
            return Maybe<TReturn>.None();

        return Maybe<TReturn>.Fail(error);
    }
}

/// <content> Defines the <c>FlatMap</c> and <c>FlatMapAsync</c> methods. </content>
public partial struct Maybe<T>
{
    /// <summary>
    /// Transforms the current result - if <c>Success</c> - into a new result using the specified
    /// <paramref name="onSuccessSelector"/> function. If the current result is <c>Fail</c>, it is transformed into a new
    /// <c>Fail</c> result with the same error. Otherwise, if the current result is <c>None</c>, it is transformed into a new
    /// <c>None</c> result.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Map</c> and <c>FlatMap</c> is in the return value of their <c>onSuccessSelector</c> function.
    /// The selector for <c>Map</c> returns a regular (non-result) value, which is the value of the returned <c>Success</c>
    /// result. The selector for <c>FlatMap</c> returns a result value, which is itself the returned result (and might not be
    /// <c>Success</c>).
    /// </remarks>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="onSuccessSelector">A function that maps the value of the incoming result to the value of the outgoing
    ///     result. Evaluated only if this is a <c>Success</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public Maybe<TReturn> FlatMap<TReturn>(Func<T, Maybe<TReturn>> onSuccessSelector)
    {
        if (onSuccessSelector is null) throw new ArgumentNullException(nameof(onSuccessSelector));

        return _type switch
        {
            MaybeType.Success => onSuccessSelector(_value!),
            MaybeType.None => Maybe<TReturn>.None(),
            _ => Maybe<TReturn>.Fail(Error()),
        };
    }

    /// <summary>
    /// Transforms the current result - if <c>Success</c> - into a new result using the specified
    /// <paramref name="onSuccessSelector"/> function. If the current result is <c>Fail</c>, it is transformed into a new
    /// <c>Fail</c> result with the same error. Otherwise, if the current result is <c>None</c>, it is transformed into a new
    /// <c>None</c> result.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Map</c> and <c>FlatMap</c> is in the return value of their <c>onSuccessSelector</c> function.
    /// The selector for <c>Map</c> returns a regular (non-result) value, which is the value of the returned <c>Success</c>
    /// result. The selector for <c>FlatMap</c> returns a result value, which is itself the returned result (and might not be
    /// <c>Success</c>).
    /// </remarks>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="onSuccessSelector">A function that maps the value of the incoming result to the value of the outgoing
    ///     result. Evaluated only if this is a <c>Success</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public async Task<Maybe<TReturn>> FlatMapAsync<TReturn>(Func<T, Task<Maybe<TReturn>>> onSuccessSelector)
    {
        if (onSuccessSelector is null) throw new ArgumentNullException(nameof(onSuccessSelector));

        return _type switch
        {
            MaybeType.Success => await onSuccessSelector(_value!).ConfigureAwait(false),
            MaybeType.None => Maybe<TReturn>.None(),
            _ => Maybe<TReturn>.Fail(Error()),
        };
    }

    /// <summary>
    /// Transforms the current result - if <c>Success</c> - into a new result using the specified
    /// <paramref name="onSuccessSelector"/> function. If the current result is <c>Fail</c>, it is transformed into a new
    /// <c>Fail</c> result with the same error. Otherwise, if the current result is <c>None</c>, it is transformed into a new
    /// <c>Fail</c> result with error code <see cref="ErrorCodes.ResultIsNone"/>.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Map</c> and <c>FlatMap</c> is in the return value of their <c>onSuccessSelector</c> function.
    /// The selector for <c>Map</c> returns a regular (non-result) value, which is the value of the returned <c>Success</c>
    /// result. The selector for <c>FlatMap</c> returns a result value, which is itself the returned result (and might not be
    /// <c>Success</c>).
    /// </remarks>
    /// <param name="onSuccessSelector">A function that maps the value of the incoming result to the outgoing result. Evaluated
    ///     only if this is a <c>Success</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public Result FlatMap(Func<T, Result> onSuccessSelector)
    {
        if (onSuccessSelector is null) throw new ArgumentNullException(nameof(onSuccessSelector));

        return _type switch
        {
            MaybeType.Success => onSuccessSelector(_value!),
            MaybeType.None => Result.Fail(Errors.ResultIsNone()),
            _ => Result.Fail(Error()),
        };
    }

    /// <summary>
    /// Transforms the current result - if <c>Success</c> - into a new result using the specified
    /// <paramref name="onSuccessSelector"/> function. If the current result is <c>Fail</c>, it is transformed into a new
    /// <c>Fail</c> result with the same error. Otherwise, if the current result is <c>None</c>, it is transformed into a new
    /// <c>Fail</c> result with error code <see cref="ErrorCodes.ResultIsNone"/>.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Map</c> and <c>FlatMap</c> is in the return value of their <c>onSuccessSelector</c> function.
    /// The selector for <c>Map</c> returns a regular (non-result) value, which is the value of the returned <c>Success</c>
    /// result. The selector for <c>FlatMap</c> returns a result value, which is itself the returned result (and might not be
    /// <c>Success</c>).
    /// </remarks>
    /// <param name="onSuccessSelector">A function that maps the value of the incoming result to the outgoing result. Evaluated
    ///     only if this is a <c>Success</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public async Task<Result> FlatMapAsync(Func<T, Task<Result>> onSuccessSelector)
    {
        if (onSuccessSelector is null) throw new ArgumentNullException(nameof(onSuccessSelector));

        return _type switch
        {
            MaybeType.Success => await onSuccessSelector(_value!).ConfigureAwait(false),
            MaybeType.None => Result.Fail(Errors.ResultIsNone()),
            _ => Result.Fail(Error()),
        };
    }

    /// <summary>
    /// Transforms the current result - if <c>Success</c> - into a new result using the specified
    /// <paramref name="onSuccessSelector"/> function. If the current result is <c>Fail</c>, it is transformed into a new
    /// <c>Fail</c> result with the same error. Otherwise, if the current result is <c>None</c>, it is transformed into a new
    /// <c>Fail</c> result with error code <see cref="ErrorCodes.ResultIsNone"/>.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Map</c> and <c>FlatMap</c> is in the return value of their <c>onSuccessSelector</c> function.
    /// The selector for <c>Map</c> returns a regular (non-result) value, which is the value of the returned <c>Success</c>
    /// result. The selector for <c>FlatMap</c> returns a result value, which is itself the returned result (and might not be
    /// <c>Success</c>).
    /// </remarks>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="onSuccessSelector">A function that maps the value of the incoming result to the outgoing result. Evaluated
    ///     only if this is a <c>Success</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public Result<TReturn> FlatMap<TReturn>(Func<T, Result<TReturn>> onSuccessSelector)
    {
        if (onSuccessSelector is null) throw new ArgumentNullException(nameof(onSuccessSelector));

        return _type switch
        {
            MaybeType.Success => onSuccessSelector(_value!),
            MaybeType.None => Result<TReturn>.Fail(Errors.ResultIsNone()),
            _ => Result<TReturn>.Fail(Error()),
        };
    }

    /// <summary>
    /// Transforms the current result - if <c>Success</c> - into a new result using the specified
    /// <paramref name="onSuccessSelector"/> function. If the current result is <c>Fail</c>, it is transformed into a new
    /// <c>Fail</c> result with the same error. Otherwise, if the current result is <c>None</c>, it is transformed into a new
    /// <c>Fail</c> result with error code <see cref="ErrorCodes.ResultIsNone"/>.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Map</c> and <c>FlatMap</c> is in the return value of their <c>onSuccessSelector</c> function.
    /// The selector for <c>Map</c> returns a regular (non-result) value, which is the value of the returned <c>Success</c>
    /// result. The selector for <c>FlatMap</c> returns a result value, which is itself the returned result (and might not be
    /// <c>Success</c>).
    /// </remarks>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="onSuccessSelector">A function that maps the value of the incoming result to the outgoing result. Evaluated
    ///     only if this is a <c>Success</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public async Task<Result<TReturn>> FlatMapAsync<TReturn>(Func<T, Task<Result<TReturn>>> onSuccessSelector)
    {
        if (onSuccessSelector is null) throw new ArgumentNullException(nameof(onSuccessSelector));

        return _type switch
        {
            MaybeType.Success => await onSuccessSelector(_value!).ConfigureAwait(false),
            MaybeType.None => Result<TReturn>.Fail(Errors.ResultIsNone()),
            _ => Result<TReturn>.Fail(Error()),
        };
    }
}

/// <content> Defines the <c>FlatMap</c> and <c>FlatMapAsync</c> methods. </content>
public static partial class ResultExtensions
{
    /// <summary>
    /// Transforms the current result - if <c>Success</c> - into a new result using the specified
    /// <paramref name="onSuccessSelector"/> function. Otherwise, if the current result is <c>Fail</c>, it is transformed into a
    /// new <c>Fail</c> result with the same error.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Map</c> and <c>FlatMap</c> is in the return value of their <c>onSuccessSelector</c> function.
    /// The selector for <c>Map</c> returns a regular (non-result) value, which is the value of the returned <c>Success</c>
    /// result. The selector for <c>FlatMap</c> returns a result value, which is itself the returned result (and might not be
    /// <c>Success</c>).
    /// </remarks>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onSuccessSelector">A function that maps the value of the incoming result to the value of the outgoing
    ///     result. Evaluated only if the source is a <c>Success</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public static async Task<Result> FlatMap(
        this Task<Result> sourceResult,
        Func<Result> onSuccessSelector) =>
        (await sourceResult.ConfigureAwait(false)).FlatMap(onSuccessSelector);

    /// <summary>
    /// Transforms the current result - if <c>Success</c> - into a new result using the specified
    /// <paramref name="onSuccessSelector"/> function. Otherwise, if the current result is <c>Fail</c>, it is transformed into a
    /// new <c>Fail</c> result with the same error.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Map</c> and <c>FlatMap</c> is in the return value of their <c>onSuccessSelector</c> function.
    /// The selector for <c>Map</c> returns a regular (non-result) value, which is the value of the returned <c>Success</c>
    /// result. The selector for <c>FlatMap</c> returns a result value, which is itself the returned result (and might not be
    /// <c>Success</c>).
    /// </remarks>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onSuccessSelector">A function that maps the value of the incoming result to the value of the outgoing
    ///     result. Evaluated only if the source is a <c>Success</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public static async Task<Result> FlatMapAsync(
        this Task<Result> sourceResult,
        Func<Task<Result>> onSuccessSelector) =>
        await (await sourceResult.ConfigureAwait(false)).FlatMapAsync(onSuccessSelector);

    /// <summary>
    /// Transforms the current result - if <c>Success</c> - into a new result using the specified
    /// <paramref name="onSuccessSelector"/> function. Otherwise, if the current result is <c>Fail</c>, it is transformed into a
    /// new <c>Fail</c> result with the same error.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Map</c> and <c>FlatMap</c> is in the return value of their <c>onSuccessSelector</c> function.
    /// The selector for <c>Map</c> returns a regular (non-result) value, which is the value of the returned <c>Success</c>
    /// result. The selector for <c>FlatMap</c> returns a result value, which is itself the returned result (and might not be
    /// <c>Success</c>).
    /// </remarks>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onSuccessSelector">A function that maps the value of the incoming result to the value of the outgoing
    ///     result. Evaluated only if the source is a <c>Success</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public static async Task<Result<TReturn>> FlatMap<TReturn>(
        this Task<Result> sourceResult,
        Func<Result<TReturn>> onSuccessSelector) =>
        (await sourceResult.ConfigureAwait(false)).FlatMap(onSuccessSelector);

    /// <summary>
    /// Transforms the current result - if <c>Success</c> - into a new result using the specified
    /// <paramref name="onSuccessSelector"/> function. Otherwise, if the current result is <c>Fail</c>, it is transformed into a
    /// new <c>Fail</c> result with the same error.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Map</c> and <c>FlatMap</c> is in the return value of their <c>onSuccessSelector</c> function.
    /// The selector for <c>Map</c> returns a regular (non-result) value, which is the value of the returned <c>Success</c>
    /// result. The selector for <c>FlatMap</c> returns a result value, which is itself the returned result (and might not be
    /// <c>Success</c>).
    /// </remarks>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onSuccessSelector">A function that maps the value of the incoming result to the value of the outgoing
    ///     result. Evaluated only if the source is a <c>Success</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public static async Task<Result<TReturn>> FlatMapAsync<TReturn>(
        this Task<Result> sourceResult,
        Func<Task<Result<TReturn>>> onSuccessSelector) =>
        await (await sourceResult.ConfigureAwait(false)).FlatMapAsync(onSuccessSelector);

    /// <summary>
    /// Transforms the current result - if <c>Success</c> - into a new result using the specified
    /// <paramref name="onSuccessSelector"/> function. Otherwise, if the current result is <c>Fail</c>, it is transformed into a
    /// new <c>Fail</c> result with the same error. A <c>None</c> result is never returned.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Map</c> and <c>FlatMap</c> is in the return value of their <c>onSuccessSelector</c> function.
    /// The selector for <c>Map</c> returns a regular (non-result) value, which is the value of the returned <c>Success</c>
    /// result. The selector for <c>FlatMap</c> returns a result value, which is itself the returned result (and might not be
    /// <c>Success</c>).
    /// </remarks>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onSuccessSelector">A function that maps the value of the incoming result to the value of the outgoing
    ///     result. Evaluated only if the source is a <c>Success</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public static async Task<Maybe<TReturn>> FlatMap<TReturn>(
        this Task<Result> sourceResult,
        Func<Maybe<TReturn>> onSuccessSelector) =>
        (await sourceResult.ConfigureAwait(false)).FlatMap(onSuccessSelector);

    /// <summary>
    /// Transforms the current result - if <c>Success</c> - into a new result using the specified
    /// <paramref name="onSuccessSelector"/> function. Otherwise, if the current result is <c>Fail</c>, it is transformed into a
    /// new <c>Fail</c> result with the same error. A <c>None</c> result is never returned.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Map</c> and <c>FlatMap</c> is in the return value of their <c>onSuccessSelector</c> function.
    /// The selector for <c>Map</c> returns a regular (non-result) value, which is the value of the returned <c>Success</c>
    /// result. The selector for <c>FlatMap</c> returns a result value, which is itself the returned result (and might not be
    /// <c>Success</c>).
    /// </remarks>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onSuccessSelector">A function that maps the value of the incoming result to the value of the outgoing
    ///     result. Evaluated only if the source is a <c>Success</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public static async Task<Maybe<TReturn>> FlatMapAsync<TReturn>(
        this Task<Result> sourceResult,
        Func<Task<Maybe<TReturn>>> onSuccessSelector) =>
        await (await sourceResult.ConfigureAwait(false)).FlatMapAsync(onSuccessSelector);

    /// <summary>
    /// Transforms the current result - if <c>Success</c> - into a new result using the specified
    /// <paramref name="onSuccessSelector"/> function. Otherwise, if the current result is <c>Fail</c>, it is transformed into a
    /// new <c>Fail</c> result with the same error.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Map</c> and <c>FlatMap</c> is in the return value of their <c>onSuccessSelector</c> function.
    /// The selector for <c>Map</c> returns a regular (non-result) value, which is the value of the returned <c>Success</c>
    /// result. The selector for <c>FlatMap</c> returns a result value, which is itself the returned result (and might not be
    /// <c>Success</c>).
    /// </remarks>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The task returning the source result.</param>
    /// <param name="onSuccessSelector">A function that maps the value of the incoming result to the outgoing result. Evaluated
    ///     only if the source is a <c>Success</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public static async Task<Result> FlatMap<T>(
        this Task<Result<T>> sourceResult,
        Func<T, Result> onSuccessSelector) =>
        (await sourceResult.ConfigureAwait(false)).FlatMap(onSuccessSelector);

    /// <summary>
    /// Transforms the current result - if <c>Success</c> - into a new result using the specified
    /// <paramref name="onSuccessSelector"/> function. Otherwise, if the current result is <c>Fail</c>, it is transformed into a
    /// new <c>Fail</c> result with the same error.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Map</c> and <c>FlatMap</c> is in the return value of their <c>onSuccessSelector</c> function.
    /// The selector for <c>Map</c> returns a regular (non-result) value, which is the value of the returned <c>Success</c>
    /// result. The selector for <c>FlatMap</c> returns a result value, which is itself the returned result (and might not be
    /// <c>Success</c>).
    /// </remarks>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The task returning the source result.</param>
    /// <param name="onSuccessSelector">A function that maps the value of the incoming result to the outgoing result. Evaluated
    ///     only if the source is a <c>Success</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public static async Task<Result> FlatMapAsync<T>(
        this Task<Result<T>> sourceResult,
        Func<T, Task<Result>> onSuccessSelector) =>
        await (await sourceResult.ConfigureAwait(false)).FlatMapAsync(onSuccessSelector).ConfigureAwait(false);

    /// <summary>
    /// Transforms the current result - if <c>Success</c> - into a new result using the specified
    /// <paramref name="onSuccessSelector"/> function. Otherwise, if the current result is <c>Fail</c>, it is transformed into a
    /// new <c>Fail</c> result with the same error.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Map</c> and <c>FlatMap</c> is in the return value of their <c>onSuccessSelector</c> function.
    /// The selector for <c>Map</c> returns a regular (non-result) value, which is the value of the returned <c>Success</c>
    /// result. The selector for <c>FlatMap</c> returns a result value, which is itself the returned result (and might not be
    /// <c>Success</c>).
    /// </remarks>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="sourceResult">The task returning the source result.</param>
    /// <param name="onSuccessSelector">A function that maps the value of the incoming result to the value of the outgoing
    ///     result. Evaluated only if the source is a <c>Success</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public static async Task<Result<TReturn>> FlatMap<T, TReturn>(
        this Task<Result<T>> sourceResult,
        Func<T, Result<TReturn>> onSuccessSelector) =>
        (await sourceResult.ConfigureAwait(false)).FlatMap(onSuccessSelector);

    /// <summary>
    /// Transforms the current result - if <c>Success</c> - into a new result using the specified
    /// <paramref name="onSuccessSelector"/> function. Otherwise, if the current result is <c>Fail</c>, it is transformed into a
    /// new <c>Fail</c> result with the same error.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Map</c> and <c>FlatMap</c> is in the return value of their <c>onSuccessSelector</c> function.
    /// The selector for <c>Map</c> returns a regular (non-result) value, which is the value of the returned <c>Success</c>
    /// result. The selector for <c>FlatMap</c> returns a result value, which is itself the returned result (and might not be
    /// <c>Success</c>).
    /// </remarks>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="sourceResult">The task returning the source result.</param>
    /// <param name="onSuccessSelector">A function that maps the value of the incoming result to the value of the outgoing
    ///     result. Evaluated only if the source is a <c>Success</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public static async Task<Result<TReturn>> FlatMapAsync<T, TReturn>(
        this Task<Result<T>> sourceResult,
        Func<T, Task<Result<TReturn>>> onSuccessSelector) =>
        await (await sourceResult.ConfigureAwait(false)).FlatMapAsync(onSuccessSelector).ConfigureAwait(false);

    /// <summary>
    /// Transforms the current result - if <c>Success</c> - into a new result using the specified
    /// <paramref name="onSuccessSelector"/> function. Otherwise, if the current result is <c>Fail</c>, it is transformed into a
    /// new <c>Fail</c> result with the same error. A <c>None</c> result is never returned.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Map</c> and <c>FlatMap</c> is in the return value of their <c>onSuccessSelector</c> function.
    /// The selector for <c>Map</c> returns a regular (non-result) value, which is the value of the returned <c>Success</c>
    /// result. The selector for <c>FlatMap</c> returns a result value, which is itself the returned result (and might not be
    /// <c>Success</c>).
    /// </remarks>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="sourceResult">The task returning the source result.</param>
    /// <param name="onSuccessSelector">A function that maps the value of the incoming result to the outgoing result. Evaluated
    ///     only if the source is a <c>Success</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public static async Task<Maybe<TReturn>> FlatMap<T, TReturn>(
        this Task<Result<T>> sourceResult,
        Func<T, Maybe<TReturn>> onSuccessSelector) =>
        (await sourceResult.ConfigureAwait(false)).FlatMap(onSuccessSelector);

    /// <summary>
    /// Transforms the current result - if <c>Success</c> - into a new result using the specified
    /// <paramref name="onSuccessSelector"/> function. Otherwise, if the current result is <c>Fail</c>, it is transformed into a
    /// new <c>Fail</c> result with the same error. A <c>None</c> result is never returned.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Map</c> and <c>FlatMap</c> is in the return value of their <c>onSuccessSelector</c> function.
    /// The selector for <c>Map</c> returns a regular (non-result) value, which is the value of the returned <c>Success</c>
    /// result. The selector for <c>FlatMap</c> returns a result value, which is itself the returned result (and might not be
    /// <c>Success</c>).
    /// </remarks>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="sourceResult">The task returning the source result.</param>
    /// <param name="onSuccessSelector">A function that maps the value of the incoming result to the outgoing result. Evaluated
    ///     only if the source is a <c>Success</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public static async Task<Maybe<TReturn>> FlatMapAsync<T, TReturn>(
        this Task<Result<T>> sourceResult,
        Func<T, Task<Maybe<TReturn>>> onSuccessSelector) =>
        await (await sourceResult.ConfigureAwait(false)).FlatMapAsync(onSuccessSelector).ConfigureAwait(false);

    /// <summary>
    /// Transforms the current result - if <c>Success</c> - into a new result using the specified
    /// <paramref name="onSuccessSelector"/> function. If the current result is <c>Fail</c>, it is transformed into a new
    /// <c>Fail</c> result with the same error. Otherwise, if the current result is <c>None</c>, it is transformed into a new
    /// <c>Fail</c> result with an error indicating that there was no value.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Map</c> and <c>FlatMap</c> is in the return value of their <c>onSuccessSelector</c> function.
    /// The selector for <c>Map</c> returns a regular (non-result) value, which is the value of the returned <c>Success</c>
    /// result. The selector for <c>FlatMap</c> returns a result value, which is itself the returned result (and might not be
    /// <c>Success</c>).
    /// </remarks>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The task returning the source result.</param>
    /// <param name="onSuccessSelector">A function that maps the value of the incoming result to the outgoing result. Evaluated
    ///     only if the source is a <c>Success</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public static async Task<Result> FlatMap<T>(
        this Task<Maybe<T>> sourceResult,
        Func<T, Result> onSuccessSelector) =>
        (await sourceResult.ConfigureAwait(false)).FlatMap(onSuccessSelector);

    /// <summary>
    /// Transforms the current result - if <c>Success</c> - into a new result using the specified
    /// <paramref name="onSuccessSelector"/> function. If the current result is <c>Fail</c>, it is transformed into a new
    /// <c>Fail</c> result with the same error. Otherwise, if the current result is <c>None</c>, it is transformed into a new
    /// <c>Fail</c> result with an error indicating that there was no value.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Map</c> and <c>FlatMap</c> is in the return value of their <c>onSuccessSelector</c> function.
    /// The selector for <c>Map</c> returns a regular (non-result) value, which is the value of the returned <c>Success</c>
    /// result. The selector for <c>FlatMap</c> returns a result value, which is itself the returned result (and might not be
    /// <c>Success</c>).
    /// </remarks>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The task returning the source result.</param>
    /// <param name="onSuccessSelector">A function that maps the value of the incoming result to the outgoing result. Evaluated
    ///     only if the source is a <c>Success</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public static async Task<Result> FlatMapAsync<T>(
        this Task<Maybe<T>> sourceResult,
        Func<T, Task<Result>> onSuccessSelector) =>
        await (await sourceResult.ConfigureAwait(false)).FlatMapAsync(onSuccessSelector).ConfigureAwait(false);

    /// <summary>
    /// Transforms the current result - if <c>Success</c> - into a new result using the specified
    /// <paramref name="onSuccessSelector"/> function. If the current result is <c>Fail</c>, it is transformed into a new
    /// <c>Fail</c> result with the same error. Otherwise, if the current result is <c>None</c>, it is transformed into a new
    /// <c>Fail</c> result with an error indicating that there was no value.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Map</c> and <c>FlatMap</c> is in the return value of their <c>onSuccessSelector</c> function.
    /// The selector for <c>Map</c> returns a regular (non-result) value, which is the value of the returned <c>Success</c>
    /// result. The selector for <c>FlatMap</c> returns a result value, which is itself the returned result (and might not be
    /// <c>Success</c>).
    /// </remarks>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="sourceResult">The task returning the source result.</param>
    /// <param name="onSuccessSelector">A function that maps the value of the incoming result to the outgoing result. Evaluated
    ///     only if the source is a <c>Success</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public static async Task<Result<TReturn>> FlatMap<T, TReturn>(
        this Task<Maybe<T>> sourceResult,
        Func<T, Result<TReturn>> onSuccessSelector) =>
        (await sourceResult.ConfigureAwait(false)).FlatMap(onSuccessSelector);

    /// <summary>
    /// Transforms the current result - if <c>Success</c> - into a new result using the specified
    /// <paramref name="onSuccessSelector"/> function. If the current result is <c>Fail</c>, it is transformed into a new
    /// <c>Fail</c> result with the same error. Otherwise, if the current result is <c>None</c>, it is transformed into a new
    /// <c>Fail</c> result with an error indicating that there was no value.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Map</c> and <c>FlatMap</c> is in the return value of their <c>onSuccessSelector</c> function.
    /// The selector for <c>Map</c> returns a regular (non-result) value, which is the value of the returned <c>Success</c>
    /// result. The selector for <c>FlatMap</c> returns a result value, which is itself the returned result (and might not be
    /// <c>Success</c>).
    /// </remarks>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="sourceResult">The task returning the source result.</param>
    /// <param name="onSuccessSelector">A function that maps the value of the incoming result to the outgoing result. Evaluated
    ///     only if the source is a <c>Success</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public static async Task<Result<TReturn>> FlatMapAsync<T, TReturn>(
        this Task<Maybe<T>> sourceResult,
        Func<T, Task<Result<TReturn>>> onSuccessSelector) =>
        await (await sourceResult.ConfigureAwait(false)).FlatMapAsync(onSuccessSelector).ConfigureAwait(false);

    /// <summary>
    /// Transforms the current result - if <c>Success</c> - into a new result using the specified
    /// <paramref name="onSuccessSelector"/> function. If the current result is <c>Fail</c>, it is transformed into a new
    /// <c>Fail</c> result with the same error. Otherwise, if the current result is <c>None</c>, it is transformed into a new
    /// <c>None</c> result.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Map</c> and <c>FlatMap</c> is in the return value of their <c>onSuccessSelector</c> function.
    /// The selector for <c>Map</c> returns a regular (non-result) value, which is the value of the returned <c>Success</c>
    /// result. The selector for <c>FlatMap</c> returns a result value, which is itself the returned result (and might not be
    /// <c>Success</c>).
    /// </remarks>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="sourceResult">The task returning the source result.</param>
    /// <param name="onSuccessSelector">A function that maps the value of the incoming result to the value of the outgoing
    ///     result. Evaluated only if the source is a <c>Success</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public static async Task<Maybe<TReturn>> FlatMap<T, TReturn>(
        this Task<Maybe<T>> sourceResult,
        Func<T, Maybe<TReturn>> onSuccessSelector) =>
        (await sourceResult.ConfigureAwait(false)).FlatMap(onSuccessSelector);

    /// <summary>
    /// Transforms the current result - if <c>Success</c> - into a new result using the specified
    /// <paramref name="onSuccessSelector"/> function. If the current result is <c>Fail</c>, it is transformed into a new
    /// <c>Fail</c> result with the same error. Otherwise, if the current result is <c>None</c>, it is transformed into a new
    /// <c>None</c> result.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Map</c> and <c>FlatMap</c> is in the return value of their <c>onSuccessSelector</c> function.
    /// The selector for <c>Map</c> returns a regular (non-result) value, which is the value of the returned <c>Success</c>
    /// result. The selector for <c>FlatMap</c> returns a result value, which is itself the returned result (and might not be
    /// <c>Success</c>).
    /// </remarks>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="sourceResult">The task returning the source result.</param>
    /// <param name="onSuccessSelector">A function that maps the value of the incoming result to the value of the outgoing
    ///     result. Evaluated only if the source is a <c>Success</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public static async Task<Maybe<TReturn>> FlatMapAsync<T, TReturn>(
        this Task<Maybe<T>> sourceResult,
        Func<T, Task<Maybe<TReturn>>> onSuccessSelector) =>
        await (await sourceResult.ConfigureAwait(false)).FlatMapAsync(onSuccessSelector).ConfigureAwait(false);
}
