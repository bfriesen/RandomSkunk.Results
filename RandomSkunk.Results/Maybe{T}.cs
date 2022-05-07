namespace RandomSkunk.Results;

/// <summary>
/// Defines a result with an optional value.
/// </summary>
/// <typeparam name="T">The type of the result value.</typeparam>
/// <remarks>
/// Use <see cref="Create"/> to create instances of this type.
/// </remarks>
public struct Maybe<T> : IEquatable<Maybe<T>>
{
    /// <summary>
    /// The factory object used to create instances of <see cref="Maybe{T}"/>. This field is
    /// read-only.
    /// </summary>
    public static readonly IMaybeFactory<T> Create = new Factory();

    private readonly MaybeType _type;
    private readonly T? _value;
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
    /// Gets the type of the result: <see cref="MaybeType.Some"/>,
    /// <see cref="MaybeType.None"/>, or <see cref="MaybeType.Fail"/>.
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

    [NotNull]
    internal T Value => _value!;

    internal Error Error => _error ?? Error.DefaultError;

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

    private sealed class Factory : IMaybeFactory<T>
    {
        public Maybe<T> Some(T value) => new(value);

        public Maybe<T> None() => new(none: true);

        public Maybe<T> Fail(Error? error = null) => new(none: false, error);
    }
}
