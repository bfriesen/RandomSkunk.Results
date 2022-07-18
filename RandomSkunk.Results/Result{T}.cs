using RandomSkunk.Results.Unsafe;
using static RandomSkunk.Results.Error;

namespace RandomSkunk.Results;

/// <summary>
/// Defines a result with a required value.
/// </summary>
/// <typeparam name="T">The type of the result value.</typeparam>
public partial struct Result<T> : IResult<T>, IEquatable<Result<T>>
{
    /// <summary>
    /// A factory object that creates <c>Fail</c> results of type <see cref="Result{T}"/>.
    /// </summary>
    /// <remarks>
    /// Applications are encouraged to define custom extension methods targeting <see cref="FailFactory{TResult}"/> that return
    /// <c>Fail</c> results relevant to the application. For example, an application could define an extension method for
    /// creating a <c>Fail</c> result when a user is not authorized:
    /// <code><![CDATA[
    /// public static TResult Unauthorized<TResult>(this FailFactory<TResult> failWith) =>
    ///     failWith.Error("User is not authorized.", 401);
    /// ]]></code>
    /// To use:
    /// <code><![CDATA[
    /// return Result<AdminUser>.FailWith.Unauthorized();
    /// ]]></code>
    /// </remarks>
    public static readonly FailFactory<Result<T>> FailWith = new FailFactory();

    internal readonly ResultType _type;
    internal readonly T? _value;
    private readonly Error? _error;

    private Result(T value)
    {
        _type = ResultType.Success;
        _value = value ?? throw new ArgumentNullException(nameof(value));
        _error = null;
    }

    private Result(Error? error)
    {
        _type = ResultType.Fail;
        _value = default;
        _error = error ?? new Error(setStackTrace: true);
    }

    /// <summary>
    /// Gets the type of the result: <see cref="ResultType.Success"/> or <see cref="ResultType.Fail"/>.
    /// </summary>
    public ResultType Type => _type;

    /// <summary>
    /// Gets a value indicating whether this is a <c>Success</c> result.
    /// </summary>
    /// <returns><see langword="true"/> if this is a <c>Success</c> result; otherwise, <see langword="false"/>.</returns>
    public bool IsSuccess => _type == ResultType.Success;

    /// <summary>
    /// Gets a value indicating whether this is a <c>Fail</c> result.
    /// </summary>
    /// <returns><see langword="true"/> if this is a <c>Fail</c> result; otherwise, <see langword="false"/>.</returns>
    public bool IsFail => _type == ResultType.Fail;

    /// <summary>
    /// Gets a value indicating whether this is a default instance of the <see cref="Result{T}"/> struct.
    /// </summary>
    public bool IsDefault => _type == ResultType.Fail && _error is null;

    /// <summary>
    /// Indicates whether the <paramref name="left"/> parameter is equal to the <paramref name="right"/> parameter.
    /// </summary>
    /// <param name="left">The left side of the comparison.</param>
    /// <param name="right">The right side of the comparison.</param>
    /// <returns><see langword="true"/> if the <paramref name="left"/> parameter is equal to the <paramref name="right"/>
    /// parameter; otherwise, <see langword="false"/>.</returns>
    public static bool operator ==(Result<T> left, Result<T> right) => left.Equals(right);

    /// <summary>
    /// Indicates whether the <paramref name="left"/> parameter is not equal to the <paramref name="right"/> parameter.
    /// </summary>
    /// <param name="left">The left side of the comparison.</param>
    /// <param name="right">The right side of the comparison.</param>
    /// <returns><see langword="true"/> if the <paramref name="left"/> parameter is not equal to the <paramref name="right"/>
    /// parameter; otherwise, <see langword="false"/>.</returns>
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
    /// <param name="errorMessage">The optional error message.</param>
    /// <param name="errorCode">The optional error code.</param>
    /// <param name="errorIdentifier">The optional identifier of the error.</param>
    /// <returns>A <c>Fail</c> result.</returns>
    public static Result<T> Fail(
        Exception exception,
        string? errorMessage = null,
        int? errorCode = null,
        string? errorIdentifier = null) =>
        Fail(FromException(exception, errorMessage, errorCode, errorIdentifier));

