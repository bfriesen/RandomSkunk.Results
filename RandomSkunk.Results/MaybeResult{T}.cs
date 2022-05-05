namespace RandomSkunk.Results;

/// <summary>
/// The result of an operation that has a return value where that return value is allowed to be
/// absent.
/// </summary>
/// <typeparam name="T">The return type of the operation.</typeparam>
public struct MaybeResult<T> : IEquatable<MaybeResult<T>>
{
    private readonly T? _value;
    private readonly Error? _error;
    private readonly MaybeResultType _type;

    private MaybeResult([DisallowNull] T value)
    {
        _value = value;
        _error = null;
        _type = MaybeResultType.Some;
    }

    private MaybeResult(bool none, Error? error = null)
    {
        if (none)
        {
            _value = default;
            _error = null;
            _type = MaybeResultType.None;
        }
        else
        {
            _value = default;
            _error = error ?? new Error();
            _type = MaybeResultType.Fail;
        }
    }

    /// <summary>
    /// Gets the return value of the successful operation, or throws an
    /// <see cref="InvalidOperationException"/> if this is not a <c>some</c> result.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// If this result is not a <c>some</c> result.
    /// </exception>
    [NotNull]
    public T Value =>
        IsSome
            ? _value!
            : throw Exceptions.CannotAccessValueUnlessSome;

    /// <summary>
    /// Gets the error from the failed operation, or throws an
    /// <see cref="InvalidOperationException"/> if this is not a <c>fail</c> result.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// If this result is not a <c>fail</c> result.
    /// </exception>
    public Error Error =>
        IsFail
            ? _error ?? Error.Default
            : throw Exceptions.CannotAccessErrorUnlessFail;

    /// <summary>
    /// Gets the type of the result: <see cref="MaybeResultType.Some"/>,
    /// <see cref="MaybeResultType.None"/>, or <see cref="MaybeResultType.Fail"/>.
    /// </summary>
    public MaybeResultType Type => _type;

    /// <summary>
    /// Gets a value indicating whether this is a <c>some</c> result.
    /// </summary>
    /// <returns>
    /// <see langword="true"/> if this is a <c>some</c> result; otherwise,
    /// <see langword="false"/>.
    /// </returns>
    public bool IsSome => _type == MaybeResultType.Some;

    /// <summary>
    /// Gets a value indicating whether this is a <c>none</c> result.
    /// </summary>
    /// <returns>
    /// <see langword="true"/> if this is a <c>none</c> result; otherwise,
    /// <see langword="false"/>.
    /// </returns>
    public bool IsNone => _type == MaybeResultType.None;

    /// <summary>
    /// Gets a value indicating whether this is a <c>fail</c> result.
    /// </summary>
    /// <returns>
    /// <see langword="true"/> if this is a <c>fail</c> result; otherwise,
    /// <see langword="false"/>.
    /// </returns>
    public bool IsFail => _type == MaybeResultType.Fail;

    /// <summary>
    /// Converts the specified value to a <c>some</c> result.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="value"/> is <see langword="null"/>.
    /// </exception>
    public static implicit operator MaybeResult<T>([DisallowNull] T value) => Some(value);

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
    public static bool operator ==(MaybeResult<T> left, MaybeResult<T> right) => left.Equals(right);

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
    public static bool operator !=(MaybeResult<T> left, MaybeResult<T> right) => !(left == right);

    /// <summary>
    /// Creates a <c>some</c> result for an operation with an optional return value.
    /// </summary>
    /// <param name="value">
    /// The value of the <c>some</c> result. Must not be <see langword="null"/>.
    /// </param>
    /// <returns>A <c>some</c> result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="value"/> is <see langword="null"/>.
    /// </exception>
    public static MaybeResult<T> Some([DisallowNull] T value) => new(value);

    /// <summary>
    /// Creates a <c>none</c> result for an operation with an optional return value.
    /// </summary>
    /// <returns>A <c>none</c> result.</returns>
    public static MaybeResult<T> None() => new(none: true);

    /// <summary>
    /// Creates a <c>fail</c> result for an operation with an optional return value.
    /// </summary>
    /// <param name="error">The optional error that describes the failure.</param>
    /// <returns>A <c>fail</c> result.</returns>
    public static MaybeResult<T> Fail(Error? error = null) => new(none: false, error);

    /// <summary>
    /// Creates a <c>fail</c> result for an operation with an optional return value.
    /// </summary>
    /// <param name="exception">The exception that caused the failure.</param>
    /// <param name="errorMessage">The optional error message.</param>
    /// <param name="errorCode">The optional error code.</param>
    /// <param name="identifier">The optional identifier of the error.</param>
    /// <returns>A <c>fail</c> result.</returns>
    public static MaybeResult<T> Fail(
        Exception exception,
        string? errorMessage = null,
        int? errorCode = null,
        string? identifier = null) =>
        Fail(Error.FromException(exception, errorMessage, errorCode, identifier));

    /// <summary>
    /// Creates a <c>fail</c> result for an operation with an optional return value.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    /// <param name="stackTrace">The optional stack trace.</param>
    /// <param name="errorCode">The optional error code.</param>
    /// <param name="identifier">The optional identifier of the error.</param>
    /// <returns>A <c>fail</c> result.</returns>
    public static MaybeResult<T> Fail(
        string errorMessage,
        string? stackTrace = null,
        int? errorCode = null,
        string? identifier = null) =>
        Fail(new Error(errorMessage, stackTrace, errorCode, identifier));

