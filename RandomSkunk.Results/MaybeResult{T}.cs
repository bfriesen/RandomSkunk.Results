namespace RandomSkunk.Results;

/// <summary>
/// The result of an operation that has a return value where that return value is allowed to be
/// absent.
/// </summary>
/// <typeparam name="T">The type of the return value of the operation.</typeparam>
public abstract class MaybeResult<T> : ResultBase<T>, IEquatable<MaybeResult<T>>
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
    /// <exception cref="InvalidOperationException">
    /// If <see cref="IsSome"/> is not true.
    /// </exception>
    [NotNull]
    public override T Value => throw Exceptions.CannotAccessValueUnlessSome;

    /// <summary>
    /// Gets the type of the result: <see cref="MaybeResultType.Some"/>,
    /// <see cref="MaybeResultType.None"/>, or <see cref="MaybeResultType.Fail"/>.
    /// </summary>
    public abstract MaybeResultType Type { get; }

    /// <summary>
    /// Converts the specified value to a <c>success</c> result.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="value"/> is <see langword="null"/>.
    /// </exception>
    public static implicit operator MaybeResult<T>([DisallowNull] T value) => Some(value);

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
    /// <exception cref="ArgumentNullException">
    /// If <paramref name="value"/> is <see langword="null"/>.
    /// </exception>
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
    /// <param name="error">The optional error that describes the failure.</param>
    /// <param name="memberName">The compiler-provided name of the member where the call originated.</param>
    /// <param name="filePath">The compiler-provided path to the source file where the call originated.</param>
    /// <param name="lineNumber">The compiler-provided line number where the call originated.</param>
    /// <returns>A <c>fail</c> result.</returns>
    public static MaybeResult<T> Fail(
        Error? error = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int lineNumber = 0) =>
        new FailResult(error ?? new Error(DefaultErrorMessage), new CallSite(memberName, filePath, lineNumber));

    /// <summary>
    /// Creates a <c>fail</c> result for an operation with a return value.
    /// </summary>
    /// <param name="exception">The exception that caused the failure.</param>
    /// <param name="messagePrefix">An optional prefix for the exception message.</param>
    /// <param name="errorCode">The optional error code.</param>
    /// <param name="identifier">The optional identifier of the error.</param>
    /// <param name="memberName">The compiler-provided name of the member where the call originated.</param>
    /// <param name="filePath">The compiler-provided path to the source file where the call originated.</param>
    /// <param name="lineNumber">The compiler-provided line number where the call originated.</param>
    /// <returns>A <c>fail</c> result.</returns>
    public static MaybeResult<T> Fail(
        Exception exception,
        string? messagePrefix = null,
        int? errorCode = null,
        string? identifier = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int lineNumber = 0) =>
        Fail(Error.FromException(exception, messagePrefix, errorCode, identifier), memberName, filePath, lineNumber);

    /// <summary>
    /// Creates a <c>fail</c> result for an operation with a return value.
    /// </summary>
    /// <param name="errorMessage">The error message that describes the failure.</param>
    /// <param name="stackTrace">The optional stack trace that describes the failure.</param>
    /// <param name="errorCode">The optional error code that describes the failure.</param>
    /// <param name="identifier">The optional identifier of the error.</param>
    /// <param name="memberName">The compiler-provided name of the member where the call originated.</param>
    /// <param name="filePath">The compiler-provided path to the source file where the call originated.</param>
    /// <param name="lineNumber">The compiler-provided line number where the call originated.</param>
    /// <returns>A <c>fail</c> result.</returns>
    public static MaybeResult<T> Fail(
        string errorMessage,
        string? stackTrace = null,
        int? errorCode = null,
        string? identifier = null,
        [CallerMemberName] string memberName = null!,
        [CallerFilePath] string filePath = null!,
        [CallerLineNumber] int lineNumber = 0) =>
        Fail(new Error(errorMessage, stackTrace, errorCode, identifier), memberName, filePath, lineNumber);

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
        if (some == null) throw new ArgumentNullException(nameof(some));
        if (none == null) throw new ArgumentNullException(nameof(none));
        if (fail == null) throw new ArgumentNullException(nameof(fail));

        return MatchCore(some, none, fail);
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
        if (some == null) throw new ArgumentNullException(nameof(some));
        if (none == null) throw new ArgumentNullException(nameof(none));
        if (fail == null) throw new ArgumentNullException(nameof(fail));

        MatchCore(some, none, fail);
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
        if (some == null) throw new ArgumentNullException(nameof(some));
        if (none == null) throw new ArgumentNullException(nameof(none));
        if (fail == null) throw new ArgumentNullException(nameof(fail));

        return MatchAsyncCore(some, none, fail);
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
        if (some == null) throw new ArgumentNullException(nameof(some));
        if (none == null) throw new ArgumentNullException(nameof(none));
        if (fail == null) throw new ArgumentNullException(nameof(fail));

        return MatchAsyncCore(some, none, fail);
    }

    /// <inheritdoc/>
    public abstract bool Equals(MaybeResult<T>? other);

    /// <inheritdoc/>
    public override abstract bool Equals(object? other);

    /// <inheritdoc/>
    public override abstract int GetHashCode();

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable SA1600 // Elements should be documented
    protected abstract TResult MatchCore<TResult>(Func<T, TResult> some, Func<TResult> none, Func<Error, TResult> fail);

    protected abstract void MatchCore(Action<T> some, Action none, Action<Error> fail);

    protected abstract Task<TResult> MatchAsyncCore<TResult>(Func<T, Task<TResult>> some, Func<Task<TResult>> none, Func<Error, Task<TResult>> fail);

    protected abstract Task MatchAsyncCore(Func<T, Task> some, Func<Task> none, Func<Error, Task> fail);
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

        public override bool Equals(MaybeResult<T>? other) =>
            other != null
                && other.IsSome
                && EqualityComparer<T>.Default.Equals(Value, other.Value);

        public override bool Equals(object? obj) =>
            obj is MaybeResult<T> other && Equals(other);

        public override int GetHashCode()
        {
            int hashCode = 1265339359;
            hashCode = (hashCode * -1521134295) + Type.GetHashCode();
            hashCode = (hashCode * -1521134295) + EqualityComparer<T>.Default.GetHashCode(Value);
            hashCode = (hashCode * -1521134295) + IsSome.GetHashCode();
            return hashCode;
        }

        protected override TResult MatchCore<TResult>(Func<T, TResult> some, Func<TResult> none, Func<Error, TResult> fail) => some(Value);

        protected override void MatchCore(Action<T> some, Action none, Action<Error> fail) => some(Value);

        protected override Task<TResult> MatchAsyncCore<TResult>(Func<T, Task<TResult>> some, Func<Task<TResult>> none, Func<Error, Task<TResult>> fail) => some(Value);

        protected override Task MatchAsyncCore(Func<T, Task> some, Func<Task> none, Func<Error, Task> fail) => some(Value);
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

        public override bool Equals(MaybeResult<T>? other) =>
            other != null && other.IsNone;

        public override bool Equals(object? obj) =>
            obj is MaybeResult<T> other && Equals(other);

        public override int GetHashCode()
        {
            int hashCode = -2070419312;
            hashCode = (hashCode * -1521134295) + Type.GetHashCode();
            hashCode = (hashCode * -1521134295) + IsNone.GetHashCode();
            return hashCode;
        }

        protected override TResult MatchCore<TResult>(Func<T, TResult> some, Func<TResult> none, Func<Error, TResult> fail) => none();

        protected override void MatchCore(Action<T> some, Action none, Action<Error> fail) => none();

        protected override Task<TResult> MatchAsyncCore<TResult>(Func<T, Task<TResult>> some, Func<Task<TResult>> none, Func<Error, Task<TResult>> fail) => none();

        protected override Task MatchAsyncCore(Func<T, Task> some, Func<Task> none, Func<Error, Task> fail) => none();
    }

    private sealed class FailResult : MaybeResult<T>
    {
        public FailResult(Error error, CallSite callSite)
            : base(callSite) => Error = error;

        public override MaybeResultType Type => MaybeResultType.Fail;

        public override bool IsFail => true;

        public override Error Error { get; }

        public override bool Equals(MaybeResult<T>? other) =>
            other != null
                && other.IsFail
                && Error.Equals(other.Error);

        public override bool Equals(object? obj) =>
            obj is MaybeResult<T> other && Equals(other);

        public override int GetHashCode()
        {
            int hashCode = 1840328550;
            hashCode = (hashCode * -1521134295) + Type.GetHashCode();
            hashCode = (hashCode * -1521134295) + EqualityComparer<Error>.Default.GetHashCode(Error);
            hashCode = (hashCode * -1521134295) + typeof(T).GetHashCode();
            hashCode = (hashCode * -1521134295) + IsFail.GetHashCode();
            return hashCode;
        }

        protected override TResult MatchCore<TResult>(Func<T, TResult> some, Func<TResult> none, Func<Error, TResult> fail) => fail(Error);

        protected override void MatchCore(Action<T> some, Action none, Action<Error> fail) => fail(Error);

        protected override Task<TResult> MatchAsyncCore<TResult>(Func<T, Task<TResult>> some, Func<Task<TResult>> none, Func<Error, Task<TResult>> fail) => fail(Error);

        protected override Task MatchAsyncCore(Func<T, Task> some, Func<Task> none, Func<Error, Task> fail) => fail(Error);
    }
}
