namespace RandomSkunk.Results;

/// <content> Defines the <c>SelectMany</c> methods. </content>
public partial struct Result
{
    /// <summary>
    /// Transforms the current result - if <c>Success</c> - into a new result using the specified
    /// <paramref name="onSuccessSelector"/> function. Otherwise, if the current result is <c>Fail</c>, it is transformed into a
    /// new <c>Fail</c> result with the same error.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Select</c> and <c>SelectMany</c> is in the return value of their <c>onSuccessSelector</c>
    /// function. The selector for <c>Select</c> returns a regular (non-result) value, which is the value of the returned
    /// <c>Success</c> result. The selector for <c>SelectMany</c> returns a result value, which is itself the returned result
    /// (and might not be <c>Success</c>).
    /// </remarks>
    /// <param name="onSuccessSelector">A function that maps the value of the incoming result to the value of the outgoing
    ///     result. Evaluated only if this is a <c>Success</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public Result SelectMany(Func<Result> onSuccessSelector)
    {
        if (onSuccessSelector is null) throw new ArgumentNullException(nameof(onSuccessSelector));

        return _outcome switch
        {
            Outcome.Success => onSuccessSelector(),
            _ => Fail(Error()),
        };
    }

    /// <summary>
    /// Transforms the current result - if <c>Success</c> - into a new result using the specified
    /// <paramref name="onSuccessSelector"/> function. Otherwise, if the current result is <c>Fail</c>, it is transformed into a
    /// new <c>Fail</c> result with the same error.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Select</c> and <c>SelectMany</c> is in the return value of their <c>onSuccessSelector</c>
    /// function. The selector for <c>Select</c> returns a regular (non-result) value, which is the value of the returned
    /// <c>Success</c> result. The selector for <c>SelectMany</c> returns a result value, which is itself the returned result
    /// (and might not be <c>Success</c>).
    /// </remarks>
    /// <param name="onSuccessSelector">A function that maps the value of the incoming result to the value of the outgoing
    ///     result. Evaluated only if this is a <c>Success</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public async Task<Result> SelectMany(Func<Task<Result>> onSuccessSelector)
    {
        if (onSuccessSelector is null) throw new ArgumentNullException(nameof(onSuccessSelector));

        return _outcome switch
        {
            Outcome.Success => await onSuccessSelector().ConfigureAwait(false),
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
    public Result<TReturn> SelectMany<TReturn>(Func<Result<TReturn>> onSuccessSelector)
    {
        if (onSuccessSelector is null) throw new ArgumentNullException(nameof(onSuccessSelector));

        return _outcome switch
        {
            Outcome.Success => onSuccessSelector(),
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
    public async Task<Result<TReturn>> SelectMany<TReturn>(Func<Task<Result<TReturn>>> onSuccessSelector)
    {
        if (onSuccessSelector is null) throw new ArgumentNullException(nameof(onSuccessSelector));

        return _outcome switch
        {
            Outcome.Success => await onSuccessSelector().ConfigureAwait(false),
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
    public Maybe<TReturn> SelectMany<TReturn>(Func<Maybe<TReturn>> onSuccessSelector)
    {
        if (onSuccessSelector is null) throw new ArgumentNullException(nameof(onSuccessSelector));

        if (_outcome == Outcome.Success)
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
    public async Task<Maybe<TReturn>> SelectMany<TReturn>(Func<Task<Maybe<TReturn>>> onSuccessSelector)
    {
        if (onSuccessSelector is null) throw new ArgumentNullException(nameof(onSuccessSelector));

        if (_outcome == Outcome.Success)
            return await onSuccessSelector().ConfigureAwait(false);

        var error = Error();
        if (error.ErrorCode == ErrorCodes.ResultIsNone)
            return Maybe<TReturn>.None();

        return Maybe<TReturn>.Fail(error);
    }
}

/// <content> Defines the <c>SelectMany</c> methods. </content>
public partial struct Result<T>
{
    /// <summary>
    /// Transforms the current result - if <c>Success</c> - into a new result using the specified
    /// <paramref name="onSuccessSelector"/> function. Otherwise, if the current result is <c>Fail</c>, it is transformed into a
    /// new <c>Fail</c> result with the same error.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Select</c> and <c>SelectMany</c> is in the return value of their <c>onSuccessSelector</c>
    /// function. The selector for <c>Select</c> returns a regular (non-result) value, which is the value of the returned
    /// <c>Success</c> result. The selector for <c>SelectMany</c> returns a result value, which is itself the returned result
    /// (and might not be <c>Success</c>).
    /// </remarks>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="onSuccessSelector">A function that maps the value of the incoming result to the value of the outgoing
    ///     result. Evaluated only if this is a <c>Success</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public Result<TReturn> SelectMany<TReturn>(Func<T, Result<TReturn>> onSuccessSelector)
    {
        if (onSuccessSelector is null) throw new ArgumentNullException(nameof(onSuccessSelector));

        return _outcome switch
        {
            Outcome.Success => onSuccessSelector(_value!),
            _ => Result<TReturn>.Fail(Error()),
        };
    }

    /// <summary>
    /// Transforms the current result - if <c>Success</c> - into a new result using the specified
    /// <paramref name="onSuccessSelector"/> function. Otherwise, if the current result is <c>Fail</c>, it is transformed into a
    /// new <c>Fail</c> result with the same error.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Select</c> and <c>SelectMany</c> is in the return value of their <c>onSuccessSelector</c>
    /// function. The selector for <c>Select</c> returns a regular (non-result) value, which is the value of the returned
    /// <c>Success</c> result. The selector for <c>SelectMany</c> returns a result value, which is itself the returned result
    /// (and might not be <c>Success</c>).
    /// </remarks>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="onSuccessSelector">A function that maps the value of the incoming result to the value of the outgoing
    ///     result. Evaluated only if this is a <c>Success</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public async Task<Result<TReturn>> SelectMany<TReturn>(Func<T, Task<Result<TReturn>>> onSuccessSelector)
    {
        if (onSuccessSelector is null) throw new ArgumentNullException(nameof(onSuccessSelector));

        return _outcome switch
        {
            Outcome.Success => await onSuccessSelector(_value!).ConfigureAwait(false),
            _ => Result<TReturn>.Fail(Error()),
        };
    }

    /// <summary>
    /// Transforms the current result - if <c>Success</c> - into a new result using the specified
    /// <paramref name="onSuccessSelector"/> function. Otherwise, if the current result is <c>Fail</c>, it is transformed into a
    /// new <c>Fail</c> result with the same error.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Select</c> and <c>SelectMany</c> is in the return value of their <c>onSuccessSelector</c>
    /// function. The selector for <c>Select</c> returns a regular (non-result) value, which is the value of the returned
    /// <c>Success</c> result. The selector for <c>SelectMany</c> returns a result value, which is itself the returned result
    /// (and might not be <c>Success</c>).
    /// </remarks>
    /// <param name="onSuccessSelector">A function that maps the value of the incoming result to the outgoing result. Evaluated
    ///     only if this is a <c>Success</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public Result SelectMany(Func<T, Result> onSuccessSelector)
    {
        if (onSuccessSelector is null) throw new ArgumentNullException(nameof(onSuccessSelector));

        return _outcome switch
        {
            Outcome.Success => onSuccessSelector(_value!),
            _ => Result.Fail(Error()),
        };
    }

    /// <summary>
    /// Transforms the current result - if <c>Success</c> - into a new result using the specified
    /// <paramref name="onSuccessSelector"/> function. Otherwise, if the current result is <c>Fail</c>, it is transformed into a
    /// new <c>Fail</c> result with the same error.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Select</c> and <c>SelectMany</c> is in the return value of their <c>onSuccessSelector</c>
    /// function. The selector for <c>Select</c> returns a regular (non-result) value, which is the value of the returned
    /// <c>Success</c> result. The selector for <c>SelectMany</c> returns a result value, which is itself the returned result
    /// (and might not be <c>Success</c>).
    /// </remarks>
    /// <param name="onSuccessSelector">A function that maps the value of the incoming result to the outgoing result. Evaluated
    ///     only if this is a <c>Success</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public async Task<Result> SelectMany(Func<T, Task<Result>> onSuccessSelector)
    {
        if (onSuccessSelector is null) throw new ArgumentNullException(nameof(onSuccessSelector));

        return _outcome switch
        {
            Outcome.Success => await onSuccessSelector(_value!).ConfigureAwait(false),
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
    /// The difference between <c>Select</c> and <c>SelectMany</c> is in the return value of their <c>onSuccessSelector</c>
    /// function. The selector for <c>Select</c> returns a regular (non-result) value, which is the value of the returned
    /// <c>Success</c> result. The selector for <c>SelectMany</c> returns a result value, which is itself the returned result
    /// (and might not be <c>Success</c>).
    /// </remarks>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="onSuccessSelector">A function that maps the value of the incoming result to the outgoing result. Evaluated
    ///     only if this is a <c>Success</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public Maybe<TReturn> SelectMany<TReturn>(Func<T, Maybe<TReturn>> onSuccessSelector)
    {
        if (onSuccessSelector is null) throw new ArgumentNullException(nameof(onSuccessSelector));

        if (_outcome == Outcome.Success)
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
    /// The difference between <c>Select</c> and <c>SelectMany</c> is in the return value of their <c>onSuccessSelector</c>
    /// function. The selector for <c>Select</c> returns a regular (non-result) value, which is the value of the returned
    /// <c>Success</c> result. The selector for <c>SelectMany</c> returns a result value, which is itself the returned result
    /// (and might not be <c>Success</c>).
    /// </remarks>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="onSuccessSelector">A function that maps the value of the incoming result to the outgoing result. Evaluated
    ///     only if this is a <c>Success</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public async Task<Maybe<TReturn>> SelectMany<TReturn>(Func<T, Task<Maybe<TReturn>>> onSuccessSelector)
    {
        if (onSuccessSelector is null) throw new ArgumentNullException(nameof(onSuccessSelector));

        if (_outcome == Outcome.Success)
            return await onSuccessSelector(_value!).ConfigureAwait(false);

        var error = Error();
        if (error.ErrorCode == ErrorCodes.ResultIsNone)
            return Maybe<TReturn>.None();

        return Maybe<TReturn>.Fail(error);
    }

    /// <summary>
    /// Projects the value of a result to an intermediate result and invokes a result selector function on the values of the
    /// source and intermediate results.
    /// </summary>
    /// <typeparam name="TIntermediate">The type of the intermediate result, as collected by
    ///     <paramref name="intermediateSelector"/>.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="intermediateSelector">A transform function to apply to the value of the input result.</param>
    /// <param name="resultSelector">A transform function to apply to the value of the intermediate result.</param>
    /// <returns>An <see cref="Maybe{T}"/> whose value is the result of invoking the transform function
    ///     <paramref name="intermediateSelector"/> on the value of this result and then mapping the values of that result and
    ///     the source result to the final result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="intermediateSelector"/> is <see langword="null"/>, or if
    ///     <paramref name="resultSelector"/> is <see langword="null"/>.</exception>
    public Result<TReturn> SelectMany<TIntermediate, TReturn>(
        Func<T, Result<TIntermediate>> intermediateSelector,
        Func<T, TIntermediate, TReturn> resultSelector)
    {
        if (intermediateSelector is null) throw new ArgumentNullException(nameof(intermediateSelector));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return SelectMany(
            sourceValue => intermediateSelector(sourceValue).Select(
                intermediateValue => resultSelector(sourceValue, intermediateValue)));
    }

    /// <summary>
    /// Projects the value of a result to an intermediate result and invokes a result selector function on the values of the
    /// source and intermediate results.
    /// </summary>
    /// <typeparam name="TIntermediate">The type of the intermediate result, as collected by
    ///     <paramref name="intermediateSelector"/>.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="intermediateSelector">A transform function to apply to the value of the input result.</param>
    /// <param name="resultSelector">A transform function to apply to the value of the intermediate result.</param>
    /// <returns>An <see cref="Maybe{T}"/> whose value is the result of invoking the transform function
    ///     <paramref name="intermediateSelector"/> on the value of this result and then mapping the values of that result and
    ///     the source result to the final result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="intermediateSelector"/> is <see langword="null"/>, or if
    ///     <paramref name="resultSelector"/> is <see langword="null"/>.</exception>
    public Maybe<TReturn> SelectMany<TIntermediate, TReturn>(
        Func<T, Maybe<TIntermediate>> intermediateSelector,
        Func<T, TIntermediate, TReturn> resultSelector)
    {
        if (intermediateSelector is null) throw new ArgumentNullException(nameof(intermediateSelector));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return SelectMany(
            sourceValue => intermediateSelector(sourceValue).Select(
                intermediateValue => resultSelector(sourceValue, intermediateValue)));
    }

    /// <summary>
    /// Projects the value of a result to an intermediate result and invokes a result selector function on the values of the
    /// source and intermediate results.
    /// </summary>
    /// <typeparam name="TIntermediate">The type of the intermediate result, as collected by
    ///     <paramref name="intermediateSelector"/>.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="intermediateSelector">A transform function to apply to the value of the input result.</param>
    /// <param name="resultSelector">A transform function to apply to the value of the intermediate result.</param>
    /// <returns>An <see cref="Maybe{T}"/> whose value is the result of invoking the transform function
    ///     <paramref name="intermediateSelector"/> on the value of this result and then mapping the values of that result and
    ///     the source result to the final result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="intermediateSelector"/> is <see langword="null"/>, or if
    ///     <paramref name="resultSelector"/> is <see langword="null"/>.</exception>
    public Task<Result<TReturn>> SelectMany<TIntermediate, TReturn>(
        Func<T, Task<Result<TIntermediate>>> intermediateSelector,
        Func<T, TIntermediate, TReturn> resultSelector)
    {
        if (intermediateSelector is null) throw new ArgumentNullException(nameof(intermediateSelector));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return SelectMany(
            sourceValue => intermediateSelector(sourceValue).Select(
                intermediateValue => resultSelector(sourceValue, intermediateValue)));
    }

    /// <summary>
    /// Projects the value of a result to an intermediate result and invokes a result selector function on the values of the
    /// source and intermediate results.
    /// </summary>
    /// <typeparam name="TIntermediate">The type of the intermediate result, as collected by
    ///     <paramref name="intermediateSelector"/>.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="intermediateSelector">A transform function to apply to the value of the input result.</param>
    /// <param name="resultSelector">A transform function to apply to the value of the intermediate result.</param>
    /// <returns>An <see cref="Maybe{T}"/> whose value is the result of invoking the transform function
    ///     <paramref name="intermediateSelector"/> on the value of this result and then mapping the values of that result and
    ///     the source result to the final result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="intermediateSelector"/> is <see langword="null"/>, or if
    ///     <paramref name="resultSelector"/> is <see langword="null"/>.</exception>
    public Task<Maybe<TReturn>> SelectMany<TIntermediate, TReturn>(
        Func<T, Task<Maybe<TIntermediate>>> intermediateSelector,
        Func<T, TIntermediate, TReturn> resultSelector)
    {
        if (intermediateSelector is null) throw new ArgumentNullException(nameof(intermediateSelector));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return SelectMany(
            sourceValue => intermediateSelector(sourceValue).Select(
                intermediateValue => resultSelector(sourceValue, intermediateValue)));
    }
}

/// <content> Defines the <c>SelectMany</c> methods. </content>
public partial struct Maybe<T>
{
    /// <summary>
    /// Transforms the current result - if <c>Success</c> - into a new result using the specified
    /// <paramref name="onSuccessSelector"/> function. If the current result is <c>Fail</c>, it is transformed into a new
    /// <c>Fail</c> result with the same error. Otherwise, if the current result is <c>None</c>, it is transformed into a new
    /// <c>None</c> result.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Select</c> and <c>SelectMany</c> is in the return value of their <c>onSuccessSelector</c>
    /// function. The selector for <c>Select</c> returns a regular (non-result) value, which is the value of the returned
    /// <c>Success</c> result. The selector for <c>SelectMany</c> returns a result value, which is itself the returned result
    /// (and might not be <c>Success</c>).
    /// </remarks>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="onSuccessSelector">A function that maps the value of the incoming result to the value of the outgoing
    ///     result. Evaluated only if this is a <c>Success</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public Maybe<TReturn> SelectMany<TReturn>(Func<T, Maybe<TReturn>> onSuccessSelector)
    {
        if (onSuccessSelector is null) throw new ArgumentNullException(nameof(onSuccessSelector));

        return _outcome switch
        {
            MaybeOutcome.Success => onSuccessSelector(_value!),
            MaybeOutcome.None => Maybe<TReturn>.None(),
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
    /// The difference between <c>Select</c> and <c>SelectMany</c> is in the return value of their <c>onSuccessSelector</c>
    /// function. The selector for <c>Select</c> returns a regular (non-result) value, which is the value of the returned
    /// <c>Success</c> result. The selector for <c>SelectMany</c> returns a result value, which is itself the returned result
    /// (and might not be <c>Success</c>).
    /// </remarks>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="onSuccessSelector">A function that maps the value of the incoming result to the value of the outgoing
    ///     result. Evaluated only if this is a <c>Success</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public async Task<Maybe<TReturn>> SelectMany<TReturn>(Func<T, Task<Maybe<TReturn>>> onSuccessSelector)
    {
        if (onSuccessSelector is null) throw new ArgumentNullException(nameof(onSuccessSelector));

        return _outcome switch
        {
            MaybeOutcome.Success => await onSuccessSelector(_value!).ConfigureAwait(false),
            MaybeOutcome.None => Maybe<TReturn>.None(),
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
    /// The difference between <c>Select</c> and <c>SelectMany</c> is in the return value of their <c>onSuccessSelector</c>
    /// function. The selector for <c>Select</c> returns a regular (non-result) value, which is the value of the returned
    /// <c>Success</c> result. The selector for <c>SelectMany</c> returns a result value, which is itself the returned result
    /// (and might not be <c>Success</c>).
    /// </remarks>
    /// <param name="onSuccessSelector">A function that maps the value of the incoming result to the outgoing result. Evaluated
    ///     only if this is a <c>Success</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public Result SelectMany(Func<T, Result> onSuccessSelector)
    {
        if (onSuccessSelector is null) throw new ArgumentNullException(nameof(onSuccessSelector));

        return _outcome switch
        {
            MaybeOutcome.Success => onSuccessSelector(_value!),
            MaybeOutcome.None => Result.Fail(Errors.ResultIsNone()),
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
    /// The difference between <c>Select</c> and <c>SelectMany</c> is in the return value of their <c>onSuccessSelector</c>
    /// function. The selector for <c>Select</c> returns a regular (non-result) value, which is the value of the returned
    /// <c>Success</c> result. The selector for <c>SelectMany</c> returns a result value, which is itself the returned result
    /// (and might not be <c>Success</c>).
    /// </remarks>
    /// <param name="onSuccessSelector">A function that maps the value of the incoming result to the outgoing result. Evaluated
    ///     only if this is a <c>Success</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public async Task<Result> SelectMany(Func<T, Task<Result>> onSuccessSelector)
    {
        if (onSuccessSelector is null) throw new ArgumentNullException(nameof(onSuccessSelector));

        return _outcome switch
        {
            MaybeOutcome.Success => await onSuccessSelector(_value!).ConfigureAwait(false),
            MaybeOutcome.None => Result.Fail(Errors.ResultIsNone()),
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
    /// The difference between <c>Select</c> and <c>SelectMany</c> is in the return value of their <c>onSuccessSelector</c>
    /// function. The selector for <c>Select</c> returns a regular (non-result) value, which is the value of the returned
    /// <c>Success</c> result. The selector for <c>SelectMany</c> returns a result value, which is itself the returned result
    /// (and might not be <c>Success</c>).
    /// </remarks>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="onSuccessSelector">A function that maps the value of the incoming result to the outgoing result. Evaluated
    ///     only if this is a <c>Success</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public Result<TReturn> SelectMany<TReturn>(Func<T, Result<TReturn>> onSuccessSelector)
    {
        if (onSuccessSelector is null) throw new ArgumentNullException(nameof(onSuccessSelector));

        return _outcome switch
        {
            MaybeOutcome.Success => onSuccessSelector(_value!),
            MaybeOutcome.None => Result<TReturn>.Fail(Errors.ResultIsNone()),
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
    /// The difference between <c>Select</c> and <c>SelectMany</c> is in the return value of their <c>onSuccessSelector</c>
    /// function. The selector for <c>Select</c> returns a regular (non-result) value, which is the value of the returned
    /// <c>Success</c> result. The selector for <c>SelectMany</c> returns a result value, which is itself the returned result
    /// (and might not be <c>Success</c>).
    /// </remarks>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="onSuccessSelector">A function that maps the value of the incoming result to the outgoing result. Evaluated
    ///     only if this is a <c>Success</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public async Task<Result<TReturn>> SelectMany<TReturn>(Func<T, Task<Result<TReturn>>> onSuccessSelector)
    {
        if (onSuccessSelector is null) throw new ArgumentNullException(nameof(onSuccessSelector));

        return _outcome switch
        {
            MaybeOutcome.Success => await onSuccessSelector(_value!).ConfigureAwait(false),
            MaybeOutcome.None => Result<TReturn>.Fail(Errors.ResultIsNone()),
            _ => Result<TReturn>.Fail(Error()),
        };
    }

    /// <summary>
    /// Projects the value of a result to an intermediate result and invokes a result selector function on the values of the
    /// source and intermediate results.
    /// </summary>
    /// <typeparam name="TIntermediate">The type of the intermediate result collected by <paramref name="intermediateSelector"/>.
    ///     </typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="intermediateSelector">A transform function to apply to the value of the input result.</param>
    /// <param name="resultSelector">A transform function to apply to the value of the intermediate result.</param>
    /// <returns>An <see cref="Maybe{T}"/> whose value is the result of invoking the transform function
    ///     <paramref name="intermediateSelector"/> on the value of this result and then mapping the values of that result and
    ///     the source result to the final result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="intermediateSelector"/> is <see langword="null"/>, or if
    ///     <paramref name="resultSelector"/> is <see langword="null"/>.</exception>
    public Result<TReturn> SelectMany<TIntermediate, TReturn>(
        Func<T, Result<TIntermediate>> intermediateSelector,
        Func<T, TIntermediate, TReturn> resultSelector)
    {
        if (intermediateSelector is null) throw new ArgumentNullException(nameof(intermediateSelector));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return SelectMany(
            sourceValue => intermediateSelector(sourceValue).Select(
                intermediateValue => resultSelector(sourceValue, intermediateValue)));
    }

    /// <summary>
    /// Projects the value of a result to an intermediate result and invokes a result selector function on the values of the
    /// source and intermediate results.
    /// </summary>
    /// <typeparam name="TIntermediate">The type of the intermediate result collected by <paramref name="intermediateSelector"/>.
    ///     </typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="intermediateSelector">A transform function to apply to the value of the input result.</param>
    /// <param name="resultSelector">A transform function to apply to the value of the intermediate result.</param>
    /// <returns>An <see cref="Maybe{T}"/> whose value is the result of invoking the transform function
    ///     <paramref name="intermediateSelector"/> on the value of this result and then mapping the values of that result and
    ///     the source result to the final result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="intermediateSelector"/> is <see langword="null"/>, or if
    ///     <paramref name="resultSelector"/> is <see langword="null"/>.</exception>
    public Maybe<TReturn> SelectMany<TIntermediate, TReturn>(
        Func<T, Maybe<TIntermediate>> intermediateSelector,
        Func<T, TIntermediate, TReturn> resultSelector)
    {
        if (intermediateSelector is null) throw new ArgumentNullException(nameof(intermediateSelector));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return SelectMany(
            sourceValue => intermediateSelector(sourceValue).Select(
                intermediateValue => resultSelector(sourceValue, intermediateValue)));
    }

    /// <summary>
    /// Projects the value of a result to an intermediate result and invokes a result selector function on the values of the
    /// source and intermediate results.
    /// </summary>
    /// <typeparam name="TIntermediate">The type of the intermediate result collected by <paramref name="intermediateSelector"/>.
    ///     </typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="intermediateSelector">A transform function to apply to the value of the input result.</param>
    /// <param name="resultSelector">A transform function to apply to the value of the intermediate result.</param>
    /// <returns>An <see cref="Maybe{T}"/> whose value is the result of invoking the transform function
    ///     <paramref name="intermediateSelector"/> on the value of this result and then mapping the values of that result and
    ///     the source result to the final result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="intermediateSelector"/> is <see langword="null"/>, or if
    ///     <paramref name="resultSelector"/> is <see langword="null"/>.</exception>
    public Task<Result<TReturn>> SelectMany<TIntermediate, TReturn>(
        Func<T, Task<Result<TIntermediate>>> intermediateSelector,
        Func<T, TIntermediate, TReturn> resultSelector)
    {
        if (intermediateSelector is null) throw new ArgumentNullException(nameof(intermediateSelector));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return SelectMany(
            sourceValue => intermediateSelector(sourceValue).Select(
                intermediateValue => resultSelector(sourceValue, intermediateValue)));
    }

    /// <summary>
    /// Projects the value of a result to an intermediate result and invokes a result selector function on the values of the
    /// source and intermediate results.
    /// </summary>
    /// <typeparam name="TIntermediate">The type of the intermediate result collected by <paramref name="intermediateSelector"/>.
    ///     </typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="intermediateSelector">A transform function to apply to the value of the input result.</param>
    /// <param name="resultSelector">A transform function to apply to the value of the intermediate result.</param>
    /// <returns>An <see cref="Maybe{T}"/> whose value is the result of invoking the transform function
    ///     <paramref name="intermediateSelector"/> on the value of this result and then mapping the values of that result and
    ///     the source result to the final result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="intermediateSelector"/> is <see langword="null"/>, or if
    ///     <paramref name="resultSelector"/> is <see langword="null"/>.</exception>
    public Task<Maybe<TReturn>> SelectMany<TIntermediate, TReturn>(
        Func<T, Task<Maybe<TIntermediate>>> intermediateSelector,
        Func<T, TIntermediate, TReturn> resultSelector)
    {
        if (intermediateSelector is null) throw new ArgumentNullException(nameof(intermediateSelector));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return SelectMany(
            sourceValue => intermediateSelector(sourceValue).Select(
                intermediateValue => resultSelector(sourceValue, intermediateValue)));
    }
}

/// <content> Defines the <c>SelectMany</c> methods. </content>
public static partial class ResultExtensions
{
    /// <summary>
    /// Transforms the current result - if <c>Success</c> - into a new result using the specified
    /// <paramref name="onSuccessSelector"/> function. Otherwise, if the current result is <c>Fail</c>, it is transformed into a
    /// new <c>Fail</c> result with the same error.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Select</c> and <c>SelectMany</c> is in the return value of their <c>onSuccessSelector</c>
    /// function. The selector for <c>Select</c> returns a regular (non-result) value, which is the value of the returned
    /// <c>Success</c> result. The selector for <c>SelectMany</c> returns a result value, which is itself the returned result
    /// (and might not be <c>Success</c>).
    /// </remarks>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onSuccessSelector">A function that maps the value of the incoming result to the value of the outgoing
    ///     result. Evaluated only if the source is a <c>Success</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public static async Task<Result> SelectMany(
        this Task<Result> sourceResult,
        Func<Result> onSuccessSelector) =>
        (await sourceResult.ConfigureAwait(false)).SelectMany(onSuccessSelector);

    /// <summary>
    /// Transforms the current result - if <c>Success</c> - into a new result using the specified
    /// <paramref name="onSuccessSelector"/> function. Otherwise, if the current result is <c>Fail</c>, it is transformed into a
    /// new <c>Fail</c> result with the same error.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Select</c> and <c>SelectMany</c> is in the return value of their <c>onSuccessSelector</c>
    /// function. The selector for <c>Select</c> returns a regular (non-result) value, which is the value of the returned
    /// <c>Success</c> result. The selector for <c>SelectMany</c> returns a result value, which is itself the returned result
    /// (and might not be <c>Success</c>).
    /// </remarks>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onSuccessSelector">A function that maps the value of the incoming result to the value of the outgoing
    ///     result. Evaluated only if the source is a <c>Success</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public static async Task<Result> SelectMany(
        this Task<Result> sourceResult,
        Func<Task<Result>> onSuccessSelector) =>
        await (await sourceResult.ConfigureAwait(false)).SelectMany(onSuccessSelector);

    /// <summary>
    /// Transforms the current result - if <c>Success</c> - into a new result using the specified
    /// <paramref name="onSuccessSelector"/> function. Otherwise, if the current result is <c>Fail</c>, it is transformed into a
    /// new <c>Fail</c> result with the same error.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Select</c> and <c>SelectMany</c> is in the return value of their <c>onSuccessSelector</c>
    /// function. The selector for <c>Select</c> returns a regular (non-result) value, which is the value of the returned
    /// <c>Success</c> result. The selector for <c>SelectMany</c> returns a result value, which is itself the returned result
    /// (and might not be <c>Success</c>).
    /// </remarks>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onSuccessSelector">A function that maps the value of the incoming result to the value of the outgoing
    ///     result. Evaluated only if the source is a <c>Success</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public static async Task<Result<TReturn>> SelectMany<TReturn>(
        this Task<Result> sourceResult,
        Func<Result<TReturn>> onSuccessSelector) =>
        (await sourceResult.ConfigureAwait(false)).SelectMany(onSuccessSelector);

    /// <summary>
    /// Transforms the current result - if <c>Success</c> - into a new result using the specified
    /// <paramref name="onSuccessSelector"/> function. Otherwise, if the current result is <c>Fail</c>, it is transformed into a
    /// new <c>Fail</c> result with the same error.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Select</c> and <c>SelectMany</c> is in the return value of their <c>onSuccessSelector</c>
    /// function. The selector for <c>Select</c> returns a regular (non-result) value, which is the value of the returned
    /// <c>Success</c> result. The selector for <c>SelectMany</c> returns a result value, which is itself the returned result
    /// (and might not be <c>Success</c>).
    /// </remarks>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onSuccessSelector">A function that maps the value of the incoming result to the value of the outgoing
    ///     result. Evaluated only if the source is a <c>Success</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public static async Task<Result<TReturn>> SelectMany<TReturn>(
        this Task<Result> sourceResult,
        Func<Task<Result<TReturn>>> onSuccessSelector) =>
        await (await sourceResult.ConfigureAwait(false)).SelectMany(onSuccessSelector);

    /// <summary>
    /// Transforms the current result - if <c>Success</c> - into a new result using the specified
    /// <paramref name="onSuccessSelector"/> function. Otherwise, if the current result is <c>Fail</c>, it is transformed into a
    /// new <c>Fail</c> result with the same error. A <c>None</c> result is never returned.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Select</c> and <c>SelectMany</c> is in the return value of their <c>onSuccessSelector</c>
    /// function. The selector for <c>Select</c> returns a regular (non-result) value, which is the value of the returned
    /// <c>Success</c> result. The selector for <c>SelectMany</c> returns a result value, which is itself the returned result
    /// (and might not be <c>Success</c>).
    /// </remarks>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onSuccessSelector">A function that maps the value of the incoming result to the value of the outgoing
    ///     result. Evaluated only if the source is a <c>Success</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public static async Task<Maybe<TReturn>> SelectMany<TReturn>(
        this Task<Result> sourceResult,
        Func<Maybe<TReturn>> onSuccessSelector) =>
        (await sourceResult.ConfigureAwait(false)).SelectMany(onSuccessSelector);

    /// <summary>
    /// Transforms the current result - if <c>Success</c> - into a new result using the specified
    /// <paramref name="onSuccessSelector"/> function. Otherwise, if the current result is <c>Fail</c>, it is transformed into a
    /// new <c>Fail</c> result with the same error. A <c>None</c> result is never returned.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Select</c> and <c>SelectMany</c> is in the return value of their <c>onSuccessSelector</c>
    /// function. The selector for <c>Select</c> returns a regular (non-result) value, which is the value of the returned
    /// <c>Success</c> result. The selector for <c>SelectMany</c> returns a result value, which is itself the returned result
    /// (and might not be <c>Success</c>).
    /// </remarks>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onSuccessSelector">A function that maps the value of the incoming result to the value of the outgoing
    ///     result. Evaluated only if the source is a <c>Success</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public static async Task<Maybe<TReturn>> SelectMany<TReturn>(
        this Task<Result> sourceResult,
        Func<Task<Maybe<TReturn>>> onSuccessSelector) =>
        await (await sourceResult.ConfigureAwait(false)).SelectMany(onSuccessSelector);

    /// <summary>
    /// Transforms the current result - if <c>Success</c> - into a new result using the specified
    /// <paramref name="onSuccessSelector"/> function. Otherwise, if the current result is <c>Fail</c>, it is transformed into a
    /// new <c>Fail</c> result with the same error.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Select</c> and <c>SelectMany</c> is in the return value of their <c>onSuccessSelector</c>
    /// function. The selector for <c>Select</c> returns a regular (non-result) value, which is the value of the returned
    /// <c>Success</c> result. The selector for <c>SelectMany</c> returns a result value, which is itself the returned result
    /// (and might not be <c>Success</c>).
    /// </remarks>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The task returning the source result.</param>
    /// <param name="onSuccessSelector">A function that maps the value of the incoming result to the outgoing result. Evaluated
    ///     only if the source is a <c>Success</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public static async Task<Result> SelectMany<T>(
        this Task<Result<T>> sourceResult,
        Func<T, Result> onSuccessSelector) =>
        (await sourceResult.ConfigureAwait(false)).SelectMany(onSuccessSelector);

    /// <summary>
    /// Transforms the current result - if <c>Success</c> - into a new result using the specified
    /// <paramref name="onSuccessSelector"/> function. Otherwise, if the current result is <c>Fail</c>, it is transformed into a
    /// new <c>Fail</c> result with the same error.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Select</c> and <c>SelectMany</c> is in the return value of their <c>onSuccessSelector</c>
    /// function. The selector for <c>Select</c> returns a regular (non-result) value, which is the value of the returned
    /// <c>Success</c> result. The selector for <c>SelectMany</c> returns a result value, which is itself the returned result
    /// (and might not be <c>Success</c>).
    /// </remarks>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The task returning the source result.</param>
    /// <param name="onSuccessSelector">A function that maps the value of the incoming result to the outgoing result. Evaluated
    ///     only if the source is a <c>Success</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public static async Task<Result> SelectMany<T>(
        this Task<Result<T>> sourceResult,
        Func<T, Task<Result>> onSuccessSelector) =>
        await (await sourceResult.ConfigureAwait(false)).SelectMany(onSuccessSelector).ConfigureAwait(false);

    /// <summary>
    /// Transforms the current result - if <c>Success</c> - into a new result using the specified
    /// <paramref name="onSuccessSelector"/> function. Otherwise, if the current result is <c>Fail</c>, it is transformed into a
    /// new <c>Fail</c> result with the same error.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Select</c> and <c>SelectMany</c> is in the return value of their <c>onSuccessSelector</c>
    /// function. The selector for <c>Select</c> returns a regular (non-result) value, which is the value of the returned
    /// <c>Success</c> result. The selector for <c>SelectMany</c> returns a result value, which is itself the returned result
    /// (and might not be <c>Success</c>).
    /// </remarks>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="sourceResult">The task returning the source result.</param>
    /// <param name="onSuccessSelector">A function that maps the value of the incoming result to the value of the outgoing
    ///     result. Evaluated only if the source is a <c>Success</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public static async Task<Result<TReturn>> SelectMany<T, TReturn>(
        this Task<Result<T>> sourceResult,
        Func<T, Result<TReturn>> onSuccessSelector) =>
        (await sourceResult.ConfigureAwait(false)).SelectMany(onSuccessSelector);

    /// <summary>
    /// Transforms the current result - if <c>Success</c> - into a new result using the specified
    /// <paramref name="onSuccessSelector"/> function. Otherwise, if the current result is <c>Fail</c>, it is transformed into a
    /// new <c>Fail</c> result with the same error.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Select</c> and <c>SelectMany</c> is in the return value of their <c>onSuccessSelector</c>
    /// function. The selector for <c>Select</c> returns a regular (non-result) value, which is the value of the returned
    /// <c>Success</c> result. The selector for <c>SelectMany</c> returns a result value, which is itself the returned result
    /// (and might not be <c>Success</c>).
    /// </remarks>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="sourceResult">The task returning the source result.</param>
    /// <param name="onSuccessSelector">A function that maps the value of the incoming result to the value of the outgoing
    ///     result. Evaluated only if the source is a <c>Success</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public static async Task<Result<TReturn>> SelectMany<T, TReturn>(
        this Task<Result<T>> sourceResult,
        Func<T, Task<Result<TReturn>>> onSuccessSelector) =>
        await (await sourceResult.ConfigureAwait(false)).SelectMany(onSuccessSelector).ConfigureAwait(false);

    /// <summary>
    /// Transforms the current result - if <c>Success</c> - into a new result using the specified
    /// <paramref name="onSuccessSelector"/> function. Otherwise, if the current result is <c>Fail</c>, it is transformed into a
    /// new <c>Fail</c> result with the same error. A <c>None</c> result is never returned.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Select</c> and <c>SelectMany</c> is in the return value of their <c>onSuccessSelector</c>
    /// function. The selector for <c>Select</c> returns a regular (non-result) value, which is the value of the returned
    /// <c>Success</c> result. The selector for <c>SelectMany</c> returns a result value, which is itself the returned result
    /// (and might not be <c>Success</c>).
    /// </remarks>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="sourceResult">The task returning the source result.</param>
    /// <param name="onSuccessSelector">A function that maps the value of the incoming result to the outgoing result. Evaluated
    ///     only if the source is a <c>Success</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public static async Task<Maybe<TReturn>> SelectMany<T, TReturn>(
        this Task<Result<T>> sourceResult,
        Func<T, Maybe<TReturn>> onSuccessSelector) =>
        (await sourceResult.ConfigureAwait(false)).SelectMany(onSuccessSelector);

    /// <summary>
    /// Transforms the current result - if <c>Success</c> - into a new result using the specified
    /// <paramref name="onSuccessSelector"/> function. Otherwise, if the current result is <c>Fail</c>, it is transformed into a
    /// new <c>Fail</c> result with the same error. A <c>None</c> result is never returned.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Select</c> and <c>SelectMany</c> is in the return value of their <c>onSuccessSelector</c>
    /// function. The selector for <c>Select</c> returns a regular (non-result) value, which is the value of the returned
    /// <c>Success</c> result. The selector for <c>SelectMany</c> returns a result value, which is itself the returned result
    /// (and might not be <c>Success</c>).
    /// </remarks>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="sourceResult">The task returning the source result.</param>
    /// <param name="onSuccessSelector">A function that maps the value of the incoming result to the outgoing result. Evaluated
    ///     only if the source is a <c>Success</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public static async Task<Maybe<TReturn>> SelectMany<T, TReturn>(
        this Task<Result<T>> sourceResult,
        Func<T, Task<Maybe<TReturn>>> onSuccessSelector) =>
        await (await sourceResult.ConfigureAwait(false)).SelectMany(onSuccessSelector).ConfigureAwait(false);

    /// <summary>
    /// Transforms the current result - if <c>Success</c> - into a new result using the specified
    /// <paramref name="onSuccessSelector"/> function. If the current result is <c>Fail</c>, it is transformed into a new
    /// <c>Fail</c> result with the same error. Otherwise, if the current result is <c>None</c>, it is transformed into a new
    /// <c>Fail</c> result with an error indicating that there was no value.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Select</c> and <c>SelectMany</c> is in the return value of their <c>onSuccessSelector</c>
    /// function. The selector for <c>Select</c> returns a regular (non-result) value, which is the value of the returned
    /// <c>Success</c> result. The selector for <c>SelectMany</c> returns a result value, which is itself the returned result
    /// (and might not be <c>Success</c>).
    /// </remarks>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The task returning the source result.</param>
    /// <param name="onSuccessSelector">A function that maps the value of the incoming result to the outgoing result. Evaluated
    ///     only if the source is a <c>Success</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public static async Task<Result> SelectMany<T>(
        this Task<Maybe<T>> sourceResult,
        Func<T, Result> onSuccessSelector) =>
        (await sourceResult.ConfigureAwait(false)).SelectMany(onSuccessSelector);

    /// <summary>
    /// Transforms the current result - if <c>Success</c> - into a new result using the specified
    /// <paramref name="onSuccessSelector"/> function. If the current result is <c>Fail</c>, it is transformed into a new
    /// <c>Fail</c> result with the same error. Otherwise, if the current result is <c>None</c>, it is transformed into a new
    /// <c>Fail</c> result with an error indicating that there was no value.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Select</c> and <c>SelectMany</c> is in the return value of their <c>onSuccessSelector</c>
    /// function. The selector for <c>Select</c> returns a regular (non-result) value, which is the value of the returned
    /// <c>Success</c> result. The selector for <c>SelectMany</c> returns a result value, which is itself the returned result
    /// (and might not be <c>Success</c>).
    /// </remarks>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The task returning the source result.</param>
    /// <param name="onSuccessSelector">A function that maps the value of the incoming result to the outgoing result. Evaluated
    ///     only if the source is a <c>Success</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public static async Task<Result> SelectMany<T>(
        this Task<Maybe<T>> sourceResult,
        Func<T, Task<Result>> onSuccessSelector) =>
        await (await sourceResult.ConfigureAwait(false)).SelectMany(onSuccessSelector).ConfigureAwait(false);

    /// <summary>
    /// Transforms the current result - if <c>Success</c> - into a new result using the specified
    /// <paramref name="onSuccessSelector"/> function. If the current result is <c>Fail</c>, it is transformed into a new
    /// <c>Fail</c> result with the same error. Otherwise, if the current result is <c>None</c>, it is transformed into a new
    /// <c>Fail</c> result with an error indicating that there was no value.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Select</c> and <c>SelectMany</c> is in the return value of their <c>onSuccessSelector</c>
    /// function. The selector for <c>Select</c> returns a regular (non-result) value, which is the value of the returned
    /// <c>Success</c> result. The selector for <c>SelectMany</c> returns a result value, which is itself the returned result
    /// (and might not be <c>Success</c>).
    /// </remarks>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="sourceResult">The task returning the source result.</param>
    /// <param name="onSuccessSelector">A function that maps the value of the incoming result to the outgoing result. Evaluated
    ///     only if the source is a <c>Success</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public static async Task<Result<TReturn>> SelectMany<T, TReturn>(
        this Task<Maybe<T>> sourceResult,
        Func<T, Result<TReturn>> onSuccessSelector) =>
        (await sourceResult.ConfigureAwait(false)).SelectMany(onSuccessSelector);

    /// <summary>
    /// Transforms the current result - if <c>Success</c> - into a new result using the specified
    /// <paramref name="onSuccessSelector"/> function. If the current result is <c>Fail</c>, it is transformed into a new
    /// <c>Fail</c> result with the same error. Otherwise, if the current result is <c>None</c>, it is transformed into a new
    /// <c>Fail</c> result with an error indicating that there was no value.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Select</c> and <c>SelectMany</c> is in the return value of their <c>onSuccessSelector</c>
    /// function. The selector for <c>Select</c> returns a regular (non-result) value, which is the value of the returned
    /// <c>Success</c> result. The selector for <c>SelectMany</c> returns a result value, which is itself the returned result
    /// (and might not be <c>Success</c>).
    /// </remarks>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="sourceResult">The task returning the source result.</param>
    /// <param name="onSuccessSelector">A function that maps the value of the incoming result to the outgoing result. Evaluated
    ///     only if the source is a <c>Success</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public static async Task<Result<TReturn>> SelectMany<T, TReturn>(
        this Task<Maybe<T>> sourceResult,
        Func<T, Task<Result<TReturn>>> onSuccessSelector) =>
        await (await sourceResult.ConfigureAwait(false)).SelectMany(onSuccessSelector).ConfigureAwait(false);

    /// <summary>
    /// Transforms the current result - if <c>Success</c> - into a new result using the specified
    /// <paramref name="onSuccessSelector"/> function. If the current result is <c>Fail</c>, it is transformed into a new
    /// <c>Fail</c> result with the same error. Otherwise, if the current result is <c>None</c>, it is transformed into a new
    /// <c>None</c> result.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Select</c> and <c>SelectMany</c> is in the return value of their <c>onSuccessSelector</c>
    /// function. The selector for <c>Select</c> returns a regular (non-result) value, which is the value of the returned
    /// <c>Success</c> result. The selector for <c>SelectMany</c> returns a result value, which is itself the returned result
    /// (and might not be <c>Success</c>).
    /// </remarks>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="sourceResult">The task returning the source result.</param>
    /// <param name="onSuccessSelector">A function that maps the value of the incoming result to the value of the outgoing
    ///     result. Evaluated only if the source is a <c>Success</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public static async Task<Maybe<TReturn>> SelectMany<T, TReturn>(
        this Task<Maybe<T>> sourceResult,
        Func<T, Maybe<TReturn>> onSuccessSelector) =>
        (await sourceResult.ConfigureAwait(false)).SelectMany(onSuccessSelector);

    /// <summary>
    /// Transforms the current result - if <c>Success</c> - into a new result using the specified
    /// <paramref name="onSuccessSelector"/> function. If the current result is <c>Fail</c>, it is transformed into a new
    /// <c>Fail</c> result with the same error. Otherwise, if the current result is <c>None</c>, it is transformed into a new
    /// <c>None</c> result.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Select</c> and <c>SelectMany</c> is in the return value of their <c>onSuccessSelector</c>
    /// function. The selector for <c>Select</c> returns a regular (non-result) value, which is the value of the returned
    /// <c>Success</c> result. The selector for <c>SelectMany</c> returns a result value, which is itself the returned result
    /// (and might not be <c>Success</c>).
    /// </remarks>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="sourceResult">The task returning the source result.</param>
    /// <param name="onSuccessSelector">A function that maps the value of the incoming result to the value of the outgoing
    ///     result. Evaluated only if the source is a <c>Success</c> result.</param>
    /// <returns>The mapped result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public static async Task<Maybe<TReturn>> SelectMany<T, TReturn>(
        this Task<Maybe<T>> sourceResult,
        Func<T, Task<Maybe<TReturn>>> onSuccessSelector) =>
        await (await sourceResult.ConfigureAwait(false)).SelectMany(onSuccessSelector).ConfigureAwait(false);

    /// <summary>
    /// Projects the value of a result to an intermediate result and invokes a result selector function on the values of the
    /// source and intermediate results.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TIntermediate">The type of the intermediate result, as collected by
    ///     <paramref name="intermediateSelector"/>.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="intermediateSelector">A transform function to apply to the value of the input result.</param>
    /// <param name="resultSelector">A transform function to apply to the value of the intermediate result.</param>
    /// <returns>An <see cref="Maybe{T}"/> whose value is the result of invoking the transform function
    ///     <paramref name="intermediateSelector"/> on the value of <paramref name="sourceResult"/> and then mapping the values
    ///     of that result and the source result to the final result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="intermediateSelector"/> is <see langword="null"/>, or if
    ///     <paramref name="resultSelector"/> is <see langword="null"/>.</exception>
    public static Task<Result<TReturn>> SelectMany<T, TIntermediate, TReturn>(
        this Task<Result<T>> sourceResult,
        Func<T, Result<TIntermediate>> intermediateSelector,
        Func<T, TIntermediate, TReturn> resultSelector)
    {
        if (intermediateSelector is null) throw new ArgumentNullException(nameof(intermediateSelector));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return sourceResult.SelectMany(
            sourceValue => intermediateSelector(sourceValue).Select(
                intermediateValue => resultSelector(sourceValue, intermediateValue)));
    }

    /// <summary>
    /// Projects the value of a result to an intermediate result and invokes a result selector function on the values of the
    /// source and intermediate results.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TIntermediate">The type of the intermediate result, as collected by
    ///     <paramref name="intermediateSelector"/>.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="intermediateSelector">A transform function to apply to the value of the input result.</param>
    /// <param name="resultSelector">A transform function to apply to the value of the intermediate result.</param>
    /// <returns>An <see cref="Maybe{T}"/> whose value is the result of invoking the transform function
    ///     <paramref name="intermediateSelector"/> on the value of <paramref name="sourceResult"/> and then mapping the values
    ///     of that result and the source result to the final result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="intermediateSelector"/> is <see langword="null"/>, or if
    ///     <paramref name="resultSelector"/> is <see langword="null"/>.</exception>
    public static Task<Maybe<TReturn>> SelectMany<T, TIntermediate, TReturn>(
        this Task<Result<T>> sourceResult,
        Func<T, Maybe<TIntermediate>> intermediateSelector,
        Func<T, TIntermediate, TReturn> resultSelector)
    {
        if (intermediateSelector is null) throw new ArgumentNullException(nameof(intermediateSelector));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return sourceResult.SelectMany(
            sourceValue => intermediateSelector(sourceValue).Select(
                intermediateValue => resultSelector(sourceValue, intermediateValue)));
    }

    /// <summary>
    /// Projects the value of a result to an intermediate result and invokes a result selector function on the values of the
    /// source and intermediate results.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TIntermediate">The type of the intermediate result, as collected by
    ///     <paramref name="intermediateSelector"/>.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="intermediateSelector">A transform function to apply to the value of the input result.</param>
    /// <param name="resultSelector">A transform function to apply to the value of the intermediate result.</param>
    /// <returns>An <see cref="Maybe{T}"/> whose value is the result of invoking the transform function
    ///     <paramref name="intermediateSelector"/> on the value of <paramref name="sourceResult"/> and then mapping the values
    ///     of that result and the source result to the final result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="intermediateSelector"/> is <see langword="null"/>, or if
    ///     <paramref name="resultSelector"/> is <see langword="null"/>.</exception>
    public static Task<Result<TReturn>> SelectMany<T, TIntermediate, TReturn>(
        this Task<Result<T>> sourceResult,
        Func<T, Task<Result<TIntermediate>>> intermediateSelector,
        Func<T, TIntermediate, TReturn> resultSelector)
    {
        if (intermediateSelector is null) throw new ArgumentNullException(nameof(intermediateSelector));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return sourceResult.SelectMany(
            sourceValue => intermediateSelector(sourceValue).Select(
                intermediateValue => resultSelector(sourceValue, intermediateValue)));
    }

    /// <summary>
    /// Projects the value of a result to an intermediate result and invokes a result selector function on the values of the
    /// source and intermediate results.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TIntermediate">The type of the intermediate result, as collected by
    ///     <paramref name="intermediateSelector"/>.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="intermediateSelector">A transform function to apply to the value of the input result.</param>
    /// <param name="resultSelector">A transform function to apply to the value of the intermediate result.</param>
    /// <returns>An <see cref="Maybe{T}"/> whose value is the result of invoking the transform function
    ///     <paramref name="intermediateSelector"/> on the value of <paramref name="sourceResult"/> and then mapping the values
    ///     of that result and the source result to the final result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="intermediateSelector"/> is <see langword="null"/>, or if
    ///     <paramref name="resultSelector"/> is <see langword="null"/>.</exception>
    public static Task<Maybe<TReturn>> SelectMany<T, TIntermediate, TReturn>(
        this Task<Result<T>> sourceResult,
        Func<T, Task<Maybe<TIntermediate>>> intermediateSelector,
        Func<T, TIntermediate, TReturn> resultSelector)
    {
        if (intermediateSelector is null) throw new ArgumentNullException(nameof(intermediateSelector));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return sourceResult.SelectMany(
            sourceValue => intermediateSelector(sourceValue).Select(
                intermediateValue => resultSelector(sourceValue, intermediateValue)));
    }

    /// <summary>
    /// Projects the value of a result to an intermediate result and invokes a result selector function on the values of the
    /// source and intermediate results.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TIntermediate">The type of the intermediate result collected by <paramref name="intermediateSelector"/>.
    ///     </typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="intermediateSelector">A transform function to apply to the value of the input result.</param>
    /// <param name="resultSelector">A transform function to apply to the value of the intermediate result.</param>
    /// <returns>An <see cref="Maybe{T}"/> whose value is the result of invoking the transform function
    ///     <paramref name="intermediateSelector"/> on the value of <paramref name="sourceResult"/> and then mapping the values
    ///     of that result and the source result to the final result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="intermediateSelector"/> is <see langword="null"/>, or if
    ///     <paramref name="resultSelector"/> is <see langword="null"/>.</exception>
    public static Task<Result<TReturn>> SelectMany<T, TIntermediate, TReturn>(
        this Task<Maybe<T>> sourceResult,
        Func<T, Result<TIntermediate>> intermediateSelector,
        Func<T, TIntermediate, TReturn> resultSelector)
    {
        if (intermediateSelector is null) throw new ArgumentNullException(nameof(intermediateSelector));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return sourceResult.SelectMany(
            sourceValue => intermediateSelector(sourceValue).Select(
                intermediateValue => resultSelector(sourceValue, intermediateValue)));
    }

    /// <summary>
    /// Projects the value of a result to an intermediate result and invokes a result selector function on the values of the
    /// source and intermediate results.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TIntermediate">The type of the intermediate result collected by <paramref name="intermediateSelector"/>.
    ///     </typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="intermediateSelector">A transform function to apply to the value of the input result.</param>
    /// <param name="resultSelector">A transform function to apply to the value of the intermediate result.</param>
    /// <returns>An <see cref="Maybe{T}"/> whose value is the result of invoking the transform function
    ///     <paramref name="intermediateSelector"/> on the value of <paramref name="sourceResult"/> and then mapping the values
    ///     of that result and the source result to the final result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="intermediateSelector"/> is <see langword="null"/>, or if
    ///     <paramref name="resultSelector"/> is <see langword="null"/>.</exception>
    public static Task<Maybe<TReturn>> SelectMany<T, TIntermediate, TReturn>(
        this Task<Maybe<T>> sourceResult,
        Func<T, Maybe<TIntermediate>> intermediateSelector,
        Func<T, TIntermediate, TReturn> resultSelector)
    {
        if (intermediateSelector is null) throw new ArgumentNullException(nameof(intermediateSelector));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return sourceResult.SelectMany(
            sourceValue => intermediateSelector(sourceValue).Select(
                intermediateValue => resultSelector(sourceValue, intermediateValue)));
    }

    /// <summary>
    /// Projects the value of a result to an intermediate result and invokes a result selector function on the values of the
    /// source and intermediate results.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TIntermediate">The type of the intermediate result collected by <paramref name="intermediateSelector"/>.
    ///     </typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="intermediateSelector">A transform function to apply to the value of the input result.</param>
    /// <param name="resultSelector">A transform function to apply to the value of the intermediate result.</param>
    /// <returns>An <see cref="Maybe{T}"/> whose value is the result of invoking the transform function
    ///     <paramref name="intermediateSelector"/> on the value of <paramref name="sourceResult"/> and then mapping the values
    ///     of that result and the source result to the final result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="intermediateSelector"/> is <see langword="null"/>, or if
    ///     <paramref name="resultSelector"/> is <see langword="null"/>.</exception>
    public static Task<Result<TReturn>> SelectMany<T, TIntermediate, TReturn>(
        this Task<Maybe<T>> sourceResult,
        Func<T, Task<Result<TIntermediate>>> intermediateSelector,
        Func<T, TIntermediate, TReturn> resultSelector)
    {
        if (intermediateSelector is null) throw new ArgumentNullException(nameof(intermediateSelector));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return sourceResult.SelectMany(
            sourceValue => intermediateSelector(sourceValue).Select(
                intermediateValue => resultSelector(sourceValue, intermediateValue)));
    }

    /// <summary>
    /// Projects the value of a result to an intermediate result and invokes a result selector function on the values of the
    /// source and intermediate results.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TIntermediate">The type of the intermediate result collected by <paramref name="intermediateSelector"/>.
    ///     </typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="intermediateSelector">A transform function to apply to the value of the input result.</param>
    /// <param name="resultSelector">A transform function to apply to the value of the intermediate result.</param>
    /// <returns>An <see cref="Maybe{T}"/> whose value is the result of invoking the transform function
    ///     <paramref name="intermediateSelector"/> on the value of <paramref name="sourceResult"/> and then mapping the values
    ///     of that result and the source result to the final result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="intermediateSelector"/> is <see langword="null"/>, or if
    ///     <paramref name="resultSelector"/> is <see langword="null"/>.</exception>
    public static Task<Maybe<TReturn>> SelectMany<T, TIntermediate, TReturn>(
        this Task<Maybe<T>> sourceResult,
        Func<T, Task<Maybe<TIntermediate>>> intermediateSelector,
        Func<T, TIntermediate, TReturn> resultSelector)
    {
        if (intermediateSelector is null) throw new ArgumentNullException(nameof(intermediateSelector));
        if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

        return sourceResult.SelectMany(
            sourceValue => intermediateSelector(sourceValue).Select(
                intermediateValue => resultSelector(sourceValue, intermediateValue)));
    }
}
