namespace RandomSkunk.Results;

/// <content> Defines the <c>SelectMany</c> methods. </content>
public partial struct Result
{
    /// <summary>
    /// Projects the result into a new <see cref="Result"/> form: a <c>Success</c> result is transformed to the new form by
    /// invoking the <paramref name="onSuccessSelector"/> function; a <c>Fail</c> result is transformed to the new form as a
    /// <c>Fail</c> result with the same error.
    /// </summary>
    /// <param name="onSuccessSelector">A transform funtion to apply to the value of a <c>Success</c> result.</param>
    /// <returns>The projected result.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public Result SelectMany(Func<Result> onSuccessSelector)
    {
        if (onSuccessSelector is null) throw new ArgumentNullException(nameof(onSuccessSelector));

        return _outcome switch
        {
            Outcome.Success => onSuccessSelector(),
            _ => Fail(GetError(), false),
        };
    }

    /// <summary>
    /// Projects the result into a new <see cref="Result"/> form: a <c>Success</c> result is transformed to the new form by
    /// invoking the <paramref name="onSuccessSelector"/> function; a <c>Fail</c> result is transformed to the new form as a
    /// <c>Fail</c> result with the same error.
    /// </summary>
    /// <param name="onSuccessSelector">A transform funtion to apply to the value of a <c>Success</c> result.</param>
    /// <returns>The projected result.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public async Task<Result> SelectMany(Func<Task<Result>> onSuccessSelector)
    {
        if (onSuccessSelector is null) throw new ArgumentNullException(nameof(onSuccessSelector));

        return _outcome switch
        {
            Outcome.Success => await onSuccessSelector().ConfigureAwait(false),
            _ => Fail(GetError(), false),
        };
    }

    /// <summary>
    /// Projects the result into a new <see cref="Result{T}"/> form: a <c>Success</c> result is transformed to the new form by
    /// invoking the <paramref name="onSuccessSelector"/> function; a <c>Fail</c> result is transformed to the new form as a
    /// <c>Fail</c> result with the same error.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Select</c> and <c>SelectMany</c> is in the return value of their <c>onSuccessSelector</c>
    /// function. The selector for <c>Select</c> returns a regular (non-result) value, which is the value of the returned
    /// <c>Success</c> result. The selector for <c>SelectMany</c> returns a result value, which is itself the returned result
    /// (and may or may not be <c>Success</c>).
    /// </remarks>
    /// <typeparam name="TReturn">The type of the <see cref="Result{T}"/> returned by <paramref name="onSuccessSelector"/>.
    ///     </typeparam>
    /// <param name="onSuccessSelector">A transform funtion to apply to the value of a <c>Success</c> result.</param>
    /// <returns>The projected result.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public Result<TReturn> SelectMany<TReturn>(Func<Result<TReturn>> onSuccessSelector)
    {
        if (onSuccessSelector is null) throw new ArgumentNullException(nameof(onSuccessSelector));

        return _outcome switch
        {
            Outcome.Success => onSuccessSelector(),
            _ => Result<TReturn>.Fail(GetError(), false),
        };
    }

    /// <summary>
    /// Projects the result into a new <see cref="Result{T}"/> form: a <c>Success</c> result is transformed to the new form by
    /// invoking the <paramref name="onSuccessSelector"/> function; a <c>Fail</c> result is transformed to the new form as a
    /// <c>Fail</c> result with the same error.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Select</c> and <c>SelectMany</c> is in the return value of their <c>onSuccessSelector</c>
    /// function. The selector for <c>Select</c> returns a regular (non-result) value, which is the value of the returned
    /// <c>Success</c> result. The selector for <c>SelectMany</c> returns a result value, which is itself the returned result
    /// (and may or may not be <c>Success</c>).
    /// </remarks>
    /// <typeparam name="TReturn">The type of the <see cref="Result{T}"/> returned by <paramref name="onSuccessSelector"/>.
    ///     </typeparam>
    /// <param name="onSuccessSelector">A transform funtion to apply to the value of a <c>Success</c> result.</param>
    /// <returns>The projected result.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public async Task<Result<TReturn>> SelectMany<TReturn>(Func<Task<Result<TReturn>>> onSuccessSelector)
    {
        if (onSuccessSelector is null) throw new ArgumentNullException(nameof(onSuccessSelector));

        return _outcome switch
        {
            Outcome.Success => await onSuccessSelector().ConfigureAwait(false),
            _ => Result<TReturn>.Fail(GetError(), false),
        };
    }

    /// <summary>
    /// Projects the result into a new <see cref="Maybe{T}"/> form: a <c>Success</c> result is transformed to the new form by
    /// invoking the <paramref name="onSuccessSelector"/> function; a <c>Fail</c> result is transformed to the new form as a
    /// <c>Fail</c> result with the same error.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Select</c> and <c>SelectMany</c> is in the return value of their <c>onSuccessSelector</c>
    /// function. The selector for <c>Select</c> returns a regular (non-result) value, which is the value of the returned
    /// <c>Success</c> result. The selector for <c>SelectMany</c> returns a result value, which is itself the returned result
    /// (and may or may not be <c>Success</c>).
    /// </remarks>
    /// <typeparam name="TReturn">The type of the <see cref="Maybe{T}"/> returned by <paramref name="onSuccessSelector"/>.
    ///     </typeparam>
    /// <param name="onSuccessSelector">A transform funtion to apply to the value of a <c>Success</c> result.</param>
    /// <returns>The projected result.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public Maybe<TReturn> SelectMany<TReturn>(Func<Maybe<TReturn>> onSuccessSelector)
    {
        if (onSuccessSelector is null) throw new ArgumentNullException(nameof(onSuccessSelector));

        if (_outcome == Outcome.Success)
            return onSuccessSelector();

        var error = GetError();
        if (error.ErrorCode == ErrorCodes.NoneResult)
            return Maybe<TReturn>.None;

        return Maybe<TReturn>.Fail(error, false);
    }

    /// <summary>
    /// Projects the result into a new <see cref="Maybe{T}"/> form: a <c>Success</c> result is transformed to the new form by
    /// invoking the <paramref name="onSuccessSelector"/> function; a <c>Fail</c> result is transformed to the new form as a
    /// <c>Fail</c> result with the same error.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Select</c> and <c>SelectMany</c> is in the return value of their <c>onSuccessSelector</c>
    /// function. The selector for <c>Select</c> returns a regular (non-result) value, which is the value of the returned
    /// <c>Success</c> result. The selector for <c>SelectMany</c> returns a result value, which is itself the returned result
    /// (and may or may not be <c>Success</c>).
    /// </remarks>
    /// <typeparam name="TReturn">The type of the <see cref="Maybe{T}"/> returned by <paramref name="onSuccessSelector"/>.
    ///     </typeparam>
    /// <param name="onSuccessSelector">A transform funtion to apply to the value of a <c>Success</c> result.</param>
    /// <returns>The projected result.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public async Task<Maybe<TReturn>> SelectMany<TReturn>(Func<Task<Maybe<TReturn>>> onSuccessSelector)
    {
        if (onSuccessSelector is null) throw new ArgumentNullException(nameof(onSuccessSelector));

        if (_outcome == Outcome.Success)
            return await onSuccessSelector().ConfigureAwait(false);

        var error = GetError();
        if (error.ErrorCode == ErrorCodes.NoneResult)
            return Maybe<TReturn>.None;

        return Maybe<TReturn>.Fail(error, false);
    }
}

/// <content> Defines the <c>SelectMany</c> methods. </content>
public partial struct Result<T>
{
    /// <summary>
    /// Projects the result into a new <see cref="Result{T}"/> form: a <c>Success</c> result is transformed to the new form by
    /// passing its value to the <paramref name="onSuccessSelector"/> function; a <c>Fail</c> result is transformed to the new
    /// form as a <c>Fail</c> result with the same error.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Select</c> and <c>SelectMany</c> is in the return value of their <c>onSuccessSelector</c>
    /// function. The selector for <c>Select</c> returns a regular (non-result) value, which is the value of the returned
    /// <c>Success</c> result. The selector for <c>SelectMany</c> returns a result value, which is itself the returned result
    /// (and may or may not be <c>Success</c>).
    /// </remarks>
    /// <typeparam name="TReturn">The type of the <see cref="Result{T}"/> returned by <paramref name="onSuccessSelector"/>.
    ///     </typeparam>
    /// <param name="onSuccessSelector">A transform funtion to apply to the value of a <c>Success</c> result.</param>
    /// <returns>The projected result.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public Result<TReturn> SelectMany<TReturn>(Func<T, Result<TReturn>> onSuccessSelector)
    {
        if (onSuccessSelector is null) throw new ArgumentNullException(nameof(onSuccessSelector));

        return _outcome switch
        {
            Outcome.Success => onSuccessSelector(_value!),
            _ => Result<TReturn>.Fail(GetError(), false),
        };
    }

    /// <summary>
    /// Projects the result into a new <see cref="Result{T}"/> form: a <c>Success</c> result is transformed to the new form by
    /// passing its value to the <paramref name="onSuccessSelector"/> function; a <c>Fail</c> result is transformed to the new
    /// form as a <c>Fail</c> result with the same error.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Select</c> and <c>SelectMany</c> is in the return value of their <c>onSuccessSelector</c>
    /// function. The selector for <c>Select</c> returns a regular (non-result) value, which is the value of the returned
    /// <c>Success</c> result. The selector for <c>SelectMany</c> returns a result value, which is itself the returned result
    /// (and may or may not be <c>Success</c>).
    /// </remarks>
    /// <typeparam name="TReturn">The type of the <see cref="Result{T}"/> returned by <paramref name="onSuccessSelector"/>.
    ///     </typeparam>
    /// <param name="onSuccessSelector">A transform funtion to apply to the value of a <c>Success</c> result.</param>
    /// <returns>The projected result.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public async Task<Result<TReturn>> SelectMany<TReturn>(Func<T, Task<Result<TReturn>>> onSuccessSelector)
    {
        if (onSuccessSelector is null) throw new ArgumentNullException(nameof(onSuccessSelector));

        return _outcome switch
        {
            Outcome.Success => await onSuccessSelector(_value!).ConfigureAwait(false),
            _ => Result<TReturn>.Fail(GetError(), false),
        };
    }

    /// <summary>
    /// Projects the result into a new <see cref="Result"/> form: a <c>Success</c> result is transformed to the new form by
    /// passing its value to the <paramref name="onSuccessSelector"/> function; a <c>Fail</c> result is transformed to the new
    /// form as a <c>Fail</c> result with the same error.
    /// </summary>
    /// <param name="onSuccessSelector">A transform funtion to apply to the value of a <c>Success</c> result.</param>
    /// <returns>The projected result.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public Result SelectMany(Func<T, Result> onSuccessSelector)
    {
        if (onSuccessSelector is null) throw new ArgumentNullException(nameof(onSuccessSelector));

        return _outcome switch
        {
            Outcome.Success => onSuccessSelector(_value!),
            _ => Result.Fail(GetError(), false),
        };
    }

    /// <summary>
    /// Projects the result into a new <see cref="Result"/> form: a <c>Success</c> result is transformed to the new form by
    /// passing its value to the <paramref name="onSuccessSelector"/> function; a <c>Fail</c> result is transformed to the new
    /// form as a <c>Fail</c> result with the same error.
    /// </summary>
    /// <param name="onSuccessSelector">A transform funtion to apply to the value of a <c>Success</c> result.</param>
    /// <returns>The projected result.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public async Task<Result> SelectMany(Func<T, Task<Result>> onSuccessSelector)
    {
        if (onSuccessSelector is null) throw new ArgumentNullException(nameof(onSuccessSelector));

        return _outcome switch
        {
            Outcome.Success => await onSuccessSelector(_value!).ConfigureAwait(false),
            _ => Result.Fail(GetError(), false),
        };
    }

    /// <summary>
    /// Projects the result into a new <see cref="Maybe{T}"/> form: a <c>Success</c> result is transformed to the new form by
    /// passing its value to the <paramref name="onSuccessSelector"/> function; a <c>Fail</c> result is transformed to the new
    /// form as a <c>Fail</c> result with the same error.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Select</c> and <c>SelectMany</c> is in the return value of their <c>onSuccessSelector</c>
    /// function. The selector for <c>Select</c> returns a regular (non-result) value, which is the value of the returned
    /// <c>Success</c> result. The selector for <c>SelectMany</c> returns a result value, which is itself the returned result
    /// (and may or may not be <c>Success</c>).
    /// </remarks>
    /// <typeparam name="TReturn">The type of the <see cref="Maybe{T}"/> returned by <paramref name="onSuccessSelector"/>.
    ///     </typeparam>
    /// <param name="onSuccessSelector">A transform funtion to apply to the value of a <c>Success</c> result.</param>
    /// <returns>The projected result.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public Maybe<TReturn> SelectMany<TReturn>(Func<T, Maybe<TReturn>> onSuccessSelector)
    {
        if (onSuccessSelector is null) throw new ArgumentNullException(nameof(onSuccessSelector));

        if (_outcome == Outcome.Success)
            return onSuccessSelector(_value!);

        var error = GetError();
        if (error.ErrorCode == ErrorCodes.NoneResult)
            return Maybe<TReturn>.None;

        return Maybe<TReturn>.Fail(error, false);
    }

