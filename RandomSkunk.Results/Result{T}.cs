namespace RandomSkunk.Results;

/// <summary>
/// The result of an operation that has a return value.
/// </summary>
/// <typeparam name="T">The return type of the operation.</typeparam>
public struct Result<T> : IEquatable<Result<T>>
{
    private readonly T? _value;
    private readonly Error? _error;
    private readonly ResultType _type;

    private Result([DisallowNull] T value)
    {
        _value = value ?? throw new ArgumentNullException(nameof(value));
        _error = null;
        _type = ResultType.Success;
    }

    private Result(Error? error)
    {
        _value = default;
        _error = error ?? new Error();
        _type = ResultType.Fail;
    }

    /// <summary>
    /// Gets the return value of the successful operation, or throws an
    /// <see cref="InvalidOperationException"/> if this is not a <c>success</c> result.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// If this result is not a <c>success</c> result.
    /// </exception>
    [NotNull]
    public T Value =>
        IsSuccess
            ? _value!
            : throw Exceptions.CannotAccessValueUnlessSuccess;

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
    /// Gets the type of the result: <see cref="ResultType.Success"/> or
    /// <see cref="ResultType.Fail"/>.
    /// </summary>
    public ResultType Type => _type;

    /// <summary>
    /// Gets a value indicating whether this is a <c>success</c> result.
    /// </summary>
    /// <returns>
    /// <see langword="true"/> if this is a <c>success</c> result; otherwise,
    /// <see langword="false"/>.
    /// </returns>
    public bool IsSuccess => _type == ResultType.Success;

    /// <summary>
    /// Gets a value indicating whether this is a <c>fail</c> result.
    /// </summary>
    /// <returns>
    /// <see langword="true"/> if this is a <c>fail</c> result; otherwise,
    /// <see langword="false"/>.
    /// </returns>
    public bool IsFail => _type == ResultType.Fail;

    /// <summary>
    /// Converts the specified value to a <c>success</c> result.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="value"/> is <see langword="null"/>.
    /// </exception>
    public static implicit operator Result<T>([DisallowNull] T value) => Success(value);

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

    /// <summary>
    /// Creates a <c>success</c> result for an operation with a return value.
    /// </summary>
    /// <param name="value">
    /// The value of the <c>success</c> result. Must not be <see langword="null"/>.
    /// </param>
    /// <returns>A <c>success</c> result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="value"/> is <see langword="null"/>.
    /// </exception>
    public static Result<T> Success([DisallowNull] T value) => new(value);

    /// <summary>
    /// Creates a <c>fail</c> result for an operation with a return value.
    /// </summary>
    /// <param name="error">The optional error that describes the failure.</param>
    /// <returns>A <c>fail</c> result.</returns>
    public static Result<T> Fail(Error? error = null) => new(error);

    /// <summary>
    /// Creates a <c>fail</c> result for an operation with a return value.
    /// </summary>
    /// <param name="exception">The exception that caused the failure.</param>
    /// <param name="errorMessage">The optional error message.</param>
    /// <param name="errorCode">The optional error code.</param>
    /// <param name="identifier">The optional identifier of the error.</param>
    /// <returns>A <c>fail</c> result.</returns>
    public static Result<T> Fail(
        Exception exception,
        string? errorMessage = null,
        int? errorCode = null,
        string? identifier = null) =>
        Fail(Error.FromException(exception, errorMessage, errorCode, identifier));

    /// <summary>
    /// Creates a <c>fail</c> result for an operation with a return value.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    /// <param name="stackTrace">The optional stack trace.</param>
    /// <param name="errorCode">The optional error code.</param>
    /// <param name="identifier">The optional identifier of the error.</param>
    /// <returns>A <c>fail</c> result.</returns>
    public static Result<T> Fail(
        string errorMessage,
        string? stackTrace = null,
        int? errorCode = null,
        string? identifier = null) =>
        Fail(new Error(errorMessage, stackTrace, errorCode, identifier));

