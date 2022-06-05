using static RandomSkunk.Results.Error;

namespace RandomSkunk.Results;

/// <summary>
/// Defines a result without a value.
/// </summary>
[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
public partial struct Result : IEquatable<Result>
{
    /// <summary>
    /// A factory object that creates <c>Fail</c> results of type <see cref="Result"/>.
    /// </summary>
    /// <remarks>
    /// Applications are encouraged to define custom extension methods targeting <see cref="ResultFactory"/> that return
    /// <c>Fail</c> results relevant to the application. For example, an application could define an extension method for
    /// creating a <c>Fail</c> result when a user is not authorized:
    /// <code><![CDATA[
    /// public static Result Unauthorized(this ResultFactory source) =>
    ///     source.Error("User is not authorized.", new StackTrace(1).ToString(), 401);
    /// ]]></code>
    /// This extension method could be used elsewhere in the application like this:
    /// <code><![CDATA[
    /// return Result.FailWith.Unauthorized();
    /// ]]></code>
    /// </remarks>
    public static readonly ResultFactory FailWith = new();

    internal readonly ResultType _type;
    private readonly Error? _error;

    private Result(bool success, Error? error = null)
    {
        if (success)
        {
            _type = ResultType.Success;
            _error = null;
        }
        else
        {
            _type = ResultType.Fail;
            _error = error ?? new Error();
        }
    }

    /// <summary>
    /// Gets the type of the result: <see cref="ResultType.Success"/> or <see cref="ResultType.Fail"/>.
    /// </summary>
    public ResultType Type => _type;

    /// <summary>
    /// Gets a value indicating whether this is a <c>Success</c> result.
    /// </summary>
    /// <returns>
    /// <see langword="true"/> if this is a <c>Success</c> result; otherwise,
    /// <see langword="false"/>.
    /// </returns>
    public bool IsSuccess => _type == ResultType.Success;

    /// <summary>
    /// Gets a value indicating whether this is a <c>Fail</c> result.
    /// </summary>
    /// <returns>
    /// <see langword="true"/> if this is a <c>Fail</c> result; otherwise,
    /// <see langword="false"/>.
    /// </returns>
    public bool IsFail => _type == ResultType.Fail;

    /// <summary>
    /// Gets a value indicating whether this is a default instance of the <see cref="Result"/>
    /// struct.
    /// </summary>
    public bool IsDefault => _type == ResultType.Fail && _error is null;

    /// <summary>
    /// Indicates whether the <paramref name="left"/> parameter is equal to the
    /// <paramref name="right"/> parameter.
    /// </summary>
    /// <param name="left">The left side of the comparison.</param>
    /// <param name="right">The right side of the comparison.</param>
    /// <returns>
    /// <see langword="true"/> if the <paramref name="left"/> parameter is equal to the
    /// <paramref name="right"/> parameter; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool operator ==(Result left, Result right) => left.Equals(right);

    /// <summary>
    /// Indicates whether the <paramref name="left"/> parameter is not equal to the
    /// <paramref name="right"/> parameter.
    /// </summary>
    /// <param name="left">The left side of the comparison.</param>
    /// <param name="right">The right side of the comparison.</param>
    /// <returns>
    /// <see langword="true"/> if the <paramref name="left"/> parameter is not equal to the
    /// <paramref name="right"/> parameter; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool operator !=(Result left, Result right) => !(left == right);

    /// <summary>
    /// Creates a <c>Success</c> result.
    /// </summary>
    /// <returns>A <c>Success</c> result.</returns>
    public static Result Success() => new(success: true);

    /// <summary>
    /// Creates a <c>Fail</c> result with the specified error.
    /// </summary>
    /// <param name="error">
    /// An error that describes the failure. If <see langword="null"/>, a default error is
    /// used.
    /// </param>
    /// <returns>A <c>Fail</c> result.</returns>
    public static Result Fail(Error? error = null) => new(success: false, error);

    /// <summary>
    /// Creates a <c>Fail</c> result.
    /// </summary>
    /// <param name="exception">The exception that caused the failure.</param>
    /// <param name="errorMessage">The optional error message.</param>
    /// <param name="errorCode">The optional error code.</param>
    /// <param name="errorIdentifier">The optional identifier of the error.</param>
    /// <param name="errorType">
    /// The optional type of the error. If <see langword="null"/>, then the
    /// <see cref="MemberInfo.Name"/> of the <see cref="Type"/> of the current instance
    /// is used instead.
    /// </param>
    /// <param name="innerError">
    /// The optional error that is the cause of the current error.
    /// </param>
    /// <returns>A <c>Fail</c> result.</returns>
    public static Result Fail(
        Exception exception,
        string? errorMessage = null,
        int? errorCode = null,
        string? errorIdentifier = null,
        string? errorType = null,
        Error? innerError = null) =>
        Fail(FromException(exception, errorMessage, errorCode, errorIdentifier, errorType, innerError));

    /// <summary>
    /// Creates a <c>Fail</c> result.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    /// <param name="stackTrace">
    /// The optional stack trace. If <see langword="null"/>, then a generated stack trace is used.
    /// </param>
    /// <param name="errorCode">The optional error code.</param>
    /// <param name="errorIdentifier">The optional identifier of the error.</param>
    /// <param name="errorType">
    /// The optional type of the error. If <see langword="null"/>, then the
    /// <see cref="MemberInfo.Name"/> of the <see cref="Type"/> of the current instance
    /// is used instead.
    /// </param>
    /// <param name="innerError">
    /// The optional error that is the cause of the current error.
    /// </param>
    /// <returns>A <c>Fail</c> result.</returns>
    public static Result Fail(
        string errorMessage,
        string? stackTrace = null,
        int? errorCode = null,
        string? errorIdentifier = null,
        string? errorType = null,
        Error? innerError = null) =>
        Fail(new Error(errorMessage, errorType)
        {
            StackTrace = stackTrace ?? new StackTrace(1).ToString(),
            ErrorCode = errorCode,
            Identifier = errorIdentifier,
            InnerError = innerError,
        });

    /// <inheritdoc/>
    public bool Equals(Result other) =>
        _type == other._type
        && (IsSuccess
            || (IsFail && EqualityComparer<Error?>.Default.Equals(_error, other._error)));

    /// <inheritdoc/>
    public override bool Equals(object? obj) =>
        obj is Result result && Equals(result);

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        int hashCode = 1710757158;
        hashCode = (hashCode * -1521134295) + _type.GetHashCode();
        hashCode = (hashCode * -1521134295) + (IsFail ? Error().GetHashCode() : 0);
        return hashCode;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal Error Error() => _error ?? DefaultError;

    private string GetDebuggerDisplay() =>
        Match(
            () => "Success",
            error => $"Fail({error.Type}: \"{error.Message}\")");
}
