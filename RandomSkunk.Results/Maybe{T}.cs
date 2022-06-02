using static RandomSkunk.Results.Error;

namespace RandomSkunk.Results;

/// <summary>
/// Defines a result with an optional value.
/// </summary>
/// <typeparam name="T">The type of the result value.</typeparam>
[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
public partial struct Maybe<T> : IEquatable<Maybe<T>>
{
    /// <summary>
    /// A factory object that creates <c>Fail</c> results of type <see cref="Maybe{T}"/>.
    /// </summary>
    /// <remarks>
    /// Applications are encouraged to define custom extension methods targeting <see cref="MaybeFactory{T}"/> that return
    /// <c>Fail</c> results relevant to the application. For example, an application could define an extension method for
    /// creating a <c>Fail</c> result when a user is not authorized:
    /// <code><![CDATA[
    /// public static Maybe<T> Unauthorized<T>(this MaybeFactory<T> source) =>
    ///     source.Error("User is not authorized.", new StackTrace(1).ToString(), 401);
    /// ]]></code>
    /// This extension method could be used elsewhere in the application like this:
    /// <code><![CDATA[
    /// return Maybe<AdminUser>.FailWith.Unauthorized();
    /// ]]></code>
    /// </remarks>
    public static readonly MaybeFactory<T> FailWith = new();

    internal readonly MaybeType _type;
    internal readonly T? _value;
    private readonly Error? _error;

    private Maybe(T value)
    {
        _type = MaybeType.Some;
        _value = value ?? throw new ArgumentNullException(nameof(value));
        _error = null;
    }

    private Maybe(bool none, Error? error = null)
    {
        if (none)
        {
            _type = MaybeType.None;
            _value = default;
            _error = null;
        }
        else
        {
            _type = MaybeType.Fail;
            _value = default;
            _error = error ?? new Error();
        }
    }

    /// <summary>
    /// Gets the type of the result: <see cref="MaybeType.Some"/>, <see cref="MaybeType.None"/>, or <see cref="MaybeType.Fail"/>.
    /// </summary>
    public MaybeType Type => _type;

    /// <summary>
    /// Gets a value indicating whether this is a <c>Some</c> result.
    /// </summary>
    /// <returns>
    /// <see langword="true"/> if this is a <c>Some</c> result; otherwise,
    /// <see langword="false"/>.
    /// </returns>
    public bool IsSome => _type == MaybeType.Some;

    /// <summary>
    /// Gets a value indicating whether this is a <c>None</c> result.
    /// </summary>
    /// <returns>
    /// <see langword="true"/> if this is a <c>None</c> result; otherwise,
    /// <see langword="false"/>.
    /// </returns>
    public bool IsNone => _type == MaybeType.None;

    /// <summary>
    /// Gets a value indicating whether this is a <c>Fail</c> result.
    /// </summary>
    /// <returns>
    /// <see langword="true"/> if this is a <c>Fail</c> result; otherwise,
    /// <see langword="false"/>.
    /// </returns>
    public bool IsFail => _type == MaybeType.Fail;

    /// <summary>
    /// Gets a value indicating whether this is a default instance of the <see cref="Maybe{T}"/>
    /// struct.
    /// </summary>
    public bool IsDefault => _type == MaybeType.Fail && _error is null;

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
    public static bool operator ==(Maybe<T> left, Maybe<T> right) => left.Equals(right);

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
    public static bool operator !=(Maybe<T> left, Maybe<T> right) => !(left == right);

    /// <summary>
    /// Creates a <c>Some</c> result with the specified value.
    /// </summary>
    /// <param name="value">
    /// The value of the <c>Some</c> result. Must not be <see langword="null"/>.
    /// </param>
    /// <returns>A <c>Some</c> result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="value"/> is <see langword="null"/>.
    /// </exception>
    public static Maybe<T> Some(T value) => new(value);

    /// <summary>
    /// Creates a <c>None</c> result.
    /// </summary>
    /// <returns>A <c>None</c> result.</returns>
    public static Maybe<T> None() => new(none: true);

    /// <summary>
    /// Creates a <c>Fail</c> result with the specified error.
    /// </summary>
    /// <param name="error">
    /// An error that describes the failure. If <see langword="null"/>, a default error is
    /// used.
    /// </param>
    /// <returns>A <c>Fail</c> result.</returns>
    public static Maybe<T> Fail(Error? error = null) => new(none: false, error);

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
    public static Maybe<T> Fail(
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
    /// <param name="stackTrace">The optional stack trace.</param>
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
    public static Maybe<T> Fail(
        string errorMessage,
        string? stackTrace = null,
        int? errorCode = null,
        string? errorIdentifier = null,
        string? errorType = null,
        Error? innerError = null) =>
        Fail(new Error(errorMessage, errorType)
        {
            StackTrace = stackTrace,
            ErrorCode = errorCode,
            Identifier = errorIdentifier,
            InnerError = innerError,
        });

    /// <summary>
    /// Creates a maybe from the specified value.
    /// </summary>
    /// <param name="value">The value. Can be <see langword="null"/>.</param>
    /// <returns>
    /// A <c>Some</c> result if <paramref name="value"/> is not null; otherwise, a <c>None</c>
    /// result.
    /// </returns>
    public static Maybe<T> FromValue(
        T? value) =>
        value is not null
            ? Some(value)
            : None();

    /// <inheritdoc/>
    public bool Equals(Maybe<T> other) =>
        _type == other._type
        && ((IsSome && EqualityComparer<T?>.Default.Equals(_value, other._value))
            || (IsFail && EqualityComparer<Error?>.Default.Equals(_error, other._error))
            || IsNone);

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is Maybe<T> result && Equals(result);

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        int hashCode = 1157318437;
        hashCode = (hashCode * -1521134295) + EqualityComparer<Type>.Default.GetHashCode(typeof(T));
        hashCode = (hashCode * -1521134295) + _type.GetHashCode();
        hashCode = (hashCode * -1521134295) + (IsFail ? Error().GetHashCode() : 0);
        hashCode = (hashCode * -1521134295) + (IsSome ? _value!.GetHashCode() : 0);
        hashCode *= 31;
        return hashCode;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal Error Error() => _error ?? DefaultError;

    private string GetDebuggerDisplay() =>
        Match(
            value => $"Some({(value is string ? $"\"{value}\"" : $"{value}")})",
            () => "None",
            error => $"Fail({error.Type}: \"{error.Message}\")");
}