    /// <summary>
    /// Evaluates either the <paramref name="some"/>, <paramref name="none"/>, or
    /// <paramref name="fail"/> function depending on whether the result type is <c>some</c>,
    /// <c>none</c>, or <c>fail</c>.
    /// </summary>
    /// <typeparam name="TResult">The return type of the functions.</typeparam>
    /// <param name="some">
    /// The function to evaluate if the result type is <c>some</c>. The value of the
    /// <c>some</c> result is passed to this function.
    /// </param>
    /// <param name="none">
    /// The function to evaluate if the result type is <c>none</c>.
    /// </param>
    /// <param name="fail">
    /// The function to evaluate if the result type is <c>fail</c>. The error message and error
    /// code of the <c>fail</c> result are passed to this function.
    /// </param>
    /// <returns>The result of the matching function evaluation.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="some"/> is <see langword="null"/>, or if <paramref name="none"/> is
    /// <see langword="null"/>, or if <paramref name="fail"/> is <see langword="null"/>.
    /// </exception>
    public TResult Match<TResult>(
        Func<T, TResult> some,
        Func<TResult> none,
        Func<Error, TResult> fail)
    {
        if (some is null) throw new ArgumentNullException(nameof(some));
        if (none is null) throw new ArgumentNullException(nameof(none));
        if (fail is null) throw new ArgumentNullException(nameof(fail));

        return _type switch
        {
            MaybeResultType.Some => some(_value!),
            MaybeResultType.None => none(),
            _ => fail(_error!),
        };
    }

    /// <summary>
    /// Evaluates either the <paramref name="some"/>, <paramref name="none"/>, or
    /// <paramref name="fail"/> function depending on whether the result type is <c>some</c>,
    /// <c>none</c>, or <c>fail</c>.
    /// </summary>
    /// <param name="some">
    /// The function to evaluate if the result type is <c>some</c>. The value of the
    /// <c>some</c> result is passed to this function.
    /// </param>
    /// <param name="none">
    /// The function to evaluate if the result type is <c>none</c>.
    /// </param>
    /// <param name="fail">
    /// The function to evaluate if the result type is <c>fail</c>. The error message and error
    /// code of the <c>fail</c> result are passed to this function.
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

        if (_type == MaybeResultType.Some)
            some(_value!);
        else if (_type == MaybeResultType.None)
            none();
        else
            fail(_error!);
    }

    /// <summary>
    /// Evaluates either the <paramref name="some"/>, <paramref name="none"/>, or
    /// <paramref name="fail"/> function depending on whether the result type is <c>some</c>,
    /// <c>none</c>, or <c>fail</c>.
    /// </summary>
    /// <typeparam name="TResult">The return type of the functions.</typeparam>
    /// <param name="some">
    /// The function to evaluate if the result type is <c>some</c>. The value of the
    /// <c>some</c> result is passed to this function.
    /// </param>
    /// <param name="none">
    /// The function to evaluate if the result type is <c>none</c>.
    /// </param>
    /// <param name="fail">
    /// The function to evaluate if the result type is <c>fail</c>. The error message and error
    /// code of the <c>fail</c> result are passed to this function.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous match operation, which wraps the result of the
    /// matching function evaluation.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="some"/> is <see langword="null"/>, or if <paramref name="none"/> is
    /// <see langword="null"/>, or if <paramref name="fail"/> is <see langword="null"/>.
    /// </exception>
    public Task<TResult> MatchAsync<TResult>(
        Func<T, Task<TResult>> some,
        Func<Task<TResult>> none,
        Func<Error, Task<TResult>> fail)
    {
        if (some is null) throw new ArgumentNullException(nameof(some));
        if (none is null) throw new ArgumentNullException(nameof(none));
        if (fail is null) throw new ArgumentNullException(nameof(fail));

        return _type switch
        {
            MaybeResultType.Some => some(_value!),
            MaybeResultType.None => none(),
            _ => fail(_error!),
        };
    }

    /// <summary>
    /// Evaluates either the <paramref name="some"/>, <paramref name="none"/>, or
    /// <paramref name="fail"/> function depending on whether the result type is <c>some</c>,
    /// <c>none</c>, or <c>fail</c>.
    /// </summary>
    /// <param name="some">
    /// The function to evaluate if the result type is <c>some</c>. The value of the
    /// <c>some</c> result is passed to this function.
    /// </param>
    /// <param name="none">
    /// The function to evaluate if the result type is <c>none</c>.
    /// </param>
    /// <param name="fail">
    /// The function to evaluate if the result type is <c>fail</c>. The error message and error
    /// code of the <c>fail</c> result are passed to this function.
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
            MaybeResultType.Some => some(_value!),
            MaybeResultType.None => none(),
            _ => fail(_error!),
        };
    }

    /// <inheritdoc/>
    public bool Equals(MaybeResult<T> other) =>
        EqualityComparer<T?>.Default.Equals(_value, other._value)
        && EqualityComparer<Error?>.Default.Equals(_error, other._error);

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is MaybeResult<T> result && Equals(result);

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        int hashCode = 1697802621;
        hashCode = (hashCode * -1521134295) + EqualityComparer<Type>.Default.GetHashCode(GetType());
        hashCode = (hashCode * -1521134295) + (_value is null ? 0 : EqualityComparer<T>.Default.GetHashCode(_value));
        hashCode = (hashCode * -1521134295) + (_error is null ? 0 : EqualityComparer<Error>.Default.GetHashCode(_error));
        return hashCode;
    }
}
