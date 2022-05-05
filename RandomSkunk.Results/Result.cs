namespace RandomSkunk.Results;

/// <summary>
/// The result of an operation that does not have a return value.
/// </summary>
public struct Result : IEquatable<Result>
{
    private readonly Error? _error;
    private readonly ResultType _type;

    private Result(bool success, Error? error = null)
    {
        if (success)
        {
            _error = null;
            _type = ResultType.Success;
        }
        else
        {
            _error = error ?? new Error();
            _type = ResultType.Fail;
        }
    }

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
    /// Creates a <c>success</c> result for an operation without a return value.
    /// </summary>
    /// <returns>A <c>success</c> result.</returns>
    public static Result Success() => new(success: true);

    /// <summary>
    /// Creates a <c>fail</c> result for an operation without a return value.
    /// </summary>
    /// <param name="error">The optional error that describes the failure.</param>
    /// <returns>A <c>fail</c> result.</returns>
    public static Result Fail(Error? error = null) => new(success: false, error);

    /// <summary>
    /// Creates a <c>fail</c> result for an operation with a return value.
    /// </summary>
    /// <param name="exception">The exception that caused the failure.</param>
    /// <param name="errorMessage">The optional error message.</param>
    /// <param name="errorCode">The optional error code.</param>
    /// <param name="identifier">The optional identifier of the error.</param>
    /// <returns>A <c>fail</c> result.</returns>
    public static Result Fail(
        Exception exception,
        string? errorMessage = null,
        int? errorCode = null,
        string? identifier = null) =>
        Fail(Error.FromException(exception, errorMessage, errorCode, identifier));

    /// <summary>
    /// Creates a <c>fail</c> result for an operation without a return value.
    /// </summary>
    /// <param name="errorMessage">The error message.</param>
    /// <param name="stackTrace">The optional stack trace.</param>
    /// <param name="errorCode">The optional error code.</param>
    /// <param name="identifier">The optional identifier of the error.</param>
    /// <returns>A <c>fail</c> result.</returns>
    public static Result Fail(
        string errorMessage,
        string? stackTrace = null,
        int? errorCode = null,
        string? identifier = null) =>
        Fail(new Error(errorMessage, stackTrace, errorCode, identifier));

    /// <summary>
    /// Creates a <c>success</c> result for an operation <em>with</em> a return value.
    /// </summary>
    /// <typeparam name="T">The return type of the operation.</typeparam>
    /// <param name="value">The value of the <c>success</c> result.</param>
    /// <returns>A <c>success</c> result.</returns>
    public static Result<T> Success<T>([DisallowNull] T value) => Result<T>.Success(value);

    /// <summary>
    /// Creates a <c>fail</c> result for an operation <em>with</em> a return value.
    /// </summary>
    /// <typeparam name="T">The return type of the operation.</typeparam>
    /// <param name="error">The optional error that describes the failure.</param>
    /// <returns>A <c>fail</c> result.</returns>
    public static Result<T> Fail<T>(Error? error = null) => Result<T>.Fail(error);

    /// <summary>
    /// Creates a <c>fail</c> result for an operation <em>with</em> a return value.
    /// </summary>
    /// <typeparam name="T">The return type of the operation.</typeparam>
    /// <param name="exception">The exception that caused the failure.</param>
    /// <param name="errorMessage">The optional error message.</param>
    /// <param name="errorCode">The optional error code.</param>
    /// <param name="identifier">The optional identifier of the error.</param>
    /// <returns>A <c>fail</c> result.</returns>
    public static Result<T> Fail<T>(
        Exception exception,
        string? errorMessage = null,
        int? errorCode = null,
        string? identifier = null) =>
        Result<T>.Fail(exception, errorMessage, errorCode, identifier);

    /// <summary>
    /// Creates a <c>fail</c> result for an operation <em>with</em> a return value.
    /// </summary>
    /// <typeparam name="T">The return type of the operation.</typeparam>
    /// <param name="errorMessage">The error message.</param>
    /// <param name="stackTrace">The optional stack trace.</param>
    /// <param name="errorCode">The optional error code.</param>
    /// <param name="identifier">The optional identifier of the error.</param>
    /// <returns>A <c>fail</c> result.</returns>
    public static Result<T> Fail<T>(
        string errorMessage,
        string? stackTrace = null,
        int? errorCode = null,
        string? identifier = null) =>
        Result<T>.Fail(errorMessage, stackTrace, errorCode, identifier);

