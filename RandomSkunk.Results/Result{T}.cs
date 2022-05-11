using static RandomSkunk.Results.ResultType;

namespace RandomSkunk.Results;

/// <summary>
/// Defines a result with a required value.
/// </summary>
/// <typeparam name="T">The type of the result value.</typeparam>
/// <remarks>
/// Use <see cref="Create"/> to create instances of this type.
/// </remarks>
public partial struct Result<T> : IEquatable<Result<T>>
{
    /// <summary>
    /// The factory object used to create instances of <see cref="Result{T}"/>. This field is
    /// read-only.
    /// </summary>
    public static readonly IResultFactory<T> Create = new Factory();

    internal readonly ResultType _type;
    internal readonly T? _value;
    private readonly Error? _error;

    private Result(T value)
    {
        _type = Success;
        _value = value ?? throw new ArgumentNullException(nameof(value));
        _error = null;
    }

    private Result(Error? error)
    {
        _type = Fail;
        _value = default;
        _error = error ?? new Error();
    }

    /// <summary>
    /// Gets the type of the result: <see cref="Success"/> or <see cref="Fail"/>.
    /// </summary>
    public ResultType Type => _type;

    /// <summary>
    /// Gets a value indicating whether this is a <c>Success</c> result.
    /// </summary>
    /// <returns>
    /// <see langword="true"/> if this is a <c>Success</c> result; otherwise,
    /// <see langword="false"/>.
    /// </returns>
    public bool IsSuccess => _type == Success;

    /// <summary>
    /// Gets a value indicating whether this is a <c>Fail</c> result.
    /// </summary>
    /// <returns>
    /// <see langword="true"/> if this is a <c>Fail</c> result; otherwise,
    /// <see langword="false"/>.
    /// </returns>
    public bool IsFail => _type == Fail;

    /// <summary>
    /// Gets a value indicating whether this is a default instance of the <see cref="Result{T}"/>
    /// struct.
    /// </summary>
    public bool IsDefault => _type == Fail && _error is null;

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
    public static bool operator ==(Result<T> left, Result<T> right) => left.Equals(right);

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
    public static bool operator !=(Result<T> left, Result<T> right) => !(left == right);

    /// <inheritdoc/>
    public bool Equals(Result<T> other) =>
        EqualityComparer<T?>.Default.Equals(_value, other._value)
        && EqualityComparer<Error?>.Default.Equals(_error, other._error);

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is Result<T> result && Equals(result);

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        int hashCode = 1157318437;
        hashCode = (hashCode * -1521134295) + _type.GetHashCode();
        hashCode = (hashCode * -1521134295) + (_error is null ? 0 : _error.GetHashCode());
        hashCode = (hashCode * -1521134295) + (_value is null ? 0 : _value.GetHashCode());
        hashCode *= 29;
        return hashCode;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal Error Error() => _error ?? DefaultError;

    private sealed class Factory : IResultFactory<T>
    {
        public Result<T> Success(T value) => new(value);

        public Result<T> Fail(Error? error = null) => new(error);
    }
}
