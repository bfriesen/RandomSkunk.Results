using static RandomSkunk.Results.MaybeType;

namespace RandomSkunk.Results;

/// <summary>
/// Defines a result with an optional value.
/// </summary>
/// <typeparam name="T">The type of the result value.</typeparam>
/// <remarks>
/// Use <see cref="Create"/> to create instances of this type.
/// </remarks>
public partial struct Maybe<T> : IEquatable<Maybe<T>>
{
    /// <summary>
    /// The factory object used to create instances of <see cref="Maybe{T}"/>. This field is
    /// read-only.
    /// </summary>
    public static readonly IMaybeFactory<T> Create = new Factory();

    internal readonly MaybeType _type;
    internal readonly T? _value;
    private readonly Error? _error;

    private Maybe(T value)
    {
        _type = Some;
        _value = value ?? throw new ArgumentNullException(nameof(value));
        _error = null;
    }

    private Maybe(bool none, Error? error = null)
    {
        if (none)
        {
            _type = None;
            _value = default;
            _error = null;
        }
        else
        {
            _type = Fail;
            _value = default;
            _error = error ?? new Error();
        }
    }

    /// <summary>
    /// Gets the type of the result: <see cref="Some"/>, <see cref="None"/>, or <see cref="Fail"/>.
    /// </summary>
    public MaybeType Type => _type;

    /// <summary>
    /// Gets a value indicating whether this is a <c>Some</c> result.
    /// </summary>
    /// <returns>
    /// <see langword="true"/> if this is a <c>Some</c> result; otherwise,
    /// <see langword="false"/>.
    /// </returns>
    public bool IsSome => _type == Some;

    /// <summary>
    /// Gets a value indicating whether this is a <c>None</c> result.
    /// </summary>
    /// <returns>
    /// <see langword="true"/> if this is a <c>None</c> result; otherwise,
    /// <see langword="false"/>.
    /// </returns>
    public bool IsNone => _type == None;

    /// <summary>
    /// Gets a value indicating whether this is a <c>Fail</c> result.
    /// </summary>
    /// <returns>
    /// <see langword="true"/> if this is a <c>Fail</c> result; otherwise,
    /// <see langword="false"/>.
    /// </returns>
    public bool IsFail => _type == Fail;

    /// <summary>
    /// Gets a value indicating whether this is a default instance of the <see cref="Maybe{T}"/>
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

    /// <inheritdoc/>
    public bool Equals(Maybe<T> other) =>
        EqualityComparer<T?>.Default.Equals(_value, other._value)
        && EqualityComparer<Error?>.Default.Equals(_error, other._error);

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is Maybe<T> result && Equals(result);

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        int hashCode = 1157318437;
        hashCode = (hashCode * -1521134295) + _type.GetHashCode();
        hashCode = (hashCode * -1521134295) + (_error is null ? 0 : _error.GetHashCode());
        hashCode = (hashCode * -1521134295) + (_value is null ? 0 : _value.GetHashCode());
        hashCode *= 31;
        return hashCode;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal Error Error() => _error ?? DefaultError;

    private sealed class Factory : IMaybeFactory<T>
    {
        public Maybe<T> Some(T value) => new(value);

        public Maybe<T> None() => new(none: true);

        public Maybe<T> Fail(Error? error = null) => new(none: false, error);
    }
}
