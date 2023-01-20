namespace RandomSkunk.Results;

/// <summary>
/// Defines a result without a value.
/// </summary>
/// <content> This struct is partial - additional methods are defined in the code files from the Operations folder. </content>
public readonly partial struct Result : IResult<Unit>, IEquatable<Result>
{
    private readonly Outcome _outcome;
    private readonly Error? _error;

    private Result(bool success, Error? error, bool? omitStackTrace)
    {
        if (success)
        {
            _outcome = Outcome.Success;
            _error = null;
        }
        else
        {
            _outcome = Outcome.Fail;
            _error = error ?? new Error();

            if (_error.StackTrace is null && !(omitStackTrace ?? FailResult.OmitStackTrace))
                _error = _error with { StackTrace = FilteredStackTrace.Create() };

            _error = FailResult.InvokeReplaceErrorIfSet(_error);
            FailResult.InvokeCallbackIfSet(_error);
        }
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
    /// Gets the error from the <c>Fail</c> result.
    /// </summary>
    /// <returns>If this is a <c>Fail</c> result, its error; otherwise throws an <see cref="InvalidStateException"/>.</returns>
    /// <exception cref="InvalidStateException">If the result is not a <c>Fail</c> result.</exception>
    public Error Error =>
        _outcome == Outcome.Fail
            ? GetError()
            : throw Exceptions.CannotAccessErrorUnlessFail();

    /// <inheritdoc/>
    Unit IResult<Unit>.Value =>
        _outcome == Outcome.Success
            ? Unit.Value
            : throw Exceptions.CannotAccessValueUnlessSuccess(GetError());

    /// <summary>
    /// Truncates the <see cref="Maybe{T}"/> of type <see cref="Unit"/> into an equivalent <see cref="Result"/>. If it is a
    /// <c>Success</c> result, then its value is ignored and a valueless <c>Success</c> result is returned. If it is a
    /// <c>None</c> result, then a <c>Fail</c> result with error code <see cref="ErrorCodes.NoValue"/> is returned. Otherwise, a
    /// <c>Fail</c> result with the same error as is returned.
    /// </summary>
    /// <param name="sourceResult">The source result.</param>
    /// <returns>An equivalent <see cref="Result"/>.</returns>
    public static implicit operator Result(Result<Unit> sourceResult) => sourceResult.Truncate();

    /// <summary>
    /// Truncates the <see cref="Maybe{T}"/> of type <see cref="Unit"/> into an equivalent <see cref="Result"/>. If it is a
    /// <c>Success</c> result, then its value is ignored and a valueless <c>Success</c> result is returned. If it is a
    /// <c>None</c> result, then a <c>Fail</c> result with error code <see cref="ErrorCodes.NoValue"/> is returned. Otherwise, a
    /// <c>Fail</c> result with the same error as is returned.
    /// </summary>
    /// <param name="sourceResult">The source result.</param>
    /// <returns>An equivalent <see cref="Result"/>.</returns>
    public static implicit operator Result(Maybe<Unit> sourceResult) => sourceResult.Truncate();

    /// <summary>
    /// Indicates whether the <paramref name="left"/> parameter is equal to the <paramref name="right"/> parameter.
    /// </summary>
    /// <param name="left">The left side of the comparison.</param>
    /// <param name="right">The right side of the comparison.</param>
    /// <returns><see langword="true"/> if the <paramref name="left"/> parameter is equal to the <paramref name="right"/>
    ///     parameter; otherwise, <see langword="false"/>.</returns>
    public static bool operator ==(Result left, Result right) => left.Equals(right);

    /// <summary>
    /// Indicates whether the <paramref name="left"/> parameter is not equal to the <paramref name="right"/> parameter.
    /// </summary>
    /// <param name="left">The left side of the comparison.</param>
    /// <param name="right">The right side of the comparison.</param>
    /// <returns><see langword="true"/> if the <paramref name="left"/> parameter is not equal to the <paramref name="right"/>
    ///     parameter; otherwise, <see langword="false"/>.</returns>
    public static bool operator !=(Result left, Result right) => !(left == right);

    /// <summary>
    /// Creates a <c>Success</c> result.
    /// </summary>
    /// <returns>A <c>Success</c> result.</returns>
    public static Result Success() => new(success: true, null, null);

    /// <summary>
    /// Creates a <c>Fail</c> result with the specified error.
    /// </summary>
    /// <param name="error">An error that describes the failure. If <see langword="null"/>, a default error is used.</param>
    /// <param name="omitStackTrace">Whether to omit the stack trace of the error to the current location. If
    ///     <see langword="null"/> or not provided, the value of the <see cref="FailResult.OmitStackTrace"/> property is used
    ///     instead to determine whether to omit the stack trace.</param>
    /// <returns>A <c>Fail</c> result.</returns>
    [StackTraceHidden]
    public static Result Fail(Error? error = null, bool? omitStackTrace = null) => new(success: false, error, omitStackTrace);

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
    public static Result Fail(
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
    public static Result Fail(
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

    /// <inheritdoc/>
    public bool Equals(Result other) =>
        _outcome == other._outcome
        && (IsSuccess
            || (IsFail && EqualityComparer<Error?>.Default.Equals(_error, other._error)));

    /// <inheritdoc/>
    public override bool Equals(object? obj) =>
        obj is Result result && Equals(result);

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        int hashCode = 1710757158;
        hashCode = (hashCode * -1521134295) + _outcome.GetHashCode();
        hashCode = (hashCode * -1521134295) + (IsFail ? GetError().GetHashCode() : 0);
        return hashCode;
    }

    /// <inheritdoc/>
    public override string ToString() =>
        Match(
            () => "Success",
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