    /// <summary>
    /// Evaluates either the <paramref name="success"/> or <paramref name="fail"/>
    /// function depending on whether the result type is <c>success</c> or <c>fail</c>.
    /// </summary>
    /// <typeparam name="TResult">The return type of the functions.</typeparam>
    /// <param name="success">
    /// The function to evaluate if the result type is <c>success</c>.
    /// </param>
    /// <param name="fail">
    /// The function to evaluate if the result type is <c>fail</c>. The error message and error
    /// code of the <c>fail</c> result are passed to this function.
    /// </param>
    /// <returns>The result of the matching function evaluation.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="success"/> is <see langword="null"/> or if <paramref name="fail"/> is
    /// <see langword="null"/>.
    /// </exception>
    public TResult Match<TResult>(
        Func<TResult> success,
        Func<Error, TResult> fail)
    {
        if (success is null) throw new ArgumentNullException(nameof(success));
        if (fail is null) throw new ArgumentNullException(nameof(fail));

        return IsSuccess
            ? success()
            : fail(_error!);
    }

    /// <summary>
    /// Evaluates either the <paramref name="success"/> or <paramref name="fail"/>
    /// function depending on whether the result type is <c>success</c> or <c>fail</c>.
    /// </summary>
    /// <param name="success">
    /// The function to evaluate if the result type is <c>success</c>.
    /// </param>
    /// <param name="fail">
    /// The function to evaluate if the result type is <c>fail</c>. The error message and error
    /// code of the <c>fail</c> result are passed to this function.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="success"/> is <see langword="null"/> or if <paramref name="fail"/> is
    /// <see langword="null"/>.
    /// </exception>
    public void Match(
        Action success,
        Action<Error> fail)
    {
        if (success is null) throw new ArgumentNullException(nameof(success));
        if (fail is null) throw new ArgumentNullException(nameof(fail));

        if (IsSuccess)
            success();
        else
            fail(_error!);
    }

    /// <summary>
    /// Evaluates either the <paramref name="success"/> or <paramref name="fail"/>
    /// function depending on whether the result type is <c>success</c> or <c>fail</c>.
    /// </summary>
    /// <typeparam name="TResult">The return type of the functions.</typeparam>
    /// <param name="success">
    /// The function to evaluate if the result type is <c>success</c>.
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
    /// If <paramref name="success"/> is <see langword="null"/> or if <paramref name="fail"/> is
    /// <see langword="null"/>.
    /// </exception>
    public Task<TResult> MatchAsync<TResult>(
        Func<Task<TResult>> success,
        Func<Error, Task<TResult>> fail)
    {
        if (success is null) throw new ArgumentNullException(nameof(success));
        if (fail is null) throw new ArgumentNullException(nameof(fail));

        return IsSuccess
            ? success()
            : fail(_error!);
    }

    /// <summary>
    /// Evaluates either the <paramref name="success"/> or <paramref name="fail"/>
    /// function depending on whether the result type is <c>success</c> or <c>fail</c>.
    /// </summary>
    /// <param name="success">
    /// The function to evaluate if the result type is <c>success</c>.
    /// </param>
    /// <param name="fail">
    /// The function to evaluate if the result type is <c>fail</c>. The error message and error
    /// code of the <c>fail</c> result are passed to this function.
    /// </param>
    /// <returns>A task representing the asynchronous match operation.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="success"/> is <see langword="null"/> or if <paramref name="fail"/> is
    /// <see langword="null"/>.
    /// </exception>
    public Task MatchAsync(
        Func<Task> success,
        Func<Error, Task> fail)
    {
        if (success is null) throw new ArgumentNullException(nameof(success));
        if (fail is null) throw new ArgumentNullException(nameof(fail));

        return IsSuccess
            ? success()
            : fail(_error!);
    }

    /// <inheritdoc/>
    public bool Equals(Result other) => EqualityComparer<Error?>.Default.Equals(_error, other._error);

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is Result result && Equals(result);

    /// <inheritdoc/>
    public override int GetHashCode() => 633584920 + (_error is null ? 0 : EqualityComparer<Error?>.Default.GetHashCode(_error));
}