    /// <summary>
    /// Projects the result into a new <see cref="Maybe{T}"/> form: a <c>Success</c> result is transformed to the new form by
    /// passing its value to the <paramref name="onSuccessSelector"/> function; a <c>Fail</c> result is transformed to the new
    /// form as a <c>Fail</c> result with the same error.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Select</c> and <c>SelectMany</c> is in the return value of their <c>onSuccessSelector</c>
    /// function. The selector for <c>Select</c> returns a regular (non-result) value, which is the value of the returned
    /// <c>Success</c> result. The selector for <c>SelectMany</c> returns a result value, which is itself the returned result
    /// (and may or may not be <c>Success</c>).
    /// </remarks>
    /// <typeparam name="TReturn">The type of the <see cref="Maybe{T}"/> returned by <paramref name="onSuccessSelector"/>.
    ///     </typeparam>
    /// <param name="onSuccessSelector">A transform funtion to apply to the value of a <c>Success</c> result.</param>
    /// <returns>The projected result.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public async Task<Maybe<TReturn>> SelectMany<TReturn>(Func<T, Task<Maybe<TReturn>>> onSuccessSelector)
    {
        if (onSuccessSelector is null) throw new ArgumentNullException(nameof(onSuccessSelector));

        if (_outcome == Outcome.Success)
            return await onSuccessSelector(_value!).ConfigureAwait(false);

        var error = GetError();
        if (error.ErrorCode == ErrorCodes.NoneResult)
            return Maybe<TReturn>.None;

        return Maybe<TReturn>.Fail(error, false);
    }

    /// <summary>
    /// Projects the value of a result to an intermediate result and invokes a result selector function on the values of the
    /// source and intermediate results.
    /// </summary>
    /// <typeparam name="TIntermediate">The type of the intermediate result, as collected by
    ///     <paramref name="intermediateSelector"/>.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="intermediateSelector">A transform function to apply to the value of the input result.</param>
    /// <param name="returnSelector">A transform function to apply to the value of the intermediate result.</param>
    /// <returns>A <see cref="Result{T}"/> whose value is the result of invoking the transform function
    ///     <paramref name="intermediateSelector"/> on the value of this result and then mapping the values of that result and
    ///     the source result to the final result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="intermediateSelector"/> is <see langword="null"/>, or if
    ///     <paramref name="returnSelector"/> is <see langword="null"/>.</exception>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public Result<TReturn> SelectMany<TIntermediate, TReturn>(
        Func<T, Result<TIntermediate>> intermediateSelector,
        Func<T, TIntermediate, TReturn> returnSelector)
    {
        if (intermediateSelector is null) throw new ArgumentNullException(nameof(intermediateSelector));
        if (returnSelector is null) throw new ArgumentNullException(nameof(returnSelector));

        return SelectMany(
            sourceValue => intermediateSelector(sourceValue).Select(
                intermediateValue => returnSelector(sourceValue, intermediateValue)));
    }

    /// <summary>
    /// Projects the value of a result to an intermediate result and invokes a result selector function on the values of the
    /// source and intermediate results.
    /// </summary>
    /// <typeparam name="TIntermediate">The type of the intermediate result, as collected by
    ///     <paramref name="intermediateSelector"/>.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="intermediateSelector">A transform function to apply to the value of the input result.</param>
    /// <param name="returnSelector">A transform function to apply to the value of the intermediate result.</param>
    /// <returns>A <see cref="Maybe{T}"/> whose value is the result of invoking the transform function
    ///     <paramref name="intermediateSelector"/> on the value of this result and then mapping the values of that result and
    ///     the source result to the final result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="intermediateSelector"/> is <see langword="null"/>, or if
    ///     <paramref name="returnSelector"/> is <see langword="null"/>.</exception>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public Maybe<TReturn> SelectMany<TIntermediate, TReturn>(
        Func<T, Maybe<TIntermediate>> intermediateSelector,
        Func<T, TIntermediate, TReturn> returnSelector)
    {
        if (intermediateSelector is null) throw new ArgumentNullException(nameof(intermediateSelector));
        if (returnSelector is null) throw new ArgumentNullException(nameof(returnSelector));

        return SelectMany(
            sourceValue => intermediateSelector(sourceValue).Select(
                intermediateValue => returnSelector(sourceValue, intermediateValue)));
    }

    /// <summary>
    /// Projects the value of a result to an intermediate result and invokes a result selector function on the values of the
    /// source and intermediate results.
    /// </summary>
    /// <typeparam name="TIntermediate">The type of the intermediate result, as collected by
    ///     <paramref name="intermediateSelector"/>.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="intermediateSelector">A transform function to apply to the value of the input result.</param>
    /// <param name="returnSelector">A transform function to apply to the value of the intermediate result.</param>
    /// <returns>A <see cref="Result{T}"/> whose value is the result of invoking the transform function
    ///     <paramref name="intermediateSelector"/> on the value of this result and then mapping the values of that result and
    ///     the source result to the final result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="intermediateSelector"/> is <see langword="null"/>, or if
    ///     <paramref name="returnSelector"/> is <see langword="null"/>.</exception>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public Task<Result<TReturn>> SelectMany<TIntermediate, TReturn>(
        Func<T, Task<Result<TIntermediate>>> intermediateSelector,
        Func<T, TIntermediate, TReturn> returnSelector)
    {
        if (intermediateSelector is null) throw new ArgumentNullException(nameof(intermediateSelector));
        if (returnSelector is null) throw new ArgumentNullException(nameof(returnSelector));

        return SelectMany(
            sourceValue => intermediateSelector(sourceValue).Select(
                intermediateValue => returnSelector(sourceValue, intermediateValue)));
    }

    /// <summary>
    /// Projects the value of a result to an intermediate result and invokes a result selector function on the values of the
    /// source and intermediate results.
    /// </summary>
    /// <typeparam name="TIntermediate">The type of the intermediate result, as collected by
    ///     <paramref name="intermediateSelector"/>.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="intermediateSelector">A transform function to apply to the value of the input result.</param>
    /// <param name="returnSelector">A transform function to apply to the value of the intermediate result.</param>
    /// <returns>A <see cref="Maybe{T}"/> whose value is the result of invoking the transform function
    ///     <paramref name="intermediateSelector"/> on the value of this result and then mapping the values of that result and
    ///     the source result to the final result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="intermediateSelector"/> is <see langword="null"/>, or if
    ///     <paramref name="returnSelector"/> is <see langword="null"/>.</exception>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public Task<Maybe<TReturn>> SelectMany<TIntermediate, TReturn>(
        Func<T, Task<Maybe<TIntermediate>>> intermediateSelector,
        Func<T, TIntermediate, TReturn> returnSelector)
    {
        if (intermediateSelector is null) throw new ArgumentNullException(nameof(intermediateSelector));
        if (returnSelector is null) throw new ArgumentNullException(nameof(returnSelector));

        return SelectMany(
            sourceValue => intermediateSelector(sourceValue).Select(
                intermediateValue => returnSelector(sourceValue, intermediateValue)));
    }
}

