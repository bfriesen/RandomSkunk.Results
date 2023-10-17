namespace RandomSkunk.Results;

/// <summary>
/// Defines a result with a required value.
/// </summary>
/// <typeparam name="T">The type of the result value.</typeparam>
public readonly struct Result<T> : IResult<T>, IEquatable<Result<T>>
{
    private readonly Outcome _outcome;
    private readonly T? _value;
    private readonly Error? _error;

    private Result(T value)
    {
        _outcome = Outcome.Success;
        _value = value ?? throw new ArgumentNullException(nameof(value));
        _error = null;
    }

    private Result(Error? error)
    {
        _outcome = Outcome.Fail;
        _value = default;
        _error = error ?? new Error();

        FailResult.Handle(ref _error);
    }

    private enum Outcome
    {
        Fail,
        Success,
    }

    /// <summary>
    /// Gets a value indicating whether this is a <c>Success</c> result.
    /// </summary>
    /// <returns><see langword="true"/> if this is a <c>Success</c> result; otherwise, <see langword="false"/>.</returns>
    public bool IsSuccess => _outcome == Outcome.Success;

    /// <summary>
    /// Gets a value indicating whether this is a <c>Fail</c> result.
    /// </summary>
    /// <returns><see langword="true"/> if this is a <c>Fail</c> result; otherwise, <see langword="false"/>.</returns>
    public bool IsFail => _outcome == Outcome.Fail;

    /// <summary>
    /// Gets the value from the <c>Success</c> result.
    /// </summary>
    /// <returns>If this is a <c>Success</c> result, its value; otherwise throws an <see cref="InvalidStateException"/>.
    ///     </returns>
    /// <exception cref="InvalidStateException">If the result is not a <c>Success</c> result.</exception>
    public T Value =>
        _outcome == Outcome.Success
            ? _value!
            : throw Exceptions.CannotAccessValueUnlessSuccess(GetError());

    /// <summary>
    /// Gets the error from the <c>Fail</c> result.
    /// </summary>
    /// <returns>If this is a <c>Fail</c> result, its error; otherwise throws an <see cref="InvalidStateException"/>.</returns>
    /// <exception cref="InvalidStateException">If the result is not a <c>Fail</c> result.</exception>
    public Error Error =>
        _outcome == Outcome.Fail
            ? GetError()
            : throw Exceptions.CannotAccessErrorUnlessFail();

    /// <summary>
    /// Converts the specified value into a <c>Success</c> result with the same value. A <see langword="null"/> value is
    /// converted into a <c>Fail</c> result with error code <see cref="ErrorCodes.BadRequest"/>.
    /// </summary>
    /// <param name="value">The value. Can be <see langword="null"/>.</param>
    /// <returns>A <c>Success</c> result if <paramref name="value"/> is not <see langword="null"/>; otherwise, a <c>Fail</c>
    ///     result with a generated stack trace.</returns>
    public static implicit operator Result<T>(T? value) => FromValue(value);

    /// <summary>
    /// Converts the specified <see cref="Results.Error"/> into a <c>Fail</c> result with the same error.
    /// </summary>
    /// <param name="error">The error for the new <c>Fail</c> result.</param>
    /// <returns>A <c>Fail</c> result with the specified error.</returns>
    public static implicit operator Result<T>(Error? error) => Fail(error);

    /// <summary>
    /// Indicates whether the <paramref name="left"/> parameter is equal to the <paramref name="right"/> parameter.
    /// </summary>
    /// <param name="left">The left side of the comparison.</param>
    /// <param name="right">The right side of the comparison.</param>
    /// <returns><see langword="true"/> if the <paramref name="left"/> parameter is equal to the <paramref name="right"/>
    ///     parameter; otherwise, <see langword="false"/>.</returns>
    public static bool operator ==(Result<T> left, Result<T> right) => left.Equals(right);

    /// <summary>
    /// Indicates whether the <paramref name="left"/> parameter is not equal to the <paramref name="right"/> parameter.
    /// </summary>
    /// <param name="left">The left side of the comparison.</param>
    /// <param name="right">The right side of the comparison.</param>
    /// <returns><see langword="true"/> if the <paramref name="left"/> parameter is not equal to the <paramref name="right"/>
    ///     parameter; otherwise, <see langword="false"/>.</returns>
    public static bool operator !=(Result<T> left, Result<T> right) => !(left == right);

    /// <summary>
    /// Creates a <c>Success</c> result with the specified value.
    /// </summary>
    /// <param name="value">The value of the <c>Success</c> result. Must not be <see langword="null"/>.</param>
    /// <returns>A <c>Success</c> result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="value"/> is <see langword="null"/>.</exception>
    public static Result<T> Success(T value) => new(value);

    /// <summary>
    /// Creates a <c>Fail</c> result with the specified error.
    /// </summary>
    /// <param name="error">An error that describes the failure. If <see langword="null"/>, a default error is used.</param>
    /// <returns>A <c>Fail</c> result.</returns>
    public static Result<T> Fail(Error? error = null) => new(error);

    /// <summary>
    /// Creates a <c>Fail</c> result.
    /// </summary>
    /// <param name="exception">The exception that caused the failure.</param>
    /// <param name="errorMessage">The error message.</param>
    /// <param name="errorCode">The error code. Default value is <see cref="ErrorCodes.CaughtException"/>.</param>
    /// <param name="errorIdentifier">The optional identifier of the error.</param>
    /// <param name="errorTitle">The optional title for the error. If <see langword="null"/>, then "Error" is used instead.
    ///     </param>
    /// <returns>A <c>Fail</c> result.</returns>
    public static Result<T> Fail(
        Exception exception,
        string errorMessage = Error.DefaultFromExceptionMessage,
        int? errorCode = ErrorCodes.CaughtException,
        string? errorIdentifier = null,
        string? errorTitle = null) =>
        Fail(Error.FromException(exception, errorMessage, errorCode, errorIdentifier, errorTitle));

    /// <summary>
    /// Creates a <c>Fail</c> result.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    /// <param name="errorCode">The error code. Default value is <see cref="ErrorCodes.InternalServerError"/>.</param>
    /// <param name="errorIdentifier">The optional identifier of the error.</param>
    /// <param name="errorTitle">The optional title for the error. If <see langword="null"/>, then "Error" is used instead.
    ///     </param>
    /// <param name="extensions">Additional properties for the error.</param>
    /// <param name="innerError">The optional error that is the cause of the current error.</param>
    /// <returns>A <c>Fail</c> result.</returns>
    public static Result<T> Fail(
        string errorMessage,
        int? errorCode = ErrorCodes.InternalServerError,
        string? errorIdentifier = null,
        string? errorTitle = null,
        IReadOnlyDictionary<string, object>? extensions = null,
        Error? innerError = null) =>
        Fail(
            new Error
            {
                Message = errorMessage,
                Title = errorTitle!,
                Identifier = errorIdentifier,
                ErrorCode = errorCode,
                Extensions = extensions!,
                InnerError = innerError,
            });

    /// <summary>
    /// Creates a <c>Success</c> result with the specified value. If the value is <see langword="null"/>, then a <c>Fail</c>
    /// result with error code <see cref="ErrorCodes.NoValue"/> is returned instead.
    /// </summary>
    /// <param name="value">The value. Can be <see langword="null"/>.</param>
    /// <returns>A <c>Success</c> result if <paramref name="value"/> is not <see langword="null"/>; otherwise, a <c>Fail</c>
    ///     result with a generated stack trace.</returns>
    public static Result<T> FromValue(T? value) =>
        value is not null
            ? Success(value)
            : Fail(Errors.NoValue());

    /// <summary>
    /// Converts this <see cref="Result{T}"/> to an equivalent <see cref="Maybe{T}"/>. If this is a <c>Success</c> result, then a
    /// <c>Success</c> result with the same value is returned. Otherwise, if the <c>Fail</c> result's error has error code
    /// <see cref="ErrorCodes.NoValue"/>, then a <c>None</c> result is returned. For any other error code, a new <c>Fail</c>
    /// result with the same error is returned.
    /// </summary>
    /// <returns>The equivalent <see cref="Result{T}"/>.</returns>
    public Maybe<T> AsMaybe() =>
        SelectMany(Maybe<T>.Success);

    /// <summary>
    /// Gets the value of the <c>Success</c> result, or the specified fallback value if it is a <c>Fail</c> result.
    /// </summary>
    /// <param name="fallbackValue">The fallback value to return if this is not a <c>Success</c> result.</param>
    /// <returns>The value of this result if this is a <c>Success</c> result; otherwise, <paramref name="fallbackValue"/>.
    ///     </returns>
    public T? GetValueOr(T? fallbackValue)
    {
        return _outcome == Outcome.Success ? _value! : fallbackValue;
    }

    /// <summary>
    /// Gets the value of the <c>Success</c> result, or the specified fallback value if it is a <c>Fail</c> result.
    /// </summary>
    /// <param name="getFallbackValue">A function that creates the fallback value to return if this is not a <c>Success</c>
    ///     result.</param>
    /// <returns>The value of this result if this is a <c>Success</c> result; otherwise, the value returned by the
    ///     <paramref name="getFallbackValue"/> function.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="getFallbackValue"/> is <see langword="null"/>.</exception>
    public T? GetValueOr(Func<T?> getFallbackValue)
    {
        if (getFallbackValue is null) throw new ArgumentNullException(nameof(getFallbackValue));

        return _outcome == Outcome.Success ? _value! : getFallbackValue();
    }

    /// <summary>
    /// Gets the value of the <c>Success</c> result, or the default value of type <typeparamref name="T"/> if it is a <c>Fail</c>
    /// result.
    /// </summary>
    /// <returns>The value of this result if this is a <c>Success</c> result; otherwise, the default value of type
    ///     <typeparamref name="T"/>.</returns>
    public T? GetValueOrDefault() => GetValueOr((T?)default);

    /// <summary>
    /// Determines whether the value of the result equals the <paramref name="otherValue"/>.
    /// </summary>
    /// <param name="otherValue">The value to compare.</param>
    /// <param name="comparer">The <see cref="IEqualityComparer{T}"/> used to determine equality of the values. If
    ///     <see langword="null"/>, then <see cref="EqualityComparer{T}.Default"/> is used instead.</param>
    /// <returns><see langword="true"/> if this is a <c>Success</c> result and its value equals <paramref name="otherValue"/>;
    ///     otherwise, <see langword="false"/>.</returns>
    public bool HasValue(T otherValue, IEqualityComparer<T>? comparer = null)
    {
        comparer ??= EqualityComparer<T>.Default;

        return _outcome == Outcome.Success && comparer.Equals(_value!, otherValue);
    }

    /// <summary>
    /// Determines whether the value of the result is equal to another value as defined by the <paramref name="comparison"/>
    /// function.
    /// </summary>
    /// <param name="comparison">A function that defines the equality of the result value.</param>
    /// <returns><see langword="true"/> if this is a <c>Success</c> result and <paramref name="comparison"/> evaluates to
    ///     <see langword="true"/> when passed its value; otherwise, <see langword="false"/>.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="comparison"/> is <see langword="null"/>.</exception>
    public bool HasValue(Func<T, bool> comparison)
    {
        if (comparison is null) throw new ArgumentNullException(nameof(comparison));

        return _outcome == Outcome.Success && comparison(_value!);
    }

    /// <summary>
    /// Evaluates either the <paramref name="onSuccess"/> or <paramref name="onFail"/> function depending on whether the result
    /// is <c>Success</c> or <c>Fail</c>.
    /// </summary>
    /// <typeparam name="TReturn">The return type of the match method.</typeparam>
    /// <param name="onSuccess">The function to evaluate if the result is <c>Success</c>. The non-null value of the
    ///     <c>Success</c> result is passed to this function.</param>
    /// <param name="onFail">The function to evaluate if the result is <c>Fail</c>. The non-null error of the <c>Fail</c> result
    ///     is passed to this function.</param>
    /// <returns>The result of the matching function evaluation.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccess"/> is <see langword="null"/> or if
    ///     <paramref name="onFail"/> is <see langword="null"/>.</exception>
    public TReturn Match<TReturn>(
        Func<T, TReturn> onSuccess,
        Func<Error, TReturn> onFail)
    {
        if (onSuccess is null) throw new ArgumentNullException(nameof(onSuccess));
        if (onFail is null) throw new ArgumentNullException(nameof(onFail));

        return _outcome == Outcome.Success
            ? onSuccess(_value!)
            : onFail(GetError());
    }

    /// <summary>
    /// Evaluates either the <paramref name="onSuccess"/> or <paramref name="onFail"/> function depending on whether the result
    /// is <c>Success</c> or <c>Fail</c>.
    /// </summary>
    /// <typeparam name="TReturn">The return type of the match method.</typeparam>
    /// <param name="onSuccess">The function to evaluate if the result is <c>Success</c>. The non-null value of the
    ///     <c>Success</c> result is passed to this function.</param>
    /// <param name="onFail">The function to evaluate if the result is <c>Fail</c>. The non-null error of the <c>Fail</c> result
    ///     is passed to this function.</param>
    /// <returns>A task that represents the asynchronous match operation, which wraps the result of the matching function
    ///     evaluation.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onSuccess"/> is <see langword="null"/> or if
    ///     <paramref name="onFail"/> is <see langword="null"/>.</exception>
    public Task<TReturn> Match<TReturn>(
        Func<T, Task<TReturn>> onSuccess,
        Func<Error, Task<TReturn>> onFail)
    {
        if (onSuccess is null) throw new ArgumentNullException(nameof(onSuccess));
        if (onFail is null) throw new ArgumentNullException(nameof(onFail));

        return _outcome == Outcome.Success
            ? onSuccess(_value!)
            : onFail(GetError());
    }

    /// <summary>
    /// Invokes the <paramref name="onFailCallback"/> function if the current result is a <c>Fail</c> result.
    /// </summary>
    /// <param name="onFailCallback">A callback function to invoke if this is a <c>Fail</c> result.</param>
    /// <returns>The current result.</returns>
    public Result<T> OnFail(Action<Error> onFailCallback)
    {
        if (onFailCallback is null) throw new ArgumentNullException(nameof(onFailCallback));

        if (_outcome == Outcome.Fail)
        {
            try
            {
                onFailCallback(GetError());
            }
            catch (TaskCanceledException ex) when (FailResult.CatchCallbackExceptions)
            {
                return Fail(CompositeError.CreateOrGetSingle(new[]
                {
                    GetError(),
                    Errors.Canceled(ex),
                }));
            }
            catch (Exception ex) when (FailResult.CatchCallbackExceptions)
            {
                return Fail(CompositeError.CreateOrGetSingle(new[]
                {
                    GetError(),
                    Error.FromException(ex, Error.GetMessageForExceptionThrownInCallback(nameof(onFailCallback))),
                }));
            }
        }

        return this;
    }

    /// <summary>
    /// Invokes the <paramref name="onFailCallback"/> function if the current result is a <c>Fail</c> result.
    /// </summary>
    /// <param name="onFailCallback">A callback function to invoke if this is a <c>Fail</c> result.</param>
    /// <returns>The current result.</returns>
    public async Task<Result<T>> OnFail(Func<Error, Task> onFailCallback)
    {
        if (onFailCallback is null) throw new ArgumentNullException(nameof(onFailCallback));

        if (_outcome == Outcome.Fail)
        {
            try
            {
                await onFailCallback(GetError()).ConfigureAwait(ContinueOnCapturedContext);
            }
            catch (TaskCanceledException ex) when (FailResult.CatchCallbackExceptions)
            {
                return Fail(CompositeError.CreateOrGetSingle(new[]
                {
                    GetError(),
                    Errors.Canceled(ex),
                }));
            }
            catch (Exception ex) when (FailResult.CatchCallbackExceptions)
            {
                return Fail(CompositeError.CreateOrGetSingle(new[]
                {
                    GetError(),
                    Error.FromException(ex, Error.GetMessageForExceptionThrownInCallback(nameof(onFailCallback))),
                }));
            }
        }

        return this;
    }

    /// <summary>
    /// Invokes the <paramref name="onSuccessCallback"/> function if the current result is a <c>Success</c> result.
    /// </summary>
    /// <param name="onSuccessCallback">A callback function to invoke if this is a <c>Success</c> result.</param>
    /// <returns>The current result.</returns>
    public Result<T> OnSuccess(Action<T> onSuccessCallback)
    {
        if (onSuccessCallback is null) throw new ArgumentNullException(nameof(onSuccessCallback));

        if (_outcome == Outcome.Success)
        {
            try
            {
                onSuccessCallback(_value!);
            }
            catch (TaskCanceledException ex) when (FailResult.CatchCallbackExceptions)
            {
                return Errors.Canceled(ex);
            }
            catch (Exception ex) when (FailResult.CatchCallbackExceptions)
            {
                return Fail(ex, Error.GetMessageForExceptionThrownInCallback(nameof(onSuccessCallback)));
            }
        }

        return this;
    }

    /// <summary>
    /// Invokes the <paramref name="onSuccessCallback"/> function if the current result is a <c>Success</c> result.
    /// </summary>
    /// <param name="onSuccessCallback">A callback function to invoke if this is a <c>Success</c> result.</param>
    /// <returns>The current result.</returns>
    public async Task<Result<T>> OnSuccess(Func<T, Task> onSuccessCallback)
    {
        if (onSuccessCallback is null) throw new ArgumentNullException(nameof(onSuccessCallback));

        if (_outcome == Outcome.Success)
        {
            try
            {
                await onSuccessCallback(_value!).ConfigureAwait(ContinueOnCapturedContext);
            }
            catch (TaskCanceledException ex) when (FailResult.CatchCallbackExceptions)
            {
                return Errors.Canceled(ex);
            }
            catch (Exception ex) when (FailResult.CatchCallbackExceptions)
            {
                return Fail(ex, Error.GetMessageForExceptionThrownInCallback(nameof(onSuccessCallback)));
            }
        }

        return this;
    }

    /// <summary>
    /// Rescues a <c>Fail</c> result by returning the output of the <paramref name="onFail"/> function. If the current result is
    /// <c>Success</c>, nothing happens and the current result is returned.
    /// </summary>
    /// <param name="onFail">The function that rescues a failed operation.</param>
    /// <returns>The output of the <paramref name="onFail"/> function if the current result is <c>Fail</c>, or the same result if
    ///     it is <c>Success</c>.</returns>
    public Result<T> Rescue(Func<Error, Result<T>> onFail)
    {
        if (onFail is null) throw new ArgumentNullException(nameof(onFail));

        if (_outcome == Outcome.Fail)
        {
            try
            {
                return onFail(GetError());
            }
            catch (TaskCanceledException ex) when (FailResult.CatchCallbackExceptions)
            {
                return Errors.Canceled(ex);
            }
            catch (Exception ex) when (FailResult.CatchCallbackExceptions)
            {
                return Fail(ex, Error.GetMessageForExceptionThrownInCallback(nameof(onFail)));
            }
        }

        return this;
    }

    /// <summary>
    /// Rescues a <c>Fail</c> result by returning the output of the <paramref name="onFail"/> function. If the current result is
    /// <c>Success</c>, nothing happens and the current result is returned.
    /// </summary>
    /// <param name="onFail">The function that rescues a failed operation.</param>
    /// <returns>The output of the <paramref name="onFail"/> function if the current result is <c>Fail</c>, or the same result if
    ///     it is <c>Success</c>.</returns>
    public async Task<Result<T>> Rescue(Func<Error, Task<Result<T>>> onFail)
    {
        if (onFail is null) throw new ArgumentNullException(nameof(onFail));

        if (_outcome == Outcome.Fail)
        {
            try
            {
                return await onFail(GetError());
            }
            catch (TaskCanceledException ex) when (FailResult.CatchCallbackExceptions)
            {
                return Errors.Canceled(ex);
            }
            catch (Exception ex) when (FailResult.CatchCallbackExceptions)
            {
                return Fail(ex, Error.GetMessageForExceptionThrownInCallback(nameof(onFail)));
            }
        }

        return this;
    }

    /// <summary>
    /// Projects the result into a new <see cref="Result{T}"/> form: a <c>Success</c> result is projected to the new form as a
    /// <c>Success</c> result by passing its value to the <paramref name="onSuccessSelector"/> function; a <c>Fail</c> result is
    /// projected to the new form as a <c>Fail</c> result with the same error.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Select</c> and <c>SelectMany</c> is in the return value of their <c>onSuccessSelector</c>
    /// function. The selector for <c>Select</c> returns a regular (non-result) value, which is the value of the returned
    /// <c>Success</c> result. The selector for <c>SelectMany</c> returns a result value, which is itself the returned result
    /// (and may or may not be <c>Success</c>).
    /// </remarks>
    /// <typeparam name="TReturn">The type of the value returned by <paramref name="onSuccessSelector"/>.</typeparam>
    /// <param name="onSuccessSelector">A transform function to apply to the value of a <c>Success</c> result.</param>
    /// <returns>The projected result.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public Result<TReturn> Select<TReturn>(Func<T, TReturn> onSuccessSelector)
    {
        if (onSuccessSelector is null) throw new ArgumentNullException(nameof(onSuccessSelector));

        if (_outcome == Outcome.Success)
        {
            try
            {
                return onSuccessSelector(_value!).ToResult();
            }
            catch (TaskCanceledException ex) when (FailResult.CatchCallbackExceptions)
            {
                return Errors.Canceled(ex);
            }
            catch (Exception ex) when (FailResult.CatchCallbackExceptions)
            {
                return Result<TReturn>.Fail(ex, Error.GetMessageForExceptionThrownInCallback(nameof(onSuccessSelector)));
            }
        }

        return Result<TReturn>.Fail(GetError());
    }

    /// <summary>
    /// Projects the result into a new <see cref="Result{T}"/> form: a <c>Success</c> result is projected to the new form as a
    /// <c>Success</c> result by passing its value to the <paramref name="onSuccessSelector"/> function; a <c>Fail</c> result is
    /// projected to the new form as a <c>Fail</c> result with the same error.
    /// </summary>
    /// <remarks>
    /// The difference between <c>Select</c> and <c>SelectMany</c> is in the return value of their <c>onSuccessSelector</c>
    /// function. The selector for <c>Select</c> returns a regular (non-result) value, which is the value of the returned
    /// <c>Success</c> result. The selector for <c>SelectMany</c> returns a result value, which is itself the returned result
    /// (and may or may not be <c>Success</c>).
    /// </remarks>
    /// <typeparam name="TReturn">The type of the value returned by <paramref name="onSuccessSelector"/>.</typeparam>
    /// <param name="onSuccessSelector">A transform function to apply to the value of a <c>Success</c> result.</param>
    /// <returns>The projected result.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public async Task<Result<TReturn>> Select<TReturn>(Func<T, Task<TReturn>> onSuccessSelector)
    {
        if (onSuccessSelector is null) throw new ArgumentNullException(nameof(onSuccessSelector));

        if (_outcome == Outcome.Success)
        {
            try
            {
                return (await onSuccessSelector(_value!).ConfigureAwait(ContinueOnCapturedContext)).ToResult();
            }
            catch (TaskCanceledException ex) when (FailResult.CatchCallbackExceptions)
            {
                return Errors.Canceled(ex);
            }
            catch (Exception ex) when (FailResult.CatchCallbackExceptions)
            {
                return Result<TReturn>.Fail(ex, Error.GetMessageForExceptionThrownInCallback(nameof(onSuccessSelector)));
            }
        }

        return Result<TReturn>.Fail(GetError());
    }

    /// <summary>
    /// Projects the result into a new <see cref="Result{T}"/> form: a <c>Success</c> result is projected to the new form by
    /// passing its value to the <paramref name="onSuccessSelector"/> function; a <c>Fail</c> result is projected to the new form
    /// as a <c>Fail</c> result with the same error.
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

        if (_outcome == Outcome.Success)
        {
            try
            {
                return onSuccessSelector(_value!);
            }
            catch (TaskCanceledException ex) when (FailResult.CatchCallbackExceptions)
            {
                return Errors.Canceled(ex);
            }
            catch (Exception ex) when (FailResult.CatchCallbackExceptions)
            {
                return Result<TReturn>.Fail(ex, Error.GetMessageForExceptionThrownInCallback(nameof(onSuccessSelector)));
            }
        }

        return Result<TReturn>.Fail(GetError());
    }

    /// <summary>
    /// Projects the result into a new <see cref="Result{T}"/> form: a <c>Success</c> result is projected to the new form by
    /// passing its value to the <paramref name="onSuccessSelector"/> function; a <c>Fail</c> result is projected to the new form
    /// as a <c>Fail</c> result with the same error.
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

        if (_outcome == Outcome.Success)
        {
            try
            {
                return await onSuccessSelector(_value!).ConfigureAwait(ContinueOnCapturedContext);
            }
            catch (TaskCanceledException ex) when (FailResult.CatchCallbackExceptions)
            {
                return Errors.Canceled(ex);
            }
            catch (Exception ex) when (FailResult.CatchCallbackExceptions)
            {
                return Result<TReturn>.Fail(ex, Error.GetMessageForExceptionThrownInCallback(nameof(onSuccessSelector)));
            }
        }

        return Result<TReturn>.Fail(GetError());
    }

    /// <summary>
    /// Projects the result into a new <see cref="Result"/> form: a <c>Success</c> result is projected to the new form by passing
    /// its value to the <paramref name="onSuccessSelector"/> function; a <c>Fail</c> result is projected to the new form as a
    /// <c>Fail</c> result with the same error.
    /// </summary>
    /// <param name="onSuccessSelector">A transform funtion to apply to the value of a <c>Success</c> result.</param>
    /// <returns>The projected result.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public Result SelectMany(Func<T, Result> onSuccessSelector)
    {
        if (onSuccessSelector is null) throw new ArgumentNullException(nameof(onSuccessSelector));

        if (_outcome == Outcome.Success)
        {
            try
            {
                return onSuccessSelector(_value!);
            }
            catch (TaskCanceledException ex) when (FailResult.CatchCallbackExceptions)
            {
                return Errors.Canceled(ex);
            }
            catch (Exception ex) when (FailResult.CatchCallbackExceptions)
            {
                return Result.Fail(ex, Error.GetMessageForExceptionThrownInCallback(nameof(onSuccessSelector)));
            }
        }

        return Result.Fail(GetError());
    }

    /// <summary>
    /// Projects the result into a new <see cref="Result"/> form: a <c>Success</c> result is projected to the new form by passing
    /// its value to the <paramref name="onSuccessSelector"/> function; a <c>Fail</c> result is projected to the new form as a
    /// <c>Fail</c> result with the same error.
    /// </summary>
    /// <param name="onSuccessSelector">A transform funtion to apply to the value of a <c>Success</c> result.</param>
    /// <returns>The projected result.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="onSuccessSelector"/> is <see langword="null"/>.</exception>
    public async Task<Result> SelectMany(Func<T, Task<Result>> onSuccessSelector)
    {
        if (onSuccessSelector is null) throw new ArgumentNullException(nameof(onSuccessSelector));

        if (_outcome == Outcome.Success)
        {
            try
            {
                return await onSuccessSelector(_value!).ConfigureAwait(ContinueOnCapturedContext);
            }
            catch (TaskCanceledException ex) when (FailResult.CatchCallbackExceptions)
            {
                return Errors.Canceled(ex);
            }
            catch (Exception ex) when (FailResult.CatchCallbackExceptions)
            {
                return Result.Fail(ex, Error.GetMessageForExceptionThrownInCallback(nameof(onSuccessSelector)));
            }
        }

        return Result.Fail(GetError());
    }

    /// <summary>
    /// Projects the result into a new <see cref="Maybe{T}"/> form: a <c>Success</c> result is projected to the new form by
    /// passing its value to the <paramref name="onSuccessSelector"/> function; a <c>Fail</c> result is projected to the new form
    /// as a <c>Fail</c> result with the same error.
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
        {
            try
            {
                return onSuccessSelector(_value!);
            }
            catch (TaskCanceledException ex) when (FailResult.CatchCallbackExceptions)
            {
                return Errors.Canceled(ex);
            }
            catch (Exception ex) when (FailResult.CatchCallbackExceptions)
            {
                return Maybe<TReturn>.Fail(ex, Error.GetMessageForExceptionThrownInCallback(nameof(onSuccessSelector)));
            }
        }

        var error = GetError();
        if (error.ErrorCode == ErrorCodes.NoValue)
            return Maybe<TReturn>.None;

        return Maybe<TReturn>.Fail(error);
    }

    /// <summary>
    /// Projects the result into a new <see cref="Maybe{T}"/> form: a <c>Success</c> result is projected to the new form by
    /// passing its value to the <paramref name="onSuccessSelector"/> function; a <c>Fail</c> result is projected to the new form
    /// as a <c>Fail</c> result with the same error.
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
        {
            try
            {
                return await onSuccessSelector(_value!).ConfigureAwait(ContinueOnCapturedContext);
            }
            catch (TaskCanceledException ex) when (FailResult.CatchCallbackExceptions)
            {
                return Errors.Canceled(ex);
            }
            catch (Exception ex) when (FailResult.CatchCallbackExceptions)
            {
                return Maybe<TReturn>.Fail(ex, Error.GetMessageForExceptionThrownInCallback(nameof(onSuccessSelector)));
            }
        }

        var error = GetError();
        if (error.ErrorCode == ErrorCodes.NoValue)
            return Maybe<TReturn>.None;

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

    /// <summary>
    /// Projects the value of a result to an intermediate result and invokes a result selector function on the values of the
    /// source and intermediate results.
    /// </summary>
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="intermediateSelector">A transform function to apply to the value of the input result.</param>
    /// <param name="returnSelector">A transform function to apply to the value of the intermediate result.</param>
    /// <returns>A <see cref="Result{T}"/> whose value is the result of invoking the transform function
    ///     <paramref name="intermediateSelector"/> on the value of this result and then mapping the values of that result and
    ///     the source result to the final result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="intermediateSelector"/> is <see langword="null"/>, or if
    ///     <paramref name="returnSelector"/> is <see langword="null"/>.</exception>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public Result<TReturn> SelectMany<TReturn>(
        Func<T, Result> intermediateSelector,
        Func<T, Unit, TReturn> returnSelector)
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
    /// <typeparam name="TReturn">The type of the returned result value.</typeparam>
    /// <param name="intermediateSelector">A transform function to apply to the value of the input result.</param>
    /// <param name="returnSelector">A transform function to apply to the value of the intermediate result.</param>
    /// <returns>A <see cref="Result{T}"/> whose value is the result of invoking the transform function
    ///     <paramref name="intermediateSelector"/> on the value of this result and then mapping the values of that result and
    ///     the source result to the final result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="intermediateSelector"/> is <see langword="null"/>, or if
    ///     <paramref name="returnSelector"/> is <see langword="null"/>.</exception>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public Task<Result<TReturn>> SelectMany<TReturn>(
        Func<T, Task<Result>> intermediateSelector,
        Func<T, Unit, TReturn> returnSelector)
    {
        if (intermediateSelector is null) throw new ArgumentNullException(nameof(intermediateSelector));
        if (returnSelector is null) throw new ArgumentNullException(nameof(returnSelector));

        return SelectMany(
            sourceValue => intermediateSelector(sourceValue).Select(
                intermediateValue => returnSelector(sourceValue, intermediateValue)));
    }

    /// <summary>
    /// Gets a <c>Fail</c> result if <paramref name="predicate"/> returns <see langword="true"/> and this is a <c>Success</c>
    /// result; otherwise returns the same result.
    /// </summary>
    /// <param name="predicate">The delegate that determines whether to return a <c>Fail</c> result.</param>
    /// <param name="getError">An optional function that gets the <c>Fail</c> result's error. If <see langword="null"/>, a
    ///     generic error is used.</param>
    /// <returns>A <c>Fail</c> result if <paramref name="predicate"/> returned <see langword="true"/>, or the same result if it
    ///     did not.</returns>
    public Result<T> ToFailIf(Func<T, bool> predicate, Func<T, Error>? getError = null)
    {
        if (predicate is null) throw new ArgumentNullException(nameof(predicate));

        if (_outcome == Outcome.Success && predicate(_value!))
            return Fail(getError?.Invoke(_value!));

        return this;
    }

    /// <summary>
    /// Truncates the <see cref="Result{T}"/> into an equivalent <see cref="Result"/>. If it is a <c>Success</c> result, then its
    /// value is ignored and a valueless <c>Success</c> result is returned. Otherwise, a <c>Fail</c> result with the same error
    /// as is returned.
    /// </summary>
    /// <returns>An equivalent <see cref="Result"/>.</returns>
    public Result Truncate() =>
        SelectMany(_ => Result.Success());

    /// <summary>
    /// Attempts to get the error of the result, returning whether this is a <c>Fail</c> result (and therefore has an error).
    /// </summary>
    /// <param name="error">When this method returns, contains the <see cref="Results.Error"/> of the <c>Fail</c> result, or
    ///     <see langword="null"/> if this is not a <c>Fail</c> result. This parameter is passed uninitialized.</param>
    /// <returns><see langword="true"/> if this is a <c>Fail</c> result; otherwise <see langword="false"/>.</returns>
    public bool TryGetError([NotNullWhen(true)] out Error? error)
    {
        if (_outcome == Outcome.Fail)
        {
            error = GetError();
            return true;
        }

        error = null;
        return false;
    }

    /// <summary>
    /// Replaces the error of a <c>Fail</c> result, otherwise does nothing.
    /// </summary>
    /// <param name="onFailGetError">A function that returns the error for the returned <c>Fail</c> result.</param>
    /// <returns>A new <c>Fail</c> result with its error specified by the <paramref name="onFailGetError"/> function if this is a
    ///     <c>Fail</c> result; otherwise, the current result.</returns>
    /// <exception cref="ArgumentNullException">If <paramref name="onFailGetError"/> is <see langword="null"/>.</exception>
    public Result<T> WithError(Func<Error, Error> onFailGetError)
    {
        if (onFailGetError is null) throw new ArgumentNullException(nameof(onFailGetError));

        return _outcome == Outcome.Fail
            ? Fail(onFailGetError(GetError()))
            : this;
    }

    /// <inheritdoc/>
    public bool Equals(Result<T> other) =>
        _outcome == other._outcome
        && ((IsSuccess && EqualityComparer<T?>.Default.Equals(_value, other._value))
            || (IsFail && EqualityComparer<Error?>.Default.Equals(_error, other._error)));

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is Result<T> result && Equals(result);

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        int hashCode = 1157318437;
        hashCode = (hashCode * -1521134295) + EqualityComparer<Type>.Default.GetHashCode(typeof(T));
        hashCode = (hashCode * -1521134295) + _outcome.GetHashCode();
        hashCode = (hashCode * -1521134295) + (IsFail ? GetError().GetHashCode() : 0);
        hashCode = (hashCode * -1521134295) + (IsSuccess ? _value!.GetHashCode() : 0);
        hashCode *= 29;
        return hashCode;
    }

    /// <inheritdoc/>
    public override string ToString() =>
        Match(
            value => $"Success({(value is string ? $"\"{value}\"" : $"{value}")})",
            error => $"Fail({error.Title}: \"{error.Message}\")");

    /// <inheritdoc/>
    Error IResult.GetNonSuccessError() =>
        _outcome switch
        {
            Outcome.Fail => GetError(),
            _ => throw Exceptions.CannotAccessErrorUnlessNonSuccess(),
        };

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private Error GetError() => _error ?? Error.DefaultError;
}
