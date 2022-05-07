namespace RandomSkunk.Results;

/// <summary>
/// Defines a result without a value.
/// </summary>
/// <remarks>
/// Use <see cref="Create"/> to create instances of this type.
/// </remarks>
public struct Result : IEquatable<Result>
{
    /// <summary>
    /// The factory object used to create instances of <see cref="Result"/>. This field is
    /// read-only.
    /// </summary>
    public static readonly IResultFactory Create = new Factory();

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
    /// Gets the type of the result: <see cref="ResultType.Success"/> or
    /// <see cref="ResultType.Fail"/>.
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
    /// Evaluates either the <paramref name="success"/> or <paramref name="fail"/>
    /// function depending on whether the result type is <c>Success</c> or <c>Fail</c>.
    /// </summary>
    /// <typeparam name="TReturn">The return type of the functions.</typeparam>
    /// <param name="success">
    /// The function to evaluate if the result type is <c>Success</c>.
    /// </param>
    /// <param name="fail">
    /// The function to evaluate if the result type is <c>Fail</c>. The error message and error
    /// code of the <c>Fail</c> result are passed to this function.
    /// </param>
    /// <returns>The result of the matching function evaluation.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="success"/> is <see langword="null"/> or if <paramref name="fail"/> is
    /// <see langword="null"/>.
    /// </exception>
    public TReturn Match<TReturn>(
        Func<TReturn> success,
        Func<Error, TReturn> fail)
    {
        if (success is null) throw new ArgumentNullException(nameof(success));
        if (fail is null) throw new ArgumentNullException(nameof(fail));

        return IsSuccess
            ? success()
            : fail(_error!);
    }

    /// <summary>
    /// Evaluates either the <paramref name="success"/> or <paramref name="fail"/>
    /// function depending on whether the result type is <c>Success</c> or <c>Fail</c>.
    /// </summary>
    /// <param name="success">
    /// The function to evaluate if the result type is <c>Success</c>.
    /// </param>
    /// <param name="fail">
    /// The function to evaluate if the result type is <c>Fail</c>. The error message and error
    /// code of the <c>Fail</c> result are passed to this function.
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
    /// function depending on whether the result type is <c>Success</c> or <c>Fail</c>.
    /// </summary>
    /// <typeparam name="TReturn">The return type of the functions.</typeparam>
    /// <param name="success">
    /// The function to evaluate if the result type is <c>Success</c>.
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
    /// If <paramref name="success"/> is <see langword="null"/> or if <paramref name="fail"/> is
    /// <see langword="null"/>.
    /// </exception>
    public Task<TReturn> MatchAsync<TReturn>(
        Func<Task<TReturn>> success,
        Func<Error, Task<TReturn>> fail)
    {
        if (success is null) throw new ArgumentNullException(nameof(success));
        if (fail is null) throw new ArgumentNullException(nameof(fail));

        return IsSuccess
            ? success()
            : fail(_error!);
    }

    /// <summary>
    /// Evaluates either the <paramref name="success"/> or <paramref name="fail"/>
    /// function depending on whether the result type is <c>Success</c> or <c>Fail</c>.
    /// </summary>
    /// <param name="success">
    /// The function to evaluate if the result type is <c>Success</c>.
    /// </param>
    /// <param name="fail">
    /// The function to evaluate if the result type is <c>Fail</c>. The error message and error
    /// code of the <c>Fail</c> result are passed to this function.
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
    public override int GetHashCode()
    {
        int hashCode = -2043725954;
        hashCode = (hashCode * -1521134295) + EqualityComparer<Type>.Default.GetHashCode(GetType());
        hashCode = (hashCode * -1521134295) + (_error is null ? 0 : EqualityComparer<Error?>.Default.GetHashCode(_error));
        return hashCode;
    }

    private sealed class Factory : IResultFactory
    {
        public Result Success() => new(success: true);

        public Result Fail(Error? error = null) => new(success: false, error);
    }
}
