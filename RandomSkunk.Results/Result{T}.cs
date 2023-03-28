namespace RandomSkunk.Results;

/// <summary>
/// Defines a result with a required value.
/// </summary>
/// <typeparam name="T">The type of the result value.</typeparam>
/// <content> This struct is partial - additional methods are defined in the code files from the Operations folder. </content>
public readonly partial struct Result<T> : IResult<T>, IEquatable<Result<T>>
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

    private Result(Error? error, bool? omitStackTrace)
    {
        _outcome = Outcome.Fail;
        _value = default;
        _error = error ?? new Error();

        if (_error.StackTrace is null && !(omitStackTrace ?? FailResult.OmitStackTrace))
            _error = _error with { StackTrace = FilteredStackTrace.Create() };

        _error = FailResult.InvokeReplaceErrorIfSet(_error);
        FailResult.InvokeCallbackIfSet(_error);
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
    [StackTraceHidden]
    public static implicit operator Result<T>(Error error) => Fail(error);

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
    /// <param name="omitStackTrace">Whether to omit the stack trace of the error to the current location. If
    ///     <see langword="null"/> or not provided, the value of the <see cref="FailResult.OmitStackTrace"/> property is used
    ///     instead to determine whether to omit the stack trace.</param>
    /// <returns>A <c>Fail</c> result.</returns>
    [StackTraceHidden]
    public static Result<T> Fail(Error? error = null, bool? omitStackTrace = null) => new(error, omitStackTrace);

    /// <summary>
    /// Creates a <c>Fail</c> result.
    /// </summary>
    /// <param name="exception">The exception that caused the failure.</param>
    /// <param name="errorMessage">The error message.</param>
    /// <param name="errorCode">The error code. Default value is <see cref="ErrorCodes.CaughtException"/>.</param>
    /// <param name="errorIdentifier">The optional identifier of the error.</param>
    /// <param name="errorTitle">The optional title for the error. If <see langword="null"/>, then "Error" is used instead.
    ///     </param>
    /// <param name="omitStackTrace">Whether to omit the stack trace of the error to the current location. If
    ///     <see langword="null"/> or not provided, the value of the <see cref="FailResult.OmitStackTrace"/> property is used
    ///     instead to determine whether to omit the stack trace.</param>
    /// <returns>A <c>Fail</c> result.</returns>
    [StackTraceHidden]
    public static Result<T> Fail(
        Exception exception,
        string errorMessage = Error.DefaultFromExceptionMessage,
        int? errorCode = ErrorCodes.CaughtException,
        string? errorIdentifier = null,
        string? errorTitle = null,
        bool? omitStackTrace = null) =>
        Fail(Error.FromException(exception, errorMessage, errorCode, errorIdentifier, errorTitle), omitStackTrace);

    /// <summary>
    /// Creates a <c>Fail</c> result.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    /// <param name="errorCode">The error code. Default value is <see cref="ErrorCodes.InternalServerError"/>.</param>
    /// <param name="errorIdentifier">The optional identifier of the error.</param>
    /// <param name="errorTitle">The optional title for the error. If <see langword="null"/>, then "Error" is used instead.
    ///     </param>
    /// <param name="isSensitive">Whether the error contains sensitive information.</param>
    /// <param name="extensions">Additional properties for the error.</param>
    /// <param name="innerError">The optional error that is the cause of the current error.</param>
    /// <param name="omitStackTrace">Whether to omit the stack trace of the error to the current location. If
    ///     <see langword="null"/> or not provided, the value of the <see cref="FailResult.OmitStackTrace"/> property is used
    ///     instead to determine whether to omit the stack trace.</param>
    /// <returns>A <c>Fail</c> result.</returns>
    [StackTraceHidden]
    public static Result<T> Fail(
        string errorMessage,
        int? errorCode = ErrorCodes.InternalServerError,
        string? errorIdentifier = null,
        string? errorTitle = null,
        bool isSensitive = false,
        IReadOnlyDictionary<string, object>? extensions = null,
        Error? innerError = null,
        bool? omitStackTrace = null) =>
        Fail(
            new Error
            {
                Message = errorMessage,
                Title = errorTitle!,
                Identifier = errorIdentifier,
                ErrorCode = errorCode,
                IsSensitive = isSensitive,
                Extensions = extensions!,
                InnerError = innerError,
            },
            omitStackTrace);

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
