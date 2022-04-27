namespace RandomSkunk.Results;

/// <summary>
/// The result of an operation that has a return value.
/// </summary>
/// <typeparam name="T">The type of the return value of the operation.</typeparam>
public abstract class Result<T> : ResultBase<T>
{
    private Result(CallSite callSite)
        : base(callSite)
    {
    }

    /// <summary>
    /// Gets the type of the result: <see cref="ResultType.Success"/> or
    /// <see cref="ResultType.Fail"/>.
    /// </summary>
    public abstract ResultType Type { get; }

    /// <summary>
    /// Creates a <c>success</c> result for an operation with a return value.
    /// </summary>
    /// <param name="value">The value of the <c>success</c> result.</param>
    /// <param name="memberName">The compiler-provided name of the member where the call originated.</param>
    /// <param name="filePath">The compiler-provided path to the source file where the call originated.</param>
    /// <param name="lineNumber">The compiler-provided line number where the call originated.</param>
    /// <returns>A <c>success</c> result.</returns>
    public static Result<T> Success(
        [DisallowNull] T value,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int lineNumber = 0) =>
        new SuccessResult(value, new CallSite(memberName, filePath, lineNumber));

    /// <summary>
    /// Creates a <c>fail</c> result for an operation with a return value.
    /// </summary>
    /// <param name="errorMessage">The error message that describes the failure.</param>
    /// <param name="errorCode">The optional error code that describes the failure.</param>
    /// <param name="stackTrace">The optional stack trace that describes the failure.</param>
    /// <param name="memberName">The compiler-provided name of the member where the call originated.</param>
    /// <param name="filePath">The compiler-provided path to the source file where the call originated.</param>
    /// <param name="lineNumber">The compiler-provided line number where the call originated.</param>
    /// <returns>A <c>fail</c> result.</returns>
    public static Result<T> Fail(
        string? errorMessage = null,
        int? errorCode = null,
        string? stackTrace = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int lineNumber = 0) =>
        new FailResult(new Error(errorMessage ?? DefaultErrorMessage, errorCode, stackTrace), new CallSite(memberName, filePath, lineNumber));

    /// <summary>
    /// Evaluates either the <paramref name="onSuccess"/> or <paramref name="onFail"/>
    /// function depending on whether the result type is <c>success</c> or <c>fail</c>.
    /// </summary>
    /// <typeparam name="TResult">The return type of the functions.</typeparam>
    /// <param name="onSuccess">
    /// The function to evaluate if the result type is <c>success</c>. The value of the <c>success</c> result
    /// is passed to this function.
    /// </param>
    /// <param name="onFail">
    /// The function to evaluate if the result type is <c>fail</c>. The error message and error code
    /// of the <c>fail</c> result are passed to this function.
    /// </param>
    /// <returns>The result of the matching function evaluation.</returns>
    public TResult Match<TResult>(
        Func<T, TResult> onSuccess,
        Func<Error, TResult> onFail)
    {
        if (onSuccess == null) throw new ArgumentNullException(nameof(onSuccess));
        if (onFail == null) throw new ArgumentNullException(nameof(onFail));

        return MatchCore(onSuccess, onFail);
    }

    /// <summary>
    /// Evaluates either the <paramref name="onSuccess"/> or <paramref name="onFail"/>
    /// function depending on whether the result type is <c>success</c> or <c>fail</c>.
    /// </summary>
    /// <param name="onSuccess">
    /// The function to evaluate if the result type is <c>success</c>. The value of the <c>success</c> result
    /// is passed to this function.
    /// </param>
    /// <param name="onFail">
    /// The function to evaluate if the result type is <c>fail</c>. The error message and error code
    /// of the <c>fail</c> result are passed to this function.
    /// </param>
    public void Match(
        Action<T> onSuccess,
        Action<Error> onFail)
    {
        if (onSuccess == null) throw new ArgumentNullException(nameof(onSuccess));
        if (onFail == null) throw new ArgumentNullException(nameof(onFail));

        MatchCore(onSuccess, onFail);
    }

