namespace RandomSkunk.Results;

/// <summary>
/// The result of an operation that has a return value where that return value is allowed to be
/// absent.
/// </summary>
/// <typeparam name="T">The type of the return value of the operation.</typeparam>
public abstract class MaybeResult<T> : ResultBase<T>
{
    private MaybeResult(CallSite callSite)
        : base(callSite)
    {
    }

    /// <summary>
    /// Gets a value indicating whether the result object is the result of a successful
    /// operation <em>with</em> a value.
    /// </summary>
    public virtual bool IsSome => false;

    /// <summary>
    /// Gets a value indicating whether the result object is the result of a successful
    /// operation <em>without</em> a value.
    /// </summary>
    public virtual bool IsNone => false;

    /// <summary>
    /// Gets the value of the success result, or throws an
    /// <see cref="InvalidOperationException"/> if <see cref="IsSome"/> is false.
    /// </summary>
    [NotNull]
    public override T Value => throw new InvalidOperationException($"{nameof(Value)} cannot be accessed if {nameof(IsSome)} is false.");

    /// <summary>
    /// Gets the type of the result: <see cref="MaybeResultType.Some"/>,
    /// <see cref="MaybeResultType.None"/>, or <see cref="MaybeResultType.Fail"/>.
    /// </summary>
    public abstract MaybeResultType Type { get; }

    /// <summary>
    /// Creates a <c>some</c> result for an operation with a return value.
    /// </summary>
    /// <param name="value">
    /// The value of the <c>some</c> result. Must not be <see langword="null"/>.
    /// </param>
    /// <param name="memberName">The compiler-provided name of the member where the call originated.</param>
    /// <param name="filePath">The compiler-provided path to the source file where the call originated.</param>
    /// <param name="lineNumber">The compiler-provided line number where the call originated.</param>
    /// <returns>A <c>some</c> result.</returns>
    public static MaybeResult<T> Some(
        [DisallowNull] T value,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int lineNumber = 0) =>
        new SomeResult(value, new CallSite(memberName, filePath, lineNumber));

    /// <summary>
    /// Creates a <c>none</c> result for an operation with a return value.
    /// </summary>
    /// <param name="memberName">The compiler-provided name of the member where the call originated.</param>
    /// <param name="filePath">The compiler-provided path to the source file where the call originated.</param>
    /// <param name="lineNumber">The compiler-provided line number where the call originated.</param>
    /// <returns>A <c>none</c> result.</returns>
    public static MaybeResult<T> None(
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int lineNumber = 0) =>
        new NoneResult(new CallSite(memberName, filePath, lineNumber));

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
    public static MaybeResult<T> Fail(
        string? errorMessage = null,
        int? errorCode = null,
        string? stackTrace = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int lineNumber = 0) =>
        new FailResult(new Error(errorMessage ?? DefaultErrorMessage, errorCode, stackTrace), new CallSite(memberName, filePath, lineNumber));

    /// <summary>
    /// Evaluates either the <paramref name="onSome"/>, <paramref name="onNone"/>, or
    /// <paramref name="onFail"/> function depending on whether the result type is <c>some</c>,
    /// <c>none</c>, or <c>fail</c>.
    /// </summary>
    /// <typeparam name="TResult">The return type of the functions.</typeparam>
    /// <param name="onSome">
    /// The function to evaluate if the result type is <c>some</c>. The value of the
    /// <c>some</c> result is passed to this function.
    /// </param>
    /// <param name="onNone">
    /// The function to evaluate if the result type is <c>none</c>.
    /// </param>
    /// <param name="onFail">
    /// The function to evaluate if the result type is <c>fail</c>. The error message and error
    /// code of the <c>fail</c> result are passed to this function.
    /// </param>
    /// <returns>The result of the matching function evaluation.</returns>
    public TResult Match<TResult>(
        Func<T, TResult> onSome,
        Func<TResult> onNone,
        Func<Error, TResult> onFail)
    {
        if (onSome == null) throw new ArgumentNullException(nameof(onSome));
        if (onNone == null) throw new ArgumentNullException(nameof(onNone));
        if (onFail == null) throw new ArgumentNullException(nameof(onFail));

        return MatchCore(onSome, onNone, onFail);
    }

    /// <summary>
    /// Evaluates either the <paramref name="onSome"/>, <paramref name="onNone"/>, or
    /// <paramref name="onFail"/> function depending on whether the result type is <c>some</c>,
    /// <c>none</c>, or <c>fail</c>.
    /// </summary>
    /// <param name="onSome">
    /// The function to evaluate if the result type is <c>some</c>. The value of the
    /// <c>some</c> result is passed to this function.
    /// </param>
    /// <param name="onNone">
    /// The function to evaluate if the result type is <c>none</c>.
    /// </param>
    /// <param name="onFail">
    /// The function to evaluate if the result type is <c>fail</c>. The error message and error
    /// code of the <c>fail</c> result are passed to this function.
    /// </param>
    public void Match(
        Action<T> onSome,
        Action onNone,
        Action<Error> onFail)
    {
        if (onSome == null) throw new ArgumentNullException(nameof(onSome));
        if (onNone == null) throw new ArgumentNullException(nameof(onNone));
        if (onFail == null) throw new ArgumentNullException(nameof(onFail));

        MatchCore(onSome, onNone, onFail);
    }