/// <content> Defines the <c>SelectMany</c> methods. </content>
public partial struct Maybe<T>
{
    /// <summary>
    /// Projects the result into a new <see cref="Maybe{T}"/> form: a <c>Success</c> result is transformed to the new form by
    /// passing its value to the <paramref name="onSuccessSelector"/> function; a <c>Fail</c> result is transformed to the new
    /// form as a <c>Fail</c> result with the same error; a <c>None</c> result is transformed to the new form as a <c>None</c>
    /// result.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Select</c> and <c>SelectMany</c> is in the return value of their <c>onSuccessSelector</c>
    /// function. The selector for <c>Select</c> returns a regular (non-result) value, which is the value of the returned
    /// <c>Success</c> result. The selector for <c>SelectMany</c> returns a result value, which is itself the returned result
    /// (and may or may not be <c>Success</c>).
    /// </remarks>
    /// <typeparam name="TReturn">The type of the <see cref="Maybe{T}"/> returned by <paramref name="onSuccessSelector"/>.
    ///     </typeparam>
    /// <param name="onSuccessSelector">A transform funtion to apply to the value of a <c>Success</c> result.</param>
    /// <param name="onNoneSelector">An optional transform function to apply to a <c>None</c> result.</param>
    /// <returns>The projected result.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public Maybe<TReturn> SelectMany<TReturn>(
        Func<T, Maybe<TReturn>> onSuccessSelector,
        Func<Maybe<TReturn>>? onNoneSelector = null)
    {
        if (onSuccessSelector is null) throw new ArgumentNullException(nameof(onSuccessSelector));

        return _outcome switch
        {
            Outcome.Success => onSuccessSelector(_value!),
            Outcome.None => onNoneSelector is null ? Maybe<TReturn>.None : onNoneSelector.Invoke(),
            _ => Maybe<TReturn>.Fail(GetError(), false),
        };
    }

    /// <summary>
    /// Projects the result into a new <see cref="Maybe{T}"/> form: a <c>Success</c> result is transformed to the new form by
    /// passing its value to the <paramref name="onSuccessSelector"/> function; a <c>Fail</c> result is transformed to the new
    /// form as a <c>Fail</c> result with the same error; a <c>None</c> result is transformed to the new form as a <c>None</c>
    /// result.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Select</c> and <c>SelectMany</c> is in the return value of their <c>onSuccessSelector</c>
    /// function. The selector for <c>Select</c> returns a regular (non-result) value, which is the value of the returned
    /// <c>Success</c> result. The selector for <c>SelectMany</c> returns a result value, which is itself the returned result
    /// (and may or may not be <c>Success</c>).
    /// </remarks>
    /// <typeparam name="TReturn">The type of the <see cref="Maybe{T}"/> returned by <paramref name="onSuccessSelector"/>.
    ///     </typeparam>
    /// <param name="onSuccessSelector">A transform funtion to apply to the value of a <c>Success</c> result.</param>
    /// <param name="onNoneSelector">An optional transform function to apply to a <c>None</c> result.</param>
    /// <returns>The projected result.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public async Task<Maybe<TReturn>> SelectMany<TReturn>(
        Func<T, Task<Maybe<TReturn>>> onSuccessSelector,
        Func<Task<Maybe<TReturn>>>? onNoneSelector = null)
    {
        if (onSuccessSelector is null) throw new ArgumentNullException(nameof(onSuccessSelector));

        return _outcome switch
        {
            Outcome.Success => await onSuccessSelector(_value!).ConfigureAwait(false),
            Outcome.None => onNoneSelector is null ? Maybe<TReturn>.None : await onNoneSelector().ConfigureAwait(false),
            _ => Maybe<TReturn>.Fail(GetError(), false),
        };
    }

    /// <summary>
    /// Projects the result into a new <see cref="Result"/> form: a <c>Success</c> result is transformed to the new form by
    /// passing its value to the <paramref name="onSuccessSelector"/> function; a <c>Fail</c> result is transformed to the new
    /// form as a <c>Fail</c> result with the same error; a <c>None</c> result is transformed to the new form as a <c>Fail</c>
    /// result with error code <see cref="ErrorCodes.NoneResult"/>.
    /// </summary>
    /// <param name="onSuccessSelector">A transform funtion to apply to the value of a <c>Success</c> result.</param>
    /// <param name="onNoneSelector">An optional transform function to apply to a <c>None</c> result.</param>
    /// <returns>The projected result.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public Result SelectMany(
        Func<T, Result> onSuccessSelector,
        Func<Result>? onNoneSelector = null)
    {
        if (onSuccessSelector is null) throw new ArgumentNullException(nameof(onSuccessSelector));

        return _outcome switch
        {
            Outcome.Success => onSuccessSelector(_value!),
            Outcome.None => onNoneSelector is null ? Result.Fail(Errors.NoneResult(), false) : onNoneSelector.Invoke(),
            _ => Result.Fail(GetError(), false),
        };
    }

    /// <summary>
    /// Projects the result into a new <see cref="Result"/> form: a <c>Success</c> result is transformed to the new form by
    /// passing its value to the <paramref name="onSuccessSelector"/> function; a <c>Fail</c> result is transformed to the new
    /// form as a <c>Fail</c> result with the same error; a <c>None</c> result is transformed to the new form as a <c>Fail</c>
    /// result with error code <see cref="ErrorCodes.NoneResult"/>.
    /// </summary>
    /// <param name="onSuccessSelector">A transform funtion to apply to the value of a <c>Success</c> result.</param>
    /// <param name="onNoneSelector">An optional transform function to apply to a <c>None</c> result.</param>
    /// <returns>The projected result.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public async Task<Result> SelectMany(
        Func<T, Task<Result>> onSuccessSelector,
        Func<Task<Result>>? onNoneSelector = null)
    {
        if (onSuccessSelector is null) throw new ArgumentNullException(nameof(onSuccessSelector));

        return _outcome switch
        {
            Outcome.Success => await onSuccessSelector(_value!).ConfigureAwait(false),
            Outcome.None => onNoneSelector is null ? Result.Fail(Errors.NoneResult(), false) : await onNoneSelector().ConfigureAwait(false),
            _ => Result.Fail(GetError(), false),
        };
    }

    /// <summary>
    /// Projects the result into a new <see cref="Result{T}"/> form: a <c>Success</c> result is transformed to the new form by
    /// passing its value to the <paramref name="onSuccessSelector"/> function; a <c>Fail</c> result is transformed to the new
    /// form as a <c>Fail</c> result with the same error; a <c>None</c> result is transformed to the new form as a <c>Fail</c>
    /// result with error code <see cref="ErrorCodes.NoneResult"/>.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Select</c> and <c>SelectMany</c> is in the return value of their <c>onSuccessSelector</c>
    /// function. The selector for <c>Select</c> returns a regular (non-result) value, which is the value of the returned
    /// <c>Success</c> result. The selector for <c>SelectMany</c> returns a result value, which is itself the returned result
    /// (and may or may not be <c>Success</c>).
    /// </remarks>
    /// <typeparam name="TReturn">The type of the <see cref="Result{T}"/> returned by <paramref name="onSuccessSelector"/>.
    ///     </typeparam>
    /// <param name="onSuccessSelector">A transform funtion to apply to the value of a <c>Success</c> result.</param>
    /// <param name="onNoneSelector">An optional transform function to apply to a <c>None</c> result.</param>
    /// <returns>The projected result.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public Result<TReturn> SelectMany<TReturn>(
        Func<T, Result<TReturn>> onSuccessSelector,
        Func<Result<TReturn>>? onNoneSelector = null)
    {
        if (onSuccessSelector is null) throw new ArgumentNullException(nameof(onSuccessSelector));

        return _outcome switch
        {
            Outcome.Success => onSuccessSelector(_value!),
            Outcome.None => onNoneSelector is null ? Result<TReturn>.Fail(Errors.NoneResult(), false) : onNoneSelector.Invoke(),
            _ => Result<TReturn>.Fail(GetError(), false),
        };
    }

    /// <summary>
    /// Projects the result into a new <see cref="Result{T}"/> form: a <c>Success</c> result is transformed to the new form by
    /// passing its value to the <paramref name="onSuccessSelector"/> function; a <c>Fail</c> result is transformed to the new
    /// form as a <c>Fail</c> result with the same error; a <c>None</c> result is transformed to the new form as a <c>Fail</c>
    /// result with error code <see cref="ErrorCodes.NoneResult"/>.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Select</c> and <c>SelectMany</c> is in the return value of their <c>onSuccessSelector</c>
    /// function. The selector for <c>Select</c> returns a regular (non-result) value, which is the value of the returned
    /// <c>Success</c> result. The selector for <c>SelectMany</c> returns a result value, which is itself the returned result
    /// (and may or may not be <c>Success</c>).
    /// </remarks>
    /// <typeparam name="TReturn">The type of the <see cref="Result{T}"/> returned by <paramref name="onSuccessSelector"/>.
    ///     </typeparam>
    /// <param name="onSuccessSelector">A transform funtion to apply to the value of a <c>Success</c> result.</param>
    /// <param name="onNoneSelector">An optional transform function to apply to a <c>None</c> result.</param>
    /// <returns>The projected result.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public async Task<Result<TReturn>> SelectMany<TReturn>(
        Func<T, Task<Result<TReturn>>> onSuccessSelector,
        Func<Task<Result<TReturn>>>? onNoneSelector = null)
    {
        if (onSuccessSelector is null) throw new ArgumentNullException(nameof(onSuccessSelector));

        return _outcome switch
        {
            Outcome.Success => await onSuccessSelector(_value!).ConfigureAwait(false),
            Outcome.None => onNoneSelector is null ? Result<TReturn>.Fail(Errors.NoneResult(), false) : await onNoneSelector().ConfigureAwait(false),
            _ => Result<TReturn>.Fail(GetError(), false),
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
    /// <param name="returnSelector">A transform function to apply to the value of the intermediate result.</param>
    /// <returns>A <see cref="Result{T}"/> whose value is the result of invoking the transform function
    ///     <paramref name="intermediateSelector"/> on the value of this result and then mapping the values of that result and
    ///     the source result to the final result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="intermediateSelector"/> is <see langword="null"/>, or if
    ///     <paramref name="returnSelector"/> is <see langword="null"/>.</exception>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public Result<TReturn> SelectMany<TIntermediate, TReturn>(
        Func<T, Result<TIntermediate>> intermediateSelector,
        Func<T, TIntermediate, TReturn> returnSelector)
    {
        if (intermediateSelector is null) throw new ArgumentNullException(nameof(intermediateSelector));
        if (returnSelector is null) throw new ArgumentNullException(nameof(returnSelector));

        return SelectMany(
            sourceValue => intermediateSelector(sourceValue).Select(
                intermediateValue => returnSelector(sourceValue, intermediateValue)));
    }

    /// <summary>
    /// Projects the value of a result to an intermediate result and invokes a result selector function on the values of the
    /// source and intermediate results.
    /// </summary>
    /// <typeparam name="TIntermediate">The type of the intermediate result collected by <paramref name="intermediateSelector"/>.
    ///     </typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="intermediateSelector">A transform function to apply to the value of the input result.</param>
    /// <param name="returnSelector">A transform function to apply to the value of the intermediate result.</param>
    /// <returns>A <see cref="Maybe{T}"/> whose value is the result of invoking the transform function
    ///     <paramref name="intermediateSelector"/> on the value of this result and then mapping the values of that result and
    ///     the source result to the final result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="intermediateSelector"/> is <see langword="null"/>, or if
    ///     <paramref name="returnSelector"/> is <see langword="null"/>.</exception>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public Maybe<TReturn> SelectMany<TIntermediate, TReturn>(
        Func<T, Maybe<TIntermediate>> intermediateSelector,
        Func<T, TIntermediate, TReturn> returnSelector)
    {
        if (intermediateSelector is null) throw new ArgumentNullException(nameof(intermediateSelector));
        if (returnSelector is null) throw new ArgumentNullException(nameof(returnSelector));

        return SelectMany(
            sourceValue => intermediateSelector(sourceValue).Select(
                intermediateValue => returnSelector(sourceValue, intermediateValue)));
    }

    /// <summary>
    /// Projects the value of a result to an intermediate result and invokes a result selector function on the values of the
    /// source and intermediate results.
    /// </summary>
    /// <typeparam name="TIntermediate">The type of the intermediate result collected by <paramref name="intermediateSelector"/>.
    ///     </typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="intermediateSelector">A transform function to apply to the value of the input result.</param>
    /// <param name="returnSelector">A transform function to apply to the value of the intermediate result.</param>
    /// <returns>A <see cref="Result{T}"/> whose value is the result of invoking the transform function
    ///     <paramref name="intermediateSelector"/> on the value of this result and then mapping the values of that result and
    ///     the source result to the final result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="intermediateSelector"/> is <see langword="null"/>, or if
    ///     <paramref name="returnSelector"/> is <see langword="null"/>.</exception>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public Task<Result<TReturn>> SelectMany<TIntermediate, TReturn>(
        Func<T, Task<Result<TIntermediate>>> intermediateSelector,
        Func<T, TIntermediate, TReturn> returnSelector)
    {
        if (intermediateSelector is null) throw new ArgumentNullException(nameof(intermediateSelector));
        if (returnSelector is null) throw new ArgumentNullException(nameof(returnSelector));

        return SelectMany(
            sourceValue => intermediateSelector(sourceValue).Select(
                intermediateValue => returnSelector(sourceValue, intermediateValue)));
    }

    /// <summary>
    /// Projects the value of a result to an intermediate result and invokes a result selector function on the values of the
    /// source and intermediate results.
    /// </summary>
    /// <typeparam name="TIntermediate">The type of the intermediate result collected by <paramref name="intermediateSelector"/>.
    ///     </typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="intermediateSelector">A transform function to apply to the value of the input result.</param>
    /// <param name="returnSelector">A transform function to apply to the value of the intermediate result.</param>
    /// <returns>A <see cref="Maybe{T}"/> whose value is the result of invoking the transform function
    ///     <paramref name="intermediateSelector"/> on the value of this result and then mapping the values of that result and
    ///     the source result to the final result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="intermediateSelector"/> is <see langword="null"/>, or if
    ///     <paramref name="returnSelector"/> is <see langword="null"/>.</exception>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public Task<Maybe<TReturn>> SelectMany<TIntermediate, TReturn>(
        Func<T, Task<Maybe<TIntermediate>>> intermediateSelector,
        Func<T, TIntermediate, TReturn> returnSelector)
    {
        if (intermediateSelector is null) throw new ArgumentNullException(nameof(intermediateSelector));
        if (returnSelector is null) throw new ArgumentNullException(nameof(returnSelector));

        return SelectMany(
            sourceValue => intermediateSelector(sourceValue).Select(
                intermediateValue => returnSelector(sourceValue, intermediateValue)));
    }
}