    /// <summary>
    /// Evaluates either the <paramref name="success"/> or <paramref name="fail"/>
    /// function depending on whether the result type is <c>success</c> or <c>fail</c>.
    /// </summary>
    /// <typeparam name="TResult">The return type of the functions.</typeparam>
    /// <param name="success">
    /// The function to evaluate if the result type is <c>success</c>. The value of the <c>success</c> result
    /// is passed to this function.
    /// </param>
    /// <param name="fail">
    /// The function to evaluate if the result type is <c>fail</c>. The error message and error code
    /// of the <c>fail</c> result are passed to this function.
    /// </param>
    /// <returns>The result of the matching function evaluation.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="success"/> is <see langword="null"/> or if <paramref name="fail"/> is
    /// <see langword="null"/>.
    /// </exception>
    public TResult Match<TResult>(
        Func<T, TResult> success,
        Func<Error, TResult> fail)
    {
        if (success is null) throw new ArgumentNullException(nameof(success));
        if (fail is null) throw new ArgumentNullException(nameof(fail));

        return IsSuccess
            ? success(_value!)
            : fail(_error!);
    }

    /// <summary>
    /// Evaluates either the <paramref name="success"/> or <paramref name="fail"/>
    /// function depending on whether the result type is <c>success</c> or <c>fail</c>.
    /// </summary>
    /// <param name="success">
    /// The function to evaluate if the result type is <c>success</c>. The value of the <c>success</c> result
    /// is passed to this function.
    /// </param>
    /// <param name="fail">
    /// The function to evaluate if the result type is <c>fail</c>. The error message and error code
    /// of the <c>fail</c> result are passed to this function.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="success"/> is <see langword="null"/> or if <paramref name="fail"/> is
    /// <see langword="null"/>.
    /// </exception>
    public void Match(
        Action<T> success,
        Action<Error> fail)
    {
        if (success is null) throw new ArgumentNullException(nameof(success));
        if (fail is null) throw new ArgumentNullException(nameof(fail));

        if (IsSuccess)
            success(_value!);
        else
            fail(_error!);
    }

    /// <summary>
    /// Evaluates either the <paramref name="success"/> or <paramref name="fail"/>
    /// function depending on whether the result type is <c>success</c> or <c>fail</c>.
    /// </summary>
    /// <typeparam name="TResult">The return type of the functions.</typeparam>
    /// <param name="success">
    /// The function to evaluate if the result type is <c>success</c>. The value of the <c>success</c> result
    /// is passed to this function.
    /// </param>
    /// <param name="fail">
    /// The function to evaluate if the result type is <c>fail</c>. The error message and error code
    /// of the <c>fail</c> result are passed to this function.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous match operation, which wraps the result of the
    /// matching function evaluation.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="success"/> is <see langword="null"/> or if <paramref name="fail"/> is
    /// <see langword="null"/>.
    /// </exception>
    public Task<TResult> MatchAsync<TResult>(
        Func<T, Task<TResult>> success,
        Func<Error, Task<TResult>> fail)
    {
        if (success is null) throw new ArgumentNullException(nameof(success));
        if (fail is null) throw new ArgumentNullException(nameof(fail));

        return IsSuccess
            ? success(_value!)
            : fail(_error!);
    }

    /// <summary>
    /// Evaluates either the <paramref name="success"/> or <paramref name="fail"/>
    /// function depending on whether the result type is <c>success</c> or <c>fail</c>.
    /// </summary>
    /// <param name="success">
    /// The function to evaluate if the result type is <c>success</c>. The value of the <c>success</c> result
    /// is passed to this function.
    /// </param>
    /// <param name="fail">
    /// The function to evaluate if the result type is <c>fail</c>. The error message and error code
    /// of the <c>fail</c> result are passed to this function.
    /// </param>
    /// <returns>A task representing the asynchronous match operation.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="success"/> is <see langword="null"/> or if <paramref name="fail"/> is
    /// <see langword="null"/>.
    /// </exception>
    public Task MatchAsync(
        Func<T, Task> success,
        Func<Error, Task> fail)
    {
        if (success is null) throw new ArgumentNullException(nameof(success));
        if (fail is null) throw new ArgumentNullException(nameof(fail));

        return IsSuccess
            ? success(_value!)
            : fail(_error!);
    }

    /// <inheritdoc/>
    public bool Equals(Result<T> other) =>
        EqualityComparer<T?>.Default.Equals(_value, other._value)
        && EqualityComparer<Error?>.Default.Equals(_error, other._error);

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is Result<T> result && Equals(result);

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
