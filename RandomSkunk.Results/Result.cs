namespace RandomSkunk.Results;

/// <summary>
/// The result of an operation that does not have a return value.
/// </summary>
public abstract class Result : ResultBase
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
    /// Creates a <c>success</c> result for an operation without a return value.
    /// </summary>
    /// <param name="memberName">The compiler-provided name of the member where the call originated.</param>
    /// <param name="filePath">The compiler-provided path to the source file where the call originated.</param>
    /// <param name="lineNumber">The compiler-provided line number where the call originated.</param>
    /// <returns>A <c>success</c> result.</returns>
    public static Result Success(
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int lineNumber = 0) =>
        new SuccessResult(new CallSite(memberName, filePath, lineNumber));

    /// <summary>
    /// Creates a <c>fail</c> result for an operation without a return value.
    /// </summary>
    /// <param name="error">The error message that describes the failure.</param>
    /// <param name="errorCode">The optional error code that describes the failure.</param>
    /// <param name="stackTrace">The optional stack trace that describes the failure.</param>
    /// <param name="memberName">The compiler-provided name of the member where the call originated.</param>
    /// <param name="filePath">The compiler-provided path to the source file where the call originated.</param>
    /// <param name="lineNumber">The compiler-provided line number where the call originated.</param>
    /// <returns>A <c>fail</c> result.</returns>
    public static Result Fail(
        string? error = null,
        int? errorCode = null,
        string? stackTrace = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int lineNumber = 0) =>
        new FailResult(new Error(error ?? DefaultError, errorCode, stackTrace), new CallSite(memberName, filePath, lineNumber));

    /// <summary>
    /// Creates a <c>success</c> result for an operation <em>with</em> a return value.
    /// </summary>
    /// <typeparam name="T">The type of the return value of the operation.</typeparam>
    /// <param name="value">The value of the <c>success</c> result.</param>
    /// <param name="memberName">The compiler-provided name of the member where the call originated.</param>
    /// <param name="filePath">The compiler-provided path to the source file where the call originated.</param>
    /// <param name="lineNumber">The compiler-provided line number where the call originated.</param>
    /// <returns>A <c>success</c> result.</returns>
    public static Result<T> Success<T>(
        [DisallowNull] T value,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int lineNumber = 0) =>
        Result<T>.Success(value, memberName, filePath, lineNumber);

    /// <summary>
    /// Creates a <c>fail</c> result for an operation <em>with</em> a return value.
    /// </summary>
    /// <typeparam name="T">The type of the return value of the operation.</typeparam>
    /// <param name="error">The error message that describes the failure.</param>
    /// <param name="errorCode">The optional error code that describes the failure.</param>
    /// <param name="stackTrace">The optional stack trace that describes the failure.</param>
    /// <param name="memberName">The compiler-provided name of the member where the call originated.</param>
    /// <param name="filePath">The compiler-provided path to the source file where the call originated.</param>
    /// <param name="lineNumber">The compiler-provided line number where the call originated.</param>
    /// <returns>A <c>fail</c> result.</returns>
    public static Result<T> Fail<T>(
        string? error = null,
        int? errorCode = null,
        string? stackTrace = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int lineNumber = 0) =>
        Result<T>.Fail(error, errorCode, stackTrace, memberName, filePath, lineNumber);

    /// <summary>
    /// Evaluates either the <paramref name="onSuccess"/> or <paramref name="onFail"/>
    /// function depending on whether the result type is <c>success</c> or <c>fail</c>.
    /// </summary>
    /// <typeparam name="TResult">The return type of the functions.</typeparam>
    /// <param name="onSuccess">
    /// The function to evaluate if the result type is <c>success</c>.
    /// </param>
    /// <param name="onFail">
    /// The function to evaluate if the result type is <c>fail</c>. The error message and error
    /// code of the <c>fail</c> result are passed to this function.
    /// </param>
    /// <returns>The result of the matching function evaluation.</returns>
    public TResult Match<TResult>(
        Func<TResult> onSuccess,
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
    /// The function to evaluate if the result type is <c>success</c>.
    /// </param>
    /// <param name="onFail">
    /// The function to evaluate if the result type is <c>fail</c>. The error message and error
    /// code of the <c>fail</c> result are passed to this function.
    /// </param>
    public void Match(
        Action onSuccess,
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
    /// The function to evaluate if the result type is <c>success</c>.
    /// </param>
    /// <param name="onFail">
    /// The function to evaluate if the result type is <c>fail</c>. The error message and error
    /// code of the <c>fail</c> result are passed to this function.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous match operation, which wraps the result of the
    /// matching function evaluation.
    /// </returns>
    public Task<TResult> MatchAsync<TResult>(
        Func<Task<TResult>> onSuccess,
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
    /// The function to evaluate if the result type is <c>success</c>.
    /// </param>
    /// <param name="onFail">
    /// The function to evaluate if the result type is <c>fail</c>. The error message and error
    /// code of the <c>fail</c> result are passed to this function.
    /// </param>
    /// <returns>A task representing the asynchronous match operation.</returns>
    public Task MatchAsync(
        Func<Task> onSuccess,
        Func<Error, Task> onFail)
    {
        if (onSuccess == null) throw new ArgumentNullException(nameof(onSuccess));
        if (onFail == null) throw new ArgumentNullException(nameof(onFail));

        return MatchAsyncCore(onSuccess, onFail);
    }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable SA1600 // Elements should be documented
    protected abstract TResult MatchCore<TResult>(Func<TResult> onSuccess, Func<Error, TResult> onFail);

    protected abstract void MatchCore(Action onSuccess, Action<Error> onFail);

    protected abstract Task<TResult> MatchAsyncCore<TResult>(Func<Task<TResult>> onSuccess, Func<Error, Task<TResult>> onFail);

    protected abstract Task MatchAsyncCore(Func<Task> onSuccess, Func<Error, Task> onFail);
#pragma warning restore SA1600 // Elements should be documented
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

    private sealed class SuccessResult : Result
    {
        public SuccessResult(CallSite callSite)
            : base(callSite)
        {
        }

        public override ResultType Type => ResultType.Success;

        public override bool IsSuccess => true;

        protected override TResult MatchCore<TResult>(Func<TResult> onSuccess, Func<Error, TResult> onFail) => onSuccess();

        protected override void MatchCore(Action onSuccess, Action<Error> onFail) => onSuccess();

        protected override Task<TResult> MatchAsyncCore<TResult>(Func<Task<TResult>> onSuccess, Func<Error, Task<TResult>> onFail) => onSuccess();

        protected override Task MatchAsyncCore(Func<Task> onSuccess, Func<Error, Task> onFail) => onSuccess();
    }

    private sealed class FailResult : Result
    {
        public FailResult(Error error, CallSite callSite)
            : base(callSite)
        {
            Error = error;
        }

        public override ResultType Type => ResultType.Fail;

        public override bool IsFail => true;

        public override Error Error { get; }

        protected override TResult MatchCore<TResult>(Func<TResult> onSuccess, Func<Error, TResult> onFail) => onFail(Error);

        protected override void MatchCore(Action onSuccess, Action<Error> onFail) => onFail(Error);

        protected override Task<TResult> MatchAsyncCore<TResult>(Func<Task<TResult>> onSuccess, Func<Error, Task<TResult>> onFail) => onFail(Error);

        protected override Task MatchAsyncCore(Func<Task> onSuccess, Func<Error, Task> onFail) => onFail(Error);
    }
}