    /// <summary>
    /// Creates a <c>Fail</c> result.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    /// <param name="errorCode">The optional error code.</param>
    /// <param name="errorIdentifier">The optional identifier of the error.</param>
    /// <param name="errorType">The optional type of the error. If <see langword="null"/>, then the <see cref="MemberInfo.Name"/>
    /// of the <see cref="Type"/> of the current instance is used instead.</param>
    /// <param name="innerError">The optional error that is the cause of the current error.</param>
    /// <param name="stackTrace">The optional stack trace. If <see langword="null"/>, then a generated stack trace is used.
    /// </param>
    /// <returns>A <c>Fail</c> result.</returns>
    public static Result<T> Fail(
        string errorMessage,
        int? errorCode = null,
        string? errorIdentifier = null,
        string? errorType = null,
        Error? innerError = null,
        string? stackTrace = null) =>
        Fail(new Error(errorMessage, errorType, setStackTrace: stackTrace is null)
        {
            StackTrace = stackTrace,

            ErrorCode = errorCode,
            Identifier = errorIdentifier,
            InnerError = innerError,
        });

    /// <summary>
    /// Creates a <c>Success</c> result with the specified non-null value. If the value is <see langword="null"/>, then a
    /// <c>Fail</c> result is returned instead.
    /// </summary>
    /// <param name="value">The value. Can be <see langword="null"/>.</param>
    /// <param name="getNullValueError">An optional function that creates the <see cref="Error"/> of the <c>Fail</c> result when
    /// the <paramref name="value"/> parameter is <see langword="null"/>. If <see langword="null"/>, a function that returns
    /// an error with message "Value cannot be null." and error code <see cref="ErrorCodes.BadRequest"/> is used instead.
    /// </param>
    /// <returns>A <c>Success</c> result if <paramref name="value"/> is not <see langword="null"/>; otherwise, a <c>Fail</c>
    /// result with a generated stack trace.</returns>
    public static Result<T> FromValue(T? value, Func<Error>? getNullValueError = null) =>
        value is not null
            ? Success(value)
            : Fail(getNullValueError is not null
                ? getNullValueError()
                : new("Value cannot be null.", setStackTrace: true) { ErrorCode = ErrorCodes.BadRequest });

    /// <inheritdoc/>
    public bool Equals(Result<T> other) =>
        _type == other._type
        && ((IsSuccess && EqualityComparer<T?>.Default.Equals(_value, other._value))
            || (IsFail && EqualityComparer<Error?>.Default.Equals(_error, other._error)));

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is Result<T> result && Equals(result);

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        int hashCode = 1157318437;
        hashCode = (hashCode * -1521134295) + EqualityComparer<Type>.Default.GetHashCode(typeof(T));
        hashCode = (hashCode * -1521134295) + _type.GetHashCode();
        hashCode = (hashCode * -1521134295) + (IsFail ? Error().GetHashCode() : 0);
        hashCode = (hashCode * -1521134295) + (IsSuccess ? _value!.GetHashCode() : 0);
        hashCode *= 29;
        return hashCode;
    }

    /// <inheritdoc/>
    public override string ToString() =>
        Match(
            value => $"Success({(value is string ? $"\"{value}\"" : $"{value}")})",
            error => $"Fail({error.Type}: \"{error.Message}\")");

    /// <inheritdoc/>
    T IResult<T>.GetSuccessValue() => this.GetValue();

    /// <inheritdoc/>
    object IResult.GetSuccessValue() => this.GetValue();

    /// <inheritdoc/>
    Error IResult.GetNonSuccessError(Func<Error>? getNoneError) =>
        _type switch
        {
            ResultType.Fail => Error(),
            _ => throw Exceptions.CannotAccessErrorUnlessNonSuccess(),
        };

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal Error Error() => _error ?? DefaultError;

    private class FailFactory : FailFactory<Result<T>>
    {
        public override Result<T> Error(Error error) => Result<T>.Fail(error);
    }
}