    /// <summary>
    /// Evaluates either the <paramref name="onSome"/>, <paramref name="onNone"/>, or
    /// <paramref name="onFail"/> function depending on whether the result type is <c>some</c>,
    /// <c>none</c>, or <c>fail</c>.
    /// </summary>
    /// <typeparam name="TResult">The return type of the functions.</typeparam>
    /// <param name="onSome">
    /// The function to evaluate if the result type is <c>some</c>. The value of the
    /// <c>some</c> result is passed to this function.
    /// </param>
    /// <param name="onNone">
    /// The function to evaluate if the result type is <c>none</c>.
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
        Func<T, Task<TResult>> onSome,
        Func<Task<TResult>> onNone,
        Func<Error, Task<TResult>> onFail)
    {
        if (onSome == null) throw new ArgumentNullException(nameof(onSome));
        if (onNone == null) throw new ArgumentNullException(nameof(onNone));
        if (onFail == null) throw new ArgumentNullException(nameof(onFail));

        return MatchAsyncCore(onSome, onNone, onFail);
    }

    /// <summary>
    /// Evaluates either the <paramref name="onSome"/>, <paramref name="onNone"/>, or
    /// <paramref name="onFail"/> function depending on whether the result type is <c>some</c>,
    /// <c>none</c>, or <c>fail</c>.
    /// </summary>
    /// <param name="onSome">
    /// The function to evaluate if the result type is <c>some</c>. The value of the
    /// <c>some</c> result is passed to this function.
    /// </param>
    /// <param name="onNone">
    /// The function to evaluate if the result type is <c>none</c>.
    /// </param>
    /// <param name="onFail">
    /// The function to evaluate if the result type is <c>fail</c>. The error message and error
    /// code of the <c>fail</c> result are passed to this function.
    /// </param>
    /// <returns>A task representing the asynchronous match operation.</returns>
    public Task MatchAsync(
        Func<T, Task> onSome,
        Func<Task> onNone,
        Func<Error, Task> onFail)
    {
        if (onSome == null) throw new ArgumentNullException(nameof(onSome));
        if (onNone == null) throw new ArgumentNullException(nameof(onNone));
        if (onFail == null) throw new ArgumentNullException(nameof(onFail));

        return MatchAsyncCore(onSome, onNone, onFail);
    }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable SA1600 // Elements should be documented
    protected abstract TResult MatchCore<TResult>(Func<T, TResult> onSome, Func<TResult> onNone, Func<Error, TResult> onFail);

    protected abstract void MatchCore(Action<T> onSome, Action onNone, Action<Error> onFail);

    protected abstract Task<TResult> MatchAsyncCore<TResult>(Func<T, Task<TResult>> onSome, Func<Task<TResult>> onNone, Func<Error, Task<TResult>> onFail);

    protected abstract Task MatchAsyncCore(Func<T, Task> onSome, Func<Task> onNone, Func<Error, Task> onFail);
#pragma warning restore SA1600 // Elements should be documented
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

    private sealed class SomeResult : MaybeResult<T>
    {
        public SomeResult(T value, CallSite callSite)
            : base(callSite) => Value = value ?? throw new ArgumentNullException(nameof(value));

        public override MaybeResultType Type => MaybeResultType.Some;

        public override bool IsSuccess => true;

        public override bool IsSome => true;

        [NotNull]
        public override T Value { get; }

        protected override TResult MatchCore<TResult>(Func<T, TResult> onSome, Func<TResult> onNone, Func<Error, TResult> onFail) => onSome(Value);

        protected override void MatchCore(Action<T> onSome, Action onNone, Action<Error> onFail) => onSome(Value);

        protected override Task<TResult> MatchAsyncCore<TResult>(Func<T, Task<TResult>> onSome, Func<Task<TResult>> onNone, Func<Error, Task<TResult>> onFail) => onSome(Value);

        protected override Task MatchAsyncCore(Func<T, Task> onSome, Func<Task> onNone, Func<Error, Task> onFail) => onSome(Value);
    }

    private sealed class NoneResult : MaybeResult<T>
    {
        public NoneResult(CallSite callSite)
            : base(callSite)
        {
        }

        public override MaybeResultType Type => MaybeResultType.None;

        public override bool IsSuccess => true;

        public override bool IsNone => true;

        protected override TResult MatchCore<TResult>(Func<T, TResult> onSome, Func<TResult> onNone, Func<Error, TResult> onFail) => onNone();

        protected override void MatchCore(Action<T> onSome, Action onNone, Action<Error> onFail) => onNone();

        protected override Task<TResult> MatchAsyncCore<TResult>(Func<T, Task<TResult>> onSome, Func<Task<TResult>> onNone, Func<Error, Task<TResult>> onFail) => onNone();

        protected override Task MatchAsyncCore(Func<T, Task> onSome, Func<Task> onNone, Func<Error, Task> onFail) => onNone();
    }

    private sealed class FailResult : MaybeResult<T>
    {
        public FailResult(Error error, CallSite callSite)
            : base(callSite) => Error = error;

        public override MaybeResultType Type => MaybeResultType.Fail;

        public override bool IsFail => true;

        public override Error Error { get; }

        protected override TResult MatchCore<TResult>(Func<T, TResult> onSome, Func<TResult> onNone, Func<Error, TResult> onFail) => onFail(Error);

        protected override void MatchCore(Action<T> onSome, Action onNone, Action<Error> onFail) => onFail(Error);

        protected override Task<TResult> MatchAsyncCore<TResult>(Func<T, Task<TResult>> onSome, Func<Task<TResult>> onNone, Func<Error, Task<TResult>> onFail) => onFail(Error);

        protected override Task MatchAsyncCore(Func<T, Task> onSome, Func<Task> onNone, Func<Error, Task> onFail) => onFail(Error);
    }
}
