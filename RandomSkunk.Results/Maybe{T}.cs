namespace RandomSkunk.Results;

/// <summary>
/// Defines a result with an optional value.
/// </summary>
/// <typeparam name="T">The type of the source result value.</typeparam>
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

    /// <summary>
    /// Gets the return value of the successful operation, or throws an
    /// <see cref="InvalidStateException"/> if this is not a <c>Some</c> result.
    /// </summary>
    /// <exception cref="InvalidStateException">
    /// If this result is not a <c>Some</c> result.
    /// </exception>
    [NotNull]
    public T Value =>
        IsSome
            ? _value!
            : throw Exceptions.CannotAccessValueUnlessSome;

    /// <summary>
    /// Gets the error from the failed operation, or throws an
    /// <see cref="InvalidStateException"/> if this is not a <c>Fail</c> result.
    /// </summary>
    /// <exception cref="InvalidStateException">
    /// If this result is not a <c>Fail</c> result.
    /// </exception>
    public Error Error =>
        IsFail
            ? _error ?? Error.DefaultError
            : throw Exceptions.CannotAccessErrorUnlessFail;

    /// <summary>
    /// Converts the specified value to a maybe.
    /// </summary>
    /// <param name="value">The value.</param>
    public static implicit operator Maybe<T>(T? value) => Create.FromValue(value);

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
    /// Evaluates either the <paramref name="some"/>, <paramref name="none"/>, or
    /// <paramref name="fail"/> function depending on whether the result type is <c>Some</c>,
    /// <c>None</c>, or <c>Fail</c>.
    /// </summary>
    /// <typeparam name="TReturn">The return type of the functions.</typeparam>
    /// <param name="some">
    /// The function to evaluate if the result type is <c>Some</c>. The value of the
    /// <c>Some</c> result is passed to this function.
    /// </param>
    /// <param name="none">
    /// The function to evaluate if the result type is <c>None</c>.
    /// </param>
    /// <param name="fail">
    /// The function to evaluate if the result type is <c>Fail</c>. The error message and error
    /// code of the <c>Fail</c> result are passed to this function.
    /// </param>
    /// <returns>The result of the matching function evaluation.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="some"/> is <see langword="null"/>, or if <paramref name="none"/> is
    /// <see langword="null"/>, or if <paramref name="fail"/> is <see langword="null"/>.
    /// </exception>
    public TReturn Match<TReturn>(
        Func<T, TReturn> some,
        Func<TReturn> none,
        Func<Error, TReturn> fail)
    {
        if (some is null) throw new ArgumentNullException(nameof(some));
        if (none is null) throw new ArgumentNullException(nameof(none));
        if (fail is null) throw new ArgumentNullException(nameof(fail));

        return _type switch
        {
            MaybeType.Some => some(_value!),
            MaybeType.None => none(),
            _ => fail(_error!),
        };
    }

    /// <summary>
    /// Evaluates either the <paramref name="some"/>, <paramref name="none"/>, or
    /// <paramref name="fail"/> function depending on whether the result type is <c>Some</c>,
    /// <c>None</c>, or <c>Fail</c>.
    /// </summary>
    /// <param name="some">
    /// The function to evaluate if the result type is <c>Some</c>. The value of the
    /// <c>Some</c> result is passed to this function.
    /// </param>
    /// <param name="none">
    /// The function to evaluate if the result type is <c>None</c>.
    /// </param>
    /// <param name="fail">
    /// The function to evaluate if the result type is <c>Fail</c>. The error message and error
    /// code of the <c>Fail</c> result are passed to this function.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="some"/> is <see langword="null"/>, or if <paramref name="none"/> is
    /// <see langword="null"/>, or if <paramref name="fail"/> is <see langword="null"/>.
    /// </exception>
    public void Match(
        Action<T> some,
        Action none,
        Action<Error> fail)
    {
        if (some is null) throw new ArgumentNullException(nameof(some));
        if (none is null) throw new ArgumentNullException(nameof(none));
        if (fail is null) throw new ArgumentNullException(nameof(fail));

        if (_type == MaybeType.Some)
            some(_value!);
        else if (_type == MaybeType.None)
            none();
        else
            fail(_error!);
    }

    /// <summary>
    /// Evaluates either the <paramref name="some"/>, <paramref name="none"/>, or
    /// <paramref name="fail"/> function depending on whether the result type is <c>Some</c>,
    /// <c>None</c>, or <c>Fail</c>.
    /// </summary>
    /// <typeparam name="TReturn">The return type of the functions.</typeparam>
    /// <param name="some">
    /// The function to evaluate if the result type is <c>Some</c>. The value of the
    /// <c>Some</c> result is passed to this function.
    /// </param>
    /// <param name="none">
    /// The function to evaluate if the result type is <c>None</c>.
    /// </param>
    /// <param name="fail">
    /// The function to evaluate if the result type is <c>Fail</c>. The error message and error
    /// code of the <c>Fail</c> result are passed to this function.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous match operation, which wraps the result of the
    /// matching function evaluation.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="some"/> is <see langword="null"/>, or if <paramref name="none"/> is
    /// <see langword="null"/>, or if <paramref name="fail"/> is <see langword="null"/>.
    /// </exception>
    public Task<TReturn> MatchAsync<TReturn>(
        Func<T, Task<TReturn>> some,
        Func<Task<TReturn>> none,
        Func<Error, Task<TReturn>> fail)
    {
        if (some is null) throw new ArgumentNullException(nameof(some));
        if (none is null) throw new ArgumentNullException(nameof(none));
        if (fail is null) throw new ArgumentNullException(nameof(fail));

        return _type switch
        {
            MaybeType.Some => some(_value!),
            MaybeType.None => none(),
            _ => fail(_error!),
        };
    }

    /// <summary>
    /// Evaluates either the <paramref name="some"/>, <paramref name="none"/>, or
    /// <paramref name="fail"/> function depending on whether the result type is <c>Some</c>,
    /// <c>None</c>, or <c>Fail</c>.
    /// </summary>
    /// <param name="some">
    /// The function to evaluate if the result type is <c>Some</c>. The value of the
    /// <c>Some</c> result is passed to this function.
    /// </param>
    /// <param name="none">
    /// The function to evaluate if the result type is <c>None</c>.
    /// </param>
    /// <param name="fail">
    /// The function to evaluate if the result type is <c>Fail</c>. The error message and error
    /// code of the <c>Fail</c> result are passed to this function.
    /// </param>
    /// <returns>A task representing the asynchronous match operation.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="some"/> is <see langword="null"/>, or if <paramref name="none"/> is
    /// <see langword="null"/>, or if <paramref name="fail"/> is <see langword="null"/>.
    /// </exception>
    public Task MatchAsync(
        Func<T, Task> some,
        Func<Task> none,
        Func<Error, Task> fail)
    {
        if (some is null) throw new ArgumentNullException(nameof(some));
        if (none is null) throw new ArgumentNullException(nameof(none));
        if (fail is null) throw new ArgumentNullException(nameof(fail));

        return _type switch
        {
            MaybeType.Some => some(_value!),
            MaybeType.None => none(),
            _ => fail(_error!),
        };
    }

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