/// <content> Defines the <c>SelectMany</c> methods. </content>
public static partial class ResultExtensions
{
    /// <summary>
    /// Projects the result into a new <see cref="Result"/> form: a <c>Success</c> result is transformed to the new form by
    /// invoking the <paramref name="onSuccessSelector"/> function; a <c>Fail</c> result is transformed to the new form as a
    /// <c>Fail</c> result with the same error.
    /// </summary>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onSuccessSelector">A transform funtion to apply to the value of a <c>Success</c> result.</param>
    /// <returns>The projected result.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public static async Task<Result> SelectMany(
        this Task<Result> sourceResult,
        Func<Result> onSuccessSelector) =>
        (await sourceResult.ConfigureAwait(false)).SelectMany(onSuccessSelector);

    /// <summary>
    /// Projects the result into a new <see cref="Result"/> form: a <c>Success</c> result is transformed to the new form by
    /// invoking the <paramref name="onSuccessSelector"/> function; a <c>Fail</c> result is transformed to the new form as a
    /// <c>Fail</c> result with the same error.
    /// </summary>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onSuccessSelector">A transform funtion to apply to the value of a <c>Success</c> result.</param>
    /// <returns>The projected result.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public static async Task<Result> SelectMany(
        this Task<Result> sourceResult,
        Func<Task<Result>> onSuccessSelector) =>
        await (await sourceResult.ConfigureAwait(false)).SelectMany(onSuccessSelector).ConfigureAwait(false);

    /// <summary>
    /// Projects the result into a new <see cref="Result{T}"/> form: a <c>Success</c> result is transformed to the new form by
    /// invoking the <paramref name="onSuccessSelector"/> function; a <c>Fail</c> result is transformed to the new form as a
    /// <c>Fail</c> result with the same error.
    /// </summary>
    /// <typeparam name="TReturn">The type of the <see cref="Result{T}"/> returned by <paramref name="onSuccessSelector"/>.
    ///     </typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onSuccessSelector">A transform funtion to apply to the value of a <c>Success</c> result.</param>
    /// <returns>The projected result.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public static async Task<Result<TReturn>> SelectMany<TReturn>(
        this Task<Result> sourceResult,
        Func<Result<TReturn>> onSuccessSelector) =>
        (await sourceResult.ConfigureAwait(false)).SelectMany(onSuccessSelector);

    /// <summary>
    /// Projects the result into a new <see cref="Result{T}"/> form: a <c>Success</c> result is transformed to the new form by
    /// invoking the <paramref name="onSuccessSelector"/> function; a <c>Fail</c> result is transformed to the new form as a
    /// <c>Fail</c> result with the same error.
    /// </summary>
    /// <typeparam name="TReturn">The type of the <see cref="Result{T}"/> returned by <paramref name="onSuccessSelector"/>.
    ///     </typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onSuccessSelector">A transform funtion to apply to the value of a <c>Success</c> result.</param>
    /// <returns>The projected result.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public static async Task<Result<TReturn>> SelectMany<TReturn>(
        this Task<Result> sourceResult,
        Func<Task<Result<TReturn>>> onSuccessSelector) =>
        await (await sourceResult.ConfigureAwait(false)).SelectMany(onSuccessSelector).ConfigureAwait(false);

    /// <summary>
    /// Projects the result into a new <see cref="Maybe{T}"/> form: a <c>Success</c> result is transformed to the new form by
    /// invoking the <paramref name="onSuccessSelector"/> function; a <c>Fail</c> result is transformed to the new form as a
    /// <c>Fail</c> result with the same error.
    /// </summary>
    /// <typeparam name="TReturn">The type of the <see cref="Maybe{T}"/> returned by <paramref name="onSuccessSelector"/>.
    ///     </typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onSuccessSelector">A transform funtion to apply to the value of a <c>Success</c> result.</param>
    /// <returns>The projected result.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public static async Task<Maybe<TReturn>> SelectMany<TReturn>(
        this Task<Result> sourceResult,
        Func<Maybe<TReturn>> onSuccessSelector) =>
        (await sourceResult.ConfigureAwait(false)).SelectMany(onSuccessSelector);

    /// <summary>
    /// Projects the result into a new <see cref="Maybe{T}"/> form: a <c>Success</c> result is transformed to the new form by
    /// invoking the <paramref name="onSuccessSelector"/> function; a <c>Fail</c> result is transformed to the new form as a
    /// <c>Fail</c> result with the same error.
    /// </summary>
    /// <typeparam name="TReturn">The type of the <see cref="Maybe{T}"/> returned by <paramref name="onSuccessSelector"/>.
    ///     </typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onSuccessSelector">A transform funtion to apply to the value of a <c>Success</c> result.</param>
    /// <returns>The projected result.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public static async Task<Maybe<TReturn>> SelectMany<TReturn>(
        this Task<Result> sourceResult,
        Func<Task<Maybe<TReturn>>> onSuccessSelector) =>
        await (await sourceResult.ConfigureAwait(false)).SelectMany(onSuccessSelector).ConfigureAwait(false);

    /// <summary>
    /// Projects the result into a new <see cref="Result"/> form: a <c>Success</c> result is transformed to the new form by
    /// passing its value to the <paramref name="onSuccessSelector"/> function; a <c>Fail</c> result is transformed to the new
    /// form as a <c>Fail</c> result with the same error.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onSuccessSelector">A transform funtion to apply to the value of a <c>Success</c> result.</param>
    /// <returns>The projected result.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public static async Task<Result> SelectMany<T>(
        this Task<Result<T>> sourceResult,
        Func<T, Result> onSuccessSelector) =>
        (await sourceResult.ConfigureAwait(false)).SelectMany(onSuccessSelector);

    /// <summary>
    /// Projects the result into a new <see cref="Result"/> form: a <c>Success</c> result is transformed to the new form by
    /// passing its value to the <paramref name="onSuccessSelector"/> function; a <c>Fail</c> result is transformed to the new
    /// form as a <c>Fail</c> result with the same error.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onSuccessSelector">A transform funtion to apply to the value of a <c>Success</c> result.</param>
    /// <returns>The projected result.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public static async Task<Result> SelectMany<T>(
        this Task<Result<T>> sourceResult,
        Func<T, Task<Result>> onSuccessSelector) =>
        await (await sourceResult.ConfigureAwait(false)).SelectMany(onSuccessSelector).ConfigureAwait(false);

    /// <summary>
    /// Projects the result into a new <see cref="Result{T}"/> form: a <c>Success</c> result is transformed to the new form by
    /// passing its value to the <paramref name="onSuccessSelector"/> function; a <c>Fail</c> result is transformed to the new
    /// form as a <c>Fail</c> result with the same error.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Select</c> and <c>SelectMany</c> is in the return value of their <c>onSuccessSelector</c>
    /// function. The selector for <c>Select</c> returns a regular (non-result) value, which is the value of the returned
    /// <c>Success</c> result. The selector for <c>SelectMany</c> returns a result value, which is itself the returned result
    /// (and may or may not be <c>Success</c>).
    /// </remarks>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the <see cref="Result{T}"/> returned by <paramref name="onSuccessSelector"/>.
    ///     </typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onSuccessSelector">A transform funtion to apply to the value of a <c>Success</c> result.</param>
    /// <returns>The projected result.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public static async Task<Result<TReturn>> SelectMany<T, TReturn>(
        this Task<Result<T>> sourceResult,
        Func<T, Result<TReturn>> onSuccessSelector) =>
        (await sourceResult.ConfigureAwait(false)).SelectMany(onSuccessSelector);

    /// <summary>
    /// Projects the result into a new <see cref="Result{T}"/> form: a <c>Success</c> result is transformed to the new form by
    /// passing its value to the <paramref name="onSuccessSelector"/> function; a <c>Fail</c> result is transformed to the new
    /// form as a <c>Fail</c> result with the same error.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Select</c> and <c>SelectMany</c> is in the return value of their <c>onSuccessSelector</c>
    /// function. The selector for <c>Select</c> returns a regular (non-result) value, which is the value of the returned
    /// <c>Success</c> result. The selector for <c>SelectMany</c> returns a result value, which is itself the returned result
    /// (and may or may not be <c>Success</c>).
    /// </remarks>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the <see cref="Result{T}"/> returned by <paramref name="onSuccessSelector"/>.
    ///     </typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onSuccessSelector">A transform funtion to apply to the value of a <c>Success</c> result.</param>
    /// <returns>The projected result.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public static async Task<Result<TReturn>> SelectMany<T, TReturn>(
        this Task<Result<T>> sourceResult,
        Func<T, Task<Result<TReturn>>> onSuccessSelector) =>
        await (await sourceResult.ConfigureAwait(false)).SelectMany(onSuccessSelector).ConfigureAwait(false);

    /// <summary>
    /// Projects the result into a new <see cref="Maybe{T}"/> form: a <c>Success</c> result is transformed to the new form by
    /// passing its value to the <paramref name="onSuccessSelector"/> function; a <c>Fail</c> result is transformed to the new
    /// form as a <c>Fail</c> result with the same error.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Select</c> and <c>SelectMany</c> is in the return value of their <c>onSuccessSelector</c>
    /// function. The selector for <c>Select</c> returns a regular (non-result) value, which is the value of the returned
    /// <c>Success</c> result. The selector for <c>SelectMany</c> returns a result value, which is itself the returned result
    /// (and may or may not be <c>Success</c>).
    /// </remarks>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the <see cref="Maybe{T}"/> returned by <paramref name="onSuccessSelector"/>.
    ///     </typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onSuccessSelector">A transform funtion to apply to the value of a <c>Success</c> result.</param>
    /// <returns>The projected result.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public static async Task<Maybe<TReturn>> SelectMany<T, TReturn>(
        this Task<Result<T>> sourceResult,
        Func<T, Maybe<TReturn>> onSuccessSelector) =>
        (await sourceResult.ConfigureAwait(false)).SelectMany(onSuccessSelector);

    /// <summary>
    /// Projects the result into a new <see cref="Maybe{T}"/> form: a <c>Success</c> result is transformed to the new form by
    /// passing its value to the <paramref name="onSuccessSelector"/> function; a <c>Fail</c> result is transformed to the new
    /// form as a <c>Fail</c> result with the same error.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Select</c> and <c>SelectMany</c> is in the return value of their <c>onSuccessSelector</c>
    /// function. The selector for <c>Select</c> returns a regular (non-result) value, which is the value of the returned
    /// <c>Success</c> result. The selector for <c>SelectMany</c> returns a result value, which is itself the returned result
    /// (and may or may not be <c>Success</c>).
    /// </remarks>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the <see cref="Maybe{T}"/> returned by <paramref name="onSuccessSelector"/>.
    ///     </typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onSuccessSelector">A transform funtion to apply to the value of a <c>Success</c> result.</param>
    /// <returns>The projected result.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public static async Task<Maybe<TReturn>> SelectMany<T, TReturn>(
        this Task<Result<T>> sourceResult,
        Func<T, Task<Maybe<TReturn>>> onSuccessSelector) =>
        await (await sourceResult.ConfigureAwait(false)).SelectMany(onSuccessSelector).ConfigureAwait(false);

    /// <summary>
    /// Projects the result into a new <see cref="Result"/> form: a <c>Success</c> result is transformed to the new form by
    /// passing its value to the <paramref name="onSuccessSelector"/> function; a <c>Fail</c> result is transformed to the new
    /// form as a <c>Fail</c> result with the same error; a <c>None</c> result is transformed to the new form as a <c>Fail</c>
    /// result with error code <see cref="ErrorCodes.NoneResult"/>.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onSuccessSelector">A transform funtion to apply to the value of a <c>Success</c> result.</param>
    /// <param name="onNoneSelector">An optional transform function to apply to a <c>None</c> result.</param>
    /// <returns>The projected result.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public static async Task<Result> SelectMany<T>(
        this Task<Maybe<T>> sourceResult,
        Func<T, Result> onSuccessSelector,
        Func<Result>? onNoneSelector = null) =>
        (await sourceResult.ConfigureAwait(false)).SelectMany(onSuccessSelector, onNoneSelector);

    /// <summary>
    /// Projects the result into a new <see cref="Result"/> form: a <c>Success</c> result is transformed to the new form by
    /// passing its value to the <paramref name="onSuccessSelector"/> function; a <c>Fail</c> result is transformed to the new
    /// form as a <c>Fail</c> result with the same error; a <c>None</c> result is transformed to the new form as a <c>Fail</c>
    /// result with error code <see cref="ErrorCodes.NoneResult"/>.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onSuccessSelector">A transform funtion to apply to the value of a <c>Success</c> result.</param>
    /// <param name="onNoneSelector">An optional transform function to apply to a <c>None</c> result.</param>
    /// <returns>The projected result.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public static async Task<Result> SelectMany<T>(
        this Task<Maybe<T>> sourceResult,
        Func<T, Task<Result>> onSuccessSelector,
        Func<Task<Result>>? onNoneSelector = null) =>
        await (await sourceResult.ConfigureAwait(false)).SelectMany(onSuccessSelector, onNoneSelector).ConfigureAwait(false);

    /// <summary>
    /// Projects the result into a new <see cref="Result{T}"/> form: a <c>Success</c> result is transformed to the new form by
    /// passing its value to the <paramref name="onSuccessSelector"/> function; a <c>Fail</c> result is transformed to the new
    /// form as a <c>Fail</c> result with the same error; a <c>None</c> result is transformed to the new form as a <c>Fail</c>
    /// result with error code <see cref="ErrorCodes.NoneResult"/>.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Select</c> and <c>SelectMany</c> is in the return value of their <c>onSuccessSelector</c>
    /// function. The selector for <c>Select</c> returns a regular (non-result) value, which is the value of the returned
    /// <c>Success</c> result. The selector for <c>SelectMany</c> returns a result value, which is itself the returned result
    /// (and may or may not be <c>Success</c>).
    /// </remarks>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the <see cref="Result{T}"/> returned by <paramref name="onSuccessSelector"/>.
    ///     </typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onSuccessSelector">A transform funtion to apply to the value of a <c>Success</c> result.</param>
    /// <param name="onNoneSelector">An optional transform function to apply to a <c>None</c> result.</param>
    /// <returns>The projected result.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public static async Task<Result<TReturn>> SelectMany<T, TReturn>(
        this Task<Maybe<T>> sourceResult,
        Func<T, Result<TReturn>> onSuccessSelector,
        Func<Result<TReturn>>? onNoneSelector = null) =>
        (await sourceResult.ConfigureAwait(false)).SelectMany(onSuccessSelector, onNoneSelector);

    /// <summary>
    /// Projects the result into a new <see cref="Result{T}"/> form: a <c>Success</c> result is transformed to the new form by
    /// passing its value to the <paramref name="onSuccessSelector"/> function; a <c>Fail</c> result is transformed to the new
    /// form as a <c>Fail</c> result with the same error; a <c>None</c> result is transformed to the new form as a <c>Fail</c>
    /// result with error code <see cref="ErrorCodes.NoneResult"/>.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Select</c> and <c>SelectMany</c> is in the return value of their <c>onSuccessSelector</c>
    /// function. The selector for <c>Select</c> returns a regular (non-result) value, which is the value of the returned
    /// <c>Success</c> result. The selector for <c>SelectMany</c> returns a result value, which is itself the returned result
    /// (and may or may not be <c>Success</c>).
    /// </remarks>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the <see cref="Result{T}"/> returned by <paramref name="onSuccessSelector"/>.
    ///     </typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onSuccessSelector">A transform funtion to apply to the value of a <c>Success</c> result.</param>
    /// <param name="onNoneSelector">An optional transform function to apply to a <c>None</c> result.</param>
    /// <returns>The projected result.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public static async Task<Result<TReturn>> SelectMany<T, TReturn>(
        this Task<Maybe<T>> sourceResult,
        Func<T, Task<Result<TReturn>>> onSuccessSelector,
        Func<Task<Result<TReturn>>>? onNoneSelector = null) =>
        await (await sourceResult.ConfigureAwait(false)).SelectMany(onSuccessSelector, onNoneSelector).ConfigureAwait(false);

    /// <summary>
    /// Projects the result into a new <see cref="Maybe{T}"/> form: a <c>Success</c> result is transformed to the new form by
    /// passing its value to the <paramref name="onSuccessSelector"/> function; a <c>Fail</c> result is transformed to the new
    /// form as a <c>Fail</c> result with the same error; a <c>None</c> result is transformed to the new form as a <c>None</c>
    /// result.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Select</c> and <c>SelectMany</c> is in the return value of their <c>onSuccessSelector</c>
    /// function. The selector for <c>Select</c> returns a regular (non-result) value, which is the value of the returned
    /// <c>Success</c> result. The selector for <c>SelectMany</c> returns a result value, which is itself the returned result
    /// (and may or may not be <c>Success</c>).
    /// </remarks>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the <see cref="Maybe{T}"/> returned by <paramref name="onSuccessSelector"/>.
    ///     </typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onSuccessSelector">A transform funtion to apply to the value of a <c>Success</c> result.</param>
    /// <param name="onNoneSelector">An optional transform function to apply to a <c>None</c> result.</param>
    /// <returns>The projected result.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public static async Task<Maybe<TReturn>> SelectMany<T, TReturn>(
        this Task<Maybe<T>> sourceResult,
        Func<T, Maybe<TReturn>> onSuccessSelector,
        Func<Maybe<TReturn>>? onNoneSelector = null) =>
        (await sourceResult.ConfigureAwait(false)).SelectMany(onSuccessSelector, onNoneSelector);

    /// <summary>
    /// Projects the result into a new <see cref="Maybe{T}"/> form: a <c>Success</c> result is transformed to the new form by
    /// passing its value to the <paramref name="onSuccessSelector"/> function; a <c>Fail</c> result is transformed to the new
    /// form as a <c>Fail</c> result with the same error; a <c>None</c> result is transformed to the new form as a <c>None</c>
    /// result.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Select</c> and <c>SelectMany</c> is in the return value of their <c>onSuccessSelector</c>
    /// function. The selector for <c>Select</c> returns a regular (non-result) value, which is the value of the returned
    /// <c>Success</c> result. The selector for <c>SelectMany</c> returns a result value, which is itself the returned result
    /// (and may or may not be <c>Success</c>).
    /// </remarks>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the <see cref="Maybe{T}"/> returned by <paramref name="onSuccessSelector"/>.
    ///     </typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onSuccessSelector">A transform funtion to apply to the value of a <c>Success</c> result.</param>
    /// <param name="onNoneSelector">An optional transform function to apply to a <c>None</c> result.</param>
    /// <returns>The projected result.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public static async Task<Maybe<TReturn>> SelectMany<T, TReturn>(
        this Task<Maybe<T>> sourceResult,
        Func<T, Task<Maybe<TReturn>>> onSuccessSelector,
        Func<Task<Maybe<TReturn>>>? onNoneSelector = null) =>
        await (await sourceResult.ConfigureAwait(false)).SelectMany(onSuccessSelector, onNoneSelector).ConfigureAwait(false);

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
    /// <param name="returnSelector">A transform function to apply to the value of the intermediate result.</param>
    /// <returns>A <see cref="Result{T}"/> whose value is the result of invoking the transform function
    ///     <paramref name="intermediateSelector"/> on the value of <paramref name="sourceResult"/> and then mapping the values
    ///     of that result and the source result to the final result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="intermediateSelector"/> is <see langword="null"/>, or if
    ///     <paramref name="returnSelector"/> is <see langword="null"/>.</exception>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static Task<Result<TReturn>> SelectMany<T, TIntermediate, TReturn>(
        this Task<Result<T>> sourceResult,
        Func<T, Result<TIntermediate>> intermediateSelector,
        Func<T, TIntermediate, TReturn> returnSelector)
    {
        if (intermediateSelector is null) throw new ArgumentNullException(nameof(intermediateSelector));
        if (returnSelector is null) throw new ArgumentNullException(nameof(returnSelector));

        return sourceResult.SelectMany(
            sourceValue => intermediateSelector(sourceValue).Select(
                intermediateValue => returnSelector(sourceValue, intermediateValue)));
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
    /// <param name="returnSelector">A transform function to apply to the value of the intermediate result.</param>
    /// <returns>A <see cref="Maybe{T}"/> whose value is the result of invoking the transform function
    ///     <paramref name="intermediateSelector"/> on the value of <paramref name="sourceResult"/> and then mapping the values
    ///     of that result and the source result to the final result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="intermediateSelector"/> is <see langword="null"/>, or if
    ///     <paramref name="returnSelector"/> is <see langword="null"/>.</exception>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static Task<Maybe<TReturn>> SelectMany<T, TIntermediate, TReturn>(
        this Task<Result<T>> sourceResult,
        Func<T, Maybe<TIntermediate>> intermediateSelector,
        Func<T, TIntermediate, TReturn> returnSelector)
    {
        if (intermediateSelector is null) throw new ArgumentNullException(nameof(intermediateSelector));
        if (returnSelector is null) throw new ArgumentNullException(nameof(returnSelector));

        return sourceResult.SelectMany(
            sourceValue => intermediateSelector(sourceValue).Select(
                intermediateValue => returnSelector(sourceValue, intermediateValue)));
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
    /// <param name="returnSelector">A transform function to apply to the value of the intermediate result.</param>
    /// <returns>A <see cref="Result{T}"/> whose value is the result of invoking the transform function
    ///     <paramref name="intermediateSelector"/> on the value of <paramref name="sourceResult"/> and then mapping the values
    ///     of that result and the source result to the final result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="intermediateSelector"/> is <see langword="null"/>, or if
    ///     <paramref name="returnSelector"/> is <see langword="null"/>.</exception>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static Task<Result<TReturn>> SelectMany<T, TIntermediate, TReturn>(
        this Task<Result<T>> sourceResult,
        Func<T, Task<Result<TIntermediate>>> intermediateSelector,
        Func<T, TIntermediate, TReturn> returnSelector)
    {
        if (intermediateSelector is null) throw new ArgumentNullException(nameof(intermediateSelector));
        if (returnSelector is null) throw new ArgumentNullException(nameof(returnSelector));

        return sourceResult.SelectMany(
            sourceValue => intermediateSelector(sourceValue).Select(
                intermediateValue => returnSelector(sourceValue, intermediateValue)));
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
    /// <param name="returnSelector">A transform function to apply to the value of the intermediate result.</param>
    /// <returns>A <see cref="Maybe{T}"/> whose value is the result of invoking the transform function
    ///     <paramref name="intermediateSelector"/> on the value of <paramref name="sourceResult"/> and then mapping the values
    ///     of that result and the source result to the final result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="intermediateSelector"/> is <see langword="null"/>, or if
    ///     <paramref name="returnSelector"/> is <see langword="null"/>.</exception>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static Task<Maybe<TReturn>> SelectMany<T, TIntermediate, TReturn>(
        this Task<Result<T>> sourceResult,
        Func<T, Task<Maybe<TIntermediate>>> intermediateSelector,
        Func<T, TIntermediate, TReturn> returnSelector)
    {
        if (intermediateSelector is null) throw new ArgumentNullException(nameof(intermediateSelector));
        if (returnSelector is null) throw new ArgumentNullException(nameof(returnSelector));

        return sourceResult.SelectMany(
            sourceValue => intermediateSelector(sourceValue).Select(
                intermediateValue => returnSelector(sourceValue, intermediateValue)));
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
    /// <param name="returnSelector">A transform function to apply to the value of the intermediate result.</param>
    /// <returns>A <see cref="Result{T}"/> whose value is the result of invoking the transform function
    ///     <paramref name="intermediateSelector"/> on the value of <paramref name="sourceResult"/> and then mapping the values
    ///     of that result and the source result to the final result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="intermediateSelector"/> is <see langword="null"/>, or if
    ///     <paramref name="returnSelector"/> is <see langword="null"/>.</exception>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static Task<Result<TReturn>> SelectMany<T, TIntermediate, TReturn>(
        this Task<Maybe<T>> sourceResult,
        Func<T, Result<TIntermediate>> intermediateSelector,
        Func<T, TIntermediate, TReturn> returnSelector)
    {
        if (intermediateSelector is null) throw new ArgumentNullException(nameof(intermediateSelector));
        if (returnSelector is null) throw new ArgumentNullException(nameof(returnSelector));

        return sourceResult.SelectMany(
            sourceValue => intermediateSelector(sourceValue).Select(
                intermediateValue => returnSelector(sourceValue, intermediateValue)));
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
    /// <param name="returnSelector">A transform function to apply to the value of the intermediate result.</param>
    /// <returns>A <see cref="Maybe{T}"/> whose value is the result of invoking the transform function
    ///     <paramref name="intermediateSelector"/> on the value of <paramref name="sourceResult"/> and then mapping the values
    ///     of that result and the source result to the final result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="intermediateSelector"/> is <see langword="null"/>, or if
    ///     <paramref name="returnSelector"/> is <see langword="null"/>.</exception>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static Task<Maybe<TReturn>> SelectMany<T, TIntermediate, TReturn>(
        this Task<Maybe<T>> sourceResult,
        Func<T, Maybe<TIntermediate>> intermediateSelector,
        Func<T, TIntermediate, TReturn> returnSelector)
    {
        if (intermediateSelector is null) throw new ArgumentNullException(nameof(intermediateSelector));
        if (returnSelector is null) throw new ArgumentNullException(nameof(returnSelector));

        return sourceResult.SelectMany(
            sourceValue => intermediateSelector(sourceValue).Select(
                intermediateValue => returnSelector(sourceValue, intermediateValue)));
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
    /// <param name="returnSelector">A transform function to apply to the value of the intermediate result.</param>
    /// <returns>A <see cref="Result{T}"/> whose value is the result of invoking the transform function
    ///     <paramref name="intermediateSelector"/> on the value of <paramref name="sourceResult"/> and then mapping the values
    ///     of that result and the source result to the final result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="intermediateSelector"/> is <see langword="null"/>, or if
    ///     <paramref name="returnSelector"/> is <see langword="null"/>.</exception>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static Task<Result<TReturn>> SelectMany<T, TIntermediate, TReturn>(
        this Task<Maybe<T>> sourceResult,
        Func<T, Task<Result<TIntermediate>>> intermediateSelector,
        Func<T, TIntermediate, TReturn> returnSelector)
    {
        if (intermediateSelector is null) throw new ArgumentNullException(nameof(intermediateSelector));
        if (returnSelector is null) throw new ArgumentNullException(nameof(returnSelector));

        return sourceResult.SelectMany(
            sourceValue => intermediateSelector(sourceValue).Select(
                intermediateValue => returnSelector(sourceValue, intermediateValue)));
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
    /// <param name="returnSelector">A transform function to apply to the value of the intermediate result.</param>
    /// <returns>A <see cref="Maybe{T}"/> whose value is the result of invoking the transform function
    ///     <paramref name="intermediateSelector"/> on the value of <paramref name="sourceResult"/> and then mapping the values
    ///     of that result and the source result to the final result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="intermediateSelector"/> is <see langword="null"/>, or if
    ///     <paramref name="returnSelector"/> is <see langword="null"/>.</exception>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static Task<Maybe<TReturn>> SelectMany<T, TIntermediate, TReturn>(
        this Task<Maybe<T>> sourceResult,
        Func<T, Task<Maybe<TIntermediate>>> intermediateSelector,
        Func<T, TIntermediate, TReturn> returnSelector)
    {
        if (intermediateSelector is null) throw new ArgumentNullException(nameof(intermediateSelector));
        if (returnSelector is null) throw new ArgumentNullException(nameof(returnSelector));

        return sourceResult.SelectMany(
            sourceValue => intermediateSelector(sourceValue).Select(
                intermediateValue => returnSelector(sourceValue, intermediateValue)));
    }

    /// <summary>
    /// Projects the result into a new <see cref="Result"/> form: a <c>Success</c> result is transformed to the new form by
    /// invoking the <paramref name="onSuccessSelector"/> function; a <c>Fail</c> result is transformed to the new form as a
    /// <c>Fail</c> result with the same error.
    /// </summary>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onSuccessSelector">A transform funtion to apply to the value of a <c>Success</c> result.</param>
    /// <returns>The projected result.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static Result SelectMany(
        this IResult<DBNull> sourceResult,
        Func<DBNull, Result> onSuccessSelector)
    {
        if (sourceResult is null) throw new ArgumentNullException(nameof(sourceResult));
        if (onSuccessSelector is null) throw new ArgumentNullException(nameof(onSuccessSelector));

        if (sourceResult is Result result)
            return result.SelectMany(() => onSuccessSelector(DBNull.Value));

        if (sourceResult is Result<DBNull> resultOfDBNull)
            return resultOfDBNull.SelectMany(onSuccessSelector);

        if (sourceResult is Maybe<DBNull> maybeOfDBNull)
            return maybeOfDBNull.SelectMany(onSuccessSelector);

        if (sourceResult.IsSuccess)
            return onSuccessSelector(sourceResult.Value);

        return Result.Fail(sourceResult.GetNonSuccessError(), false);
    }

    /// <summary>
    /// Projects the result into a new <see cref="Result"/> form: a <c>Success</c> result is transformed to the new form by
    /// invoking the <paramref name="onSuccessSelector"/> function; a <c>Fail</c> result is transformed to the new form as a
    /// <c>Fail</c> result with the same error.
    /// </summary>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onSuccessSelector">A transform funtion to apply to the value of a <c>Success</c> result.</param>
    /// <returns>The projected result.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static async Task<Result> SelectMany(
        this IResult<DBNull> sourceResult,
        Func<DBNull, Task<Result>> onSuccessSelector)
    {
        if (sourceResult is null) throw new ArgumentNullException(nameof(sourceResult));
        if (onSuccessSelector is null) throw new ArgumentNullException(nameof(onSuccessSelector));

        if (sourceResult is Result result)
            return await result.SelectMany(() => onSuccessSelector(DBNull.Value)).ConfigureAwait(false);

        if (sourceResult is Result<DBNull> resultOfDBNull)
            return await resultOfDBNull.SelectMany(onSuccessSelector).ConfigureAwait(false);

        if (sourceResult is Maybe<DBNull> maybeOfDBNull)
            return await maybeOfDBNull.SelectMany(onSuccessSelector).ConfigureAwait(false);

        if (sourceResult.IsSuccess)
            return await onSuccessSelector(sourceResult.Value).ConfigureAwait(false);

        return Result.Fail(sourceResult.GetNonSuccessError(), false);
    }

    /// <summary>
    /// Projects the result into a new <see cref="Result"/> form: a <c>Success</c> result is transformed to the new form by
    /// invoking the <paramref name="onSuccessSelector"/> function; a <c>Fail</c> result is transformed to the new form as a
    /// <c>Fail</c> result with the same error.
    /// </summary>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onSuccessSelector">A transform funtion to apply to the value of a <c>Success</c> result.</param>
    /// <returns>The projected result.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static async Task<Result> SelectMany(
        this Task<IResult<DBNull>> sourceResult,
        Func<DBNull, Result> onSuccessSelector) =>
        (await sourceResult.ConfigureAwait(false)).SelectMany(onSuccessSelector);

    /// <summary>
    /// Projects the result into a new <see cref="Result"/> form: a <c>Success</c> result is transformed to the new form by
    /// invoking the <paramref name="onSuccessSelector"/> function; a <c>Fail</c> result is transformed to the new form as a
    /// <c>Fail</c> result with the same error.
    /// </summary>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onSuccessSelector">A transform funtion to apply to the value of a <c>Success</c> result.</param>
    /// <returns>The projected result.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static async Task<Result> SelectMany(
        this Task<IResult<DBNull>> sourceResult,
        Func<DBNull, Task<Result>> onSuccessSelector) =>
        await (await sourceResult.ConfigureAwait(false)).SelectMany(onSuccessSelector).ConfigureAwait(false);

    /// <summary>
    /// Projects the result into a new <see cref="Result{T}"/> form: a <c>Success</c> result is transformed to the new form by
    /// invoking the <paramref name="onSuccessSelector"/> function; a <c>Fail</c> result is transformed to the new form as a
    /// <c>Fail</c> result with the same error.
    /// </summary>
    /// <typeparam name="TReturn">The type of the <see cref="Result{T}"/> returned by <paramref name="onSuccessSelector"/>.
    ///     </typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onSuccessSelector">A transform funtion to apply to the value of a <c>Success</c> result.</param>
    /// <returns>The projected result.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static Result<TReturn> SelectMany<TReturn>(
        this IResult<DBNull> sourceResult,
        Func<DBNull, Result<TReturn>> onSuccessSelector)
    {
        if (sourceResult is null) throw new ArgumentNullException(nameof(sourceResult));
        if (onSuccessSelector is null) throw new ArgumentNullException(nameof(onSuccessSelector));

        if (sourceResult is Result result)
            return result.SelectMany(() => onSuccessSelector(DBNull.Value));

        if (sourceResult is Result<DBNull> resultOfDBNull)
            return resultOfDBNull.SelectMany(onSuccessSelector);

        if (sourceResult is Maybe<DBNull> maybeOfDBNull)
            return maybeOfDBNull.SelectMany(onSuccessSelector);

        if (sourceResult.IsSuccess)
            return onSuccessSelector(sourceResult.Value);

        return Result<TReturn>.Fail(sourceResult.GetNonSuccessError(), false);
    }

    /// <summary>
    /// Projects the result into a new <see cref="Result{T}"/> form: a <c>Success</c> result is transformed to the new form by
    /// invoking the <paramref name="onSuccessSelector"/> function; a <c>Fail</c> result is transformed to the new form as a
    /// <c>Fail</c> result with the same error.
    /// </summary>
    /// <typeparam name="TReturn">The type of the <see cref="Result{T}"/> returned by <paramref name="onSuccessSelector"/>.
    ///     </typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onSuccessSelector">A transform funtion to apply to the value of a <c>Success</c> result.</param>
    /// <returns>The projected result.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static async Task<Result<TReturn>> SelectMany<TReturn>(
        this IResult<DBNull> sourceResult,
        Func<DBNull, Task<Result<TReturn>>> onSuccessSelector)
    {
        if (sourceResult is null) throw new ArgumentNullException(nameof(sourceResult));
        if (onSuccessSelector is null) throw new ArgumentNullException(nameof(onSuccessSelector));

        if (sourceResult is Result result)
            return await result.SelectMany(() => onSuccessSelector(DBNull.Value)).ConfigureAwait(false);

        if (sourceResult is Result<DBNull> resultOfDBNull)
            return await resultOfDBNull.SelectMany(onSuccessSelector).ConfigureAwait(false);

        if (sourceResult is Maybe<DBNull> maybeOfDBNull)
            return await maybeOfDBNull.SelectMany(onSuccessSelector).ConfigureAwait(false);

        if (sourceResult.IsSuccess)
            return await onSuccessSelector(sourceResult.Value).ConfigureAwait(false);

        return Result<TReturn>.Fail(sourceResult.GetNonSuccessError(), false);
    }

    /// <summary>
    /// Projects the result into a new <see cref="Result{T}"/> form: a <c>Success</c> result is transformed to the new form by
    /// invoking the <paramref name="onSuccessSelector"/> function; a <c>Fail</c> result is transformed to the new form as a
    /// <c>Fail</c> result with the same error.
    /// </summary>
    /// <typeparam name="TReturn">The type of the <see cref="Result{T}"/> returned by <paramref name="onSuccessSelector"/>.
    ///     </typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onSuccessSelector">A transform funtion to apply to the value of a <c>Success</c> result.</param>
    /// <returns>The projected result.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static async Task<Result<TReturn>> SelectMany<TReturn>(
        this Task<IResult<DBNull>> sourceResult,
        Func<DBNull, Result<TReturn>> onSuccessSelector) =>
        (await sourceResult.ConfigureAwait(false)).SelectMany(onSuccessSelector);

    /// <summary>
    /// Projects the result into a new <see cref="Result{T}"/> form: a <c>Success</c> result is transformed to the new form by
    /// invoking the <paramref name="onSuccessSelector"/> function; a <c>Fail</c> result is transformed to the new form as a
    /// <c>Fail</c> result with the same error.
    /// </summary>
    /// <typeparam name="TReturn">The type of the <see cref="Result{T}"/> returned by <paramref name="onSuccessSelector"/>.
    ///     </typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onSuccessSelector">A transform funtion to apply to the value of a <c>Success</c> result.</param>
    /// <returns>The projected result.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static async Task<Result<TReturn>> SelectMany<TReturn>(
        this Task<IResult<DBNull>> sourceResult,
        Func<DBNull, Task<Result<TReturn>>> onSuccessSelector) =>
        await (await sourceResult.ConfigureAwait(false)).SelectMany(onSuccessSelector).ConfigureAwait(false);

    /// <summary>
    /// Projects the result into a new <see cref="Maybe{T}"/> form: a <c>Success</c> result is transformed to the new form by
    /// invoking the <paramref name="onSuccessSelector"/> function; a <c>Fail</c> result is transformed to the new form as a
    /// <c>Fail</c> result with the same error.
    /// </summary>
    /// <typeparam name="TReturn">The type of the <see cref="Maybe{T}"/> returned by <paramref name="onSuccessSelector"/>.
    ///     </typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onSuccessSelector">A transform funtion to apply to the value of a <c>Success</c> result.</param>
    /// <returns>The projected result.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static Maybe<TReturn> SelectMany<TReturn>(
        this IResult<DBNull> sourceResult,
        Func<DBNull, Maybe<TReturn>> onSuccessSelector)
    {
        if (sourceResult is null) throw new ArgumentNullException(nameof(sourceResult));
        if (onSuccessSelector is null) throw new ArgumentNullException(nameof(onSuccessSelector));

        if (sourceResult is Result result)
            return result.SelectMany(() => onSuccessSelector(DBNull.Value));

        if (sourceResult is Result<DBNull> resultOfDBNull)
            return resultOfDBNull.SelectMany(onSuccessSelector);

        if (sourceResult is Maybe<DBNull> maybeOfDBNull)
            return maybeOfDBNull.SelectMany(onSuccessSelector);

        if (sourceResult.IsSuccess)
            return onSuccessSelector(sourceResult.Value);

        var error = sourceResult.GetNonSuccessError();
        if (error.ErrorCode == ErrorCodes.NoneResult)
            return Maybe<TReturn>.None;

        return Maybe<TReturn>.Fail(error, false);
    }

    /// <summary>
    /// Projects the result into a new <see cref="Maybe{T}"/> form: a <c>Success</c> result is transformed to the new form by
    /// invoking the <paramref name="onSuccessSelector"/> function; a <c>Fail</c> result is transformed to the new form as a
    /// <c>Fail</c> result with the same error.
    /// </summary>
    /// <typeparam name="TReturn">The type of the <see cref="Maybe{T}"/> returned by <paramref name="onSuccessSelector"/>.
    ///     </typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onSuccessSelector">A transform funtion to apply to the value of a <c>Success</c> result.</param>
    /// <returns>The projected result.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static async Task<Maybe<TReturn>> SelectMany<TReturn>(
        this IResult<DBNull> sourceResult,
        Func<DBNull, Task<Maybe<TReturn>>> onSuccessSelector)
    {
        if (sourceResult is null) throw new ArgumentNullException(nameof(sourceResult));
        if (onSuccessSelector is null) throw new ArgumentNullException(nameof(onSuccessSelector));

        if (sourceResult is Result result)
            return await result.SelectMany(() => onSuccessSelector(DBNull.Value)).ConfigureAwait(false);

        if (sourceResult is Result<DBNull> resultOfDBNull)
            return await resultOfDBNull.SelectMany(onSuccessSelector).ConfigureAwait(false);

        if (sourceResult is Maybe<DBNull> maybeOfDBNull)
            return await maybeOfDBNull.SelectMany(onSuccessSelector).ConfigureAwait(false);

        if (sourceResult.IsSuccess)
            return await onSuccessSelector(sourceResult.Value).ConfigureAwait(false);

        var error = sourceResult.GetNonSuccessError();
        if (error.ErrorCode == ErrorCodes.NoneResult)
            return Maybe<TReturn>.None;

        return Maybe<TReturn>.Fail(error, false);
    }

    /// <summary>
    /// Projects the result into a new <see cref="Maybe{T}"/> form: a <c>Success</c> result is transformed to the new form by
    /// invoking the <paramref name="onSuccessSelector"/> function; a <c>Fail</c> result is transformed to the new form as a
    /// <c>Fail</c> result with the same error.
    /// </summary>
    /// <typeparam name="TReturn">The type of the <see cref="Maybe{T}"/> returned by <paramref name="onSuccessSelector"/>.
    ///     </typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onSuccessSelector">A transform funtion to apply to the value of a <c>Success</c> result.</param>
    /// <returns>The projected result.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static async Task<Maybe<TReturn>> SelectMany<TReturn>(
        this Task<IResult<DBNull>> sourceResult,
        Func<DBNull, Maybe<TReturn>> onSuccessSelector) =>
        (await sourceResult.ConfigureAwait(false)).SelectMany(onSuccessSelector);

    /// <summary>
    /// Projects the result into a new <see cref="Maybe{T}"/> form: a <c>Success</c> result is transformed to the new form by
    /// invoking the <paramref name="onSuccessSelector"/> function; a <c>Fail</c> result is transformed to the new form as a
    /// <c>Fail</c> result with the same error.
    /// </summary>
    /// <typeparam name="TReturn">The type of the <see cref="Maybe{T}"/> returned by <paramref name="onSuccessSelector"/>.
    ///     </typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="onSuccessSelector">A transform funtion to apply to the value of a <c>Success</c> result.</param>
    /// <returns>The projected result.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static async Task<Maybe<TReturn>> SelectMany<TReturn>(
        this Task<IResult<DBNull>> sourceResult,
        Func<DBNull, Task<Maybe<TReturn>>> onSuccessSelector) =>
        await (await sourceResult.ConfigureAwait(false)).SelectMany(onSuccessSelector).ConfigureAwait(false);

    /// <summary>
    /// Projects the value of a result to an intermediate result and invokes a result selector function on the values of the
    /// source and intermediate results.
    /// </summary>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="intermediateSelector">A transform function to apply to the value of the input result.</param>
    /// <param name="returnSelector">A transform function to apply to the value of the intermediate result.</param>
    /// <returns>A <see cref="Result{T}"/> whose value is the result of invoking the transform function
    ///     <paramref name="intermediateSelector"/> on the value of this result and then mapping the values of that result and
    ///     the source result to the final result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="intermediateSelector"/> is <see langword="null"/>, or if
    ///     <paramref name="returnSelector"/> is <see langword="null"/>.</exception>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static Result<TReturn> SelectMany<TReturn>(
        this IResult<DBNull> sourceResult,
        Func<DBNull, Result> intermediateSelector,
        Func<DBNull, DBNull, TReturn> returnSelector)
    {
        if (sourceResult is null) throw new ArgumentNullException(nameof(sourceResult));
        if (intermediateSelector is null) throw new ArgumentNullException(nameof(intermediateSelector));
        if (returnSelector is null) throw new ArgumentNullException(nameof(returnSelector));

        return sourceResult.SelectMany(
            sourceValue => intermediateSelector(sourceValue).Select(
                intermediateValue => returnSelector(sourceValue, intermediateValue)));
    }

    /// <summary>
    /// Projects the value of a result to an intermediate result and invokes a result selector function on the values of the
    /// source and intermediate results.
    /// </summary>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="intermediateSelector">A transform function to apply to the value of the input result.</param>
    /// <param name="returnSelector">A transform function to apply to the value of the intermediate result.</param>
    /// <returns>A <see cref="Result{T}"/> whose value is the result of invoking the transform function
    ///     <paramref name="intermediateSelector"/> on the value of this result and then mapping the values of that result and
    ///     the source result to the final result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="intermediateSelector"/> is <see langword="null"/>, or if
    ///     <paramref name="returnSelector"/> is <see langword="null"/>.</exception>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static Task<Result<TReturn>> SelectMany<TReturn>(
        this IResult<DBNull> sourceResult,
        Func<DBNull, Task<Result>> intermediateSelector,
        Func<DBNull, DBNull, TReturn> returnSelector)
    {
        if (sourceResult is null) throw new ArgumentNullException(nameof(sourceResult));
        if (intermediateSelector is null) throw new ArgumentNullException(nameof(intermediateSelector));
        if (returnSelector is null) throw new ArgumentNullException(nameof(returnSelector));

        return sourceResult.SelectMany(
            async sourceValue => (await intermediateSelector(sourceValue).ConfigureAwait(false)).Select(
                intermediateValue => returnSelector(sourceValue, intermediateValue)));
    }

    /// <summary>
    /// Projects the value of a result to an intermediate result and invokes a result selector function on the values of the
    /// source and intermediate results.
    /// </summary>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="intermediateSelector">A transform function to apply to the value of the input result.</param>
    /// <param name="returnSelector">A transform function to apply to the value of the intermediate result.</param>
    /// <returns>A <see cref="Result{T}"/> whose value is the result of invoking the transform function
    ///     <paramref name="intermediateSelector"/> on the value of this result and then mapping the values of that result and
    ///     the source result to the final result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="intermediateSelector"/> is <see langword="null"/>, or if
    ///     <paramref name="returnSelector"/> is <see langword="null"/>.</exception>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static Task<Result<TReturn>> SelectMany<TReturn>(
        this Task<IResult<DBNull>> sourceResult,
        Func<DBNull, Result> intermediateSelector,
        Func<DBNull, DBNull, TReturn> returnSelector)
    {
        if (sourceResult is null) throw new ArgumentNullException(nameof(sourceResult));
        if (intermediateSelector is null) throw new ArgumentNullException(nameof(intermediateSelector));
        if (returnSelector is null) throw new ArgumentNullException(nameof(returnSelector));

        return sourceResult.SelectMany(
            sourceValue => intermediateSelector(sourceValue).Select(
                intermediateValue => returnSelector(sourceValue, intermediateValue)));
    }

    /// <summary>
    /// Projects the value of a result to an intermediate result and invokes a result selector function on the values of the
    /// source and intermediate results.
    /// </summary>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="intermediateSelector">A transform function to apply to the value of the input result.</param>
    /// <param name="returnSelector">A transform function to apply to the value of the intermediate result.</param>
    /// <returns>A <see cref="Result{T}"/> whose value is the result of invoking the transform function
    ///     <paramref name="intermediateSelector"/> on the value of this result and then mapping the values of that result and
    ///     the source result to the final result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="intermediateSelector"/> is <see langword="null"/>, or if
    ///     <paramref name="returnSelector"/> is <see langword="null"/>.</exception>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static Task<Result<TReturn>> SelectMany<TReturn>(
        this Task<IResult<DBNull>> sourceResult,
        Func<DBNull, Task<Result>> intermediateSelector,
        Func<DBNull, DBNull, TReturn> returnSelector)
    {
        if (sourceResult is null) throw new ArgumentNullException(nameof(sourceResult));
        if (intermediateSelector is null) throw new ArgumentNullException(nameof(intermediateSelector));
        if (returnSelector is null) throw new ArgumentNullException(nameof(returnSelector));

        return sourceResult.SelectMany(
            async sourceValue => (await intermediateSelector(sourceValue).ConfigureAwait(false)).Select(
                intermediateValue => returnSelector(sourceValue, intermediateValue)));
    }

    /// <summary>
    /// Projects the value of a result to an intermediate result and invokes a result selector function on the values of the
    /// source and intermediate results.
    /// </summary>
    /// <typeparam name="TIntermediate">The type of the intermediate result, as collected by
    ///     <paramref name="intermediateSelector"/>.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="intermediateSelector">A transform function to apply to the value of the input result.</param>
    /// <param name="returnSelector">A transform function to apply to the value of the intermediate result.</param>
    /// <returns>A <see cref="Result{T}"/> whose value is the result of invoking the transform function
    ///     <paramref name="intermediateSelector"/> on the value of this result and then mapping the values of that result and
    ///     the source result to the final result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="intermediateSelector"/> is <see langword="null"/>, or if
    ///     <paramref name="returnSelector"/> is <see langword="null"/>.</exception>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static Result<TReturn> SelectMany<TIntermediate, TReturn>(
        this IResult<DBNull> sourceResult,
        Func<DBNull, Result<TIntermediate>> intermediateSelector,
        Func<DBNull, TIntermediate, TReturn> returnSelector)
    {
        if (sourceResult is null) throw new ArgumentNullException(nameof(sourceResult));
        if (intermediateSelector is null) throw new ArgumentNullException(nameof(intermediateSelector));
        if (returnSelector is null) throw new ArgumentNullException(nameof(returnSelector));

        return sourceResult.SelectMany(
            sourceValue => intermediateSelector(sourceValue).Select(
                intermediateValue => returnSelector(sourceValue, intermediateValue)));
    }

    /// <summary>
    /// Projects the value of a result to an intermediate result and invokes a result selector function on the values of the
    /// source and intermediate results.
    /// </summary>
    /// <typeparam name="TIntermediate">The type of the intermediate result, as collected by
    ///     <paramref name="intermediateSelector"/>.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="intermediateSelector">A transform function to apply to the value of the input result.</param>
    /// <param name="returnSelector">A transform function to apply to the value of the intermediate result.</param>
    /// <returns>A <see cref="Result{T}"/> whose value is the result of invoking the transform function
    ///     <paramref name="intermediateSelector"/> on the value of this result and then mapping the values of that result and
    ///     the source result to the final result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="intermediateSelector"/> is <see langword="null"/>, or if
    ///     <paramref name="returnSelector"/> is <see langword="null"/>.</exception>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static Task<Result<TReturn>> SelectMany<TIntermediate, TReturn>(
        this IResult<DBNull> sourceResult,
        Func<DBNull, Task<Result<TIntermediate>>> intermediateSelector,
        Func<DBNull, TIntermediate, TReturn> returnSelector)
    {
        if (sourceResult is null) throw new ArgumentNullException(nameof(sourceResult));
        if (intermediateSelector is null) throw new ArgumentNullException(nameof(intermediateSelector));
        if (returnSelector is null) throw new ArgumentNullException(nameof(returnSelector));

        return sourceResult.SelectMany(
            sourceValue => intermediateSelector(sourceValue).Select(
                intermediateValue => returnSelector(sourceValue, intermediateValue)));
    }

    /// <summary>
    /// Projects the value of a result to an intermediate result and invokes a result selector function on the values of the
    /// source and intermediate results.
    /// </summary>
    /// <typeparam name="TIntermediate">The type of the intermediate result, as collected by
    ///     <paramref name="intermediateSelector"/>.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="intermediateSelector">A transform function to apply to the value of the input result.</param>
    /// <param name="returnSelector">A transform function to apply to the value of the intermediate result.</param>
    /// <returns>A <see cref="Result{T}"/> whose value is the result of invoking the transform function
    ///     <paramref name="intermediateSelector"/> on the value of this result and then mapping the values of that result and
    ///     the source result to the final result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="intermediateSelector"/> is <see langword="null"/>, or if
    ///     <paramref name="returnSelector"/> is <see langword="null"/>.</exception>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static Task<Result<TReturn>> SelectMany<TIntermediate, TReturn>(
        this Task<IResult<DBNull>> sourceResult,
        Func<DBNull, Result<TIntermediate>> intermediateSelector,
        Func<DBNull, TIntermediate, TReturn> returnSelector)
    {
        if (sourceResult is null) throw new ArgumentNullException(nameof(sourceResult));
        if (intermediateSelector is null) throw new ArgumentNullException(nameof(intermediateSelector));
        if (returnSelector is null) throw new ArgumentNullException(nameof(returnSelector));

        return sourceResult.SelectMany(
            sourceValue => intermediateSelector(sourceValue).Select(
                intermediateValue => returnSelector(sourceValue, intermediateValue)));
    }

    /// <summary>
    /// Projects the value of a result to an intermediate result and invokes a result selector function on the values of the
    /// source and intermediate results.
    /// </summary>
    /// <typeparam name="TIntermediate">The type of the intermediate result, as collected by
    ///     <paramref name="intermediateSelector"/>.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="intermediateSelector">A transform function to apply to the value of the input result.</param>
    /// <param name="returnSelector">A transform function to apply to the value of the intermediate result.</param>
    /// <returns>A <see cref="Result{T}"/> whose value is the result of invoking the transform function
    ///     <paramref name="intermediateSelector"/> on the value of this result and then mapping the values of that result and
    ///     the source result to the final result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="intermediateSelector"/> is <see langword="null"/>, or if
    ///     <paramref name="returnSelector"/> is <see langword="null"/>.</exception>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static Task<Result<TReturn>> SelectMany<TIntermediate, TReturn>(
        this Task<IResult<DBNull>> sourceResult,
        Func<DBNull, Task<Result<TIntermediate>>> intermediateSelector,
        Func<DBNull, TIntermediate, TReturn> returnSelector)
    {
        if (sourceResult is null) throw new ArgumentNullException(nameof(sourceResult));
        if (intermediateSelector is null) throw new ArgumentNullException(nameof(intermediateSelector));
        if (returnSelector is null) throw new ArgumentNullException(nameof(returnSelector));

        return sourceResult.SelectMany(
            sourceValue => intermediateSelector(sourceValue).Select(
                intermediateValue => returnSelector(sourceValue, intermediateValue)));
    }

    /// <summary>
    /// Projects the value of a result to an intermediate result and invokes a result selector function on the values of the
    /// source and intermediate results.
    /// </summary>
    /// <typeparam name="TIntermediate">The type of the intermediate result, as collected by
    ///     <paramref name="intermediateSelector"/>.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="intermediateSelector">A transform function to apply to the value of the input result.</param>
    /// <param name="returnSelector">A transform function to apply to the value of the intermediate result.</param>
    /// <returns>A <see cref="Maybe{T}"/> whose value is the result of invoking the transform function
    ///     <paramref name="intermediateSelector"/> on the value of this result and then mapping the values of that result and
    ///     the source result to the final result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="intermediateSelector"/> is <see langword="null"/>, or if
    ///     <paramref name="returnSelector"/> is <see langword="null"/>.</exception>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static Maybe<TReturn> SelectMany<TIntermediate, TReturn>(
        this IResult<DBNull> sourceResult,
        Func<DBNull, Maybe<TIntermediate>> intermediateSelector,
        Func<DBNull, TIntermediate, TReturn> returnSelector)
    {
        if (sourceResult is null) throw new ArgumentNullException(nameof(sourceResult));
        if (intermediateSelector is null) throw new ArgumentNullException(nameof(intermediateSelector));
        if (returnSelector is null) throw new ArgumentNullException(nameof(returnSelector));

        return sourceResult.SelectMany(
            sourceValue => intermediateSelector(sourceValue).Select(
                intermediateValue => returnSelector(sourceValue, intermediateValue)));
    }

    /// <summary>
    /// Projects the value of a result to an intermediate result and invokes a result selector function on the values of the
    /// source and intermediate results.
    /// </summary>
    /// <typeparam name="TIntermediate">The type of the intermediate result, as collected by
    ///     <paramref name="intermediateSelector"/>.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="intermediateSelector">A transform function to apply to the value of the input result.</param>
    /// <param name="returnSelector">A transform function to apply to the value of the intermediate result.</param>
    /// <returns>A <see cref="Maybe{T}"/> whose value is the result of invoking the transform function
    ///     <paramref name="intermediateSelector"/> on the value of this result and then mapping the values of that result and
    ///     the source result to the final result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="intermediateSelector"/> is <see langword="null"/>, or if
    ///     <paramref name="returnSelector"/> is <see langword="null"/>.</exception>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static Task<Maybe<TReturn>> SelectMany<TIntermediate, TReturn>(
        this IResult<DBNull> sourceResult,
        Func<DBNull, Task<Maybe<TIntermediate>>> intermediateSelector,
        Func<DBNull, TIntermediate, TReturn> returnSelector)
    {
        if (sourceResult is null) throw new ArgumentNullException(nameof(sourceResult));
        if (intermediateSelector is null) throw new ArgumentNullException(nameof(intermediateSelector));
        if (returnSelector is null) throw new ArgumentNullException(nameof(returnSelector));

        return sourceResult.SelectMany(
            sourceValue => intermediateSelector(sourceValue).Select(
                intermediateValue => returnSelector(sourceValue, intermediateValue)));
    }

    /// <summary>
    /// Projects the value of a result to an intermediate result and invokes a result selector function on the values of the
    /// source and intermediate results.
    /// </summary>
    /// <typeparam name="TIntermediate">The type of the intermediate result, as collected by
    ///     <paramref name="intermediateSelector"/>.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="intermediateSelector">A transform function to apply to the value of the input result.</param>
    /// <param name="returnSelector">A transform function to apply to the value of the intermediate result.</param>
    /// <returns>A <see cref="Maybe{T}"/> whose value is the result of invoking the transform function
    ///     <paramref name="intermediateSelector"/> on the value of this result and then mapping the values of that result and
    ///     the source result to the final result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="intermediateSelector"/> is <see langword="null"/>, or if
    ///     <paramref name="returnSelector"/> is <see langword="null"/>.</exception>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static Task<Maybe<TReturn>> SelectMany<TIntermediate, TReturn>(
        this Task<IResult<DBNull>> sourceResult,
        Func<DBNull, Maybe<TIntermediate>> intermediateSelector,
        Func<DBNull, TIntermediate, TReturn> returnSelector)
    {
        if (sourceResult is null) throw new ArgumentNullException(nameof(sourceResult));
        if (intermediateSelector is null) throw new ArgumentNullException(nameof(intermediateSelector));
        if (returnSelector is null) throw new ArgumentNullException(nameof(returnSelector));

        return sourceResult.SelectMany(
            sourceValue => intermediateSelector(sourceValue).Select(
                intermediateValue => returnSelector(sourceValue, intermediateValue)));
    }

    /// <summary>
    /// Projects the value of a result to an intermediate result and invokes a result selector function on the values of the
    /// source and intermediate results.
    /// </summary>
    /// <typeparam name="TIntermediate">The type of the intermediate result, as collected by
    ///     <paramref name="intermediateSelector"/>.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="intermediateSelector">A transform function to apply to the value of the input result.</param>
    /// <param name="returnSelector">A transform function to apply to the value of the intermediate result.</param>
    /// <returns>A <see cref="Maybe{T}"/> whose value is the result of invoking the transform function
    ///     <paramref name="intermediateSelector"/> on the value of this result and then mapping the values of that result and
    ///     the source result to the final result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="intermediateSelector"/> is <see langword="null"/>, or if
    ///     <paramref name="returnSelector"/> is <see langword="null"/>.</exception>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static Task<Maybe<TReturn>> SelectMany<TIntermediate, TReturn>(
        this Task<IResult<DBNull>> sourceResult,
        Func<DBNull, Task<Maybe<TIntermediate>>> intermediateSelector,
        Func<DBNull, TIntermediate, TReturn> returnSelector)
    {
        if (sourceResult is null) throw new ArgumentNullException(nameof(sourceResult));
        if (intermediateSelector is null) throw new ArgumentNullException(nameof(intermediateSelector));
        if (returnSelector is null) throw new ArgumentNullException(nameof(returnSelector));

        return sourceResult.SelectMany(
            sourceValue => intermediateSelector(sourceValue).Select(
                intermediateValue => returnSelector(sourceValue, intermediateValue)));
    }

    /// <summary>
    /// Projects the value of a result to an intermediate result and invokes a result selector function on the values of the
    /// source and intermediate results.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="intermediateSelector">A transform function to apply to the value of the input result.</param>
    /// <param name="returnSelector">A transform function to apply to the value of the intermediate result.</param>
    /// <returns>A <see cref="Result{T}"/> whose value is the result of invoking the transform function
    ///     <paramref name="intermediateSelector"/> on the value of this result and then mapping the values of that result and
    ///     the source result to the final result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="intermediateSelector"/> is <see langword="null"/>, or if
    ///     <paramref name="returnSelector"/> is <see langword="null"/>.</exception>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static Result<TReturn> SelectMany<T, TReturn>(
        this Result<T> sourceResult,
        Func<T, Result> intermediateSelector,
        Func<T, DBNull, TReturn> returnSelector)
    {
        if (intermediateSelector is null) throw new ArgumentNullException(nameof(intermediateSelector));
        if (returnSelector is null) throw new ArgumentNullException(nameof(returnSelector));

        return sourceResult.SelectMany(
            sourceValue => intermediateSelector(sourceValue).Select(
                intermediateValue => returnSelector(sourceValue, intermediateValue)));
    }

    /// <summary>
    /// Projects the value of a result to an intermediate result and invokes a result selector function on the values of the
    /// source and intermediate results.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="intermediateSelector">A transform function to apply to the value of the input result.</param>
    /// <param name="returnSelector">A transform function to apply to the value of the intermediate result.</param>
    /// <returns>A <see cref="Result{T}"/> whose value is the result of invoking the transform function
    ///     <paramref name="intermediateSelector"/> on the value of this result and then mapping the values of that result and
    ///     the source result to the final result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="intermediateSelector"/> is <see langword="null"/>, or if
    ///     <paramref name="returnSelector"/> is <see langword="null"/>.</exception>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static Task<Result<TReturn>> SelectMany<T, TReturn>(
        this Result<T> sourceResult,
        Func<T, Task<Result>> intermediateSelector,
        Func<T, DBNull, TReturn> returnSelector)
    {
        if (intermediateSelector is null) throw new ArgumentNullException(nameof(intermediateSelector));
        if (returnSelector is null) throw new ArgumentNullException(nameof(returnSelector));

        return sourceResult.SelectMany(
            async sourceValue => (await intermediateSelector(sourceValue).ConfigureAwait(false)).Select(
                intermediateValue => returnSelector(sourceValue, intermediateValue)));
    }

    /// <summary>
    /// Projects the value of a result to an intermediate result and invokes a result selector function on the values of the
    /// source and intermediate results.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="intermediateSelector">A transform function to apply to the value of the input result.</param>
    /// <param name="returnSelector">A transform function to apply to the value of the intermediate result.</param>
    /// <returns>A <see cref="Result{T}"/> whose value is the result of invoking the transform function
    ///     <paramref name="intermediateSelector"/> on the value of this result and then mapping the values of that result and
    ///     the source result to the final result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="intermediateSelector"/> is <see langword="null"/>, or if
    ///     <paramref name="returnSelector"/> is <see langword="null"/>.</exception>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static Task<Result<TReturn>> SelectMany<T, TReturn>(
        this Task<Result<T>> sourceResult,
        Func<T, Result> intermediateSelector,
        Func<T, DBNull, TReturn> returnSelector)
    {
        if (intermediateSelector is null) throw new ArgumentNullException(nameof(intermediateSelector));
        if (returnSelector is null) throw new ArgumentNullException(nameof(returnSelector));

        return sourceResult.SelectMany(
            sourceValue => intermediateSelector(sourceValue).Select(
                intermediateValue => returnSelector(sourceValue, intermediateValue)));
    }

    /// <summary>
    /// Projects the value of a result to an intermediate result and invokes a result selector function on the values of the
    /// source and intermediate results.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="intermediateSelector">A transform function to apply to the value of the input result.</param>
    /// <param name="returnSelector">A transform function to apply to the value of the intermediate result.</param>
    /// <returns>A <see cref="Result{T}"/> whose value is the result of invoking the transform function
    ///     <paramref name="intermediateSelector"/> on the value of this result and then mapping the values of that result and
    ///     the source result to the final result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="intermediateSelector"/> is <see langword="null"/>, or if
    ///     <paramref name="returnSelector"/> is <see langword="null"/>.</exception>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static Task<Result<TReturn>> SelectMany<T, TReturn>(
        this Task<Result<T>> sourceResult,
        Func<T, Task<Result>> intermediateSelector,
        Func<T, DBNull, TReturn> returnSelector)
    {
        if (intermediateSelector is null) throw new ArgumentNullException(nameof(intermediateSelector));
        if (returnSelector is null) throw new ArgumentNullException(nameof(returnSelector));

        return sourceResult.SelectMany(
            async sourceValue => (await intermediateSelector(sourceValue).ConfigureAwait(false)).Select(
                intermediateValue => returnSelector(sourceValue, intermediateValue)));
    }

    /// <summary>
    /// Projects the value of a result to an intermediate result and invokes a result selector function on the values of the
    /// source and intermediate results.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="intermediateSelector">A transform function to apply to the value of the input result.</param>
    /// <param name="returnSelector">A transform function to apply to the value of the intermediate result.</param>
    /// <returns>A <see cref="Result{T}"/> whose value is the result of invoking the transform function
    ///     <paramref name="intermediateSelector"/> on the value of this result and then mapping the values of that result and
    ///     the source result to the final result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="intermediateSelector"/> is <see langword="null"/>, or if
    ///     <paramref name="returnSelector"/> is <see langword="null"/>.</exception>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static Result<TReturn> SelectMany<T, TReturn>(
        this Maybe<T> sourceResult,
        Func<T, Result> intermediateSelector,
        Func<T, DBNull, TReturn> returnSelector)
    {
        if (intermediateSelector is null) throw new ArgumentNullException(nameof(intermediateSelector));
        if (returnSelector is null) throw new ArgumentNullException(nameof(returnSelector));

        return sourceResult.SelectMany(
            sourceValue => intermediateSelector(sourceValue).Select(
                intermediateValue => returnSelector(sourceValue, intermediateValue)));
    }

    /// <summary>
    /// Projects the value of a result to an intermediate result and invokes a result selector function on the values of the
    /// source and intermediate results.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="intermediateSelector">A transform function to apply to the value of the input result.</param>
    /// <param name="returnSelector">A transform function to apply to the value of the intermediate result.</param>
    /// <returns>A <see cref="Result{T}"/> whose value is the result of invoking the transform function
    ///     <paramref name="intermediateSelector"/> on the value of this result and then mapping the values of that result and
    ///     the source result to the final result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="intermediateSelector"/> is <see langword="null"/>, or if
    ///     <paramref name="returnSelector"/> is <see langword="null"/>.</exception>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static Task<Result<TReturn>> SelectMany<T, TReturn>(
        this Maybe<T> sourceResult,
        Func<T, Task<Result>> intermediateSelector,
        Func<T, DBNull, TReturn> returnSelector)
    {
        if (intermediateSelector is null) throw new ArgumentNullException(nameof(intermediateSelector));
        if (returnSelector is null) throw new ArgumentNullException(nameof(returnSelector));

        return sourceResult.SelectMany(
            async sourceValue => (await intermediateSelector(sourceValue).ConfigureAwait(false)).Select(
                intermediateValue => returnSelector(sourceValue, intermediateValue)));
    }

    /// <summary>
    /// Projects the value of a result to an intermediate result and invokes a result selector function on the values of the
    /// source and intermediate results.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="intermediateSelector">A transform function to apply to the value of the input result.</param>
    /// <param name="returnSelector">A transform function to apply to the value of the intermediate result.</param>
    /// <returns>A <see cref="Result{T}"/> whose value is the result of invoking the transform function
    ///     <paramref name="intermediateSelector"/> on the value of this result and then mapping the values of that result and
    ///     the source result to the final result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="intermediateSelector"/> is <see langword="null"/>, or if
    ///     <paramref name="returnSelector"/> is <see langword="null"/>.</exception>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static Task<Result<TReturn>> SelectMany<T, TReturn>(
        this Task<Maybe<T>> sourceResult,
        Func<T, Result> intermediateSelector,
        Func<T, DBNull, TReturn> returnSelector)
    {
        if (intermediateSelector is null) throw new ArgumentNullException(nameof(intermediateSelector));
        if (returnSelector is null) throw new ArgumentNullException(nameof(returnSelector));

        return sourceResult.SelectMany(
            sourceValue => intermediateSelector(sourceValue).Select(
                intermediateValue => returnSelector(sourceValue, intermediateValue)));
    }

    /// <summary>
    /// Projects the value of a result to an intermediate result and invokes a result selector function on the values of the
    /// source and intermediate results.
    /// </summary>
    /// <typeparam name="T">The type of the source result value.</typeparam>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="sourceResult">The source result.</param>
    /// <param name="intermediateSelector">A transform function to apply to the value of the input result.</param>
    /// <param name="returnSelector">A transform function to apply to the value of the intermediate result.</param>
    /// <returns>A <see cref="Result{T}"/> whose value is the result of invoking the transform function
    ///     <paramref name="intermediateSelector"/> on the value of this result and then mapping the values of that result and
    ///     the source result to the final result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="intermediateSelector"/> is <see langword="null"/>, or if
    ///     <paramref name="returnSelector"/> is <see langword="null"/>.</exception>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static Task<Result<TReturn>> SelectMany<T, TReturn>(
        this Task<Maybe<T>> sourceResult,
        Func<T, Task<Result>> intermediateSelector,
        Func<T, DBNull, TReturn> returnSelector)
    {
        if (intermediateSelector is null) throw new ArgumentNullException(nameof(intermediateSelector));
        if (returnSelector is null) throw new ArgumentNullException(nameof(returnSelector));

        return sourceResult.SelectMany(
            async sourceValue => (await intermediateSelector(sourceValue).ConfigureAwait(false)).Select(
                intermediateValue => returnSelector(sourceValue, intermediateValue)));
    }
}
