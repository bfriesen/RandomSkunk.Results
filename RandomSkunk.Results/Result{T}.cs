namespace RandomSkunk.Results;

/// <summary>
/// The result of an operation that has a return value.
/// </summary>
/// <typeparam name="T">The return type of the operation.</typeparam>
public abstract class Result<T> : IEquatable<Result<T>>
{
    private Result()
    {
    }

    /// <summary>
    /// Gets the return value of the successful operation, or throws an
    /// <see cref="InvalidOperationException"/> if this is not a <c>success</c> result.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// If this result is not a <c>success</c> result.
    /// </exception>
    [NotNull]
    public virtual T Value => throw Exceptions.CannotAccessValueUnlessSuccess;

    /// <summary>
    /// Gets the error from the failed operation, or throws an
    /// <see cref="InvalidOperationException"/> if this is not a <c>fail</c> result.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// If this result is not a <c>fail</c> result.
    /// </exception>
    public virtual Error Error => throw Exceptions.CannotAccessErrorUnlessFail;

    /// <summary>
    /// Gets the type of the result: <see cref="ResultType.Success"/> or
    /// <see cref="ResultType.Fail"/>.
    /// </summary>
    public abstract ResultType Type { get; }

    /// <summary>
    /// Gets a value indicating whether this is a <c>success</c> result.
    /// </summary>
    /// <returns>
    /// <see langword="true"/> if this is a <c>success</c> result; otherwise,
    /// <see langword="false"/>.
    /// </returns>
    public bool IsSuccess => Type == ResultType.Success;

    /// <summary>
    /// Gets a value indicating whether this is a <c>fail</c> result.
    /// </summary>
    /// <returns>
    /// <see langword="true"/> if this is a <c>fail</c> result; otherwise,
    /// <see langword="false"/>.
    /// </returns>
    public bool IsFail => Type == ResultType.Fail;

    /// <summary>
    /// Converts the specified value to a <c>success</c> result.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="value"/> is <see langword="null"/>.
    /// </exception>
    public static implicit operator Result<T>([DisallowNull] T value) => Success(value);

    /// <summary>
    /// Creates a <c>success</c> result for an operation with a return value.
    /// </summary>
    /// <param name="value">The value of the <c>success</c> result.</param>
    /// <returns>A <c>success</c> result.</returns>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="value"/> is <see langword="null"/>.
    /// </exception>
    public static Result<T> Success([DisallowNull] T value) => new SuccessResult(value);

    /// <summary>
    /// Creates a <c>fail</c> result for an operation with a return value.
    /// </summary>
    /// <param name="error">The optional error that describes the failure.</param>
    /// <returns>A <c>fail</c> result.</returns>
    public static Result<T> Fail(Error? error = null) => new FailResult(error ?? new Error());

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
        if (success == null) throw new ArgumentNullException(nameof(success));
        if (fail == null) throw new ArgumentNullException(nameof(fail));

        return MatchCore(success, fail);
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
        if (success == null) throw new ArgumentNullException(nameof(success));
        if (fail == null) throw new ArgumentNullException(nameof(fail));

        MatchCore(success, fail);
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
        if (success == null) throw new ArgumentNullException(nameof(success));
        if (fail == null) throw new ArgumentNullException(nameof(fail));

        return MatchAsyncCore(success, fail);
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
        if (success == null) throw new ArgumentNullException(nameof(success));
        if (fail == null) throw new ArgumentNullException(nameof(fail));

        return MatchAsyncCore(success, fail);
    }

    /// <inheritdoc/>
    public abstract bool Equals(Result<T>? other);

    /// <inheritdoc/>
    public override abstract bool Equals(object? other);

    /// <inheritdoc/>
    public override abstract int GetHashCode();

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable SA1600 // Elements should be documented
    protected abstract TResult MatchCore<TResult>(Func<T, TResult> success, Func<Error, TResult> fail);

    protected abstract void MatchCore(Action<T> success, Action<Error> fail);

    protected abstract Task<TResult> MatchAsyncCore<TResult>(Func<T, Task<TResult>> success, Func<Error, Task<TResult>> fail);

    protected abstract Task MatchAsyncCore(Func<T, Task> success, Func<Error, Task> fail);
#pragma warning restore SA1600 // Elements should be documented
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

    private sealed class SuccessResult : Result<T>
    {
        public SuccessResult(T value) => Value = value ?? throw new ArgumentNullException(nameof(value));

        public override ResultType Type => ResultType.Success;

        [NotNull]
        public override T Value { get; }

        public override bool Equals(Result<T>? other) =>
            other != null
                && other.IsSuccess
                && EqualityComparer<T>.Default.Equals(Value, other.Value);

        public override bool Equals(object? obj) =>
            obj is Result<T> other && Equals(other);

        public override int GetHashCode()
        {
            int hashCode = 1265339359;
            hashCode = (hashCode * -1521134295) + GetType().GetHashCode();
            hashCode = (hashCode * -1521134295) + EqualityComparer<T>.Default.GetHashCode(Value);
            return hashCode;
        }

        protected override TResult MatchCore<TResult>(Func<T, TResult> success, Func<Error, TResult> fail) => success(Value);

        protected override void MatchCore(Action<T> success, Action<Error> fail) => success(Value);

        protected override Task<TResult> MatchAsyncCore<TResult>(Func<T, Task<TResult>> success, Func<Error, Task<TResult>> fail) => success(Value);

        protected override Task MatchAsyncCore(Func<T, Task> success, Func<Error, Task> fail) => success(Value);
    }

    private sealed class FailResult : Result<T>
    {
        public FailResult(Error error) => Error = error;

        public override ResultType Type => ResultType.Fail;

        public override Error Error { get; }

        public override bool Equals(Result<T>? other) =>
            other != null
                && other.IsFail
                && Error.Equals(other.Error);

        public override bool Equals(object? obj) =>
            obj is Result<T> other && Equals(other);

        public override int GetHashCode()
        {
            int hashCode = 1840328550;
            hashCode = (hashCode * -1521134295) + GetType().GetHashCode();
            hashCode = (hashCode * -1521134295) + EqualityComparer<Error>.Default.GetHashCode(Error);
            return hashCode;
        }

        protected override TResult MatchCore<TResult>(Func<T, TResult> success, Func<Error, TResult> fail) => fail(Error);

        protected override void MatchCore(Action<T> success, Action<Error> fail) => fail(Error);

        protected override Task<TResult> MatchAsyncCore<TResult>(Func<T, Task<TResult>> success, Func<Error, Task<TResult>> fail) => fail(Error);

        protected override Task MatchAsyncCore(Func<T, Task> success, Func<Error, Task> fail) => fail(Error);
    }
}