    /// <summary>
    /// Evaluates either the <paramref name="onSuccess"/> or <paramref name="onFail"/>
    /// function depending on whether the result type is <c>success</c> or <c>fail</c>.
    /// </summary>
    /// <typeparam name="TResult">The return type of the functions.</typeparam>
    /// <param name="onSuccess">
    /// The function to evaluate if the result type is <c>success</c>. The value of the <c>success</c> result
    /// is passed to this function.
    /// </param>
    /// <param name="onFail">
    /// The function to evaluate if the result type is <c>fail</c>. The error message and error code
    /// of the <c>fail</c> result are passed to this function.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous match operation, which wraps the result of the
    /// matching function evaluation.
    /// </returns>
    public Task<TResult> MatchAsync<TResult>(
        Func<T, Task<TResult>> onSuccess,
        Func<Error, Task<TResult>> onFail)
    {
        if (onSuccess == null) throw new ArgumentNullException(nameof(onSuccess));
        if (onFail == null) throw new ArgumentNullException(nameof(onFail));

        return MatchAsyncCore(onSuccess, onFail);
    }

    /// <summary>
    /// Evaluates either the <paramref name="onSuccess"/> or <paramref name="onFail"/>
    /// function depending on whether the result type is <c>success</c> or <c>fail</c>.
    /// </summary>
    /// <param name="onSuccess">
    /// The function to evaluate if the result type is <c>success</c>. The value of the <c>success</c> result
    /// is passed to this function.
    /// </param>
    /// <param name="onFail">
    /// The function to evaluate if the result type is <c>fail</c>. The error message and error code
    /// of the <c>fail</c> result are passed to this function.
    /// </param>
    /// <returns>A task representing the asynchronous match operation.</returns>
    public Task MatchAsync(
        Func<T, Task> onSuccess,
        Func<Error, Task> onFail)
    {
        if (onSuccess == null) throw new ArgumentNullException(nameof(onSuccess));
        if (onFail == null) throw new ArgumentNullException(nameof(onFail));

        return MatchAsyncCore(onSuccess, onFail);
    }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable SA1600 // Elements should be documented
    protected abstract TResult MatchCore<TResult>(Func<T, TResult> onSuccess, Func<Error, TResult> onFail);

    protected abstract void MatchCore(Action<T> onSuccess, Action<Error> onFail);

    protected abstract Task<TResult> MatchAsyncCore<TResult>(Func<T, Task<TResult>> onSuccess, Func<Error, Task<TResult>> onFail);

    protected abstract Task MatchAsyncCore(Func<T, Task> onSuccess, Func<Error, Task> onFail);
#pragma warning restore SA1600 // Elements should be documented
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

    private sealed class SuccessResult : Result<T>
    {
        public SuccessResult(T value, CallSite callSite)
            : base(callSite) => Value = value ?? throw new ArgumentNullException(nameof(value));

        public override ResultType Type => ResultType.Success;

        public override bool IsSuccess => true;

        [NotNull]
        public override T Value { get; }

        protected override TResult MatchCore<TResult>(Func<T, TResult> onSuccess, Func<Error, TResult> onFail) => onSuccess(Value);

        protected override void MatchCore(Action<T> onSuccess, Action<Error> onFail) => onSuccess(Value);

        protected override Task<TResult> MatchAsyncCore<TResult>(Func<T, Task<TResult>> onSuccess, Func<Error, Task<TResult>> onFail) => onSuccess(Value);

        protected override Task MatchAsyncCore(Func<T, Task> onSuccess, Func<Error, Task> onFail) => onSuccess(Value);
    }

    private sealed class FailResult : Result<T>
    {
        public FailResult(Error error, CallSite callSite)
            : base(callSite) => Error = error;

        public override ResultType Type => ResultType.Fail;

        public override bool IsFail => true;

        public override Error Error { get; }

        protected override TResult MatchCore<TResult>(Func<T, TResult> onSuccess, Func<Error, TResult> onFail) => onFail(Error);

        protected override void MatchCore(Action<T> onSuccess, Action<Error> onFail) => onFail(Error);

        protected override Task<TResult> MatchAsyncCore<TResult>(Func<T, Task<TResult>> onSuccess, Func<Error, Task<TResult>> onFail) => onFail(Error);

        protected override Task MatchAsyncCore(Func<T, Task> onSuccess, Func<Error, Task> onFail) => onFail(Error);
    }
}
