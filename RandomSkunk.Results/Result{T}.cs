using RandomSkunk.Results.Unsafe;
using static RandomSkunk.Results.Error;

namespace RandomSkunk.Results;

/// <summary>
/// Defines a result with a required value.
/// </summary>
/// <typeparam name="T">The type of the result value.</typeparam>
public partial struct Result<T> : IResult<T>, IEquatable<Result<T>>
{
    internal readonly Outcome _outcome;
    internal readonly T? _value;
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
        _error = error ?? new Error(setStackTrace: true);
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
    /// Gets a value indicating whether this is a default instance of the <see cref="Result{T}"/> struct.
    /// </summary>
    public bool IsDefault => _outcome == Outcome.Fail && _error is null;

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
    [StackTraceHidden]
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
    [StackTraceHidden]
    public static Result<T> Fail(
        Exception exception,
        string errorMessage = _defaultFromExceptionMessage,
        int? errorCode = ErrorCodes.CaughtException,
        string? errorIdentifier = null,
        string? errorTitle = null) =>
        Fail(FromException(exception, errorMessage, errorCode, errorIdentifier, errorTitle));

    /// <summary>
    /// Creates a <c>Fail</c> result.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    /// <param name="errorCode">The error code. Default value is <see cref="ErrorCodes.InternalServerError"/>.</param>
    /// <param name="errorIdentifier">The optional identifier of the error.</param>
    /// <param name="errorTitle">The optional title for the error. If <see langword="null"/>, then "Error" is used instead.
    ///     </param>
    /// <param name="innerError">The optional error that is the cause of the current error.</param>
    /// <param name="stackTrace">The optional stack trace. If <see langword="null"/>, then a generated stack trace is used.
    ///     </param>
    /// <returns>A <c>Fail</c> result.</returns>
    [StackTraceHidden]
    public static Result<T> Fail(
        string errorMessage,
        int? errorCode = ErrorCodes.InternalServerError,
        string? errorIdentifier = null,
        string? errorTitle = null,
        Error? innerError = null,
        string? stackTrace = null) =>
        Fail(new Error(errorMessage, errorTitle, setStackTrace: stackTrace is null)
        {
            StackTrace = stackTrace,
            ErrorCode = errorCode,
            Identifier = errorIdentifier,
            InnerError = innerError,
        });

    /// <summary>
    /// Creates a <c>Success</c> result with the specified value. If the value is <see langword="null"/>, then a <c>Fail</c>
    /// result with error code <see cref="ErrorCodes.BadRequest"/> is returned instead.
    /// </summary>
    /// <param name="value">The value. Can be <see langword="null"/>.</param>
    /// <returns>A <c>Success</c> result if <paramref name="value"/> is not <see langword="null"/>; otherwise, a <c>Fail</c>
    ///     result with a generated stack trace.</returns>
    public static Result<T> FromValue(T? value) =>
        value is not null
            ? Success(value)
            : Fail(Errors.BadRequest("Result value cannot be null."));

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
        hashCode = (hashCode * -1521134295) + (IsFail ? Error().GetHashCode() : 0);
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
    T IResult<T>.GetSuccessValue() => this.GetValue();

    /// <inheritdoc/>
    Error IResult.GetNonSuccessError() =>
        _outcome switch
        {
            Outcome.Fail => Error(),
            _ => throw Exceptions.CannotAccessErrorUnlessNonSuccess(),
        };

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal Error Error() => _error ?? DefaultError;
}
